using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Threading.Tasks;
using WebProyectoPrograV.Services;

namespace WebProyectoPrograV
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarDashboard();
            }
        }

        private async Task CargarDashboard()
        {
            try
            {
                await CargarEstadisticas();
                await CargarUltimosEmpleados();
                OcultarMensaje();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el dashboard: " + ex.Message, "alert-danger");
            }
        }

        private async Task CargarEstadisticas()
        {
            try
            {
                var empleados = await ApiService.ObtenerEmpleadosAsync();
                lblTotalEmpleados.Text = empleados.Count.ToString();

                var departamentos = await ApiService.ObtenerDepartamentosAsync();
                lblDepartamentos.Text = departamentos.Count.ToString();

                var vacaciones = await ApiService.ObtenerVacacionesAsync();
                lblVacacionesPendientes.Text = vacaciones.Count(v => v.Estado == 0).ToString();

                var peticiones = await ApiService.ObtenerPeticionesAsync(estado: 0);
                lblPeticionesPendientes.Text = peticiones.Count.ToString();
            }
            catch
            {
                lblTotalEmpleados.Text = "0";
                lblDepartamentos.Text = "0";
                lblVacacionesPendientes.Text = "0";
                lblPeticionesPendientes.Text = "0";
            }
        }

        private async Task CargarUltimosEmpleados()
        {
            try
            {
                var empleados = await ApiService.ObtenerEmpleadosAsync();
                var ultimosEmpleados = empleados.OrderByDescending(e => e.FechaContratacion).Take(5).ToList();

                gvUltimosEmpleados.DataSource = ultimosEmpleados;
                gvUltimosEmpleados.DataBind();
            }
            catch
            {
                gvUltimosEmpleados.DataSource = null;
                gvUltimosEmpleados.DataBind();
            }
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            await CargarDashboard();
            MostrarMensaje("Dashboard actualizado correctamente", "alert-success");
        }

        protected void btnVerEmpleados_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empleados.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnNuevoEmpleado_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EmpleadoForm.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnVerVacaciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vacaciones.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnVerPeticiones_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Peticiones.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void MostrarMensaje(string mensaje, string cssClass)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"alert {cssClass}";
            lblMensaje.Visible = true;
        }

        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
        }
    }
}