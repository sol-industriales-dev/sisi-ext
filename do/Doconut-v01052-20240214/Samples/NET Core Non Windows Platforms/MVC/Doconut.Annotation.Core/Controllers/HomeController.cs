using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

/* Please reference the three DLLs from the .NET Core Setup Zip or just copy them to bin */

using Doconut.Config;
using Doconut.Viewer;
using Doconut.Config.Viewer.Annotations;
using System.Drawing;

namespace Doconut.Annotation.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(IWebHostEnvironment hostingEnvironment, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _accessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OpenDocument(string fileName)
        {
            var pathToFile = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "files"), fileName);

            var fileInfo = new FileInfo(pathToFile);

            var licenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Doconut.lic");

            var docViewer = new DocViewer(_cache, _accessor, licenseFilePath);

            var documentOptions = new DocOptions
            {
                Password = "",
                ImageResolution = 200,
                Watermark = "Sample Copy~Red~24~Verdana~50~-45",               
                TimeOut = 30            
            };

            // Webfarm

            var cachePath = Path.Combine(_hostingEnvironment.WebRootPath, Startup.webFarmFolder);

            if (Startup.useWebfarm)
            {
                documentOptions.IsWebfarm = true;
                documentOptions.WebfarmPath = cachePath;
                documentOptions.TimeOut = 2; // close as soon as possible for webfarm
            }

            BaseConfig config = null;

            switch (fileInfo.Extension.ToUpper())
            {
                case ".DOC":
                case ".DOCX":
                case ".ODT":
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5 } };
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                    config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, DocumentCulture = "en-US", PaperSize = ExcelPaperSize.PaperA3, AutoFitContents = true };
                    break;
                case ".PPT":
                case ".PPTX":
                case ".ODP":
                    config = new PptConfig();
                    break;
                case ".DWG":
                case ".DXF":
                    config = new CadConfig { DefaultRender = true, ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                    break;
                case ".EML":
                case ".MSG":
                    config = new EmailConfig { EmailEncoding = Encoding.UTF8 };
                    break;
                case ".PDF":
                    config = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5 };
                    break;
                case ".PNG":
                case ".BMP":
                case ".JPG":
                case ".JPEG":
                case ".PSD":
                case ".GIF":
                    config = new ImageConfig { MaxImagePixelSize = 2000, TransparentPng = false };
                    break;
                case ".TXT":
                    config = new WordConfig { PaperSize = DocPaperSize.A4 };
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


            try
            {
                var token = docViewer.OpenDocument(pathToFile, config, documentOptions);

	        // Optional : You can add annotation by code
                // AddAnnotations(docViewer);

                // Optional: _cache.Set
                if (!Startup.useWebfarm)
                {
                    // You need to store this in cache if you want to 
                    // call methods like export, annotation export etc on the document

                    _cache.Set("docViewer-" + token, docViewer, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(documentOptions.TimeOut) });
                }


                // Webfarm
                if (Startup.useWebfarm)
                {
                    docViewer.ExportToPng(cachePath); 

                    // Use below, ONLY if you want to use option #2 ExportAnnotationToPdf(exportFilePath, 100);  
                    var pdfBytes = docViewer.ExportToPdf();
                    if (null != pdfBytes)
                    {
                        var pathWithToken = Path.Combine(cachePath, token);
                        System.IO.File.WriteAllBytes(Path.Combine(pathWithToken, "Document.pdf"), pdfBytes); // Keep name as it is "Document.pdf"
                    }
                }

                return Content(token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }

        }

	private void AddAnnotations(DocViewer docViewer)
        {
            using (var firstPage = docViewer.GetThumbnail(1, 0, 100, false))
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

                    docViewer.LoadAnnotationXML(annMgr.GetAnnotationXml());
                    // OR
                    // docViewer.LoadAnnotationData(annMgr.GetAnnotationData());
                }
            }
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            var isSavedSuccessfully = true;
            string fName;

            try
            {
                if (file == null || file.Length == 0)
                    return Content("");


                fName = DateTime.Now.Ticks + file.FileName;


                var invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                var fileExtension = new FileInfo(file.FileName).Extension.ToUpper();

                if (invalidFiles.IndexOf(fileExtension, StringComparison.Ordinal) > -1)
                {
                    return Content("");
                }

                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fName = fName.Replace(c, '-');
                }

                fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");

                var path = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "files"), fName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            catch (Exception ex)
            {
                fName = ex.InnerException?.Message;
                isSavedSuccessfully = false;
            }

            return Content(isSavedSuccessfully ? fName : "");
        }


        [HttpPost]
        public ActionResult ExportAnnotations(string token)
        {
            var documentViewer = GetViewerInstance(token);

            try
            {
                var fileName = $"{DateTime.Now.Ticks}-Export.pdf";
                var exportFilePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "files"), fileName);

                // #1
                // This exports all pages as image, this will not be a searchable or scalable Pdf                
                // var exportBytes = documentViewer.ExportToPdf(true);
                // System.IO.File.WriteAllBytes(exportFilePath, exportBytes);

                // #2
                // This is a smart pdf export, it replaces just the annotated pages. Rest of the pages remain as true pdf.
                var pdfExport = documentViewer.ExportAnnotationsToPdf(exportFilePath, 0, true);  // adjust zoom if  you set nativePdf to false

                // #3
                // If you just want the annotated pages as PNG
                // var exportResult = documentViewer.ExportAnnotationsToPng(Path.Combine(_hostingEnvironment.WebRootPath, "files"), 100);

                return Content(fileName);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content("error : ExportAnnotations " + ex.Message);
            }
        }

        private DocViewer GetViewerInstance(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("error: GetViewerInstance Missing token");
            }

            if (!Startup.useWebfarm)
            {
                // Get stored instance from Cache
                _cache.TryGetValue("docViewer-" + token, out DocViewer documentViewer);

                if (null == documentViewer)
                {
                    throw new Exception("error: GetViewerInstance DocumentViewer is null");
                }

                return documentViewer;
            }
            else
            { 
                // Use this is for WebFarm

                var licenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Doconut.lic");
                var cachePath = Path.Combine(_hostingEnvironment.WebRootPath, Startup.webFarmFolder);

                var docViewerWebFarm = new DocViewer(_cache, _accessor, licenseFilePath);

                if (!docViewerWebFarm.OpenDocumentWebFarm(cachePath, token))
                {
                    throw new Exception("error: GetViewerInstance DocumentViewer is null");
                }

                return docViewerWebFarm;
            }
            
        }

        [HttpPost]
        public ActionResult ExportXml(string token)
        {
            var documentViewer = GetViewerInstance(token);

            try
            {
                var annXml = documentViewer.GetAnnotationXML();

                var exportFileName = $"{DateTime.Now.Ticks}-Export.xml";

                var exportFilePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "files"), exportFileName);

                annXml.Save(exportFilePath);

                return Content(exportFileName);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content("error : ExportXml " + ex.Message);
            }
        }

    }

}
