using System;
using System.Linq;
using System.Windows.Input;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Page
{
    public class ExecutePageVM : Base.ViewModel
    {
        private readonly Action _updateCurrentSet;
        private readonly TrainingExercise _trainingExercise;
        private Set _currentSet;

        public ExecutePageVM(long id, Action updateCurrentSet)
        {
            _updateCurrentSet = updateCurrentSet;
            _trainingExercise = RepoTrainingExercise.Instance.FindById(id);

            if (_trainingExercise.Status == TrainingExerciseStatus.Created)
            {
                _trainingExercise.Status = TrainingExerciseStatus.Started;
                RepoTrainingExercise.Instance.Save(_trainingExercise);
                NextSet();
            }
        }

        public Set CurrentSet
        {
            get
            {
                return _currentSet;
            }
            set
            {
                _currentSet = value;
                NotifyPropertyChanged("CurrentSet");
            }
        }

        public ICommand FinishCommand
        {
            get
            {
                return GetOrCreateCommand("FinishCommand", FinishExecute);
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return GetOrCreateCommand("PreviousCommand", PreviousSet, o => CurrentSet != null && CurrentSet.OrdinalNumber > 1);
            }
        }

        public ICommand NextCommand
        {
            get
            {
                return GetOrCreateCommand("FinishCommand", FinishExecute);
            }
        }

        private void SaveCurrent()
        {
            _updateCurrentSet();
            RepoSet.Instance.Save(CurrentSet);
        }

        private void FinishExecute()
        {
            SaveCurrent();
            _trainingExercise.Status = TrainingExerciseStatus.Finished;
            RepoTrainingExercise.Instance.Save(_trainingExercise);
        }


        private void PreviousSet(object o)
        {
            if (CurrentSet != null)
            {
                SaveCurrent();
                int previousNumber = CurrentSet.OrdinalNumber - 1;
                Set previousSet = _trainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == previousNumber);
                if (previousSet != null)
                {
                    CurrentSet = previousSet;
                    return;
                }
            }
        }

        private void NextSet()
        {
            if (CurrentSet != null)
            {
                SaveCurrent();
                int nextNumber = CurrentSet.OrdinalNumber + 1;
                Set nextSet = _trainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == nextNumber);
                if (nextSet != null)
                {
                    CurrentSet = nextSet;
                    return;
                }

                nextSet = NewSet(nextNumber);
                // Initialize reps and sets
                CurrentSet = nextSet;
                return;
            }

            var firstSet = NewSet(1);
            // Initialize reps and sets
            CurrentSet = firstSet;
        }

        private Set NewSet(int ordinal)
        {
            Set set = new Set { OrdinalNumber = ordinal, StartTime = DateTime.Now };
            RepoSet.Instance.Save(set);
            _trainingExercise.Sets.Add(set);
            Data.Core.DatabaseContext.Instance.SubmitChanges();
            return set;
        }
    }
}