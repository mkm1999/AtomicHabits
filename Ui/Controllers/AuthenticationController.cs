using Application.UserService.Command.Register;
using Application.UserService.Query.ILoginService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ui.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly ILogin _login;
        public AuthenticationController(IRegisterUserService registerUserService , ILogin login)
        {
            _registerUserService = registerUserService;
            _login = login;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register([FromBody]RequestRegisterUserDto request)
        {
            var result = _registerUserService.Execute(request);
            if (!result.IsSuccess)
            {
                return Json(result);
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,result.Data.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, "User"));
            claims.Add(new Claim(ClaimTypes.Name,request.UserName));
            var ClaimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var Properties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5),
            };
            var Principal = new ClaimsPrincipal(ClaimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,Principal,Properties);
            return Json(result);
        }
        [HttpGet]
        public IActionResult SignIn()
		{
            return View();
		}
        [HttpPost]
        public IActionResult SignIn([FromBody]RequestLoginDto request)
		{
            var result = _login.Execute(request);
			if (result.IsSuccess)
			{
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, result.Data.role));
                claims.Add(new Claim(ClaimTypes.Name, request.UserName));
                var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var Properties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5),
                };
                var Principal = new ClaimsPrincipal(ClaimsIdentity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal, Properties);
                return Json(result);
            }
            return Json(result);
		}

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
    }
}
