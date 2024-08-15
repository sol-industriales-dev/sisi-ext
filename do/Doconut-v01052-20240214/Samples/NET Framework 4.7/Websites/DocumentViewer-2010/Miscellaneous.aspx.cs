using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Miscellaneous : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ctlDoc.CloseDocument(); // close existing document first (if any)

            // Text watermark
            // var waterMarkInfo =  ctlDoc.SetWatermark("Sample Copy", Color.Gray, new Font("Arial", 24), 75, -45, true);
           
            // Image watermark
            // var waterMarkInfo = ctlDoc.SetWatermark(Server.MapPath("~/files/Google.png"), 50, -45, true);

            ctlDoc.OpenDocument(Server.MapPath("~/files/Sample.ppt"));

            ctlDoc.HidePages(3, 5); // Hide pages from 3 to 5
            ctlDoc.HidePages(9, 9); // Hide page 9
            ctlDoc.HidePages(20, 21); // Hide pages 20, 21 
        }
    }
}