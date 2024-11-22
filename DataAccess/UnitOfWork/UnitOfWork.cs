using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.DataAccess.UnitOfWork;
using UniversityStudyPlatform.DataAccess.UnitOfWork.Repository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
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

        public ApplicationDbContext db { get; }

        public UnitOfWork(ApplicationDbContext _db)
        {
            db = _db;
            accountBookRepository = new Repository<AccountBook>(_db);
            assignmentRepository = new AssignmentRepository(_db);
            courseRepository = new CourseRepository(_db);
            courseGroupRepository = new CourseGroupRepository(_db);
            groupRepository = new Repository<Group>(_db);
            loginDataRepository = new Repository<LoginData>(_db);
            messageRepository = new Repository<Message>(_db);
            sheduleRepository = new SheduleRepository(_db);
            studentRepository = new StudentRepository(_db);
            studentIndividualTaskRepository = new Repository<StudentIndividualTask>(_db);
            studentPerformanceRepository = new StudentPerfomanceRepository(_db);
            personRepository = new PersonRepository(_db);
            teacherRepository = new TeacherRepository(_db);
            subjectRepository = new SubjectRepository(_db);
            vmLoginRepository = new Repository<VMLogin>(_db);
            creditFormRepository = new Repository<CreditForm>(_db);
            termRepository = new Repository<Term>(_db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
