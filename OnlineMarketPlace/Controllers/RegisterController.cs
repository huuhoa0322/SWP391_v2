using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RegisterController : Controller
{
    private readonly ILogger<RegisterController> _logger;

    private readonly UserRepository _userRepository = new();
    private const string CaptchaSessionKey = "CaptchaCode";

    public RegisterController(ILogger<RegisterController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public IActionResult Register()
    {
        // Generate a new CAPTCHA when the Register page is loaded
        string captchaCode = GenerateRandomCode(5);
        HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user, string captcha)
    {
        var errors = new List<string>();

        // Verify CAPTCHA
        var sessionCaptcha = HttpContext.Session.GetString(CaptchaSessionKey);
        if (string.IsNullOrEmpty(sessionCaptcha) || !sessionCaptcha.Equals(captcha, StringComparison.OrdinalIgnoreCase))
        {
            errors.Add("Captcha is incorrect. Please try again.");
        }

        // Validate username
        var checkUserNameExist = await _userRepository.checkUserName(user.Username);
        if (checkUserNameExist != null && !checkUserNameExist.IsDeleted)
        {
            errors.Add("Username already exists. Please choose another username.");
        }

        // Validate email
        var checkEmailExist = await _userRepository.checkEmail(user.Email);
        if (checkEmailExist != null && !checkEmailExist.IsDeleted)
        {
            errors.Add("Email already exists. Please use another email.");
        }

        // Validate date of birth
        if (user.Dob > DateTime.Now)
        {
            errors.Add("Date of birth cannot be in the future.");
        }

        if (errors.Any())
        {
            ViewBag.Errors = errors;
            return View("Register");
        }

        if (ModelState.IsValid)
        {
            // Encrypt the password
            user.Password = ncryptpasswordmd5.HashPasswordMD5(user.Password);

            // Add the user to the database asynchronously
            await _userRepository.AddAsync(user);

            TempData["Message"] = "Registration successful. Please log in!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Login", "Login");
        }

        ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        return View("Register");
    }

    [HttpGet]
    public async Task<IActionResult> CheckUsername(string username)
    {
        if (username.Length < 8)
        {
            return Json(new { isValid = false, message = "Username need length more than 8 and contain at least one uppercase letter and one lowercase letter" });
        }
        else
        {
            if (!username.Any(char.IsUpper) || !username.Any(char.IsLower))
            {
                return Json(new { isValid = false, message = "Username must contain at least one uppercase letter and one lowercase letter." });
            }
            var exists = await _userRepository.checkUserName(username);
            if (exists != null && !exists.IsDeleted)
            {
                return Json(new { isValid = false, message = "Username already exists. Please choose another." });
            }
        }
        return Json(new { isValid = true });
    }
    [HttpGet]
    public async Task<IActionResult> CheckCaptcha(string captcha)
    {
        Console.WriteLine(captcha);
        var sessionCaptcha = HttpContext.Session.GetString(CaptchaSessionKey);
        if (string.IsNullOrEmpty(sessionCaptcha) || !sessionCaptcha.Equals(captcha))
        {
            return Json(new { isValid = false, message = "Captcha is incorrect. Please try again." });
        }

        return Json(new { isValid = true });
    }
    [HttpGet]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var exists = await _userRepository.checkEmail(email);
        if (exists != null && !exists.IsDeleted)
        {
            return Json(new { isValid = false, message = "Email already exists. Please use another email." });
        }

        return Json(new { isValid = true });
    }

    [HttpGet]
    public IActionResult ValidateDob(DateTime dob)
    {
        if (dob > DateTime.Now || dob.Year<2024)
        {
            return Json(new { isValid = false, message = "Date of birth Invald." });
        }

        return Json(new { isValid = true });
    }

    [HttpGet]
    public async Task<IActionResult> CheckPassword(string password)
    {
        Console.WriteLine(password);
        if (password.Length < 8 )
        {
            return Json(new { isValid = false, message = "Passwords need to length more than 8 and contain at least one uppercase letter, one lowercase letter, and one digit." });
        }
        if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
        {
            return Json(new { isValid = false, message = "Password must contain at least one uppercase letter, one lowercase letter, and one digit." });
        }
        return Json(new { isValid = true });
    }
    [HttpGet]
    public async Task<IActionResult> CheckconfirmPassword(string password, string confirmPassword)
    {
        Console.WriteLine(password);
        Console.WriteLine(confirmPassword);
        if (confirmPassword != password)
        {
            return Json(new { isValid = false, message = "Passwords do not match." });
        }

        return Json(new { isValid = true });
    }
    [HttpGet]
    public IActionResult GenerateCaptcha()
    {
        try
        {
            string captchaCode = GenerateRandomCode(5);
            HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);

            using var bitmap = new Bitmap(150, 50);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw background
            graphics.Clear(Color.LightGray);

            // Draw CAPTCHA text
            var font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold, GraphicsUnit.Pixel);
            var brush = new SolidBrush(Color.Black);
            graphics.DrawString(captchaCode, font, brush, 10, 10);

            // Add noise
            var rand = new Random();
            var pen = new Pen(Color.Gray);
            for (int i = 0; i < 10; i++)
            {
                int x1 = rand.Next(0, bitmap.Width);
                int y1 = rand.Next(0, bitmap.Height);
                int x2 = rand.Next(0, bitmap.Width);
                int y2 = rand.Next(0, bitmap.Height);
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }

            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/png");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating CAPTCHA.");
            return StatusCode(500, "An error occurred while generating CAPTCHA.");
        }
    }

    private string GenerateRandomCode(int length)
    {
        //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
