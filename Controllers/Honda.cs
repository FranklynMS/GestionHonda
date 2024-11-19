using GestionHonda.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionHonda.Controllers
{
    public class Honda : Controller
    {
        private static List<HondaModel> _hondaModels = new List<HondaModel>
        {
            
        };

        public IActionResult Index(string? searchTerm)
        {
           
            var filteredModels = string.IsNullOrEmpty(searchTerm)
                ? _hondaModels
                : _hondaModels.Where(m => m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(filteredModels);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(HondaModel model)
        {
            if (!ModelState.IsValid)
            {
               
                return View(model);
            }

       
            model.Id = _hondaModels.Count == 0 ? 1 : _hondaModels.Max(m => m.Id) + 1;
            _hondaModels.Add(model);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var model = _hondaModels.FirstOrDefault(m => m.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(HondaModel model)
        {
            var existing = _hondaModels.FirstOrDefault(m => m.Id == model.Id);
            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Year = model.Year;
                existing.Price = model.Price;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var model = _hondaModels.FirstOrDefault(m => m.Id == id);
            if (model != null)
            {
                _hondaModels.Remove(model);
            }
            return RedirectToAction(nameof(Index));
        }

  

    }
}

