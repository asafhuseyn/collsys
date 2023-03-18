using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CollSys.Matm.Kitabxana.Presentation.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CollSys.Matm.Kitabxana.Presentation.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
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
            [Display(Name = "Ad")]
            [Required(ErrorMessage = "Ad yazılmalıdır! ")]
            [MaxLength(50, ErrorMessage = "Ad 50 xarakterdən az olmalıdır! ")]
            public string FirstName { get; set; }

            [Display(Name = "Soyad")]
            [Required(ErrorMessage = "Soyad yazılmalıdır! ")]
            [MaxLength(50, ErrorMessage = "Soyad 50 xarakterdən az olmalıdır! ")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} ən az {2} ən çox {1} xarakterdən ibarət ola bilər.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Şifrə")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Şifrə (təkrar)")]
            [Compare("Password", ErrorMessage = "Şifrə eyni deyil.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Email (Referans istifadəçi)")]
            [MaxLength(1000)]
            public string referenceEmail { get; set; }

            [Required]
            [Display(Name = "Şifrə (Referans istifadəçi)")]
            [MaxLength(1000)]
            [DataType(DataType.Password)]
            public string referenceUserPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var refUser = _userManager.FindByEmailAsync(Input.referenceEmail).Result;
                var refControl = _userManager.CheckPasswordAsync(refUser, Input.referenceUserPassword).Result;

                if (refControl)
                {
                    var user = new UserModel { FirstName = Input.FirstName, LastName = Input.LastName, UserName = Input.FirstName + "_" + Input.LastName, Email = Input.Email, ReferenceId = refUser.Id };
                    bool flag = false;
                    try
                    {
                        var result = await _userManager.CreateAsync(user, Input.Password);
                        if (result.Succeeded)
                            flag = true;
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("CurrMoInfo.CurrMo", "Qeydiyyat tamamlana bilmədi. Yazdığınız email adresini dəyişdirib yenidən yoxlayın.");
                        flag = false;
                    }
                    if (flag)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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
                }
                else
                {
                    ModelState.AddModelError("CurrMoInfo.CurrMo", "Referans istifadəçiya aid məlumatlar səhvdir.");
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
