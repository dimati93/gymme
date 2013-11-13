using System.Collections.ObjectModel;
using Gymme.Resources;
using Gymme.Data.Repository;
using Gymme.ViewModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System;

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

        public ICommand AddWorkoutCommand
        {
            get
            {
                return GetOrCreateCommand("AddWorkoutCommand", NavigationManager.GotoAddWorkout);
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
            Workouts.Clear();

            foreach (var workout in RepoWorkout.Instance.FindAll())
            {
                Workouts.Add(new WorkoutVM(workout));
            }

            IsDataLoaded = true;
        }
    }
}