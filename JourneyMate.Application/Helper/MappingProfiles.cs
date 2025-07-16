using AutoMapper;
using JourneyMate.Application.DTOs;

namespace JourneyMate.Application.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserVm, AppUser>().ReverseMap();
            CreateMap<UserSignUpVm, AppUser>().ReverseMap();
            CreateMap<RoleVm, IdentityRole>().ForMember(D => D.Name, O => O.MapFrom(s => s.RoleName)).ReverseMap();


            CreateMap<Travel, TravelVm>()
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category != null ? src.category.CategoryName : null))
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.category != null ? src.Company.CompanyName : null))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<TravelVm, Travel>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()); // We handle images manually
            CreateMap<Company, CompanyVm>()
            .ForMember(dest => dest.ProfileImageFile, opt => opt.Ignore())
            .ForMember(dest => dest.Travels, opt => opt.Ignore())
            .ForMember(dest => dest.SocialMediaLinks, opt => opt.MapFrom(src => src.SocialMediaLinks))
            .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src => src.PaymentMethods));

            CreateMap<CompanyVm, Company>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Travels, opt => opt.Ignore())
                .ForMember(dest => dest.SocialMediaLinks, opt => opt.MapFrom(src => src.SocialMediaLinks))
                .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src => src.PaymentMethods))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl));

            CreateMap<SocialMediaLink, SocialMediaLinkViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
            .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => src.Platform))
            .ReverseMap();
            CreateMap<PaymentMethodViewModel, PaymentMethod>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.MethodName))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Details))
            .ReverseMap();

            CreateMap<WorkingHourViewModel, WorkingHour>().ReverseMap();

            CreateMap<ImageVM, Image>().ReverseMap();
            CreateMap<TourismCompanyRequest, TourismCompanyRequestDto>().ReverseMap();
            CreateMap<APIEventItem, EventItem>().ReverseMap();

            CreateMap<ItineraryDayVm, ItinraryDay>().ReverseMap();
        }

    }
}
