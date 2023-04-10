using Assets.Scripts;
using NUnit.Framework;

namespace Tests
{
    public class NewTestScript
    {
        [SetUp]
        public void Setup() {}

        [Test]
        public void TestIncrement()
        {
            //Assign
            var counter = new BasicCounter(0);

            //Act
            counter.Increment();

            //Assert
            Assert.AreEqual(1, counter.Count);
        }

        [Test]
        public void TestMaxCount()
        {
            //Assign
            var counter = new BasicCounter(10);

            //Act
            counter.Increment();

            //Assert
            Assert.AreEqual(10, counter.Count);
        }
    }
}