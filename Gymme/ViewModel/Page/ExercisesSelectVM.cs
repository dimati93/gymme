using System.Collections.ObjectModel;
using System.Linq;
using Gymme.Data.Interfaces;
using Gymme.Resources;

namespace Gymme.ViewModel.Page
{
    public class ExercisesSelectVM : Base.ViewModel
    {
        private readonly long _workoutId;

        public ExercisesSelectVM(long workoutId)
        {
            _workoutId = workoutId;
            LoadExercises();
        }

        public ObservableCollection<ExerciseCategory> Items { get; private set; }

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

            Items = new ObservableCollection<ExerciseCategory>(ExerciseData.Instance.PersetExercises.Select(x => (IExercise)new ExerciseSelectItemVM(x, WorkoutId)).GroupBy(x => x.Category).Select(x => new ExerciseCategory(x)));
        }
    }
}