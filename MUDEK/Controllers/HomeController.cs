using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mudek.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mudek.Extensions;


namespace Mudek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger
            , ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name == "Admin Admin")
            {
                return Redirect("/Mail/Index");
            }
            else if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Teacher");
            }
            return View();
        }


        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(Teacher teacher, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;





            var credentials = _context.Teachers.Where(x => x.Mail == teacher.Mail && x.Password == teacher.Password).SingleOrDefault();
            if (credentials is not null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", (credentials.Id).ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, credentials.Mail));
                claims.Add(new Claim(ClaimTypes.Name, credentials.FirstName + " "+ credentials.LastName));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (teacher.Mail == "admin" && teacher.Password == "admin")
                {
                    return Redirect("/Mail/Index");
                }
                else
                {
                    return RedirectToAction("Index", "Teacher");
                }
            }
            TempData["Error"] = "Error. Username or Password is invalid";
            return View("login");
        }

        

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
