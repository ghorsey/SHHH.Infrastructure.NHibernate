using System.Collections.Generic;

namespace SHHH.Infrastructure
{
    public class Collector<T> where T : class
    {
        public IList<T> Items { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPage { get; private set; }

        public Collector(List<T> items, int totalPages, int currentPage)
        {
            Items = items;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
    }
}
