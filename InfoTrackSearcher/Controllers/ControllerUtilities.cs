using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfoTrackSearcher.Controllers
{
    public static class ControllerUtilities
    {
        private const string _infoTrackURL = "https://www.infotrack.com.au";
        private const int _maxResultCount = 100;
        private const int _resultsPerPage = 10;

        public static async Task<string> GetSOEResults(string keywords, string url, string regexString, string pageString, int firstPageResultsCount)
        {
            var returnResults = string.Empty;
            var pageStartCounter = 0;
            var tasks = new List<Task>();

            // Asynchronously Loop, adding tasks to go fetch pages of results, totalling at least 100 results
            for (int page = 1; page < (_maxResultCount / _resultsPerPage) + 1; page++)
            {
                string passedURL = url + "/search?q=" + keywords.Replace(' ', '+') + "&" + pageString + "=" + pageStartCounter;
                tasks.Add(GetResultsForPageAsync(passedURL, regexString, pageStartCounter));
                pageStartCounter += page == 1 ? firstPageResultsCount : _resultsPerPage;
            }

            // Loop through each task processing the results
            foreach (Task<Dictionary<int, Match>> task in tasks)
            {
                await task;

                foreach (var item in task.Result)
                {
                    if (item.Value.Value.Contains(_infoTrackURL) && item.Key <= _maxResultCount)
                    {
                        returnResults = returnResults == string.Empty ? item.Key.ToString() : returnResults + ", " + item.Key.ToString();
                    }
                }
            }

            // If no instances of the InfoTrack URL is found, return 0 as a result
            if (returnResults == string.Empty) returnResults = "0";

            return returnResults;
        }

        private static async Task<Dictionary<int, Match>> GetResultsForPageAsync(string url, string regexString, int startingPoint)
        {
            var results = new Dictionary<int, Match>();
            var counter = 1;

            // Get HTML Content from URL
            var htmlContent = await GetHTMLForUrlAsync(url);

            // Add results to matches
            foreach (Match match in Regex.Matches(htmlContent, regexString).ToList())
            {
                results.Add(startingPoint + counter, match);
                counter += 1;
            }

            return results;
        }

        private static async Task<string> GetHTMLForUrlAsync(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = WebRequestMethods.Http.Get;
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    return responseStream.ReadToEnd();
                }
            }
        }
    }
}
