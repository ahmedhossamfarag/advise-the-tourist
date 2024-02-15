using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdviseTheTourist.Controllers
{
    public class ExploreController : Controller
    {
        private DatabaseContext _context;

        public ExploreController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            if(id > (int)PlaceType.Other)
            {
                return NotFound();
            }
            var places = from p in _context.Place
                         where p.Type == id
                         select p;
            return View(await places.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            if(string.IsNullOrEmpty(searchText))
            {
                return RedirectToAction("Index", "Home");
            }
            var result = from p in _context.Place
                         where p.Name.Contains(searchText)
                         select p;
            return View("Index", await result.ToListAsync());
        }

        public async Task<IActionResult> RatingOf(string id, int type)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var ratings = from r in _context.Rating
                         where r.CriteriaName == id
                         group r by r.PlaceName into g
                         select new
                         {
                             Name = g.Key,
                             Rating = g.Average(k => k.Value ?? 0),
                         };

            var places = from r in ratings
                         join p in _context.Place on r.Name equals p.Name
                         where p.Type == type
                         select new RatingCRModel(
                             r.Name, p.Image, r.Rating
                             );
            return View((await places.ToListAsync()).OrderBy(p => p.Rating));        
        }
    }
}
