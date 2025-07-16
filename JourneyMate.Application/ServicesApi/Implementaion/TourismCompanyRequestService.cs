using JourneyMate.Application.Helper;
using JourneyMate.Application.ServicesApi.Interfaces;
namespace JourneyMate.Application.ServicesApi.Implementaion;

public class TourismCompanyRequestService : ITourismCompanyRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TourismCompanyRequestService> _logger;
    private readonly DocumentSetting _documentSetting;

    public TourismCompanyRequestService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TourismCompanyRequestService> logger , DocumentSetting documentSetting)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _documentSetting = documentSetting;
    }

    public async Task<bool> CreateTourismCompanyRequestAsync(TourismCompanyRequestDto tourismCompanyRequestDto)
    {
        try
        {
            if (tourismCompanyRequestDto == null)
            {
                _logger.LogWarning("Invalid data received for travel creation.");
                return await Task.FromResult(false);
            }
           

            var tourismCompanyRequest = _mapper.Map<TourismCompanyRequest>(tourismCompanyRequestDto);
            

            _unitOfWork.Repository<TourismCompanyRequest>().Add(tourismCompanyRequest);
             _unitOfWork.Complete();

            return await Task.FromResult(true);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating tourismCompanyRequest.");

            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public Task<bool> UpdateTourismCompanyRequestAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid data received for travel update.");
                return Task.FromResult(false);
            }
            var tourismCompanyRequest = _unitOfWork.Repository<TourismCompanyRequest>().GetById(id);
            if (tourismCompanyRequest == null)
            {
                _logger.LogWarning($"TourismCompanyRequest not found for ID: {id}");
                return Task.FromResult(false);
            }
            tourismCompanyRequest.Status = Core.Models.Enum.Status.Approved;

            _unitOfWork.Repository<TourismCompanyRequest>().Update(tourismCompanyRequest);
            _unitOfWork.Complete();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tourismCompanyRequest.");
            Console.WriteLine(ex.Message);
            return Task.FromResult(false);
        }
    }
    public Task<bool> DeleteTourismCompanyRequestAsync(int id)
    {
        try
        {
            if ( id <= 0)
            {
                _logger.LogWarning("Invalid data received for Delete tourismCompanyRequest.");
                return Task.FromResult(false);
            }
            var tourismCompanyRequest = _unitOfWork.Repository<TourismCompanyRequest>().GetById(id);
            if (tourismCompanyRequest == null)
            {
                _logger.LogWarning($"TourismCompanyRequest not found for ID: {id}");
                return Task.FromResult(false);
            }
            _unitOfWork.Repository<TourismCompanyRequest>().Delete(tourismCompanyRequest);
            _unitOfWork.Complete();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while Deleting tourismCompanyRequest.");
            Console.WriteLine(ex.Message);
            return Task.FromResult(false);
        }
    }

    public Task<List<TourismCompanyRequestDto>> GetAllTourismCompanyRequestAsync()
    {
        var tourismCompanyRequests = _unitOfWork.Repository<TourismCompanyRequest>().GetAll().OrderBy(e=>e.CreatedAt).ToList();
      
        List<TourismCompanyRequestDto>? tourismCompanyRequestDtos = _mapper.Map<List<TourismCompanyRequestDto>>(tourismCompanyRequests) ?? new();
       
        return Task.FromResult(tourismCompanyRequestDtos ) ;
    }
}
