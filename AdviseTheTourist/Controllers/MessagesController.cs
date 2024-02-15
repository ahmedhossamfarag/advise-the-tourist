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
    public class MessagesController : Controller
    {
        private readonly DatabaseContext _context;

        public MessagesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue("Email");
            if(email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var messages = _context.Message.Where(m => m.MemberEmail == email || m.Member2Email == email)
                .OrderByDescending(m => m.SentTime);
            return View(await messages.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Details([Bind("MemberEmail,Member2Email,SentTime")] Message keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var message = await _context.Message
                .FindAsync(keys.MemberEmail, keys.Member2Email, keys.SentTime);
            if (message == null || (message.MemberEmail != email && message.Member2Email != email))
            {
                return NotFound();
            }
            return View(message);
        }

         public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberEmail,Member2Email,Content")] Message message)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            message.MemberEmail = email;
            message.SentTime = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
               if(_context.Member.Any(m => m.Email == message.Member2Email))
                {
                    _context.Add(message);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(message.Member2Email), "Email does not Exist");
            }
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("MemberEmail,Member2Email,SentTime")] Message keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var message = await _context.Message.FindAsync(keys.MemberEmail, keys.Member2Email, keys.SentTime);
            if (message == null || message.Member2Email != email)
            {
                return NotFound();
            }
            return View(message);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed([Bind("Content,Reply,MemberEmail,Member2Email,SentTime")] Message message)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (message.Member2Email != email)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message))
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
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([Bind("MemberEmail,Member2Email,SentTime")] Message keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var message = await _context.Message
                .FindAsync(keys.MemberEmail, keys.Member2Email, keys.SentTime);
            if (message == null || (message.MemberEmail != email && message.Member2Email != email))
            {
                return NotFound();
            }

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("MemberEmail,Member2Email,SentTime")] Message keys)
        {
            var email = User.FindFirstValue("Email");
            if (email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var message = await _context.Message.FindAsync(keys.MemberEmail, keys.Member2Email, keys.SentTime);
            if (message == null || (message.MemberEmail != email && message.Member2Email != email))
            {
                return NotFound();
            }
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(Message keys)
        {
            return _context.Message.Find(keys.MemberEmail, keys.Member2Email, keys.SentTime) != null;
        }
    }
}
