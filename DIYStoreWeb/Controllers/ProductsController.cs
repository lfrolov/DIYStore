using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DIYDb;
using DIYDb.Models;
using DIYStoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using DIYStoreWeb.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DIYStoreWeb.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly DIYDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IOptions<PaginationOptions> _paginationOptions;
        private readonly int pageSize;

        public ProductsController(DIYDbContext context, IWebHostEnvironment appEnvironment, IOptions<PaginationOptions> paginationOptions)
        {
            _context = context;
            this._appEnvironment = appEnvironment;
            this._paginationOptions = paginationOptions;
            pageSize = _paginationOptions.Value.PageSize;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var productsQuery = _context.Products.Include(p => p.Unit);
            var productsCount = await productsQuery.CountAsync();
            var productItems = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var products = productItems.Select(ProductViewModelExtension.ToVm);


            var model = new ProductsViewModel
            {
                Pager = new PageViewModel(productsCount, page, pageSize),
                Products = products
            };

            return View(model);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product.ToVm());
        }

        // GET: Products/Create
        [HttpGet]
        public IActionResult Create(string imagesource)
        {
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "ShortName");
            ProductEditViewModel product = null;
            if (!string.IsNullOrEmpty(imagesource)) 
            {
                product = new ProductEditViewModel { ImageSource = imagesource };
            }
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,UnitId,Quantity,ImageSource")] ProductEditViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = editVM.Name,
                    Brand = editVM.Brand,
                    Description = editVM.Description,
                    UnitId = editVM.UnitId,
                    Quantity = editVM.Quantity
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitId"] = new SelectList(_context.Set<Unit>(), "UnitId", "ShortName", editVM.UnitId);
            return View(editVM);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            //ToDo: Need to add caching
            if (product == null)
            {
                return NotFound();
            }
            var units = await _context.Units.Select(u => new SelectListItem { Value = u.UnitId.ToString(), Text = u.ShortName, Selected = product.UnitId == u.UnitId }).ToListAsync();
            var productVm = new ProductEditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                UnitId = product.UnitId,
                Units = units,
                Quantity = product.Quantity,
                ImageSource = product.ImageSource
            };

            return View(productVm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Brand,Description,UnitId,Quantity,ImageSource")]ProductEditViewModel productVM)
        {
            if (id != productVM.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product
                    {
                        ProductId = productVM.ProductId,
                        Name = productVM.Name,
                        Brand = productVM.Brand,
                        Description = productVM.Description,
                        Quantity = productVM.Quantity,
                        UnitId = productVM.UnitId,
                        ImageSource = productVM.ImageSource
                    };
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productVM.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            productVM.Units = await _context.Units.Select(u => new SelectListItem { Value = u.UnitId.ToString(), Text = u.ShortName, Selected = productVM.UnitId == u.UnitId }).ToListAsync();
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageFile(int id, string redirectaction, IFormFile ImageFile)
        {
            string fileName = null;
            if (ImageFile != null)
            {
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                // путь к папке Files
                string path = $"/images/" + fileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
                //update image source
                if (id != 0) 
                {
                    var product = await _context.Products.FindAsync(id);
                    product.ImageSource = fileName;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(redirectaction, new { id = id, imagesource = fileName});
        }



        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product.ToVm());
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
