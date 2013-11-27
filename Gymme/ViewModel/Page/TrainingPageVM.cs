using System;
using System.Collections.ObjectModel;
using System.Windows;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Page
{
    public class TrainingPageVM : Base.ViewModel
    {
        private Training _training;
        private Workout _workout;

        public TrainingPageVM(Workout workout)
        {
            Training training = new Training(workout);
            RepoTraining.Instance.Save(training);

            foreach (var exercise in workout.Exercises)
            {
                training.Exercises.Add(new TrainingExercise(exercise));
            }
            Data.Core.DatabaseContext.Instance.SubmitChanges();

            Initialize(training);
            BackCount = 2;
        }

        public TrainingPageVM(Training training)
        {
            Initialize(training);
            BackCount = 1;
        }

        private void Initialize(Training training)
        {
            _training = training;
            _workout = RepoWorkout.Instance.FindById(Training.IdWorkout);

            Exercises = new ObservableCollection<TrainingExerciseVM>();
            Update();
        }

        public int BackCount { get; set; }

        public string Title { get { return _workout.Title; } }

        public TimeSpan Time { get { return new TimeSpan(0, 10, 29); } }

        public ObservableCollection<TrainingExerciseVM> Exercises { get; private set; }

        public Training Training
        {
            get { return _training; }
        }

        public void Update()
        {
            Exercises.Clear();
            Training.Exercises.Load();
            foreach (var trainingExercise in Training.Exercises)
            {
                Exercises.Add(new TrainingExerciseVM(trainingExercise, () => NotifyPropertyChanged("Exercises")));
            }

            NotifyPropertyChanged("Exercises");
        }

        public void Finish()
        {
            Training.Status = TrainingStatus.Finished;
            RepoTraining.Instance.Save(Training);
        }

        public bool Delete()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Training_DeleteWarning,
                                                      Resources.AppResources.Training_DeleteWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                RepoTraining.Instance.Delete(Training);
                return true;
            }

            return false;
        }
    }
}