using System.Windows.Input;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.ViewModel.Page;

namespace Gymme.ViewModel
{
    public class ExerciseVM : Base.ViewModel
    {
        private readonly Exercise _exercise;
        private readonly WorkoutPageVM _wpageVM;

        private string _title;
        private string _note;

        private bool _exercisesLoaded;

        public ExerciseVM()
            : this (new Exercise(), null)
        {
        }

        public ExerciseVM(Exercise exercise, WorkoutPageVM wpageVM)
        {
            _exercise = exercise;
            _wpageVM = wpageVM;
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
                return GetOrCreateCommand("EditCommand", EditExercise);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return GetOrCreateCommand("DeleteCommand", DeleteExercise);
            }
        }

        public double Order
        {
            get { return _exercise.Order.HasValue ? _exercise.Order.Value : _exercise.Id; }
            set { _exercise.Order = value; }
        }

        private void GotoPageView()
        {
            NavigationManager.GotoExercisePage(_exercise.Id);
        }

        private void EditExercise()
        {
            NavigationManager.GotoEditExercise(_exercise.Id);
        }

        private void DeleteExercise()
        {
            RepoWorkout.Instance.FindById(_wpageVM.Item.Id).Exercises.Remove(_exercise);
            Data.Core.DatabaseContext.Instance.SubmitChanges();
            RepoExercise.Instance.Delete(_exercise);
            _wpageVM.Update();
        }

        public void Save()
        {
            RepoExercise.Instance.Save(_exercise);
        }
    }
}