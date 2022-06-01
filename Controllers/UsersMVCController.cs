using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignalR_Chat.Data;

namespace SignalR_Chat.Controllers
{
    public class UsersMVCController : Controller
    {
        private readonly ChatContext _context;

        public UsersMVCController(ChatContext context)
        {
            _context = context;
        }

        // GET: UsersMVC
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'ChatContext.Users'  is null.");
        }

        [HttpPost]
        public ActionResult Create(string userName)
        {
            if (_context.Users == null)
            {
                return Json("Entity set 'ChatContext.Users'  is null.");
            }
            var userNr = _context.Users.Where(x => x.Name == userName).ToList().Count;
            if (userNr == 0)
            {
                User user = new User
                {
                    Name = userName
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                return Json("User added successfully!");
            }
            else
            {
                return Json("User " + userName + " exists in db.");
            }
        }
    }
}
