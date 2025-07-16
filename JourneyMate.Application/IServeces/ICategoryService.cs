using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.IServeces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CateroryDto>> GetCaterories();
    }
}
