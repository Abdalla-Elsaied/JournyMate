using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.TravelSpecifications
{
    public class TravelSpecifications:BaseSpecification<Travel>
    {
        public TravelSpecifications(TravelSpecPrams travelPrams) : base(x =>

        (string.IsNullOrEmpty(travelPrams.Search) || x.Description!.ToLower().Contains(travelPrams.Search) 
        || x.Title!.ToLower().Contains(travelPrams.Search)) &&
        (!travelPrams.MinPrice.HasValue || x.Price >= travelPrams.MinPrice) &&
        (!travelPrams.MaxPrice.HasValue || x.Price <= travelPrams.MaxPrice)&&
        (!travelPrams.CategorieyId.HasValue || x.CategoryId == travelPrams.CategorieyId)&&
        (!travelPrams.ComapnyId.HasValue || x.CompanyId == travelPrams.ComapnyId)
        )

        {

            Includes.Add(x => x.Company);
            Includes.Add(x => x.category);
            Includes.Add(x => x.Images);

            if (!string.IsNullOrEmpty(travelPrams.Sort))
            {
                switch (travelPrams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(C => C.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDes(C => C.Price);
                        break;
                    default:
                        AddOrderBy(c => c.Title!);
                        break;

                }
            }
            ApplyPagination(((travelPrams.PageIndex - 1) * travelPrams.PageSize), travelPrams.PageSize);
        }


        public TravelSpecifications(int id):base(
            x=>x.Id == id)
        {
            Includes.Add(x => x.Company!);
            Includes.Add(x => x.Images);
            Includes.Add(x => x.itineraries);

        }
    }
}
