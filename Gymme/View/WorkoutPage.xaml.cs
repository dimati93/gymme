using System;
using System.Windows;
using System.Windows.Navigation;
using Gymme.Resources;
using Gymme.ViewModel;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View
{
    public partial class WorkoutPage : PhoneApplicationPage
    {
        private WorkoutPageVM _viewModel;

        public WorkoutPage()
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

        private WorkoutPageVM GetDataContext(string target, long id)
        {
            return new WorkoutPageVM(id);
        }

        private void InitializeAppMenu()
        {
            var addExercise = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Add
                };

            addExercise.Click += AddExercise_Click;
            ApplicationBar.Buttons.Add(addExercise);

            var editWorkoutMenuItem = new ApplicationBarMenuItem(AppResources.WorkoutPage_EditWorkout);
            editWorkoutMenuItem.Click += EditWorkout_Click;
            ApplicationBar.MenuItems.Add(editWorkoutMenuItem);

            var deleteWorkoutMenuItem = new ApplicationBarMenuItem(AppResources.WorkoutPage_DeleteWorkout);
            deleteWorkoutMenuItem.Click += DeleteWorkout_Click;
            ApplicationBar.MenuItems.Add(deleteWorkoutMenuItem);
        }

        private void AddExercise_Click(object sender, EventArgs e)
        {
            NavigationManager.GotoExercisesSelectPage();
        }

        private void EditWorkout_Click(object sender, EventArgs e)
        {
            _viewModel.EditWorkout();
        }
        
        private void DeleteWorkout_Click(object sender, EventArgs e)
        {
            if (_viewModel.DeleteWorkout())
            {
                NavigationManager.GoBack();
            }
        }

        private void Ex_Hold(object sender, GestureEventArgs e)
        {
            ContextMenuService.GetContextMenu((DependencyObject)sender).IsOpen = true;
        }
    }
}