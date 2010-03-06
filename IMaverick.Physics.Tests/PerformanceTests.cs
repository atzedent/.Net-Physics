using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IMaverick.Physics.Extensions;
using IMaverick.Gallio.Extensions;

namespace IMaverick.Physics.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void TestCreationOfSequences()
        {
            var incemented = Sequence.AddRange(0, 100, Length.Meter);
            var decremented = Sequence.AddRange(100, 0, Length.Meter);

            Assert.AreElementsEqual(incemented, decremented.Reverse());
        }

        [Test]
        public void TestListCreationEquality()
        {
            var bartsValues = ParallelEnumerable.Range(1, 1000000).AsOrdered().Select(p => ((double)p * 10).cm());
            Func<IEnumerable<PhysicalValue<Length>>> creator = () => Sequence.AddRange(10d.cm(), 1E+2.km(), 10d.cm());
            var myValues = creator().ToList();

            Assert.AreElementsEqual(myValues, bartsValues.AsSequential());
        }

        [Test]
        public void TestSequenceCreationExtension()
        {
            var myValues = Sequence.AddRange(1, 10000000, Length.Centimeter, 10);

            var count = myValues.Count();
            Assert.AreEqual(10000000, count);

            TestLog.Write("Number of elements created:");
            TestLog.WriteHighlighted(string.Format("{0:N}",count));
        }

        [Test]
        public void TestCreationOfSequenceOfInts()
        {
            var values = ParallelEnumerable.Range(0, 10000000).AsOrdered().Select(p => ((double)p).cm());

            TestLog.Write("Number of elements created:");
            TestLog.WriteHighlighted(string.Format("{0:N}", values.Count()));
        }

        [Test]
        public void TestConstructionOfPhysicalValues()
        {
            Func<IEnumerable<PhysicalValue<Length>>> creator = () => Sequence.AddRange(1, 10000000, Length.Centimeter, 10);
            var values = creator().ToList();

            Assert.AreEqual(1E+7, values.Count);

            TestLog.Write("Number of elements created:");
            TestLog.WriteHighlighted(string.Format("{0:N}", values.Count));
        }

        [Test]
        public void TestConstructionOfTuplesOfPhysicalValues()
        {
            var yValues = Sequence.AddRange(1, 5000000, Length.Centimeter);
            var x = default(double);
            var curve = from y in yValues select Tuple.Create(x++.s(), y);

            Assert.AreEqual(5000000, curve.Count());

            TestLog.Write("Number of curve elements created:");
            TestLog.WriteHighlighted(string.Format("{0:N}\n", curve.Count()));

            TestLog.Write("Number of PhysicalValue elements created:");
            TestLog.WriteHighlighted(string.Format("{0:N}", curve.Count() * 2));
        }

        private static List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>> curve;

        [StaticTestFactory]
        public static IEnumerable<Test> TestComarison()
        {
            yield return new TestSuite("Comparison Tests")
            {
                SuiteSetUp = () =>
                {
                    Func<IEnumerable<PhysicalValue<Length>>> listCreator = () => Sequence.AddRange(1, 5000000, Length.Centimeter, 10);
                    Func<IEnumerable<PhysicalValue<Length>>, Double, IEnumerable<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>>> tupleCreator = (values, indexer) => from y in values select Tuple.Create(indexer++.s(), y);
                    curve = tupleCreator(listCreator(), default(double)).ToList();

                    using (TestLog.BeginSection("Construction of curve lements"))
                    { 
                        TestLog.Write("Elements in curve: ");
                        TestLog.WriteHighlighted(string.Format("{0:N}", curve.Count));
                    }
                },
                Children =
                {
                    new TestCase("Test Equidistance of Y Values",
                        () => 
                        {
                            AssertEx.Multiple(() =>
                                {
                                    for (var i = 1; i < curve.Count - 1; i++)
                                    {
                                        Assert.AreEqual(curve[i].Item2 - curve[i - 1].Item2, curve[i + 1].Item2 - curve[i].Item2);
                                    }
                                });
                            TestLog.Write("Compared number of PhysicalValues: ");
                            TestLog.WriteHighlighted(string.Format("{0:N}", (curve.Count - 2) * 3));
                        }),

                    new TestCase("Test Equidistance of Y Values with Spec",
                        () => 
                        {
                            AssertEx.Multiple(() =>
                                {
                                    Func<List<Tuple<PhysicalValue<Time>, PhysicalValue<Length>>>, bool> equiDistFunc = p =>
                                    {
                                        for (var i = 1; i < p.Count - 1; i++)
                                        {
                                            if (p[i].Item2 - p[i - 1].Item2 != p[i + 1].Item2 - p[i].Item2) return false;
                                        } return true;
                                    };

                                    AssertEx.That(() => curve.IsSatisfied(curve.CreateSpecification( p => equiDistFunc(p.ToList()) )));
                                });
                            TestLog.Write("Compared number of PhysicalValues: ");
                            TestLog.WriteHighlighted(string.Format("{0:N}", (curve.Count - 2) * 3));
                        })
                }
            };
        }
    }
}
