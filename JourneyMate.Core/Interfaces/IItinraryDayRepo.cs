using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Interfaces
{
    public interface IItinraryDayRepo: IGenericRepository<ItinraryDay>
    {
        Task<List<ItinraryDay>> GetItinerariesByTravelIdWithInclude(int TravelId);
    }
}
