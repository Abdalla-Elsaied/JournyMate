using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.IServeces
{
    public interface IitineraryService
    {
        Task<List<ItineraryDayVm>> GetItinerariesByTravelIdAsync(int TravelId);
        Task<bool> CreateItineraryAsync(ItineraryDayVm itineraryVm);
        Task<bool> UpdateItineraryAsync(ItineraryDayVm itineraryVm);
        Task<bool> DeleteItineraryAsync(int itineraryId);
        Task<ItineraryDayVm> GetItineraryByIdAsync(int itineraryId);
    }
}