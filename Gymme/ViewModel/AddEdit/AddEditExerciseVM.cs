using System.Collections.Generic;
using Gymme.Data.AuxModels;
using Gymme.Data.Core;
using Gymme.Data.Interfaces;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.AddEdit
{
    public class AddEditExerciseVM : AddEditVM, IExercise
    {
        private readonly Exercise _item;

        private string _name = string.Empty;
        private string _category = string.Empty;
        private bool _withoutWeight;

        public AddEditExerciseVM(long workoutId)
            : base(false)
        {
            _item = new Exercise { IdWorkout = workoutId };
            PageName = AppResources.AddEdit_Exercise;
        }

        public AddEditExerciseVM(long workoutId, PersetExercise exercise)
            : base(false)
        {
            _item = new Exercise(exercise) { IdWorkout = workoutId };
            PageName = AppResources.AddEdit_Exercise;
            Rollback();
        }

        public AddEditExerciseVM(Exercise exercise)
            : base(true)
        {
            _item = exercise;
            PageName = AppResources.AddEdit_Exercise;
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

        public List<string> Categories
        {
            get
            {
                if (!ExerciseData.Instance.IsDataLoaded)
                {
                    ExerciseData.Instance.LoadData();
                }

                return ExerciseData.Instance.PersetCategories;
            }
        }

        public string WithoutWeight
        {
            get
            {
                return _withoutWeight ? AppResources.AEExercise_WithoutWeight : AppResources.AEExercise_WithWeight;
            }

            set
            {
                var newValue = value == AppResources.AEExercise_WithoutWeight;
                if (_withoutWeight != newValue)
                {
                    _withoutWeight = newValue;
                    NotifyPropertyChanged("WithoutWeight");
                    NotifyPropertyChanged("WeightModeText");
                }
            }
        }

        public string[] WeightModes
        {
            get
            {
                return new[] { AppResources.AEExercise_WithWeight, AppResources.AEExercise_WithoutWeight };
            }
        }

        public override sealed void Rollback()
        {
            Name = _item.Name;
            Category = _item.Category;
            _withoutWeight = _item.WithoutWeight;
        }

        public override void Commit()
        {
            _item.Name = Name;
            _item.Category = Category;
            _item.WithoutWeight = _withoutWeight;
            if (IsEdit)
            {
                RepoExercise.Instance.Save(_item);
            }
            else
            {
                RepoWorkout.Instance.FindById(_item.IdWorkout).Exercises.Add(_item);
                DatabaseContext.Instance.SubmitChanges();
            }
        }
    }
}
