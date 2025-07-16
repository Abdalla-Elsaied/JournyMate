using JourneyMate.Core.Models;
using JourneyMate.Core.Models.Company_Aggregation;
using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JourneyMate.Infrastracture.Data;

public static class SeedData
{
   
    public static async Task SeedAsync(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // 1. Seed Roles
        if (!dbContext.Roles.Any())
        {
            var rolesData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Roles.json");
            var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesData);

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = role.Name,
                    NormalizedName = role.Name.ToUpper()
                });
            }
        }
        
        // 2. Seed Users with Company Role
        List<AppUser> companyUsers = new();
        if (!dbContext.Users.Any())
        {
            var usersData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Users.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

            int count = 0;
            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "P@ssword123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Company");
                    companyUsers.Add(user);
                    count++;

                    if (count == 20) break; // Only seed 20 users
                }
            }
        }
        else
        {
            companyUsers = dbContext.Users
                .Where(u => dbContext.UserRoles
                    .Any(ur => ur.UserId == u.Id &&
                               dbContext.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Company")))
                .Take(20)
                .ToList();
        }

        // 3. Seed Companies and assign UserId
        if (!dbContext.Companies.Any() && companyUsers.Any())
        {
            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var companiesData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Companies.json");
            var companies = JsonSerializer.Deserialize<List<Company>>(companiesData);

            for (int i = 0; i < companies.Count && i < companyUsers.Count; i++)
            {
                companies[i].UserId = companyUsers[i].Id;
                foreach (var day in daysOfWeek)
                {
                    companies[i].WorkingHours.Add(new WorkingHour
                    {

                        DayOfWeek = day,
                        OpenTime = day == "Saturday" || day == "Sunday" ? (TimeSpan?)null : new TimeSpan(9, 0, 0), // Closed on weekends
                        CloseTime = day == "Saturday" || day == "Sunday" ? (TimeSpan?)null : new TimeSpan(18, 0, 0)
                    });
                }
            }

            dbContext.Companies.AddRange(companies);
            await dbContext.SaveChangesAsync();
        }
        // 4 - seeding company request 
        if (!dbContext.tourismCompanyRequests.Any())
        {
            var RowTourismCompanyRequest = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/tourismCompanyRequests.json");
            var TourismCompanyRequestModels = JsonSerializer.Deserialize<List<TourismCompanyRequest>>(RowTourismCompanyRequest);

            dbContext.Set<TourismCompanyRequest>().AddRange(TourismCompanyRequestModels);
            await dbContext.SaveChangesAsync();

        }
        if (!dbContext.Categories.Any())
        {
            var CategoryData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(CategoryData);

            // Ensure CompanyIds are valid by mapping to existing Company Ids
            dbContext.Categories.AddRange(categories);
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Travel.Any())
        {
            var travelsData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Travels.json");
            var travels = JsonSerializer.Deserialize<List<Travel>>(travelsData);

            // Ensure CompanyIds are valid by mapping to existing Company Ids
            var companyIds = dbContext.Companies.Select(c => c.Id).ToList();
            var CategoriesId = dbContext.Categories.Select(c => c.Id).ToList();
            for (int i = 0; i < travels.Count; i++)
            {
                // Assign a valid CompanyId from the seeded companies (cycling through available Ids)
                travels[i].CompanyId = companyIds[i % companyIds.Count];    
                travels[i].CategoryId = CategoriesId[i % CategoriesId.Count];    
            }

            dbContext.Travel.AddRange(travels);
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Images.Any())
        {
            var imagesData = File.ReadAllText("../JourneyMate.Infrastracture/Data/DataSeed/Images.json");
            var images = JsonSerializer.Deserialize<List<Image>>(imagesData);

            dbContext.Images.AddRange(images);
            await dbContext.SaveChangesAsync();
        }
      

    }


    public static async Task EnsureAdminExists(UserManager<AppUser> userManager)
    {
        string adminEmail = "Admin@1.com";
        string adminPassword = "Admin@1.com"; // غيّرها لاحقًا 🚀

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

       
    }
}
