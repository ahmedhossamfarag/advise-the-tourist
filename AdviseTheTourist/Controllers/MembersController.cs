
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace AdviseTheTourist.Controllers
{
    public class MembersController : Controller
    {
        public static string PersonalImagesPath = "Images/PersonalImages/";
        private readonly DatabaseContext _context;
        private IWebHostEnvironment _environment;
        public MembersController(DatabaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }



        // GET: Members
        [Authorize(Policy = "IsLogIn")]
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var friends = _context.Friend.Where(f => f.MemberEmail == email || f.Member2Email == email);
            var requests = _context.FriendRequest.Where(f => f.MemberEmail == email || f.Member2Email == email);
            var members =
                          from m in _context.Member
                          join fr in friends on m.Email equals fr.MemberEmail into frr
                          from fri in frr.DefaultIfEmpty()
                          join fl in friends on m.Email equals fl.Member2Email into flr
                          from fli in flr.DefaultIfEmpty()
                          join qr in requests on m.Email equals qr.MemberEmail into qrr
                          from qri in requests.DefaultIfEmpty()
                          join ql in requests on m.Email equals ql.Member2Email into qlr
                          from qli in requests.DefaultIfEmpty()
                          where m.Email != email
                          select new MemberFriendModel()
                          {
                              Email = m.Email,
                              Firstname = m.Firstname,
                              Lastname = m.Lastname,
                              Image = m.Image,
                              IsFriend = fri.MemberEmail != null || fli.Member2Email != null,
                              IsRequest = qri.MemberEmail != null,
                              IsResponse = qli.Member2Email != null,
                              Accepted = qli.Accepted,
                          };

            var list = await members.ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string email)
        {
            if(email.IsNullOrEmpty())
            {
                return NotFound();
            }
            var userEmail = User.FindFirstValue("Email");
            if(userEmail == null)
            {
                return RedirectToAction("Index", "Login");
            }
            _context.Add(new FriendRequest { MemberEmail = userEmail, Member2Email = email, Accepted = true });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(string email)
        {
            if(email.IsNullOrEmpty())
            {
                return NotFound();
            }
            var userEmail = User.FindFirstValue("Email");
            if(userEmail == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var request = await _context.FriendRequest.FirstOrDefaultAsync(r => r.MemberEmail == email && r.Member2Email == userEmail);
            if(request == null)
            {
                return NotFound();
            }
            _context.Add(new Friend { MemberEmail = userEmail, Member2Email = email });
            _context.FriendRequest.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRequest([Bind("email")]string email)
        {
            if(email.IsNullOrEmpty())
            {
                return NotFound();
            }
            var userEmail = User.FindFirstValue("Email");
            if(userEmail == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var request = await _context.FriendRequest.FirstOrDefaultAsync(r => r.MemberEmail == userEmail && r.Member2Email == email);
            if(request == null)
            {
                return NotFound();
            }
            _context.FriendRequest.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        


        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,Firstname,Lastname,Nationality,ImageFile")] NewMemberModel member)
        {
            if (ModelState.IsValid)
            {
                if(MemberExists(member.Email))
                {
                    ModelState.AddModelError(nameof(member.Email), "Email Already Exists");
                    return View(member);
                }
                if (member.ImageFile?.Length > 0)
                {
                    var filename = PersonalImagesPath + Path.GetFileName(member.ImageFile.FileName);
                    
                    var filePath = Path.Combine(_environment.WebRootPath,
                            filename);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await member.ImageFile.CopyToAsync(stream);
                    }
                    member.Image = $"~/{filename}";
                    _context.Add((Member)member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(member.ImageFile), "File Not Valid");
            }
            return View(member);
        }




        // GET: Members/Edit/5
        [Authorize(Policy = "IsLogIn")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var model = new NewMemberModel
            {
                Firstname = member.Firstname,
                Lastname = member.Lastname,
                Email = member.Email,
                Nationality = member.Nationality,
                Password = member.Password,
                Image = member.Image,
            };
            return View(model);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsLogIn")]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Password,Firstname,Lastname,Nationality,Image,ImageFile")] NewMemberModel member)
        {
            if (id != member.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (member.ImageFile?.Length > 0)
                    {
                        var filename = PersonalImagesPath + Path.GetFileName(member.ImageFile.FileName);

                        var filePath = Path.Combine(_environment.WebRootPath,
                                filename);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await member.ImageFile.CopyToAsync(stream);
                        }
                        member.Image = $"~/{filename}";
                    }
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProfileController.Index), "Profile");
            }
            return View(member);
        }




        // GET: Members/Delete/5
        [Authorize(Policy = "IsLogIn")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Email == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsLogIn")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(id != User.FindFirstValue("Email"))
            {
                return NotFound();
            }
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.Email == id);
        }
    }
}
