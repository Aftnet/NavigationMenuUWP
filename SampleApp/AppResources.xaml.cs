using NavigationMenuUWP;
using SampleApp.Pages;
using Windows.UI.Xaml;

namespace SampleApp
{
    public sealed partial class AppResources : ResourceDictionary
    {
        private readonly NavMenuItem[] TopNavItems = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = "\ue10f", Label = "Main page", Param = typeof(MainPage) },
            new NavMenuItem { Symbol = "\ue12b", Label = "Other page", Param = typeof(OtherPage) },
        };

        private readonly NavMenuItem[] BottomNavItems = new NavMenuItem[]
        {
            new NavMenuItem { Symbol = "\ue115", Label = "Settings", Param = typeof(SettingsPage) },
        };

        public AppResources()
        {
            this.InitializeComponent();

            this.Add(nameof(TopNavItems), TopNavItems);
            this.Add(nameof(BottomNavItems), BottomNavItems);
        }
    }
}
