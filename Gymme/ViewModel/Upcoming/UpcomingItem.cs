using System.Windows.Input;
using System.Windows.Media;

namespace Gymme.ViewModel.Upcoming
{
    public abstract class UpcomingItem : Base.ViewModel
    {
        private string _title;
        private Color _accent;

        public ICommand GotoPageViewCommand
        {
            get
            {
                return GetOrCreateCommand("GotoPageViewCommand", GotoPageView);
            }
        }

        protected abstract void GotoPageView();

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        public string Description
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Description"); }
        }

        public Color Accent
        {
            get { return _accent; }
            set { _accent = value; NotifyPropertyChanged("Accent"); }
        }
    }
}