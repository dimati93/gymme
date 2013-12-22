using System.Collections.Generic;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.Upcoming
{
    public class UnfinishedTraining : UpcomingAlgorithm
    {
        public override IEnumerable<UpcomingItem> GetUpcoming()
        {
            var unfinished = RepoTraining.Instance.FindUnfinished();
            foreach (var training in unfinished)
            {
                if (Intelligent.IsTrainingExperate(training))
                {
                    training.Status = TrainingStatus.Finished;
                    RepoTraining.Instance.Save(training);
                    continue;
                }

                yield return new UnfinishedTrainingItem(training);
            }
        }
    }
}