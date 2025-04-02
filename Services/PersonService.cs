using MySecondMVC.Models;
using MySecondMVC.Repositories;

namespace MySecondMVC.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;

        public PersonService(IPersonRepository repo)
        {
            _repo = repo;
        }

        public List<Person> GetAll() => _repo.GetAll();

        public List<Person> GetMales() => _repo.GetAll().Where(p => p.Gender == "Male").ToList();

        public Person? GetOldest() => _repo.GetAll().OrderByDescending(p => p.Age).FirstOrDefault();

        public List<string> GetFullNames() => _repo.GetAll().Select(p => p.FullName).ToList();

        public List<Person> FilterByBirthYear(int year, string filterType)
        {
            return filterType switch
            {
                "equal" => _repo.GetAll().Where(p => p.DateOfBirth.Year == year).ToList(),
                "before" => _repo.GetAll().Where(p => p.DateOfBirth.Year < year).ToList(),
                "after" => _repo.GetAll().Where(p => p.DateOfBirth.Year > year).ToList(),
                _ => new List<Person>()
            };
        }

        public Person? GetById(Guid id) => _repo.GetById(id);

        public void Add(Person person) => _repo.Add(person);

        public void Update(Person person) => _repo.Update(person);

        public void Delete(Guid id) => _repo.Delete(id);
    }
}