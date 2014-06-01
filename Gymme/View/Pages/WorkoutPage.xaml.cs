using System;
using System.Windows;
using System.Windows.Navigation;

using Gymme.Resources;
using Gymme.View.Helpers;
using Gymme.ViewModel;
using Gymme.ViewModel.Page;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Pages
{
    public partial class WorkoutPage : PhoneApplicationPage
    {
        private WorkoutPageVM _viewModel;
        private ApplicationBarIconButton _startWorkout;
        private RadDataBoundListBoxItem _reorderItem;

        public WorkoutPage()
        {
            InitializeComponent();
            InitializeAppMenu();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FlurryWP8SDK.Api.LogPageView();

            string target;
            if (!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                if (_viewModel != null)
                {
                    _viewModel.Update();
                    UpdateAppMenu();
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
                UpdateAppMenu();
            }
        }

        private void UpdateAppMenu()
        {
            _startWorkout.IsEnabled = !_viewModel.IsExercisesEmpty;
        }

        private WorkoutPageVM GetDataContext(string target, long id)
        {
            return new WorkoutPageVM(id) { UpdateAppMenu = UpdateAppMenu };
        }

        private void InitializeAppMenu()
        {
            _startWorkout = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.transport.play.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Start
                };

            _startWorkout.Click += StartWorkout_Click;
            ApplicationBar.Buttons.Add(_startWorkout);

            var addExercise = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Add
                };

            addExercise.Click += AddExercise_Click;
            ApplicationBar.Buttons.Add(addExercise);

            var editWorkoutMenuItem = new ApplicationBarMenuItem(AppResources.WorkoutPage_EditWorkout);
            editWorkoutMenuItem.Click += EditWorkout_Click;
            ApplicationBar.MenuItems.Add(editWorkoutMenuItem);

            var deleteWorkoutMenuItem = new ApplicationBarMenuItem(AppResources.WorkoutPage_DeleteWorkout);
            deleteWorkoutMenuItem.Click += DeleteWorkout_Click;
            ApplicationBar.MenuItems.Add(deleteWorkoutMenuItem);
        }

        private void StartWorkout_Click(object sender, EventArgs e)
        {
            _viewModel.StartWorkout();
        }

        private void AddExercise_Click(object sender, EventArgs e)
        {
            NavigationManager.GotoExercisesSelectPage(_viewModel.Item.Id);
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

        private void Ex_Hold(object sender, GestureEventArgs e)
        {
            var item = (FrameworkElement) sender;
            _reorderItem = item.ParentOfType<RadDataBoundListBoxItem>();
            RadContextMenu menu = RadContextMenu.GetContextMenu(item);
            menu.DataContext = item.DataContext;
            menu.IsOpen = true;
        }

        private void ContextMenu_Reorder(object sender, GestureEventArgs e)
        {
            if (_reorderItem != null)
            {
                exList.ActivateItemReorderForItem(_reorderItem);
            }
        }

        private void ExList_OnItemReorderStateChanged(object sender, ItemReorderStateChangedEventArgs e)
        {
            if (e.IsReorderActive == false)
            {
                _viewModel.ApplyReorder((ExerciseVM)_reorderItem.DataContext);
            }
        }
    }
}