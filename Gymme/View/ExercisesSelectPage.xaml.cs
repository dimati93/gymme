using System.Windows;
using System.Windows.Navigation;
using Gymme.ViewModel;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View
{
    public partial class ExercisesSelectPage : PhoneApplicationPage
    {
        private ExercisesSelectVM _viewModel;

        public ExercisesSelectPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            DataContext = _viewModel = new ExercisesSelectVM(long.Parse(NavigationContext.QueryString[AddEditChooser.Param.WorkoutId]));
        }

        private void Exercise_Tap(object sender, GestureEventArgs e)
        {
            ((ExerciseSelectItemVM)((FrameworkElement) sender).DataContext).Choose();
        }
    }
}