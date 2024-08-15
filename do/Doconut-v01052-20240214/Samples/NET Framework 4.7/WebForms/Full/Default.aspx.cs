using System;
using System.Web.UI;

namespace Full
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.RedirectPermanent("index.html");
        }
    }
}