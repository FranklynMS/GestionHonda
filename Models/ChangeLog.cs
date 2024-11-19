using System;

namespace GestionHonda.Models
{
    public class ChangeLog
    {
        public int Id { get; set; } // Identificador único del cambio
        public string Action { get; set; } // Tipo de acción (Editado, Eliminado, etc.)
        public string ModelName { get; set; } // Nombre del modelo afectado
        public DateTime Timestamp { get; set; } // Fecha y hora del cambio
    }
}
