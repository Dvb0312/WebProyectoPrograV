using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProyectoPrograV.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public int IdUsuario { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdRol { get; set; }
        public string Correo { get; set; }
    }
}