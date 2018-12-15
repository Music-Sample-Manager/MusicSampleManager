using Core.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PublicWebsite.Pages.ManagePackages
{
    public class AddRevisionModel : PageModel
    {
        private readonly APIClient.APIClient _apiClient = new APIClient.APIClient();

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