using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DIYStoreWeb.Models;
using DIYStoreWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DIYStoreWeb.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;

        
        public LoginController(IUserService userService):base()
        {
            _userService = userService;
        }
        // GET: /<controller>/
        [HttpGet]        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var user = await _userService.Authenticate(model.Login, model.Password);
                if (user != null) { 
                    await Authenticate(user.Login); 
                }
                return RedirectToAction("Index","Products");
            }
            return BadRequest(new { message = "Login or password is not valid" });
        }        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Products");
        }

        //ToDo: Move to authenti
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
