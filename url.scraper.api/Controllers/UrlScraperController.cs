using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using url.scraper.api.Services;

namespace url.scraper.api.Controllers
{
    public class UrlScraperController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Scraper(string url)
        {

            ScraperService scraperService = new ScraperService(url);
            var dataInfo = await scraperService.GetWordsAndImgs();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    new JavaScriptSerializer().Serialize(dataInfo), 
                    Encoding.UTF8, 
                    "application/json")
            };
            return result;
        }
    }
}
