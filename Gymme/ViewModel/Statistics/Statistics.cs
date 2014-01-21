using System.Threading.Tasks;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Statistics
{
    public abstract class Statistics : Base.ViewModel
    {
        private bool _isLoaded;
        private bool _isLoading;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
                NotifyPropertyChanged("IsLoaded");
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public void LoadStatistics()
        {
            IsLoading = true;
            ProcedeLoad();

            IsLoading = false;
            IsLoaded = true;
        }

        protected abstract void ProcedeLoad();
    }
}