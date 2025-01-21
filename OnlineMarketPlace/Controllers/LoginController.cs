using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Controllers;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly IEmailService _emailService;

    private readonly UserRepository _userRepository = new();

    public LoginController(ILogger<LoginController> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult ForgetPass()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> NewPass(string email, string token)
    {

        var checkuser = await _userRepository.GetUserByEmailAndToken(email, token);

        if (checkuser != null)
        {
            ViewBag.Email = checkuser.Email;
            ViewBag.Token = token;
            return View();
        }

        else
        {
            TempData["error"] = "Email not found or token is not right";
            return RedirectToAction("ForgetPass", "Login");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CheckLogin(string username, string password)
    {

        var user = await _userRepository.GetUser(username, password);
        if (user == null)
        {
            ViewBag.ErrorMessage = "Username or password is incorrect!";
            return View("Login");
        }
        else
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

    }

    public async Task LoginByGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
    }

    public async Task<IActionResult> GoogleResponse()
    {
        //var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            //Neu xác thuc ko thanh cong quay ve trang Login
            return RedirectToAction("Login");
        }
        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        //return Json(claims);
        string username = email.Split('@')[0];
        var existUser = await _userRepository.GetUserByEmail(email);
        if (existUser == null) //Email chua duoc dky trong DB
        {
            var u = new User
            {
                Username = username,
                Email = email,
                Password = ncryptpasswordmd5.HashPasswordMD5("123456789"),
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Unknown",
                Gender = false,
                Dob = DateTime.Parse("01/01/2000"),
                IsDeleted = false,
                LoginBy = true
            };
            await _userRepository.AddAsync(u);
            return RedirectToAction("Index", "Home");
        }
        else if (existUser.LoginBy == false) //Email da duoc dang ky bang Register
        {
            ViewBag.ErrorMessage = "Email is existed";
            return View("Login");
        }
        else return RedirectToAction("Index", "Home"); //Email da duoc dky trong DB
    }

    public async Task<IActionResult> SendMailForgetPass(string email)
    {
        var checkMail = await _userRepository.GetUserByEmail(email);
        if (checkMail == null)
        {
            TempData["error"] = "Email not found";
            return RedirectToAction("ForgetPass", "Login");
        }
        else
        {
            string token = Guid.NewGuid().ToString();
            checkMail.Token = token;
            await _userRepository.UpdateUserAsync(checkMail);
            var receiver = checkMail.Email;
            var subject = "Change password for user " + checkMail.Email;
            var message = "Click on link to change password " + "<a href='" + $"{Request.Scheme}://{Request.Host}/Login/NewPass?email={checkMail.Email}&token={checkMail.Token}>";

            await _emailService.SendEmailAsync(receiver, subject, message);

            TempData["success"] = "An email has been sent to you to change password.";
            return RedirectToAction("ForgetPass", "Login");

        }

    }

    public async Task<IActionResult> ResetPass(string email, string pass)
    {   
        var checkMail = await _userRepository.GetUserByEmail(email);
        if (checkMail == null)
        {
            TempData["error"] = "Email not found";
            return RedirectToAction("ForgetPass", "Login");
        }
        else
        {
            checkMail.Password = ncryptpasswordmd5.HashPasswordMD5(pass);
            await _userRepository.UpdateUserAsync(checkMail);
            TempData["success"] = "Login successfully";
            return RedirectToAction("Index", "Home");
        }
    }


}
