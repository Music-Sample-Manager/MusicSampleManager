using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicWebsite.APIClient
{
    public class APIClient
    {
        public async Task<List<PackageRec>> GetPackagesByAuthorId(int authorId)
        {
            string requestUrl = RequestUrlBuilder.BuildUrl($"ListPackages?authorId={authorId}");

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync(requestUrl);
                var stringResult = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<PackageRec>>(stringResult);
            }
        }

        public async Task<PackageRec> GetPackageByIdAsync(int packageId)
        {
            string requestUrl = RequestUrlBuilder.BuildUrl($"GetPackageById?packageId={packageId}");

            using (HttpClient client = new HttpClient())
            {
                var apiResult = await client.GetAsync(requestUrl);
                var stringResult = await apiResult.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PackageRec>(stringResult);
            }
        }

        public void AddPackageRevision(string versionNumber, IFormFile packageRevisionFile)
        {
            string requestUrl = RequestUrlBuilder.BuildUrl($"AddPackageRevision?versionNumber={versionNumber}");

            // TODO Use the Blob sdk to create a Blob here from the file.
            // TODO Attach the versionNumber to the Blob as metadata.
            // TODO Set up a Blob storage-triggered function that reads the Blob's
            //      versionNumber metadata and then stores the reference to the blob
            //      in our relational store.
            throw new NotImplementedException();
        }

        public async Task<List<PackageRevisionRec>> GetPackageRevisionsByPackageId(int packageId)
        {
            string requestUrl = RequestUrlBuilder.BuildUrl($"GetPackageRevisionsByPackageId?packageId={packageId}");

            using (HttpClient client = new HttpClient())
            {
                var apiResult = await client.GetAsync(requestUrl);
                var stringResult = await apiResult.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<PackageRevisionRec>>(stringResult);
            }
        }

        public async void CreatePackage(string packageName, string packageDescription, int packageAuthorId)
        {
            string requestUrl = RequestUrlBuilder.BuildUrl($"/CreatePackage?packageName={packageName}&packageDescription={packageDescription}&authorId={packageAuthorId}");

            using (HttpClient client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = true }))
            {
                var result = await client.PostAsync(requestUrl, new StringContent(string.Empty));

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Invalid HTTP status: {result.StatusCode}");
                }
            }
        }
    }
}