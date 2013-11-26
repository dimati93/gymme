using System.Windows.Input;

namespace Gymme.ViewModel.Upcoming
{
    public abstract class UpcomingItem : Base.ViewModel
    {
        public ICommand GotoPageViewCommand
        {
            get
            {
                return GetOrCreateCommand("GotoPageViewCommand", GotoPageView);
            }
        }

        protected abstract void GotoPageView();
    }
}