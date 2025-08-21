using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProyectoPrograV.Models
{
    public class VacacionModel
    {
        public int IdVacacion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int DiasSolicitados { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte Estado { get; set; }
        public string Observaciones { get; set; }

        // Propiedades adicionales
        public string NombreEmpleado { get; set; }
        public string EstadoTexto
        {
            get
            {
                switch (Estado)
                {
                    case 0:
                        return "Pendiente";
                    case 1:
                        return "Aprobada";
                    case 2:
                        return "Rechazada";
                    default:
                        return "Desconocido";
                }
            }
        }
    }
}
