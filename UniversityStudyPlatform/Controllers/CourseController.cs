using Microsoft.AspNetCore.Mvc;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CourseController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index(int courseId)
        {
            CourseGroup courseGroup = unitOfWork.courseGroupRepository.GetFirstOrDefault(u => u.Id == courseId);
            IEnumerable<Message> messages = unitOfWork.messageRepository.GetAll(u => u.CourseGroupId == courseGroup.Id);
            IEnumerable<Assignment> assignments = unitOfWork.assignmentRepository.GetAll(u => u.CourseGroupId == courseGroup.Id);

            ViewBag.CourseGroup = courseGroup;
            ViewBag.Messages = messages;
            ViewBag.Assignments = assignments;

            //Person person = unitOfWork.personRepository.GetFirstOrDefault();
            return View();
        }
    }
}
