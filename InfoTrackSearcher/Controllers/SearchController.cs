using InfoTrackSearcher.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfoTrackSearcher.Controllers
{
    public class SearchController : Controller
    {
        // GET: SearchController
        public ViewResult Index()
        {
            return View(new SearchModel());
        }

        // Raw: <a\s+(?:[^>]*?\s+)?href=(["'])\/url\?q=(?!https:\/\/accounts\.google\.com)(.*?)\1
        private const string _googleRegexString = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])\\/url\\?q=(?!https:\\/\\/accounts\\.google\\.com)(.*?)\\1";
        private const string _pageString = "start";
        private const int _firstPageResultCount = 10;

        // POST: SearchController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Index(SearchModel returnedModel)
        {
            ModelState.Remove(nameof(SearchModel.Results));
            returnedModel.Results = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    // Get Results
                    returnedModel.Results = ControllerUtilities.GetSOEResults(returnedModel.Keywords, returnedModel.URL, _googleRegexString, _pageString, _firstPageResultCount).Result;
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
