using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeltExam1.Models;

namespace BeltExam1.Controllers
{
    public class DashController : Controller
    {
        private BeltContext _context;
 
        public DashController(BeltContext context)
        {
            _context = context;
        }
        private User ActiveUser 
        {
            get{ return _context.Users.Where(u => u.UserId == HttpContext.Session.GetInt32("Id")).SingleOrDefault();}
        }
        [Route("Home")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            if (ActiveUser == null){
                return RedirectToAction("Index", "User");
            }
            Dashboard dashData = new Dashboard
            {
                Activities = _context.Activities.Include(w => w.Players).OrderBy(t => t.Date).ToList(),
                User = ActiveUser,
                Users = _context.Users.ToList(),
            };
            return View(dashData);
        }

        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            if (ActiveUser != null){
                TempData["Success"] = "Successfully Logged Out";
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }

        [Route("GoHome")]
        [HttpGet]
        public IActionResult GoHome()
        {
            if (ActiveUser == null){
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Dashboard", "Dash");
        }

        [Route("CreateActivity")]
        [HttpGet]
        public IActionResult CreateActivity()
        {
            if(ActiveUser == null){
               return RedirectToAction("Index", "User");
            }
            return View();
        }

        [Route("New")]
        [HttpPost]
        public IActionResult New(ActivityView model)
        {
            if (ModelState.IsValid){
                Activity NewActivity = new Activity();
                NewActivity.Title = model.Title;
                NewActivity.Time = model.Time;
                NewActivity.Date = model.Date;
                NewActivity.Duration = model.Duration;
                NewActivity.Description = model.Description;
                NewActivity.UserId = ActiveUser.UserId;
                _context.Activities.Add(NewActivity);
                _context.SaveChanges();
                return RedirectToAction("Show", new { id = NewActivity.ActivityId});
            }
            ViewBag.errors = ModelState.Values;
            return View("CreateActivity");
        }

        [Route("activity/{id}")]
        [HttpGet]
        public IActionResult Show(int id)
        {
            if(ActiveUser == null){
               return RedirectToAction("Index", "User");
            }
            Showdata showData = new Showdata
            {
                Activity = _context.Activities.Include(w => w.Players).ThenInclude(g => g.Players).Where(w => w.ActivityId == id).SingleOrDefault(),
                User = ActiveUser,
                Users = _context.Users.ToList(),
            };
            Activity query = _context.Activities.Include(w => w.Players).ThenInclude(g => g.Players).Where(w => w.ActivityId == id).SingleOrDefault();
            return View(showData);
        }

        [Route("join/{id}")]
        [HttpGet]
        public IActionResult Join(int id)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "User");
            Signup signup = new Signup
            {
                UserId = ActiveUser.UserId,
                ActivityId = id
            };
            _context.Signups.Add(signup);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [Route("leave/{id}")]
        [HttpGet]
        public IActionResult Leave(int id)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "User");
            Signup toDelete = _context.Signups.Where(r => r.ActivityId == id)
                                          .Where(r => r.UserId == ActiveUser.UserId)
                                          .SingleOrDefault();
            _context.Signups.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("DashBoard");
        }
        
        [Route("delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "User");
            Activity toDelete = _context.Activities.Where(r => r.ActivityId == id)
                                          .Where(r => r.UserId == ActiveUser.UserId)
                                          .SingleOrDefault();
            _context.Activities.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("DashBoard");
        }
    }
}