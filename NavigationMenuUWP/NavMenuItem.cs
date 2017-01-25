using System;
using Windows.UI.Xaml.Controls;

namespace NavigationMenuUWP
{
    /// <summary>
    /// Data to represent an item in the nav menu.
    /// </summary>
    public sealed class NavMenuItem
    {
        public string Label { get; set; }
        public string Symbol { get; set; }

        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}
