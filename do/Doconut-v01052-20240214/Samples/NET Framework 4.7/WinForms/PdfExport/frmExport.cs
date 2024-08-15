using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;


using DotnetDaddy.DocumentViewer;

// IMPORTANT NOTE:  Using OpenDocumentService method in [production] requires a "Distribution" license 

namespace PdfExport
{
    public partial class frmExport : Form
    {
        public frmExport()
        {
            InitializeComponent();
        }

        private string input_file = Path.Combine(Application.StartupPath, "Sample.ppt");
        private string output_file = Path.Combine(Application.StartupPath, "Sample.ppt.pdf");

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;

            try
            {
                using (var docViewer = new DocViewer())
                {
                    // IMPORTANT NOTE:  Using OpenDocumentService method in [production] requires a "Distribution" license 

                    var lic = new XmlDocument();
                    lic.LoadXml("<License><Owner>Doconut Trial - 60 Days - 2022</Owner><Key>@KS@qYqbu65rqHLBh65ql3PI88NyzbB4yCb96dV0uUA//9k=</Key><Type>@KS@viyehl/gncUz6h+aIvz8Hg==</Type><Domain>@KS@31zDJ3WwXdSPZGKYyNWzoA==</Domain><Annotation>@KS@40kQXx1bIn6XOEo2915pSA==</Annotation><Search>@KS@gX4SKEOC2w0r9q4VDh+inQ==</Search><Updates>@KS@vd8NQateAA2yzyOQfnWCYg==</Updates><EULA>Trial version. Limited use.</EULA></License>");
                    DocViewer.DoconutLicense(lic);

                    var success = docViewer.OpenDocumentService(input_file);

                    // If you want to load Annotations

                    // XmlDocument xmlDoc = new XmlDocument();
                    // xmlDoc.Load(@"C:\Annotations.xml");
                    // docViewer.LoadAnnotationXML(xmlDoc);

                    if (success)
                    {
                        // export pdf
                        var fileBytes = docViewer.ExportToPdf();

                        // If you want to get a thumbnail for any page
                        // var bmp = docViewer.GetThumbnail(1, 0, 50, false);
                        // Save the bitmap as jpeg or png

                        File.Delete(output_file);

                        using (var fs = new FileStream(output_file, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(fileBytes, 0, fileBytes.Length);
                        }

                        // open the resulting pdf

                        System.Diagnostics.Process.Start(output_file);
                    }
                    else
                    {
                        throw new Exception("Error opening the document");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            btnExport.Enabled = true;
        }
    }
}
