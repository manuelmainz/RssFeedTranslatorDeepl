using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RssFeedTranslator.Utils;
using RssFeedTranslator.Utils.DeepL;
using RssFeedTranslator.Utils.Json;
using RssFeedTranslator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssFeedTranslator.WinUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            string? deeplAuthKey = Environment.GetEnvironmentVariable("DEEPL_AUTH_KEY");

            if (deeplAuthKey is null)
            {
                throw new System.ArgumentException("Please configure environment variable 'DEEPL_AUTH_KEY'.");
            }

            var deeplTranslator = new DeeplTranslator(deeplAuthKey);

            string filePath = Path.Combine(Path.GetTempPath(), "RssFeedTranslator.json");
            var persister = new JsonPersister(filePath);

            var translator = new CacheTranslator(deeplTranslator, persister);

            var vm = new MainWindowViewModel(translator);
            RootPanel.DataContext = vm;
        }
    }
}
