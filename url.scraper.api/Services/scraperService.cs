using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using url.scraper.api.Objects;

namespace url.scraper.api.Services
{
    public class ScraperService
    {
        private string _baseUrl;
        public ScraperService(string pageUrl) {
            _baseUrl = pageUrl.Contains("http") ? pageUrl : string.Format("https://{0}", pageUrl);
        }

        public async Task<ResultScrape> GetWordsAndImgs()
        {
            
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_baseUrl);
            var resp = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(resp);
                       
            ResultScrape resultScrape = new ResultScrape();
            var rlistWordsSorted = GetWords(document).OrderByDescending(x => x.count).Take(10).ToArray();

            resultScrape.ListImages = GetImages(document);
            resultScrape.ListWords = rlistWordsSorted.ToList();

            return resultScrape;
        }

        private List<string> GetImages(HtmlDocument document)
        {
            List<string> rListImages = new List<string>();

            var imgSrcs = GetItensByAttribute(document, "src");                                    
            rListImages.AddRange(FixHttpUrls(imgSrcs));

            var imgSrcSets = GetItensByAttribute(document, "srcset");
            rListImages.AddRange(FixHttpUrls(imgSrcSets));

            return rListImages;
        }

        private List<string> FixHttpUrls(List<string> listStrs)
        {
            var rListImages = new List<string>();
            foreach (string src in listStrs)
            {
                if (src.Contains("http")) rListImages.Add(src);
                else rListImages.Add(string.Format("{0}/{1}", _baseUrl, src));
            }
            return rListImages;
        }

        private List<string> GetItensByAttribute(HtmlDocument document, string attr)
        {
            return document.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue(attr, null))
                .Where(s => !String.IsNullOrEmpty(s))
                .ToList();
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