using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Gymme.Data.Repository;

namespace Gymme.Data.Models
{
    public enum TrainingStatus : byte
    {
        Started = 0,
        Finished = 0xFF
    }

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

        private readonly EntitySet<TrainingExercise> _exercises = new EntitySet<TrainingExercise>();

        public Training()
        {
        }

        public Training(Workout workout)
        {
            IdWorkout = workout.Id;
            StartTime = DateTime.Now;
            Status = TrainingStatus.Started;
        }

        [Column]
        public long IdWorkout { get; set; }

        [Column]
        public DateTime StartTime { get; set; }

        [Column]
        public byte StatusId { get; set; }

        public TrainingStatus Status
        {
            get
            {
                return (TrainingStatus)StatusId;
            }
            set
            {
                StatusId = (byte)value;
                switch (value)
                {
                    case TrainingStatus.Finished:
                        foreach (var exercise in Exercises.Where(x => x.Status == TrainingExerciseStatus.Started))
                        {
                            exercise.Status = TrainingExerciseStatus.Unfinished;
                            RepoTrainingExercise.Instance.Save(exercise);
                        }

                        foreach (var exercise in Exercises.Where(x => x.Status == TrainingExerciseStatus.Created))
                        {
                            exercise.Status = TrainingExerciseStatus.Skiped;
                            RepoTrainingExercise.Instance.Save(exercise);
                        }
                        break;
                }
            }
        }

        [Association(Name = "FK_Training_TrExercise", Storage = "_exercises", OtherKey = "IdTraining", DeleteRule = "CASCADE")]
        public EntitySet<TrainingExercise> Exercises
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
