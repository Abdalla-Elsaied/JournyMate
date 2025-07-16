using JourneyMate.Application.ServicesApi.Implementaion;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Application.Servieces;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Presentation.Extensions;

public static class AppServicesExtension
{

    //adding Identity 
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        //adding Identity 
        services.AddIdentity<AppUser, IdentityRole>(config =>
        {
            config.Password.RequiredUniqueChars = 2;
            config.Password.RequireDigit = true;
            config.Password.RequireNonAlphanumeric = false;  // special char
            config.Password.RequiredLength = 5;
            config.Password.RequireUppercase = false;
            config.Password.RequireLowercase = false;
            config.User.RequireUniqueEmail = true;
            config.Lockout.MaxFailedAccessAttempts = 3;
            config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(Config =>
        {
            Config.LoginPath = "/Account/LogIn";
            Config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        });
        // policy controll
        services.AddAuthorization(option =>
        {
            option.AddPolicy("ManageAdmins", policy => policy.RequireClaim("Permission", "ManageAdmins"));
            option.AddPolicy("CreateRole", policy => policy.RequireClaim("Permission", "CreateRole"));
            option.AddPolicy("ManageRoles", policy => policy.RequireClaim("Permission", "ManageRoles"));
            option.AddPolicy("CreateAdmin", policy => policy.RequireClaim("Permission", "CreateAdmin"));
        });
        //add email
        services.AddTransient<EmailSetting>();

        // add UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAiTravelService, AiTravelService>();
        services.AddHttpClient();
        services.AddScoped<IEventItemServece,EventItemServece>();
      /*  services.AddHostedService<GetEventsService>();
        services.AddHostedService<DeleteExpiredEventsService>();*/
        services.AddScoped<IAiEventService, AiEventService>();
        services.AddScoped<DocumentSetting>(provider =>
        {
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            return new DocumentSetting(env);
        });
        services.AddScoped<ICompanyDashService,CompanyDashService>();
        services.AddScoped<ITravelService, TravelService>();
        services.AddScoped<IitineraryService, ItineraryService>();
        services.AddScoped<ITourismCompanyRequestService, TourismCompanyRequestService>();

        //adding Mapper
        services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
        //services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        return services;
    }
}
