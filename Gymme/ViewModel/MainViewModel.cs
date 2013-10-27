using System.Collections.ObjectModel;
using Gymme.Resources;

namespace Gymme.ViewModels
{
    public class MainViewModel : Base.ViewModelBase
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

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
            // Sample data; replace with real data
            Items.Add(new ItemViewModel { LineOne = "Legs", LineTwo = "squats, deadlift, hyperextension", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            Items.Add(new ItemViewModel { LineOne = "Arms", LineTwo = "dumbbells and barrel lift", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            Items.Add(new ItemViewModel { LineOne = "Chest", LineTwo = "bench press, blocks", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            IsDataLoaded = true;
        }
    }
}