using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuctionSystem.Presentation.Controller
{
    /// <summary>
    /// واجهة الوصول الخاصة بالأدوار والصلاحيات
    /// </summary>
    [ApiController]
    [Route("api/access")]
    [Authorize(Roles = "Admin")]
    public class AccessController : ControllerBase
    {
        private readonly RoleService _roleService;

        /// <summary>
        /// منشئ الـ Controller
        /// </summary>
        /// <param name="roleService">خدمة الأدوار والصلاحيات</param>
        public AccessController(RoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// الحصول على جميع الأدوار والصلاحيات
        /// </summary>
        /// <remarks>
        /// </remarks>

        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesWithPermissionsAsync();
            return Ok(roles);
        }
    }
}
