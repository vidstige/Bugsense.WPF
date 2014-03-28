﻿using System.Windows;
using Bugsense.WPF;

namespace SampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string BugsenseApiKey = "<your app key here>";

        public App()
        {
            BugSense.Init(BugsenseApiKey);
            BugSense.AddExtra("foo", "bar");
            BugSense.AddExtra("bacon", "awesome");
        }
    }
}
