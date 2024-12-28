using CanIEatIt.Models;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using System.Diagnostics;
using System.Text.Json.Nodes;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.Text.RegularExpressions;


namespace CanIEatIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            if (TempData["SuccessMessage"] == null)
            {
                TempData["SuccessMessage"] = "false";
            }
            var contactModel = new ContactViewModel
            {
                EmailBody = "",
                EmailFirstName = "",
                EmailLastName = "",
                EmailSender = "",
                EmailSubject = "",
                success = TempData["SuccessMessage"]!.ToString()
            };

            return View(contactModel);
        }

        public async Task<IActionResult> Email(string emailFirstName, string emailLastName, string emailSender, string emailSubject, string emailBody)
        {
            /// Input validation

            var contactModel = new ContactViewModel
            {
                EmailBody = "",
                EmailFirstName = "",
                EmailLastName = "",
                EmailSender = "",
                EmailSubject = "",
                success = "false"
            };

            if (string.IsNullOrEmpty(emailFirstName))
            {
                TempData["SuccessMessage"] = "firstname";
                return RedirectToAction("Contact");
            }

            if (string.IsNullOrEmpty(emailLastName))
            {
                TempData["SuccessMessage"] = "lastname";
                return RedirectToAction("Contact");
            }

            if (string.IsNullOrEmpty(emailSender))
            {
                TempData["SuccessMessage"] = "sender";
                return RedirectToAction("Contact");
            }

            if (string.IsNullOrEmpty(emailSubject))
            {
                TempData["SuccessMessage"] = "subject";
                return RedirectToAction("Contact");
            }

            if (string.IsNullOrEmpty(emailBody))
            {
                TempData["SuccessMessage"] = "body";
                return RedirectToAction("Contact");
            }

            if(!Regex.IsMatch(emailSender, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                TempData["SuccessMessage"] = "invalidemail";
                return RedirectToAction("Contact");
            }


            /// Set-up email
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailFirstName + " " + emailLastName, emailSender));
            email.To.Add(new MailboxAddress("CanIEatIt", "qavanheppenstall@gmail.com"));
            email.Subject = "Contact Form: " + emailSubject;
            email.Body = new TextPart(emailBody);

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("6b32efba95d91d", "9b7e9393e5acc8");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }

            TempData["SuccessMessage"] = "success";
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}