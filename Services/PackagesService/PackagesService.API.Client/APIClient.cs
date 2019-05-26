using System;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PackagesService.Domain;
using Semver;

namespace PackagesService.API.Client
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
            // TODO If this fails we should bubble that up to the UI somehow.
            ValidatePackageName(targetPackage);

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
            catch (Exception ex)
            {
                // TODO We should probably log something here.
                package = default(Package);
                return false;
            }
        }

        public PackageRevision DownloadPackage(string packageName, SemVersion packageVersion)
        {
            // TODO If this fails we should bubble that up to the UI somehow.
            ValidatePackageName(packageName);

            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packageZips", $"packageName={packageName}")).Result;

            var package = result.Content.ReadAsStringAsync().Result;

            return new PackageRevision(new Package(packageName),
                                       packageVersion,
                                       string.IsNullOrEmpty(package) ?
                                                    default(ZipArchive) :
                                                    JsonConvert.DeserializeObject<ZipArchive>(package));
        }

        public async Task<PackageRevision> GetLatestPackageZip(string targetPackageName)
        {
            // TODO If this fails we should bubble that up to the UI somehow.
            ValidatePackageName(targetPackageName);

            var result = _httpClient.GetAsync(UriBuilder.BuildUri(_apiBaseUrl, "api/packageZips/latest", $"packageName={targetPackageName}")).Result;

            var packageRevisionJson = await result.Content.ReadAsStringAsync();
            var deserializedPackageRevision = JsonConvert.DeserializeObject<PackageRevision>(packageRevisionJson);

            return deserializedPackageRevision;
        }

        private bool ValidatePackageName(string packageName)
        {
            if (packageName == string.Empty)
            {
                throw new ArgumentException(nameof(packageName));
            }
            else if (packageName == null)
            {
                throw new ArgumentNullException(nameof(packageName));
            }
            else
            {
                return true;
            }
        }
    }
}