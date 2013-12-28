using System;
using System.Windows.Navigation;
using Gymme.Data.Models;
using Gymme.Resources;
using Gymme.ViewModel.Page;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Gymme.View.Pages
{
    public partial class ExecutionPage : PhoneApplicationPage
    {
        private ExecutePageVM _viewModel;

        public ExecutionPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationManager.GoBackParams = TrainingPage.GobBackUpdate;
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
                UpdateAppMenu();
            }
        }

        private void UpdateAppMenu()
        {
            ApplicationBar.Buttons.Clear();

            if (_viewModel != null)
            {
                switch (_viewModel.TrainingExercise.Status)
                {
                    case TrainingExerciseStatus.Started:
                        var finishExercise = new ApplicationBarIconButton
                            {
                                IconUri = new Uri("/Assets/AppBar/appbar.check.rest.png", UriKind.Relative),
                                Text = AppResources.Command_Finish,
                                IsEnabled = false
                            };

                        _viewModel.UpdadeFinishButtonState = x => finishExercise.IsEnabled = x;
                        finishExercise.Click += FinishExecute_Click;
                        ApplicationBar.Buttons.Add(finishExercise);

                        var skipExercise = new ApplicationBarIconButton
                            {
                                IconUri = new Uri("/Assets/AppBar/appbar.stop.rest.png", UriKind.Relative),
                                Text = AppResources.Training_Skip
                            };

                        skipExercise.Click += SkipExecute_Click;
                        ApplicationBar.Buttons.Add(skipExercise);
                        break;
                    
                    case TrainingExerciseStatus.Skiped:
                        var startExercise = new ApplicationBarIconButton
                            {
                                IconUri = new Uri("/Assets/AppBar/appbar.transport.play.rest.png", UriKind.Relative),
                                Text = AppResources.Command_Edit
                            };

                        startExercise.Click += StartExecute_Click;
                        ApplicationBar.Buttons.Add(startExercise);

                        break;
                }
            }
        }

        private void StartExecute_Click(object sender, EventArgs e)
        {
            _viewModel.Start();
            UpdateAppMenu();
        }

        private void FinishExecute_Click(object sender, EventArgs eventArgs)
        {
            _viewModel.FinishExecute();
            NavigationManager.GoBack(TrainingPage.GobBackUpdate);
        }

        private void SkipExecute_Click(object sender, EventArgs eventArgs)
        {
            if (_viewModel.SkipExecute())
            {
                UpdateAppMenu();
            }
        }

        private ExecutePageVM GetDataContext(string target, long id)
        {
            return new ExecutePageVM(id, ctrlSets.UpdateDataSources);
        }
    }
}