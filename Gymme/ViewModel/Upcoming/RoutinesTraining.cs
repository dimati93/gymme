using System;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Upcoming
{
    public class RoutinesTraining : UpcomingAlgorithm
    {
        public const int DayCycleLimit = 15;

        public const int DayAvarageActual = 5;
        public const int TimeAvarageActual = 3;

        public override IEnumerable<UpcomingItem> GetUpcoming()
        {
            DateTime today = DateTime.Now.Date;
            List<UpcomingItem> routines = new List<UpcomingItem>();
            foreach (var workout in RepoWorkout.Instance.FindAll())
            {
                var trainingsByTime = RepoTraining.Instance.FindByWorkoutId(workout.Id).OrderByDescending(x => x.StartTime);
                var nextDay = GetNextDay(trainingsByTime);
                if (nextDay == null)
                {
                    continue;
                }

                int dayDiff = (today - nextDay.Value).Days;

                if (dayDiff > 1 || dayDiff < -1)
                {
                    continue;
                }

                var suggestedTime = GetTrainingTime(trainingsByTime);
                nextDay += suggestedTime;
                switch (dayDiff)
                {
                    case -1 : 
                        routines.Add(new RoutineTrainingItem(workout, RoutineTimeType.PreRemind, nextDay.Value));
                        break;
                    case 0 : 
                        routines.Add(new RoutineTrainingItem(workout, RoutineTimeType.Actual, nextDay.Value));
                        break;
                    case 1 : 
                        routines.Add(new RoutineTrainingItem(workout, RoutineTimeType.Expired, nextDay.Value));
                        break;
                }
            }

            return routines;
        }

        private TimeSpan GetTrainingTime(IEnumerable<Training> trainingsByTime)
        {
            var timeSelection =
                trainingsByTime.Take(TimeAvarageActual).Select(x => x.StartTime - x.StartTime.Date).OrderBy(x => x).ToArray();
            if (timeSelection.Length < 0)
            {
                return TimeSpan.FromHours(12);
            }

            int center = (timeSelection.Length - 1) / 2;
            return Trim10Minutes(timeSelection[center]);
        }

        private TimeSpan Trim10Minutes(TimeSpan span)
        {
            long minutes = (span.Ticks / (10 * TimeSpan.TicksPerMinute)) * 10;
            return TimeSpan.FromMinutes(minutes);
        }

        private DateTime? GetNextDay(IEnumerable<Training> trainingsByTime)
        {
            var days = trainingsByTime.Take(DayAvarageActual + 1).Select(x => x.StartTime.Date).ToArray();
            if (days.Length < 2)
            {
                return null;
            }

            TimeSpan dayAccum = TimeSpan.Zero;
            int daysCounted = 0;
            for (int i = 0; i < days.Length - 1; i++)
            {
                var dayDiff = days[i] - days[i + 1];
                if (dayDiff.Days < 1 || dayDiff.Days > DayCycleLimit)
                {
                    continue;
                }
                dayAccum += dayDiff;
                daysCounted++;
            }

            if (daysCounted == 0)
            {
                return null;
            }

            int dayAvarage = (int)Math.Round((double)dayAccum.Days / daysCounted);
            
            DateTime today = DateTime.Now.Date;

            
            int lastDayDiff = (today - days[0]).Days;
            int cycledays = lastDayDiff / dayAvarage;
            if (cycledays == 0)
            {
                cycledays = 1;
            }

            return days[0] + TimeSpan.FromDays(dayAvarage * cycledays);
        }
    }
}