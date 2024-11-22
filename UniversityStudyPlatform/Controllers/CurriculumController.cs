using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class CurriculumController : Controller
    {
        private IUnitOfWork unitOfWork;

        public CurriculumController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            List<Course> coursesByStudent = new List<Course>();
            List<Subject> subjects = new List<Subject>();
            List<string> teachersNames = new List<string>();
            List<CreditForm> creditForms = new List<CreditForm>();
            List<Term> terms = new List<Term>();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            int? studentId = Int32.Parse(claimsIdentity.FindFirst("StudentId").Value);

            if (studentId != null)
            {
                coursesByStudent = unitOfWork.courseRepository.GetCoursesByStudent(unitOfWork.studentRepository.GetFirstOrDefault(u => u.Id == studentId));

                foreach(Course course in coursesByStudent)
                {
                    subjects.Add(unitOfWork.subjectRepository.GetFirstOrDefault(u => u.SubjectId == course.SubjectId));
                    teachersNames.Add(unitOfWork.teacherRepository.GetTeacherNameById(course.TeacherId));
                    creditForms.Add(unitOfWork.creditFormRepository.GetFirstOrDefault(u => u.Id == course.CreditFormId));
                    terms.Add(unitOfWork.termRepository.GetFirstOrDefault(u => u.Id == course.TermId));
                }

                ViewBag.Courses = coursesByStudent;
                ViewBag.Subjects = subjects;
                ViewBag.Teachers = teachersNames;
                ViewBag.CreditForms = creditForms;
                ViewBag.Terms = terms;
            }

            return View();
        }
    }
}
