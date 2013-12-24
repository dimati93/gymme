using System.Globalization;
using System.Linq;

using System;
using System.Windows.Navigation;

using Gymme.View.Pages;
using AEC = Gymme.View.Controls.AddEditChooser;

namespace Gymme
{
    public static class NavigationManager
    {
        private const string AddEditPagePath = "/View/Pages/AddEditPage.xaml";
        private const string WorkoutPagePath = "/View/Pages/WorkoutPage.xaml";
        private const string ExercisePagePath = "/View/Pages/ExercisePage.xaml";
        private const string TrainingPagePath = "/View/Pages/TrainingPage.xaml";
        private const string ExecutionPagePath = "/View/Pages/ExecutionPage.xaml";
        private const string ExercisesSelectPath = "/View/Pages/ExercisesSelectPage.xaml";
        private const string HelpPagePath = "/View/Pages/HelpPage.xaml";

        private static NavigationService NavigationService;

        public static string GoBackParams { get; set; }

        public static void GotoAddWorkout()
        {
            InitializeNavigation();
            Navigate(AddEditPagePath, AEC.Variant.AddWorkout);
        }

        public static void GotoEditWorkout(long id)
        {
            InitializeNavigation();
            Navigate(AddEditPagePath, AEC.Variant.EditWorkout, Id(id));
        }

        public static void GotoWorkoutPage(long id)
        {
            InitializeNavigation();
            Navigate(WorkoutPagePath, "none", Id(id));
        }

        public static void GotoExercisesSelectPage(long workoutId)
        {
            InitializeNavigation();
            Navigate(ExercisesSelectPath, "none", Param(AEC.Param.WorkoutId, workoutId));
        }

        public static void GotoAddExercisePage(long workoutId)
        {
            InitializeNavigation();
            Navigate(AddEditPagePath, AEC.Variant.AddExercise, Param(AEC.Param.WorkoutId, workoutId));
        }

        public static void GotoAddExercisePage(long workoutId, long id)
        {
            InitializeNavigation();
            Navigate(AddEditPagePath, AEC.Variant.AddExercise, Id(id) + Param(AEC.Param.WorkoutId, workoutId));
        }

        public static void GotoEditExercise(long id)
        {
            InitializeNavigation();
            Navigate(AddEditPagePath, AEC.Variant.EditExercise, Id(id));
        }

        public static void GotoExercisePage(long id)
        {
            InitializeNavigation();
            Navigate(ExercisePagePath, "none", Id(id));
        }

        public static void GotoTrainingPageByTrainingId(long id)
        {
            InitializeNavigation();
            Navigate(TrainingPagePath, TrainingPage.ByTraining, Id(id));
        }

        public static void GotoTrainingPageByWorkoutId(long id)
        {
            InitializeNavigation();
            Navigate(TrainingPagePath, TrainingPage.ByWorkout, Id(id));
        } 

        public static void GotoTrainingPageFromWorkout(long id, bool startNew)
        {
            InitializeNavigation();
            Navigate(TrainingPagePath, startNew ? TrainingPage.FromWorkoutStart : TrainingPage.FromWorkoutContinue , Id(id));
        }

        public static void GotoExecutePage(long id)
        {
            InitializeNavigation();
            Navigate(ExecutionPagePath, "none", Id(id));
        }

        public static void GotoHelp()
        {
            InitializeNavigation();
            Navigate(HelpPagePath);
        }

        public static void GoBack(string parameters = null, int times = 1)
        {
            while (times > 1 && NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
                times--;
            }

            if (NavigationService.CanGoBack) 
            {
                NavigationService.GoBack();
            }

            GoBackParams = parameters ?? string.Empty;
        }

        public static void SetNavigationService(NavigationService ns)
        {
            if (ns == null)
            {
                throw new InvalidOperationException("NavigationService cannot be null");
            }

            NavigationService = ns;
        }

        private static void InitializeNavigation()
        {
            GoBackParams = null;
        }

        #region BuildUri

        private static void Navigate(string path, string navtgt, string query)
        {
            NavigationService.Navigate(BuildUri(path, navtgt, query));
        }

        private static void Navigate(string path, string navtgt = null)
        {
            NavigationService.Navigate(BuildUri(path, navtgt));
        }

        private static string Id(long id)
        {
            return Param("id", id);
        }

        private static string Param(string paramName, object param)
        {
            return string.Format(CultureInfo.InvariantCulture, "&{0}={1}", paramName, param);
        }

        private static Uri BuildUri(string path, string navtgt, string query)
        {
            return new Uri(string.Format("{0}{1}", GetBaseUriString(path, navtgt), query), UriKind.Relative);
        }

        private static Uri BuildUri(string path, string navtgt)
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
