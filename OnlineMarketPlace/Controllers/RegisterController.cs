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

    private readonly ILogger<HomeController> _logger;
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    private OnlineShoppingContext _context;

    public RegisterController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            // Lưu thông tin người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);         
            // Sử dụng TempData để truyền thông báo
            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập!";

            // Chuyển hướng đến trang login
            return RedirectToAction("Index", "Home"); // Thay "Account" bằng controller trang login của bạn
        }
        else
        {
            var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            ViewBag.Errors = errors; // Gửi danh sách lỗi đến View
            TempData["SuccessMessage"] = "Đăng ký k thành công. Vui lòng đăng nhập!";

            // Trả về lại form đăng ký với thông báo lỗi
            return View("Index");
        }
    }
}
