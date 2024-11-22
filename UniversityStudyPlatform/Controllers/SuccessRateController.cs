using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class SuccessRateController : Controller
    {
        private IUnitOfWork unitOfWork;

        public SuccessRateController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            List<StudentPerfomance> studentPerfomancesByStudent = new List<StudentPerfomance>();
            List<Subject> subjects = new List<Subject>();
            List<float> сurrentPoints = new List<float>();
            List<float> examPoints = new List<float>();
            List<float> totalPoints = new List<float>();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            int? studentId = Int32.Parse(claimsIdentity.FindFirst("StudentId").Value);

            if (studentId != null)
            {
                studentPerfomancesByStudent = unitOfWork.studentPerformanceRepository.GetStudentPerfomanceByStudent(unitOfWork.studentRepository.GetFirstOrDefault(u => u.Id == studentId));

                foreach(StudentPerfomance studentPerfomance in studentPerfomancesByStudent)
                {
                    subjects.Add(unitOfWork.subjectRepository.GetFirstOrDefault(u => u.SubjectId == studentPerfomance.SubjectId));
                    сurrentPoints.Add(studentPerfomance.CurrentPoint);
                    examPoints.Add(studentPerfomance.ExamPoint);
                    totalPoints.Add(studentPerfomance.TotalPoint);
                }

                ViewBag.StudentPerfomancesByStudent = studentPerfomancesByStudent;
                ViewBag.Subjects = subjects;
                ViewBag.CurrentPoints = сurrentPoints;
                ViewBag.ExamPoints = examPoints;
                ViewBag.TotalPoints = totalPoints;
            }

            return View();
        }
    }
}
