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
    public partial class EmpleadoForm : System.Web.UI.Page
    {
        private int IdEmpleado => Request.QueryString["id"] != null ?
            Convert.ToInt32(Request.QueryString["id"]) : 0;

        private bool EsEdicion => IdEmpleado > 0;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarDatos();

                if (EsEdicion)
                {
                    await CargarEmpleado();
                    lblTitulo.Text = "Editar Empleado";
                }
                else
                {
                    lblTitulo.Text = "Nuevo Empleado";
                    // Establecer fecha actual por defecto
                    txtFechaContratacion.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }

                OcultarMensaje();
            }
        }

        private async Task CargarDatos()
        {
            try
            {
                // Cargar departamentos
                await CargarDepartamentos();

                // Cargar roles
                await CargarRoles();
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
                var departamentos = await ApiService.ObtenerDepartamentosAsync();

                ddlDepartamento.Items.Clear();
                ddlDepartamento.Items.Add(new ListItem("-- Seleccione departamento --", ""));

                foreach (var depto in departamentos)
                {
                    ddlDepartamento.Items.Add(new ListItem(depto.Nombre, depto.IdDepartamento.ToString()));
                }
            }
            catch (Exception)
            {
                ddlDepartamento.Items.Clear();
                ddlDepartamento.Items.Add(new ListItem("Error al cargar departamentos", ""));
            }
        }

        private async Task CargarRoles()
        {
            try
            {
                var roles = await ApiService.ObtenerRolesAsync();

                ddlRol.Items.Clear();
                ddlRol.Items.Add(new ListItem("-- Seleccione rol --", ""));

                foreach (var rol in roles)
                {
                    ddlRol.Items.Add(new ListItem(rol.Nombre, rol.IdRol.ToString()));
                }
            }
            catch (Exception)
            {
                ddlRol.Items.Clear();
                ddlRol.Items.Add(new ListItem("Error al cargar roles", ""));
            }
        }

        private async Task CargarEmpleado()
        {
            try
            {
                var empleado = await ApiService.ObtenerEmpleadoPorIdAsync(IdEmpleado);

                if (empleado != null)
                {
                    txtNombre.Text = empleado.Nombre ?? "";
                    txtApellido.Text = empleado.Apellido ?? "";
                    txtCorreo.Text = empleado.CorreoElectronico ?? "";
                    txtTelefono.Text = empleado.Telefono ?? "";
                    txtPuesto.Text = empleado.Puesto ?? "";
                    txtFechaContratacion.Text = empleado.FechaContratacion.ToString("yyyy-MM-dd");
                    txtSalario.Text = empleado.Salario.ToString("F2");

                    ddlDepartamento.SelectedValue = empleado.IdDepartamento.ToString();
                    ddlRol.SelectedValue = empleado.IdRol.ToString();
                    ddlEstado.SelectedValue = empleado.Estado.ToString();
                }
                else
                {
                    MostrarMensaje("No se encontró el empleado", "alert-danger");
                    Response.Redirect("~/Empleados.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el empleado: " + ex.Message, "alert-danger");
            }
        }

        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                var empleado = new EmpleadoModel
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    CorreoElectronico = string.IsNullOrWhiteSpace(txtCorreo.Text) ? null : txtCorreo.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Puesto = string.IsNullOrWhiteSpace(txtPuesto.Text) ? null : txtPuesto.Text.Trim(),
                    FechaContratacion = DateTime.Parse(txtFechaContratacion.Text),
                    Salario = decimal.Parse(txtSalario.Text),
                    IdDepartamento = int.Parse(ddlDepartamento.SelectedValue),
                    IdRol = int.Parse(ddlRol.SelectedValue),
                    Estado = byte.Parse(ddlEstado.SelectedValue)
                };

                bool exito = false;
                string mensaje = "";

                if (EsEdicion)
                {
                    exito = await ApiService.ActualizarEmpleadoAsync(IdEmpleado, empleado);
                    mensaje = exito ? "Empleado actualizado correctamente" : "No se pudo actualizar el empleado";
                }
                else
                {
                    exito = await ApiService.CrearEmpleadoAsync(empleado);
                    mensaje = exito ? "Empleado creado correctamente" : "No se pudo crear el empleado";
                }

                if (exito)
                {
                    MostrarMensaje(mensaje, "alert-success");

                    // Esperar un poco para que se vea el mensaje y luego redirigir
                    ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                        "setTimeout(function(){ window.location.href='Empleados.aspx'; }, 2000);", true);
                }
                else
                {
                    MostrarMensaje(mensaje, "alert-danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, "alert-danger");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Limpiar el formulario
            LimpiarFormulario();
            MostrarMensaje("Operación cancelada", "alert-warning");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empleados.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtPuesto.Text = "";
            txtFechaContratacion.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtSalario.Text = "";
            ddlDepartamento.SelectedIndex = 0;
            ddlRol.SelectedIndex = 0;
            ddlEstado.SelectedValue = "1";
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