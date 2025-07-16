using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Common
{
    public class PagedResult<T> where T : class
    {
        public PagedResult(IEnumerable<T> items, int totalCount,int pageNumber , int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            TotalPages  = (int)Math.Ceiling(totalCount / (double)pageSize);
            FromItem = ((pageNumber - 1) * pageSize) + 1;
            ToItem = Math.Min(FromItem+ pageSize-1,totalCount);     

        }
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }  
        public int FromItem { get; set; }
        public int ToItem { get; set; }     
    }
}
