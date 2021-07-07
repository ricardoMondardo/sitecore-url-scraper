using Sitecore.Pipelines;
using System.Web.Http;

namespace url.scraper.api.Pipelines.WebApi
{
    public class RegisterWebApi
    {
        public void Process(PipelineArgs args)
        {
            var config = GlobalConfiguration.Configuration;

            config.Routes.MapHttpRoute("UrlScraperApiRoute",
                "api/UrlScraperApi/{action}/{url}",
                new { controller = "UrlScraper", action = "All", url = RouteParameter.Optional});

        }
    }
}
