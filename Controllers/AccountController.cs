using Bookstore.Models;
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bookstore.DTOs;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly object signInManager;

        public AccountController (IAuthService authService, IMailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync( RegisterDto registerRequest)
        {
            //if (!ModelState.IsValid)
            //    return View(registerRequest);
            var authResult = await _authService.RegisterAsync(registerRequest);

            //if (!authResult.Success)
            //{
            //    if (authResult.ErrorMessage == "It already exists an account with this email")
            //    {
            //        ModelState.AddModelError("Email", authResult.ErrorMessage);
            //    }

            //    ModelState.AddModelError(string.Empty, authResult.ErrorMessage);
            //    return View(registerRequest);
            //}
            if (!authResult.Success)
            {
                
                if (authResult.ErrorMessage == "Email already registered")
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, authResult.ErrorMessage);
                }

                return View(registerRequest);
            }

            var welcomeEmail = new MailMessage
            {
                Subject = "Welcome to Bookstore!",
                Body = $"""
                    Hello {registerRequest.FirstName},

                    Thank you for registering in the Bookstore App.
                    We’re glad to have you with us!

                    📚 Happy reading!
                    """,
                            IsBodyHtml = false,
                        };
            welcomeEmail.To.Add(registerRequest.Email);

            welcomeEmail.From = new MailAddress("MS_lorIzz@test-69oxl5eex2xl785k.mlsender.net", "Bookstore App");

            await _mailService.SendEmailAsync(welcomeEmail);

            return RedirectToAction("LogIn", "Account");

        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync( LogInDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(loginRequest); 
            }
                var authResult = await _authService.LoginAsync(loginRequest);

            if (authResult == null)
            {
                if (string.IsNullOrEmpty(loginRequest.Email))
                {
                    ModelState.AddModelError("Email", "Email is required.");
                }
                else if (string.IsNullOrEmpty(loginRequest.Password))
                {
                    ModelState.AddModelError("Password", "Password is required.");
                }
                else
                {
                    // Adaugă eroarea pentru email/parolă greșite
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
                return View(loginRequest);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authResult.UserId),
                new Claim(ClaimTypes.Name, authResult.UserName),
                new Claim(ClaimTypes.Role, authResult.UserRole)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
            //return Ok();
        }

        [HttpGet]
        public IActionResult SendResetCode()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendResetCode(SendResetCodeDto resetCode)
        {
            var user = await _authService.GenerateOneTimeCodeAsync(resetCode.Email);

            if (user == null)
            {
                //return NotFound("User doesn't exist");  
                ModelState.AddModelError(string.Empty, "User doesn't exist.");
                return View(resetCode);

            }
            var email = new MailMessage
            {
                Subject = "Code for reset password",
                Body = $"""
                Hello,

                Use this code to reset your password in Bookstore App: {user.CodeResetPassword}.

                Do not share it with anyone. It expires in 5 minutes.
                """,
                IsBodyHtml = false,
            };

            email.To.Add(resetCode.Email); // destinatarul

            email.From = new MailAddress("MS_lorIzz@test-69oxl5eex2xl785k.mlsender.net", "Bookstore App");

            await _mailService.SendEmailAsync(email);

            TempData["OpenModal"] = true; 
            TempData["Email"] = resetCode.Email;


            return View(resetCode);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var email = TempData["Email"]?.ToString();

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("SendResetCode");

            var user = await _authService.GetUserByEmailAsync(email);

            if (user == null || user.CodeResetPassword != code || user.TimeCodeExpires < DateTime.UtcNow)
            {
                TempData["Error"] = "Invalid or expired code. Please try again.";
                return RedirectToAction("SendResetCode");
            }

            TempData["Email"] = email; 
            return RedirectToAction("ChangePassword"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = TempData["Email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Session expired. Please restart the reset process.";
                return RedirectToAction("SendResetCode", "Account");
            }

            var result = await _authService.ChangePasswordAsync(email, model.Password);
            if (!result)
            {
                TempData["Error"] = "An error occurred while changing the password.";
                return RedirectToAction("SendResetCode", "Account");
            }

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
