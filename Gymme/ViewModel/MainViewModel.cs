using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymme.Data.Repository;

namespace Gymme.ViewModel
{
    public class MainViewModel : Base.ViewModel
    {
        public MainViewModel()
        {
            Workouts = new ObservableCollection<WorkoutVM>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<WorkoutVM> Workouts { get; private set; }

        public bool IsUpcomingEmpty
        {
            get { return true; }
        }

        public bool IsWorkoutsEmpty
        {
            get { return Workouts.Count == 0; }
        }

        public ICommand AddWorkoutCommand
        {
            get
            {
                return GetOrCreateCommand("AddWorkoutCommand", NavigationManager.GotoAddWorkout);
            }
        }

        public bool IsDataLoaded { get; private set; }
                     
        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            Workouts.Clear();

            foreach (var workout in RepoWorkout.Instance.FindAll())
            {
                Workouts.Add(new WorkoutVM(workout, this));
            }

            NotifyPropertyChanged("IsWorkoutsEmpty");
            IsDataLoaded = true;
        }
    }
}