// <copyright file="ILogicalDelete.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    /// <summary>
    /// An interface used by the NHibernate library to logically delete an object
    /// </summary>
    public interface ILogicalDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; set; }
    }
}
