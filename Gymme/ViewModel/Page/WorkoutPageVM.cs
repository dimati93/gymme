using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

using Gymme.ViewModel.Statistics;

namespace Gymme.ViewModel.Page
{
    public class WorkoutPageVM : Base.ViewModel
    {
        private readonly Workout _workout;
        private WorkoutStatistics _workoutStatistics;
        private int _selectedPageIndex;

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

        public Action UpdateAppMenu { get; set; }

        public WorkoutStatistics Statistics
        {
            get
            {
                return _workoutStatistics ?? (_workoutStatistics = new WorkoutStatistics(_workout));
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

        public void EditWorkout()
        {
            NavigationManager.GotoEditWorkout(_workout.Id);
        }

        public bool DeleteWorkout()
        {
            MessageBoxResult result = MessageBox.Show(AppResources.Workout_DeleteWarning,
                                                      AppResources.Workout_DeleteWarningTitle,
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
            foreach (Exercise exercise in _workout.Exercises.OrderBy(x => x.Order))
            {
                Exercises.Add(new ExerciseVM(exercise, this));
            }

            NotifyPropertyChanged("IsExercisesEmpty");
            if (UpdateAppMenu != null)
            {
                UpdateAppMenu();
            }
        }

        public void StartWorkout()
        {
            Training tr = RepoTraining.Instance.FindLastByWorkoutId(Item.Id);
            if (tr != null && tr.Status == TrainingStatus.Started)
            {
                if (Intelligent.IsTrainingExperate(tr))
                {
                    tr.Status = TrainingStatus.Finished;
                    RepoTraining.Instance.Save(tr);
                    NavigationManager.GotoTrainingPageFromWorkout(Item.Id, true);
                    return;
                }

                NavigationManager.GotoTrainingPageFromWorkout(tr.Id, false);
                return;
            }

            NavigationManager.GotoTrainingPageFromWorkout(Item.Id, true);
        }

        public void ApplyReorder(ExerciseVM exercise)
        {
            if (Exercises.Count < 2)
                return;

            int i = Exercises.IndexOf(exercise);
            if (i == 0)
            {
                exercise.Order = Exercises[1].Order - 1;
            }
            else
            {
                if (i == Exercises.Count - 1)
                {
                    exercise.Order = Exercises[i - 1].Order + 1;
                }
                else
                {
                    exercise.Order = (Exercises[i - 1].Order + Exercises[i + 1].Order) / 2;
                }
            }

            exercise.Save();
        }
    }
}