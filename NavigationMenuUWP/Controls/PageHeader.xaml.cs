using NavigationMenuUWP.Tools;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NavigationMenuUWP.Controls
{
    public sealed partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            this.InitializeComponent();

            this.Loaded += (s, a) =>
            {
                var frame = Window.Current.Content.FindVisualChild<NavigationFrame>();
                frame.TogglePaneButtonRectChanged += Current_TogglePaneButtonSizeChanged;
                this.titleBar.Margin = new Thickness(frame.TogglePaneButtonRect.Right, 0, 0, 0);
            };
        }

        private void Current_TogglePaneButtonSizeChanged(NavigationFrame sender, Rect e)
        {
            this.titleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }

        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(UIElement), typeof(PageHeader), new PropertyMetadata(DependencyProperty.UnsetValue));
    }
}
