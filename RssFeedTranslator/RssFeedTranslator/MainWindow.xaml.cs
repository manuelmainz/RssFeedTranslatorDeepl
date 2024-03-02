using RssFeedTranslator.Utils;
using RssFeedTranslator.Utils.DeepL;
using RssFeedTranslator.Utils.Json;
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
using Wpf.Ui.Appearance;

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
            ApplicationThemeManager.Apply(this);

            //Wpf.Ui.Appearance.ApplicationThemeManager.Apply(
            // ApplicationTheme.Dark,     // Theme type
            //  Wpf.Ui.Controls.WindowBackdropType.Mica, // Background type
            //  true                                   // Whether to change accents automatically
            //);

            //Loaded += (sender, args) =>
            //{
            //    Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
            //        this,                                  // Window class
            //        Wpf.Ui.Controls.WindowBackdropType.Mica, // Background type
            //        true                                   // Whether to change accents automatically
            //    );
            //};

            string? deeplAuthKey = Environment.GetEnvironmentVariable("DEEPL_AUTH_KEY");

            if (deeplAuthKey is null)
            {
                throw new ConfigurationErrorsException("Please configure environment variable 'DEEPL_AUTH_KEY'.");
            }

            var deeplTranslator = new DeeplTranslator(deeplAuthKey);
            var persister = new JsonPersister("filesote.json");

            var translator = new CacheTranslator(deeplTranslator, persister);

            DataContext = new MainWindowViewModel(translator);
        }
    }
}