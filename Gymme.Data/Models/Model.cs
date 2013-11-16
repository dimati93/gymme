// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="d93">
//   Copyright © d93 2013. All rights reserved.
// </copyright>
// <summary>
//   The model type base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gymme.Data.Models
{
    /// <summary>
    /// The model type base class.
    /// </summary>
    public abstract class Model
    {
        protected Model()
        {
            IsNew = true;
        }
        
        public bool IsNew { get; protected set; }
    }
}
