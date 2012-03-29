using System.Windows;
using Bugsense.WPF;

namespace SampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            BugSense.Init(this, "f7be3d04");
        }
    }
}
