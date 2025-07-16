using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JourneyMate.Infrastracture.Specefications;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface IIpiEnventService
    {
        Task<IEnumerable<APIEventItem>?> GetIpiEvenstAsync();
        Task<APIEventItem?> GetIpiEventByIdAsync(int id);
        Task<IEnumerable<APIEventItem>?> GetIpiEvenstWithSpecAsync(BaseSpecPrams specPrams);
    }
}
