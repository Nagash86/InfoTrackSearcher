using InfoTrackSearcher.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InfoTrackSearcher.Controllers
{
    public class SearchController : Controller
    {
        // GET: SearchController
        public ActionResult Index()
        {
            return View(new SearchModel { Keywords = "online title search", URL = "https://www.google.com.au" });
        }

        // Raw: <a\s+(?:[^>]*?\s+)?href=(["'])\/url\?q=(?!https:\/\/accounts\.google\.com)(.*?)\1
        private const string GoogleRegexString = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])\\/url\\?q=(?!https:\\/\\/accounts\\.google\\.com)(.*?)\\1";
        private const string PageString = "start";

        // POST: SearchController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchModel returnedModel)
        {
            ModelState.Remove(nameof(SearchModel.Results));
            returnedModel.Results = string.Empty;

            if (ModelState.IsValid)
            {
                // Field Validation - Validation done on the controller as model is used in different controllers, each with different validation
                if (returnedModel.Keywords == null) ModelState[nameof(SearchModel.Keywords)].Errors.Add("Keywords must be entered.");
                if (returnedModel.URL == null) ModelState[nameof(SearchModel.URL)].Errors.Add("URL must be entered.");

                // Check for any errors
                if (ModelState.Values.Where(x => x.Errors.Count > 0).Count() > 0) return View(returnedModel);

                // Get Results
                var error = string.Empty;
                var results = string.Empty;
                if (!ControllerUtilities.GetSOEResults(returnedModel.Keywords, returnedModel.URL, GoogleRegexString, PageString, out results, out error))
                {
                    ModelState[nameof(SearchModel.URL)].Errors.Add(error);
                }
                else
                {
                    returnedModel.Results = results;
                }
            }

            return View(returnedModel);
        }
    }
}
