using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class AssignmentController : Controller
    {
        private IUnitOfWork unitOfWork;

        public AssignmentController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Person person = unitOfWork.personRepository.GetFirstOrDefault(u => u.LoginData.Email == claim.Value);
            Student student = unitOfWork.studentRepository.GetStudentByPersonId(person.Id);
            IEnumerable<Assignment> assignmentsOfStudent = unitOfWork.assignmentRepository.GetAllAssignmentsOfStudent(student);

            List<CourseGroup> courseGroups = new List<CourseGroup>();
            foreach(Assignment assignment in assignmentsOfStudent)
            {
                courseGroups.Add(unitOfWork.courseGroupRepository.GetCourseGroupByAssignment(assignment));
            }

            ViewBag.CourseGroups = courseGroups;
            ViewBag.AssignmentsOfStudent = assignmentsOfStudent;
            return View();
        }

        public IActionResult Details(int assignmentId)
        {
            Assignment assignment = unitOfWork.assignmentRepository.GetFirstOrDefault(u => u.Id == assignmentId);
            CourseGroup courseGroup = unitOfWork.courseGroupRepository.GetCourseGroupByAssignment(assignment);

            ViewBag.Assignment = assignment;
            ViewBag.CourseGroup = courseGroup;
            return View();
        }
    }
}
