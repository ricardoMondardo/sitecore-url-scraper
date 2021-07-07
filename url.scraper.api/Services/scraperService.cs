using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace url.scraper.api.Services
{
    public class ScraperService
    {
        public ScraperService() { }

        public async Task<List<string>> GetWordsAndImgs(string pageUrl)
        {

            var BaseUrl = string.Format("https://{0}", pageUrl);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(BaseUrl);

            var resp = await response.Content.ReadAsStringAsync();

            var result = new List<string>()
            {
                "aaa",
                "vvvv",
                BaseUrl,
                resp
            };
            return result;
        }
    }
}