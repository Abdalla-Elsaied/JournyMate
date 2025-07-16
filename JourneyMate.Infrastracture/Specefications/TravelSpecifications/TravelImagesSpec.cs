using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Specefications.TravelSpecifications
{
    public class TravelImagesSpec :BaseSpecification<Travel>
    {
        public TravelImagesSpec(int id):base(T=>T.Id==id)
        {
            Includes.Add(x => x.Images);
            Includes.Add(x => x.itineraries);
        }
    }
}
