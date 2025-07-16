using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation
{
    public class Category :ModelBase
    {
        public string CategoryName { get; set; }

        public List<Travel> Travels { get; set; } = new List<Travel>();


    }
}
