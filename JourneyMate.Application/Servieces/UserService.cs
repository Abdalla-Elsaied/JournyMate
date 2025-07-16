
using AutoMapper;
using JourneyMate.Application.Setting;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using JourneyMate.Application.ViewModel;
using JourneyMate.Core.Interfaces;

namespace JourneyMate.Application.Servieces;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
      
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UsersViewModel> GetAllUsersAsync(string? email)
    {
        var usersViewModel = new UsersViewModel();

        if (string.IsNullOrEmpty(email))
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userVm = new UserVm
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Roles = roles
                };

                if (roles.Contains("Company"))
                {
                    usersViewModel.CompanyUsers.Add(userVm);
                }
                else
                {
                    usersViewModel.OtherUsers.Add(userVm);
                }
            }
        }
        else
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return usersViewModel;

            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserVm
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Roles = roles
            };

            if (roles.Contains("Company"))
            {
                usersViewModel.CompanyUsers.Add(userVm);
            }
            else
            {
                usersViewModel.OtherUsers.Add(userVm);
            }
        }

        return usersViewModel;
    }

    public async Task<UserVm?> GetUserDetailsAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var userVM = _mapper.Map<AppUser, UserVm>(user ?? new());
        userVM.Roles = await _userManager.GetRolesAsync(user ?? new());
        return userVM;
    }

    public async Task<CompanyDetailsVM?> GetCompanyDetailsAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        var spec = new CompanyUserIdSpec(id);
        var company = await _unitOfWork.Repository<Company>().GetByIdWithSpecifications(spec);
        return new CompanyDetailsVM
        {
            Id = company.UserId,
            Name = user.UserName,
            Email = user.Email,
            phoneNumber = user.PhoneNumber,
            Travels = company.Travels
        };
    }

    public async Task<IdentityResult> UpdateUserAsync(string id, UserVm userVm)
    {
        var existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        existingUser.FullName = userVm.FullName;
        existingUser.PhoneNumber = userVm.PhoneNumber;
        existingUser.Email = userVm.Email;
        existingUser.SecurityStamp = Guid.NewGuid().ToString();

        return await _userManager.UpdateAsync(existingUser);
    }

    public async Task<IdentityResult> DeleteUserAsync(string id)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        var v = await _userManager.DeleteAsync(user);
        return user == null ? IdentityResult.Failed(new IdentityError { Description = "User not found" }) : await _userManager.DeleteAsync(user);
    }
}

