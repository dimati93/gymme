using System.Windows.Navigation;
using Gymme.ViewModel;
using Microsoft.Phone.Controls;

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
            if (_viewModel == null)
            {
                _viewModel = new ExercisesSelectVM();
            }

            DataContext = _viewModel;
        }
    }
}