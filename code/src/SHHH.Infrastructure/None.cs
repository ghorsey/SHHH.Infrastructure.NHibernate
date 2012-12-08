// <copyright file="None.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    /// <summary>
    /// Represents an <see cref="Option{T}"/> where the object is not defined
    /// </summary>
    /// <typeparam name="T">Of a class</typeparam>
    public class None<T> : Option<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="None{T}" /> class.
        /// </summary>
        public None()
            : base(default(T))
        {
            this.IsDefined = false;
        }
    }
}
