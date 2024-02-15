using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdviseTheTourist.Models;
using System.Security.Claims;

namespace AdviseTheTourist.Controllers
{
    public class FriendRequestsController : Controller
    {
        private readonly DatabaseContext _context;

        public FriendRequestsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: FriendRequests
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var requestes = _context.FriendRequest.Where(r => r.Member2Email == email);
            var responses = _context.FriendRequest.Where(r => r.MemberEmail == email);
            var requestesMembers = from r in requestes
                                   join m in _context.Member on r.MemberEmail equals m.Email
                                   select new FriendRequestModel
                                   {
                                       Firstname = m.Firstname, Lastname = m.Lastname,
                                       Image = m.Image, MemberEmail = r.MemberEmail, Member2Email = r.Member2Email,
                                   };
            var responsesMembers = from r in responses
                                   join m in _context.Member on r.Member2Email equals m.Email
                                   select new FriendResponseModel
                                   {
                                       Firstname = m.Firstname, Lastname = m.Lastname,
                                       Image = m.Image, MemberEmail = r.MemberEmail,  Member2Email = r.Member2Email,
                                       Accepted = r.Accepted,
                                   };
            var model = new RequestsModel{Requests = await  requestesMembers.ToListAsync(), Responses = await responsesMembers.ToListAsync()};
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject([Bind("MemberEmail,Member2Email")] FriendRequest friendRequest)
        {
            try
            {
                friendRequest.Accepted = false;
                _context.Update(friendRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendRequestExists(friendRequest))
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept([Bind("MemberEmail,Member2Email")] FriendRequest request)
        {
            var friendRequest = await _context.FriendRequest.FindAsync(request.MemberEmail, request.Member2Email);
            if (friendRequest != null)
            {
                _context.Add(new Friend { MemberEmail = friendRequest.MemberEmail, Member2Email = friendRequest.Member2Email });
                _context.FriendRequest.Remove(friendRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("MemberEmail,Member2Email")] FriendRequest request)
        {
            var friendRequest = await _context.FriendRequest.FindAsync(request.MemberEmail, request.Member2Email);
            if (friendRequest != null)
            {
                _context.FriendRequest.Remove(friendRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendRequestExists(FriendRequest request)
        {
            return _context.FriendRequest.Any(e => e.MemberEmail == request.MemberEmail && e.Member2Email == request.Member2Email);
        }
    }
}
