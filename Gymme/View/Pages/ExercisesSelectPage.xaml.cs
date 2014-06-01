using System;
using System.Windows.Navigation;
using Gymme.Resources;
using Gymme.View.Controls;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
            FlurryWP8SDK.Api.LogPageView();

            DataContext = _viewModel = new ExercisesSelectVM(long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]));
            LoadContent();
        }

        private void LoadContent()
        {
            ContentPanel.Children.Clear();
            var selector = ExerciseSelector.Create(_viewModel.WorkoutId);
            ContentPanel.Children.Add(selector);
            if (ExerciseSelector.LastChoosen != null)
            {
                Dispatcher.BeginInvoke(() => selector.BringIntoView(ExerciseSelector.LastChoosen));
            }
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