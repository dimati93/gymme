// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CEDataContext.cs" company="CoreI4">
//   Copyright © CoreI4 2012. All rights reserved.
// </copyright>
// <summary>
//   The SQL Server CE data context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Linq;

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
    }
}