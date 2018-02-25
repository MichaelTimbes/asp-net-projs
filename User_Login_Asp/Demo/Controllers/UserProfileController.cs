using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Models;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Http;

namespace Demo.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserProfileContext _context;
        private readonly UserContext _context2;
        const string SessionUserID = "_UserID";

        public UserProfileController(UserProfileContext context, UserContext context2)
        {
            _context = context;
            _context2 = context2;
        }

        // GET: UserProfile
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserProfile.ToListAsync());
        }


        // GET: UserProfile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .SingleOrDefaultAsync(m => m.ID == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfile/Create
        public IActionResult Create()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> LinkToProfileFromLayout(int? id)
        {
            if (id == null)
            {
                id = (int)HttpContext.Session.GetInt32(SessionUserID);
            }
            // Find a possible userProfile if it exists
            var userProfile = await _context.UserProfile.SingleOrDefaultAsync(m => m.UserModelID == (int)id );
            var UserVal = await _context2.UserModel.SingleOrDefaultAsync(n => n.ID == (int)id);
            // If no Match
            if (userProfile == null)
            {
                
                // Check that the model is up to date
                if (ModelState.IsValid)
                {
                    // Create One
                    UserProfile temp = new UserProfile
                    {
                        UserModelID = (int)id,
                        UserProfileSummary = "",
                        UserProfileStatusUpdate = ""
                    };
                    // Add and Save
                    _context.Add(temp);
                    await _context.SaveChangesAsync();

                    UserVal.UserProfileID = temp.ID;
                    _context2.Update(UserVal);
                    await _context2.SaveChangesAsync();

                    return RedirectToAction("Details", routeValues: new { id=temp.ID });
                    //return RedirectToAction("ProfileView", routeValues: new { id = temp.ID });
                }
                // Something Bad Happened on the Server Side
                else
                {
                    return StatusCode(400);
                }
            }
            // If it does exist, return a detail view
            else
            {
                return RedirectToAction("Details",routeValues: new{id=userProfile.ID});
                //return RedirectToAction("ProfileView", routeValues: new { id = userProfile.ID });
            }

        }
        // POST: UserProfile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserModelID,UserProfileSummary,UserProfileStatusUpdate")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }
        // GET: UserProfile/Edit/5
        public IActionResult ProfEdit(int? id,string summary, string status)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var Uprof = _context.UserProfile.Where(e=>e.ID.Equals(id));

            foreach( UserProfile el in Uprof)
            {
                if ((string)status != null)
                {
                    el.UserProfileStatusUpdate = (string)status;
                }

                if ((string)summary != null)
                {
                    el.UserProfileSummary = (string)summary;
                }

            }
            _context.Update(Uprof);


            return View(Uprof.First());
        }


        // GET: UserProfile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.SingleOrDefaultAsync(m => m.ID == id);


            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserProfileSummary,UserProfileStatusUpdate,UserModelID")] UserProfile userProfile)
        {
            if (id != userProfile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //var Userval = await _context2.UserModel.SingleOrDefaultAsync(n => n.UserProfileID == id);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", routeValues: new { id = userProfile.ID });
            }
            return View(userProfile);
        }

        // GET: UserProfile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .SingleOrDefaultAsync(m => m.ID == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProfile = await _context.UserProfile.SingleOrDefaultAsync(m => m.ID == id);
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.ID == id);
        }
    }
}
