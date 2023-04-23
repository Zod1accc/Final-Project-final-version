using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechnoStore.Abstractions.MailService;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;
using TechnoStore.ViewModels.AccountViewModels;
using TechnoStore.ViewModels.Mailsender;

namespace TechnoStore.Controllers
{
    public class AccountController : Controller
    {
        private DataContext _dataContext;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IMailService _mailService;

		public AccountController(DataContext dataContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IMailService mailService)
		{
			_dataContext = dataContext;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mailService = mailService;
		}

		public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if(!ModelState.IsValid) return View(registerVM);

            AppUser user = await _userManager.FindByNameAsync(registerVM.Username);

            if(user != null)
            {
                ModelState.AddModelError("Username", "Username has taken!");
                return View(registerVM);
            }

            user = await _userManager.FindByEmailAsync(registerVM.Email);

            if(user is not null)
            {
                ModelState.AddModelError("Email", "Email has taken!");
                return View(registerVM);
            }

            user = new AppUser
            {
                UserName = registerVM.Username,
                Fullname = registerVM.Fullname,
                Email = registerVM.Email,
            };

            var result = await _userManager.CreateAsync(user,registerVM.Password);
            
            if(!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(registerVM);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "SuperAdmin");

            if (!roleResult.Succeeded)
            {
                foreach (var err in roleResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(registerVM);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index","Home");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if(!ModelState.IsValid) return View(loginVM);

            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password incorrect!");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect!");
                return View(loginVM);
            }
            
			return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (!ModelState.IsValid) return View(forgotPasswordVM);

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user is null)
            {
                ModelState.AddModelError("Email", "Email not found");
				return View(forgotPasswordVM);
			}
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token },HttpContext.Request.Scheme);
            await _mailService.SendEmailAsync(new MailRequestVM { ToEmail = forgotPasswordVM.Email, Subject = "ResetPassword", Body = $"<a href='{link}' >ResetPassword</a>" });
            return RedirectToAction("login","account");
        }

        public async Task<IActionResult> ResetPassword(string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null) return NotFound();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel reseetPasswordVM,string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            if (!ModelState.IsValid) return View(reseetPasswordVM);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

            //string yeniSifreHash = "";
            //if (user is not null)
            //{
            //    using (var sha256 = SHA256.Create())
            //    {
            //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(reseetPasswordVM.Password));
            //        yeniSifreHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            //    }
            //    if (user.PasswordHash == yeniSifreHash)
            //    {
            //        ModelState.AddModelError("", "Eynidi");
            //        return View();
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Eynidi deyil");
            //        return View();
            //    }
            //}

            var identityUser = await _userManager.ResetPasswordAsync(user, token, reseetPasswordVM.ConfirmPassword);
            return RedirectToAction("index","home");
        }
	}
}
