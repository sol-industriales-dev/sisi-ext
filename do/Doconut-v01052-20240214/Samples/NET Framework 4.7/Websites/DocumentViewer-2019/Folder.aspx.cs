using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotnetDaddy.DocumentViewer;

public partial class Folder : System.Web.UI.Page
{
    private const string FOLDER = "~/files/";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string dir = Server.MapPath(FOLDER);

            if (Directory.Exists(dir))
            {
                var fileList = new DirectoryInfo(dir).GetFiles("*", SearchOption.TopDirectoryOnly)
                                .Where(file => file.Extension == ".doc" || file.Extension == ".ppt") // you can have your own or remove this line for *ALL*
                                .Select(file => new { Title = file.Name, Token = ctlDoc.OpenDocument(file.FullName) })
                                .ToList();

                var validFiles = fileList
                                 .Where(doc => doc.Token.Length > 0)
                                 .ToList();

                lstDocs.DataSource = validFiles;
                lstDocs.DataBind();
            }
        }
    }
    
}