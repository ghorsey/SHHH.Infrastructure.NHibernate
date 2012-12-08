// <copyright file="Option.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    /// <summary>
    /// The option class
    /// </summary>
    /// <typeparam name="T">Of a Class</typeparam>
    public abstract class Option<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Option{T}" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public Option(T obj)
        {
            this.Object = obj;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public T Object { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is defined.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is defined; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefined { get; protected set; }

        /// <summary>
        /// Makes the option.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="Option{T}"/></returns>
        public static Option<T> MakeOption(T obj)
        {
            if (default(T) == obj)
            {
                return new None<T>();
            }
            else
            {
                return new Some<T>(obj);
            }
        }

        /// <summary>
        /// Options the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="Some{T}"/></returns>
        public static implicit operator Option<T>(T obj)
        {
            return new Some<T>(obj);
        }
    }
}
