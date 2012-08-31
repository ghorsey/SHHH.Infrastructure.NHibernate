using NUnit.Framework;
using SHHH.Infrastructure;

namespace SHHH.Infractructure.Tests
{
    [TestFixture]
    public class Option_TestFixture
    {
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
