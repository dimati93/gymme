using System.Windows;

using Gymme.ViewModel;

using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Controls
{
    public partial class ExerciseSelector : RadJumpList
    {
        private readonly ExerciseSelectorVM _viewModel;
        
        public ExerciseSelector(long workoutId)
        {
            InitializeComponent();
            DataContext = _viewModel = new ExerciseSelectorVM(workoutId);
        }

        public ExerciseSelectorVM ViewModel
        {
            get
            {
                return _viewModel;
            }
        }

        private void Exercise_Tap(object sender, GestureEventArgs gestureEventArgs)
        {
            var choosen = ((ExerciseSelectItemVM) ((FrameworkElement) sender).DataContext);
            choosen.Choose();
        }
    }
}
