using RssFeedTranslator.Utils;
using RssFeedTranslator.Utils.DeepL;
using RssFeedTranslator.Utils.Json;
using RssFeedTranslator.ViewModels;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RssFeedTranslator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string? deeplAuthKey = Environment.GetEnvironmentVariable("DEEPL_AUTH_KEY");

            if (deeplAuthKey is null)
            {
                throw new ConfigurationErrorsException("Please configure environment variable 'DEEPL_AUTH_KEY'.");
            }

            var deeplTranslator = new DeeplTranslator(deeplAuthKey);
            
            string filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "RssFeedTranslator.json");
            var persister = new JsonPersister(filePath);

            var translator = new CacheTranslator(deeplTranslator, persister);

            DataContext = new MainWindowViewModel(translator);
        }
    }
}