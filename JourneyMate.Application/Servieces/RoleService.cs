using AutoMapper;
using JourneyMate.Application.ViewModel;

namespace JourneyMate.Application.Servieces;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<bool> CreateRoleAsync(RoleVm roleVm)
    {
        var mappeRole = _mapper.Map<RoleVm, IdentityRole>(roleVm);
        IdentityResult? identityResult = await _roleManager.CreateAsync(mappeRole);
       
        return identityResult.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string id)
    {
        var Role = await _roleManager.FindByIdAsync(id);
        var result = await _roleManager.DeleteAsync(Role??new());
        return result.Succeeded;
    }

    public async Task<List<RoleVm>> GetAllRolesAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await _roleManager.Roles
                .Select(R => new RoleVm { Id = R.Id, RoleName = R.Name ?? "" })
                .ToListAsync();
        }
        else
        {
            var role = await _roleManager.FindByNameAsync(name);
            return role != null ? new List<RoleVm> { _mapper.Map<RoleVm>(role) } : new ();
        }
    }

    public async Task<RoleVm> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return _mapper.Map<RoleVm>(role);
    }

    public async Task<bool> UpdateRoleAsync(string id, RoleVm roleVm)
    {
        var existingRole = await _roleManager.FindByIdAsync(id);
        if (existingRole == null)return false;

        existingRole.Name = roleVm.RoleName;
        var result = await _roleManager.UpdateAsync(existingRole);

        return result.Succeeded;
    }
}
