using System.Diagnostics;
using System.Globalization;
using System.Linq;

using System;
using System.Windows;
using System.Windows.Navigation;

using Gymme.View.Pages;
using AEC = Gymme.View.Controls.AddEditChooser;

namespace Gymme
{
    public static class NavigationManager
    {
        private static NavigationService _navigationService;

        public static string GoBackParams { get; set; }

        public static void GotoAddWorkout()
        {
            InitializeNavigation();
            Navigate("/AddEdit", AEC.Variant.AddWorkout);
        }

        public static void GotoEditWorkout(long id)
        {
            InitializeNavigation();
            Navigate("/AddEdit", AEC.Variant.EditWorkout, Id(id));
        }

        public static void GotoWorkoutPage(long id)
        {
            InitializeNavigation();
            Navigate("/Workout", "none", Id(id));
        }

        public static void GotoExercisesSelectPage(long workoutId)
        {
            InitializeNavigation();
            Navigate("/ExercisesSelect", "none", Param(AEC.Param.WorkoutId, workoutId));
        }
        
        public static void GotoAddExercisePage(long workoutId)
        {
            InitializeNavigation();
            Navigate("/AddEdit", AEC.Variant.AddExercise, Param(AEC.Param.WorkoutId, workoutId));
        }

        public static void GotoAddExercisePage(long workoutId, long id)
        {
            InitializeNavigation();
            Navigate("/AddEdit", AEC.Variant.AddExercise, Id(id) + Param(AEC.Param.WorkoutId, workoutId));
        }

        public static void GotoEditExercise(long id)
        {
            InitializeNavigation();
            Navigate("/AddEdit", AEC.Variant.EditExercise, Id(id));
        }

        public static void GotoExercisePage(long id)
        {
            InitializeNavigation();
            Navigate("/Exercise", "none", Id(id));
        }

        public static void GotoTrainingPageByTrainingId(long id)
        {
            InitializeNavigation();
            Navigate("/Training", TrainingPage.ByTraining, Id(id));
        }

        public static void GotoTrainingPageByWorkoutId(long id)
        {
            InitializeNavigation();
            Navigate("/Training", TrainingPage.ByWorkout, Id(id));
        } 

        public static void GotoTrainingPageFromWorkout(long id, bool startNew)
        {
            InitializeNavigation();
            Navigate("/Training", startNew ? TrainingPage.FromWorkoutStart : TrainingPage.FromWorkoutContinue, Id(id));
        }

        public static void GotoExecutePage(long id)
        {
            InitializeNavigation();
            Navigate("/Execution", "none", Id(id));
        }

        public static void GotoHelp()
        {
            InitializeNavigation();
            Navigate("/Help");
        }

        public static void GoBack(string parameters = null, int times = 1)
        {
            while (times > 1 && _navigationService.BackStack.Any())
            {
                _navigationService.RemoveBackEntry();
                times--;
            }

            if (_navigationService.CanGoBack) 
            {
                _navigationService.GoBack();
            }

            GoBackParams = parameters ?? string.Empty;
        }

        public static void SetNavigationService(NavigationService ns)
        {
            if (ns == null)
            {
                throw new InvalidOperationException("NavigationService cannot be null");
            }

            _navigationService = ns;
        }

        private static void InitializeNavigation()
        {
            GoBackParams = null;
        }

        #region BuildUri

        private static void Navigate(string path, string navtgt, params string[] query)
        {
            _navigationService.Navigate(BuildUri(path, navtgt, string.Join("", query)));
        }

        private static void Navigate(string path, string navtgt = null)
        {
            try
            {
                _navigationService.Navigate(BuildUri(path, navtgt));
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show("Navigatition failed: " + path);
                Debug.WriteLine(e);
#endif
            }
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
