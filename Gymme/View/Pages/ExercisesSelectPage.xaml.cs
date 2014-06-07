using System;
using System.ComponentModel;
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

        private ExerciseSelector _selector;
        private ExerciseSearch _search;

        public ExercisesSelectPage()
        {
            InitializeComponent();
            InitializeAppMenu(true);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FlurryWP8SDK.Api.LogPageView();

            DataContext = _viewModel = new ExercisesSelectVM(long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]));
            LoadContent();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (_search != null)
            {
                LayoutRoot.Children.Remove(_search);
                _search = null;

                _selector.IsEnabled = true;

                ApplicationBar.Buttons.Clear();
                InitializeAppMenu(true);
                e.Cancel = true;
            }

            base.OnBackKeyPress(e);
        }

        private void LoadContent()
        {
            ContentPanel.Children.Clear();
            _selector = ExerciseSelector.Create(_viewModel.WorkoutId);
            ContentPanel.Children.Add(_selector);
            if (ExerciseSelector.LastChoosen != null)
            {
                Dispatcher.BeginInvoke(() => _selector.BringIntoView(ExerciseSelector.LastChoosen));
            }
        }

        private void InitializeAppMenu(bool searchEnabled)
        {
            if (searchEnabled)
            {
                var searchExercise = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.feature.search.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Search
                };

                searchExercise.Click += SearchExercise_Click;
                ApplicationBar.Buttons.Add(searchExercise);
            }

            var newExercise = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/appbar.edit.rest.png", UriKind.Relative),
                Text = AppResources.Command_New
            };

            newExercise.Click += NewExercise_Click;
            ApplicationBar.Buttons.Add(newExercise);
        }

        private void SearchExercise_Click(object sender, EventArgs e)
        {
            _search = new ExerciseSearch(_selector.ViewModel);
            LayoutRoot.Children.Add(_search);

            _selector.IsEnabled = false;

            ApplicationBar.Buttons.Clear();
            InitializeAppMenu(false);
        }

        private void NewExercise_Click(object sender, EventArgs e)
        {
            NavigationManager.GotoAddExercisePage(_viewModel.WorkoutId);
        }
    }
}