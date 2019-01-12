using System;
using System.IO.Compression;
using System.Net.Http;
using Domain;
using Newtonsoft.Json;

namespace APIClient
{
    public class APIClient
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;

        public APIClient(string apiBaseUrl, HttpClient httpClient)
        {
            _apiBaseUrl = apiBaseUrl;
            _httpClient = httpClient;
        }

        public bool TryFindPackageByName(string targetPackage, out Package package)
        {
            try
            {
                var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packages", $"packageName={targetPackage}")).Result;

                var resultPackage = result.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrEmpty(resultPackage))
                {
                    package = default(Package);
                }
                else
                {
                    package = JsonConvert.DeserializeObject<Package>(resultPackage);
                }
                return true;                
            }
            catch (Exception)
            {
                // TODO We should probably log something here.
                package = default(Package);
                return false;
            }
        }

        public PackageRevision DownloadPackage(string packageName, SemVer.Version packageVersion)
        {
            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packageZips", $"packageName={packageName}")).Result;

            var package = result.Content.ReadAsStringAsync().Result;

            return new PackageRevision(new Package(packageName),
                                       packageVersion,
                                       string.IsNullOrEmpty(package) ?
                                                    default(ZipArchive) :
                                                    JsonConvert.DeserializeObject<ZipArchive>(package));
        }

        public PackageRevision DownloadLatestVersionOfPackage(string targetPackageName)
        {
            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packageZips/latest", $"packageName={targetPackageName}")).Result;

            var packageRevision = result.Content.ReadAsStringAsync().Result;

            return (PackageRevision)((object)packageRevision);
        }
    }
}