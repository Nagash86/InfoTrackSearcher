using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace InfoTrackSearcher.Controllers
{
    public static class ControllerUtilities
    {
        public const string _infoTrackURL = "https://www.infotrack.com.au";
        public const int _maxResultCount = 100;

        public static bool GetSOEResults(string keywords, string url, string regexString, string pageString, out string returnResults, out string returnError)
        {
            returnResults = string.Empty;
            returnError = string.Empty;
            var counter = 1;           
            var searchResults = new List<Match>();

            // Loop, getting at least the top 100 results, retrieving the results using the supplied Regex from the raw HTML response
            while (searchResults.Count < _maxResultCount)
            {
                var htmlContent = string.Empty;

                if (!ControllerUtilities.GetHTMLForURL(url + "/search?q=" + keywords.Replace(' ', '+') + "&" + pageString + "=" + searchResults.Count, out htmlContent, out returnError))
                {
                    return false;
                };
                searchResults.AddRange(Regex.Matches(htmlContent, regexString).ToList());
            }

            // Search the top 100 results for the Infotrack URL, adding it to return URL
            foreach (var searchResult in searchResults)
            {
                if (searchResult.Value.Contains(ControllerUtilities._infoTrackURL) && counter <= _maxResultCount)
                {
                    returnResults = returnResults == string.Empty ? counter.ToString() : returnResults + ", " + counter.ToString();
                }

                counter += 1;
            }

            // If no instances of the InfoTrack URL is found, return 0 as a result
            if (returnResults == string.Empty) returnResults = "0";

            return true;
        }

        public static bool GetHTMLForURL(string url, out string returnHtml, out string error)
        {
            error = string.Empty;
            returnHtml = string.Empty;

            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Get;
                var webResponse = webRequest.GetResponse();
                var streamReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                returnHtml = streamReader.ReadToEnd();

                streamReader.Close();
                webResponse.Close();

                return true;
            }
            catch (UriFormatException)
            {
                error = "Invalid URL.";
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
