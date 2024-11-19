using System.ComponentModel.DataAnnotations;

namespace GestionHonda.Models
{
    public class HondaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio")]
        public string Name { get; set; }

        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int Year { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal Price { get; set; }
    }
}
