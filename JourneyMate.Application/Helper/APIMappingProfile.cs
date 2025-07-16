using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Core.Models.Booking_Aggregation;
using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.Helper
{
    public class APIMappingProfile:Profile
    {
        public APIMappingProfile()
        {

            #region company mapping 
            CreateMap<Company, CompanyListDto>();

            CreateMap<Company, CompanyDetailsDto>()
                .ForMember(dest => dest.SocialMediaLinks, opt => opt.MapFrom(src => src.SocialMediaLinks))
                .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src => src.PaymentMethods))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.WorkingHours))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom<ImageUrlResolverCompanyProfile>())
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<ImageUrlResolverCompanyCover>());

            CreateMap<CompanyDetailsDto, Company>()
             .ForMember(dest => dest.SocialMediaLinks, opt => opt.MapFrom(src => src.SocialMediaLinks))
             .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src => src.PaymentMethods))
             .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings))
             .ForMember(dest => dest.Travels, opt => opt.MapFrom(src => src.Travels));

            CreateMap<SocialMediaLink, SocialMediaLinkDto>().ReverseMap();

            CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();

            CreateMap<UserRating, CompanyRatingDto>().ReverseMap();

            CreateMap<WorkingHour, WorkingHourDto>()
             .ForMember(dest => dest.WorkingTime, opt => opt.MapFrom(src =>
               src.IsClosed
                   ? "Closed"
                   : $"{TimeOnly.FromTimeSpan(src.OpenTime.Value):h\\:mm tt} - {TimeOnly.FromTimeSpan(src.CloseTime.Value):h\\:mm tt}"
           ));
            #endregion
  
            #region Travels Mapping
            CreateMap<Travel, TravelListDto>()
           .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.CompanyName : null))
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category != null ? src.category.CategoryName : null));

            CreateMap<Travel, TravelDetailsDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.CompanyName : null))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom<ImageUrlsForTravelResolverProfiles>())
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<ImageUrlsForTravelResolverCover>())
                .ForMember(dest => dest.CompanyProfileImageUrl, opt => opt.MapFrom<CompanyTravelImageUrlForResolver>())
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category != null ? src.category.CategoryName : null))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom<ImageUrlsForTravelResolver>());

            CreateMap<ItinraryDay, ItinraryDayDto>();
            #endregion

            #region Booking mapping
   
            CreateMap<Booking, BookingToReturnDto>()
            .ForMember(dest => dest.BookingItem, opt => opt.MapFrom(src => src.Item));

            CreateMap<BookingItem, BookingItemDto>()
                .ForMember(D => D.TravelId, O => O.MapFrom(s => s.TravelItemBooked.TravelId))
                .ForMember(D => D.Title, O => O.MapFrom(s => s.TravelItemBooked.Title))
                .ForMember(D => D.TravelProfileUrl, O => O.MapFrom(s => s.TravelItemBooked.TravelProfileUrl));
            #endregion

            #region Category Mapping
            CreateMap<Category, CateroryDto>().ReverseMap();
            #endregion 
            CreateMap<APIEventItem, EventDto>().ReverseMap();

            CreateMap<TourismCompanyRequest, TourismCompanyRequestDto>().ReverseMap();

        }
    }
}
