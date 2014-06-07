using System;
using System.Collections.ObjectModel;

using Telerik.Windows.Controls;

namespace Gymme.ViewModel
{
    public class ExerciseSearchVM : ViewModelBase
    {
        private readonly ExerciseSelectorVM _selector;

        private string _searchText;

        private int _suggestionCount;

        public ExerciseSearchVM(ExerciseSelectorVM selector)
        {
            _selector = selector;
        }

        public ObservableCollection<ExerciseSelectItemVM> Items
        {
            get
            {
                return _selector.Items;
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (_searchText == value) return;

                _searchText = value;
                OnPropertyChanged("ShowList");
                OnPropertyChanged("BackgroundOpacity");
            }
        }

        public double BackgroundOpacity
        {
            get
            {
                return ShowList ? 1 : 0.5;
            }
        }

        public bool ShowList
        {
            get
            {
                return !string.IsNullOrEmpty(_searchText);
            }
        }

        public int SuggestionCount
        {
            get
            {
                return _suggestionCount;
            }
            set
            {
                if (_suggestionCount == value) return;

                _suggestionCount = value;
                OnPropertyChanged("ShowEmptyHint");
            }
        }

        public bool ShowEmptyHint
        {
            get
            {
                return ShowList && _suggestionCount == 0;
            }
        }
    }
}