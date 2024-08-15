using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotnetDaddy.DocumentViewer;
using DotnetDaddy.DocumentConfig;
using System.Collections;

public partial class ExcelViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Upload_Insert_Click(object sender, EventArgs e)
    {
        if (txtUpload.HasFile)
        {
            string fileName = txtUpload.FileName;

            string validFiles = ".XLS .XLSX .ODS";
            string fileExtension = new System.IO.FileInfo(fileName).Extension.ToUpper();

            if (validFiles.IndexOf(fileExtension) < 0)
            {
                return;
            }

            string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
            txtUpload.SaveAs(uploadedFile);

            ProcessExcel(uploadedFile);

        }
        else
        {
            Response.Write("<H1>Please select a file first.</H3>");

            Response.End();

        }
    }

    private void ProcessExcel(string myFile)
    {
        using (DocViewer ctlDoc = new DocViewer { ID = "ctlDoc", ImageResolution=150 })
        {
            var config = new ExcelConfig { SplitWorksheets = false, ShowEmptyWorkSheets = true };
            string token = ctlDoc.OpenDocument(myFile, config);

            if (token.Length > 0)
            {
                int totalSheets = ctlDoc.TotalPages;
                GenerateUI(token, totalSheets, config.SheetNames);
            }
            else
            {
                Response.Write("<H1>" + ctlDoc.InternalError + "</H1>");
                Response.End();
            }
        }
    }

    private string GenerateUI(string token, int sheets, ArrayList sheetNames)
    {
        string sheetNameCommaSep = string.Join("^", (string[])sheetNames.ToArray(Type.GetType("System.String")));

        ClientScript.RegisterStartupScript(this.GetType(), "showExcelUI", "$().ready(function() { BuildExcelUI('" + token + "'," + sheets.ToString() + ",'" + sheetNameCommaSep + "'); } );", true);
        return "";
    }

}