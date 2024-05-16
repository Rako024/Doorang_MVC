using Core.Models;
using Doorang_MVC.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Doorang_MVC.Controllers
{
    public class AccountController : Controller
    {
        
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
           var user = await _userManager.FindByNameAsync(login.UserName);
            if(user == null)
            {
                ModelState.AddModelError("", "User Or Password Not Valid!");
                return View();
            }

            var result =  await _signInManager.PasswordSignInAsync(user, login.Password,login.RememberMe,false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "User Or Password Not Valid!");
                return View();
            }
            return RedirectToAction("index","Home");
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser()
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                UserName = register.UserName,
            };
           var result = await  _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                    foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            
               await _userManager.AddToRoleAsync(user, "Member");
            
            
            return Ok(result);
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("Admin");
        //    IdentityRole role2= new IdentityRole("Member");
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role1);
        //    return Ok("piuvv");
        //}

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user = new AppUser()
        //    {
        //        Name = "Admin",
        //        Surname = "Admin",
        //        Email = "Admin@gmail.com",
        //        UserName = "Admin",
        //    };
        //    var result = await _userManager.CreateAsync(user, "Admin123@");
        //    return Ok(result);
        //}
    }
}
