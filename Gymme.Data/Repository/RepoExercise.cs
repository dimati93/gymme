using System.Linq;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoExercise : RepositoryBase<Exercise>
    {
        private static RepoExercise _instance;

        public static RepoExercise Instance { get { return _instance ?? (_instance = new RepoExercise()); } }

        private RepoExercise()
        {
        }

        public override Exercise FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }
    }
}
