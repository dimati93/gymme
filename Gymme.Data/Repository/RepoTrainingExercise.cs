using System;
using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Core;
using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;

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

        public IEnumerable<TrainingExerciseHistory> GetHistoryForId(TrainingExercise exercise, int takeCount)
        {
            return (from te in Instance.Table 
                   join t in RepoTraining.Instance.Table on te.IdTraining equals t.Id
                   where te.IdExecise == exercise.IdExecise
                   orderby t.StartTime descending
                   select new TrainingExerciseHistory { TrainingExercise = te, StartTime = t.StartTime }).Take(takeCount);
        }
    }
}