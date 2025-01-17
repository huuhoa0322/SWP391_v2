using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Controllers;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly UserRepository _userRepository = new();

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CheckLogin(string username, string password)
    {

        var user = _userRepository.GetUser(username, password);
        if (user == null)
        {
            ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng.";
            return View("Login");
        }
        else
        {
            //HttpContext.Session.SetString("Username", user.Username);
            //HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

    }
}
