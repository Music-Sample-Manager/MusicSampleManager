﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PackagesService.DAL.Entities;
using System.Collections.Generic;

namespace PublicWebsite.Pages.ManagePackages
{
    public class PackageModel : PageModel
    {
        private readonly APIClient.APIClient _apiClient;

        public PackageModel(APIClient.APIClient apiClient)
        {
            _apiClient = apiClient;
        }

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