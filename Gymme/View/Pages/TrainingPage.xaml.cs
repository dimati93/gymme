using System;
using System.Windows;
using System.Windows.Navigation;
using Gymme.Data.Repository;
using Gymme.Resources;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Pages
{
    public partial class TrainingPage : PhoneApplicationPage
    {
        public const string FromWorkoutStart = "fromWorkoutStart";
        public const string FromWorkoutContinue = "fromWorkoutContinue";
        public const string ByTraining = "byTraining";
        public const string ByWorkout = "byWorkout";
        public const string GobBackUpdate = "update";

        private TrainingPageVM _viewModel;

        public TrainingPage()
        {
            InitializeComponent();
            InitializeAppMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationManager.GoBackParams == GobBackUpdate)
            {
                if (_viewModel != null)
                {
                    _viewModel.Update();
                    return;
                }
            }

            string target;
            if (!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                NavigationManager.GoBack();
            }

            string id;
            if (!NavigationContext.QueryString.TryGetValue("id", out id))
            {
                NavigationManager.GoBack();
            }
            else
            {
                DataContext = _viewModel = GetDataContext(target, long.Parse(id));
            }
        }

        private TrainingPageVM GetDataContext(string target, long id)
        {
            switch (target)
            {
                case ByWorkout:
                    return new TrainingPageVM(RepoWorkout.Instance.FindById(id)) { BackCount = 1 };
                case ByTraining:
                case GobBackUpdate:
                    return new TrainingPageVM(RepoTraining.Instance.FindById(id)) { BackCount = 1 };
                case FromWorkoutStart:
                    return new TrainingPageVM(RepoWorkout.Instance.FindById(id)) { BackCount = 2 };
                case FromWorkoutContinue:
                    return new TrainingPageVM(RepoTraining.Instance.FindById(id)) { BackCount = 2 };
                default:
                    throw new InvalidOperationException();
            }
        }

        private void InitializeAppMenu()
        {
            var finishExercise = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/appbar.check.rest.png", UriKind.Relative),
                Text = AppResources.Command_Finish
            };

            finishExercise.Click += Finish_Click;
            ApplicationBar.Buttons.Add(finishExercise);

            var skipExercise = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/appbar.stop.rest.png", UriKind.Relative),
                Text = AppResources.Command_Delete
            };

            skipExercise.Click += Delete_Click;
            ApplicationBar.Buttons.Add(skipExercise);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (_viewModel.Delete())
            {
                NavigationManager.GoBack(MainPage.TargetUpcomingList);
            }
        }

        private void Finish_Click(object sender, EventArgs e)
        {
            _viewModel.Finish();
            NavigationManager.GoBack(MainPage.TargetUpcomingList, _viewModel.BackCount);
        }

        private void Ex_Hold(object sender, GestureEventArgs e)
        {
            ContextMenuService.GetContextMenu((DependencyObject)sender).IsOpen = true;
        }
    }
}