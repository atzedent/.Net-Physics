using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IMaverick.Physics.Extensions;
using IMaverick.Gallio.Extensions;

namespace IMaverick.Physics.Tests
{
    [TestFixture]
    public class TemperatureTests
    {
        [Test]
        public void Test()
        {
            var celsius = 30d.Celsius();
            var fahrenheit = 86d.Fahrenheit();

            Assert.AreEqual(celsius, fahrenheit);
            TestLog.WriteLine("Compared equality of {0} and {1}", celsius, fahrenheit);
            TestLog.WriteHighlighted(string.Format("Values are: {0} | {1}\n", celsius.ToDefault(), fahrenheit.ToDefault()));
            Assert.AreEqual(fahrenheit, 303.15.Kelvin());
            TestLog.WriteLine("Compared equality of {0} and {1}", fahrenheit, 303.15.Kelvin());
            TestLog.WriteHighlighted(string.Format("Values are: {0} | {1}\n", fahrenheit.ToDefault(), 303.15.Kelvin()));
        }

        [Test]
        public void ScaleTest()
        {
            var celsius = 30d.Celsius();
            var kelvin = 303.15.Kelvin();
            var fahrenheit = 86d.Fahrenheit();

            Assert.AreEqual(kelvin, celsius.ScaleTo(Temperature.Kelvin));
            Assert.AreEqual(fahrenheit, celsius.ScaleTo(Temperature.Fahrenheit));
            Assert.AreEqual(celsius.ToDefault(), kelvin.ToDefault());
            Assert.AreEqual(celsius, kelvin.ScaleTo(Temperature.Celsius));
            Assert.AreEqual(celsius, kelvin);
            Assert.AreEqual(1, celsius.DivideBy(fahrenheit));
            Assert.AreEqual(celsius, celsius.ScaleTo(Temperature.Celsius));
        }

        [Test]
        public void TestPlot()
        {
            Func<int, int, Temperature, int, IEnumerable<PhysicalValue<Temperature>>> lineCreator = (start, end, unit, step) => Sequence.AddRange(start, end, unit, step);
            var yValues = lineCreator(0, 250000, Temperature.Celsius, 10).Concat(lineCreator(249999, 0, Temperature.Celsius, 10));

            Func<IEnumerable<PhysicalValue<Temperature>>, Double, IEnumerable<Tuple<PhysicalValue<Time>, PhysicalValue<Temperature>>>> curveCreator = (values, indexer) => from y in values select Tuple.Create(indexer++.s(), y);
            var curve = curveCreator(yValues, default(double)).ToList();

            curve.Plot();
        }

        [Test]
        public void TestGaussianCurve()
        {
            /// http://de.wikipedia.org/wiki/Normalverteilung
            /// d ist Standardabweichung
            /// n ist Erwartungswert
            Func<double, double, double, double> gauss = (x, n, d) => (1 / (d * Math.Sqrt(Math.PI * 2))) * Math.Exp((-0.5) * Math.Pow((x - n) / d, 2));

            var sequence = Sequence.AddRange(40, 1, Time.Second).Select(v => v * -1).Concat(Sequence.AddRange(0, 40, Time.Second));

            Func<IEnumerable<PhysicalValue<Time>>, IEnumerable<Tuple<PhysicalValue<Time>, PhysicalValue<Temperature>>>> curveCreator = values => from x in values select Tuple.Create(x, (gauss(x.Value / 10, 0, 1)).Celsius());
            var curve = curveCreator(sequence).ToList();

            curve.Plot();
        }
    }
}
