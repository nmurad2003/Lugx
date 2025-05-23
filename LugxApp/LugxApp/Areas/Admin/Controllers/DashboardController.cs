using Microsoft.AspNetCore.Mvc;

namespace LugxApp.Areas.Admin.Controllers;

public class DashboardController : AdminBaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
