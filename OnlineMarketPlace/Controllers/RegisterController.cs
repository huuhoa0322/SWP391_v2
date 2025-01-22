using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Controllers;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class RegisterController : Controller
{

    private readonly ILogger<LoginController> _logger;

    private readonly UserRepository _userRepository = new();
    private const string CaptchaSessionKey = "CaptchaCode";

    public RegisterController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {

        var errors = new List<string>();

        // Kiểm tra tên đăng nhập

        var checkUserNameExist = await _userRepository.checkUserName(user.Username);
        if (checkUserNameExist != null)
        {
            // Nếu tài khoản tồn tại nhưng IsDeleted = false, báo lỗi
            if (checkUserNameExist.IsDeleted == false)
            {
                errors.Add("Username already exists. Please choose another username.");
            }
        }

        // Kiểm tra email
        var checkEmailExist = await _userRepository.checkEmail(user.Email);
        if (checkEmailExist != null)
        {
            // Nếu tài khoản tồn tại nhưng IsDeleted = false, báo lỗi
            if (checkEmailExist.IsDeleted == false)
            {
                errors.Add("Email already exists. Please use another email.");
            }
        }


        // Kiểm tra ngày sinh
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
            //Ma hoa mk
            user.Password = ncryptpasswordmd5.HashPasswordMD5(user.Password);
            // Thêm dữ liệu vào cơ sở dữ liệu một cách bất đồng bộ

            await _userRepository.AddAsync(user);
            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập!";
            return RedirectToAction("Login", "Login"); // Chuyển hướng sau khi đăng ký thành công
        }
        else
        {

            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View("Register");
        }

    }
    // API kiểm tra username
    [HttpGet]
    public async Task<IActionResult> CheckUsername(string username)
    {
        var exists = await _userRepository.checkUserName(username);
        if (exists != null && !exists.IsDeleted)
        {
            return Json(new { isValid = false, message = "Username already exists. Please choose another." });
        }

        return Json(new { isValid = true });
    }
    // API kiểm tra email
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

    
    // API kiểm tra ngày sinh
    
    [HttpGet]
    public IActionResult ValidateDob(DateTime dob)
    {
        if (dob > DateTime.Now)
        {
            return Json(new { isValid = false, message = "Date of birth cannot be in the future." });
        }

        return Json(new { isValid = true });
    }
    [HttpGet]
    public IActionResult GenerateCaptcha()
    {
        string captchaCode = GenerateRandomCode(5);
        HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);

        using var bitmap = new Bitmap(150, 50);
        using var graphics = Graphics.FromImage(bitmap);
        var font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold, GraphicsUnit.Pixel);
        var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

        // Vẽ nền
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.FillRectangle(Brushes.LightGray, rectangle);

        // Vẽ chữ CAPTCHA
        var brush = new SolidBrush(Color.Black);
        graphics.DrawString(captchaCode, font, brush, 10, 10);

        // Thêm nhiễu
        var rand = new Random();
        for (int i = 0; i < 20; i++)
        {
            int x = rand.Next(0, bitmap.Width);
            int y = rand.Next(0, bitmap.Height);
            bitmap.SetPixel(x, y, Color.Black);
        }

        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return File(stream.ToArray(), "image/png");
    }

    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}