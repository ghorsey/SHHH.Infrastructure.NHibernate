// <copyright file="Collector.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to track pagination of data
    /// </summary>
    /// <typeparam name="T">A class</typeparam>
    public class Collector<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Collector{T}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="currentPage">The current page.</param>
        public Collector(List<T> items, int totalPages, int currentPage)
        {
            this.Items = items;
            this.TotalPages = totalPages;
            this.CurrentPage = currentPage;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<T> Items { get; private set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; private set; }
    }
}
