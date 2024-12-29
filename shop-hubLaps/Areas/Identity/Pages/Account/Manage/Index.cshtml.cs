// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using shop_hubLaps.Areas.Identity.Data;

namespace shop_hubLaps.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly SignInManager<SampleUser> _signInManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IndexModel(
            UserManager<SampleUser> userManager,
            SignInManager<SampleUser> signInManager,
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string WarningMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Avatar { get; set; }

        public class InputModel
        {
            [Display(Name = "Profile Picture")]
            public IFormFile ProfilePicture { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Address")]
            public string DiaChi { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime? NgaySinh { get; set; }
        }

        private async Task LoadAsync(SampleUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Avatar = user.Avatar; // Get avatar path

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DiaChi = user.DiaChi,
                NgaySinh = user.NgaySinh
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadProfilePictureAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.ProfilePicture != null && Input.ProfilePicture.Length > 0) // Handle new avatar upload
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure upload folder exists

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(stream);
                }

                user.Avatar = "/uploads/" + fileName; // Save the new avatar path
                StatusMessage = "Hình ảnh đại diện đã được cập nhật."; // Update status message

                await _userManager.UpdateAsync(user);
                return RedirectToPage();
            }

            StatusMessage = "Không có hình ảnh nào được tải lên."; // Update status message if no file uploaded
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveProfilePictureAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.Avatar = null; // Clear avatar
            StatusMessage = "Hình ảnh đại diện đã bị xóa."; // Update status message

            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Validate the model state
            if (!ModelState.IsValid)
            {
                await LoadAsync(user); // Reload user data if the form has errors
                TempData["ErrorMessage"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại!";
                return Page();
            }

            // Regex to validate Vietnamese phone numbers
            string phonePattern = "^(03|05|07|08|09)\\d{8}$";
            if (!Regex.IsMatch(Input.PhoneNumber, phonePattern))
            {
                TempData["WarningMessage"] = "Số điện thoại không hợp lệ. Vui lòng nhập số bắt đầu với 03, 05, 07, 08, hoặc 09 và đủ 10 chữ số.";
                return RedirectToPage();
            }

            // Track updates
            bool isUpdated = false;

            if (string.IsNullOrWhiteSpace(Input.FirstName))
            {
                TempData["WarningMessage"] = "Họ tên không được để trống!";
                return RedirectToPage();
            }
            if (user.FirstName != Input.FirstName)
            {
                user.FirstName = Input.FirstName;
                isUpdated = true;
            }

            if (string.IsNullOrWhiteSpace(Input.LastName))
            {
                TempData["WarningMessage"] = "Tên không được để trống!";
                return RedirectToPage();
            }
            if (user.LastName != Input.LastName)
            {
                user.LastName = Input.LastName;
                isUpdated = true;
            }

            if (user.DiaChi != Input.DiaChi)
            {
                user.DiaChi = Input.DiaChi;
                isUpdated = true;
            }

            if (user.NgaySinh != Input.NgaySinh)
            {
                if (Input.NgaySinh > DateTime.Now)
                {
                    TempData["WarningMessage"] = "Ngày sinh không thể là ngày trong tương lai!";
                    return RedirectToPage();
                }
                user.NgaySinh = Input.NgaySinh;
                isUpdated = true;
            }

            var currentPhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != currentPhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật số điện thoại. Vui lòng thử lại!";
                    return RedirectToPage();
                }
                isUpdated = true;
            }

            if (isUpdated)
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    await LoadAsync(user); // Reload user data if update fails
                    TempData["ErrorMessage"] = "Không thể cập nhật thông tin. Vui lòng thử lại!";
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "Thông tin cá nhân đã được cập nhật thành công!";
            }
            else
            {
                TempData["StatusMessage"] = "Không có thay đổi nào được thực hiện.";
            }

            return RedirectToPage();
        }
    }
}
