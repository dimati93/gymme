using System;

namespace Gymme.ViewModel.Statistics
{
    public class FloatStatPoint : Base.ViewModel
    {
        public FloatStatPoint(DateTime date, float value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; private set; }

        public float Value { get; private set; }
    }
}