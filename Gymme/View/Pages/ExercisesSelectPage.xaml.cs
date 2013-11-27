using System;
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
        }

        private void Exercise_Tap(object sender, GestureEventArgs e)
        {
            ((ExerciseSelectItemVM)((FrameworkElement) sender).DataContext).Choose();
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