using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Gymme.ViewModel;

using Telerik.Windows.Controls;

namespace Gymme.View.Controls
{
    public partial class ExerciseSearch : UserControl
    {
        private readonly ExerciseSearchVM _viewModel;

        private BindingExpression _textBinding;

        public ExerciseSearch(ExerciseSelectorVM viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel = new ExerciseSearchVM(viewModel);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _textBinding = searchBox.GetBindingExpression(TextBox.TextProperty);
            searchBox.Focus();
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _textBinding.UpdateSource();
            _viewModel.SuggestionCount = searchBox.FilteredSuggestions.Count();
        }
    }
}
