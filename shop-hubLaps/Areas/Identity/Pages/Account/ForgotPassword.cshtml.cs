using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using shop_hubLaps.Areas.Identity.Data;

namespace shop_hubLaps.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<SampleUser> _userManager;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(UserManager<SampleUser> userManager, ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Email { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(Input.Email);

            // Instead of checking for null or email confirmation, 
            // directly redirect to the Reset Password page
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                // Optionally, you could still send an email
                // await _emailSender.SendPasswordResetLinkAsync(user, Input.Email, callbackUrl);

                // Redirect to Reset Password page directly
                return RedirectToPage("/Account/ResetPassword", new { area = "Identity", userId = user.Id, code });
            }

            // Redirect to confirmation page if user is null
            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
