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
using System.Threading.Tasks;

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
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true } };
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                    config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, DocumentCulture = "en-US", PaperSize = ExcelPaperSize.PaperA3, AutoFitContents = true };
                    break;
                case ".PPT":
                case ".PPTX":
                case ".ODP":
                    config = new PptConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, AllowSearch = true, AllowCopy = true } };
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
                    config = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true };
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

                // Webfarm
                if (Startup.useWebfarm)
                {
                    docViewer.ExportToPng(cachePath);

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Export to SRH
                        var srhbytes = docViewer.SaveSearch();

                        if (null != srhbytes && srhbytes.Length > 0)
                        {
                            var msSearch = new MemoryStream(srhbytes)
                            {
                                Position = 0
                            };

                            docViewer.LoadSearchData(msSearch);
                        }
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

}
