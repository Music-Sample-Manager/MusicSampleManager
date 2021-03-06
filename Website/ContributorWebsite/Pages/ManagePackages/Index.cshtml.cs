﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using PackagesService.DAL.Entities;
using PublicWebsite.APIClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContributorWebsite.Pages
{
    public class ManagePackagesModel : PageModel
    {
        private readonly APIClient _apiClient;

        public ManagePackagesModel(APIClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<PackageRec> Packages { get; set; }

        public async Task OnGetAsync()
        {
            int dummyAuthorId = 1;

            var packages = await _apiClient.GetPackagesByAuthorId(dummyAuthorId);

            Packages = packages;
        }
    }
}