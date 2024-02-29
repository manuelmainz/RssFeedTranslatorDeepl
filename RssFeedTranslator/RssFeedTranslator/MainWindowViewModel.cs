using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RssFeedTranslator.ViewModels;
using System;
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
    class MainWindowViewModel //: ObservableObject
    {
        private ArticleViewModel? selectedArticle;

        public ICommand DoSomethingCommand { get; }

        public ObservableCollection<ArticleViewModel> Articles { get; } = new ObservableCollection<ArticleViewModel>();

        public ArticleViewModel? SelectedArticle
        {
            get => selectedArticle;
            set => selectedArticle = value; // SetProperty(ref selectedArticle, value);
        }

        public MainWindowViewModel()
        {
             DoSomethingCommand = new RelayCommand(DoSomething);
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
    }
}
