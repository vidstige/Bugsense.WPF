Discontinued
============
This library has ben discontinued in favor for the official Bugsense for WPF library found here:
https://www.nuget.org/packages/BugSense.WPF.Official/

This will still be available on NuGet

Advantages over the official library
* Open Source

Disadvantages
* Does not have all the features of the official library


Bugsense.WPF
============

This contains the necessary files to get started with Bugsense for Windows Desktop application. Primary usage is for WPF-applications,
but will work equally fine with Windows Forms application.

What is this?
-------------
Bugsense is a great tool to collect crashes in the wild. They will be sortable by most common crash for priorization and the stacktrace
is easily fetched for easier fixing of bugs.

Read more at http://bugsense.com/

Getting started
---------------
Reference the Bugsense.WPF assembly and just add this in your App.xaml.cs

        private const string bugsenseApiKey = "<apikey>";

        public App()
        {
            BugSense.Init(bugsenseApiKey);
        }

You will need to get your own Api Key to paste in. This is so that the bugsense serverside will recognize your application. Just go to
http://bugsense.com/ and register for free.
