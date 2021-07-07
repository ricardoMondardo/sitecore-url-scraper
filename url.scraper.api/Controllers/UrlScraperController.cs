using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace url.scraper.api.Controllers
{
    public class UrlScraperController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Scraper(string url)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(
                    new { data = url }), Encoding.UTF8, "application/json")
            };
            return result;
        }
    }
}
