using GestionHonda.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionHonda.Controllers
{
    public class Honda : Controller
    {
        private static List<HondaModel> _hondaModels = new List<HondaModel>();

        public IActionResult Index(string? searchTerm)
        {
            // Si no hay búsqueda, mostrar todos los modelos
            var filteredModels = string.IsNullOrEmpty(searchTerm)
                ? _hondaModels
                : _hondaModels.Where(m => m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.SearchTerm = searchTerm; // Pasar el término de búsqueda a la vista
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
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
