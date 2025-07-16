using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Helper
{
    public class CompanyTravelImageUrlForResolver: IValueResolver<Travel, TravelDetailsDto, string?>
    {
        private readonly IConfiguration _configuration;

        public CompanyTravelImageUrlForResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Travel source, TravelDetailsDto destination, string? destMember, ResolutionContext context)
        {
            var baseUrl =  _configuration["ApiBase"];

            if (!string.IsNullOrEmpty(source.Company?.ProfileImageUrl))
            {
                return $"{baseUrl}{source.Company.ProfileImageUrl}";
            }
            return string.Empty;
        }
    }
}
