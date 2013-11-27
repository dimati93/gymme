using System;
using System.Data.Linq.Mapping;

namespace Gymme.Data.Models
{
    [Table]
    public class Set : Model
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

        [Column]
        public long IdTrainingExercise { get; set; }

        [Column]
        public float Lift { get; set; }

        [Column]
        public float Reps { get; set; }

        [Column]
        public DateTime StartTime { get; set; }
        
        [Column]
        public DateTime? EndTime { get; set; }

        [Column]
        public int OrdinalNumber { get; set; }
    }
}