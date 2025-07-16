using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.TravelSpecifications
{
    public class TravelSpecPrams
    {
        const int maxPaginSize = 10;
        public int? CategorieyId { get; set; }
        public int? ComapnyId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = (value > maxPaginSize) ? maxPaginSize : value;
            }
        }

        private string? search;
        public string? Search { get { return search; } set { search = value?.ToLower(); } }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }      
    }
}
