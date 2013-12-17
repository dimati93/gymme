using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoSet : RepositoryBase<Set>
    {
        private static RepoSet _instance;

        public static RepoSet Instance { get { return _instance ?? (_instance = new RepoSet()); } }

        private RepoSet()
        {
        }

        public override Set FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }

        public IEnumerable<Set> FindAllForExecuteId(long id)
        {
            return Table.Where(x => x.IdTrainingExercise == id);
        }
    }
}