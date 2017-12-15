using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BeltExam1.Models;

namespace BeltExam1.Controllers
{
    public class UserController : Controller
    {
        private BeltContext _context;
 
        public UserController(BeltContext context)
        {
            _context = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.errors = ModelState.Values;
            if (TempData["Success"] != null){
                ViewBag.success = TempData["Success"];
            }
            return View();
        }

        [Route("")]
        [HttpPost]
        public IActionResult Process(UserView model)
            {
            bool HasUser = _context.Users.Any(user => user.Email == model.Email);
            if (ModelState.IsValid && HasUser == false){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User();
                NewUser.FirstName = model.FirstName;
                NewUser.LastName = model.LastName;
                NewUser.Email = model.Email;
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                TempData["Success"] = "User has been registered";
                return RedirectToAction("Index");
            }
            ViewBag.errors = ModelState.Values;
            if (HasUser){
                ModelState.AddModelError("Email", "Email address already exists. Please enter a different email address.");
            }
            return View("Index");
            
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginView form)
        {
            if(ModelState.IsValid)
            { 
                User query = _context.Users.Where(u => u.Email ==form.LogEmail ).SingleOrDefault();
                if (query == null){
                    ModelState.AddModelError("LogPassword", "Invalid Email/Password");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(query, query.Password, form.LogPassword)){
                    HttpContext.Session.SetInt32("Id", query.UserId);
                    return RedirectToAction("Dashboard", "Dash"); 
                }
                ModelState.AddModelError("LogPassword", "Invalid Email/Password");
            }
            return View("Index");
        }
    }
}
