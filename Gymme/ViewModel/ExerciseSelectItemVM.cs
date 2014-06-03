using Gymme.Data.AuxModels;

namespace Gymme.ViewModel
{
    public class ExerciseSelectItemVM : Base.ViewModel
    {
        private readonly PersetExercise _exercise;
        private readonly long _workoutId;

        public ExerciseSelectItemVM(PersetExercise exercise, long workoutId)
        {
            _exercise = exercise;
            _workoutId = workoutId;
        }

        public string Name { get { return _exercise.Name; } }

        public string Category { get { return _exercise.Category; } }

        public void Choose()
        {
            NavigationManager.GotoAddExercisePage(_workoutId, _exercise.Index);
        }
    }
}
