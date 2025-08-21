using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebProyectoPrograV
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar si el usuario está autenticado
            if (Session["IdUsuario"] == null)
            {
                // Redirigir al login si no está autenticado
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Mostrar información del usuario
                string correo = Session["Correo"]?.ToString() ?? "";
                lblUsuarioInfo.Text = $"Bienvenido: {correo}";

                // Ocultar enlaces según el rol si es necesario
                ConfigurarMenuSegunRol();
            }
        }

        private void ConfigurarMenuSegunRol()
        {
            if (Session["IdRol"] != null)
            {
                int idRol = Convert.ToInt32(Session["IdRol"]);

                // Ejemplo: Solo administradores pueden ver todo
                // Aquí puedes personalizar la navegación según el rol
                switch (idRol)
                {
                    case 1: // Administrador
                        // Todos los enlaces visibles
                        break;
                    case 2: // Gerente
                        // Acceso limitado
                        break;
                    case 3: // Empleado
                        // Solo dashboard y sus propias solicitudes
                        break;
                    case 4: // RRHH
                        // Gestión de vacaciones y peticiones
                        break;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpiar sesión
            Session.Clear();
            Session.Abandon();

            // Redirigir al login
            Response.Redirect("~/Login.aspx");
        }
    }
}