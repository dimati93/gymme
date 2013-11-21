using System.Linq;
using Gymme.View;
using System;
using System.Windows.Navigation;

namespace Gymme
{
    public static class NavigationManager
    {
        private const string AddEditPagePath = "/View/AddEditPage.xaml";
        private const string WorkoutPagePath = "/View/WorkoutPage.xaml";
        private const string ExercisesSelectPath = "/View/ExercisesSelectPage.xaml";

        private static NavigationService NavigationService;
        private static string _gobackParams;
        private static int _gobackTimes;

        public static void GotoAddWorkout()
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AddEditPage.VariantWorkout));
        }

        public static void GotoEditWorkout(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AddEditPage.VariantWorkout, id));
        }

        public static void GotoWorkoutPage(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(WorkoutPagePath, "none", id));
        }

        public static void GotoExercisesSelectPage()
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(ExercisesSelectPath, "none"));
        }

        public static void GotoAddExercisePage(bool scipCurrentPageOnGoBack = false)
        {
            InitializeNavigation();
            if (scipCurrentPageOnGoBack)
            {
                _gobackTimes++;
            }

            NavigationService.Navigate(BuildUri(ExercisesSelectPath, AddEditPage.VariantExercise));
        }

        public static void GoBack(string parameters = null)
        {
            while (_gobackTimes > 0 && NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
                _gobackTimes--;
            }

            if (NavigationService.CanGoBack) 
            {
                NavigationService.GoBack();
            }

            _gobackParams = parameters ?? string.Empty;
        }

        public static void SetNavigationService(NavigationService ns)
        {
            if (ns == null)
            {
                throw new InvalidOperationException("NavigationService cannot be null");
            }

            NavigationService = ns;
        }

        public static string GetGoBackParams()
        {
            return _gobackParams;
        }

        private static void InitializeNavigation()
        {
            _gobackParams = null;
        }

        #region BuildUri

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

        #endregion
    }
}
