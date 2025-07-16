using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Interfaces
{
    public interface ISpec<T> where T : ModelBase
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; set; }
        List<string> ThenIncludeStrings { get; set; }
        Expression<Func<T, object>> OrderBy { get; set; }
        Expression<Func<T, object>> OrderByDes { get; set; }

        int Skip { get; set; }
        int Take { get; set; }
        bool IsPaginationEnable { get; set; }

        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }
}
