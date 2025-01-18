using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Models;

public class EmailController : Controller
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> SendEmail(String email)
    {
        await _emailService.SendEmailAsync(email, "Test Subject", "Test Body");
        //return Ok("Email Sent");
        ViewData["mailmesage"] = "mail sent complete";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SendEmail(String email, String subject, String Body)
    {
        await _emailService.SendEmailAsync(email, subject, Body);
        //return Ok("Email Sent");
        ViewData["mailmesage"] = "mail sent complete";
        return RedirectToAction("Index", "Home");
    }


}
