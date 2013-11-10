using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Gymme.Data.Models
{
    [Table]
    public class Exercise : Model
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
        public long IdWorkout { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Note { get; set; }
    }
}
