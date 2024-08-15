using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

/* Reference all the three DLLs from .NET Core Setup Zip or copy them over to the bin folder */
using Doconut.Config;
using Doconut.Viewer;

namespace Doconut.Mvc.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(IWebHostEnvironment hostingEnvironment, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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

        [HttpGet]
        public IActionResult Modal()
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
                Watermark = "Sample Copy~Red~24~Verdana~50~-45", // add a ^ at front, for all corner display
                TimeOut = 30
            };


            BaseConfig config = null;

            switch (fileInfo.Extension.ToUpper())
            {
                case ".DOC":
                case ".DOCX":
                case ".ODT":
                case ".RTF":
                    var wpdfConfig = new PdfConfig { ExtractHyperlinks = true, DefaultRender = true }; // If you face any issue viewing then set DefaultRender = false 
                    config = new WordConfig { ConvertPdf = true, PdfConfig = wpdfConfig };
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
                    config = new PptConfig { ConvertPdf = false };
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
                    config = new PdfConfig
                    {
                        DefaultRender = true,  // If you face any issue viewing then set DefaultRender = false 
                        ExtractHyperlinks = true,
                        HyperlinksPageCount = 5
                    };
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

                return Content(token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }

        }

        public ActionResult Print()
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

    }

    public class UploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ConvertResult : UploadResult
    {
        public string PdfPath { get; set; }
    }
}
