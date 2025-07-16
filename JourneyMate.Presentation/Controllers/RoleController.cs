using Microsoft.AspNetCore.Authorization;

namespace JourneyMate.Presentation.Controllers
{
    [Authorize(Roles = "Super,Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(string name)
        {
            var roles = await _roleService.GetAllRolesAsync(name);

            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVm roleVm)
        {
            if (ModelState.IsValid)
            {
                var isSucceeded = await _roleService.CreateRoleAsync(roleVm);
                if (isSucceeded)
                    return RedirectToAction("Index");
            }
            return View(roleVm);
        }
        public async Task<IActionResult> Edit(String? id)
        {
            var role = await _roleService.GetRoleByIdAsync(id ?? "");
            if (role == null) return NotFound();
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleVm roleVm)
        {
            if (id != roleVm.Id)
                return BadRequest();
            if (ModelState.IsValid) // Server-side validation
            {
                var isSucceeded = await _roleService.UpdateRoleAsync(id, roleVm);
                if (isSucceeded) return RedirectToAction(nameof(Index));
            }
            return View(roleVm);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            var role = await _roleService.GetRoleByIdAsync(id ?? "");
            if (role == null) return NotFound();
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleVm deletedRole)
        {
            if (id != deletedRole.Id)
                return BadRequest();
            var isDeleted = await _roleService.DeleteRoleAsync(id);
            if (isDeleted) return RedirectToAction(nameof(Index));

            return View(deletedRole);
        }
    }
}