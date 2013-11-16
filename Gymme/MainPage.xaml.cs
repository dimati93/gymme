using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme
{
    public partial class MainPage : PhoneApplicationPage
    {
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
        }

        private void Workout_Hold(object sender, GestureEventArgs e)
        {
            Grid dataTemplateGrid = (Grid)sender;

            ContextMenu context = ContextMenuService.GetContextMenu(dataTemplateGrid);
            context.IsOpen = true;
        }
    }
}