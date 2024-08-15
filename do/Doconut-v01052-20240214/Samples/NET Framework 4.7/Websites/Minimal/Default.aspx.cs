using System;
using System.IO;
using System.Web.UI;


// ADD MISSING DLL REFERENCES  DocumentViewer.dll, DocumentFormats.dll, DocumentConfig.dll
using DotnetDaddy.DocumentConfig;


public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // refer documentation for various config options for all document types
            var config = new WordConfig { ConvertPdf = true };
            config.PdfConfig.ExtractHyperlinks = true;

            var fileName = "Sample.doc";

            var filePath = Path.Combine(Server.MapPath("~/files"), fileName);
            var bytes = File.ReadAllBytes(filePath);

            // there are many overloads for OpenDocument method, see documentation pdf.
            var viewToken = ctlDoc.OpenDocument(bytes, ".DOC", config);

            if (viewToken.Length == 0)
            {
                throw new Exception(ctlDoc.InternalError);
            }
        }
    }
}
