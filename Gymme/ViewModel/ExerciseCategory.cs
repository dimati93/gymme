using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Gymme.Data.Interfaces;

namespace Gymme.ViewModel
{
    public class ExerciseCategory : IGrouping<string, IExercise>
    {
        private readonly IGrouping<string, IExercise> _exercises;

        public ExerciseCategory(IGrouping<string, IExercise> grouping)
        {
            _exercises = grouping;
        }

        public IEnumerator<IExercise> GetEnumerator()
        {
            return _exercises.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Key { get { return _exercises.Key; } }

    }
}