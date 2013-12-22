using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.Upcoming
{
    public class UnfinishedTrainingItem : UpcomingItem
    {
        private readonly Training _training;

        public UnfinishedTrainingItem(Training training)
        {
            _training = training;
            Accent = AccentColors.Blue;
            Title = RepoWorkout.Instance.FindById(_training.IdWorkout).Title;
            Description = string.Format("{0} {1}", AppResources.Training_Started, GetTimeDescription(_training.StartTime)).UppercaseFirst();
            Priority = 2;
        }

        protected override void GotoPageView()
        {
            NavigationManager.GotoTrainingPageByTrainingId(_training.Id);
        }
    }
}