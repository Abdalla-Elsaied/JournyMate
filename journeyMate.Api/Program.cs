
using journeyMate.Api.Chat;
using journeyMate.Api.Errors;
using journeyMate.Api.MiddelWears;
using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Application.Extensions;
using JourneyMate.Application.IServeces;
using JourneyMate.Application.Servieces;
using JourneyMate.Application.Setting;
using JourneyMate.Core.Models;
using JourneyMate.Infrastracture.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using X.Paymob.CashIn;

namespace journeyMate.Api
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
            builder.Services.AddSignalR();
            builder.Services.AddApplication();
            var connectionString = builder.Configuration.GetConnectionString("constr");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                // used to add info about the prameter of the query Ex ... where id  = @p1(22)
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging();
            });
             
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "");

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience =builder.Configuration["JWT:ValidAudience"],
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(double.Parse(builder.Configuration["JWT:Duration"])),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                    };
                });
           
            #region Validation Confguration Error 
            builder.Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(E => E.Value.Errors.Count > 0)
                                                        .SelectMany(V => V.Value.Errors)
                                                        .Select(M => M.ErrorMessage).ToArray();
                    var ValidationResponse = new ErrorValidationResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationResponse);
                };
            });
            #endregion

            builder.Services.AddAuthorization();
      
            builder.Services.Configure<AiApiSettings>(
            builder.Configuration.GetSection("AiApiSettings"));

            #region CROS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #endregion
            // builder.Services.AddSwaggerGen();
            #region Swagger Setting
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This�is�to�generate�the�Default�UI�of�Swagger�Documentation����
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET�8�Web�API",
                    Description = " ITI Projrcy"
                });
                //�To�Enable�authorization�using�Swagger�(JWT)����
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter�'Bearer'�[space]�and�then�your�valid�token�in�the�text�input�below.\r\n\r\nExample:�\"Bearer�eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
         new OpenApiSecurityScheme
         {
         Reference = new OpenApiReference
         {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
         }
         },
         new string[] {}
         }
         });
            });
            #endregion
            var app = builder.Build();
            // Use CORS before controllers
            app.UseCors("AllowAll");
            #region Explicitly Injection
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;
            var _dbContext = Services.GetRequiredService<AppDbContext>();
            var _userManager = Services.GetRequiredService<UserManager<AppUser>>();
            var _roleManger = Services.GetRequiredService<RoleManager<IdentityRole>>();
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();
                await SeedData.SeedAsync(_dbContext, _userManager, _roleManger);
                await SeedData.EnsureAdminExists(_userManager);
            }
            catch (Exception Ex)
            {

                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(Ex, "an error has been occurred during apply the Migration");
            }
            #endregion

   
            app.UseMiddleware<ServerMiddelWear>();  // external middelwear for server error 
            app.UseStatusCodePagesWithReExecute("/endpointerrors/{0}");   // for notfound endpoint 
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JourneyMate API V1");
            });

            app.MapHub<ChatHub>("/chathub");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
