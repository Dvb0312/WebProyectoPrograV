using API_Proyecto_PrograV.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System;
using System.IO;
using System.Threading.Tasks;
using WebProyectoPrograV.Services;

namespace WebProyectoPrograV
{
    public partial class EmpleadoConstanciaSalarial : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idEmpleado = int.Parse(Request.QueryString["id"]);
                int constanciaId = int.Parse(Request.QueryString["constanciaId"]);

                var constancia = await ApiService.ObtenerConstanciaSalarialPorIdAsync(constanciaId);

                if (constancia != null)
                {
                    GenerarPdf(constancia);
                }
                else
                {
                    Response.Write("Error: no se encontró la constancia.");
                }
            }
        }

        private void GenerarPdf(ConstanciaSalarialModel constancia)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Constancia_{constancia.Id}.pdf");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(ms))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document doc = new Document(pdf);

                        // Título
                        Paragraph titulo = new Paragraph("CONSTANCIA SALARIAL")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER);
                        doc.Add(titulo);

                        doc.Add(new Paragraph("\n"));

                        // Información del empleado
                        doc.Add(new Paragraph($"Nombre: {constancia.NombreCompleto}"));
                        doc.Add(new Paragraph($"Departamento: {constancia.NombreDepartamento}"));
                        doc.Add(new Paragraph($"Cargo: {constancia.Cargo}"));
                        doc.Add(new Paragraph($"Periodo: {constancia.PeriodoInicio:dd/MM/yyyy} - {constancia.PeriodoFin:dd/MM/yyyy}"));

                        doc.Add(new Paragraph("\n"));

                        // Salarios
                        doc.Add(new Paragraph($"Salario Base: {constancia.SalarioBase:C}"));
                        doc.Add(new Paragraph($"Bonificaciones: {constancia.Bonificaciones:C}"));
                        doc.Add(new Paragraph($"Deducciones: {constancia.Deducciones:C}"));
                        doc.Add(new Paragraph($"Salario Neto: {constancia.SalarioNeto:C}"));

                        doc.Add(new Paragraph("\n"));
                        doc.Add(new Paragraph($"Observaciones: {constancia.Observaciones}"));

                        doc.Add(new Paragraph("\n\n"));
                        doc.Add(new Paragraph("______________________________").SetTextAlignment(TextAlignment.CENTER));
                        doc.Add(new Paragraph("Firma Autorizada").SetTextAlignment(TextAlignment.CENTER));

                        doc.Close();
                    }
                }

                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
    }
}
