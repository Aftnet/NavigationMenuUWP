using NavigationMenuUWP;
using SampleApp.Pages;
using Windows.UI.Xaml.Controls;

namespace SampleApp.Navigation
{
    class NavigationItems
    {
        private static readonly NavMenuItem[] StaticItemsTop = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = Symbol.AllApps, Label = "Main page", DestPage = typeof(MainPage) },
            new NavMenuItem { Symbol = Symbol.Favorite, Label = "Other page", DestPage = typeof(OtherPage) },
        };

        private static readonly NavMenuItem[] StaticItemsBottom = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = Symbol.Setting, Label = "Settings", DestPage = typeof(SettingsPage) },
        };

        public NavMenuItem[] TopItems { get { return StaticItemsTop; } }

        public NavMenuItem[] BottomItems { get { return StaticItemsBottom; } }
    }
}
