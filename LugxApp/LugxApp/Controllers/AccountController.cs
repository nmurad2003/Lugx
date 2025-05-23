using LugxApp.Models;
using LugxApp.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LugxApp.Controllers;

public class AccountController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
{
    #region Initializing Roles and Admins
    //public async Task<IActionResult> CreateRoles()
    //{
    //    List<string> roleNames = ["admin", "member"];

    //    foreach (string roleName in roleNames)
    //        await _roleManager.CreateAsync(new IdentityRole() { Name = roleName });

    //    return Ok("Roles Created!");
    //}

    //public async Task<IActionResult> CreateAdmins()
    //{
    //    var admin = new AppUser()
    //    {
    //        UserName = "admin@code.edu.az",
    //        Email = "admin@code.edu.az",
    //        FirstName = "admin",
    //        LastName = "admin",
    //    };

    //    await _userManager.CreateAsync(admin, "admin1234");
    //    await _userManager.AddToRoleAsync(admin, "admin");

    //    return Ok("Admins Created!");
    //}
    #endregion

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new AppUser()
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "member");

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        AppUser? user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password!");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid email or password!");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
