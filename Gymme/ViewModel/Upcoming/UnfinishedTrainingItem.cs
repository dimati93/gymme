using Gymme.Data.Models;
using Gymme.Resources;

namespace Gymme.ViewModel.Upcoming
{
    public class UnfinishedTrainingItem : UpcomingItem
    {
        private readonly Training _training;

        public UnfinishedTrainingItem(Training training, Workout workout)
        {
            _training = training;
            Accent = AccentColors.Blue;
            Title = workout.Title;
            Description = string.Format("{0} {1}", AppResources.Training_Started, GetTimeDescription(_training.StartTime)).UppercaseFirst();
            Priority = 2;
        }

        protected override void GotoPageView()
        {
            NavigationManager.GotoTrainingPageByTrainingId(_training.Id);
        }
    }
}