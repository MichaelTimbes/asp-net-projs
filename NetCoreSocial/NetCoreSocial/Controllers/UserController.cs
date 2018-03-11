using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSocial.Models;
using Microsoft.AspNetCore.Identity;
using NetCoreSocial.Models.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using System;

namespace NetCoreSocial.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        /*         CONTROLLER INITIALIZE                             */
        public UserController(UserContext context,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /*        END  CONTROLLER INITIALIZE                             */


        /* USER HOME REGION
         * GET User/UserHome
         * Redirects to the User Home Page OR
         * To Login if user isn't signed in.
        */

        #region USER_HOME

        // GET: User/UserHome/
        public IActionResult UserHome()
        {
            var res = _signInManager.IsSignedIn(User);
            if (res)
            {
                var ActiveUser = _userManager.GetUserId(User);
                var userModel = _context.Users.Where(m => m.Id == ActiveUser);
                return View(userModel.First());
            }
            // Redirect to user login if the user is not signed in
            return View();
        }

        #endregion USER_HOME

        /* USER LOGIN REGION
         * GET User/UserLogin
         * POST User/UserLogin
         * Redirects to the User Home Page
        */

        #region USER_LOGIN

        [HttpGet] 
        public IActionResult Login(string returnURL = null )
        {
            ViewData["ReturnUrl"] = returnURL;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVC model, string returnURL=null)
        {
            ViewData["ReturnUrl"] = returnURL;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    return RedirectToAction(actionName:"UserHome");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion USER_LOGIN

        /* REGISTER USER REGION
         * Presents a form to register a user.
         * Handles secure user creation.
        */
        #region REGISTER_USR
        // GET: /User/RegisterUser
        public IActionResult RegisterUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();

        }

        // POST: /User/RegisterUser/{RegisterUserVC&returnUrl}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserVC model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new UserModel { UserName = model.UserName, Email = model.UserEmail };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //return RedirectToLocal(returnUrl);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(actionName: "Index");
                }
            }
            // Model State is in valid
            return View(model);
        }
        #endregion REGISTER_USR

        /*                                              HELPER FUNCTIONS                                        */
        #region HELPER_REGION

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    #endregion HELPER_REGION









        /*                                  BASIC CREATE READ UPDATE METHODS                         */









        #region USER_CRUD

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserModel.ToListAsync());
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] UserModel userModel)
        {
            if (id != userModel.Id)
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
                    if (!UserModelExists(userModel.Id))
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
            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userModel = await _context.UserModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(string id)
        {
            return _context.UserModel.Any(e => e.Id == id);
        }
        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .SingleOrDefaultAsync(m => m.Id == id);
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

#endregion USER_CRUD
    }
}
