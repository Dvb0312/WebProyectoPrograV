using API_Proyecto_PrograV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebProyectoPrograV.Models;
using WebProyectoPrograV.Services;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

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
                ddlDepartamento.Items.Add(new System.Web.UI.WebControls.ListItem("Todos los departamentos", ""));

                foreach (var depto in departamentos)
                {
                    ddlDepartamento.Items.Add(new System.Web.UI.WebControls.ListItem(depto.Nombre, depto.IdDepartamento.ToString()));
                }
            }
            catch (Exception)
            {
                // Si hay error, al menos mostrar la opción "Todos"
                ddlDepartamento.Items.Clear();
                ddlDepartamento.Items.Add(new System.Web.UI.WebControls.ListItem("Todos los departamentos", ""));
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

                case "ConstanciaSalarial":  // ← NUEVO
                    await GenerarConstanciaSalarial(idEmpleado);
                    break;

                    //case "Reportes":
                    // Response.Redirect($"~/EmpleadoReportes.aspx?id={idEmpleado}", false);
                    //Context.ApplicationInstance.CompleteRequest();
                    //break;
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

        /*private async Task GenerarConstanciaSalarial(int idEmpleado)
        {
            try
            {
                // ✅ DEBUGGING - 
                Response.Write($"<script>alert('Generando constancia para empleado {idEmpleado}');</script>");

                // 1. Obtener datos actuales del empleado
                var empleado = todosLosEmpleados.FirstOrDefault(e => e.IdEmpleado == idEmpleado);
                if (empleado == null)
                {
                    MostrarMensaje("Empleado no encontrado", "alert-danger");
                    return;
                }

                // 2. Crear DTO para la constancia
                var constanciaDto = new CrearConstanciaSalarialDto
                {
                    IdEmpleado = empleado.IdEmpleado,
                    IdDepartamento = empleado.IdDepartamento,
                    IdRol = empleado.IdRol,
                    PeriodoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    PeriodoFin = DateTime.Now.Date,
                    SalarioBase = empleado.Salario,
                    Bonificaciones = 0,
                    Deducciones = 0,
                    Cargo = empleado.Puesto,
                    Observaciones = $"Constancia generada automáticamente el {DateTime.Now:dd/MM/yyyy}",
                    UsuarioGeneradorId = 1
                };

                // 3. Llamar a la API para crear la constancia
                int constanciaId = await ApiService.CrearConstanciaSalarialAsync(constanciaDto);

                // 4. Redirigir a la página de constancia para generar y descargar el PDF
                Response.Redirect($"~/EmpleadoConstanciaSalarial.aspx?id={idEmpleado}&constanciaId={constanciaId}", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al generar constancia salarial: " + ex.Message, "alert-danger");
            }
        }*/


        private async Task GenerarConstanciaSalarial(int idEmpleado)
        {
            try
            {
                var empleado = todosLosEmpleados.FirstOrDefault(e => e.IdEmpleado == idEmpleado);

                // PASO 1: Confirmar que encuentra el empleado
                MostrarMensaje($"PASO 1: Empleado encontrado: {empleado?.NombreCompleto}", "alert-info");

                if (empleado == null) return;

                var constanciaDto = new CrearConstanciaSalarialDto
                {
                    IdEmpleado = empleado.IdEmpleado,
                    IdDepartamento = empleado.IdDepartamento,
                    IdRol = empleado.IdRol,
                    PeriodoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    PeriodoFin = DateTime.Now.Date,
                    SalarioBase = empleado.Salario,
                    Bonificaciones = 0,
                    Deducciones = 0,
                    Cargo = empleado.Puesto,
                    Observaciones = $"Constancia generada automáticamente el {DateTime.Now:dd/MM/yyyy}",
                    UsuarioGeneradorId = 1
                };

                // PASO 2: Antes de llamar la API
                MostrarMensaje("PASO 2: Llamando a la API...", "alert-warning");

                int constanciaId = await ApiService.CrearConstanciaSalarialAsync(constanciaDto);

                // PASO 3: Después de llamar la API
                MostrarMensaje($"PASO 3: API devolvió ID: {constanciaId}", "alert-success");

                if (constanciaId > 0)
                {
                    // PASO 4: Antes de redireccionar
                    MostrarMensaje("PASO 4: Redirigiendo...", "alert-info");

                    Response.Redirect($"~/EmpleadoConstanciaSalarial.aspx?id={idEmpleado}&constanciaId={constanciaId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"ERROR EN PASO: {ex.Message}", "alert-danger");
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