using System;
using System.Windows;
using System.Windows.Navigation;
using Gymme.Data.Repository;
using Gymme.Resources;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View
{
    public partial class ExecutionPage : PhoneApplicationPage
    {
        public const string FromWorkoutPage = "fromWorkoutPage";

        private TrainingPageVM _viewModel;

        public ExecutionPage()
        {
            InitializeComponent();
            InitializeAppMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string target;
            if (!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                if (_viewModel != null)
                {
                    _viewModel.Update();
                    return;
                }
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
                case FromWorkoutPage:
                    return new TrainingPageVM(RepoWorkout.Instance.FindById(id));
                default:
                    throw new InvalidOperationException();
            }
        }

        private void InitializeAppMenu()
        {
            //var addExercise = new ApplicationBarIconButton
            //    {
            //        IconUri = new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative),
            //        Text = AppResources.Command_Add
            //    };

            //addExercise.Click += AddExercise_Click;
            //ApplicationBar.Buttons.Add(addExercise);

            var finishTrainingMenuItem = new ApplicationBarMenuItem(AppResources.Training_Finish);
            finishTrainingMenuItem.Click += Finish_Click;
            ApplicationBar.MenuItems.Add(finishTrainingMenuItem);
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