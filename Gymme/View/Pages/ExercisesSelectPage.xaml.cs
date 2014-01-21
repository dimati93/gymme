using System;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Gymme.Resources;
using Gymme.View.Controls;
using Gymme.ViewModel;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Pages
{
    public partial class ExercisesSelectPage : PhoneApplicationPage
    {
        private ExercisesSelectVM _viewModel;

        public ExercisesSelectPage()
        {
            InitializeComponent();
            InitializeAppMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = _viewModel = new ExercisesSelectVM(long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]));
            LoadContent();
        }

        private void LoadContent()
        {
            ContentPanel.Children.Clear();
            var selector = ExerciseSelector.Create(_viewModel.WorkoutId);
            ContentPanel.Children.Add(selector);
            Dispatcher.BeginInvoke(() => selector.ScrollTo(ExerciseSelector.LastChoosen));
        }

        private void InitializeAppMenu()
        {
            var newExercise = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/appbar.edit.rest.png", UriKind.Relative),
                Text = AppResources.Command_New
            };

            newExercise.Click += NewExercise_Click;
            ApplicationBar.Buttons.Add(newExercise);
        }


        private void NewExercise_Click(object sender, EventArgs e)
        {
            NavigationManager.GotoAddExercisePage(_viewModel.WorkoutId);
        }
    }
}