using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;

namespace Gymme.Data.Repository
{
    public class RepoTraining : RepositoryBase<Training>
    {
        private static RepoTraining _instance;

        public static RepoTraining Instance { get { return _instance ?? (_instance = new RepoTraining()); } }

        private RepoTraining()
        {
        }

        public override Training FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }

        public Training FindLastByWorkoutId(long id)
        {
            return Table.Where(x => x.IdWorkout == id).OrderByDescending(x => x.StartTime).FirstOrDefault();
        }

        public IEnumerable<Training> FindUnfinished()
        {
            return Table.Where(x => x.StatusId == (int)TrainingStatus.Started);
        }

        public IEnumerable<Training> FindByWorkoutId(long id)
        {
            return Table.Where(x => x.IdWorkout == id);
        }

        public IEnumerable<Training> GetHistory(Workout workout, int take)
        {
            return Table.Where(x => x.IdWorkout == workout.Id).OrderByDescending(x => x.StartTime).Take(take);
        }
    }
}