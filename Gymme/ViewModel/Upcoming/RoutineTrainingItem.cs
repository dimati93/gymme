using System;
using System.Windows.Media;
using Gymme.Data.Models;
using Gymme.Resources;

namespace Gymme.ViewModel.Upcoming
{
    public enum RoutineTimeType
    {
        PreRemind = -1,
        Actual = 0,
        Expired = 1
    }

    public class RoutineTrainingItem : UpcomingItem
    {
        private readonly Workout _workout;
        private readonly RoutineTimeType _timeType;

        public RoutineTrainingItem(Workout workout, RoutineTimeType timeType, DateTime time)
        {
            _workout = workout;
            _timeType = timeType;
            
            Title = _workout.Title;
            Accent = SwitchAccent();
            Description = GetTimeDescription(time).UppercaseFirst();
            Priority = (int) timeType;
        }

        private Color SwitchAccent()
        {
            switch (_timeType)
            {
                case RoutineTimeType.PreRemind:
                    return AccentColors.Grey;
                case RoutineTimeType.Actual:
                    return AccentColors.Green;
                case RoutineTimeType.Expired:
                    return AccentColors.Red;
            }

            return AccentColors.Default;
        }

        protected override void GotoPageView()
        {
            NavigationManager.GotoTrainingPageByWorkoutId(_workout.Id);
        }
    }
}