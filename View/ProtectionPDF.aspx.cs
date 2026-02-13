using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


namespace FileProtection.View
{
    public partial class ProtectionPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.BindGrid();
            }
        }
        private void BindGrid()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["conn_resgisteredASP"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand("getStudentDetails"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            grdStudents.DataSource = dt;
                            grdStudents.DataBind();
                        }
                    }
                }
            }
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document doc = new Document(pdfDoc);

                //To Export all pages
                grdStudents.AllowPaging = false;
                this.BindGrid();

                foreach (var row in grdStudents.Rows)
                {
                    foreach (var cell in ((GridViewRow)row).Cells)
                    {
                        doc.Add(new Paragraph(cell.Text));
                    }
                }

                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                using (MemoryStream output = new MemoryStream())
                {
                    string password = "pass@123";
                    PdfReader reader = new PdfReader(bytes);
                    PdfWriter encryptedWriter = new PdfWriter(output,
                        new WriterProperties().SetStandardEncryption(
                            new PdfWriter.EncryptionProperties().SetPassword(password),
                            new PdfReader.EncryptionProperties().AddStandardPermissions(PdfName.Printing)));

                    PdfDocument encryptedDoc = new PdfDocument(reader, encryptedWriter);

                    encryptedDoc.Close();

                    bytes = output.ToArray();

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

       
    }
}