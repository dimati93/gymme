using System.Windows;
using System.Windows.Media;

namespace Gymme.View.Helpers
{
    public static class ParentOfTypeExtention
    {
        public static T ParentOfType<T>(this DependencyObject control) where T : DependencyObject
        {
            var currentItem = VisualTreeHelper.GetParent(control);
            while (currentItem != null)
            {
                var searchParent = currentItem as T;
                if (searchParent != null)
                {
                    return searchParent;
                }

                currentItem = VisualTreeHelper.GetParent(currentItem);
            } 

            return null;
        }
    }
}