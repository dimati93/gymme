using System.Data.Linq;
using System.Windows;

using Gymme.ViewModel;

using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Controls
{
    public partial class ExerciseSelector : RadJumpList
    {
        private static ExerciseSelector _lastCreated;
        private static ExerciseSelectItemVM _lastChoosen;

        private readonly ExerciseSelectorVM _viewModel;
        
        private ExerciseSelector(ExerciseSelectorVM viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel = viewModel;
        }

        public static ExerciseSelectItemVM LastChoosen
        {
            get { return _lastChoosen; }
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
            _lastChoosen = ((ExerciseSelectItemVM) ((FrameworkElement) sender).DataContext);
            LastChoosen.Choose();
        }

        public static ExerciseSelector Create(long workoutId)
        {
            if (_lastCreated == null || workoutId != _lastCreated.ViewModel.WorkoutId)
            {
                _lastCreated = new ExerciseSelector(new ExerciseSelectorVM(workoutId));
                return _lastCreated;
            }

            _lastCreated = new ExerciseSelector(_lastCreated.ViewModel);
            return _lastCreated;
        }
    }
}
