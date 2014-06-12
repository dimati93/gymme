using Gymme.Data.Interfaces;

namespace Gymme.Data.AuxModels
{
    public class PersetExercise : IExercise
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public bool? WithoutWeight { get; set; }
    }
}