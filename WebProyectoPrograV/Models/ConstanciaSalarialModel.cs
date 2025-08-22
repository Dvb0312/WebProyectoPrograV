using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Proyecto_PrograV.Models
{
    public class ConstanciaSalarialModel
    {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public int IdDepartamento { get; set; }
        public int IdRol { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal Bonificaciones { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioNeto { get; set; }
        public string Cargo { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioGeneradorId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Propiedades adicionales para mostrar información relacionada
        public string NombreCompleto { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreRol { get; set; }
    }

    public class CrearConstanciaSalarialDto
    {
        public int IdEmpleado { get; set; }
        public int IdDepartamento { get; set; }
        public int IdRol { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal Bonificaciones { get; set; }
        public decimal Deducciones { get; set; }
        public string Cargo { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioGeneradorId { get; set; }
    }

}