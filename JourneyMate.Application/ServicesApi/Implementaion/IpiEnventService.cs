using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications;
using JourneyMate.Infrastracture.Specefications.EventSpecifications;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class IpiEnventService : IIpiEnventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IpiEnventService> _logger;

        public IpiEnventService(IUnitOfWork unitOfWork ,ILogger<IpiEnventService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public async Task<IEnumerable<APIEventItem>?> GetIpiEvenstAsync()
        {
            try
            {
                IEnumerable<APIEventItem>? apiEvents = await _unitOfWork.Repository<APIEventItem>().GetAllAsync();
                return apiEvents;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "Error occurred while fetching events.");

                return null;

            }
        }

          public async Task<IEnumerable<APIEventItem>?> GetIpiEvenstWithSpecAsync(BaseSpecPrams specPrams)
        {
            try
            {
                var spec = new EventSpecWithFilter(specPrams);
                IEnumerable<APIEventItem>? apiEvents = await _unitOfWork.Repository<APIEventItem>().GetAllWithSpecificaiton(spec);
                return apiEvents;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "Error occurred while fetching events.");

                return null;

            }
        }

        public async Task<APIEventItem?> GetIpiEventByIdAsync(int id)
        {
            try
            {
                APIEventItem? apiEvent = await _unitOfWork.Repository<APIEventItem>().GetByIdAsync(id);
                return apiEvent;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "Error occurred while fetching event.");

                return null;

            }

        }
    }
}
