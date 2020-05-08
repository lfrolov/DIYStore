using System;

namespace DIYStoreWeb.Models
{

    /// <summary>
    /// Pagination model. Start Index = 1.
    /// </summary>
    public class PageViewModel
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int page, int pageSize) 
        {
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(count / (double) pageSize);
        }

        public bool HasPrevPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage 
        { 
            get {
                return CurrentPage < TotalPages;
            } 
        }
    }
}
