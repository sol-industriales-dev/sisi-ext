using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DocView : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void OpenDocument(string documentPath)
    {
        ctlDoc.OpenDocument(documentPath);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "initscript", "InitControl()", true);
    }
}