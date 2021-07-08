using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using url.scraper.api.Objects;

namespace url.scraper.api.Services
{
    public class ScraperService
    {
        public ScraperService() { }

        public async Task<ResultScrape> GetWordsAndImgs(string pageUrl)
        {
            var baseUrl = pageUrl.Contains("http") ? pageUrl : string.Format("https://{0}", pageUrl);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl);

            var resp = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(resp);


            
            var rlistWordsSorted = GetWords(document).OrderByDescending(x => x.count).Take(10).ToArray();

            ResultScrape resultScrape = new ResultScrape();

            resultScrape.ListImages = GetImages(document, baseUrl);
            resultScrape.ListWords = rlistWordsSorted.ToList();

            return resultScrape;
        }

        private List<String> GetImages(HtmlDocument document, string baseUrl)
        {
            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//img/@src").ToArray();
            var rListImages = new List<string>();
            foreach (HtmlNode item in nodes)
            {
                var src = item.Attributes.FirstOrDefault(x => x.Name.Equals("src"));
                if (src.Value.Contains("http")) rListImages.Add(src.Value);
                else rListImages.Add(string.Format("{0}/{1}", baseUrl, src.Value));
            }
            return rListImages;
        }

        private List<WordCount> GetWords(HtmlDocument document)
        {
            var nodesWods = document.DocumentNode.SelectSingleNode("//body").DescendantsAndSelf();
            var excludeTag = new List<string>() { "script", "style", "td" };
            var rListWords = new List<WordCount>();
            foreach (HtmlNode node in document.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']"))
            {
                if (!excludeTag.Contains(node.ParentNode.Name))
                {
                    var arrPhase = node.InnerText.Trim().Split(new[] { " " }, StringSplitOptions.None);
                    foreach (var word in arrPhase)
                    {
                        if (word.Length > 2)
                        {
                            var indexWord = rListWords.FindIndex(x => x.word.Equals(word));
                            if (indexWord > -1) rListWords[indexWord].count = rListWords[indexWord].count + 1;
                            else rListWords.Add(new WordCount() { word = word, count = 1 });
                        }
                    }
                }
            }
            return rListWords;
        }
    }
}