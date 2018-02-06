using NavigationMenuUWP.Controls;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using NavigationMenuUWP.Tools;

namespace NavigationMenuUWP
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class NavigationFrame : UserControl
    {
        public UIElement FrameContent
        {
            get => (UIElement)GetValue(FrameContentProperty);
            set { SetValue(FrameContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FrameContent.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty frameContentProperty = DependencyProperty.Register(nameof(FrameContent), typeof(UIElement), typeof(NavigationFrame), new PropertyMetadata(DependencyProperty.UnsetValue));
        internal static DependencyProperty FrameContentProperty => frameContentProperty;

        public IEnumerable<NavMenuItem> NavigationItemsTop
        {
            get => (IEnumerable<NavMenuItem>)GetValue(NavigationItemsTopProperty);
            set { SetValue(NavigationItemsTopProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NavigationItems.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty navigationItemsTopProperty = DependencyProperty.Register(nameof(NavigationItemsTop), typeof(IEnumerable<NavMenuItem>), typeof(NavigationFrame), new PropertyMetadata(new NavMenuItem[0]));
        internal static DependencyProperty NavigationItemsTopProperty => navigationItemsTopProperty;

        public IEnumerable<NavMenuItem> NavigationItemsBottom
        {
            get => (IEnumerable<NavMenuItem>)GetValue(NavigationItemsBottomProperty);
            set { SetValue(NavigationItemsBottomProperty, value); }
        }
        private static readonly DependencyProperty navigationItemsBottomProperty = DependencyProperty.Register(nameof(NavigationItemsBottom), typeof(IEnumerable<NavMenuItem>), typeof(NavigationFrame), new PropertyMetadata(new NavMenuItem[0]));
        internal static DependencyProperty NavigationItemsBottomProperty => navigationItemsBottomProperty;

        public event TypedEventHandler<NavigationFrame, NavMenuItem> ItemSelected;

        public Rect TogglePaneButtonRect { get; private set; }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<NavigationFrame, Rect> TogglePaneButtonRectChanged;

        // Declare the top level nav items

        /// <summary>
        /// Initializes a new instance of the AppShell, sets the static 'Current' reference,
        /// adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        /// provide the nav menu list with the data to display.
        /// </summary>
        public NavigationFrame()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                CheckTogglePaneButtonSizeChanged();

                var currentPageType = GetHostingFrame().CurrentSourcePageType;

                var itemToSelect = NavigationItemsTop.FirstOrDefault(d => d.Page == currentPageType);
                if (itemToSelect != null)
                {
                    NavMenuListTop.SelectedItem = itemToSelect;
                }

                itemToSelect = NavigationItemsBottom.FirstOrDefault(d => d.Page == currentPageType);
                if (itemToSelect != null)
                {
                    NavMenuListBottom.SelectedItem = itemToSelect;
                }
            };

            RootSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (s, a) =>
            {
                // Ensure that we update the reported size of the TogglePaneButton when the SplitView's
                // DisplayMode changes.
                CheckTogglePaneButtonSizeChanged();
            });
        }


        /// <summary>
        /// Public method to allow pages to open SplitView's pane.
        /// Used for custom app shortcuts like navigating left from page's left-most item
        /// </summary>
        public void OpenNavePane()
        {
            TogglePaneButton.IsChecked = true;
            NavPaneDivider.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        #region Navigation

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var frame = GetHostingFrame();
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);

            if (item != null)
            {
                ItemSelected?.Invoke(this, item);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
        }

        #endregion

        /// <summary>
        /// Hides divider when nav pane is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RootSplitView_PaneClosed(SplitView sender, object args)
        {
            NavPaneDivider.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Callback when the SplitView's Pane is toggled closed.  When the Pane is not visible
        /// then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Callback when the SplitView's Pane is toggled opened.
        /// Restores divider's visibility and ensures that margins around the floating hamburger are correctly set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            NavPaneDivider.Visibility = Visibility.Visible;
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            if (handler != null)
            {
                // handler(this, this.TogglePaneButtonRect);
                handler.DynamicInvoke(this, this.TogglePaneButtonRect);
            }
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        private Frame GetHostingFrame()
        {
            var frame = Window.Current.Content.FindVisualChild<Frame>();
            return frame;
        }
    }
}
