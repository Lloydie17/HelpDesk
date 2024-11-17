using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementController(IUserService userService, 
                                        IHttpContextAccessor httpContextAccessor,
                                        ITeamService teamService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _teamService = teamService;
        }

        // GET: UserManagement
        public IActionResult Index()
        {
            // Get the role of the logged-in user
            var userRole = HttpContext.Session.GetString("UserRole");

            var users = _userService.GetUsers().Where(u => u.IsActive);

            Console.WriteLine($"Logged-in user role: {userRole}");

            if (userRole == "Admin")
            {
                // If the user is Admin, exclude Superadmin
                users = users.Where(u => u.Role != "Superadmin");
            }

            return View(users);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            // Fetch teams for dropdown
            (bool result, IEnumerable<Team> teams) = _teamService.GetTeams();

            if (result)
            {
                ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");
            }
            else
            {
                ViewBag.Teams = new SelectList(new List<Team>(), "TeamId", "TeamName");
            }

            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                // Fetch teams for dropdown
                (bool result, IEnumerable<Team> teams) = _teamService.GetTeams();

                if (result)
                {
                    ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");
                }
                else
                {
                    ViewBag.Teams = new SelectList(new List<Team>(), "TeamId", "TeamName");
                }

                return View(user);
            }

            _userService.AddUser(user);
            return RedirectToAction("Index");
        }

        // POST: UserManagement/Edit
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (user != null)
            {
                _userService.UpdateUser(user);
                TempData["SuccessMessage"] = "User has been edited";
            }

            // Fetch teams for dropdown
            (bool result, IEnumerable<Team> teams) = _teamService.GetTeams();

            if (result)
            {
                ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");
            }
            else
            {
                ViewBag.Teams = new SelectList(new List<Team>(), "TeamId", "TeamName");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            if (user != null)
            {
                _userService.DeleteUser(user);
            }

            return RedirectToAction("Index");
        }
    }
}