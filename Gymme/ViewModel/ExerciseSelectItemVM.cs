using System.Windows.Input;

using Gymme.Data.Interfaces;
using Gymme.Resources;

namespace Gymme.ViewModel
{
    public class ExerciseSelectItemVM : Base.ViewModel
    {
        private readonly IExercise _exercise;
        private readonly long _workoutId;

        public ExerciseSelectItemVM(IExercise exercise, long workoutId)
        {
            _exercise = exercise;
            _workoutId = workoutId;
        }

        public string Name { get { return _exercise.Name; } }

        public string Category { get { return _exercise.Category; } }

        public ICommand GotoPageViewCommand
        {
            get
            {
                return GetOrCreateCommand("GotoPageViewCommand", Choose);
            }
        }

        public void Choose()
        {
            int index = ExerciseData.Instance.PersetExercises.IndexOf(_exercise);
            if (index < 0) return;

            NavigationManager.GotoAddExercisePage(_workoutId, index);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
