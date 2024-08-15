using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

/* Please ensure that the three DLLs from .NET Core Setup are referenced or copied to the bin folder */
using Doconut.Config;
using Doconut.Viewer;

namespace Doconut.Mvc.Core.Controllers
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

            if (!System.IO.File.Exists(pathToFile))
            {
                Response.StatusCode = 404;
                return Content($"File does not exists: {pathToFile}");
            }

            var fileInfo = new FileInfo(pathToFile);

            var licenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Doconut.lic");

            var docViewer = new DocViewer(_cache, _accessor, licenseFilePath);

            var documentOptions = new DocOptions
            {
                Password = "",
                ImageResolution = 200,
                Watermark = "^Sample Copy~Red~24~Verdana~50~-45",
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
                case ".RTF":
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true } };
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                case ".CSV":
                    config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, DocumentCulture = "en-US", PaperSize = ExcelPaperSize.PaperA3, AutoFitContents = true };
                    break;
                case ".PPT":
                case ".PPTX":
                case ".ODP":
                    config = new PptConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, AllowSearch = true, AllowCopy = true } };
                    break;
                case ".DWG":
                case ".DXF":
                case ".DGN":
                    config = new CadConfig { DefaultRender = true, ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                    break;
                case ".EML":
                case ".MSG":
                    config = new EmailConfig { EmailEncoding = Encoding.UTF8 };
                    break;
                case ".PDF":
                    config = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true };
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

                // Webfarm
                if (Startup.useWebfarm)
                {
                    docViewer.ExportToPng(cachePath);
                }

                // Optional: _cache.Set
                if (!Startup.useWebfarm)
                {
                    // You need to store this in cache if you want to 
                    // call methods like export, annotation export etc on the document

                    _cache.Set("docViewer-" + token, docViewer, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(documentOptions.TimeOut) });
                }


                return Content(token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }

        }

        [HttpPost]
        public IActionResult CloseDocument(string token)
        {
            using var doc = new DocViewer(_cache, _accessor,"");
            doc.CloseDocument(token);

            return Content("OK");
        }

        public ActionResult Print()
        {
            return View();
        }

        public IActionResult Modal()
        {
            return View();
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
                fName = ex.Message;
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
                // var exportBytes = documentViewer.ExportToPdf();
                // System.IO.File.WriteAllBytes(exportFilePath, exportBytes);

                // #2
                // This true pdf export with annotations
                var pdfExport = documentViewer.ExportAnnotationsToPdf(exportFilePath, 0, true);  // adjust zoom if you set nativePdf as false

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
