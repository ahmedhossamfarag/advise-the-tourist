using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace AdviseTheTourist.Controllers
{
    [Authorize(Policy = "IsLogIn")]
    public class PlacesController : Controller
    {
        public static string PlacesPhotosPath = "Images/PlacesPhotos/";
        private readonly DatabaseContext _context;
        private IWebHostEnvironment _environment;
        public PlacesController(DatabaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var adminPlaces = from p in _context.Place
                              where p.AdminEmail == email
                              select p;
            return View(await adminPlaces.ToListAsync());
        }



        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place
                .FirstOrDefaultAsync(m => m.Name == id);
            if (place == null)
            {
                return NotFound();
            }
            PlaceModel model = new PlaceModel() { Place = place };
            var name = place.Name;
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddHotelFacility([Bind("HotelName,Name,Description")]HotelFacility facility)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Details), new {id = facility.HotelName });
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteHotelFacility([Bind("HotelName,Name")] HotelFacility keys)
        {
            var facility = await _context.HotelFacility.FindAsync(keys.HotelName, keys.Name);
            if (facility != null)
            {
                _context.HotelFacility.Remove(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = facility.HotelName });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddHotelRoom([Bind("HotelName,Type,Price")]HotelRoom room)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
            }catch (Exception) { }
            return RedirectToAction(nameof(Details), new { id = room.HotelName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHotelRoom([Bind("HotelName,Type")] HotelRoom keys)
        {
            var room = await _context.HotelRoom.FindAsync(keys.HotelName, keys.Type);
            if (room != null)
            {
                _context.HotelRoom.Remove(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = room.HotelName });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMuseumMonument([Bind("MuseumName,Name,Description")]MuseumMonument monument)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                _context.Add(monument);
                await _context.SaveChangesAsync();
            }
            catch(Exception) { }
            return RedirectToAction(nameof(Details), new { id = monument.MuseumName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMuseumMonument([Bind("MuseumName,Name")] MuseumMonument keys)
        {
            var monument = await _context.MuseumMonument.FindAsync(keys.MuseumName,keys.Name);
            if (monument != null)
            {
                _context.MuseumMonument.Remove(monument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = monument.MuseumName });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMuseumTicket([Bind("MuseumName,Type,Price")]MuseumTicket ticket)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Details), new { id = ticket.MuseumName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMuseumTicket([Bind("MuseumName,Type")] MuseumTicket keys)
        {
            var ticket = await _context.MuseumTicket.FindAsync(keys.MuseumName,keys.Type);
            if (ticket != null)
            {
                _context.MuseumTicket.Remove(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = ticket.MuseumName });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto([Bind("PlaceName,PhotoFile")] NewPhotoModel photoModel)
        {
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (photoModel.PhotoFile?.Length > 0)
            {
                var filename = PlacesPhotosPath + Path.GetFileName(photoModel.PhotoFile.FileName);

                var filePath = Path.Combine(_environment.WebRootPath,
                        filename);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await photoModel.PhotoFile.CopyToAsync(stream);
                }
                photoModel.Photo = $"~/{filename}";
                photoModel.AdminEmail = email;
                try
                {
                    _context.Add((PlacePhoto)photoModel);
                    await _context.SaveChangesAsync();
                }
                catch { }
                return RedirectToAction(nameof(Details), new { id = photoModel.PlaceName });
            }
            ModelState.AddModelError(nameof(photoModel.PhotoFile), "File Not Valid");
            return RedirectToAction(nameof(Details), new { id = photoModel.PlaceName });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto([Bind("AdminEmail,PlaceName,Photo")] PlacePhoto keys)
        {
            var photo = await _context.PlacePhoto.FindAsync(keys.AdminEmail, keys.PlaceName,keys.Photo);
            if (photo != null)
            {
                _context.PlacePhoto.Remove(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = photo.PlaceName });
            }

            return NotFound();
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteComment([Bind("MemberEmail,PlaceName,Content")] Comment keys)
        {
            var comment = await _context.Comment.FindAsync(keys.MemberEmail, keys.PlaceName, keys.Content);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = comment.PlaceName });
            }

            return NotFound();
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteHashtag([Bind("MemberEmail,PlaceName,Content")] Hashtag keys)
        {
            var hashtag = await _context.Hashtag.FindAsync(keys.MemberEmail,keys.PlaceName,keys.Content);
            if (hashtag != null)
            {
                _context.Hashtag.Remove(hashtag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id = hashtag.PlaceName});
            }
            return NotFound();
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteRatingCriteria([Bind("Name,PlaceName")] RatingCriteria keys)
        {
            var criteria = await _context.RatingCriteria.FindAsync(keys.Name, keys.PlaceName);
            if (criteria != null)
            {
                await _context.Database.OpenConnectionAsync();
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "DeleteRatingCriteria";
                cmd.Parameters.Add(new SqlParameter("@name", criteria.Name));
                cmd.Parameters.Add(new SqlParameter("@place", criteria.PlaceName));
                await cmd.ExecuteNonQueryAsync();
                return RedirectToAction(nameof(Details), new { id = criteria.PlaceName });
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestionAnswer([Bind("Content,MemberEmail,PlaceName,Answer")] Question question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Details), new { id = question.PlaceName });
            }
            return RedirectToAction(nameof(Details), new {id = question.PlaceName});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion([Bind("Content,MemberEmail,PlaceName")] Question keys)
        {
            var question = await _context.Question.FindAsync(keys.MemberEmail, keys.PlaceName, keys.Content);
            if (question != null)
            {
                _context.Question.Remove(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = question.PlaceName });
            }
            return NotFound();
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Buildingdate,MapLongitude,MapLatitude,Type,Location,Coastal,Cuisine,Style,OpenTime,CloseTime,ImageFile")] NewPlaceModel place)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue("Email");
                if(email == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                place.AdminEmail = email;
                if(PlaceExists(place.Name))
                {
                    ModelState.AddModelError(nameof(place.Name), "Place name already exists");
                    return View(place);
                }
                if (place.ImageFile?.Length > 0)
                {
                    var filename = PlacesPhotosPath + Path.GetFileName(place.ImageFile.FileName);

                    var filePath = Path.Combine(_environment.WebRootPath,
                            filename);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await place.ImageFile.CopyToAsync(stream);
                    }
                    place.Image = $"~/{filename}";
                    _context.Add((Place)place);
                    await _context.SaveChangesAsync();
                    switch ((PlaceType)place.Type)
                    {
                        case PlaceType.City:
                            _context.Add(new City { Name = place.Name, Coastal = place.Coastal, Location = place.Location }); ;
                            break;
                        case PlaceType.Hotel:
                            _context.Add(new Hotel { Name = place.Name });
                            break;
                        case PlaceType.Museum:
                            _context.Add(new Museum { Name = place.Name, CloseTime = place.CloseTime, OpenTime = place.OpenTime });
                            break;
                        case PlaceType.Restaurant:
                            _context.Add(new Restaurant { Name = place.Name, Cuisine = place.Cuisine, Style = place.Style });
                            break;
                        default:
                            break;
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(place.ImageFile), "File Not Valid");
                
            }
            return View(place);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            var model = new NewPlaceModel 
            { 
                Name = place.Name, AdminEmail = place.AdminEmail, Buildingdate = place.Buildingdate,
                MapLongitude = place.MapLongitude, MapLatitude = place.MapLatitude, Image = place.Image,
            };
            switch ((PlaceType)place.Type)
            {
                case PlaceType.City:
                    var city = await _context.City.FirstAsync(c => c.Name == place.Name);
                    model.Location = city.Location;
                    model.Coastal = city.Coastal;
                    break;
                case PlaceType.Museum:
                    var museum = await _context.Museum.FirstAsync(m => m.Name == place.Name);
                    model.OpenTime = museum.OpenTime;
                    model.CloseTime = museum.CloseTime;
                    break;
                case PlaceType.Restaurant:
                    var restaurant = await _context.Restaurant.FirstAsync(r => r.Name == place.Name);
                    model.Cuisine = restaurant.Cuisine;
                    model.Style = restaurant.Style;
                    break;
                default:
                    break;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Buildingdate,MapLongitude,MapLatitude,AdminEmail,Type,Image,Location,Coastal,Cuisine,Style,OpenTime,CloseTime,ImageFile")] NewPlaceModel place)
        {
            if (id != place.Name)
            {
                return NotFound();
            }
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            place.AdminEmail = email;
            if (ModelState.IsValid)
            {
                try
                {
                    if (place.ImageFile?.Length > 0)
                    {
                        var filename = PlacesPhotosPath + Path.GetFileName(place.ImageFile.FileName);

                        var filePath = Path.Combine(_environment.WebRootPath,
                                filename);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await place.ImageFile.CopyToAsync(stream);
                        }
                        place.Image = $"~/{filename}";
                    }
                    _context.Update((Place)place);
                    switch ((PlaceType)place.Type)
                    {
                        case PlaceType.City:
                            _context.Update(new City { Name = place.Name, Coastal = place.Coastal, Location = place.Location }); ;
                            break;
                        case PlaceType.Museum:
                            _context.Update(new Museum { Name = place.Name, CloseTime = place.CloseTime, OpenTime = place.OpenTime });
                            break;
                        case PlaceType.Restaurant:
                            _context.Update(new Restaurant { Name = place.Name, Cuisine = place.Cuisine, Style = place.Style });
                            break;
                        default:
                            break;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(place);
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place
                .FirstOrDefaultAsync(m => m.Name == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _context.Database.OpenConnectionAsync();
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "DeletePlace";
            cmd.Parameters.Add(new SqlParameter("@name", id));
            await cmd.ExecuteNonQueryAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool PlaceExists(string id)
        {
            return _context.Place.Any(e => e.Name == id);
        }
    }
}
