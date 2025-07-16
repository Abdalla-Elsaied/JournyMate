using JourneyMate.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Helper
{
    public class ImageUrlsForTravelResolverCover : IValueResolver<Travel, TravelDetailsDto, string?>
    {
        private readonly IConfiguration _configuration;

        public ImageUrlsForTravelResolverCover(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      

        public string? Resolve(Travel source, TravelDetailsDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.CoverImageUrl))
            {
                return $"{_configuration["ApiBase"]}/{source.CoverImageUrl}";
            }
            return string.Empty;
        }
    }

}
