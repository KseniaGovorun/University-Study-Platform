using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.Controllers
{
    public class SheduleController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public SheduleController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        //GET: SheduleController
        //public ActionResult Index()
        //{
        //    IEnumerable<Shedule> coursesList = sheduleRepository.GetAll();
        //    return View(coursesList);
        //}

        // GET: SheduleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Index()
        {
            //AccountBook AccountBookCard = unitOfWork.accountBookRepository.GetFirstOrDefault(
            //    u => u.StudentId == studentCard.Id);
            //int groupId = AccountBookCard.GroupId;
            //IEnumerable<Shedule> sheduleList = unitOfWork.sheduleRepository.GetAll(
            //    u => u.GroupId == groupId);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Person person = unitOfWork.personRepository.GetFirstOrDefault(u => u.LoginData.Email == claim.Value);
            Student student = unitOfWork.studentRepository.GetStudentByPersonId(person.Id);
            List<List<Shedule>> sheduleList = unitOfWork.sheduleRepository.GetAllShedule(student);
            List<List<string>> subjectsNames = new List<List<string>>();

            //int SubjectsPerDayCount = 5;
            //for(int i = 0; i < sheduleList.Count; i++)
            //{
            //    List<string> dayList = new List<string>();
            //    for(int j = 0; j < SubjectsPerDayCount; j++)
            //    {
            //        dayList.Add(sheduleList[i][j].Faculty);
            //    }
            //    subjectsNames.Add(dayList);
            //}

            //for (int i = 0; i < sheduleList.Count(); i++)
            //{ 
            //    for (int j = 0; j < sheduleList[i].Count(); j++)
            //    {
            //        sheduleList[i][j].Subject.SubjectName
            //        resultAssignments.Add(assignments.ToList().ElementAt(j));
            //    }
            //}

            return View(sheduleList);
        }

        // GET: SheduleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SheduleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SheduleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SheduleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SheduleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
