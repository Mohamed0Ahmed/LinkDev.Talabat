using LinkDev.Talabat.Dashboard.Models;

namespace Route.Talabat.Dashboard.Models
{
    namespace LinkDev.Talabat.Dashboard.Models
    
    {
        public class PaginatedProductViewModel
        {
            public IEnumerable<ProductViewModel> Products { get; set; } = null!;
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public int TotalCount { get; set; }

            public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

            public bool HasPreviousPage => PageIndex > 1;
            public bool HasNextPage => PageIndex < TotalPages;
        }

    }
}
