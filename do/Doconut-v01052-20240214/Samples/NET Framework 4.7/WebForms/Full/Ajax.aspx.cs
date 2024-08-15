using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string scriptJs = "var objctlDoc; " + ctlDoc.GetAjaxInitArguments(""); // ctlDoc.JsObject
                this.ClientScript.RegisterStartupScript(this.GetType(), "docAjaxView", scriptJs, true);
            }

            // Following code is for Folder.aspx sample page
            if (!Page.IsPostBack)
            {
                if (null != Request.QueryString["Token"])
                {
                    string scriptJs = "objctlDoc.View('" + Request.QueryString["Token"] + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), scriptJs, true);
                }
            }
            // End code
        }

        protected void View_Click(object sender, EventArgs e)
        {
            if (null != (sender as Button))
            {
                string fileName = "";

                switch ((sender as Button).Text)
                {
                    case "TIF":
                        fileName = "Sample.tif";
                        break;
                    case "PPT":
                        fileName = "Sample.ppt";
                        break;
                    case "DOC":
                        fileName = "Sample.doc";
                        break;
                }

                string uploadedFile = Server.MapPath("~/files/" + fileName);

                if (File.Exists(uploadedFile))
                {
                    string token = "";

                    // The use of viewstate here is just to prevent reloading of same file again
                    // instead it uses the existing token reference

                    if (null == ViewState[fileName])
                    {
                        token = ctlDoc.OpenDocument(uploadedFile);
                        ViewState[fileName] = token;
                    }
                    else
                    {
                        token = Convert.ToString(ViewState[fileName]);
                    }

                    if (token.Length > 0 && ctlDoc.InternalError.Equals(string.Empty))
                    {
                        string scriptJs = "objctlDoc.View('" + token + "');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), scriptJs, true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('" + ctlDoc.InternalError + "');", true);
                    }
                }

            }
        }
    }
}