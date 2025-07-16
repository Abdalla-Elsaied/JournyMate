using JourneyMate.Core.Models;


namespace JourneyMate.Infrastracture.Specefications.EventSpecifications
{
    public class EventSpecWithFilter : BaseSpecification<APIEventItem>
    {
        public EventSpecWithFilter(BaseSpecPrams specPrams)
            : base(x =>
                (
                 x.Title.Contains(specPrams.Search) ||
                 x.Description.Contains(specPrams.Search) ||
                 x.Location.Contains(specPrams.Search))
            )
        {
           
            ApplyPagination(
                specPrams.PageSize * (specPrams.PageIndex - 1),
                specPrams.PageSize
            );

            
        }
    }
}

