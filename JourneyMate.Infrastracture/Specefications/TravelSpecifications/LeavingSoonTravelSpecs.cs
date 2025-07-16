using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.TravelSpecifications
{
    public class LeavingSoonTravelSpecs : BaseSpecification<Travel>
    {
        public LeavingSoonTravelSpecs(TravelSpecPrams travelPrams) : base(t =>
             (t.StartDate >= DateTime.UtcNow &&
                 t.StartDate <= DateTime.UtcNow.AddDays(2) &&
                 t.AvailableSeats > 0)
            &&(!travelPrams.ComapnyId.HasValue || t.CompanyId == travelPrams.ComapnyId)
            )
        {
            Includes.Add(x => x.Company!);
            Includes.Add(x => x.Images);
            AddOrderBy(x => x.StartDate);
            ApplyPagination(((travelPrams.PageIndex - 1) * travelPrams.PageSize), travelPrams.PageSize);
        }
    }   
    
    
}
