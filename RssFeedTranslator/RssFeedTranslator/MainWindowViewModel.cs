using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RssFeedTranslator.Utils;
using RssFeedTranslator.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace RssFeedTranslator
{
    class MainWindowViewModel : ObservableObject
    {
        private ArticleViewModel? selectedArticle;
        private string? translatedSummary;
        private readonly ITranslator translator;

        public ICommand DoSomethingCommand { get; }
        
        public ICommand TranslateArticleCommand { get; }

        public ObservableCollection<ArticleViewModel> Articles { get; } = new ObservableCollection<ArticleViewModel>();

        public ArticleViewModel? SelectedArticle
        {
            get => selectedArticle;
            set
            {
                if (SetProperty(ref selectedArticle, value))
                {
                    TranslatedSummary = null;
                }
            }
        }

        public string? TranslatedSummary
        {
            get => translatedSummary;
            set => SetProperty(ref translatedSummary, value);
        }

        public MainWindowViewModel(ITranslator translator)
        {
            this.translator = translator;
            DoSomethingCommand = new RelayCommand(DoSomething);
            TranslateArticleCommand = new RelayCommand(TranslateArticle);
        }

        public void DoSomething()
        {
            string url = "https://www.ad.nl/binnenland/rss.xml";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                Articles.Add(new ArticleViewModel(item));
            }
        }

        public async void TranslateArticle()
        {
            if (SelectedArticle is not ArticleViewModel article)
            {
                return;
            }

            if (translator is null)
            {
                throw new NotImplementedException("Translator is not available.");
            }

            string translatedText = await translator.Translate(article.Summary);

            TranslatedSummary = translatedText;
        }
    }
}
