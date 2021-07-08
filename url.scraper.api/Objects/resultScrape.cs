using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace url.scraper.api.Objects
{
    public class ResultScrape
    {
        public List<string> ListImages { get; set; }
        public List<WordCount> ListWords { get; set; }        
    }
}