using Microsoft.AspNetCore.Mvc;
using MySecondMVC.Models;
using MySecondMVC.Services;

namespace MySecondMVC.Controllers
{
    [Route("NashTech/[controller]/[action]")]
    public class RookiesController : Controller
    {
        private readonly IPersonService _service;

        public RookiesController(IPersonService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var people = _service.GetAll();
            return View(people);
        }

        public IActionResult Details(Guid id)
        {
            var person = _service.GetById(id);
            if (person == null) return NotFound();
            return View(person);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (!ModelState.IsValid) return View(person);

            person.Id = Guid.NewGuid();
            _service.Add(person);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            var person = _service.GetById(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost]
        public IActionResult Edit(Person person)
        {
            if (!ModelState.IsValid) return View(person);

            _service.Update(person);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            var person = _service.GetById(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var person = _service.GetById(id);
            if (person == null) return NotFound();

            _service.Delete(id);
            TempData["Message"] = $"Person {person.FullName} was removed from the list successfully!";
            return RedirectToAction("DeleteConfirmation");
        }

        public IActionResult DeleteConfirmation()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        public IActionResult GetMales() => View("FilteredList", _service.GetMales());

        public IActionResult GetOldest()
        {
            var oldest = _service.GetOldest();
            return View("Details", oldest);
        }

        public IActionResult GetFullNames()
        {
            var names = _service.GetFullNames();
            return View("FullNames", names);
        }

        public IActionResult FilterByBirthYear(int year, string filterType)
        {
            var result = _service.FilterByBirthYear(year, filterType);
            return View("FilteredList", result);
        }

        public IActionResult ExportToExcel()
        {
            var people = _service.GetAll();
            var csv = "FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated\n" +
                      string.Join("\n", people.Select(p => $"{p.FirstName},{p.LastName},{p.Gender},{p.DateOfBirth:yyyy-MM-dd},{p.PhoneNumber},{p.BirthPlace},{p.IsGraduated}"));
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "people.csv");
        }
    }
}