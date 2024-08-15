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
using Doconut.Config.Cloud;

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
            // If you already have your document saved to any Cloud location
            // provide its token here. Using doconut demo CDN token below.
            //
            // To upload a new document just change the following string to an empty ""

            //var token = "cf5943c9-d6ba-4d22-a5b0-50c9fae13141";
            var token = "";

            if (string.IsNullOrEmpty(token))
            {
                var pathToFile = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "files"), fileName);

                if (!System.IO.File.Exists(pathToFile))
                {
                    Response.StatusCode = 404;
                    return Content($"File does not exists: {pathToFile}");
                }

                var fileInfo = new FileInfo(pathToFile);

                var licenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Doconut.lic");

                // create a document viewer instance
                var docViewer = new DocViewer(_cache, _accessor, licenseFilePath);

                var documentOptions = new DocOptions
                {
                    Password = "",
                    ImageResolution = 200,
                    Watermark = "^Sample Copy~Red~24~Verdana~50~-45",
                    TimeOut = 30
                };

                BaseConfig config = GetConfig(fileInfo.Extension.ToUpper());

                try
                {
                    token = docViewer.OpenDocument(pathToFile, config, documentOptions);

                    // Get Cloud Config
                    CloudUploadConfig cloudConfig = GetUploadConfig(CloudLocation.CDN); // Pass your preferred provider.

                    var cachePath = Path.Combine(_hostingEnvironment.WebRootPath, Startup.webFarmFolder);

                    // Common properties
                    cloudConfig.WebfarmPath = cachePath;
                    cloudConfig.PerDocumentFunctions = false;
                    cloudConfig.SaveConfigToCache = true;
                    cloudConfig.SaveConfigToDisk = false;

                    // if need to show watermark
                    cloudConfig.FunctionsConfig.Watermark = true;

                    // important to set the token to current token
                    cloudConfig.Token = token;

                    var cloudError = docViewer.ExportToCloud(cloudConfig);

                    if (!string.IsNullOrEmpty(cloudError))
                    {
                        Response.StatusCode = 500;
                        return Content("Error : " + cloudError);
                    }

                }
                catch (Exception e)
                {
                    Response.StatusCode = 500;
                    return Content("Error : " + e.Message);
                }
            }

            return Content(token);
        }

        private CloudUploadConfig GetUploadConfig(CloudLocation cloudLocation)
        {
            CloudUploadConfig cloudConfig = null;

            // Please set values for your cloud provider

            switch (cloudLocation)
            {
                case CloudLocation.Azure:
                    cloudConfig = new AzureConfig
                    {
                        DoconutAzureContainerName = "",
                        DoconutAzureStorageConnectionString = ""
                    };
                    break;
                case CloudLocation.AmazonS3:
                    cloudConfig = new AmazonConfig
                    {
                        DoconutAwsS3Key = "",
                        DoconutAwsS3BucketName = "",
                        DoconutAwsS3RegionEndpoint = "",
                        DoconutAwsS3Secret = ""
                    };
                    break;
                case CloudLocation.GoogleCloud:
                    cloudConfig = new GoogleCloudConfig
                    {
                        DoconutGoogleBucketName = "",
                        DoconutGoogleServiceAuthJsonFile = ""
                    };
                    break;
                case CloudLocation.DropBox:
                    cloudConfig = new DropBoxConfig
                    {
                        DoconutDropBoxToken = ""
                    };
                    break;
                case CloudLocation.Redis:
                    cloudConfig = new RedisConfig
                    {
                        DoconutRedisConnectionString = ""
                    };
                    break;
                case CloudLocation.CDN:
                    cloudConfig = new CDNConfig
                    {
                        // For demo purpose, using our CDN / web path
                        DoconutCdnUrl = "http://cdn.doconut.com"
                    };
                    break;
                default:
                    break;
            }

            // Common properties

            cloudConfig.DefaultConfigFallBack = false;
            cloudConfig.PerDocumentFunctions = false;

            cloudConfig.SaveConfigToCache = false;
            cloudConfig.SaveConfigToDisk = false;

            cloudConfig.FunctionsConfig.CachePages = false;
            cloudConfig.FunctionsConfig.CacheInMemory = false;

            cloudConfig.FunctionsConfig.Annotation = false;
            cloudConfig.FunctionsConfig.Search = false;
            cloudConfig.FunctionsConfig.Links = false;
            cloudConfig.FunctionsConfig.RotateFlip = false;
            cloudConfig.FunctionsConfig.Hide = false;
            cloudConfig.FunctionsConfig.Watermark = false;

            return cloudConfig;
        }

        private BaseConfig GetConfig(string fileExt)
        {
            BaseConfig config = null;

            switch (fileExt)
            {
                case ".DOC":
                case ".DOCX":
                case ".ODT":
                case ".RTF":
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowCopy = true } };
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
                    config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowCopy = true };
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

            return config;
        }

        public ContentResult CloseDocument(string token)
        {
            using var doc = new DocViewer(_cache, _accessor, "");
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

    }

}
