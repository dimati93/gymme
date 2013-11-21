using System.Collections.ObjectModel;
using System.Linq;
using Gymme.Data.Interfaces;
using Gymme.Resources;

namespace Gymme.ViewModel
{
    public class ExercisesSelectVM : Base.ViewModel
    {
        public ExercisesSelectVM()
        {
            LoadExercises();
        }
        
        public ObservableCollection<ExerciseCategory> Items { get; private set; }

        private void LoadExercises()
        {
            if (!ExerciseData.Instance.IsDataLoaded)
            {
                ExerciseData.Instance.LoadData();
            }

            Items = new ObservableCollection<ExerciseCategory>(ExerciseData.Instance.PersetExercises.Cast<IExercise>().GroupBy(x => x.Category).Select(x => new ExerciseCategory(x)));
        }
    }
}