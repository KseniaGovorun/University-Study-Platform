using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepository<AccountBook> accountBookRepository { get; set; }
        public IAssignmentRepository assignmentRepository { get; set; }
        public ICourseRepository courseRepository { get; set; }
        public ICourseGroupRepository courseGroupRepository { get; set; }
        public IRepository<Group> groupRepository { get; set; }
        public IRepository<LoginData> loginDataRepository { get; set; }
        public IRepository<Message> messageRepository { get; set; }
        public ISheduleRepository sheduleRepository { get; set; }
        public IStudentRepository studentRepository { get; set; }
        public IRepository<StudentIndividualTask> studentIndividualTaskRepository { get; set; }
        public IStudentPerfomanceRepository studentPerformanceRepository { get; set; }
        public IPersonRepository personRepository { get; set; }
        public ITeacherRepository teacherRepository { get; set; }
        public ISubjectRepository subjectRepository { get; set; }
        public IRepository<VMLogin> vmLoginRepository { get; set; }
        public IRepository<CreditForm> creditFormRepository { get; set; }
        public IRepository<Term> termRepository { get; set; }

        void Save();
    }
}
