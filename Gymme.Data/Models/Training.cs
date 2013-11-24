﻿using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
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
                return (TrainingStatus) StatusId;
            }
            set
            {
                StatusId = (byte) value;
            }
        }
        private EntitySet<TrainingExercise> _exercises = new EntitySet<TrainingExercise>();

        [Association(Name = "FK_Training_TrExercise", Storage = "_exercises", OtherKey = "IdTraining", DeleteRule = "NO ACTION")]
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
