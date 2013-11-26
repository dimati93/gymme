using System;
using System.Windows;
using System.Windows.Navigation;
using Gymme.Resources;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View
{
    public partial class ExecutionPage : PhoneApplicationPage
    {
        public const string FromWorkoutPage = "fromWorkoutPage";

        private ExecutePageVM _viewModel;

        public ExecutionPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string target;
            if (!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                throw new InvalidOperationException("Navigation dead end");
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

        private ExecutePageVM GetDataContext(string target, long id)
        {
            return new ExecutePageVM(id);
        }
    }
}