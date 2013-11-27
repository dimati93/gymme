using System;
using System.Linq;
using System.Windows;
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
        private Set _lastSet;

        public ExecutePageVM(long id, Action updateCurrentSet)
        {
            _updateCurrentSet = updateCurrentSet;
            _trainingExercise = RepoTrainingExercise.Instance.FindById(id);

            if (TrainingExercise.Status == TrainingExerciseStatus.Created)
            {
                Start();
            }
            else
            {
                if (TrainingExercise.Sets.Count != 0)
                {
                    int lastOrdinal = TrainingExercise.Sets.Max(x => x.OrdinalNumber);
                    CurrentSet = _lastSet = TrainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == lastOrdinal);
                }
                
                if (TrainingExercise.Status == TrainingExerciseStatus.Started)
                {
                    NextSet(null);
                }
            }
        }

        public bool IsSkiped
        {
            get { return TrainingExercise.Status == TrainingExerciseStatus.Skiped; }
        }

        public string NextButtonText
        {
            get
            {
                return TrainingExercise.Status != TrainingExerciseStatus.Finished && CurrentSet == _lastSet
                    ? Resources.AppResources.ExecutePage_Done
                    : Resources.AppResources.ExecutePage_Next;
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
                NotifyPropertyChanged("NextButtonText");
                NotifyPropertyChanged("PreviousCommand");
                NotifyPropertyChanged("NextCommand");
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return GetOrCreateCommand("PreviousCommand", PreviousSet, o => !IsSkiped && CurrentSet != null && CurrentSet.OrdinalNumber > 1);
            }
        }

        public ICommand NextCommand
        {
            get
            {
                return GetOrCreateCommand("NextCommand", NextSet, o => !IsSkiped && (TrainingExercise.Status != TrainingExerciseStatus.Finished || CurrentSet != _lastSet));
            }
        }

        public TrainingExercise TrainingExercise
        {
            get { return _trainingExercise; }
        }

        public Action<bool> UpdadeFinishButtonState { get; set; }

        private void SaveCurrent()
        {
            _updateCurrentSet();
            RepoSet.Instance.Save(CurrentSet);
        }

        public void FinishExecute()
        {
            SaveCurrent();
            TrainingExercise.Status = TrainingExerciseStatus.Finished;
            RepoTrainingExercise.Instance.Save(TrainingExercise);
        }

        public bool SkipExecute()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Execute_SkipWarning,
                                                      Resources.AppResources.Execute_SkipWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                foreach (var set in TrainingExercise.Sets)
                {
                    RepoSet.Instance.Delete(set);
                }

                TrainingExercise.Sets.Clear();
                CurrentSet = _lastSet = null;
                TrainingExercise.Status = TrainingExerciseStatus.Skiped;
                RepoTrainingExercise.Instance.Save(TrainingExercise);
                NotifyPropertyChanged("IsSkiped");
                return true;
            }

            return false;
        }

        public void Start()
        {
            TrainingExercise.Status = TrainingExerciseStatus.Started;
            RepoTrainingExercise.Instance.Save(TrainingExercise);
            NextSet(null);
            NotifyPropertyChanged("IsSkiped");
        }

        private void PreviousSet(object o)
        {
            if (CurrentSet != null)
            {
                SaveCurrent();
                int previousNumber = CurrentSet.OrdinalNumber - 1;
                Set previousSet = TrainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == previousNumber);
                if (previousSet != null)
                {
                    CurrentSet = previousSet;
                }
            }
        }

        private void NextSet(object o)
        {
            if (CurrentSet != null)
            {
                SaveCurrent();
                int nextNumber = CurrentSet.OrdinalNumber + 1;
                Set nextSet = TrainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == nextNumber);
                if (nextSet != null)
                {
                    CurrentSet = nextSet;
                    return;
                }

                if (nextNumber == _lastSet.OrdinalNumber)
                {
                    CurrentSet = _lastSet;
                    return;
                }
                
                _lastSet = NewSet(nextNumber);
                FinishCurrentSet();
                if (UpdadeFinishButtonState != null)
                {
                    UpdadeFinishButtonState(true);
                }

                CurrentSet = _lastSet;
                return;
            }

            _lastSet = NewSet(1);
            CurrentSet = _lastSet;
        }

        private void FinishCurrentSet()
        {
            CurrentSet.EndTime = DateTime.Now;
            RepoSet.Instance.Save(CurrentSet);
            TrainingExercise.Sets.Add(CurrentSet);
            Data.Core.DatabaseContext.Instance.SubmitChanges();
        }

        private Set NewSet(int ordinal)
        {
            return new Set { OrdinalNumber = ordinal, StartTime = DateTime.Now };
        }
    }
}