using System;
using System.Web.UI;
using System.Xml;
using System.IO;
using System.Drawing;

using DocumentConfig.Viewer.Annotations;


public partial class Annotations : System.Web.UI.Page
{
    private string documentToView = "~/files/Sample.ppt";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string token = ctlDoc.OpenDocument(Server.MapPath((documentToView)));

            if (token.Length == 0)
            {
                ShowInternalError();
            }
        }
    }

    protected void btnGetAnnData_Click(object sender, EventArgs e)
    {
        // This is how you get annotation data for this document (can be saved in db or so)
        string annotations = ctlDoc.GetAnnotationData();
        if (annotations.Length > 0)
        {
            Response.Write(annotations);
            Response.End();
        }
        else
        {
            Response.Write("No annotation data available");
            Response.End();
        }
    }

    protected void btnExportXML_Click(object sender, EventArgs e)
    {
        XmlDocument annXML = ctlDoc.GetAnnotationXML();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=Annotations.xml");
        Response.ContentType = "application/octet-stream";
        annXML.Save(Response.OutputStream);
        Response.End();
    }

    protected void btnCodeAnn_Click(object sender, EventArgs e)
    {
        using (var firstPage = ctlDoc.GetThumbnail(1, 0, 100, false))
        {
            var pageWidth = firstPage.Width;
            var pageHeight = firstPage.Height;

            using (var annMgr = new AnnotationManager(pageWidth, pageHeight))
            {

                var stampAnn = new StampAnnotation(1, new Rectangle(200, 200, 700, 250), "confidential", 28, 4, Color.Maroon)
                {
                    Opacity = 50,
                    Rotate = -25,
                };

                annMgr.Add(stampAnn);

                var rectAnn = new RectangleAnnotation(2, new Rectangle(200, 100, 700, 250), 4, Color.Maroon, Color.Yellow)
                {
                    Opacity = 50
                };

                annMgr.Add(rectAnn);

                var circleAnn = new CircleAnnotation(1, new Rectangle(500, 500, 300, 750), 4, Color.Red, Color.Yellow)
                {
                    Opacity = 50
                };

                annMgr.Add(circleAnn);

                var ellipseAnn = new EllipseAnnotation(1, new Rectangle(600, 700, 750, 300), 2, Color.Green, Color.Blue)
                {
                    Opacity = 50
                };

                annMgr.Add(ellipseAnn);

                var noteAnn = new NoteAnnotation(1, new Rectangle(600, 1200, 300, 50), "Hello World!", Color.Blue)
                {
                    BackColor = Color.Yellow,
                    Opacity = 30,
                    ShowBorder = true,
                    BorderColor = Color.Black,
                    Rotate = -45
                };

                annMgr.Add(noteAnn);


                var imgAnn = new ImageAnnotation(2, new Rectangle(600, 700, 750, 300), "http://www.google.com/images/srpr/logo11w.png")
                {
                    Opacity = 50
                };

                annMgr.Add(imgAnn);

                var lineAnn = new LineAnnotation(2, new Rectangle(400, 500, 100, 500), 8, Color.Green, false)
                {
                };

                annMgr.Add(lineAnn);

                var arrowAnn = new ArrowAnnotation(2, new Rectangle(400, 500, 100, 500), 8, Color.Brown, ArrowDirection.SW)
                {
                };

                annMgr.Add(arrowAnn);

                var triangleAnn = new TriangleAnnotation(2, new Rectangle(600, 800, 300, 300), 8, Color.Pink)
                {
                };

                annMgr.Add(triangleAnn);

                ctlDoc.OpenDocument(Server.MapPath(documentToView));
                ctlDoc.LoadAnnotationXML(annMgr.GetAnnotationXml());
            }
        }
    }

    protected void SavePDF_Click(object sender, EventArgs e)
    {
        var exportFolder = Server.MapPath("~/Export");
        var exportPdf = Path.Combine(exportFolder, Guid.NewGuid().ToString() + ".pdf");

        ctlDoc.ExportAnnotationsToPdf(exportPdf);

        if (File.Exists(exportPdf))
        {
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=Doconut-Annotations.pdf");
            Response.ContentType = "application/octet-stream";
            Response.TransmitFile(exportPdf);
            Response.End();
        }
        else
        {
            ShowInternalError();
        }
    }

    private void ShowInternalError()
    {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showalert",
                       "$(window).load(function () {alert('Error: " + ctlDoc.InternalError + "');});", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        // Upload file, need WRITE access to files folder

        string uploadedXMLFile = Path.Combine(Server.MapPath("~/files"), DateTime.Now.Ticks.ToString() + ".xml");
        uploadXml.PostedFile.SaveAs(uploadedXMLFile);

        XmlDocument uploadedXML = new XmlDocument();
        uploadedXML.LoadXml(File.ReadAllText(uploadedXMLFile));

        // Use below code only if you need to merge existing and new Annotations

        // START XML MERGE CODE

        XmlDocument existingXML = ctlDoc.GetAnnotationXML();
        XmlNodeList xnList = existingXML.SelectNodes("/Pages/Page");
        bool importNewNodes = false;

        if (xnList.Count == 0)
        {
            existingXML = uploadedXML;
        }
        else
        {
            foreach (XmlNode xn in xnList)
            {
                int pageNumber = Convert.ToInt32(xn.Attributes[0].Value);

                XmlNodeList xnListUpload = uploadedXML.SelectNodes("/Pages/Page[@Number='" + pageNumber.ToString() + "']/Annotations");
                foreach (XmlNode xnAnn in xnListUpload)
                {
                    xn.FirstChild.InnerXml = xn.FirstChild.InnerXml + xnAnn.InnerXml;
                    xnAnn.InnerXml = "";
                    importNewNodes = true;
                }
            }
        }

        if (importNewNodes)
        {
            XmlNodeList xnListUploadNew = uploadedXML.SelectNodes("/Pages/Page");
            foreach (XmlNode xnAnnNew in xnListUploadNew)
            {
                if (xnAnnNew.InnerText.Trim().Length > 0)
                {
                    XmlNode xChildNode = existingXML.ImportNode(xnAnnNew, true);
                    existingXML.DocumentElement.AppendChild(xChildNode);
                }
            }
        }

        // END XML MERGE CODE



        ctlDoc.OpenDocument(Server.MapPath((documentToView)));
        ctlDoc.LoadAnnotationXML(existingXML);
    }
}