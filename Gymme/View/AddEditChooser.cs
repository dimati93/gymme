using System.Windows.Navigation;

namespace Gymme.View
{
    public static class AddEditChooser
    {
        public static class Variant 
        {
            public const string AddWorkout = "addWorkout";
            public const string EditWorkout = "editWorkout";
            public const string AddExercise = "addExercise";
            public const string EditExercise = "editExercise";
        }

        public static class Param
        {
            public const string WorkoutId = "workoutId";
        }
    }
}