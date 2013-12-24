using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Gymme.Data.Repository;
using Gymme.ViewModel.Upcoming;
using Microsoft.Phone.Tasks;

namespace Gymme.ViewModel
{
    public class MainViewModel : Base.ViewModel
    {
        private readonly static UpcomingAlgorithm[] UpAlgorithms = { new UnfinishedTraining(), new RoutinesTraining() };

        public MainViewModel()
        {
            Workouts = new ObservableCollection<WorkoutVM>();
            UpcomingItems = new ObservableCollection<UpcomingItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<WorkoutVM> Workouts { get; private set; }

        public ObservableCollection<UpcomingItem> UpcomingItems { get; private set; }

        public bool IsUpcomingEmpty
        {
            get { return UpcomingItems.Count == 0; }
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

        public ICommand RateCommand
        {
            get
            {
                return GetOrCreateCommand("RateCommand", () => new MarketplaceReviewTask().Show());
            }
        }

        public ICommand HelpCommand
        {
            get
            {
                return GetOrCreateCommand("HelpCommand", NavigationManager.GotoHelp);
            }
        }

        public bool IsDataLoaded { get; private set; }
                     
        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            LoadUpcoming();
            LoadWorkouts();
            IsDataLoaded = true;
        }

        private void LoadWorkouts()
        {
            Workouts.Clear();
            foreach (var workout in RepoWorkout.Instance.FindAll())
            {
                Workouts.Add(new WorkoutVM(workout, this));
            }

            NotifyPropertyChanged("IsWorkoutsEmpty");
        }


        private void LoadUpcoming()
        {
            UpcomingItems.Clear();
            foreach (var upcomingItem in UpAlgorithms.SelectMany(x => x.GetUpcoming().OrderBy(y => y.Priority)))
            {
                UpcomingItems.Add(upcomingItem);
            }

            NotifyPropertyChanged("IsUpcomingEmpty");
        }
    }
}