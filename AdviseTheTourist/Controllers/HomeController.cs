using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdviseTheTourist.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;


        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<JsonResult> SearchPlaces(string name)
        {
            var places = from c in _context.Place
                            where c.Name.Contains(name)
                            select c.Name;
            return Json((await places.ToListAsync()).Take(10));
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
