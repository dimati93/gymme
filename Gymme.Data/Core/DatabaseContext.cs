// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CEDataContext.cs" company="CoreI4">
//   Copyright © CoreI4 2012. All rights reserved.
// </copyright>
// <summary>
//   The SQL Server CE data context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Gymme.Data.Models;
using System.Data.Linq;
using Microsoft.Phone.Data.Linq;

namespace Gymme.Data.Core
{
    /// <summary>
    /// The data context.
    /// </summary>
    public class DatabaseContext : DataContext
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private const string ConnectionString = "isostore:/gymmydb.sdf";
        
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DatabaseContext"/> class from being created.
        /// </summary>
        private DatabaseContext()
            : base(ConnectionString)
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
                
#if DEBUG        
                Workout.InsertAllOnSubmit(new [] { 
                    new Workout { Title = "Legs", Note = "squats, deadlift, hyperextension" }, 
                    new Workout { Title = "Arms", Note = "dumbbells and barrel lift" } 
                });

                SubmitChanges();
#endif
                DatabaseInitialiser.SetLatestVersion(this);
            }
            else
            {
                DatabaseInitialiser.UpdateDatabase(this);
            }
        }

        #endregion

        #region Singleton

        /// <summary>
        /// The instance of.
        /// </summary>
        private static DatabaseContext instance;

        /// <summary>
        /// Gets the instance of CEDataContext class.
        /// </summary>
        public static DatabaseContext Instance
        {
            get
            {
                return instance ?? (instance = new DatabaseContext());
            }
        }

        #endregion

        public Table<Workout> Workout
        {
            get
            {
                return GetTable<Workout>();
            }
        }

        public Table<Exercise> Excercise
        {
            get
            {
                return GetTable<Exercise>();
            }
        }

        public Table<Training> Training
        {
            get
            {
                return GetTable<Training>();
            }
        }

        public Table<TrainingExercise> TrainingExercise
        {
            get
            {
                return GetTable<TrainingExercise>();
            }
        }
        
        public Table<Set> Set
        {
            get
            {
                return GetTable<Set>();
            }
        }
    }
}