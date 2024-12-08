//using Login.Areas.Identity.Data;
//using Login.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace Login.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly SignInManager<ApplicationUser> _signInManager;

//        public AccountController(SignInManager<ApplicationUser> signInManager)
//        {
//            _signInManager = signInManager;
//        }

//        [HttpGet]
//        public IActionResult Login()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await _signInManager.PasswordSignInAsync(
//                    model.Email,
//                    model.Password,
//                    model.RememberMe,
//                    lockoutOnFailure: false);

//                if (result.Succeeded)
//                {
//                    return RedirectToAction("RedirectToRolePage", "Home");
//                }

//                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//            }
//            return View(model);
//        }
//    }
//}
