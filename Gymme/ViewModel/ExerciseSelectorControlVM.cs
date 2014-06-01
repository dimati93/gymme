﻿using System.Collections.ObjectModel;
using System.Linq;
using Gymme.Data.Interfaces;
using Gymme.Resources;

namespace Gymme.ViewModel
{
    public class ExerciseSelectorControlVM : Base.ViewModel
    {
        private readonly long _workoutId;

        public ExerciseSelectorControlVM(long workoutId)
        {
            _workoutId = workoutId;
            LoadExercises();
        }

        public ObservableCollection<IExercise> Items { get; private set; }

        public long WorkoutId
        {
            get { return _workoutId; }
        }

        private void LoadExercises()
        {
            if (!ExerciseData.Instance.IsDataLoaded)
            {
                ExerciseData.Instance.LoadData();
            }

            Items = new ObservableCollection<IExercise>(ExerciseData.Instance.PersetExercises.Select(x => (IExercise)new ExerciseSelectItemVM(x, WorkoutId)));
        }
    }
}