using System;
using System.Windows.Navigation;
using Gymme.Resources;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Gymme.View.Pages
{
    public partial class ExercisePage : PhoneApplicationPage
    {
        private ExercisePageVM _viewModel;

        public ExercisePage()
        {
            InitializeComponent();
            InitializeAppMenu();
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

        private ExercisePageVM GetDataContext(string target, long id)
        {
            return new ExercisePageVM(id);
        }

        private void InitializeAppMenu()
        {
            var editExercise = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.edit.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Edit
                };

            editExercise.Click += EditExercise_Click;
            ApplicationBar.Buttons.Add(editExercise);
            
            var deleteExercise = new ApplicationBarIconButton
                {
                    IconUri = new Uri("/Assets/AppBar/appbar.delete.rest.png", UriKind.Relative),
                    Text = AppResources.Command_Delete
                };

            deleteExercise.Click += DeleteExercise_Click;
            ApplicationBar.Buttons.Add(deleteExercise);
        }


        private void EditExercise_Click(object sender, EventArgs e)
        {
            _viewModel.EditExercise();
        }
        
        private void DeleteExercise_Click(object sender, EventArgs e)
        {
            if (_viewModel.DeleteExercise())
            {
                NavigationManager.GoBack();
            }
        }
    }
}