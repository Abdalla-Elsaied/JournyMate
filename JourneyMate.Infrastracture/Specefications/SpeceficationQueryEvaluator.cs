using JourneyMate.Core.Interfaces;

namespace JourneyMate.Infrastracture.Specefications
{
    public static class SpeceficationQueryEvaluator<TEntity> where TEntity : ModelBase
    {  
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
           ISpec<TEntity> specefication)
        {
            var query = inputQuery;

            if (specefication.Criteria is not null)
            {
                query = query.Where(specefication.Criteria);
            }
            if (specefication.OrderBy is not null)
            {
                query = query.OrderBy(specefication.OrderBy);
            }
            else if (specefication.OrderByDes is not null)
            {
                query = query.OrderByDescending(specefication.OrderByDes);
            }
            if (specefication.IsPaginationEnable)
            {
                query=query.Skip(specefication.Skip).Take(specefication.Take);
            }
            if (specefication.Includes is not null)
            {
                query = specefication.Includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if(specefication.ThenIncludeStrings is not null)
            {
                query = specefication.ThenIncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}
