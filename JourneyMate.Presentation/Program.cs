using JourneyMate.Application.DTOs.Recommendation;

var builder = WebApplication.CreateBuilder(args);
//Add Session
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
// Add all services in Extention Class
builder.Services.AddAppServices();
//adding Dbcontext
builder.Services.AddDbContext<AppDbContext>(
   options =>
   {
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
   });
//adding Mailkit
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CompanyDash}/{action=index}/{id?}");

app.Run();