using System.ComponentModel.DataAnnotations;

namespace GestionHonda.Models
{
    public class HondaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Suministrar Un Nombre")]
        public string Name { get; set; }

        [Range(1900, 2100, ErrorMessage = "Solo Se Aceptan años desde 1900 hasta 2100")]
        public int Year { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Tiene que Insertar Un Valor Positivo")]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; } 

        [Required(ErrorMessage = "Debe suministrar un color")]
        public string Color { get; set; }

       [Required(ErrorMessage = "Debe indicar el tipo de motor")]
        public string TipoMotor { get; set; }

        [Required(ErrorMessage = "Debe especificar la transmisión")]
        public string Transmision { get; set; }
    }
}
