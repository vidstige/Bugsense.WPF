using System.Windows;
using Bugsense.WPF;

namespace SampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string bugsenseApiKey = "f7be3d04";

        public App()
        {
            BugSense.Init(bugsenseApiKey);
        }
    }
}
