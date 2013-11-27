using System;
using System.Windows.Navigation;
using Gymme.Data.Repository;
using Gymme.Resources;
using Gymme.View.Controls;
using Gymme.ViewModel.AddEdit;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Gymme.View.Pages
{
    public partial class AddEditPage : PhoneApplicationPage
    {
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
                case AddEditChooser.Variant.AddWorkout: 
                    return new AddEditWorkoutVM 
                    { 
                        Control = new AEWorkout(),
                        BackTarget = MainPage.TargetWorkoutsList 
                    };
                case AddEditChooser.Variant.AddExercise:
                    return new AddEditExerciseVM(long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]))
                    {
                        Control = new AEExercise(),
                        BackCount = 2
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
                case AddEditChooser.Variant.EditWorkout: 
                    return new AddEditWorkoutVM(id)
                        {
                            Control = new AEWorkout()
                        };
                case AddEditChooser.Variant.AddExercise:
                    return new AddEditExerciseVM
                        (
                            long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]),
                            Gymme.Resources.ExerciseData.Instance.PersetExercises[(int)id]
                        )
                        {
                            Control = new AEExercise(),
                            BackCount = 2
                        };
                case AddEditChooser.Variant.EditExercise:
                    return new AddEditExerciseVM
                        (
                            RepoExercise.Instance.FindById(id)
                        )
                        {
                            Control = new AEExercise()
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
            NavigationManager.GoBack(_viewModel.BackTarget, _viewModel.BackCount);
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