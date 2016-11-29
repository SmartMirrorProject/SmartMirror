using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SmartMirror.NewsModule
{
    class NewsService
    {
        public static NewsData FetchNews(string newsCategory)
        {
            NewsJSON data = GetNewsData(newsCategory);
            NewsData newsData = new NewsData(newsCategory)
            {
                NewsHeadline1 = data.items[0].title,
                NewsHeadline2 = data.items[1].title,
                NewsHeadline3 = data.items[2].title
            };
            return newsData;
        }

        private static NewsJSON GetNewsData(string newsCategory)
        {
            //            // MOVE THIS SOMEHWHERE ELSE TO BE THE DEFAULT CATEGORY
            //            newsCategory = "cnn_topstories.rss";

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    ($"http://api.rss2json.com/v1/api.json?rss_url=http://rss.cnn.com/rss/" + newsCategory + "&api_key=m7nyrntqaw41kbq3wopdnyiuyvg19shpvs45poai"));
                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;
                //Parse the object from the JSON and return it
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(NewsJSON));
                        var data = (NewsJSON)serializer.ReadObject(stream);
                        return data;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }

    public class NewsJSON
    {
        public List<NewsItems> items { get; set; }
    }

    public class NewsItems
    {
        public string title { get; set; }
    }
}