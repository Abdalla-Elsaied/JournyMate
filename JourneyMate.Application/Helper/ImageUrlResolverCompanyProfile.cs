using JourneyMate.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Helper
{
    public class ImageUrlResolverCompanyProfile : IValueResolver<Company, CompanyDetailsDto, string?>
    {
        private readonly IConfiguration _configuration;

        public ImageUrlResolverCompanyProfile(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Company source, CompanyDetailsDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImageUrl))
            {
                return $"{_configuration["ApiBase"]}{source.ProfileImageUrl}";
            }
            return string.Empty;
        }
    }
}
