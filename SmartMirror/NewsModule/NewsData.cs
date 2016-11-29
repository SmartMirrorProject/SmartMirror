using SmartMirror.MainModule;

namespace SmartMirror.NewsModule
{
    public class NewsData : BindableBase
    {
        public string NewsCategory { get; set; }

        public string NewsHeadline1 { get; set; }
        public string NewsHeadline2 { get; set; }
        public string NewsHeadline3 { get; set; }

        public NewsData(string newsCategory, string newsHeadline1, string newsHeadline2, string newsHeadline3)
        {
            NewsCategory = newsCategory;

            NewsHeadline1 = newsHeadline1;
            NewsHeadline2 = newsHeadline2;
            NewsHeadline3 = newsHeadline3;
        }

        public NewsData(string newsCategory)
        {
            NewsCategory = newsCategory;
        }
    }
}
