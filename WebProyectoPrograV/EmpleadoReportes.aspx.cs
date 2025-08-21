using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebProyectoPrograV.Models;
using WebProyectoPrograV.Services;

using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants; // Necesario para StandardFonts

using System.IO;

namespace WebProyectoPrograV
{
    public partial class EmpleadoReportes : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await GenerarReporte();
            }
        }

        private async Task GenerarReporte()
        {
            try
            {
                // Debug: Verificar parámetro
                string idParam = Request.QueryString["id"];
                if (string.IsNullOrEmpty(idParam))
                {
                    throw new Exception("No se proporcionó ID de empleado");
                }

                int idEmpleado = int.Parse(idParam);

                // Debug: Obtener empleado
                var empleados = await ApiService.ObtenerEmpleadosAsync();
                var empleado = empleados.FirstOrDefault(e => e.IdEmpleado == idEmpleado);

                if (empleado == null)
                {
                    throw new Exception("Empleado no encontrado");
                }

                // Debug: Obtener departamento
                var departamentos = await ApiService.ObtenerDepartamentosAsync();
                var departamento = departamentos.FirstOrDefault(d => d.IdDepartamento == empleado.IdDepartamento);
                string nombreDepartamento = departamento?.Nombre ?? "Sin departamento";

                // Debug: Preparar datos seguros (sin nulos)
                string nombre = empleado.NombreCompleto ?? "Sin nombre";
                string puesto = empleado.Puesto ?? "Sin puesto";
                string email = empleado.CorreoElectronico ?? "Sin email";
                string telefono = empleado.Telefono ?? "Sin telefono";
                string estado = empleado.EstadoTexto ?? "Sin estado";
                string salario = empleado.Salario.ToString();

                // Debug: Crear directorio
                string reportesPath = Server.MapPath("~/Reportes/");
                if (!Directory.Exists(reportesPath))
                {
                    Directory.CreateDirectory(reportesPath);
                }

                // Debug: Crear archivo
                string fileName = $"Empleado_{empleado.IdEmpleado}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(reportesPath, fileName);

                // Debug: Mensaje antes de crear PDF
                System.Diagnostics.Debug.WriteLine("Iniciando creación de PDF...");

                // Crear PDF con manejo de errores específico
                try
                {
                    using (var writer = new PdfWriter(filePath))
                    {
                        System.Diagnostics.Debug.WriteLine("PdfWriter creado...");

                        using (var pdf = new PdfDocument(writer))
                        {
                            System.Diagnostics.Debug.WriteLine("PdfDocument creado...");

                            var document = new Document(pdf);
                            System.Diagnostics.Debug.WriteLine("Document creado...");

                            // Agregar contenido muy básico primero
                            document.Add(new Paragraph("REPORTE DE EMPLEADO"));
                            System.Diagnostics.Debug.WriteLine("Título agregado...");

                            document.Add(new Paragraph("ID: " + empleado.IdEmpleado.ToString()));
                            document.Add(new Paragraph("Nombre: " + nombre));
                            document.Add(new Paragraph("Puesto: " + puesto));
                            document.Add(new Paragraph("Departamento: " + nombreDepartamento));
                            document.Add(new Paragraph("Email: " + email));
                            document.Add(new Paragraph("Telefono: " + telefono));
                            document.Add(new Paragraph("Salario: " + salario));
                            document.Add(new Paragraph("Estado: " + estado));
                            document.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));

                            System.Diagnostics.Debug.WriteLine("Contenido agregado...");

                            document.Close();
                            System.Diagnostics.Debug.WriteLine("Document cerrado...");
                        }
                        System.Diagnostics.Debug.WriteLine("PdfDocument cerrado...");
                    }
                    System.Diagnostics.Debug.WriteLine("PdfWriter cerrado...");
                }
                catch (Exception pdfEx)
                {
                    throw new Exception($"Error específico al crear PDF: {pdfEx.Message}", pdfEx);
                }

                // Debug: Verificar que el archivo existe
                if (!File.Exists(filePath))
                {
                    throw new Exception("El archivo PDF no se creó correctamente");
                }

                System.Diagnostics.Debug.WriteLine("PDF creado exitosamente, iniciando descarga...");

                // Descargar archivo
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.TransmitFile(filePath);

                // NO usar Response.End() - causa ThreadAbortException
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Log detallado del error
                string errorDetail = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorDetail += $" | Inner: {ex.InnerException.Message}";
                }

                System.Diagnostics.Debug.WriteLine($"Error completo: {errorDetail}");

                // Mostrar error al usuario
                ClientScript.RegisterStartupScript(this.GetType(), "showError",
                    $"alert('Error al generar reporte: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empleados.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}