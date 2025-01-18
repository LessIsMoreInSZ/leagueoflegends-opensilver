using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Leagueoflegends
{
    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            var bootstrapper = new LeagueOfLegendsBootstrapper();
            bootstrapper.Run();

            var mainPage = new MainPage();
            Window.Current.Content = mainPage;
        }
    }
}
