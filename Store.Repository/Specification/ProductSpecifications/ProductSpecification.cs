using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductSpecifications
{
    public class ProductSpecification
    {
        // Includes
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        //Ordering
        public string? Sort { get; set; }

        //Pagination
        public int PageIndex { get; set; } = 1;
        private const int MAXPAGESIZE = 50;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value;
        }

        //Search
        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower(); 
        }


    }
}
