using System;
using System.Collections.ObjectModel;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class ExecuteHistoryItemVM : Base.ViewModel
    {
        private TrainingExercise _historyItem;

        public ExecuteHistoryItemVM(TrainingExerciseHistory history)
        {
            _historyItem = history.TrainingExercise;
            StartDate = history.StartTime.Date;
            Sets = new ObservableCollection<Set>(_historyItem.Sets.OrderBy(x => x.OrdinalNumber));
        }

        public DateTime StartDate { get; set; }

        public ObservableCollection<Set> Sets { get; set; }

        public TrainingExerciseStatus Status
        {
            get
            {
                return _historyItem.Status;
            }
        }

        public bool IsSkiped
        {
            get
            {
                return _historyItem.Status == TrainingExerciseStatus.Skiped;
            }
        }

        public TrainingExercise Item
        {
            get
            {
                return _historyItem;
            }
        }

        public void UpdateSets()
        {
            Sets.Clear();
            foreach (var set in RepoSet.Instance.FindAllForTraining(_historyItem))
            {
                Sets.Add(set);
            }
        }

        public void UpdateState()
        {
            _historyItem = RepoTrainingExercise.Instance.FindById(_historyItem.Id);
            NotifyPropertyChanged("IsSkiped");
            NotifyPropertyChanged("Status");
        }
    }
}