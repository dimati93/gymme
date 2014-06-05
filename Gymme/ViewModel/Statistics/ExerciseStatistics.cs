// Type: Gymme.ViewModel.Statistics.ExerciseStatistics
// Assembly: Gymme, Version=1.1.48.0, Culture=neutral, PublicKeyToken=null
// MVID: 75B8BCB2-4771-4894-99E2-0BE3700EFFF3
// Assembly location: W:\Gymme\Gymme\Bin\Debug\Gymme.dll

using System;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;
using Gymme.Data.Repository;
using Gymme.Resources;

namespace Gymme.ViewModel.Statistics
{
    public class ExerciseStatistics : Statistics
    {
        private readonly Exercise _exercise;
        private List<FloatStatPoint> _repsPerSet;
        private List<FloatStatPoint> _totalReps;

        public List<FloatStatPoint> RepsPerSet
        {
            get
            {
                return _repsPerSet;
            }
            set
            {
                _repsPerSet = value;
                NotifyPropertyChanged("RepsPerSet");
            }
        }

        public List<FloatStatPoint> TotalReps
        {
            get
            {
                return _totalReps;
            }
            set
            {
                _totalReps = value;
                NotifyPropertyChanged("TotalReps");
            }
        }

        public virtual string TotalResultText
        {
            get
            {
                return AppResources.ExercisePage_Chart_TotalResult;
            }
        }

        public virtual string PerSetResultText
        {
            get
            {
                return AppResources.ExercisePage_Chart_ResultPerSet;
            }
        }

        public ExerciseStatistics(Exercise exercise)
        {
            _exercise = exercise;
        }

        private static IEnumerable<FloatStatPoint> GenerateDebugTotalData()
        {
            Random random = new Random();
            DateTime now = DateTime.Now;
            return Enumerable.Range(-9, 10).Select(x => new FloatStatPoint(now.AddDays(x * 7), random.Next(100, 160) / 2f));
        }

        private static IEnumerable<FloatStatPoint> GenerateDebugPerSetData(IEnumerable<FloatStatPoint> totalList)
        {
            Random random = new Random();
            return totalList.Select(x => new FloatStatPoint(x.Date, (float)(x.Value * 0.300000011920929 + random.NextDouble() * 10.0 - 5.0)));
        }

        protected override void ProcedeLoad()
        {
            ProcedeLoadStats(RepoTrainingExercise.Instance.GetHistory(_exercise, 10).ToArray());
        }

        protected virtual void ProcedeLoadStats(TrainingExerciseHistory[] trainings)
        {
            TotalReps = GetTotalStat(trainings);
            RepsPerSet = GetPerSetStat(trainings);
        }

        private List<FloatStatPoint> GetTotalStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var total = GenerateDebugTotalData();
#else
            var total = trainings.Select(
                    x => new FloatStatPoint(x.StartTime, RepoSet.Instance.FindTotalReps(x.TrainingExercise)));
#endif
            return total.ToList();
        }

        private List<FloatStatPoint> GetPerSetStat(IEnumerable<TrainingExerciseHistory> trainings)
        {
#if DEBUG
            var perSet = GenerateDebugPerSetData(TotalReps);
#else
            var perSet = trainings.Select(
                                x => new FloatStatPoint(x.StartTime, RepoSet.Instance.FindRepsPerSet(x.TrainingExercise)));
#endif
            return perSet.ToList();
        }
    }
}
