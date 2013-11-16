using System;
using System.Data.Linq.Mapping;

namespace Gymme.Data.Models
{
    [Table]
    public class Training : Model
    {
        #region Common

        [Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public override long Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }

        #endregion

        public Training(Workout workout)
        {
            IdWorkout = workout.Id;
            StartTime = DateTime.Now;
        }

        [Column]
        public long IdWorkout { get; set; }

        [Column]
        public DateTime StartTime { get; set; }
    }
}
