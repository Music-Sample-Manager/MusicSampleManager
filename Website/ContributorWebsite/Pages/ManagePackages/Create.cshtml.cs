﻿using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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



        public async void OnPostAsync()
        {
            string baseUrl = "http://localhost:7071"; // https://contributorwebsitebackend.azurewebsites.net
            string requestUrl = $"{baseUrl}/api/CreatePackage?packageName={PackageName}&packageDescription={PackageDescription}&authorId={PackageAuthorId}";

            using (HttpClient client = new HttpClient())
            {
                await client.PostAsync(requestUrl, new StringContent(string.Empty));
            }

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_db.Customers.Add(Customer);
            //await _db.SaveChangesAsync();
            //return RedirectToPage("/Index");
        }
    }
}