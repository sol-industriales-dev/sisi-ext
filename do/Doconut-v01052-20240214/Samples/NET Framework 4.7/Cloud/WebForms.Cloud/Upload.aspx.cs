using System;
using System.IO;

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentConfig.Cloud;
using DotnetDaddy.DocumentViewer;

public partial class Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string token = "";

        token = "cf5943c9-d6ba-4d22-a5b0-50c9fae13141"; // IMPORTANT : This token is for CDN, demo. Delete this line!!
        Response.Redirect("Default.aspx?token=" + token); // Delete this line!!


        // Open document and upload to cloud
        using (var ctlDoc = new DocViewer { TimeOut = 10 })
        {
            var config = new WordConfig { ConvertPdf = true };
            var fileName = "Sample.doc"; // change file as required
            var filePath = Path.Combine(Server.MapPath("~/files"), fileName);

            if (!File.Exists(filePath))
                throw new Exception("File not found " + filePath);

            // open a document to get its token
            token = ctlDoc.OpenDocument(filePath, config);

            if (token.Length == 0)
            {
                throw new Exception(ctlDoc.InternalError);
            }
            else
            {
                // Get Cloud Config
                CloudUploadConfig cloudConfig = Utility.GetUploadConfig(CloudLocation.CDN); // Pass your preferred provider.
                                                                                            // Match it in web.config Handler too.
                cloudConfig.WebfarmPath = Server.MapPath("~/export");

                if (null != cloudConfig)
                {
                    // Important to set the token to current token
                    cloudConfig.Token = token;

                    // Upload to cloud
                    var cloudError = ctlDoc.ExportToCloud(cloudConfig);

                    if (!string.IsNullOrEmpty(cloudError))
                    {
                        throw new Exception(cloudError);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx?token=" + token);
                    }
                }
                else {
                    throw new Exception("Invalid CloudUploadConfig");
                }
            }
        }

    }
}