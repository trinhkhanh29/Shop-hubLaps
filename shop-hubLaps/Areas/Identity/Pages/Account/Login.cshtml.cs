// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using shop_hubLaps.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using shop_hubLaps.Controllers;
using System.Security.Claims;

namespace shop_hubLaps.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<SampleUser> _signInManager;
        private readonly UserManager<SampleUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<SampleUser> signInManager, UserManager<SampleUser> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

      
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

     
        public string ReturnUrl { get; set; }

     
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
        
            [Required]
            [EmailAddress]
            public string Email { get; set; }

          
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

          
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public IActionResult OnPostExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"External login error: {remoteError}");
                return RedirectToPage("./Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./Login");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                // Ensure FirstName and LastName are always set (default if not provided)
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "DefaultFirstName";
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "DefaultLastName";

                var user = new SampleUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,  // Ensure it's never null
                    LastName = lastName     // Ensure it's never null
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            ModelState.AddModelError(string.Empty, "Email claim not received from external provider.");
            return RedirectToPage("./Login");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Tìm người dùng theo email
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            /*var username = new EmailAddressAttribute().IsValid(Input.Email)
                ? (await _userManager.FindByEmailAsync(Input.Email))?.UserName 
                : Input.Email;*/
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Lấy thông tin FirstName và LastName sau khi đăng nhập thành công
                    var firstName = user.FirstName;

                    var lastName = user.LastName;

                    // Sử dụng TempData hoặc ViewBag để chuyển thông tin sang giao diện
                    TempData["FullName"] = $"{firstName} {lastName}";

                    // Kiểm tra vai trò của người dùng và chuyển hướng tương ứng
                    //if (await _userManager.IsInRoleAsync(user, "Admin"))
                   // {
                    //    return LocalRedirect("/Admin/Index");
                   // }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
           
           
           

            // If we got this far, something failed, redisplay form
            return Page();
        }



    }
}
