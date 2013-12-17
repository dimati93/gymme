using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoTrainingExercise : RepositoryBase<TrainingExercise>
    {
        private static RepoTrainingExercise _instance;

        public static RepoTrainingExercise Instance { get { return _instance ?? (_instance = new RepoTrainingExercise()); } }

        private RepoTrainingExercise()
        {
        }

        public override TrainingExercise FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }

        public IEnumerable<TrainingExercise> GetHistoryForId(TrainingExercise exercise, int take)
        {
            return Table.Where(x => x.IdExecise == exercise.IdExecise).OrderByDescending(x => x.StartTime).Take(take);
        }
    }
}