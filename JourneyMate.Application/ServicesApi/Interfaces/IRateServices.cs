using JourneyMate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface IRateServices
    {
        Task<double> GetAverageRatingForCompany(int companyId);
        Task<UserRating?> GetUserRatingForCompany(int  companyId,string userId);
        Task<UserRating> AddOrUpdateUserRating(CompanyRatingDto dto);
    }
}
