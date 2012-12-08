// <copyright file="Some.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    /// <summary>
    /// Represents an <see cref="Option{T}"/> where the reference object is defined
    /// </summary>
    /// <typeparam name="T">Of a class</typeparam>
    public class Some<T> : Option<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Some{T}" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public Some(T obj) 
            : base(obj)
        {
            this.IsDefined = obj != null;
        }
    }
}
