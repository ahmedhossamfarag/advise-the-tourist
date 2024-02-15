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
    public class FriendsController : Controller
    {
        private readonly DatabaseContext _context;

        public FriendsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var friends1Emails = from f1 in _context.Friend
                                where f1.MemberEmail == email
                                select f1.Member2Email;
            var friends2Emails = from f2 in _context.Friend
                                 where f2.Member2Email == email
                                 select f2.MemberEmail;
            var friendsEmails = friends1Emails.Concat(friends2Emails);
            var friends = from m in _context.Member
                          join f in friendsEmails on m.Email equals f
                          select new FriendModel
                          {
                              Email = m.Email,
                              Image = m.Image,
                              Firstname = m.Firstname,
                              Lastname = m.Lastname,

                          };
            return View(await friends.ToListAsync());
        }

    }
}
