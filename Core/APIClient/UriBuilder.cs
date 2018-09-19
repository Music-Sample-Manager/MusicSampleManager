using System;

namespace APIClient
{
    public class UriBuilder
    {
        public static Uri BuildUri(string baseUrl, string path, string urlParameter)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (baseUrl == string.Empty)
            {
                throw new ArgumentException(nameof(baseUrl));
            }

            return new Uri($"{baseUrl}//{path}?{urlParameter}");
        }
    }
}
