using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProyectoPrograV.Models
{
    public class PeticionModel
    {
        public int IdPeticion { get; set; }
        public int IdEmpleado { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public byte Estado { get; set; }
        public string DatosJson { get; set; }

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
                        return "Procesada";
                    case 2:
                        return "Rechazada";
                    default:
                        return "Desconocido";
                }
            }
        }

    }
}