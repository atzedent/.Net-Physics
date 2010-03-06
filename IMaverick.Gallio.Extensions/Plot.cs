using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Gallio.Framework;
using MbUnit.Framework;
using IMaverick.Physics;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;

namespace IMaverick.Gallio.Extensions
{
    public static class TestLogExtensions
    {
        public static void Plot<TXDimension, TYDimension>(this IEnumerable<Tuple<PhysicalValue<TXDimension>, PhysicalValue<TYDimension>>> curve, TXDimension xUnit, TYDimension yUnit, string name = "Plot", string description = "")
            where TXDimension : Dimension<TXDimension>, new()
            where TYDimension : Dimension<TYDimension>, new()
        {
            Contract.Ensures(curve != null, "curve must not be null");
            Contract.Ensures(curve.Count() != 0, "curve must contain at least one element");

            var plotter = CreatePlot();
            var source = curve.AsDataSource();

            source.SetXMapping(x => x.Item1.ScaleTo(xUnit).Value);
            source.SetYMapping(y => y.Item2.ScaleTo(yUnit).Value);

            var desc = FormulateDescription(description, xUnit, yUnit);

            Plot(source, name, desc);
        }

        public static void Plot<TXDimension, TYDimension>(this IEnumerable<Tuple<PhysicalValue<TXDimension>, PhysicalValue<TYDimension>>> curve, string name = "Plot", string description = "")
            where TXDimension : Dimension<TXDimension>, new()
            where TYDimension : Dimension<TYDimension>, new()
        {
            Contract.Ensures(curve != null, "curve must not be null");
            Contract.Ensures(curve.Count() != 0, "curve must contain at least one element");

            var plotter = CreatePlot();
            var source = curve.AsDataSource();

            source.SetXMapping(x => x.Item1.Value);
            source.SetYMapping(y => y.Item2.Value);

            var desc = FormulateDescription(description, curve.FirstOrDefault().Item1.Unit, curve.FirstOrDefault().Item2.Unit);

            Plot(source, name, desc);
        }

        public static void Plot<TXDimension, TYDimension>(this IEnumerable<Tuple<Double, Double>> curve, string name = "Plot", string description = "Line")
            where TXDimension : Dimension<TXDimension>, new()
            where TYDimension : Dimension<TYDimension>, new()
        {
            Contract.Ensures(curve != null, "curve must not be null");
            Contract.Ensures(curve.Count() != 0, "curve must contain at least one element");

            var source = curve.AsDataSource();

            source.SetXMapping(x => x.Item1);
            source.SetYMapping(y => y.Item2);

            Plot(source, name, description);
        }

        private static void Plot<T>(EnumerableDataSource<T> source, string plotname, string description)
        {
            Contract.Ensures(source != null, "source must not be null");

            var plotter = CreatePlot();
            plotter.AddLineGraph(source, description);

            var filename = Path.ChangeExtension(plotname, ".png");
            plotter.SaveScreenshot(filename);
            EmbedPlotInReport(filename, plotname);
        }

        private static string FormulateDescription<TXDimension, TYDimension>(string description, TXDimension xUnit, TYDimension yUnit)
            where TXDimension : Dimension<TXDimension>, new()
            where TYDimension : Dimension<TYDimension>, new()
        {
            return string.IsNullOrEmpty(description) ? string.Format("{0} over {1}", yUnit, xUnit) : description;
        }

        private static ChartPlotter CreatePlot()
        {
            var plotter = new ChartPlotter { Height = 566, Width = 800 };
            plotter.Legend.LegendLeft = 10;
            plotter.Legend.LegendRight = double.NaN;

            return plotter;
        }

        private static void EmbedPlotInReport(string filename, string name = "Plot")
        {
            Contract.Ensures(!string.IsNullOrEmpty(filename), "filename must not be null or empty");

            var image = System.Drawing.Image.FromFile(filename);

            using (TestLog.BeginSection(name))
                TestLog.EmbedImage(name, image);
        }
    }
}
