using System;

namespace PublicWebsite.APIClient
{
    internal class RequestUrlBuilder
    {
        private static readonly string BaseUrl = "http://localhost:7071/api/"; // https://contributorwebsitebackend.azurewebsites.net

        internal static string BuildUrl(string action)
        {
            // TODO Switch over to UriBuilder
            return $"{BaseUrl}{action}";
        }
    }
}