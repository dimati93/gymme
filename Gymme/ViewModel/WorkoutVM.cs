using System.Windows;
using System.Windows.Input;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class WorkoutVM : Base.ViewModelBase
    {
        private Workout _workout;
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
            RollBack();
        }

        public string Title 
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }

        }

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                if (_note != value)
                {
                    _note = value;
                    NotifyPropertyChanged("Note");
                }
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

        private void RollBack()
        {
            Title = _workout.Title;
            Note = _workout.Note;
        }        
    }
}
