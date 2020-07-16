using InfoTrackSearcher.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static InfoTrackSearcher.Models.SearchModel;

namespace InfoTrackSearcher.Controllers
{
    public class EngineController : Controller
    {
        // GET: EngineController
        public ActionResult Index()
        {
            return View(new SearchModel { Keywords = "online title search", SearchEngine = SearchModel.SearchEngines.Google.ToString() });
        }

        private Dictionary<string, EngineDetail> _searchEngines = new Dictionary<string, EngineDetail> {
            { SearchModel.SearchEngines.Google.ToString(), new EngineDetail { URL = "https://www.google.com.au", RegexFindString = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])\\/url\\?q=(?!https:\\/\\/accounts\\.google\\.com)(.*?)\\1", PageString = "start" } }
            ,{ SearchModel.SearchEngines.Bing.ToString(), new EngineDetail { URL = "https://www.bing.com.au", RegexFindString =  "<h2><a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1", PageString = "first"} }
        };

        // POST: EngineController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchModel returnedModel)
        {
            ModelState.Remove(nameof(SearchModel.Results));
            returnedModel.Results = string.Empty;
            var engineDetail = new EngineDetail();

            if (ModelState.IsValid)
            {
                // Field Validation - Validation done on the controller as model is used in different controllers, each with different validation
                if (returnedModel.Keywords == null) ModelState[nameof(SearchModel.Keywords)].Errors.Add("Keywords must be entered.");

                if (returnedModel.SearchEngine == null)
                {
                    ModelState[nameof(SearchModel.SearchEngine)].Errors.Add("Search Engine must be entered.");
                }
                else
                {
                    // Find Search Engine Settings for selected Search Engine
                    if (!_searchEngines.TryGetValue(returnedModel.SearchEngine, out engineDetail)) ModelState[nameof(SearchModel.SearchEngine)].Errors.Add("Invalid Item.");
                }

                // Check for any errors
                if (ModelState.Values.Where(x => x.Errors.Count > 0).Count() > 0) return View(returnedModel);

                // Get Results
                var error = string.Empty;
                var results = string.Empty;
                if (!ControllerUtilities.GetSOEResults(returnedModel.Keywords, engineDetail.URL, engineDetail.RegexFindString, engineDetail.PageString, out results, out error))
                {
                    ModelState[nameof(SearchModel.SearchEngine)].Errors.Add(error);
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
