using System.Linq;
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

        public override Workout FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }
    }
}
