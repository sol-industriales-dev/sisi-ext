using DotnetDaddy.DocumentConfig.Cloud;

public class Utility
{
    public static CloudUploadConfig GetUploadConfig(CloudLocation cloudLocation)
    {
        CloudUploadConfig cloudConfig = null;

        // Please set values for your cloud provider

        switch (cloudLocation)
        {
            case CloudLocation.Azure:
                cloudConfig = new AzureConfig
                {
                    DoconutAzureContainerName = "",
                    DoconutAzureStorageConnectionString = ""
                };
                break;
            case CloudLocation.AmazonS3:
                cloudConfig = new AmazonConfig
                {
                    DoconutAwsS3Key = "",
                    DoconutAwsS3BucketName = "",
                    DoconutAwsS3RegionEndpoint = "",
                    DoconutAwsS3Secret = ""
                };
                break;
            case CloudLocation.GoogleCloud:
                cloudConfig = new GoogleCloudConfig
                {
                    DoconutGoogleBucketName = "",
                    DoconutGoogleServiceAuthJsonFile = ""
                };
                break;
            case CloudLocation.DropBox:
                cloudConfig = new DropBoxConfig
                {
                    DoconutDropBoxToken = ""
                };
                break;
            case CloudLocation.Redis:
                cloudConfig = new RedisConfig
                {
                    DoconutRedisConnectionString = ""
                };
                break;
            case CloudLocation.FTP:
                cloudConfig = new FTPConfig
                {
                    DoconutFtpHost = "",
                    DoconutFtpUser = "",
                    DoconutFtpPassword = "",
                    DoconutFtpSecure = false
                };
                break;
            case CloudLocation.CDN:
                cloudConfig = new CDNConfig
                {
                    // For demo purpose, using our CDN / web path
                    DoconutCdnUrl = "http://cdn.doconut.com"
                };
                break;
            default:
                break;
        }

        // Common properties

        if (null != cloudConfig)
        {
            cloudConfig.LogErrors = true;

            cloudConfig.FunctionsConfig.CachePages = false;
            cloudConfig.FunctionsConfig.CacheInMemory = false;

            cloudConfig.WebConfigFallBack = false;
            cloudConfig.PerDocumentFunctions = false;

            // Refer Global.asax > Application_Start
            cloudConfig.SaveConfigToCache = true;
            cloudConfig.SaveConfigToDisk = false;
            cloudConfig.ConfigCacheTimeMinutes = 360;

            // Document level functions

            cloudConfig.FunctionsConfig.Annotation = false;
            cloudConfig.FunctionsConfig.Search = false;
            cloudConfig.FunctionsConfig.Links = false;
            cloudConfig.FunctionsConfig.RotateFlip = false;              
            cloudConfig.FunctionsConfig.Hide = false;
            cloudConfig.FunctionsConfig.Watermark = false;
        }

        return cloudConfig;
    }
}