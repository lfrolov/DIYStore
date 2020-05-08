using DIYStoreWeb.Helpers;
using DIYStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;

namespace DIYStoreWeb.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory, IOptions<PaginationOptions> options)
        {
            urlHelperFactory = helperFactory;
            Options = options;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }
        protected IOptions<PaginationOptions> Options { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            // набор ссылок будет представлять список ul
            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");
            TagBuilder navTagBuilder;
            
            TagBuilder firstItem = CreateTag(1, urlHelper, "Первая", PageModel.TotalPages == 0);
            tag.InnerHtml.AppendHtml(firstItem);

            //// создаем ссылку на предыдущую страницу, если она есть
            TagBuilder prevItem = CreateTag(PageModel.CurrentPage - 1, urlHelper, "Предыдущая", !PageModel.HasPrevPage);
            tag.InnerHtml.AppendHtml(prevItem);

            //// формируем три ссылки - на текущую, предыдущую и следующую
            //TagBuilder currentItem = CreateTag(PageModel.CurrentPage, urlHelper);
            //tag.InnerHtml.AppendHtml(currentItem);

            int lbound = 1, rbound = Math.Min(PageModel.TotalPages, Options.Value.DisplayPageLinksNumber);
            //Если кол-во элементов больше чем кол-во отображаемых страниц 
            if (PageModel.TotalPages > Options.Value.DisplayPageLinksNumber)
            {
                var halfBound = (int)Math.Round((rbound - 1) / 2.0);
                if (PageModel.CurrentPage - halfBound > 1)
                {
                    lbound = PageModel.CurrentPage - halfBound;
                    rbound = lbound + Options.Value.DisplayPageLinksNumber - 1;
                }
                if (PageModel.CurrentPage + halfBound > PageModel.TotalPages) 
                {
                    rbound = PageModel.TotalPages;
                    lbound = rbound - Options.Value.DisplayPageLinksNumber + 1;
                }
            }
            for (var i = lbound; i <= rbound; i++) 
            {
                navTagBuilder = CreateTag(i, urlHelper);
                tag.InnerHtml.AppendHtml(navTagBuilder);
            }
            
            //// создаем ссылку на следующую страницу, если она есть            
            TagBuilder nextItem = CreateTag(PageModel.CurrentPage + 1, urlHelper, "Следующая", !PageModel.HasNextPage);
            tag.InnerHtml.AppendHtml(nextItem);

            //Последняя страница
            TagBuilder lastItem = CreateTag(PageModel.TotalPages, urlHelper, "Последняя", !(PageModel.TotalPages > 1));
            tag.InnerHtml.AppendHtml(lastItem);


            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper, string linkText = null, bool disabled = false)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if (pageNumber == this.PageModel.CurrentPage)
            {
                item.AddCssClass("active");
            }
            else
            {
                link.Attributes["href"] = urlHelper.Action(PageAction, new { page = pageNumber });
            }
            if (disabled)
            {
                item.AddCssClass("disabled");
            }
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(linkText ?? pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
