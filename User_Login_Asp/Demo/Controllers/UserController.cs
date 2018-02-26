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
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly FriendContext _context2;

        const string SessionUserID = "_UserID";

        public UserController(UserContext context, FriendContext context2)
        {
            _context = context;
            _context2 = context2;
        }

        // GET: User
        // Default Route for Controller
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserModel.ToListAsync());
        }

        // GET: User/IndexSearch/?UserName=&UserRelayID=
        /* The UserName is the user to search. 
         * The UserRelayID is the Active UserID which is to be secret.
        */
        public async Task<IActionResult> IndexSearch(string UserName)
        {
            // Find User with the ID information
            var userFound = from uf in _context.UserModel select uf;
            var UserFound = userFound.Where(m => m.User_Name.Contains(UserName));

            return View(viewName:"DetailsEnV2", model: await UserFound.ToListAsync());
        }


        // GET: User/UsersIndex
        // Alternate View of the Users without access to edit and details.
        public async Task<IActionResult> UsersIndex()
        {
            return View(await _context.UserModel.ToListAsync());
        }
        // GET: User/FindUser/?
        /* Based on the UserModel the argument is a user ID.
         * From the user ID, it is confirmed that the user exists.
         * Then a redirect sends the id information to the ProfileContoller
         * Which will return a Profile.
        */
        [HttpPost]
        public IActionResult FindUserProfile(string User_Name)
        {
            // If no ID to base search on, return error
            if (!String.IsNullOrEmpty(User_Name))
            {
                return NotFound();
            }
            // Find User with the ID information
            var userFound = from uf in _context.UserModel select uf;
            var UserFound = userFound.Where(m=>m.User_Name == User_Name);

            if(UserFound == null)
            {
                return NotFound();   
            }

            RedirectToAction(controllerName:"UserProfileController", actionName: "Details", routeValues: new { id=UserFound.First().ID });
            return View();
        }

        // GET: User/GoBackHome/?id
        /* Function that sends a user based on User.ID to their home page.
         * 
        */
        public async Task<IActionResult> GoBackHome(int? id)
        {
            if (id == null)
            {
                id = HttpContext.Session.GetInt32(SessionUserID);

                if (id == null)
                {
                    return RedirectToAction(controllerName: "Home", actionName: "LogIn");
                }
            }


                var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.ID == id);

                if (userModel == null)
                {
                    return NotFound();
                }

                //return View(userModel);
                return View("UserHome", userModel);

        }

        // GET: User/UserHome/?User_Name=&User_Password=
        // Used at LogIn Screen, LINQ on User_Name and User_Password
        public IActionResult UserHome(string User_Name, string User_Password)
        {
            // Begin by selecting the values in the database
            var userModel = from value_ in _context.UserModel select value_;
            // Check Strings
            if(!String.IsNullOrEmpty(User_Name) && !String.IsNullOrEmpty(User_Password))
            {
                // Search User 
                userModel = userModel.Where(s => s.User_Name.Equals(User_Name) && 
                                            s.User_Password.Equals(User_Password));
            }
            // If No User Exists
            if(userModel.Count() == 0){
                
                ViewData["Message"] = "User Not Found";

                return RedirectToAction("UserNotFound");
            }
            // Else 
            ViewData["Message"] = "Hello";

            HttpContext.Items.TryAdd("SessUserID",userModel.First().ID);



            HttpContext.Session.SetInt32(SessionUserID,userModel.First().ID);

            return View(userModel.First());
        }

        //GET User/UserNotFound/?
        public IActionResult UserNotFound()
        {
            return View();
        }
        // GET: User/Details/?
        public async Task<IActionResult> DetailsBasic(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                id = HttpContext.Session.GetInt32(SessionUserID);
            }

            var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }
        // GET: User/Details/?
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                id = HttpContext.Session.GetInt32(SessionUserID);

                if (id == null)
                {
                    return NotFound();
                }
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
        // Not an active route to creating, brings up the view which contains the form.
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
                //return RedirectToAction(nameof(UsersIndex));
                string UNAME = userModel.User_Name;
                string UPASS = userModel.User_Password;
                //return RedirectToAction(actionName: "UserHome", routeValues: new {User_Name = UNAME, User_Pass = UPASS });
                //return RedirectToAction(actionName: "UserHome");
                RedirectToAction(controllerName: "UserProfileController", actionName: "CreateWithUser", routeValues: new
                {
                    UserModelID = userModel.ID,
                    UserProfileSummary = "",
                    UserProfileStatusUpdate = "Hello World!"

                });
                return View("UserHome", userModel);

            }

            return View(userModel);
        }

        // GET: User/Edit/5
        // Action to Pull User ID Data from HTML Element
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                id = HttpContext.Session.GetInt32(SessionUserID);

                if (id == null)
                {
                    return NotFound();
                }
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
                return RedirectToAction("UserHome", routeValues: new{User_Name =userModel.User_Name, User_Password= userModel.User_Password});
            }
            return RedirectToAction("UserHome", routeValues: new { User_Name = userModel.User_Name, User_Password = userModel.User_Password });
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
