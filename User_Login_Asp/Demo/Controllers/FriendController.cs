using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using Microsoft.AspNetCore.Http;

namespace Demo.Controllers
{
    public class FriendController : Controller
    {
        private readonly FriendContext _context;
        private readonly UserContext _context2;
        const string SessionUserID = "_UserID";

        public FriendController(FriendContext context, UserContext context2)
        {
            _context = context;
            _context2 = context2;
        }

        // GET: Friend
        public async Task<IActionResult> Index()
        {
            return View(await _context.FriendModel.ToListAsync());
        }

        // GET: FriendSearch/?id
       
        public async Task<IActionResult> FriendSearch(int? id)
        {

            /* if (id==null)
             {
                 id = HttpContext.Session.GetInt32(SessionUserID);
                 if (id == null)
                 {
                     return NotFound();
                 }
             }
             // Get Active User ID
             var SessionUsr = _context2.UserModel.Where(n => n.ID == id);
             // Find Friends
             var FriendList = _context.FriendModel.Where(m => m.UserProfileIDA == SessionUsr.First().UserProfileID 
                                                         || m.UserProfileIDB == SessionUsr.First().UserProfileID);
             if (FriendList != null)
             {
                 return View(await FriendList.ToListAsync());
             }*/
            ViewData["Message"] = id;
            return View(await _context.FriendModel.ToListAsync());
        }

        // GET: Friend/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendModel = await _context.FriendModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (friendModel == null)
            {
                return NotFound();
            }

            return View(friendModel);
        }

        // GET: Friend/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Friend/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserAcceptA,UserAcceptB,User_NameA,User_NameB,UserProfileIDA,UserProfileIDB")] FriendModel friendModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friendModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(friendModel);
        }

        // GET: Friend/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendModel = await _context.FriendModel.SingleOrDefaultAsync(m => m.ID == id);
            if (friendModel == null)
            {
                return NotFound();
            }
            return View(friendModel);
        }

        // POST: Friend/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserAcceptA,UserAcceptB,User_NameA,User_NameB,UserProfileIDA,UserProfileIDB")] FriendModel friendModel)
        {
            if (id != friendModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friendModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendModelExists(friendModel.ID))
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
            return View(friendModel);
        }

        // GET: Friend/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendModel = await _context.FriendModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (friendModel == null)
            {
                return NotFound();
            }

            return View(friendModel);
        }

        // POST: Friend/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friendModel = await _context.FriendModel.SingleOrDefaultAsync(m => m.ID == id);
            _context.FriendModel.Remove(friendModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendModelExists(int id)
        {
            return _context.FriendModel.Any(e => e.ID == id);
        }
    }
}
