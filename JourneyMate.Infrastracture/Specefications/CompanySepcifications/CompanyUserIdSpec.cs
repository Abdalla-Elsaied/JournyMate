using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.CompanySepcifications
{
    public class CompanyUserIdSpec:BaseSpecification<Company>
    {
        public CompanyUserIdSpec(string UserId):base(C=>C.UserId==UserId)  //return specific company  by user id
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
