using System;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;
using Gymme.Resources;

namespace Gymme.ViewModel.Upcoming
{
    public abstract class UpcomingItem : Base.ViewModel
    {
        private string _title;
        private string _description;
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
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }

        public Color Accent
        {
            get { return _accent; }
            set { _accent = value; NotifyPropertyChanged("Accent"); }
        }

        public int Priority { get; set; }

        protected string GetTimeDescription(DateTime time)
        {
            DateTime todayDate = DateTime.Now.Date;

            if (time.Date == todayDate.AddDays(1))
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0} {1} {2:t}", AppResources.Up_Tomorrow, AppResources.Up_At, time);
            }

            if (time.Date == todayDate)
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0} {1} {2:t}", AppResources.Up_Today, AppResources.Up_At, time);
            }

            if (time.Date == todayDate.AddDays(-1))
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0} {1} {2:t}", AppResources.Up_Yday, AppResources.Up_At, time);
            }

            return string.Format(CultureInfo.CurrentUICulture, "{0:d} {1} {0:t}", time, AppResources.Up_At);
        }
    }
}