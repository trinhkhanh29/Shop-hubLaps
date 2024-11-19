// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
                await LoadAsync(user);
                return Page();
            }

            // Update user properties
            user.FirstName = Input.FirstName; // Update First Name
            user.LastName = Input.LastName; // Update Last Name
            user.DiaChi = Input.DiaChi; // Update address
            user.NgaySinh = Input.NgaySinh; // Update date of birth

            // Update phone number
            var currentPhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != currentPhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Attempt to update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Add errors to model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            // Refresh sign-in to ensure authentication state is updated
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }

    }
}
