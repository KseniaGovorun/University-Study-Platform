using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class CoursesController : Controller
    {
        private IUnitOfWork unitOfWork;

        public CoursesController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            //check if user is logged in
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Person person = unitOfWork.personRepository.GetFirstOrDefault(u => u.LoginData.Email == claim.Value);
            Student student = unitOfWork.studentRepository.GetFirstOrDefault(u => u.PersonId == person.Id);
            AccountBook accountBook = unitOfWork.accountBookRepository.GetFirstOrDefault(u => u.StudentId == student.Id);
            Group group = unitOfWork.groupRepository.GetFirstOrDefault(u => u.Id == accountBook.GroupId);
            IEnumerable<CourseGroup> courseGroups = unitOfWork.courseGroupRepository.GetAll(u => u.GroupId == group.Id);
            //List<Course> coursesList = new List<Course>();
            //coursesList = courseGroups.Select(x => unitOfWork.courseRepository.GetFirstOrDefault(u => u.Id == x.CourseId)).ToList();

            return View(courseGroups);
        }
    }
}
