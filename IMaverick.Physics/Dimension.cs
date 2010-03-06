using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;
using IMaverick.Physics.Extensions;

namespace IMaverick.Physics
{
    public abstract class Dimension<TDimension> where TDimension : Dimension<TDimension>, new()
    {
        private static readonly UnitCollection m_units = new UnitCollection();
        protected static UnitCollection Units { get { return m_units; } }
        protected static String BaseUnitSymbol { get; set; }

        protected Dimension()
            : this(Units[BaseUnitSymbol])
        {
        }

        protected Dimension(String scale)
            : this(Units[scale])
        {
        }

        protected Dimension(Unit scale)
        {
            Unit = scale;
        }

        protected static void InitializeOrdersOfMagnitude(String baseUnitSymbol)
        {
            BaseUnitSymbol = baseUnitSymbol;

            m_units.Add(new Unit { Factor = 1, Name = baseUnitSymbol });

            foreach (var property in typeof(OrderOfMagnitude).GetProperties())
            {
                var prop = (Magnitude)property.GetValue(null, null);
                m_units.Add(new Unit { Factor = new Decimal(prop.Value), Name = prop.Prefix + baseUnitSymbol });
            }

        }

        protected static TDimension GetNewInstanceFor(Magnitude magnitude)
        {
            return new TDimension { Unit = m_units[magnitude.Prefix + BaseUnitSymbol] };
        }

        public virtual Double ToDefault(Double value)
        {
            if (Unit.Name.Equals(BaseUnitSymbol)) return value;

            return Decimal.ToDouble(Decimal.Multiply(Decimal.Add(new Decimal(value), new Decimal(Unit.Offset)), Unit.Factor));
        }

        public virtual Double ScaleTo(Double value)
        {
            return Decimal.ToDouble(Decimal.Subtract((Decimal.Divide(new Decimal(value), Unit.Factor)), new Decimal(Unit.Offset)));
        }

        public Unit Unit { get; set; }

        public static TDimension FromString(string unit)
        {
            return new TDimension { Unit = m_units[unit] };
        }

        public override string ToString()
        {
            return Unit.Name;
        }

        public override int GetHashCode()
        {
            return unchecked((Unit.Factor.GetHashCode() * 83 + Unit.Offset.GetHashCode()) + Unit.Name.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is Dimension<TDimension>)
            {
                var rhs = (Dimension<TDimension>)obj;

                return this.Unit.Equals(rhs.Unit);
            }
            return false;
        }

        protected class UnitCollection : KeyedCollection<string, Unit>
        {
            protected override string GetKeyForItem(Unit item)
            {
                return item.Name;
            }
        }
    }
}
