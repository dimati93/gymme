using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Gymme.Data.Models
{
    /// <summary>
    /// Store information about user trainings
    /// </summary>
    [Table]
    public class Workout : Model
    {
        #region Common

        private long _id;

        [Column(AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public override long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        #endregion

        [Column]
        public string Title { get; set; }

        [Column]
        public string Note { get; set; }

        private EntitySet<Exercise> _exercises = new EntitySet<Exercise>();

        [Association(Name = "FK_Workout_Exercise", Storage = "_exercises", OtherKey = "IdWorkout", DeleteRule = "NO ACTION")]
        public EntitySet<Exercise> Products
        {
            get
            {
                return _exercises;
            }

            private set
            {
                _exercises.Assign(value);
            }
        }
    }
}
