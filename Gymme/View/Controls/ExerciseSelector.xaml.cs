using System.Windows;

using Gymme.ViewModel;

using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Controls
{
    public partial class ExerciseSelector : LongListSelector
    {
        private static ExerciseSelector _lastCreated;
        private static ExerciseSelectItemVM _lastChoosen;

        private readonly ExerciseSelectorControlVM _viewModel;
        
        private ExerciseSelector(ExerciseSelectorControlVM viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel = viewModel;
        }

        public static ExerciseSelectItemVM LastChoosen
        {
            get { return _lastChoosen; }
        }

        private void Exercise_Tap(object sender, GestureEventArgs gestureEventArgs)
        {
            _lastChoosen = ((ExerciseSelectItemVM) ((FrameworkElement) sender).DataContext);
            LastChoosen.Choose();
        }

        public static ExerciseSelector Create(long workoutId)
        {
            if (_lastCreated == null || workoutId != _lastCreated._viewModel.WorkoutId)
            {
                _lastCreated = new ExerciseSelector(new ExerciseSelectorControlVM(workoutId));
                return _lastCreated;
            }

            _lastCreated = new ExerciseSelector(_lastCreated._viewModel);
            return _lastCreated;
        }
    }
}
