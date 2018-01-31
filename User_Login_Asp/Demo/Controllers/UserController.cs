using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;


namespace Demo.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserModel.ToListAsync());
        }
        // GET: User/UsersIndex
        public async Task<IActionResult> UsersIndex()
        {
            return View(await _context.UserModel.ToListAsync());
        }

        // GET: User/UserHome/?User_Name=&User_Password=
        // Used at LogIn Screen, LINQ on User_Name and User_Password
        public IActionResult UserHome(string User_Name, string User_Password)
        {
            var userModel = from value_ in _context.UserModel select value_;

            if(!String.IsNullOrEmpty(User_Name) && !String.IsNullOrEmpty(User_Password))
            {
                userModel = userModel.Where(s => s.User_Name.Contains(User_Name) && 
                                            s.User_Password.Equals(User_Password));
            }
            if(userModel.Count() == 0){
                
                ViewData["Message"] = "User Not Found";

                return RedirectToAction("UserNotFound");
            }
            ViewData["Message"] = "Hello";

            return View(userModel.First());
        }
        //GET User/UserNotFound/?
        public IActionResult UserNotFound()
        {
            return View();
        }

        // GET: User/Details/?
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User_Name,User_Password,User_SignIn_Status")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UsersIndex));
            }

            return View(userModel);
        }

        // GET: User/Edit/5
        // Action to Pull User ID Data from HTML Element
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.SingleOrDefaultAsync(m => m.ID == id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,User_Name,User_Password,User_SignIn_Status")] UserModel userModel)
        {
            if (id != userModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserHome));
            }
            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.UserModel.SingleOrDefaultAsync(m => m.ID == id);
            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
            return _context.UserModel.Any(e => e.ID == id);
        }
    }
}
