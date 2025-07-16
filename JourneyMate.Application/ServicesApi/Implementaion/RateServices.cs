using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Implementaion
{
    public class RateServices : IRateServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RateServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<UserRating?> GetUserRatingForCompany(int companyId, string userId)
        {
            return await _unitOfWork.RateRepo
                .FindAsync(r => r.CompanyId == companyId && r.UserId == userId);
        }
        public async Task<double> GetAverageRatingForCompany(int companyId)
        {
            var ratings = await _unitOfWork.RateRepo
                .WhereAsync(r => r.CompanyId == companyId); // New method needed
            return ratings.Any() ? ratings.Average(r => r.Rating) : 0;
        }
        public async Task<UserRating> AddOrUpdateUserRating(CompanyRatingDto dto)
        {
            var existing = await GetUserRatingForCompany(dto.CompanyId, dto.UserId!);

            if (existing != null)
            {
                existing.Rating = dto.Rating;
                existing.RatedAt = DateTime.Now;    
            }
            else
            {
                existing = new UserRating
                {
                    CompanyId = dto.CompanyId,
                    UserId = dto.UserId,
                    Rating = dto.Rating,
                    Message=dto.Message,
                };
                _unitOfWork.RateRepo.Add(existing);
            }
           await _unitOfWork.CompleteAsync();
           return existing;
        }
    }
}
