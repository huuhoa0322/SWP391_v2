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

    public RegisterController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

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
}