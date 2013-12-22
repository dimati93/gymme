using System;
using System.Collections.ObjectModel;
using System.Linq;

using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class ExecuteHistoryItemVM : Base.ViewModel
    {
        private TrainingExercise _historyItem;

        public ExecuteHistoryItemVM(TrainingExercise historyItem, Training training)
        {
            _historyItem = historyItem;
            StartDate = training.StartTime.Date;
            Sets = new ObservableCollection<Set>(historyItem.Sets.OrderBy(x => x.OrdinalNumber));
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
            foreach (var set in RepoSet.Instance.FindAllForExecuteId(_historyItem.Id))
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