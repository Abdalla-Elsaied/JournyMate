using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications
{
    public class BaseSpecPrams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        private int pageIndex = 1;

        private string? search;

        public string? Search
        {
            get => search;
            set => search = value?.ToLower();
        }

        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value <= 0) pageSize = 5; // fallback default
                else pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }

        public int PageIndex
        {
            get => pageIndex;
            set
            {
                pageIndex = (value <= 0) ? 1 : value;
            }
        }
    }

}
