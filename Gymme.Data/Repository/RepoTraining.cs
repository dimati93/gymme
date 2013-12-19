using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Models;

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
            return Table.Where(x => x.Status == TrainingStatus.Started);
        }
    }
}