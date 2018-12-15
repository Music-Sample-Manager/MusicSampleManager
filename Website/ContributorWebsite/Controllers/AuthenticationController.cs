using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Mvc;

namespace PublicWebsite.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                AzureADDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToPage("Index");
            }

            return View();
        }
    }
}