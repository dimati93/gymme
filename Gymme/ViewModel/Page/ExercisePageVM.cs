using System.Windows;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;
using Gymme.ViewModel.Statistics;

namespace Gymme.ViewModel.Page
{
    public class ExercisePageVM : Base.ViewModel
    {
        private readonly Exercise _exercise;
        private readonly Workout _workout;
        private ExerciseStatistics _exerciseStatistics;
        private int _selectedPageIndex;

        public ExercisePageVM(long id)
        {
            _exercise = RepoExercise.Instance.FindById(id);
            _workout = RepoWorkout.Instance.FindById(_exercise.IdWorkout);
        }

        public Exercise Item { get { return _exercise; } }

        public string WorkoutTitle
        {
            get
            {
                return _workout.Title;
            }
        }

        public string Name
        {
            get
            {
                return _exercise.Name;
            }
        }

        public string Category
        {
            get
            {
                return _exercise.Category;
            }
        }

        public string WeightMode
        {
            get
            {
                return  _exercise.WithoutWeight ? AppResources.AEExercise_WithoutWeight : AppResources.AEExercise_WithWeight;
            }
        }

        public ExerciseStatistics Statistics
        {
            get
            {
                return _exerciseStatistics ?? (_exerciseStatistics = new ExerciseStatistics(_exercise));
            }
        }

        public int SelectedPageIndex
        {
            get { return _selectedPageIndex; }
            set
            {
                _selectedPageIndex = value;
                if (_selectedPageIndex == 1 && !Statistics.IsLoaded)
                {
                    Statistics.LoadStatistics();
                }
            }
        }

        public void EditExercise()
        {
            NavigationManager.GotoEditExercise(_exercise.Id);
        }

        public bool DeleteExercise()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Exercise_DeleteWarning,
                                                      Resources.AppResources.Exercise_DeleteWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _workout.Exercises.Remove(_exercise);
                Data.Core.DatabaseContext.Instance.SubmitChanges();
                RepoExercise.Instance.Delete(_exercise);
                return true;
            }

            return false;
        }

        public void Update()
        {
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Category");
        }
    }
}