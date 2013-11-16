using System;
using System.Windows.Navigation;

using Gymme.ViewModel;

using Microsoft.Phone.Controls;

namespace Gymme.View
{
    public partial class WorkoutPage : PhoneApplicationPage
    {
        private WorkoutPageVM _viewModel;

        public WorkoutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string target;
            if (!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                if (_viewModel != null)
                {
                    _viewModel.Update();
                    return;
                }
            }

            string id;
            if (!NavigationContext.QueryString.TryGetValue("id", out id))
            {
                NavigationManager.GoBack();
            }
            else
            {
                DataContext = _viewModel = GetDataContext(target, long.Parse(id));
            }
        }

        private WorkoutPageVM GetDataContext(string target, long id)
        {
            return new WorkoutPageVM(id);
        }

        private void EditWorkout_Click(object sender, EventArgs e)
        {
            _viewModel.EditWorkout();
        }
        
        private void DeleteWorkout_Click(object sender, EventArgs e)
        {
            if (_viewModel.DeleteWorkout())
            {
                NavigationManager.GoBack();
            }
        }
    }
}