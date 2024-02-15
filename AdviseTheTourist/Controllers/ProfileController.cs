using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AdviseTheTourist.Controllers
{
    [Authorize(Policy = "IsLogIn")]
    public class ProfileController : Controller
    {
        private DatabaseContext _context;
        public ProfileController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var profile = await CreateProfile(email);

            return View(profile);
        }


        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            if(email.IsNullOrEmpty())
            {
                return NotFound();
            }
            var userEmail = User.FindFirstValue("Email");
            if (userEmail == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var friend = await _context.Friend.FirstOrDefaultAsync(f => 
                (f.MemberEmail == email && f.Member2Email == userEmail) ||
                (f.MemberEmail == userEmail && f.Member2Email == email)
            );
            if (friend == null)
            {
                return NotFound();
            }
            var profile = await CreateProfile(email);
            return View(profile);
        }



        private async Task<ProfileModel> CreateProfile(string email)
        {
            return new ProfileModel()
            { 
                IsActive = email == User.FindFirstValue("Email"),
                Member = await _context.Member.FirstOrDefaultAsync(x => x.Email == email),
                Friends = await ReadFrinds(email),
                Visits = await
                    _context.Visit.Where(x => x.MemberEmail == email)
                    .Join(_context.Place, v => v.PlaceName, p => p.Name, (v, p) =>
                    new VisitModel
                    {
                        Name = v.PlaceName,
                        Liked = v.Liked,
                        Photo = p.Image,
                    })
                    .ToListAsync(),

                MemberAddresses = await _context.MemberAddress.Where(a => a.MemberEmail == email).ToListAsync(),
                MemberPhoneNumbers = await _context.MemberPhoneNo.Where(p => p.MemberEmail == email).ToListAsync(),
                AdminPlaces = await _context.Place.Where(p => p.AdminEmail == email).ToListAsync(),
            };
        }

        private async Task<List<FriendModel>> ReadFrinds(string email)
        {
            var emails = _context.Friend
                        .Where(f => f.MemberEmail == email)
                        .Select(f => f.Member2Email)
                        .Union(_context.Friend
                            .Where(f => f.Member2Email == email)
                            .Select(f => f.MemberEmail)
                         );
            var members = _context.Member.Join(
                      emails,
                      m => m.Email,
                      e => e,
                      (m, e) => new FriendModel
                      {
                          Email = m.Email,
                          Firstname = m.Firstname,
                          Lastname = m.Lastname,
                          Image = m.Image,
                      }
                  );

            return await members.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> NewAddress([Bind("Address")] MemberAddress memberAddress)
        {
            memberAddress.MemberEmail = User.FindFirstValue("Email") ?? "";
            if(ModelState.IsValid)
            {
                _context.Add(memberAddress);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress([Bind("Address")] MemberAddress memberAddress)
        {
            memberAddress.MemberEmail = User.FindFirstValue("Email") ?? "";
            if(ModelState.IsValid)
            {
                _context.MemberAddress.Remove(memberAddress);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> NewPhone([Bind("PhoneNo")] MemberPhoneNo memberPhone)
        {
            memberPhone.MemberEmail = User.FindFirstValue("Email") ?? "";
            if(ModelState.IsValid)
            {
                _context.Add(memberPhone);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhone([Bind("PhoneNo")] MemberPhoneNo memberPhone)
        {
            memberPhone.MemberEmail = User.FindFirstValue("Email") ?? "";
            if(ModelState.IsValid)
            {
                _context.MemberPhoneNo.Remove(memberPhone);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
