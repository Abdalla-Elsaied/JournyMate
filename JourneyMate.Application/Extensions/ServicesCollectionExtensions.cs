using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Application.Helper;
using JourneyMate.Application.ServicesApi.Implementaion;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Application.Servieces;
using JourneyMate.Application.Setting;
using JourneyMate.Infrastracture.Data;
using JourneyMate.Infrastracture.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JourneyMate.Application.Extensions
{
    public  static class ServicesCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<EmailSetting>();
            services.AddHttpClient<IPaymobServices, PaymobServices>();
            services.AddScoped(typeof(IPaymobServices), typeof(PaymobServices));
            services.AddHttpClient<PaymobServices>();
            services.AddAutoMapper(M => M.AddProfile(new APIMappingProfile()));
            services.AddScoped<ICompanyServices, CompanyServices>();
            services.AddScoped<ITourismCompanyRequestService, TourismCompanyRequestService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IRateServices, RateServices>();
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<ImageUrlResolverCompanyCover>();
            services.AddScoped<ImageUrlResolverCompanyProfile>();
            services.AddScoped<CompanyTravelImageUrlForResolver>();
            services.AddScoped<ImageUrlsForTravelResolverCover>();
            services.AddScoped<ImageUrlsForTravelResolverProfiles>();
            services.AddScoped<ImageUrlsForTravelResolver>();
            services.AddScoped<ITravelServices, TravelServices>();
            services.AddScoped<IIpiEnventService, IpiEnventService>();
            services.AddScoped<IRecommendationServices, RecommendationServices>();
           
            services.AddScoped<ISearchServices, SearchServices>();
          
            services.AddScoped<DocumentSetting>(provider =>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                return new DocumentSetting(env);
            });
            
        

        }
    }
}
