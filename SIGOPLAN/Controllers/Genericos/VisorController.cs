using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System.IO;
using System.Text;
using System.Collections;
using Infrastructure.Utils;
using System.Xml;

namespace SIGOPLAN.Controllers.Genericos
{
    public class VisorController : BaseController
    {
        // GET: Visor
        public ActionResult _visorIndex()
        {
            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                DebugMode = true,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true // use this to enable link for doc and pdf
                // TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
            };

            // if you want to use an older version of jQuery specify this above contructor: 
            // OldJQuery = true

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            var token = viewer.OpenDocument(Server.MapPath("~/files/Sample.doc"));   // open default document (optional)

            if(string.IsNullOrWhiteSpace(token))
            {
                throw new Exception(viewer.InternalError);
            }


            // Get final Init arguments to render the viewer

            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token); // you may also pass an empty string

            ViewBag.token = token;      // initiate value for JS token variable.

            return View();
        }

        [HttpPost]
        public ActionResult OpenFile(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("File not found");
            }

            var viewer = new DocViewer
            {
                ID = "ctlDoc",  // ID is important and required.
                DebugMode = false
                // TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
            };

            BaseConfig config = null;

            switch(new FileInfo(name).Extension.ToUpper())
            {
                case ".DWG":
                case ".DXF":
                    config = new CadConfig { ShowColor = false, WhiteBackground = true, ShowModel = false, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                    break;
                case ".DOC":
                case ".DOCX":
                    config = new WordConfig { ConvertPdf = true, ExtractHyperlinks = true }; // specify false if links are not needed
                    break;
                case ".TXT":
                    config = new WordConfig { PaperSize = DocPaperSize.A4 };
                    break;
                case ".EML":
                case ".MSG":
                    var emlConf = new EmailConfig { EmailEncoding = Encoding.UTF8, ConvertHtml = false };
                    emlConf.PdfConfiguration.DefaultRender = true;
                    config = emlConf;
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                    config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false };
                    break;
                case ".PDF":
                    config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true };// specify false if links are not needed
                    break;
                case ".PNG":
                case ".BMP":
                case ".JPG":
                case ".JPEG":
                case ".PSD":
                case ".GIF":
                    config = new ImageConfig { MaxImagePixelSize = 2000 };
                    break;
            }

            var token = viewer.OpenDocument(name, config);

            if(string.IsNullOrWhiteSpace(token))
            {
                throw new Exception(viewer.InternalError);
            }

            // (optional)
            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            // Session[token] = viewer;

            return Content(token);
        }
        public ActionResult setFile(HttpPostedFileBase archivo)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var esArchivo = false;
                if(archivo != null)
                {
                    byte[] base64 = GlobalUtils.ConvertFileToByte(archivo.InputStream);
                    var extencion = GlobalUtils.getExtencionArchivo(archivo.ContentType);
                    var archivoVisor = new Tuple<byte[], string>(base64, extencion);
                    Session["archivoVisor"] = archivoVisor;
                    esArchivo = true;
                }
                else
                {
                    Session["archivoVisor"] = null;
                }
                resultado.Add(SUCCESS, esArchivo);
            }
            catch(Exception o_O)
            {
                Session["archivoVisor"] = null;
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadFile()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var fileData = Session["archivoVisor"] as Tuple<byte[], string>;

                if(fileData == null || fileData.Item1 == null || fileData.Item2 == null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se pudo cargar el archivo seleccionado");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                var fileExtension = fileData.Item2.ToUpper();

                var extensionesValidas = GlobalUtils.ObtenerExtensionesValidasVisor();

                if(extensionesValidas.Contains(fileExtension) == false)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "El archivo que se intentó abrir tiene una extensión inválida.");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                var viewer = new DocViewer
                {
                    ID = "ctlDoc",  // ID is important and required.
                    DebugMode = true,
                    IncludeJQuery = false,
                    BasePath = "/",
                    ResourcePath = "/",
                    FitType = "width",
                    FixedZoom = true
                    // TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
                };

                BaseConfig config = null;

                switch(fileExtension)
                {
                    case ".DWG":
                    case ".DXF":
                        config = new CadConfig { ShowColor = false, WhiteBackground = true, ShowModel = false, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                        break;
                    case ".DOC":
                    case ".DOCX":
                        config = new WordConfig { ConvertPdf = true, ExtractHyperlinks = true }; // specify false if links are not needed
                        break;
                    case ".TXT":
                        config = new WordConfig { PaperSize = DocPaperSize.A4 };
                        break;
                    case ".EML":
                    case ".MSG":
                        var emlConf = new EmailConfig { EmailEncoding = Encoding.UTF8, ConvertHtml = false };
                        emlConf.PdfConfiguration.DefaultRender = true;
                        config = emlConf;
                        break;
                    case ".XLS":
                    case ".XLSX":
                    case ".ODS":
                        config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false };
                        break;
                    case ".PDF":
                        config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true };// specify false if links are not needed
                        break;
                    case ".PNG":
                    case ".BMP":
                    case ".JPG":
                    case ".JPEG":
                    case ".PSD":
                    case ".GIF":
                        config = new ImageConfig { MaxImagePixelSize = 2000 };
                        break;
                    default:
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El archivo que se intentó abrir tiene una extensión inválida.");
                        return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                var token = viewer.OpenDocument(fileData.Item1, fileExtension, config);

                if(string.IsNullOrWhiteSpace(token))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar abrir el archivo.");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                Session["archivoVisor"] = null;
                resultado.Add(SUCCESS, true);
                resultado.Add("token", token);
            }
            catch(Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al intentar abrir el archivo.");
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _visorGrid()
        {
            // Init the main viewer object.

            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                DebugMode = false,
                BasePath = "/",
                ResourcePath = "/",
                FitType = "width",
                FixedZoom = true
                //Zoom = 40, 
                //TimeOut = 20 
            };

            // Get the required client side script and css
            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;

            // Get final Init arguments to render the viewer
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return PartialView();
        }
        public PartialViewResult _visorPage()
        {
            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                DebugMode = false,
                BasePath = "/",
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true
            };
            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            var Ruta = (string)Session["RutaVisor"];
            if(string.IsNullOrWhiteSpace(Ruta))
            {
                throw new Exception(viewer.InternalError);
            }
            Session["RutaVisor"] = null;
            var token = viewer.OpenDocument(Ruta);
            if(string.IsNullOrWhiteSpace(token))
            {
                throw new Exception(viewer.InternalError);
            }
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token);
            return PartialView();
        }
        public ActionResult _visorPrint()
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
                foreach(string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];

                    if(null == file)
                        continue;

                    if(file.ContentLength <= 0)
                        continue;

                    // check for any malicious file types
                    var invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                    var fileExtension = new FileInfo(file.FileName).Extension.ToUpper();

                    if(invalidFiles.IndexOf(fileExtension, StringComparison.Ordinal) > -1)
                    {
                        throw new Exception("Invalid file extension");
                    }


                    fName = DateTime.Now.ToShortDateString().Replace("/", "-") + "--" + file.FileName;

                    foreach(char c in Path.GetInvalidFileNameChars())
                    {
                        fName = fName.Replace(c, '-');
                    }

                    // Remove special characters
                    fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");
                    var filePath = Server.MapPath(@"~\files") + "\\" + fName;

                    file.SaveAs(filePath);
                }

            }
            catch(Exception)
            {
                isSavedSuccessfully = false;
            }

            return Content(isSavedSuccessfully ? fName : "");
        }
    }
}