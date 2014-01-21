using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Page
{
    public class ExercisesSelectVM : Base.ViewModel
    {
        private readonly Workout _workout;

        public ExercisesSelectVM(long workoutId)
        {
            _workout = RepoWorkout.Instance.FindById(workoutId);
        }

        public string Title
        {
            get
            {
                return _workout.Title;
            }
        }

        public long WorkoutId
        {
            get
            {
                return _workout.Id;
            }
        }
    }
}