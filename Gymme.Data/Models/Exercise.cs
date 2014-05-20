using System.Data.Linq.Mapping;
using Gymme.Data.Interfaces;

namespace Gymme.Data.Models
{
    [Table]
    public class Exercise : Model, IExercise
    {
        #region Common
        private long _id;

        [Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                IsNew = false;
            }
        }
        #endregion

        public Exercise() {}

        public Exercise(IExercise anotherExercise)
        {
            Name = anotherExercise.Name;
            Category = anotherExercise.Category;
        }

        [Column]
        public long IdWorkout { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Note { get; set; }

        [Column]
        public string Category { get; set; }

        [Column]
        public bool? WithoutWeight { get; set; }
    }
}
