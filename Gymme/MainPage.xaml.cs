using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme
{
    public partial class MainPage : PhoneApplicationPage
    {
        public const string TargetWorkoutsList = "tgt:Workouts";

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
            string target = NavigationManager.GetGoBackParams();
            if (target != null)
            {
                if (target == TargetWorkoutsList)
                {
                    MainPanorama.SetValue(Panorama.SelectedItemProperty, MainPanorama.Items[1]);
                }
            }
        }

        private void Workout_Hold(object sender, GestureEventArgs e)
        {
            ContextMenuService.GetContextMenu((DependencyObject)sender).IsOpen = true;
        }
    }
}