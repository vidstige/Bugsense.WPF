using System.Windows;
using Bugsense.WPF;

namespace SampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string bugsenseApiKey = "<api key here>";

        public App()
        {
            BugSense.Init(bugsenseApiKey);
        }
    }
}
