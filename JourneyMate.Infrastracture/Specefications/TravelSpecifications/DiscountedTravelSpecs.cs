using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.TravelSpecifications
{
    public class DiscountedTravelSpecs:BaseSpecification<Travel>
    {
        public DiscountedTravelSpecs(TravelSpecPrams travelPrams) :base(x =>
            (x.SaleDiscount>0 )&&
            (!travelPrams.ComapnyId.HasValue || x.CompanyId == travelPrams.ComapnyId)
        )
        {

            Includes.Add(x => x.Company!);
            Includes.Add(x => x.Images);
            AddOrderByDes(x => x.SaleDiscount);

            ApplyPagination(((travelPrams.PageIndex - 1) * travelPrams.PageSize), travelPrams.PageSize);
        }
    }
}
