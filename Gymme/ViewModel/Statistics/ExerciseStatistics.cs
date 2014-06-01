using System;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Statistics
{
    public class ExerciseStatistics : Statistics
    {
        private static List<WeightStatPoint> GenerateDebugMaxData()
        {
            Random random = new Random();
            DateTime now = DateTime.Now;
            return Enumerable.Range(-9, 10).Select(x => new WeightStatPoint(now.AddDays(x * 7), (float)random.Next(100, 160)/2)).ToList();
        }

        private static List<WeightStatPoint> GenerateDebugAvgData(IEnumerable<WeightStatPoint> maxList)
        {
            Random random = new Random();
            return maxList.Select(x => new WeightStatPoint(x.Date, x.Weight * 0.8f + (float)random.NextDouble() * 20 - 10)).ToList();
        }

        private readonly Exercise _exercise;
        private List<WeightStatPoint> _averageWeight;
        private List<WeightStatPoint> _maxWeight;
        
        public ExerciseStatistics(Exercise exercise)
        {
            _exercise = exercise;
        }

        public List<WeightStatPoint> AverageWeight
        {
            get { return _averageWeight; }
            set
            {
                _averageWeight = value;
                NotifyPropertyChanged("AverageWeight");
            }
        }

        public List<WeightStatPoint> MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                _maxWeight = value;
                NotifyPropertyChanged("MaxWeight");
            }
        }

        protected override void ProcedeLoad()
        {
            var trainings = RepoTrainingExercise.Instance.GetHistory(_exercise, 10).ToArray();
            MaxWeight = GetMaxStat(trainings);
            AverageWeight = GetAvarageStat(trainings);
        }

        private List<WeightStatPoint> GetMaxStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var max = GenerateDebugMaxData();
#else
            var max = trainings.Select(x => new WeightStatPoint(x.StartTime, RepoSet.Instance.FindMax(x.TrainingExercise)));
#endif
            return new List<WeightStatPoint>(max);
        }

        private List<WeightStatPoint> GetAvarageStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var avg = GenerateDebugAvgData(MaxWeight);
#else
            var avg = trainings.Select(x => new WeightStatPoint(x.StartTime, RepoSet.Instance.FindAvg(x.TrainingExercise)));
#endif
            return new List<WeightStatPoint>(avg);
        }
    }
}