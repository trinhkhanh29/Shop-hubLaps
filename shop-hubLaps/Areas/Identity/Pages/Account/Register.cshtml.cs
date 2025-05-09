﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Services;


namespace shop_hubLaps.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SampleUser> _signInManager;
        private readonly UserManager<SampleUser> _userManager;
        private readonly IUserStore<SampleUser> _userStore;
        private readonly IUserEmailStore<SampleUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly EmailService _emailService;

        public RegisterModel(
            UserManager<SampleUser> userManager,
            IUserStore<SampleUser> userStore,
            SignInManager<SampleUser> signInManager,
            ILogger<RegisterModel> logger,
            EmailService emailService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
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

                    // Thêm vai trò mặc định 'User'
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");


                    // Send the password via email
                    await _emailService.SendEmailAsync(Input.Email, "Your Registration Details", $"Your password is: {Input.Password}");
                    
                   
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page(); // Return to the page if role assignment fails
                    }
                    // Tạo mã xác nhận email
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    
                    
                    // Tạo liên kết xác nhận email
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

        private async Task<bool> SendEmailSync(string email, string subject, string confirmLink)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            message.From = new MailAddress("noreply@zethit.com");
            message.To.Add(email);
            message.Subject = subject;
            message.IsBodyHtml = true; 
            message.Body = confirmLink;

            smtpClient.Port = 587;
            smtpClient.Host = "smtp.simply.com";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("USERNAME", "PASSWORD");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                Console.WriteLine("Email send error: " + ex.Message);
                return false;
            }
        }

        public IActionResult OnPostExternalLogin(string provider, string returnUrl = null)
        {
            // Configure the redirect URL to the ExternalLoginCallback method.
            var redirectUrl = Url.Page("./ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            // Challenge the external provider (Microsoft in this case)
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            // If there's a remote error, return to the login page
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"External login error: {remoteError}");
                return RedirectToPage("./Login");
            }

            // Retrieve external login information
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "External login information not received.");
                return RedirectToPage("./Login");
            }

            // Get the email claim from the external provider
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // If the user doesn't exist, create a new user
                    user = new SampleUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "DefaultFirstName",
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "DefaultLastName"
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        // Add external login info
                        await _userManager.AddLoginAsync(user, info);

                        // Sign the user in
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl); // This will redirect the user to the return URL after successful login
                    }
                    else
                    {
                        // Handle errors during user creation
                        foreach (var error in createResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page();
                    }
                }
                else
                {
                    // If the user exists, sign them in
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl); // This will redirect the user to the return URL after successful login
                }
            }

            // Handle the case where the email claim is not found
            ModelState.AddModelError(string.Empty, "Email claim not received from external provider.");
            return RedirectToPage("./Login");
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
