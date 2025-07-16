using JourneyMate.Core.Models.Company_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Servieces
{
    public class ItineraryService : IitineraryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ItineraryService> _logger;

        public ItineraryService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<ItineraryService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> CreateItineraryAsync(ItineraryDayVm itinerariesvm)
        {
            try
            {
                ItinraryDay itinerary = _mapper.Map<ItinraryDay>(itinerariesvm);
                await _unitOfWork.itinraryDayRepo.AddAsync(itinerary);
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating itinerary.");
                return false;
            }
        }

        public async Task<bool> UpdateItineraryAsync(ItineraryDayVm itineraryVm)
        {
            try
            {
                // Get the existing itinerary from database
                var existingItinerary = await _unitOfWork.itinraryDayRepo.GetByIdAsync(itineraryVm.Id);
                if (existingItinerary == null)
                {
                    _logger.LogWarning($"Itinerary not found for ID: {itineraryVm.Id}");
                    return false;
                }

                // Map the updated values to the existing entity
                _mapper.Map(itineraryVm, existingItinerary);

                // Update the entity
                _unitOfWork.itinraryDayRepo.Update(existingItinerary);
                var result = await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"Itinerary with ID {itineraryVm.Id} updated successfully.");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating itinerary {itineraryVm.Id}");
                return false;
            }
        }

        public async Task<List<ItineraryDayVm>> GetItinerariesByTravelIdAsync(int TravelId)
        {
            try
            {
                var ItinerariesModel = await _unitOfWork.itinraryDayRepo.GetItinerariesByTravelIdWithInclude(TravelId);
                List<ItineraryDayVm> itinerary = _mapper.Map<List<ItineraryDayVm>>(ItinerariesModel);
                return itinerary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting itineraries for travel ID: {TravelId}");
                return new List<ItineraryDayVm>();
            }
        }

        public async Task<ItineraryDayVm> GetItineraryByIdAsync(int itineraryId)
        {
            try
            {
                var itinerary = await _unitOfWork.itinraryDayRepo.GetByIdAsync(itineraryId);
                if (itinerary == null)
                {
                    _logger.LogWarning($"Itinerary not found for ID: {itineraryId}");
                    return null;
                }
                return _mapper.Map<ItineraryDayVm>(itinerary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting itinerary by ID: {itineraryId}");
                return null;
            }
        }

        public async Task<bool> DeleteItineraryAsync(int itineraryId)
        {
            try
            {
                var itinerary = await _unitOfWork.itinraryDayRepo.GetByIdAsync(itineraryId);
                if (itinerary == null)
                {
                    _logger.LogError($"Itinerary not found for ID: {itineraryId}");
                    return false;
                }
                _unitOfWork.itinraryDayRepo.Delete(itinerary);
                var result = await _unitOfWork.CompleteAsync();
                _logger.LogInformation($"Itinerary with ID {itineraryId} deleted successfully.");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting itinerary {itineraryId}");
                return false;
            }
        }
    }
}