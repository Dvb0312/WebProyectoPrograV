using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProyectoPrograV.Models
{
    public class EmpleadoModel
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Puesto { get; set; }
        public DateTime FechaContratacion { get; set; }
        public decimal Salario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public int IdDepartamento { get; set; }
        public int IdRol { get; set; }
        public byte Estado { get; set; }

        // Propiedades adicionales para mostrar nombres
        public string NombreDepartamento { get; set; }
        public string NombreRol { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string EstadoTexto => Estado == 1 ? "Activo" : "Inactivo";
    }
}