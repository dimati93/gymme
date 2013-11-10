using System.Collections.ObjectModel;
using Gymme.Resources;
using Gymme.Data.Repository;
using Gymme.ViewModel;

namespace Gymme.ViewModel
{
    public class MainViewModel : Base.ViewModelBase
    {
        public MainViewModel()
        {
            Workouts = new ObservableCollection<WorkoutVM>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<WorkoutVM> Workouts { get; private set; }

        private string _mainTitle = AppResources.ApplicationTitle;
        private string _upcommingWorkoutTitle = AppResources.Main_UpcomingWorkout;
        private string _workoutsTitle = AppResources.Common_Workouts;

        /// <summary>
        /// Main title property
        /// </summary>
        public string MainTitle
        {
            get
            {
                return _mainTitle;
            }
            set
            {
                _mainTitle = value;
                NotifyPropertyChanged("MainTitle");
            }
        }

        public string UpcommingWorkoutTitle
        {
            get
            {
                return _upcommingWorkoutTitle;
            }
            set
            {
                _upcommingWorkoutTitle = value;
                NotifyPropertyChanged("UpcommingWorkoutTitle");
            }
        }

        public string WorkoutsTitle
        {
            get
            {
                return _workoutsTitle;
            }
            set
            {
                _workoutsTitle = value;
                NotifyPropertyChanged("WorkoutsTitle");
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            foreach (var workout in RepoWorkout.Instance.FindAll())
            {
                Workouts.Add(new WorkoutVM(workout));
            }

            IsDataLoaded = true;
        }
    }
}