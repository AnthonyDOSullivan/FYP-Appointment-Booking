using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FYP_Appointment_Booking.Data;
using FYP_Appointment_Booking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;


namespace FYP_Appointment_Booking.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class DoctorRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DoctorRegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;



        public DoctorRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DoctorRegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnDocUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Name { get; set; }

            public int DoctorId { get; set; }
            public Doctor? Doctor { get; set; }

            public int? PatientId { get; set; }

            public Patient? Patient { get; set; }

        }

        public async Task OnGetAsync(string returnDocUrl = null, string validationMsg = null)
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ViewData["msg"] = validationMsg;
            ReturnDocUrl = returnDocUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnDocUrl = null)
        {
            returnDocUrl = returnDocUrl ?? Url.Content("~/");
            var role = _roleManager.FindByIdAsync(Input.Name).Result;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                if (!_context.Doctors.Any(d => d.DoctorId == Input.DoctorId))
                {
                    Doctor doctor = new Doctor
                    {
                        Name = "Name Required"

                    };
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    string valmsg = "The doctor id " + Input.DoctorId + " does not exist in our database. We created a new doctor Id for you: " + doctor.DoctorId + ", please use this in the registration";
                    return RedirectToPage("DoctorRegister", new { validationMsg = valmsg });
                }
                if (_context.Users.Any(u => u.DoctorId == Input.DoctorId))
                {
                    string valmsg = "The doctor id " + Input.DoctorId + " is already linked to a user profile";
                    return RedirectToPage("DoctorRegister", new { validationMsg = valmsg });
                }

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PatientId = Input.PatientId, Name = Input.Name, DoctorId = Input.DoctorId };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, role.Name);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnDocUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }

            }
            else
            {
                return Content("Invalid data");

            }

            ViewData["roles"] = _roleManager.Roles.ToList();
            // If we got this far, something failed, redisplay form  
            return Page();
        }
    }
}
