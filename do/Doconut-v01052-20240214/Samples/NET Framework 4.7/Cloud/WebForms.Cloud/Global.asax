<%@ Application Language="C#" %>
<%@ Import Namespace="DotnetDaddy.DocumentViewer" %>
<%@ Import Namespace="DotnetDaddy.DocumentConfig.Cloud" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // We are saving the config instances to the webserver's cache
        // This is useful when using webfarm and you don't want to save
        // the credentials in web.config or to disk

        using (var objViewer = new DocViewer())
        {
            var cdnConfig = Utility.GetUploadConfig(CloudLocation.CDN);

             // use this only if SaveConfigToDisk = true 
            cdnConfig.WebfarmPath = Server.MapPath("~/export");

            objViewer.SaveCloudConfig(cdnConfig);
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
