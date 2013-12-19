using System;
using System.Globalization;
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
            Accent = AccentColors.Started;
            Title = RepoWorkout.Instance.FindById(_training.IdWorkout).Title;
            Description = string.Format("{0} {1}", AppResources.Training_Started, GetTimeDesctiption());
        }

        private string GetTimeDesctiption()
        {
            DateTime todayDate = DateTime.Now.Date;
            if (_training.StartTime.Date == todayDate)
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0} {1} {2:t}", AppResources.Up_Today, AppResources.Up_At, _training.StartTime);
            }

            if (_training.StartTime.Date == todayDate.AddDays(-1))
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0} {1} {2:t}", AppResources.Up_Yday, AppResources.Up_At, _training.StartTime);
            }

            return string.Format(CultureInfo.CurrentUICulture, "{0:d} {1} {0:t}", _training.StartTime, AppResources.Up_At);
        }

        protected override void GotoPageView()
        {
            NavigationManager.GotoTrainingPage(_training.Id);
        }
    }
}