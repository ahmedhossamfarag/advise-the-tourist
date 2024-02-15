using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdviseTheTourist.Controllers
{
    public class PlaceViewController : Controller
    {
        private DatabaseContext _context;

        public PlaceViewController(DatabaseContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            var place = await _context.Place.FirstOrDefaultAsync(x => x.Name == name);
            if (place == null)
            {
                return NotFound();
            }
            PlaceViewModel model = new PlaceViewModel() { Place = place };
            switch ((PlaceType)place.Type)
            {
                case PlaceType.Hotel:
                    model.Hotel = await _context.Hotel.FirstOrDefaultAsync(h => h.Name == name);
                    model.HotelRooms = await _context.HotelRoom.Where(h => h.HotelName == name).ToListAsync();
                    model.HotelFacilities = await _context.HotelFacility.Where(h => h.HotelName == name).ToListAsync();
                    break;
                case PlaceType.City:
                    model.City = await _context.City.FirstOrDefaultAsync(c => c.Name == name);
                    break;
                case PlaceType.Museum:
                    model.Museum = await _context.Museum.FirstOrDefaultAsync(m => m.Name == name);
                    model.MuseumMonuments = await _context.MuseumMonument.Where(m => m.MuseumName == name).ToListAsync();
                    model.MuseumTickets = await _context.MuseumTicket.Where(m => m.MuseumName == name).ToListAsync();
                    break;
                case PlaceType.Restaurant:
                    model.Restaurant = await _context.Restaurant.FirstOrDefaultAsync(r => r.Name == name);
                    break;
                default: break;
            }
            model.Comments = await _context.Comment.Where(c => c.PlaceName == name).ToListAsync();
            model.Hashtags = await _context.Hashtag.Where(h => h.PlaceName == name).ToListAsync();
            model.Questions = await _context.Question.Where(q => q.PlaceName == name).ToListAsync();
            model.RatingCriterias = await _context.RatingCriteria.Where(r => r.PlaceName == name).ToListAsync();
            model.Photos = await _context.PlacePhoto.Where(p => p.PlaceName == name).ToListAsync();
            var visits = from v in _context.Visit
                         where v.PlaceName == name
                         select v;
            model.VisitsNo = await visits.CountAsync();
            var likes = visits.Where(v => v.Liked);
            model.LikesNo = await likes.CountAsync();
            var email = User.FindFirstValue("Email");
            if (email != null)
            {
                model.Visit = await visits.FirstOrDefaultAsync(v => v.MemberEmail == email);
            }
            var criterias = from c in _context.RatingCriteria
                            where c.PlaceName == name
                            select c;
            var allratings = 
                            from c in criterias
                            join r in _context.Rating on new { PN = c.PlaceName, CN = c.Name } equals new { PN = r.PlaceName, CN = r.CriteriaName } into rr
                            from re in rr.DefaultIfEmpty()
                            select new
                            {
                              CriteriaName = c.Name,
                              Value = re.Value ?? 0,
                              Email = re.MemberEmail
                            };
            var ratings = from r in allratings
                          group r by r.CriteriaName into g
                          select new RatingModel
                          {
                              CriteriaName = g.Key,
                              Value = g.Average(v => v.Value),
                              Active = !g.Any(e => e.Email == email)
                          };
            model.RatingModels = await ratings.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmVisit(string id)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
               _context.Add(new Visit { MemberEmail = email, PlaceName = id });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index), new { name = id });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmLike(string id)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
               _context.Update(new Visit { MemberEmail = email, PlaceName = id, Liked = true });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index), new { name = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([Bind("Content,PlaceName")]Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            comment.MemberEmail = email;
            try
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Index), new { name = comment.PlaceName });
        }
        
        [HttpPost]
        public async Task<IActionResult> AddHashtag([Bind("Content,PlaceName")]Hashtag hashtag)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            hashtag.MemberEmail = email;
            try
            {
                _context.Add(hashtag);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Index), new { name = hashtag.PlaceName });
        }
        
        [HttpPost]
        public async Task<IActionResult> AddQuestion([Bind("Content,PlaceName")]Question question)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            question.MemberEmail = email;
            try
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
            }catch { }
            return RedirectToAction(nameof(Index), new { name = question.PlaceName });
        }

        [HttpPost]
        public async Task<IActionResult> PostRating([Bind("PlaceName,CriteriaName,Value")]Rating rating)
        {
            if (rating.Value < 0 || rating.Value > 10)
            {
                return BadRequest();
            }
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            rating.MemberEmail = email;
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Index), new { name = rating.PlaceName });
        }

        [HttpPost]
        public async Task<IActionResult> AddRatingCriteria([Bind("PlaceName,Name")]RatingCriteria criteria)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            criteria.MemberEmail = email;
            try
            {
                _context.Add(criteria);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Index), new { name = criteria.PlaceName });
        }

    }
}
