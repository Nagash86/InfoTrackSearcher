using InfoTrackSearcher.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static InfoTrackSearcher.Models.SearchModel;

namespace InfoTrackSearcher.Controllers
{
    public class EngineController : Controller
    {
        // GET: EngineController
        public ViewResult Index()
        {
            return View(new SearchModel());
        }

        private Dictionary<string, EngineDetail> _searchEngines = new Dictionary<string, EngineDetail> {
            { SearchModel.SearchEngines.Google.ToString(), new EngineDetail { URL = "https://www.google.com.au", RegexFindString = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])\\/url\\?q=(?!https:\\/\\/accounts\\.google\\.com)(.*?)\\1", PageString = "start", FirstPageResults = 10 } }
            ,{ SearchModel.SearchEngines.Bing.ToString(), new EngineDetail { URL = "https://www.bing.com.au", RegexFindString =  "<h2><a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1", PageString = "first", FirstPageResults = 8} }
        };

        // POST: EngineController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Index(SearchModel returnedModel)
        {
            ModelState.Remove(nameof(SearchModel.Results));
            returnedModel.Results = string.Empty;

            if (ModelState.IsValid)
            {
                var engineDetail = _searchEngines[returnedModel.SearchEngine];

                try
                {
                    // Get Results
                    returnedModel.Results = ControllerUtilities.GetSOEResults(returnedModel.Keywords, engineDetail.URL, engineDetail.RegexFindString, engineDetail.PageString, engineDetail.FirstPageResults).Result;
                }
                catch (AggregateException ex)
                {
                    returnedModel.Results = ex.Message;
                }
            }

            return View(returnedModel);
        }
    }
}
