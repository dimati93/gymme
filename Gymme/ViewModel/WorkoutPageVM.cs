using System.Windows;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class WorkoutPageVM : Base.ViewModel
    {
        private Workout _workout;

        private bool _exercisesLoaded;

        public WorkoutPageVM(long id)
        {
            _workout = RepoWorkout.Instance.FindById(id);
        }

        public string Title 
        {
            get
            {
                return _workout.Title;
            }
        }

        public void EditWorkout()
        {
            NavigationManager.GotoEditWorkout(_workout.Id);
        }

        public bool DeleteWorkout()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Workout_DeleteWarning, 
                                                      Resources.AppResources.Workout_DeleteWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                RepoWorkout.Instance.Delete(_workout);
                return true;
            }

            return false;
        }

        public void Update()
        {
            NotifyPropertyChanged("Title");
        }
    }
}