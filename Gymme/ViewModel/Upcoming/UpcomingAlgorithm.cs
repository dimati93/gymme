using System.Collections.Generic;

namespace Gymme.ViewModel.Upcoming
{
    public abstract class UpcomingAlgorithm
    {
        public abstract IEnumerable<UpcomingItem> GetUpcoming();
    }
}