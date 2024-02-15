using AdviseTheTourist.Models;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdviseTheTourist.Controllers
{
    public class LoginController : Controller
    {
        public static string AuthenticationName = "TouristAuth";
        private readonly DatabaseContext _context;


        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["LoginState"] = LoginState.INITIAL;
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Email", "Password")] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var member = await _context.Member.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (member == null)
            {
                ModelState.AddModelError(nameof(model.Email),"Email Not Found");
                return View(model);
            }
            else
            {
                if (member.Password != model.Password)
                {
                    ModelState.AddModelError(nameof(model.Password), "Password Not Correct");
                    return View(model);
                }
                else
                {
                    Claim[] claims =
                    {
                        new Claim("Email", model.Email),
                    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationName);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true,
                    };
                    await HttpContext.SignInAsync(AuthenticationName, principal, authenticationProperties);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
        }
    }
}
