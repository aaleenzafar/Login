using System.Diagnostics;
using Login.Areas.Identity.Data;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationUserDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        // Default action
        public IActionResult Index()
        {
            return View();
        }

        // Redirects user based on their role
        [Authorize]
        public async Task<IActionResult> RedirectToRolePage()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogError("User is null.");
                return RedirectToAction("Index");
            }

            // Check if user is approved
            if (!user.IsApproved)
            {
                TempData["Error"] = "Your account is pending approval by the admin.";
                _logger.LogWarning("Unapproved user attempted to access.");
                return RedirectToAction("Index");
            }

            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation($"User {user.UserName} has roles: {string.Join(", ", roles)}");

            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminPage");
            }
            else if (roles.Contains("Student"))
            {
                return RedirectToAction("StudentPage");
            }
            else if (roles.Contains("Faculty"))
            {
                return RedirectToAction("FacultyPage");
            }

            _logger.LogWarning("User has no matching roles.");
            return RedirectToAction("Index");
        }

        // Admin Page
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        // Student Page
        [Authorize(Roles = "Admin,Student")]
        public IActionResult StudentPage()
        {
            return View();
        }

        // Faculty Page
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult FacultyPage()
        {
            return View();
        }
        [Authorize(Roles = "Student,Faculty")]
        public IActionResult ActiveParticipation()
        {
            return View();
        }

        // Manage all user registrations (Admin-only)
        [Authorize(Roles = "Admin")]
        public IActionResult ManageRegistrations()
        {
            // Fetch all users and exclude users with the "Admin" role
            var allUsers = _context.Users
                .Where(user => user.Role != null && !user.Role.Contains("Admin"))
                .ToList();

            return View(allUsers);
        }

        // Approve a user
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("ManageRegistrations");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsApproved = true;
                await _userManager.UpdateAsync(user);
                _logger.LogInformation($"User {user.UserName} approved.");
                TempData["Success"] = "User approved successfully.";
                return RedirectToAction("ManageRegistrations");
            }

            TempData["Error"] = "User not found.";
            return RedirectToAction("ManageRegistrations");
        }

        // Decline and delete a user
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeclineUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("ManageRegistrations");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                _logger.LogInformation($"User {user.UserName} declined and removed.");
                TempData["Success"] = "User declined and removed successfully.";
                return RedirectToAction("ManageRegistrations");
            }

            TempData["Error"] = "User not found.";
            return RedirectToAction("ManageRegistrations");
        }

        // Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
