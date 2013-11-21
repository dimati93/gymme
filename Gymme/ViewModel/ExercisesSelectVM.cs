using System.Collections.ObjectModel;
using System.Linq;
using Gymme.Data.Interfaces;
using Gymme.Resources;

namespace Gymme.ViewModel
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

        private void LoadExercises()
        {
            if (!ExerciseData.Instance.IsDataLoaded)
            {
                ExerciseData.Instance.LoadData();
            }

            Items = new ObservableCollection<ExerciseCategory>(ExerciseData.Instance.PersetExercises.Select(x => (IExercise)new ExerciseSelectItemVM(x, _workoutId)).GroupBy(x => x.Category).Select(x => new ExerciseCategory(x)));
        }
    }
}