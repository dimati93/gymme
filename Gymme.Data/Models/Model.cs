// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="d93">
//   Copyright © d93 2013. All rights reserved.
// </copyright>
// <summary>
//   The model type base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Linq.Mapping;

namespace Gymme.Data.Models
{
    /// <summary>
    /// The model type base class.
    /// </summary>
    public abstract class Model
    {
        private long _id;

        protected Model()
        {
            IsNew = true;
        }

        [Column(Name="Id")]
        public abstract long Id { get; set; }
        
        public bool IsNew { get; protected set; }
    }
}
