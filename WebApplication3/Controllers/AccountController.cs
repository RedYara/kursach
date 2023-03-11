using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Intrerfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManage;
        private readonly _dbContext _context;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManage, _dbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManage = roleManage;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            _signInManager.SignOutAsync();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/apps/index");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginRequest loginData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByNameAsync(loginData.Username);
                    if (user != null)
                    {
                        await _signInManager.SignOutAsync();
                        if ((await _signInManager.PasswordSignInAsync(user, loginData.Password, false, false)).Succeeded)
                        {
                            return Redirect("/Apps");
                        }
                    }
                }
                ModelState.AddModelError("", "Неверный логин или пароль");
            }
            catch (System.Exception e)
            {

                System.Console.WriteLine("Exception: " + e.Message);
            }


            return View("login");

        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectPermanent("Login");
        }


        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterRequest registerData)
        {
            var entity = await _userManager.FindByNameAsync(registerData.Username);
            if (entity != null) return BadRequest("Пользователь с данным именем уже зарегистрирован");
            else
            {
                await _roleManage.CreateAsync(new IdentityRole(roleName: "User"));
                var user = new User
                {
                    Email = registerData.Email,
                    UserName = registerData.Username,
                    SecurityStamp = "dummyStamp",
                };
                var result = await _userManager.CreateAsync(user, registerData.Password);
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "User").Wait();
                    return Redirect("Login");
                }
                else
                {
                    return BadRequest("Could not create user.");
                }
            }

        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public bool CheckUsername(string username)
        {
            if (_context.Users.Select(x => x.UserName).Contains(username))
                return false;
            return true;
        }
        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public bool CheckEmail(string email)
        {
            if (_context.Users.Select(x => x.Email).Contains(email))
                return false;
            return true;
        }
    }
}
