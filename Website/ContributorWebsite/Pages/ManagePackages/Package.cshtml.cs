using Core.DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace PublicWebsite.Pages.ManagePackages
{
    public class PackageModel : PageModel
    {
        private readonly APIClient.APIClient _apiClient = new APIClient.APIClient();

        [BindProperty]
        public PackageRec Package { get; set; }

        [BindProperty]
        public List<PackageRevisionRec> PackageRevisions { get; set; }


        public void OnGet(int packageId)
        {
            Package = _apiClient.GetPackageByIdAsync(packageId).Result;
            PackageRevisions = _apiClient.GetPackageRevisionsByPackageId(packageId).Result;
        }
    }
}