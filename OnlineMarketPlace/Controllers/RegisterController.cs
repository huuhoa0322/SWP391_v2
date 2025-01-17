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
        if (ModelState.IsValid)
        {
            // Thêm dữ liệu vào cơ sở dữ liệu một cách bất đồng bộ
            await _userRepository.AddAsync(user);
            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập!";
            return RedirectToAction("Index", "Home"); // Chuyển hướng sau khi đăng ký thành công
        }
        return View(user);
    }

}