using Gymme.View;
using System;
using System.Windows.Navigation;

namespace Gymme
{
    public static class NavigationManager
    {
        private const string AddEditPagePath = "/View/AddEditPage.xaml";

        private static NavigationService NavigationService;

        public static void GotoAddWorkout()
        {
            NavigationService.Navigate(BuildUri(AddEditPagePath, AddEditPage.VariantAddWorkout));
        }

        private static Uri BuildUri(string path, object navtgt = null)
        {
            string uri = path;
            if (navtgt != null)
            {
                uri += string.Format("?navtgt={0}", navtgt);
            }

            return new Uri(uri, UriKind.Relative);
        }

        public static void GoBack()
        {
            if (NavigationService.CanGoBack) 
            {
                NavigationService.GoBack();
            }
        }

        public static void SetNavigationService(NavigationService ns)
        {
            if (ns == null)
            {
                throw new InvalidOperationException("NavigationService cannot be null");
            }

            NavigationService = ns;
        }
    }
}
