using System;
using System.IO;
using System.Text;
using System.Web.Mvc;


/* Please copy the 3 DLLs (from Asp.Net zip) to the bin folder and then open solution. */

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;



namespace Doconut.Viewer.PdfJs.Controllers
{
    public class PdfJsController : Controller
    {
        // GET: PdfJS
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            var isSavedSuccessfully = true;
            var fName = "";

            try
            {
                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];

                    if (null == file)
                        continue;

                    if (file.ContentLength <= 0) continue;

                    // check for any malicious file types
                    var invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                    var fileExtension = new FileInfo(file.FileName).Extension.ToUpper();

                    if (invalidFiles.IndexOf(fileExtension, StringComparison.Ordinal) > -1)
                    {
                        throw new Exception("Invalid file extension");
                    }

                    fName = DateTime.Now.ToShortDateString().Replace("/", "-") + "--" + file.FileName;

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        fName = fName.Replace(c, '-');
                    }

                    // Remove any special characters
                    fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");

                    var filePath = Path.Combine(Server.MapPath(@"~\Content\files"), fName);

                    file.SaveAs(filePath);

                    // If file is not PDF then first convert it and then return as PDF

                    if(new FileInfo(filePath).Extension.ToUpper() != ".PDF")
                    {
                        isSavedSuccessfully = ConvertPdf(filePath);
                        
                        if(isSavedSuccessfully)
                        {
                            System.IO.File.Delete(filePath);
                            fName += ".pdf";
                        }
                    }
                }

            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            // Return just the file name
            return Content(isSavedSuccessfully ? fName : "");
        }

        private bool ConvertPdf(string filePath)
        {
            var viewer = new DocViewer
            {
                ID = "ctlDoc",  // ID is important and required.
                DebugMode = true,
                TimeOut = 5   // close after export automatically
            };

            BaseConfig config = null;

            switch (new FileInfo(filePath).Extension.ToUpper())
            {
                case ".DWG":
                case ".DXF":
                case ".DGN":
                    config = new CadConfig { DefaultRender = true, ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                    break;
                case ".DOC":
                case ".DOCX":
                case ".ODT":
                case ".RTF":
                    config = new WordConfig { ConvertPdf = true, }; 
                    break;
                case ".TXT":
                    config = new WordConfig { PaperSize = DocPaperSize.A4, FileEncoding = Encoding.UTF8 };
                    break;
                case ".EML":
                case ".MSG":
                    config = new EmailConfig { EmailEncoding = Encoding.UTF8, ConvertHtml = false };
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                case ".CSV":
                    config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, PaperSize = ExcelPaperSize.PaperA3, DocumentCulture = "en-US", AutoFitContents = true };
                    break;
                case ".PPT":
                case ".PPTX":
                case ".ODP":
                    config = new PptConfig();
                    break;
                case ".PDF":
                    config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = false }; 
                    break;
                case ".BMP":
                case ".CDR":
                case ".CMX":
                case ".DCM":
                case ".DNG":
                case ".EPS":
                case ".GIF":
                case ".ICO":
                case ".JPG":
                case ".JPEG":
                case ".PNG":
                case ".PSD":
                case ".TGA":
                case ".WEBP":
                    config = new ImageConfig { MaxImagePixelSize = 2000, TransparentPng = false };
                    break;
                case ".MPP":
                case ".MPPX":
                    config = new ProjectConfig { ExportPdfA = true, PaperSize = MppPaperSize.A3 };
                    break;
                case ".VSD":
                case ".VSDX":
                    config = new VisioConfig { ExportPdfA = true };
                    break;
            }

            var token = viewer.OpenDocument(filePath, config);

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception(viewer.InternalError);
            }

            var exportDone = false;

            var pdfBytes = viewer.ExportToPdf(); 

            if(null != pdfBytes && pdfBytes.Length > 0)
            {
                System.IO.File.WriteAllBytes(filePath + ".pdf", pdfBytes);

                viewer.CloseDocument();

                exportDone = true;
            }            

            return exportDone;
        }
    }
}