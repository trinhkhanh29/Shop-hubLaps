using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shop_hubLaps.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(UserManager<SampleUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Danh sách người dùng và vai trò
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }

            return View(userRoles);
        }

        // Quản lý vai trò của người dùng
        public async Task<IActionResult> ManageRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new ManageRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                AssignedRoles = userRoles.ToList(),
                AvailableRoles = roles.Select(r => r.Name).ToList()
            };

            return View(model);
        }

        // Gán vai trò cho người dùng
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Vai trò không tồn tại.");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                // Thiết lập thông báo thành công vào TempData
                TempData["SuccessMessage"] = $"Vai trò '{roleName}' đã được gán cho người dùng {user.UserName} thành công.";
            }
            else
            {
                // Xử lý lỗi nếu có
                TempData["ErrorMessage"] = "Gán vai trò không thành công.";
            }

            // Chuyển hướng về trang Index sau khi gán vai trò
            return RedirectToAction(nameof(Index));
        }



        // Xóa vai trò khỏi người dùng
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Vai trò không tồn tại.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                // Thiết lập thông báo thành công vào TempData
                TempData["SuccessMessage"] = $"Vai trò '{roleName}' đã được xóa khỏi người dùng {user.UserName} thành công.";
            }
            else
            {
                // Xử lý lỗi nếu có
                TempData["ErrorMessage"] = "Xóa vai trò không thành công.";
            }

            // Chuyển hướng về trang Index sau khi xóa vai trò
            return RedirectToAction(nameof(Index));
        }
        // GET: User/Create
        public IActionResult Create()
        {
            return View(); // Return the view where the form is present
        }

        // POST: User/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new SampleUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DiaChi = model.DiaChi,
                    NgaySinh = model.NgaySinh,
                    Profile = model.Profile
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Success message
                    TempData["SuccessMessage"] = "Người dùng đã được tạo thành công!";
                }
                else
                {
                    // Error message
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi tạo người dùng!";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Thông tin nhập vào không hợp lệ!";
            }

            // Always redirect to the Index page, whether it's success or failure
            return RedirectToAction(nameof(Index));
        }

        // Xóa người dùng
        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Thông báo thành công khi xóa người dùng
                TempData["SuccessMessage"] = "Người dùng đã được xóa thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi khi xóa, thêm thông báo lỗi vào ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // Thông báo lỗi khi xóa không thành công
            TempData["ErrorMessage"] = "Có lỗi khi xóa người dùng. Vui lòng thử lại!";
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public async Task<IActionResult> LockUser(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    user.LockoutEnabled = true;
        //    user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100); // Khóa vĩnh viễn
        //    var result = await _userManager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return Json(new { success = true });
        //    }

        //    return Json(new { success = false, errors = result.Errors.Select(e => e.Description) });
        //}

        //[HttpPost]
        //public async Task<IActionResult> UnlockUser(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    user.LockoutEnd = null; // Mở khóa
        //    var result = await _userManager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return Json(new { success = true });
        //    }

        //    return Json(new { success = false, errors = result.Errors.Select(e => e.Description) });
        //}

    }

    // ViewModel cho danh sách người dùng và vai trò
    public class UserWithRolesViewModel
    {
        public SampleUser User { get; set; }
        public List<string> Roles { get; set; }
    }

    // ViewModel để quản lý vai trò
    public class ManageRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> AssignedRoles { get; set; }
        public List<string> AvailableRoles { get; set; }
    }

    // ViewModel cho tạo người dùng
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DiaChi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Profile { get; set; }
        public string Password { get; set; }
    }
}
