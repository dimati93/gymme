using System;
using System.Windows.Navigation;
using Gymme.Resources;
using Microsoft.Phone.Controls;

using Gymme.ViewModel.AddEdit;
using Microsoft.Phone.Shell;

namespace Gymme.View
{
    public partial class AddEditPage : PhoneApplicationPage
    {
        public const string VariantWorkout = "workout";
        public const string VariantExercise = "exercise";

        private AddEditVM _viewModel;

        public AddEditPage()
        {
            InitializeComponent();
            InitializeAppMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string target;
            if(!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                NavigationManager.GoBack();
            }

            string id;
            if (!NavigationContext.QueryString.TryGetValue("id", out id))
            {
                DataContext = _viewModel = GetDataContext(target);
            }
            else
            {
                DataContext = _viewModel = GetDataContext(target, long.Parse(id));
            }
        }

        private AddEditVM GetDataContext(string target)
        {
            switch (target)
            {
                case VariantWorkout: 
                    return new AddEditWorkoutVM 
                    { 
                        Control = new AEWorkout(),
                        BackTarget = MainPage.TargetWorkoutsList 
                    };
                default: 
                    NavigationManager.GoBack();
                    return null;
            }
        }
        
        private AddEditVM GetDataContext(string target, long id)
        {
            switch (target)
            {
                case VariantWorkout: 
                    return new AddEditWorkoutVM(id)
                    {
                        Control = new AEWorkout()
                    };
                default: 
                    NavigationManager.GoBack();
                    return null;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ((IAEView)_viewModel.Control).UpdateDataSources();
            _viewModel.Commit();
            NavigationManager.GoBack(_viewModel.BackTarget);
        }

        private void InitializeAppMenu()
        {
            var saveAction = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/appbar.save.rest.png", UriKind.Relative),
                Text = AppResources.Command_Save
            };

            saveAction.Click += SaveButton_Click;
            ApplicationBar.Buttons.Add(saveAction);
        }
    }
}