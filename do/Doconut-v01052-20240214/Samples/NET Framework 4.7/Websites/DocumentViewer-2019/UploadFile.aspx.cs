using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];

                if (null == file)
                    continue;

                if (file.ContentLength <= 0) continue;

                string invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                string fileExtension = new System.IO.FileInfo(file.FileName).Extension.ToUpper();

                if (invalidFiles.IndexOf(fileExtension) > -1)
                {
                    continue;
                }

                var fName = DateTime.Now.ToShortDateString().Replace("/", "-") + "--" + file.FileName;

                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fName = fName.Replace(c, '-');
                }

                fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");

                var filePath = System.IO.Path.Combine(Server.MapPath("~/files/"), fName);
                file.SaveAs(filePath);

                Response.Write(fName);
            }

        }
        catch (Exception ex)
        {
            var err = ex.Message;
            Response.StatusCode = 404;
            Response.End();
        }

        
    }
}