// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrainingExercise.cs" company="">
//   
// </copyright>
// <summary>
//   The training exercise status.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Gymme.Data.Models
{
    /// <summary>
    /// The training exercise status.
    /// </summary>
    public enum TrainingExerciseStatus : byte
    {
        /// <summary>
        /// The created.
        /// </summary>
        Created = 0, 

        /// <summary>
        /// The started.
        /// </summary>
        Started = 1, 

        /// <summary>
        /// The skiped.
        /// </summary>
        Skiped = 0x0F, 

        /// <summary>
        /// The unfinished.
        /// </summary>
        Unfinished = 0x1F, 

        /// <summary>
        /// The finished.
        /// </summary>
        Finished = 0xFF
    }

    /// <summary>
    /// The training exercise.
    /// </summary>
    [Table]
    public class TrainingExercise : Model
    {
        #region Common

        /// <summary>
        /// The _id.
        /// </summary>
        private long _id;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
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

        /// <summary>
        /// The Exrsice.
        /// </summary>
        private EntityRef<Exercise> _exrsice;

        /// <summary>
        /// The _sets.
        /// </summary>
        private readonly EntitySet<Set> _sets = new EntitySet<Set>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingExercise"/> class.
        /// </summary>
        public TrainingExercise()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingExercise"/> class.
        /// </summary>
        /// <param name="exerciseId">
        /// The exercise id.
        /// </param>
        public TrainingExercise(long exerciseId)
        {
            IdExecise = exerciseId;
            Status = TrainingExerciseStatus.Created;
        }

        /// <summary>
        /// Gets or sets the id training.
        /// </summary>
        [Column]
        public long IdTraining { get; set; }

        /// <summary>
        /// Gets or sets the id execise.
        /// </summary>
        [Column]
        public long IdExecise { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [Column]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the finish time.
        /// </summary>
        [Column]
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Gets or sets the status id.
        /// </summary>
        [Column]
        public byte StatusId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
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
                        FinishTime = Sets.Select(x => x.EndTime).LastOrDefault() ?? DateTime.Now;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the sets.
        /// </summary>
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
