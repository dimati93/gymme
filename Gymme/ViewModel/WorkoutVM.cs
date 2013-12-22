using System.Windows;
using System.Windows.Input;
using Gymme.Data.Core;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class WorkoutVM : Base.ViewModel
    {
        private readonly Workout _workout;
        private readonly MainViewModel _mainVM;

        private string _title;
        private string _note;

        private bool _exercisesLoaded;

        public WorkoutVM()
            : this (new Workout(), null)
        {
        }

        public WorkoutVM(Workout workout, MainViewModel mainVM)
        {
            _workout = workout;
            _mainVM = mainVM;
        }

        public string Title 
        {
            get
            {
                return _workout.Title;
            }
        }

        public string Note
        {
            get
            {
                return _workout.Note;
            }
        }

        public ICommand GotoPageViewCommand
        {
            get
            {
                return GetOrCreateCommand("GotoPageViewCommand", GotoPageView);
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return GetOrCreateCommand("EditCommand", EditWorkout);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return GetOrCreateCommand("DeleteCommand", DeleteWorkout);
            }
        }

        private void GotoPageView()
        {
            NavigationManager.GotoWorkoutPage(_workout.Id);
        }

        private void EditWorkout()
        {
            NavigationManager.GotoEditWorkout(_workout.Id);
        }

        private void DeleteWorkout()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Workout_DeleteWarning, 
                                                      Resources.AppResources.Workout_DeleteWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                RepoWorkout.Instance.Delete(_workout);
                _mainVM.LoadData();
            }
        }
    }
}
