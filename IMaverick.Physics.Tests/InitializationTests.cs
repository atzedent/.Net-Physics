using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IMaverick.Physics.Extensions;

namespace IMaverick.Physics.Tests
{
    [TestFixture]
    public class InitializationTests
    {
        [Test]
        public void Test()
        {
            var length = new PhysicalValue<Length>(100, Length.Meter);

            var length2 = 100d.m();

            Assert.AreEqual(length, length2);
        }

        [Test]
        public void TestScaling()
        {
            var centimeter = 100d.cm();
            var meter = 1d.m();
            var kilometer = 0.1.km();

            Assert.AreEqual(centimeter, meter);
            Assert.AreEqual(kilometer, meter * 100);
            Assert.AreEqual(centimeter, 50d.cm() + 50d.cm());
            Assert.AreEqual(meter, kilometer - (centimeter * 99));
        }

        [Test]
        public void TestConstruction()
        {
            var length = new PhysicalValue<Length>();

            Assert.AreEqual(length, 0d.m());
        }

        [Test]
        public void TestHashing()
        {
            var length1 = 1d.m();
            var length2 = 1d.m();
            var length3 = 100d.cm();

            Assert.AreEqual(length1.GetHashCode(), length2.GetHashCode());
            Assert.AreNotEqual(length1.GetHashCode(), length3.GetHashCode());

            Assert.AreEqual(length1.Unit.GetHashCode(), length2.Unit.GetHashCode());
            Assert.AreNotEqual(length1.Unit.GetHashCode(), length3.Unit.GetHashCode());
        }

        [Test]
        public void TestScaleTo()
        {
            var cm = 100d.cm();
            var m = 1d.m();

            Assert.AreEqual(cm.ScaleTo(Length.Meter), m);
            Assert.AreEqual(m.ScaleTo(Length.Centimeter), cm);
        }

        [Test]
        public void TestGeneralExtensions()
        {
            var km = 1d.Unit(Length.Kilometer);

            Assert.AreEqual(1d.km(), km);
            Assert.AreEqual(1, km.DivideBy(km));
        }

        [Test]
        public void TestUnitFromString()
        {
            var length1 = new PhysicalValue<Length>(1, Length.FromString("cm"));
            var length2 = new PhysicalValue<Length>(1, "cm");

            Assert.AreEqual(1d.cm(), length1);
            Assert.AreEqual(length1, length2);
        }

        [Test]
        [MultipleAsserts]
        public void DimensionTest()
        {
            Assert.AreEqual(1E+24, OrderOfMagnitude.Yotta.Value);
            Assert.AreEqual(1E+21, OrderOfMagnitude.Zetta.Value);
            Assert.AreEqual(1000000000000000000, OrderOfMagnitude.Exa.Value);
            Assert.AreEqual(1000000000000000, OrderOfMagnitude.Peta.Value);
            Assert.AreEqual(1000000000000, OrderOfMagnitude.Tera.Value);
            Assert.AreEqual(1000000000, OrderOfMagnitude.Giga.Value);
            Assert.AreEqual(1000000, OrderOfMagnitude.Mega.Value);
            Assert.AreEqual(1000, OrderOfMagnitude.Kilo.Value);
            Assert.AreEqual(100, OrderOfMagnitude.Hecto.Value);
            Assert.AreEqual(10, OrderOfMagnitude.Deca.Value);
            Assert.AreEqual(0.1, OrderOfMagnitude.Deci.Value);
            Assert.AreEqual(0.01, OrderOfMagnitude.Centi.Value);
            Assert.AreEqual(0.001, OrderOfMagnitude.Milli.Value);
            Assert.AreEqual(0.000001, OrderOfMagnitude.Micro.Value);
            Assert.AreEqual(0.000000001, OrderOfMagnitude.Nano.Value);
            Assert.AreEqual(0.000000000001, OrderOfMagnitude.Pico.Value);
            Assert.AreEqual(0.000000000000001, OrderOfMagnitude.Femto.Value);
            Assert.AreEqual(0.000000000000000001, OrderOfMagnitude.Atto.Value);
            Assert.AreEqual(0.000000000000000000001, OrderOfMagnitude.Zepto.Value);
            Assert.AreEqual(0.000000000000000000000001, OrderOfMagnitude.Yokto.Value);
        }

        [Test]
        [MultipleAsserts]
        public void TestOperators()
        {
            Assert.AreEqual(5.0 * 2.0, (5d.m() * 2).Value);
            Assert.AreEqual(5.0 / 2.0, (5d.m() / 2).Value);
            Assert.AreEqual(5.0 % 2.0, (5d.m() % 2).Value);
            Assert.AreEqual(5.0 == 5.0, 5d.km() == 5d.km());
            Assert.AreEqual(5.0 != 2.0, 5d.m() != 2d.m());
            Assert.AreEqual(5.0 > 2.0, 5d.m() > 2d.m());
            Assert.AreEqual(5.0 >= 5.0, 5d.m() >= 5d.m());
            Assert.AreEqual(2.0 < 5.0, 2d.m() < 5d.m());
            Assert.AreEqual(5.0 <= 5.0, 5d.m() <= 5d.m());
            Assert.AreEqual(5.0 - 2.0, (5d.m() - 2d.m()).Value);
            Assert.AreEqual(5.0 + 2.0, (5d.m() + 2d.m()).Value);

            Assert.AreEqual(5.0 / 9.0, (5d.m() / 9).Value);
            AssertEx.That(() => (5.0 / 9.0) == (5d.m() / 9).Value);
            Assert.AreEqual(5.0 / 9.0, 5d.m().DivideBy(9d.m()));
        }

        [Test]
        [MultipleAsserts]
        public void TestCurveSpecifications()
        {
            var curve = Curve;
            var max = curve.Max(d => d.Item2);

            var twoZeroPointsSpec = curve.CreateSpecification(p => (from c in p where c.Item2 == 0d.m() select c).Count() == 2);
            var onePeakSpec = curve.CreateSpecification(p => (from c in p where c.Item2 == max select c).Count() == 1);
            var isoEquiDistSpec = curve.CreateSpecification(p => IsoEquidistantCheck(p.ToList()));

            AssertEx.That(() => curve.IsSatisfied(twoZeroPointsSpec.And(onePeakSpec.And(isoEquiDistSpec))));    
            
            TestLog.Write("Number of elements processed: ");
            TestLog.WriteHighlighted(string.Format("{0:N}\n", curve.Count));
        }

        [Test]
        public void TestCurveSpecInFunc()
        {
            var curve = Curve;
            var max = curve.Max(d => d.Item2);
            Func<List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>>, bool> twoZeroPointsFunc = p =>
                {
                    var crv = p.AsParallel();
                    return (from c in crv where c.Item2 == 0d.m() select c).Count() == 2;
                };
            var twoZeroPointsSpec = curve.CreateSpecification(p => twoZeroPointsFunc(p.ToList()));
            Func<List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>>, bool> onePeakFunc = p =>
                {
                    var crv = p.AsParallel();
                    return (from c in crv where c.Item2 == max select c).Count() == 1;
                };
            var onePeakSpec = curve.CreateSpecification(p => onePeakFunc(p.ToList()));
            Func<List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>>, bool> isoEquiDistFunc = p =>
            {
                for (var i = 1; i < p.Count - 1; i++)
                {
                    if (curve[i].Item2 != max)
                        return curve[i].Item2 - curve[i - 1].Item2 == curve[i + 1].Item2 - curve[i].Item2;
                } return true;
            };
            var isoEquiDistSpec = curve.CreateSpecification(p => isoEquiDistFunc(p.ToList()));

            var result = curve.IsSatisfied(twoZeroPointsSpec.And(onePeakSpec.And(isoEquiDistSpec)));

            Assert.IsTrue(result);

            TestLog.Write("Number of elements processed: ");
            TestLog.WriteHighlighted(string.Format("{0:N}\n", curve.Count));
        }
        
        [FixtureInitializer]
        public void Init ( )
        {
            Func<int, int, Length, int, IEnumerable<PhysicalValue<Length>>> lineCreator = (start, end, unit, step) => Sequence.AddRange(start, end, unit, step);
            var yValues = lineCreator(0, 250000, Length.Centimeter, 10).Concat(lineCreator(249999, 0, Length.Centimeter, 10));

            Func<IEnumerable<PhysicalValue<Length>>,Double,IEnumerable<Tuple<PhysicalValue<Time>,PhysicalValue<Length>>>> curveCreator = (values,indexer) => from y in values select Tuple.Create(indexer++.s(), y);

            Curve = curveCreator(yValues, default(double)).ToList();
        }
        
        private List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>> Curve { get; set; }
        
        [Test]
        [MultipleAsserts]
        public void TestListOperations()
        {
            var curve = Curve;
            var max = curve.Max(d => d.Item2);
            
            for ( var i = 1; i < curve.Count-1; i++)
            {
                if (curve[i].Item2 != max)
                    Assert.AreEqual(curve[i].Item2 - curve[i-1].Item2, curve[i+1].Item2 - curve[i].Item2);
            }
        }
        
        private Boolean IsoEquidistantCheck ( IList<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>> curve )
        { 
            var max = curve.Max(d => d.Item2);
            
            for ( var i = 1; i < curve.Count-1; i++)
            {
                if (curve[i].Item2 != max)
                    return curve[i].Item2 - curve[i-1].Item2 == curve[i+1].Item2 - curve[i].Item2;
            }
            
            return true;
        }        
		
		[Test]
        public void TestTupleCalculationOperation()
        {
            var curve = Curve;

            var curve2 = curve.Aggregate((p, c) => Tuple.Create(p.Item1, p.Item2 - c.Item2));
            var curve3 = curve.Aggregate((p, c) => Tuple.Create(p.Item1, p.Item2 - c.Item2));
            var curve4 = curve.Aggregate((p, c) => Tuple.Create(p.Item1, p.Item2 - c.Item2));
            var curve5 = curve.Aggregate((p, c) => Tuple.Create(p.Item1, p.Item2 - c.Item2));

        }
    }
}
