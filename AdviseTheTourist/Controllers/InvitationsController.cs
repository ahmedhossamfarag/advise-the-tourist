using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdviseTheTourist.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AdviseTheTourist.Controllers
{
    [Authorize(Policy = "IsLogIn")]
    public class InvitationsController : Controller
    {
        private readonly DatabaseContext _context;

        public InvitationsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Invitations
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitations = _context.Invitation.Where(m => m.MemberEmail == email || m.Member2Email == email)
                .OrderByDescending(m => m.SentTime);
            return View(await invitations.ToListAsync());
        }

        // GET: Invitations/Details/5
        [HttpPost]
        public async Task<IActionResult> Details([Bind("MemberEmail,Member2Email,PlaceName")] Invitation keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitation = await _context.Invitation
                .FindAsync(keys.MemberEmail, keys.Member2Email, keys.PlaceName);
            if (invitation == null || (invitation.MemberEmail != email && invitation.Member2Email != email))
            {
                return NotFound();
            }

            return View(invitation);
        }

        // GET: Invitations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invitations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberEmail,Member2Email,PlaceName,Accepted,SentTime")] Invitation invitation)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            invitation.MemberEmail = email;
            invitation.SentTime = DateTime.Now;
            invitation.Accepted = true;
            if (ModelState.IsValid)
            {
                if(_context.Member.Any(m => m.Email == invitation.Member2Email))
                {
                    var place = await _context.Place.FirstOrDefaultAsync(p => p.Name == invitation.PlaceName);
                    if (place != null)
                    {
                        if(place.AdminEmail == email)
                        {
                            _context.Add(invitation);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        ModelState.AddModelError(nameof(invitation.PlaceName), "You are not the admin of this place");
                    }
                    else
                        ModelState.AddModelError(nameof(invitation.PlaceName), "Place does not Exist");
                }
                else
                    ModelState.AddModelError(nameof(invitation.Member2Email), "Email does not Exist");
            }
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("MemberEmail,Member2Email,PlaceName")] Invitation keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitation = await _context.Invitation.FindAsync(keys.MemberEmail, keys.Member2Email, keys.PlaceName);
            if (invitation == null || (invitation.MemberEmail != email && invitation.Member2Email != email))
            {
                return NotFound();
            }
            invitation.Accepted = false;
            _context.Update(invitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); ;
        }

        // GET: Invitations/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("MemberEmail,Member2Email,PlaceName")] Invitation keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitation = await _context.Invitation
                .FindAsync(keys.MemberEmail, keys.Member2Email, keys.PlaceName);
            if (invitation == null || (invitation.MemberEmail != email && invitation.Member2Email != email))
            {
                return NotFound();
            }

            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("MemberEmail,Member2Email,PlaceName")] Invitation keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitation = await _context.Invitation.FindAsync(keys.MemberEmail, keys.Member2Email, keys.PlaceName);
            if (invitation == null || (invitation.MemberEmail != email && invitation.Member2Email != email))
            {
                return NotFound();
            }
            _context.Invitation.Remove(invitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept([Bind("MemberEmail,Member2Email,PlaceName")] Invitation keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var invitation = await _context.Invitation.FindAsync(keys.MemberEmail, keys.Member2Email, keys.PlaceName);
            if (invitation == null || invitation.Member2Email != email)
            {
                return NotFound();
            }
            _context.Invitation.Remove(invitation);
            var place = await _context.Place.FirstOrDefaultAsync(p => p.Name == invitation.PlaceName);
            if(place != null) 
            {
                place.AdminEmail = invitation.Member2Email;
                _context.Update(place);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvitationExists(Invitation keys)
        {
            return _context.Invitation.Find(keys.MemberEmail, keys.Member2Email, keys.PlaceName) != null;
        }
    }
}
