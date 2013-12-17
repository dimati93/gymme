using System;
using System.Collections.ObjectModel;
using System.Linq;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class ExecuteHistoryItemVM : Base.ViewModel
    {
        private readonly TrainingExercise _historyItem;

        public ExecuteHistoryItemVM(TrainingExercise historyItem)
        {
            _historyItem = historyItem;
            Sets = new ObservableCollection<Set>(historyItem.Sets.OrderBy(x => x.OrdinalNumber));
        }

        public DateTime StartDate
        {
            get { return _historyItem.StartTime.GetValueOrDefault(new DateTime()); }
            set { _historyItem.StartTime = value; }
        }

        public ObservableCollection<Set> Sets { get; set; }

        public TrainingExerciseStatus Status
        {
            get
            {
                return _historyItem.Status;
            }
        }

        public TrainingExercise Item
        {
            get
            {
                return _historyItem;
            }
        }
    }
}