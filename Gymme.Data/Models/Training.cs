using System;
using System.Data.Linq.Mapping;
namespace Gymme.Data.Models
{
    [Table]
    public class Training : Model
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
