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
    public class MessagesMVCController : Controller
    {
        private readonly ChatContext _context;

        public MessagesMVCController(ChatContext context)
        {
            _context = context;
        }

        // GET: MessagesMVC
        public async Task<IActionResult> Index()
        {
            var chatContext = _context.Messages.Include(m => m.User);
            return View(await chatContext.ToListAsync());
        }

        public ActionResult InsertMessage(string messageText, string userName)
        {
            try
            {
                var user = _context.Users?.Where(x => x.Name == userName).ToList().FirstOrDefault();
                if (user == null)
                {
                    user = new User
                    {
                        Name = userName
                    };
                    _context.Users.Add(user);
                }
                var message = new Message
                {
                    CreatedDate = DateTime.Now,
                    MessageText = messageText,
                    UserId = user.Id,
                    UserName = userName,
                    User = user
                };
                _context.Messages.Add(message);
                _context.SaveChanges();
                return Json("Message was insert!");
            }
            catch (Exception ex)
            {
                return Json("Error occured. Message was'n insert!");
            }
        }
       [HttpGet]
       public ActionResult GetAllMessages()
       {
            var messages = _context.Messages.Where(x => x.CreatedDate > DateTime.Now.AddDays(-7)).ToList();
            return Json(messages);
       }
    }
}
