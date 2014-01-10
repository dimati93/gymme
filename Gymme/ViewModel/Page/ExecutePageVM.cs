using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using Gymme.Data.Models;
using Gymme.Data.Models.QueryResult;
using Gymme.Data.Repository;
using Gymme.ViewModel.Base;

namespace Gymme.ViewModel.Page
{
    public class ExecutePageVM : Base.ViewModel, IDisposable
    {
        #region State selestion

        private class PageState
        {
            private readonly ExecutePageVM _page;

            public PageState(ExecutePageVM page)
            {
                _page = page;
            }

            protected ExecutePageVM Page
            {
                get { return _page; }
            }

            public virtual bool IsSkiped { get { return false; } }
            public virtual string NextButtonText { get { return Resources.AppResources.ExecutePage_Next; } }

            public virtual void Start()
            {
            }

            public virtual void Initialize()
            {
            }

            public virtual void Skip()
            {
            }

            public virtual bool CanDoNext(object obj)
            {
                return true;
            }
        }

        private class PageStateFactory
        {
            private readonly ExecutePageVM _page;

            public PageStateFactory(ExecutePageVM page)
            {
                _page = page;
            }

            public PageState GetState()
            {
                switch (_page.Status)
                {
                    case TrainingExerciseStatus.Started:
                        return new StartedState(_page);
                    case TrainingExerciseStatus.Finished:
                        return new FinishedState(_page);
                    case TrainingExerciseStatus.Skiped:
                        return new SkipedState(_page);
                }

                return new PageState(_page);
            }
        }

        private class StartedState : PageState
        {
            public StartedState(ExecutePageVM page) 
                : base(page)
            {
            }

            public override string NextButtonText
            {
                get
                {
                    return Page.CurrentSet == Page.LastSet
                        ? Resources.AppResources.ExecutePage_Done
                        : Resources.AppResources.ExecutePage_Next;
                }
            }

            public override void Start()
            {
                base.Start();
                Initialize();
            }

            public override void Initialize()
            {
                base.Initialize();
                Page.CurrentSet = Page.LastSet = Page.CreateNewSet(Page.CurrentSet == null ? 1 : Page.CurrentSet.OrdinalNumber + 1);
                Page.StartTimer();
                Page._currentHistoryItem.UpdateState();
                Page.NotifyPropertyChanged("IsSkiped");
            }

            public override void Skip()
            {
                base.Skip();
                foreach (var set in Page._trainingExercise.Sets)
                {
                    RepoSet.Instance.Delete(set);
                }

                Page._trainingExercise.Sets.Clear();
                Page.CurrentSet = Page.LastSet = null;
                Page._timer.Stop();
                Page._timer = null;
                Page._currentHistoryItem.UpdateState();
                Page.NotifyPropertyChanged("IsSkiped");
            }
        }

        private class FinishedState : PageState
        {
            public FinishedState(ExecutePageVM page) 
                : base(page)
            {
            }

            public override bool CanDoNext(object obj)
            {
                return Page.CurrentSet != Page.LastSet;
            }
        }

        private class SkipedState : PageState
        {
            public SkipedState(ExecutePageVM page)
                : base(page)
            {
            }

            public override bool IsSkiped { get { return true; } }
        }

        #endregion

        private readonly PageStateFactory _stateFactory;
        private readonly Action _updateCurrentSet;
        private readonly TrainingExercise _trainingExercise;
        private TrainingExerciseHistory[] _history;

        private SetVM _currentSet;

        private DispatcherTimer _timer;
        private TimeSpan _time;

        private ExecuteHistoryItemVM _currentHistoryItem;
        private bool _finishButtonState;
        private Action<bool> _updadeFinishButtonState;
        private PageState _state;

        public ExecutePageVM(long id, Action updateCurrentSet)
        {
            _updateCurrentSet = updateCurrentSet;
            _trainingExercise = RepoTrainingExercise.Instance.FindById(id);
            
            _stateFactory = new PageStateFactory(this);
            _state = _stateFactory.GetState();

            if (Status == TrainingExerciseStatus.Created)
            {
                Start();
                return;
            }
            
            InitializeHistoryOnDemand();

            if (Sets.Count != 0)
            {
                int lastOrdinal = Sets.Max(x => x.OrdinalNumber);
                CurrentSet = LastSet = new SetVM(Sets.SingleOrDefault(x => x.OrdinalNumber == lastOrdinal));
                FinishButtonState = true;
            }

            _state.Initialize();
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

        public string Title
        {
            get { return RepoExercise.Instance.FindById(_trainingExercise.IdExecise).Name; }
        }

        private IEnumerable<TrainingExerciseHistory> History
        {
            get
            {
                InitializeHistoryOnDemand();
                return _history;
            }
        }

        public ObservableCollection<ExecuteHistoryItemVM> HistoryItems { get; private set; }

        public bool IsHistoryEmpty { get { return HistoryItems.Count == 0; } }

        private IList<Set> Sets
        {
            get { return _trainingExercise.Sets; }
        }

        private SetVM LastSet { get; set; }

        public SetVM CurrentSet
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
                if (_currentHistoryItem != null)
                {
                    _currentHistoryItem.UpdateSets();
                }

                PreviousCommand.RaiseCanExecuteChanged();
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsSkiped
        {
            get { return _state.IsSkiped; }
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
                return GetOrCreateCommand("NextCommand", NextSet, _state.CanDoNext);
            }
        }

        public string NextButtonText
        {
            get
            {
                return _state.NextButtonText;
            }
        }

        public bool FinishButtonState
        {
            get { return _finishButtonState; }
            set
            {
                _finishButtonState = value;
                if (UpdadeFinishButtonState != null)
                {
                    UpdadeFinishButtonState(_finishButtonState);
                }
                NotifyPropertyChanged("FinishButtonState");
            }
        }

        public Action<bool> UpdadeFinishButtonState
        {
            get { return _updadeFinishButtonState; }
            set
            {
                _updadeFinishButtonState = value;
                if (_updadeFinishButtonState != null)
                {
                    _updadeFinishButtonState(_finishButtonState);
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
            Time = LastSet != null ? (DateTime.Now - LastSet.Model.StartTime).TrimToSeconds() : TimeSpan.Zero;
        }

        private void ShowTime()
        {
            if (_trainingExercise.Status == TrainingExerciseStatus.Started)
            {
                TimeTick();
                return;
            }

            Time = CurrentSet != null && CurrentSet.Model.EndTime != null ? (CurrentSet.Model.EndTime.Value - CurrentSet.Model.StartTime).TrimToSeconds() : TimeSpan.Zero;
        }

        private void InitializeHistoryOnDemand()
        {
            if (_history == null)
            {
                _history = RepoTrainingExercise.Instance.GetHistoryForId(_trainingExercise, 5).ToArray();
                HistoryItems = new ObservableCollection<ExecuteHistoryItemVM>(_history.Select(x => new ExecuteHistoryItemVM(x)));
                _currentHistoryItem = HistoryItems.SingleOrDefault(x => x.Item.Id == _trainingExercise.Id);
            }
        }

        private void SaveCurrent(bool editedOnly)
        {
            if (editedOnly && !CurrentSet.IsEdited)
            {
                return;
            }

            SaveCurrent();
        }

        private void SaveCurrent()
        {
            _updateCurrentSet();
            SaveModel(CurrentSet);

            var set = CurrentSet.Model;
            if (!_trainingExercise.Sets.Contains(set))
            {
                set.IdTrainingExercise = _trainingExercise.Id;
                _trainingExercise.Sets.Add(set);
                Data.Core.DatabaseContext.Instance.SubmitChanges();
            }
        }

        private void SaveModel(SetVM setVM)
        {
            RepoSet.Instance.Save(setVM.Model);
        }

        public void FinishExecute()
        {
            SaveCurrent(true);
            Status = TrainingExerciseStatus.Finished;
        }

        public bool SkipExecute()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Execute_SkipWarning,
                                                      Resources.AppResources.Execute_SkipWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _state.Skip();
                Status = TrainingExerciseStatus.Skiped;
                return true;
            }

            return false;
        }

        public void Start()
        {
            Status = TrainingExerciseStatus.Started; 
            _state.Start();
        }

        public TrainingExerciseStatus Status
        {
            get
            {
                return _trainingExercise.Status;
            }
            set
            {
                _trainingExercise.Status = value;
                RepoTrainingExercise.Instance.Save(_trainingExercise);
                _state = _stateFactory.GetState();
                NotifyPropertyChanged("IsSkiped");
            }
        }

        private void PreviousSet(object o)
        {
            if (CurrentSet != null)
            {
                SaveCurrent(true);
                int previousNumber = CurrentSet.OrdinalNumber - 1;
                Set previousSet = Sets.SingleOrDefault(x => x.OrdinalNumber == previousNumber);
                if (previousSet != null)
                {
                    CurrentSet = new SetVM(previousSet);
                }
            }
        }

        private void NextSet(object o)
        {
            if (CurrentSet != null)
            {
                SaveCurrent();
                int nextNumber = CurrentSet.OrdinalNumber + 1;
                Set nextSet = Sets.SingleOrDefault(x => x.OrdinalNumber == nextNumber);
                if (nextSet != null)
                {
                    CurrentSet = new SetVM(nextSet);
                    return;
                }

                if (nextNumber == LastSet.OrdinalNumber)
                {
                    CurrentSet = LastSet;
                    FinishButtonState = true;
                    return;
                }

                LastSet = CreateNewSet(nextNumber);
                FinishCurrentSet();
                FinishButtonState = true;

                CurrentSet = LastSet;
                return;
            }

            LastSet = CreateNewSet(1);
            CurrentSet = LastSet;
        }

        private void FinishCurrentSet()
        {
            CurrentSet.Model.EndTime = DateTime.Now;
            SaveModel(CurrentSet);
            Sets.Add(CurrentSet.Model);
            Data.Core.DatabaseContext.Instance.SubmitChanges();
        }

        private SetVM CreateNewSet(int ordinal)
        {
            Set newSet = new Set { OrdinalNumber = ordinal, StartTime = DateTime.Now };
            Set[] sets = History.Select(x => x.TrainingExercise.Sets.FirstOrDefault(set => set.OrdinalNumber == ordinal)).ToArray();
            Set prevSet = sets.FirstOrDefault(x => x != null);

            if (prevSet != null)
            {
                newSet.Lift = prevSet.Lift;
                newSet.Reps = prevSet.Reps; 
            }
            
            return new SetVM(newSet);
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}