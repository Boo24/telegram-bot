using System;
using System.Linq;
using NUnit.Framework;
using source.Infrastructure.Databases;
using source.Infrastructure.Serialization;
using FakeItEasy;
using System.IO;

namespace Tests
{
    [TestFixture]
    class ArrayDatabaseTests
    {
        string[] data = new string[] { "lala", "bubu", "lala" };
        ArrayDatabase<string> database;

        [SetUp]
        public void SetUp()
        {
            var fakeSerializer = A.Fake<ISerializer>();
            A.CallTo(() => fakeSerializer.Deserialize<string[]>(A<Stream>.Ignored))
                .Returns(data);

            database = new ArrayDatabase<string>(A.Fake<Stream>(), fakeSerializer);
        }

        [Test]
        public void should_get_all_suitable()
        {
            var suitables = database.GetAllSuitable(str => str == "lala");
            Assert.AreEqual(suitables.Count(), 2);
            foreach (var suitable in suitables)
            {
                Assert.AreEqual(suitable, "lala");
            }
        }

        [Test]
        public void should_get_any_suitable()
        {
            var suitable = database.GetAnySuitable(str => str == "lala");
            Assert.AreEqual(suitable, "lala");

            suitable = database.GetAnySuitable(str => str == "bubu");
            Assert.AreEqual(suitable, "bubu");
        }

        [Test]
        public void should_throw_exception_when_no_suitable()
        {
            Assert.That(() => database.GetAnySuitable(str => str == "missing"),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}
