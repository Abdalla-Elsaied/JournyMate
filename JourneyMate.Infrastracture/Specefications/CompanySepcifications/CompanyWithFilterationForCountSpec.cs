using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.CompanySepcifications
{
    public class CompanyWithFilterationForCountSpec:BaseSpecification<Company>
    {
        public CompanyWithFilterationForCountSpec(CompanySpecParams companySpecParams)  //TOLower should be repleaced by normalized name in table 
            :base(c=>
            (string.IsNullOrEmpty(companySpecParams.Search)||c.CompanyName.ToLower().Contains(companySpecParams.Search))
            && (!companySpecParams.rate.HasValue|| ((c.Rating >= companySpecParams.rate && c.Rating < (companySpecParams.rate + 1)))))
        {
            
        }
    }
}
