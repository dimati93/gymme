using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme
{
    public partial class MainPage : PhoneApplicationPage
    {
        public const string TargetWorkoutsList = "tgt:Workouts";
        public const string TargetUpcomingList = "tgt:Upcoming";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            Loaded += (o, e) => NavigationManager.SetNavigationService(NavigationService);
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.ViewModel.LoadData();
            string target = NavigationManager.GoBackParams;
            if (target != null)
            {
                switch (target)
                {
                    case TargetWorkoutsList:
                        MainPanorama.SetValue(Panorama.SelectedItemProperty, MainPanorama.Items[1]);
                        break;
                    case TargetUpcomingList:
                        MainPanorama.SetValue(Panorama.SelectedItemProperty, MainPanorama.Items[0]);
                        break;
                }
            }
        }

        private void Workout_Hold(object sender, GestureEventArgs e)
        {
            RadContextMenu.GetContextMenu((DependencyObject)sender).IsOpen = true;
        }
    }
}