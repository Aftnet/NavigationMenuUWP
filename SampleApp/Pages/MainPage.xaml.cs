﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SampleApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavigationFrame_ItemSelected(NavigationMenuUWP.NavigationFrame sender, NavigationMenuUWP.NavMenuItem args)
        {
            var frame = (Frame)Window.Current.Content;
            if (args.Page != frame.CurrentSourcePageType)
            {
                frame.Navigate(args.Page);
            }
        }
    }
}
