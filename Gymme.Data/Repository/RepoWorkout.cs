using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoWorkout : RepositoryBase<Workout>
    {
        private static RepoWorkout _instance;

        public static RepoWorkout Instance { get { return _instance ?? (_instance = new RepoWorkout()); } }

        private RepoWorkout()
        {
        }
    }
}
