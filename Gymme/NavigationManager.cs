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

        public static void GotoEditWorkout(long id)
        {
            NavigationService.Navigate(BuildUri(AddEditPagePath, AddEditPage.VariantAddWorkout, id));
        }

        private static Uri BuildUri(string path, string navtgt, long id)
        {
            return new Uri(string.Format("{0}&id={1}", GetBaseUriString(path, navtgt), id), UriKind.Relative);
        }

        private static Uri BuildUri(string path, string navtgt = null)
        {
           return new Uri(GetBaseUriString(path, navtgt), UriKind.Relative);
        }

        private static string GetBaseUriString(string path, string navtgt)
        {
            string uri = path;
            if (navtgt != null)
            {
                uri += string.Format("?navtgt={0}", navtgt);
            }
            return uri;
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
