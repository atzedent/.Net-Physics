using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static class OrderOfMagnitude
    {
        /// <summary>
        /// (Y) 1E+24
        /// </summary>
        public static Magnitude Yotta { get { return new Magnitude("Y", 1E+24); } }
        /// <summary>
        /// (Z) 1E+21
        /// </summary>
        public static Magnitude Zetta { get { return new Magnitude("Z", 1E+21); } }
        /// <summary>
        /// (E) 1E+18
        /// </summary>
        public static Magnitude Exa { get { return new Magnitude("E", 1E+18); } }
        /// <summary>
        /// (P) 1E+15
        /// </summary>
        public static Magnitude Peta { get { return new Magnitude("P", 1E+15); } }
        /// <summary>
        /// (T) 1E+12
        /// </summary>
        public static Magnitude Tera { get { return new Magnitude("T", 1E+12); } }
        /// <summary>
        /// (G) 1E+9
        /// </summary>
        public static Magnitude Giga { get { return new Magnitude("G", 1E+9); } }
        /// <summary>
        /// (M) 1E+6
        /// </summary>
        public static Magnitude Mega { get { return new Magnitude("M", 1E+6); } }
        /// <summary>
        /// (k) 1E+3
        /// </summary>
        public static Magnitude Kilo { get { return new Magnitude("k", 1E+3); } }
        /// <summary>
        /// (h) 1E+2
        /// </summary>
        public static Magnitude Hecto { get { return new Magnitude("h", 1E+2); } }
        /// <summary>
        /// (da) 1E+1
        /// </summary>
        public static Magnitude Deca { get { return new Magnitude("da", 1E+1); } }
        /// <summary>
        /// (d) 1E-1
        /// </summary>
        public static Magnitude Deci { get { return new Magnitude("d", 1E-1); } }
        /// <summary>
        /// (c) 1E-2
        /// </summary>
        public static Magnitude Centi { get { return new Magnitude("c", 1E-2); } }
        /// <summary>
        /// (m) 1E-3
        /// </summary>
        public static Magnitude Milli { get { return new Magnitude("m", 1E-3); } }
        /// <summary>
        /// (µ) 1E-6
        /// </summary>
        public static Magnitude Micro { get { return new Magnitude("µ", 1E-6); } }
        /// <summary>
        /// (n) 1E-9
        /// </summary>
        public static Magnitude Nano { get { return new Magnitude("n", 1E-9); } }
        /// <summary>
        /// (p) 1E-12
        /// </summary>
        public static Magnitude Pico { get { return new Magnitude("p", 1E-12); } }
        /// <summary>
        /// (f) 1E-15
        /// </summary>
        public static Magnitude Femto { get { return new Magnitude("f", 1E-15); } }
        /// <summary>
        /// (a) 1E-18
        /// </summary>
        public static Magnitude Atto { get { return new Magnitude("a", 1E-18); } }
        /// <summary>
        /// (z) 1E-21
        /// </summary>
        public static Magnitude Zepto { get { return new Magnitude("z", 1E-21); } }
        /// <summary>
        /// (y) 1E-24
        /// </summary>
        public static Magnitude Yokto { get { return new Magnitude("y", 1E-24); } }
    }

    public struct Magnitude
    {
        public Double Value { get; set; }
        public String Prefix { get; set; }

        public Magnitude(String prefix, Double @value)
            : this()
        {
            Prefix = prefix;
            Value = @value;
        }
    }
}
