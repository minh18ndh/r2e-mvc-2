using Microsoft.AspNetCore.Mvc;
using MySecondMVC.Models;

namespace MySecondMVC.Controllers
{
    [Route("NashTech/[controller]/[action]")]
    public class RookiesController : Controller
    {
        private static List<Person> people = new List<Person>
        {
            new Person { FirstName = "Minh", LastName = "Nguyen", Gender = "Male", DateOfBirth = new DateOnly(2003, 6, 11), PhoneNumber = "0913234848", BirthPlace = "Vietnam", IsGraduated = true },
            new Person { FirstName = "Van", LastName = "Vu", Gender = "Female", DateOfBirth = new DateOnly(1999, 7, 15), PhoneNumber = "0494848743", BirthPlace = "Czech", IsGraduated = false },
            new Person { FirstName = "Toan", LastName = "Le", Gender = "Male", DateOfBirth = new DateOnly(1997, 4, 25), PhoneNumber = "038994384", BirthPlace = "Poland", IsGraduated = false },
            new Person { FirstName = "Ngoc", LastName = "Tran", Gender = "Female", DateOfBirth = new DateOnly(2000, 11, 6), PhoneNumber = "01388844939", BirthPlace = "Thailand", IsGraduated = true }
        };

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetMales()
        {
            var males = people.Where(p => p.Gender == "Male").ToList();
            return Json(males);
        }

        [HttpGet]
        public IActionResult GetOldest()
        {
            var oldest = people.OrderByDescending(p => p.Age).FirstOrDefault();
            return Json(oldest);
        }

        [HttpGet]
        public IActionResult GetFullNames()
        {
            var fullNames = people.Select(p => p.FullName).ToList();
            return Json(fullNames);
        }

        [HttpGet]
        public IActionResult FilterByBirthYear(int year, string filterType)
        {
            Console.WriteLine($"Year: {year}, filterType: {filterType}");

            List<Person> result;
            switch (filterType)
            {
                case "equal":
                    result = people.Where(p => p.DateOfBirth.Year == year).ToList();
                    break;
                case "before":
                    result = people.Where(p => p.DateOfBirth.Year < year).ToList();
                    break;
                case "after":
                    result = people.Where(p => p.DateOfBirth.Year > year).ToList();
                    break;
                default:
                    return BadRequest("Invalid filterType parameter!");
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            var csv = "FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated\n" +
                      string.Join("\n", people.Select(p => $"{p.FirstName},{p.LastName},{p.Gender},{p.DateOfBirth:yyyy-MM-dd},{p.PhoneNumber},{p.BirthPlace},{p.IsGraduated}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "people.csv");
        }
    }
}