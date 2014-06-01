using System;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Statistics
{
    public class WorkoutStatistics : Statistics
    {
        private static List<TimeStatPoint> GenerateDebugData()
        {
            Random random = new Random();
            DateTime now = DateTime.Now;
            return Enumerable.Range(-9, 10).Select(x => new TimeStatPoint(now.AddDays(x * 7), TimeSpan.FromMinutes(random.Next(50, 80)))).ToList();
        }

        private readonly static DateTime _dateZero = new DateTime(2014, 01, 01);
        private readonly Workout _workout;
        private List<TimeStatPoint> _spentTime;

        public WorkoutStatistics(Workout workout)
        {
            _workout = workout;
        }

        public static DateTime DateZero
        {
            get
            {
                return _dateZero;
            }
        }

        public List<TimeStatPoint> SpentTime
        {
            get { return _spentTime; }
            set
            {
                _spentTime = value;
                NotifyPropertyChanged("SpentTime");
            }
        }

        protected override void ProcedeLoad()
        {
            var trainings = RepoTraining.Instance.GetHistory(_workout, 10).OrderBy(x => x.StartTime);
            SpentTime = GetSpentStat(trainings);
        }

        private List<TimeStatPoint> GetSpentStat(IEnumerable<Training> trainings)
        {
#if DEBUG
            var max = GenerateDebugData();
#else
            var max = trainings.Select(x => new TimeStatPoint(x.StartTime, RepoTrainingExercise.Instance.FindFinalTime(x)));
#endif
            return new List<TimeStatPoint>(max);
        }
    }
}
