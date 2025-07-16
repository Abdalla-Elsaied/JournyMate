using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.CompanySepcifications
{
    public class CompanyTravelImagesSpecification : BaseSpecification<Company>
    {
        public CompanyTravelImagesSpecification(int companyId)
        {
            Criteria = c => c.Id == companyId;
            Includes.Add(c => c.Travels);
            ThenIncludeStrings.Add("Travels.Images");

        }
    }
}
