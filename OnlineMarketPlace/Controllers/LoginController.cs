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
        HttpContext.Session.SetString("url", "Login/Login");
        return View();
    }

    [HttpGet]
    public IActionResult ForgetPass()
    {
        HttpContext.Session.SetString("url", "Login/ForgetPass");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CheckLogin(string username, string password)
    {
        var user = await _userRepository.GetUser(username, password);
        if (user == null)
        {
            TempData["Message"] = "Username or password is incorrect!";
            TempData["MessageType"] = "error";
            return View("Login");
        }
        else
        {
            HttpContext.Session.SetString("Id", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

    }

    [HttpGet]
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
            existUser = new User
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
            await _userRepository.AddAsync(existUser);
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", existUser.Role);
            return RedirectToAction("Index", "Home");
        }
        else if (existUser.LoginBy == false) //Email da duoc dang ky bang Register
        {
            TempData["Message"] = "Cannot login! Email is existed.";
            TempData["MessageType"] = "error";

            Console.WriteLine("ủl:" + HttpContext.Session.GetString("url"));
            var previousUrl = Request.Headers["Referer"].ToString().Trim() + HttpContext.Session.GetString("url");
            Console.WriteLine("pre" + previousUrl);
            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            return View("Login");
        }
        else
        {
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", existUser.Role);
            return RedirectToAction("Index", "Home"); //Email da duoc dky trong DB
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }


    //public async Task<IActionResult> SendMailForgetPass(string email)
    //{
    //    var checkMail = await _userRepository.GetUserByEmail(email);
    //    if (checkMail == null)
    //    {
    //        TempData["error"] = "Email not found";
    //        return RedirectToAction("ForgetPass", "Login");
    //    }
    //    else
    //    {
    //        string token = Guid.NewGuid().ToString();
    //        checkMail.Token = token;
    //        await _userRepository.UpdateUserAsync(checkMail);
    //        var receiver = checkMail.Email;
    //        var subject = "Change password for user " + checkMail.Email;
    //        var message = "Click on link to change password " + "<a href='" + $"{Request.Scheme}://{Request.Host}/Login/NewPass?email={checkMail.Email}&token={checkMail.Token}>";

    //        await _emailService.SendEmailAsync(receiver, subject, message);

    //        TempData["success"] = "An email has been sent to you to change password.";
    //        return RedirectToAction("ForgetPass", "Login");

    //    }

    //}

    [HttpPost]
    public async Task<IActionResult> SendMailForgetPass(string email)
    {
        var lastRequestTime = HttpContext.Request.Cookies["ResetPasswordTimestamp"];
        if (!string.IsNullOrEmpty(lastRequestTime))
        {
            var lastRequestDateTime = DateTime.Parse(lastRequestTime);
            var remainTime = (DateTime.UtcNow - lastRequestDateTime);

            if (remainTime.TotalMinutes < 1)
            {
                TempData["Message"] = $"Please wait {60 - (int)remainTime.TotalSeconds}s to send another password reset request.";
                TempData["MessageType"] = "error";
                Console.WriteLine("kkkkk");
                return RedirectToAction("ForgetPass", "Login");
            }
        }

        var checkMail = await _userRepository.GetUserByEmail(email);
        Console.WriteLine(checkMail);
        if (checkMail == null || checkMail.LoginBy == true)
        {
            TempData["Message"] = "This email address is not registered.";
            TempData["MessageType"] = "error";
            return RedirectToAction("ForgetPass", "Login");
        }

        string token = Guid.NewGuid().ToString();

        HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(5) 
        });

        // Luu thoi gian hien tai vao cookie
        HttpContext.Response.Cookies.Append("ResetPasswordTimestamp", DateTime.UtcNow.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(1) 
        });

        var receiver = email;
        var subject = "Change password for user " + email;
        var message = $"Click on the link to change password: {Request.Scheme}://{Request.Host}/Login/NewPass?email={email}&token={token}";

        await _emailService.SendEmailAsync(receiver, subject, message);

        TempData["Message"] = "An email has been sent to you to change password.";
        TempData["MessageType"] = "success";
        return RedirectToAction("ForgetPass", "Login");
    }


    [HttpGet]
    public async Task<IActionResult> NewPass(string email, string token)
    {
        var checkToken = HttpContext.Request.Cookies["Token"];
        if (string.IsNullOrEmpty(checkToken) || checkToken != token)
        {

            TempData["Message"] = "Token has expired or does not exist. Please resend your request.";
            TempData["MessageType"] = "error";
            return RedirectToAction("ForgetPass", "Login");
        }
        else
        {
            ViewBag.Email = email;
            return View();
        }

        //var checkuser = await _userRepository.GetUserByEmailAndToken(email, token);

        //if (checkuser != null)
        //{
        //    ViewBag.Email = checkuser.Email;
        //    ViewBag.Token = token;
        //    return View();
        //}

        //else
        //{
        //    TempData["error"] = "Email not found or token is not right";
        //    return RedirectToAction("ForgetPass", "Login");
        //}
    }

    [HttpPost]
    public async Task<IActionResult> ResetPass(string email, string password)
    {   
        var checkMail = await _userRepository.GetUserByEmail(email);
        if (checkMail == null)
        {

            TempData["Message"] = "Email not found!";
            TempData["MessageType"] = "error";
            return RedirectToAction("ForgetPass", "Login");
        }
        else
        {
            checkMail.Password = ncryptpasswordmd5.HashPasswordMD5(password);
            await _userRepository.UpdateUserAsync(checkMail);
            HttpContext.Response.Cookies.Delete("Token"); //Xoa cookie
            TempData["Message"] = "Reset password successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Login", "Login");
        }
    }


}
