using JourneyMate.Application.ViewModel;

namespace JourneyMate.Application.IServeces;

public interface IRoleService
{
    Task<List<RoleVm>> GetAllRolesAsync(string name);
    Task<RoleVm> GetRoleByIdAsync(string id);
    Task<bool> CreateRoleAsync(RoleVm roleVm);
    Task<bool> UpdateRoleAsync(string id, RoleVm roleVm);
    Task<bool> DeleteRoleAsync(string id);
}
