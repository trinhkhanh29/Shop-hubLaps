// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using shop_hubLaps.Areas.Identity.Data;


namespace shop_hubLaps.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SampleUser> _signInManager;
        private readonly UserManager<SampleUser> _userManager;
        private readonly IUserStore<SampleUser> _userStore;
        private readonly IUserEmailStore<SampleUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<SampleUser> userManager,
            IUserStore<SampleUser> userStore,
            SignInManager<SampleUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

       
        [BindProperty]
        public InputModel Input { get; set; }

     
        public string ReturnUrl { get; set; }

      
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [StringLength(256, ErrorMessage = "Email phải là một địa chỉ email hợp lệ.")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp nhau.")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và tối đa {1} ký tự.", MinimumLength = 6)]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "Tên phải có tối đa {1} ký tự.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }


            [Required]
            [StringLength(50, ErrorMessage = "Họ phải có tối đa {1} ký tự.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }



        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                // Gán giá trị cho các trường FirstName và LastName
                user.FirstName = Input.FirstName;

                user.LastName = Input.LastName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã tạo một tài khoản mới với mật khẩu.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn",
                        $"Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấn vào đây</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private SampleUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<SampleUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Không thể tạo một thể hiện của '{nameof(SampleUser)}'. " +
                   $"Đảm bảo rằng '{nameof(SampleUser)}' không phải là một lớp trừu tượng và có một hàm khởi tạo không tham số, hoặc " +
                   $"thay vào đó hãy ghi đè trang đăng ký trong /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<SampleUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Giao diện mặc định yêu cầu một kho người dùng có hỗ trợ email.");
            }
            return (IUserEmailStore<SampleUser>)_userStore; 
        }
    }
}
