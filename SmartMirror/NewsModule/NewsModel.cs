using SmartMirror.VoiceControlModule;
using Windows.UI.Xaml;

namespace SmartMirror.NewsModule
{
    public class NewsModel : IVoiceControllableModule
    {
        public NewsData Headline { get; set; }
        private readonly NewsViewModel viewModel;

        public IVoiceController VoiceController { get; set; }

        public NewsModel(NewsViewModel vm)
        {
            viewModel = vm;
            VoiceController = new NewsVoiceController("Grammar\\newsGrammar.xml", this);
            //Start the news on top stories.
            UpdateNews("cnn_topstories.rss");
        }

        public void UpdateNews(string category)
        {
            Headline = NewsService.FetchNews(category);
            viewModel.Headline1 = Headline.NewsHeadline1;
            viewModel.Headline2 = Headline.NewsHeadline2;
            viewModel.Headline3 = Headline.NewsHeadline3;
        }

        public void HandleVoiceCommand(string cmd, string type)
        {
            if (NewsCommands.CMD_SHOW.Equals(cmd))
            {
                if (viewModel.Visible.Equals(Visibility.Collapsed))
                {
                    viewModel.Visible = Visibility.Visible;
                }
                else if (NewsCommands.TYPE_TOP.Equals(type))
                {
                    UpdateNews("cnn_topstories.rss");
                }
                else if (NewsCommands.TYPE_US.Equals(type))
                {
                    UpdateNews("cnn_us.rss");
                }
                else if (NewsCommands.TYPE_BUSINESS.Equals(type))
                {
                    UpdateNews("money_latest.rss");
                }
                else if (NewsCommands.TYPE_POLITICS.Equals(type))
                {
                    UpdateNews("cnn_allpolitics.rss");
                }
                else if (NewsCommands.TYPE_TECHNOLOGY.Equals(type))
                {
                    UpdateNews("cnn_tech.rss");
                }
                else if (NewsCommands.TYPE_ENTERTAINMENT.Equals(type))
                {
                    UpdateNews("cnn_showbiz.rss");
                }
            }
            else if (NewsCommands.CMD_HIDE.Equals(cmd))
            {
                if (viewModel.Visible.Equals(Visibility.Visible))
                {
                    viewModel.Visible = Visibility.Collapsed;
                }
            }
        }
    }
}
