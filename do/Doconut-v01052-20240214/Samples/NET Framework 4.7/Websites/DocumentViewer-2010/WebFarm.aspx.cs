using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotnetDaddy.DocumentViewer;
using System.Configuration;
using System.IO;

public partial class WebFarm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // BEFORE USING THIS SAMPLE PLEASE VISIT WEB.CONFIG AND READ COMMENTS FOR DiskImageHandler and DoconutPngExportPath

        if (!Page.IsPostBack)
        {
            var ctlDoc = new DocViewer();

            ctlDoc.ID = "ctlDoc";

            ctlDoc.LargeDoc = true;
            ctlDoc.ImageResolution =200;
            ctlDoc.AutoScrollThumbs = true;
            ctlDoc.Zoom=75;
            ctlDoc.IncludeJQuery = false;
            ctlDoc.WatermarkInfo = "^Sample Copy~Gray~24~Arial~80~-45";

            ctlDoc.TimeOut = 1; // close after one minute of non-use

            string token = ctlDoc.OpenDocument(Server.MapPath("~/Sample.ppt"));

            // for pre-loaded documents just provide the token string value
            // example:  string token = "5e645443-202a-44c9-b8a2-359829905e49"; 

            if (token.Length > 0 && ctlDoc.InternalError.Equals(string.Empty))
            {
                // You may avoid Server.MapPath when using network or full path

                if (null == ConfigurationManager.AppSettings["DoconutPngExportPath"])
                {
                    Response.Write("Please provide appSetting value for [DoconutPngExportPath] in web.config <br/>Also make sure you are using DiskImageHandler.");
                    Response.End();
                }

                string exportPath = Server.MapPath(Convert.ToString(ConfigurationManager.AppSettings["DoconutPngExportPath"]));

                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath); // Ideally you should have this created beforehand
                }

                string exportError = ctlDoc.ExportToPng(exportPath);


                if (exportError.Equals(string.Empty))
                {
                    string scriptJs = " $(document).ready(function () { setTimeout(function(){ " + ctlDoc.GetAjaxInitArguments(token) + "; $('#loading').hide(); }, 5000); });";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "docView", scriptJs, true);
                }
                else
                {
                    Response.Write(exportError);
                    Response.End();
                }
            }
            else
            {
                Response.Write(ctlDoc.InternalError);
                Response.End();
            }
        }
    }

}