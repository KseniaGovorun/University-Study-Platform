using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UniversityStudyPlatform.Models;
using UniversityStudyPlatform.DataAccess.UnitOfWork;

namespace UniversityStudyPlatform.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AccessController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Login()
        {
            //if user is already logged in
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {

            //find student in database
            //var student = studentRepository.GetFirstOrDefault(u => u.Email == modelLogin.Email
            //&& u.Password == modelLogin.Password);

            LoginData userLoginData = unitOfWork.loginDataRepository.GetFirstOrDefault(u => 
                        u.Email == modelLogin.Email &&
                        u.Password == modelLogin.Password);

            Person person = unitOfWork.personRepository.GetFirstOrDefault(u =>
                        u.LoginData.Id == userLoginData.Id);

            Teacher teacher = unitOfWork.teacherRepository.GetFirstOrDefault(u =>
                        u.PersonId == person.Id);

            Student student = unitOfWork.studentRepository.GetFirstOrDefault(u =>
                        u.PersonId == person.Id);

            if (student != null)
            {
                ViewBag.StudentId = student.Id;
            }

            if (student != null || teacher != null)
            {
                string userName = "";
                userName = person.Name;
                ViewBag.UserName = userName;

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("StudentId", student.Id.ToString())
                };

                ClaimsIdentity claimIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimIdentity), properties);
                return RedirectToAction("Index", "Home");

            }

            ViewData["ValidateMessage"] = "user is not found";
            return View();
        }
	}
}
