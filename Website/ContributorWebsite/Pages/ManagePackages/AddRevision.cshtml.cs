using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PackagesService.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicWebsite.Pages.ManagePackages
{
    public class AddRevisionModel : PageModel
    {
        private readonly APIClient.APIClient _apiClient;

        public AddRevisionModel(APIClient.APIClient apiClient)
        {
            _apiClient = apiClient;
        }

        [Required, BindProperty, StringLength(20)]
        public string VersionNumber { get; set; }


        [BindProperty]
        public PackageRec Package { get; set; }

        public void OnGet(int packageId)
        {
            Package = _apiClient.GetPackageByIdAsync(packageId).Result;
        }
    }
}