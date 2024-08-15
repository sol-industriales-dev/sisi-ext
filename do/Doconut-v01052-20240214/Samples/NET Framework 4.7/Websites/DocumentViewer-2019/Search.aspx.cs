using System;
using System.Web.UI;
using DotnetDaddy.DocumentConfig;

public partial class Search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // ALSO TRY THE DEDICATED SEARCH SAMPLE;

        if (!Page.IsPostBack)
        {
            // Using Direct Pdf Search
            ctlDoc.OpenDocument(Server.MapPath("~/files/Sample.pdf"), new PdfConfig { ExtractHyperlinks = true, AllowSearch = true, AllowCopy = true, DefaultRender = true });


            // Optional, faster search preloading search .SRH file 

            /*
            byte[] searchBytes = ctlDoc.SaveSearch();

            if (searchBytes != null)
            {
                var msSearch = new MemoryStream(searchBytes);
                msSearch.Position = 0;

                ctlDoc.LoadSearchData(msSearch);
            }
            */

            // Using the OCR Method

            // ctlDoc.OpenDocument(Server.MapPath("~/files/Session Management.dcn"));
            // ctlDoc.LoadSearchData(Server.MapPath("~/files/Session Management.dcn.srh"));


            if (ctlDoc.Token.Length == 0)
            {
                Response.Write("<H1>" + ctlDoc.InternalError + "</H1>");
                Response.End();
            }

        }
    }
}