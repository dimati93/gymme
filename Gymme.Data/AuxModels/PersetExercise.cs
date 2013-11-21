using Gymme.Data.Interfaces;

namespace Gymme.Data.AuxModels
{
    public class PersetExercise : IExercise
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}