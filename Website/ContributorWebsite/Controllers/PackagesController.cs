﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PublicWebsite
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly APIClient.APIClient _apiClient = new APIClient.APIClient();

        [HttpPost("Create")]
        public ActionResult Create()
        {

            var packageName = Request.Form["PackageName"];
            var packageDescription = Request.Form["PackageDescription"];
            var packageAuthorId = int.Parse(Request.Form["PackageAuthorId"]);

            _apiClient.CreatePackage(packageName, packageDescription, packageAuthorId);

            return new RedirectToPageResult("/ManagePackages/Index");
        }

        [HttpPost("AddRevision")]
        public ActionResult AddRevision(IFormFile packageRevisionFile)
        {
            var versionNumber = Request.Form["VersionNumber"];

            _apiClient.AddPackageRevision(versionNumber, packageRevisionFile);

            return new OkResult();
        }
    }
}