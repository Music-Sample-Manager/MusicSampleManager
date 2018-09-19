using System;
using System.IO.Compression;
using System.Net.Http;
using Domain;
using Newtonsoft.Json;
using SemVer;

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

        public Package FindPackageByName(string targetPackage)
        {
            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packages", $"packageName={targetPackage}")).Result;

            var package = result.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(package) ?
                       default(Package) :
                       JsonConvert.DeserializeObject<Package>(package);
        }

        public PackageRevision DownloadPackage(string packageName, Version packageVersion)
        {
            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packageZips", $"packageName={packageName}")).Result;

            var package = result.Content.ReadAsStringAsync().Result;

            return new PackageRevision(new Package(packageName),
                                       packageVersion,
                                       string.IsNullOrEmpty(package) ?
                                                    default(ZipArchive) :
                                                    JsonConvert.DeserializeObject<ZipArchive>(package));
        }
    }
}