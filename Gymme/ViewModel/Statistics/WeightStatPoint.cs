using System;

namespace Gymme.ViewModel.Statistics
{
    public class WeightStatPoint : Base.ViewModel
    {
        public WeightStatPoint(DateTime date, float weight)
        {
            Date = date;
            Weight = weight;
        }

        public DateTime Date { get; private set; }

        public float Weight { get; private set; }
    }
}