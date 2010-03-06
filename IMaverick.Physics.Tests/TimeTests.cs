using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IMaverick.Physics.Extensions;

namespace IMaverick.Physics.Tests
{
    [TestFixture]
    public class TimeTests
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual(1d.min(), 60d.s());
            Assert.AreEqual(60d.s().ToTimespan(), TimeSpan.FromMinutes(1));
            Assert.AreEqual(60d.s(), TimeSpan.FromSeconds(60).AsTime());
        }

        [Test]
        public void TestListOfTime()
        {
            var times = new List<PhysicalValue<Time>>();
            for ( var i = 1.0; i <= 100; ++i)
                times.Add(i.s());

            Assert.AreElementsEqual(times, times.ScaleTo(Time.Millisecond));
        }

        [Test]
        public void TestTimeSpan()
        {
            Assert.AreEqual(TimeSpan.FromDays(1).AsTime(), 1d.d());
        }
    }
}
