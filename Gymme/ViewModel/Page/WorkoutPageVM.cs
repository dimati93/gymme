using System;
using System.Collections.ObjectModel;
using System.Windows;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.Page
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
        public Action UpdateAppMenu { get; set; }

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
            foreach (Exercise exercise in _workout.Exercises)
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
            if (tr == null || Intelligent.IsTrainingExperate(tr))
            {
                if (tr != null)
                {
                    tr.Status = TrainingStatus.Finished;
                    RepoTraining.Instance.Save(tr);
                }

                NavigationManager.GotoTrainingPageFromWorkout(Item.Id, true);
                return;
            }

            NavigationManager.GotoTrainingPageFromWorkout(tr.Id, false);
        }
    }
}