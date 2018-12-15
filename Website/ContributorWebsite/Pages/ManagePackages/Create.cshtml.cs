using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PublicWebsite.Pages.ManagePackages
{
    public class CreateModel : PageModel
    {
        [Required, BindProperty, StringLength(128)]
        public string PackageName { get; set; }

        [Required, BindProperty]
        public string PackageDescription { get; set; }

        [Required, BindProperty]
        public int PackageAuthorId { get; set; }
    }
}