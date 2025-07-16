
using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using JourneyMate.Core.Interfaces;
using JourneyMate.Core.Models.Company_Aggregation;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class CompanyServices : ICompanyServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CompanyServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CompanyDetailsDto>?> GetAllCompaniessBySpec(CompanySpecParams companySpecParams)
        {
            var companySpec=new CompanyWithIncludesSpec(companySpecParams);
            var Companies = await _unitOfWork.Repository<Company>().GetAllWithSpecificaiton(companySpec);
            if (Companies == null)
                return null;
            var companyDtos = _mapper.Map<IEnumerable<CompanyDetailsDto>>(Companies);
            return companyDtos;
        }

        public async Task<CompanyDetailsDto?> GetCompanyById(int id)
        {
            var companySpec =  new CompanyWithIncludesSpec(id);
            var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(companySpec);
            if (company is null)
                return null!;
            // mapping
            var companyDto =  _mapper.Map<CompanyDetailsDto>(company);          
            return companyDto;  


        }

        public async Task<List<string>> GetCompanyImages(int id)
        {
            var CompanyImageSpec =new CompanyTravelImagesSpecification(id);
            var companyWithTravels = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(CompanyImageSpec);
            if (companyWithTravels is null)
                return null!;
            var imageUrls = companyWithTravels.Travels
            .SelectMany(t => t.Images)
            .Select(img => img.ImageUrl)
            .ToList();
            return imageUrls;
        }

        public async Task<int> GetCountAsync(CompanySpecParams companySpec)
        {
            var DataCountspec = new CompanyWithFilterationForCountSpec(companySpec);

            return await _unitOfWork.Repository<Company>().GetCountSpecAsync(DataCountspec);
        }

        public async Task<CompanyDetailsDto?> UpdateRate(CompanyDetailsDto companyDetailsDto)
        {
            var companySpec = new CompanyWithIncludesSpec(companyDetailsDto.Id);
            var companyDatabase = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(companySpec);
            if (companyDatabase is null)
                return null;

            // ✅ Recalculate and set the new rating
            companyDatabase.Rating =(float) companyDetailsDto.Rating;

            // ✅ Get the actual list of updated ratings from DB
            var updatedRatings = await _unitOfWork.RateRepo.WhereAsync(r => r.CompanyId == companyDetailsDto.Id);
            companyDatabase.Ratings = updatedRatings.ToList(); // 💡 Ensure UserId is not null

            _unitOfWork.Repository<Company>().Update(companyDatabase);
            _unitOfWork.Complete();

            return _mapper.Map<CompanyDetailsDto>(companyDatabase); // ✅ Return updated state
        }


    }
}
