using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fenco.Data;
using Fenco.Models;
using Fenco.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(AppDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Login(VmLogin model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not Valid!");
                    return View(model);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        public IActionResult Users()
        {
            VmUser model = new VmUser()
            {
                CustomUsers = _context.CustomUsers.ToList()
            };

            Dictionary<string, string> userRoles = new Dictionary<string, string>();

            foreach (var user in _context.CustomUsers.ToList())
            {
                IdentityUserRole<string> role = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                if (role!=null)
                {
                    string roleName = _context.Roles.Find(role.RoleId).Name;
                    userRoles.Add(user.Id, roleName);
                }
            }

            model.UserRoles = userRoles;

            return View(model);
        }

    }
}
