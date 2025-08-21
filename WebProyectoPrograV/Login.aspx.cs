using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Login.aspx.cs
using System.Threading.Tasks;
using WebProyectoPrograV.Models;
using WebProyectoPrograV.Services;


namespace WebProyectoPrograV
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiar cualquier sesión existente
                Session.Clear();
            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                lblMensaje.Visible = false;

                var loginResult = await ApiService.LoginAsync(txtCorreo.Text.Trim(), txtPassword.Text);

                if (loginResult != null)
                {
                    // Guardar información del usuario en la sesión
                    Session["IdUsuario"] = loginResult.IdUsuario;
                    Session["IdEmpleado"] = loginResult.IdEmpleado;
                    Session["IdRol"] = loginResult.IdRol;
                    Session["Correo"] = loginResult.Correo;
                    Session["Token"] = loginResult.Token;

                    // Redirigir al dashboard SIN abortar el hilo
                    Response.Redirect("~/Dashboard.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    MostrarMensaje("Credenciales inválidas. Por favor, verifique su correo y contraseña.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al intentar iniciar sesión: " + ex.Message);
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Visible = true;
        }
    }
}