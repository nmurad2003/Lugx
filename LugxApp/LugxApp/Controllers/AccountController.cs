using Microsoft.AspNetCore.Mvc;

namespace LugxApp.Controllers;

public class AccountController : Controller
{
    #region initializing roles and admins

    #endregion

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return View();
    }
}
