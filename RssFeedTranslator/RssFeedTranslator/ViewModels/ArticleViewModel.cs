using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedTranslator.ViewModels
{
    class ArticleViewModel
    {
        private readonly SyndicationItem item;

        public string Title => item.Title.Text;

        public string Summary => item.Summary.Text;

        public ArticleViewModel(SyndicationItem item)
        {
            this.item = item;
        }
    }
}
