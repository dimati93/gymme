using Gymme.Data.AuxModels;
using Gymme.Data.Interfaces;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.AddEdit
{
    public class AddEditExerciseVM : AddEditVM, IExercise
    {
        private readonly Exercise _item;

        private string _name = string.Empty;
        private string _category = string.Empty;

        public AddEditExerciseVM(long workoutId) 
            : base(false)
        {
            _item = new Exercise {IdWorkout = workoutId};
            PageName = Resources.AppResources.AddEdit_Exercise;
        }

        public AddEditExerciseVM(long workoutId, PersetExercise exercise)
            : base(false)
        {
            _item = new Exercise(exercise) { IdWorkout = workoutId };
            PageName = Resources.AppResources.AddEdit_Exercise;
            Rollback();
        }
        
        public AddEditExerciseVM(Exercise exercise)
            : base(true)
        {
            _item = exercise;
            PageName = Resources.AppResources.AddEdit_Exercise;
            Rollback();
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }

        }

        public string Category
        {
            get
            {
                return _category;
            }

            set
            {
                if (_category != value)
                {
                    _category = value;
                    NotifyPropertyChanged("Category");
                }
            }
        }

        public override sealed void Rollback()
        {
            Name = _item.Name;
            Category = _item.Category;
        }

        public override void Commit()
        {
            _item.Name = Name;
            _item.Category = Category;
            if (IsEdit)
            {
                RepoExercise.Instance.Save(_item);
            }
            else
            {
                RepoWorkout.Instance.FindById(_item.IdWorkout).Exercises.Add(_item);
                Data.Core.DatabaseContext.Instance.SubmitChanges();
            }
        }
    }
}
