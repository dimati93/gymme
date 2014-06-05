using System;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.Statistics
{
    public class ExerciseWeightStatistics : ExerciseStatistics
    {
        private static IEnumerable<FloatStatPoint> GenerateDebugMaxData()
        {
            Random random = new Random();
            DateTime now = DateTime.Now;
            return Enumerable.Range(-9, 10).Select(x => new FloatStatPoint(now.AddDays(x * 7), (float)random.Next(100, 160)/2));
        }

        private static IEnumerable<FloatStatPoint> GenerateDebugAvgData(IEnumerable<FloatStatPoint> maxList)
        {
            Random random = new Random();
            return maxList.Select(x => new FloatStatPoint(x.Date, x.Value * 0.8f + (float)random.NextDouble() * 20 - 10));
        }

        private List<FloatStatPoint> _averageWeight;
        private List<FloatStatPoint> _maxWeight;
        
        public ExerciseWeightStatistics(Exercise exercise)
            : base(exercise)
        {
        }

        public List<FloatStatPoint> AverageWeight
        {
            get { return _averageWeight; }
            set
            {
                _averageWeight = value;
                NotifyPropertyChanged("AverageWeight");
            }
        }

        public List<FloatStatPoint> MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                _maxWeight = value;
                NotifyPropertyChanged("MaxWeight");
            }
        }

        public override string TotalResultText
        {
            get
            {
                return AppResources.ExercisePage_Chart_TotalReps;
            }
        }

        public override string PerSetResultText
        {
            get
            {
                return AppResources.ExercisePage_Chart_RepsPerSet;
            }
        }

        protected override void ProcedeLoadStats(TrainingExerciseHistory[] trainings)
        {
            base.ProcedeLoadStats(trainings);
            MaxWeight = GetMaxStat(trainings);
            AverageWeight = GetAvarageStat(trainings);
        }

        private List<FloatStatPoint> GetMaxStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var max = GenerateDebugMaxData();
#else
            var max = trainings.Select(x => new FloatStatPoint(x.StartTime, RepoSet.Instance.FindMaxWeight(x.TrainingExercise)));
#endif
            return max.ToList();
        }

        private List<FloatStatPoint> GetAvarageStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var avg = GenerateDebugAvgData(MaxWeight);
#else
            var avg = trainings.Select(x => new FloatStatPoint(x.StartTime, RepoSet.Instance.FindAvgWeight(x.TrainingExercise)));
#endif
            return avg.ToList();
        }
    }
}