using NavigationMenuUWP;
using SampleApp.Pages;

namespace SampleApp.Navigation
{
    class NavigationItems
    {
        private static readonly NavMenuItem[] StaticItemsTop = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = "\ue10f", Label = "Main page", DestPage = typeof(MainPage) },
            new NavMenuItem { Symbol = "\ue12b", Label = "Other page", DestPage = typeof(OtherPage) },
        };

        private static readonly NavMenuItem[] StaticItemsBottom = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = "\ue115", Label = "Settings", DestPage = typeof(SettingsPage) },
        };

        public NavMenuItem[] TopItems { get { return StaticItemsTop; } }

        public NavMenuItem[] BottomItems { get { return StaticItemsBottom; } }
    }
}
