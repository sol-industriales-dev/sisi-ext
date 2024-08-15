using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;


/* Reference all the three DLLs from .NET Core Setup Zip or copy them over to the bin folder */
using Doconut.Config;
using Doconut.Viewer;


namespace DoconutWebApp.Pages
{
    public class OpenModel : PageModel
    {
        public OpenModel(IWebHostEnvironment hostingEnvironment, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _accessor = httpContextAccessor;
        }

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _accessor;

        public IActionResult OnGet(string fileName)
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
                    var pdfConfig = new PdfConfig { ExtractHyperlinks = true };
                    config = new WordConfig { ConvertPdf = true, PdfConfig = pdfConfig };
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
                    config = new PdfConfig
                    {
                        DefaultRender = true, // For {Linux, Docker} use DefaultRender = false 
                        ImageResolution = 200,
                        ExtractHyperlinks = true,
                        HyperlinksPageCount = 5,
                        ResizeImages = false,
                        ResizeResolution = 150
                    };
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

                // For {webfarm} 'uncomment' below lines (also refer Startup.cs)

                // docViewer.ExportToPng(Path.Combine(_hostingEnvironment.WebRootPath, "cache")); // create a cache folder in wwwroot
                // System.Threading.Thread.Sleep(2000); // adjust 2 seconds as required

                return Content(token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Content(e.Message);
            }
        }
    }
}
