// --------------------------------------------------------------------
// <copyright file="DatabaseInitializer.cs" company="Reflexor">
//      Copyright © Reflexor 2013-2014. All rights reserved.
// </copyright>
// <author>Дмитрий В. Петрович</author>
// ---------------------------------------------------------------------

using Gymme.Data.Models;
using Microsoft.Phone.Data.Linq;

namespace Gymme.Data.Core
{
    public class DatabaseInitialiser
    {
        public const int DATA_VERSION = 1;
        
        public static void UpdateDatabase(DatabaseContext db)
        {
            DatabaseSchemaUpdater dbUpdater = db.CreateDatabaseSchemaUpdater();
            if (dbUpdater.DatabaseSchemaVersion < DATA_VERSION)
            {
                // Update date from 1.1.48.0 version
                if (dbUpdater.DatabaseSchemaVersion < 1)
                {
                    dbUpdater.AddColumn<Exercise>("WithoutWeight");
                    dbUpdater.AddColumn<Exercise>("Order");
                }

                dbUpdater.DatabaseSchemaVersion = DATA_VERSION;
                dbUpdater.Execute();
            }
        }

        public static void SetLatestVersion(DatabaseContext db)
        {
            DatabaseSchemaUpdater dbUpdater = db.CreateDatabaseSchemaUpdater();
            dbUpdater.DatabaseSchemaVersion = DATA_VERSION;
            dbUpdater.Execute();
        }
    }
}