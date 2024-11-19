using GestionHonda.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionHonda.Controllers
{
    public class Honda : Controller
    {
        private static List<ChangeLog> _changeLogs = new List<ChangeLog>();

        private static List<HondaModel> _hondaModels = new List<HondaModel>();

      public IActionResult Index(string? searchTerm, string? sortOrder)
 {
     var filteredModels = string.IsNullOrEmpty(searchTerm)
         ? _hondaModels
         : _hondaModels.Where(m => m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

     // Ordenar según el criterio
     filteredModels = sortOrder switch
     {
         "Name" => filteredModels.OrderBy(m => m.Name).ToList(),
         "Year" => filteredModels.OrderBy(m => m.Year).ToList(),
         "Price" => filteredModels.OrderBy(m => m.Price).ToList(),
         _ => filteredModels
     };

     ViewBag.SortOrder = sortOrder; // Pasar el criterio a la vista
     return View(filteredModels);
 }

        public IActionResult Create()
        {
            return View(new HondaModel());
        }

        [HttpPost]
        public IActionResult Create(HondaModel model, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                model.ImagePath = "/uploads/" + uniqueFileName;
            }

            model.Id = _hondaModels.Count == 0 ? 1 : _hondaModels.Max(m => m.Id) + 1;
            _hondaModels.Add(model);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeLog()
        {
            return View(_changeLogs);
        }

        public IActionResult Delete(int id)
        {
            var model = _hondaModels.FirstOrDefault(m => m.Id == id);
            if (model != null)
            {
                _hondaModels.Remove(model);

                // Registrar el cambio
                _changeLogs.Add(new ChangeLog
                {
                    Id = _changeLogs.Count == 0 ? 1 : _changeLogs.Max(c => c.Id) + 1,
                    Action = "Eliminado",
                    ModelName = model.Name,
                    Timestamp = DateTime.Now
                });
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var model = _hondaModels.FirstOrDefault(m => m.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(HondaModel model, IFormFile? image)
        {
            var existing = _hondaModels.FirstOrDefault(m => m.Id == model.Id);
            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Year = model.Year;
                existing.Price = model.Price;
              

                if (image != null && image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    existing.ImagePath = "/uploads/" + uniqueFileName;
                }

                // Registrar el cambio
                _changeLogs.Add(new ChangeLog
                {
                    Id = _changeLogs.Count == 0 ? 1 : _changeLogs.Max(c => c.Id) + 1,
                    Action = "Editado",
                    ModelName = existing.Name,
                    Timestamp = DateTime.Now
                });
            }

            return RedirectToAction(nameof(Index));
        }

     
    }
}
