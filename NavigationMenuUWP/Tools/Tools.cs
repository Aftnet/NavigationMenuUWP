using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace NavigationMenuUWP.Tools
{
    public static class Tools
    {
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
    }
}
