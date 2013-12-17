using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Gymme.Data.Models;
using Gymme.Data.Repository;
using Gymme.ViewModel.Base;

namespace Gymme.ViewModel.Page
{
    public class ExecutePageVM : Base.ViewModel, IDisposable
    {
        private DispatcherTimer _timer;
        private readonly Action _updateCurrentSet;
        private readonly TrainingExercise _trainingExercise;
        private readonly IEnumerable<TrainingExercise> _history;
        private Set _currentSet;
        private Set _lastSet;
        private TimeSpan _time;
        private ObservableCollection<ExecuteHistoryItemVM> _historyItems;

        public ExecutePageVM(long id, Action updateCurrentSet)
        {
            _updateCurrentSet = updateCurrentSet;
            _trainingExercise = RepoTrainingExercise.Instance.FindById(id);
            _history = GetHistory(_trainingExercise);

            if (TrainingExercise.Status == TrainingExerciseStatus.Created)
            {
                Start();
            }
            else
            {
                if (TrainingExercise.Sets.Count != 0)
                {
                    int lastOrdinal = TrainingExercise.Sets.Max(x => x.OrdinalNumber);
                    CurrentSet = LastSet = TrainingExercise.Sets.SingleOrDefault(x => x.OrdinalNumber == lastOrdinal);
                }

                if (TrainingExercise.Status == TrainingExerciseStatus.Started)
                {
                    CurrentSet = LastSet = CreateNewSet(CurrentSet == null ? 1 : CurrentSet.OrdinalNumber + 1);
                    StartTimer();
                }
            }
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.04) };
            _timer.Tick += (o, e) => TimeTick();
            _timer.Start();
        }

        private void TimeTick()
        {
            Time = LastSet != null ? TimeSpan.FromSeconds((DateTime.Now - LastSet.StartTime).Ticks / TimeSpan.TicksPerSecond) : TimeSpan.Zero;
        }

        private void ShowTime()
        {
            if (_trainingExercise.Status == TrainingExerciseStatus.Started)
            {
                TimeTick();
                return;
            }

            Time = CurrentSet != null && CurrentSet.EndTime != null ? CurrentSet.EndTime.Value - _currentSet.StartTime : TimeSpan.Zero;
        }

        private IEnumerable<TrainingExercise> GetHistory(TrainingExercise exercise)
        {
            return RepoTrainingExercise.Instance.GetHistoryForId(exercise, 5).ToArray();
        }

        public string Title
        {
            get { return RepoExercise.Instance.FindById(_trainingExercise.IdExecise).Name; }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        public ObservableCollection<ExecuteHistoryItemVM> HistoryItems
        {
            get
            {
                return _historyItems 
                    ?? (_historyItems = new ObservableCollection<ExecuteHistoryItemVM>(_history.Select(x => new ExecuteHistoryItemVM(x))));
            }
        }

        public bool IsHistoryEmpty { get { return HistoryItems.Count == 0; } }

        public bool IsSkiped
        {
            get { return TrainingExercise.Status == TrainingExerciseStatus.Skiped; }
        }

        public string NextButtonText
        {
            get
            {
                return TrainingExercise.Status != TrainingExerciseStatus.Finished && CurrentSet == LastSet
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
                ShowTime();
                PreviousCommand.RaiseCanExecuteChanged();
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public Command PreviousCommand
        {
            get
            {
                return GetOrCreateCommand("PreviousCommand", PreviousSet, o => CurrentSet != null && CurrentSet.OrdinalNumber > 1);
            }
        }

        public Command NextCommand
        {
            get
            {
                return GetOrCreateCommand("NextCommand", NextSet, o => TrainingExercise.Status != TrainingExerciseStatus.Finished || CurrentSet != LastSet);
            }
        }

        public TrainingExercise TrainingExercise
        {
            get { return _trainingExercise; }
        }

        public Action<bool> UpdadeFinishButtonState { get; set; }

        private Set LastSet
        {
            get { return _lastSet; }
            set
            {
                _lastSet = value;
            }
        }

        private void SaveCurrent()
        {
            _updateCurrentSet();
            RepoSet.Instance.Save(CurrentSet);

            if (!_trainingExercise.Sets.Contains(CurrentSet))
            {
                CurrentSet.IdTrainingExercise = _trainingExercise.Id;
                _trainingExercise.Sets.Add(CurrentSet);
                Data.Core.DatabaseContext.Instance.SubmitChanges();
            }
        }

        public void FinishExecute()
        {
            if (CurrentSet != _lastSet)
            {
                SaveCurrent();
            }

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
                CurrentSet = LastSet = null;
                TrainingExercise.Status = TrainingExerciseStatus.Skiped;
                RepoTrainingExercise.Instance.Save(TrainingExercise);
                _timer.Stop();
                _timer = null;
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
            StartTimer();
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

                if (nextNumber == LastSet.OrdinalNumber)
                {
                    CurrentSet = LastSet;
                    return;
                }

                LastSet = CreateNewSet(nextNumber);
                FinishCurrentSet();
                if (UpdadeFinishButtonState != null)
                {
                    UpdadeFinishButtonState(true);
                }

                CurrentSet = LastSet;
                return;
            }

            LastSet = CreateNewSet(1);
            CurrentSet = LastSet;
        }

        private void FinishCurrentSet()
        {
            CurrentSet.EndTime = DateTime.Now;
            RepoSet.Instance.Save(CurrentSet);
            TrainingExercise.Sets.Add(CurrentSet);
            Data.Core.DatabaseContext.Instance.SubmitChanges();
        }

        private Set CreateNewSet(int ordinal)
        {
            Set newSet = new Set { OrdinalNumber = ordinal, StartTime = DateTime.Now };
            Set[] sets = _history.Select(x => x.Sets.FirstOrDefault(set => set.OrdinalNumber == ordinal)).ToArray();
            Set prevSet = sets.FirstOrDefault(x => x != null);
            if (prevSet != null)
            {
                newSet.Lift = prevSet.Lift;
                newSet.Reps = prevSet.Reps;
            }

            return newSet;
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}