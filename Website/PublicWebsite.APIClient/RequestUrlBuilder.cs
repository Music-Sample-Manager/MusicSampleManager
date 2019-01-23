namespace PublicWebsite.APIClient
{
    internal class RequestUrlBuilder
    {
        private static readonly string BaseUrl = "http://host.docker.internal:7071/api/"; // "http://localhost:7071/api/"; // https://contributorwebsitebackend.azurewebsites.net

        internal static string BuildUrl(string action)
        {
            // TODO Switch over to UriBuilder
            return $"{BaseUrl}{action}";
        }
    }
}