using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class Documents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                /* Just get the include and init scripts */
                var viewer = new DocViewer
                {
                    ID = "ctlDoc",
                    IncludeJQuery = false,
                    DebugMode = true,
                    FitType = "width",
                    Zoom = 50
                };

                ViewState["ViewerScripts"] = viewer.ReferenceScripts();
                ViewState["ViewerCSS"] = viewer.ReferenceCss();
                ViewState["ViewerID"] = viewer.ClientID;
                ViewState["ViewerObject"] = viewer.JsObject;
                ViewState["ViewerInit"] = viewer.GetAjaxInitArguments("");
                /* end */
            }

            /* Get the token for document being viewed */
            if (Request.QueryString["token"] != null)
            {
                var viewer = new DocViewer
                {
                    ID = "ctlDoc",
                    TimeOut = 30
                };

                var fileName = Request.QueryString["file"];
                var filePath = Path.Combine(Server.MapPath("~/files"), fileName);

                BaseConfig config = null;

                switch (new FileInfo(fileName).Extension.ToUpper())
                {
                    case ".DOC":
                    case ".DOCX":
                        config = new WordConfig { ConvertPdf = false };
                        break;
                    case ".XLS":
                    case ".XLSX":
                    case ".ODS":
                        config = new ExcelConfig { SplitWorksheets = true };
                        break;
                    case ".PDF":
                        config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                        break;
                }

                var viewToken = viewer.OpenDocument(filePath, config);

                if (viewToken.Length > 0)
                {
                    Response.Write(viewToken);
                    Response.End();
                }
                else
                {
                    throw new Exception(viewer.InternalError);
                }
            }
        }
    }
}