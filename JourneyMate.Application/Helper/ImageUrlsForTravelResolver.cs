using JourneyMate.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Helper
{
    public class ImageUrlsForTravelResolver : IValueResolver<Travel, TravelDetailsDto, List<string>>
    {
        private readonly IConfiguration _configuration;

        public ImageUrlsForTravelResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> Resolve(Travel source, TravelDetailsDto destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = _configuration["ApiBase"];
            return source.Images?
                .Where(img => !string.IsNullOrEmpty(img.ImageUrl))
                .Select(img => $"{baseUrl}/{img.ImageUrl}")
                .ToList() ?? new List<string>();
        }
    }

}
