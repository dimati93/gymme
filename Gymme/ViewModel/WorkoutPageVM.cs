using System.Collections.ObjectModel;
using System.Windows;

using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class WorkoutPageVM : Base.ViewModel
    {
        private readonly Workout _workout;

        public WorkoutPageVM(long id)
        {
            _workout = RepoWorkout.Instance.FindById(id);
            Exercises = new ObservableCollection<ExerciseVM>();
            Update();
        }

        public Workout Item { get { return _workout; } }

        public string Title
        {
            get
            {
                return _workout.Title;
            }
        }

        public ObservableCollection<ExerciseVM> Exercises { get; set; }

        public bool IsExercisesEmpty { get { return Exercises.Count == 0; } }

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
            Exercises.Clear();
            foreach (Exercise exercise in _workout.Exercises)
            {
                Exercises.Add(new ExerciseVM(exercise, this));
            }
        }
    }
}