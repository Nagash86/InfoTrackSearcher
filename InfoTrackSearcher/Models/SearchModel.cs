using System.ComponentModel.DataAnnotations;

namespace InfoTrackSearcher.Models
{
    public class SearchModel
    {
        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "Search Engine")]
        public string SearchEngine { get; set; }

        [Display(Name = "SEO Results")]
        public string Results { get; set; }

        public enum SearchEngines
        {
            Google,
            Bing
        }

        public class EngineDetail
        {
            public string URL = string.Empty;
            public string RegexFindString = string.Empty;
            public string PageString = string.Empty;
        }
    }
}
