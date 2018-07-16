using Domain;
using Newtonsoft.Json;
using System.Net.Http;

namespace APIClient
{
    public class APIClient
    {
        public Package FindPackageByName(string targetPackage)
        {
            var client = new HttpClient();

            var result = client.GetAsync("https://localhost:44349/api/packages?packageName=Test").Result;


            var package = result.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(package) ?
                       default(Package) :
                       JsonConvert.DeserializeObject<Package>(package);
        }
    }
}