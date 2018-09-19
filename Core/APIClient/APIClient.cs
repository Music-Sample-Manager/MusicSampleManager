using System.IO.Compression;
using System.Net.Http;
using Domain;
using Newtonsoft.Json;
using SemVer;

namespace APIClient
{
    public class APIClient
    {
        public Package FindPackageByName(string targetPackage)
        {
            var client = new HttpClient();

            var result = client.GetAsync(UriBuilder.BuildUri("https://localhost:44349", "api/packages", $"packageName={targetPackage}").ToString()).Result;

            var package = result.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(package) ?
                       default(Package) :
                       JsonConvert.DeserializeObject<Package>(package);
        }

        public PackageRevision DownloadPackage(string packageName, Version packageVersion)
        {
            var client = new HttpClient();

            var result = client.GetAsync(UriBuilder.BuildUri("https://localhost:44349", "api/packageZips", "packageName={packageName}").ToString()).Result;

            var package = result.Content.ReadAsStringAsync().Result;

            return new PackageRevision(new Package(packageName),
                                       packageVersion,
                                       string.IsNullOrEmpty(package) ?
                                                    default(ZipArchive) :
                                                    JsonConvert.DeserializeObject<ZipArchive>(package));
        }
    }
}