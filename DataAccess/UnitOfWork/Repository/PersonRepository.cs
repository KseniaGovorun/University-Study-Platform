using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext _db) : base(_db) { }

        // Add method to get Person by name
        public Person GetPersonByName(string name)
        {
            return db.Persons.FirstOrDefault(p => p.Name == name);
        }

        // Add method to get Person by surname
        public Person GetPersonBySurname(string surname)
        {
            return db.Persons.FirstOrDefault(p => p.Surname == surname);
        }

        // Add method to check if a Person exists by their LoginData
        public bool PersonExistsByLoginData(LoginData loginData)
        {
            return db.Persons.Any(p => p.LoginData == loginData);
        }

        // Add method to update a person's details
        public void UpdatePerson(Person person)
        {
            db.Persons.Update(person);
            db.SaveChanges();
        }

        // Add method to get all persons with a specific surname
        public List<Person> GetPersonsBySurname(string surname)
        {
            return db.Persons.Where(p => p.Surname == surname).ToList();
        }
    }
}
