

using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using JourneyMate.Infrastracture.Specefications.TravelSpecifications;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class TravelServices : ITravelServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public TravelServices(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async  Task< IReadOnlyList<TravelDetailsDto>> GetAllTravles(TravelSpecPrams travelSpecPrams)
        {
            var spec = new TravelSpecifications(travelSpecPrams);
            var travels = await _unitOfWork.Repository<Travel>().GetAllWithSpecificaiton(spec);
            if (travels is null)
            {
                return null!;       

            }
            // mapping  
            var travelDtos = _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
            return travelDtos;
        }

        public async Task<TravelDetailsDto?> GetTravelById(int id)
        {
            var spec = new TravelSpecifications(id);
            var travel =  await _unitOfWork.Repository<Travel>().GetByIdWithSpecifications(spec);
            if(travel is null) 
                return null!;
            var travelDto = _mapper.Map<TravelDetailsDto>(travel);
            return travelDto;
        }

        public async Task<int> GetCountAsync(TravelSpecPrams travelSpec, ISpec<Travel> spec)
        {
            
            return await _unitOfWork.Repository<Travel>().CountSpecAsync(spec);
        }

        public async Task<IReadOnlyList<TravelDetailsDto>> GetDiscountedTravles(TravelSpecPrams travelPrams)
        {
            var spec = new DiscountedTravelSpecs( travelPrams);

            var travels = await _unitOfWork.Repository<Travel>().GetAllWithSpecificaiton(spec);
            if (travels is null)
            {
                return null!;

            }
            // mapping  
            var travelDtos = _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
            return travelDtos;

        }
        public async Task<IReadOnlyList<TravelDetailsDto>> GetTravelsLeavingSoon(TravelSpecPrams travelPrams)
        {
            var spec = new LeavingSoonTravelSpecs(travelPrams);

            var travels = await _unitOfWork.Repository<Travel>().GetAllWithSpecificaiton(spec);
            if (travels is null)
            {
                return null!;

            }
            // mapping  
            var travelDtos = _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
            return travelDtos;

        }
        public async Task<ServiceResult> AddLikeToTravel(int travelId,string userId)
        {
            var travel =  _unitOfWork.Repository<Travel>().GetById(travelId);
            if (travel is null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Travel not found"
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var alreadyLiked = await _unitOfWork.FavTravelRepo.AnyAsync(x => x.UserId == userId && x.TravelId == travelId);
            if (alreadyLiked)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "You already liked this travel."
                };
            }

            travel.FavoritedByUsers.Add(new FavoriteTravel
            {
                TravelId = travelId,
                UserId = userId
            });

             _unitOfWork.Complete();

            return new ServiceResult
            {
                Success = true,
                Message = "Travel liked successfully"
            };
        }
        public async Task<ServiceResult> RemoveLikeFromTravel(int travelId, string userId)
        {
           
            var favorite = await _unitOfWork.FavTravelRepo.FirstOrDefaultAsync(x => x.TravelId == travelId && x.UserId == userId);
            if (favorite == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "You have not liked this travel."
                };
            }

            _unitOfWork.Repository<FavoriteTravel>().Delete(favorite);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Failed to remove from favorites."
                };
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Travel removed from favorites successfully."
            };
        }

        public async Task<IReadOnlyList<TravelDetailsDto>> GetNewestTravelsForCompany(TravelSpecPrams travelPrams, int companyId)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(companyId);
            if (company is null) return new List<TravelDetailsDto>(); // Return empty list instead of null

            var specs = new NewestTravelSpecifications(travelPrams, companyId);
            var travels = await _unitOfWork.Repository<Travel>().GetAllWithSpecificaiton(specs);

            if (travels is null || !travels.Any())
            {
                return new List<TravelDetailsDto>(); // Return empty list instead of null
            }

            // mapping  
            var travelDtos = _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
            return travelDtos;
        }

        public async Task<IReadOnlyList<TravelDetailsDto>> GetNewestTravels(TravelSpecPrams travelPrams)
        {
            var specs = new NewestTravelSpecifications(travelPrams);
            var travels = await _unitOfWork.Repository<Travel>().GetAllWithSpecificaiton(specs);
            if (travels is null)
            {
                return null!;

            }
            // mapping  
            var travelDtos = _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
            return travelDtos;

        }

        public async Task<List<TravelDetailsDto>> GetFavTravelsByUserId(string userId)
        {
            var listOfFavTravel = await _unitOfWork.FavTravelRepo.FavtrvelForUser(userId);
            if(listOfFavTravel is null) { return null!; }
            var UserTravel= new List<TravelDetailsDto>();
            foreach (var fav in listOfFavTravel)
            {
                var travel = await GetTravelById(fav.TravelId);
                if (travel != null)
                { 
                  UserTravel.Add(travel);
                }
            }
            return UserTravel;

        }
    }
}
