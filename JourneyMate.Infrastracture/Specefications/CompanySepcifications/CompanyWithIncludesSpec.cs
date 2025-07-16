using JourneyMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.CompanySepcifications
{
    public class CompanyWithIncludesSpec:BaseSpecification<Company>
    {
        public CompanyWithIncludesSpec(CompanySpecParams companySpecParams) ///this ctor used to return all companies
            :base(c => 
            (string.IsNullOrEmpty(companySpecParams.Search) || c.CompanyName.ToLower().Contains(companySpecParams.Search))
            && (!companySpecParams.rate.HasValue||((c.Rating>=companySpecParams.rate && c.Rating<(companySpecParams.rate+1)))))
        {
            Includes.Add(C=>C.Travels);
            Includes.Add(C=>C.SocialMediaLinks);
            Includes.Add(C=>C.PaymentMethods);
            Includes.Add(C=>C.Ratings);
            if (!string.IsNullOrEmpty(companySpecParams.sort))
            {
                switch (companySpecParams.sort)
                {
                    case "rateAsc":
                        AddOrderBy(C=>C.Rating);
                        break;
                    case "rateDesc":
                        AddOrderByDes(C=>C.Rating);
                        break;
                    case "name":
                        AddOrderBy(C => C.CompanyName);
                        break;
                    default:
                        AddOrderByDes(c => c.Rating);
                        break;

                }
            }
            else
            {
                AddOrderByDes(C=>C.Rating);
            }
           ApplyPagination(((companySpecParams.PageIndex-1)*companySpecParams.PageSize), companySpecParams.PageSize);
        }
        public CompanyWithIncludesSpec(int id) : base(c => c.Id == id)   // return spasfic company  from id 
        {
            Includes.Add(C => C.Travels);
            ThenIncludeStrings.Add("Travels.Images");
            Includes.Add(C => C.WorkingHours);
            Includes.Add(C => C.SocialMediaLinks);
            Includes.Add(C => C.PaymentMethods);
            Includes.Add(C => C.Ratings);
        }
    }
}
