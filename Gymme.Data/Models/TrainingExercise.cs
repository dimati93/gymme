using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Gymme.Data.Models
{
    public enum TrainingExerciseStatus : byte
    {
        Created = 0,
        Started = 1,
        Skiped = 0x0F,
        Unfinished = 0x1F,
        Finished = 0xFF
    }

    [Table]
    public class TrainingExercise : Model
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

        private readonly EntitySet<Set> _sets = new EntitySet<Set>();

        public TrainingExercise()
        {
        }

        public TrainingExercise(Exercise exercise)
        {
            IdExecise = exercise.Id;
            Status = TrainingExerciseStatus.Created;
        }

        [Column]
        public long IdTraining { get; set; }

        [Column]
        public long IdExecise { get; set; }

        [Column]
        public DateTime? StartTime { get; set; }

        [Column]
        public DateTime? FinishTime { get; set; }

        [Column]
        public byte StatusId { get; set; }

        public TrainingExerciseStatus Status
        {
            get
            {
                return (TrainingExerciseStatus)StatusId;
            }
            set
            {
                StatusId = (byte)value;
                switch (value)
                {
                    case TrainingExerciseStatus.Started:
                        StartTime = DateTime.Now;
                        break;
                    case TrainingExerciseStatus.Unfinished:
                    case TrainingExerciseStatus.Finished:
                        FinishTime = DateTime.Now;
                        break;
                }
            }
        }

        [Association(Name = "FK_Training_TrExercise", Storage = "_sets", OtherKey = "IdTrainingExercise", DeleteRule = "CASCADE")]
        public EntitySet<Set> Sets
        {
            get
            {
                return _sets;
            }

            private set
            {
                _sets.Assign(value);
            }
        }
    }
}
