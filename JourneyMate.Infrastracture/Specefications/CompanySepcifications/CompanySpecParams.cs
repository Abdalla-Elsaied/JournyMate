using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.CompanySepcifications
{
    public class CompanySpecParams
    {
        //if the front didn't apply pagintion so will  put in page 5 object and the first page , if he ask with alot of object in the page the max will be 10
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        private string? search;
        public string? sort { get; set; }
        public int? rate { get; set; }
        public string? Search { get { return search; } set { search = value?.ToLower(); } }
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
        public int PageIndex { get; set; } = 1;

    }
}
