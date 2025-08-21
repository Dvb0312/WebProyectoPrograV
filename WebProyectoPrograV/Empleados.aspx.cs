using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using WebProyectoPrograV.Models;
using WebProyectoPrograV.Services;


namespace WebProyectoPrograV
{
    public partial class Empleados : System.Web.UI.Page
    {
        private List<EmpleadoModel> todosLosEmpleados = new List<EmpleadoModel>();
        private List<DepartamentoModel> departamentos = new List<DepartamentoModel>();
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarDatos();
            }
        }

        private async Task CargarDatos()
        {
            try
            {
                // Cargar departamentos para el filtro
                await CargarDepartamentos();

                // Cargar empleados
                await CargarEmpleados();

                OcultarMensaje();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar los datos: " + ex.Message, "alert-danger");
            }
        }

        private async Task CargarDepartamentos()
        {
            try
            {
                departamentos = await ApiService.ObtenerDepartamentosAsync();

                ddlDepartamento.Items.Clear();
                ddlDepartamento.Items.Add(new ListItem("Todos los departamentos", ""));

                foreach (var depto in departamentos)
                {
                    ddlDepartamento.Items.Add(new ListItem(depto.Nombre, depto.IdDepartamento.ToString()));
                }
            }
            catch (Exception)
            {
                // Si hay error, al menos mostrar la opción "Todos"
                ddlDepartamento.Items.Clear();
                ddlDepartamento.Items.Add(new ListItem("Todos los departamentos", ""));
            }
        }

        private async Task CargarEmpleados()
        {
            try
            {
                todosLosEmpleados = await ApiService.ObtenerEmpleadosAsync();

                // Enriquecer datos con nombres de departamento
                foreach (var empleado in todosLosEmpleados)
                {
                    var depto = departamentos.FirstOrDefault(d => d.IdDepartamento == empleado.IdDepartamento);
                    empleado.NombreDepartamento = depto?.Nombre ?? "Sin departamento";
                }

                AplicarFiltros();

                lblTotalRegistros.Text = todosLosEmpleados.Count.ToString();
            }
            catch (Exception)
            {
                gvEmpleados.DataSource = null;
                gvEmpleados.DataBind();
                lblTotalRegistros.Text = "0";
            }
        }

        private void AplicarFiltros()
        {
            var empleadosFiltrados = todosLosEmpleados.AsEnumerable();

            // Filtro por departamento
            if (!string.IsNullOrEmpty(ddlDepartamento.SelectedValue))
            {
                int idDepartamento = int.Parse(ddlDepartamento.SelectedValue);
                empleadosFiltrados = empleadosFiltrados.Where(e => e.IdDepartamento == idDepartamento);
            }

            // Filtro por estado
            if (!string.IsNullOrEmpty(ddlEstado.SelectedValue))
            {
                byte estado = byte.Parse(ddlEstado.SelectedValue);
                empleadosFiltrados = empleadosFiltrados.Where(e => e.Estado == estado);
            }

            // Filtro por nombre
            if (!string.IsNullOrEmpty(txtBuscar.Text.Trim()))
            {
                string buscar = txtBuscar.Text.Trim().ToLower();
                empleadosFiltrados = empleadosFiltrados.Where(e =>
                    (e.Nombre?.ToLower().Contains(buscar) ?? false) ||
                    (e.Apellido?.ToLower().Contains(buscar) ?? false) ||
                    (e.NombreCompleto?.ToLower().Contains(buscar) ?? false));
            }

            var listaFiltrada = empleadosFiltrados.ToList();

            gvEmpleados.DataSource = listaFiltrada;
            gvEmpleados.DataBind();

            lblTotalRegistros.Text = listaFiltrada.Count.ToString();
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            await CargarDatos();
            MostrarMensaje("Lista de empleados actualizada", "alert-success");
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnNuevoEmpleado_Click(object sender, EventArgs e)
        {
            // ✅ Evita ThreadAbortException usando false + CompleteRequest
            Response.Redirect("~/EmpleadoForm.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected async void gvEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idEmpleado = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Editar":
                    // ✅ Corrección
                    Response.Redirect($"~/EmpleadoForm.aspx?id={idEmpleado}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    break;

                case "Eliminar":
                    await EliminarEmpleado(idEmpleado);
                    break;

                case "Reportes":
                    Response.Redirect($"~/EmpleadoReportes.aspx?id={idEmpleado}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    break;
            }
        }

        private async Task EliminarEmpleado(int idEmpleado)
        {
            try
            {
                bool eliminado = await ApiService.EliminarEmpleadoAsync(idEmpleado);

                if (eliminado)
                {
                    MostrarMensaje("Empleado eliminado correctamente", "alert-success");
                    await CargarEmpleados();
                }
                else
                {
                    MostrarMensaje("No se pudo eliminar el empleado", "alert-danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar empleado: " + ex.Message, "alert-danger");
            }
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