using System.ComponentModel.DataAnnotations;

namespace InfoTrackSearcher.Models
{
    public class SearchModel
    {
        [Display(Name = "Keywords")]
        [Required(ErrorMessage = "Keywords must be entered.")]
        public string Keywords { get; set; }

        [Display(Name = "URL")]
        [Required(ErrorMessage = "URL must be entered.")]
        [Url]
        public string URL { get; set; }

        [Display(Name = "Search Engine")]
        [Required(ErrorMessage = "Search Engine must be entered.")]
        public string SearchEngine { get; set; }

        [Display(Name = "SEO Results")]
        public string Results { get; set; }

        public SearchModel()
        {
            Keywords = "online title search";
            URL = "https://www.google.com.au";
            SearchEngine = nameof(SearchEngines.Google);
            Results = string.Empty;
        }

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
            public int FirstPageResults = 10;
        }
    }
}