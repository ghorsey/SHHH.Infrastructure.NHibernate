// <copyright file="Option_TestFixture.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infractructure.Tests
{
    using NUnit.Framework;
    using SHHH.Infrastructure;

    /// <summary>
    /// The test fixture for <see cref="Option{T}"/> objects
    /// </summary>
    [TestFixture]
    public class Option_TestFixture
    {
        /// <summary>
        /// Test using <c>MakeOption</c>
        /// </summary>
        [Test]
        public void MakeOption_Test()
        {
            var list = new List();
            var some = Option<List>.MakeOption(list);
            var none = Option<List>.MakeOption(null);

            Assert.IsTrue(some is Some<List>);
            Assert.IsTrue(some.IsDefined);
            Assert.IsNotNull(some.Object);

            Assert.AreSame(list, some.Object);
            Assert.IsTrue(none is None<List>);
            Assert.IsFalse(none.IsDefined);
            Assert.IsNull(none.Object);
        }

        /// <summary>
        /// Test the implicit class operation
        /// </summary>
        [Test]
        public void ImplicitCast_Test()
        {
            var list = new List();
            Option<List> some = list;
            
            Assert.IsNotNull(some.Object);
            Assert.AreSame(list, some.Object);
            Assert.IsTrue(some.IsDefined);
        }
    }
}
