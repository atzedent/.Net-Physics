using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace IMaverick.Physics
{
    [Serializable]
    [XmlType(Namespace = "", TypeName = "IMaverick.Physics.PhysicalValue`1")]
    [XmlRoot(Namespace = "", ElementName = "PhysicalValue", DataType = "string", IsNullable = false)]
    public struct PhysicalValue<TDimension>
        : IEquatable<PhysicalValue<TDimension>>
        , IComparable<PhysicalValue<TDimension>>
        , ISerializable where TDimension : Dimension<TDimension>, new()
    {
        private TDimension m_unit;

        [XmlIgnore()]
        public TDimension Unit
        {
            get
            {
                if (m_unit == null)
                {
                    m_unit = new TDimension();
                }
                return m_unit;
            }
            set { m_unit = value; }
        }

        [XmlAttribute(DataType = "string", AttributeName = "unit")]
        public String UnitString { get { return Unit.Unit.Name; } set { Unit = Dimension<TDimension>.FromString(value); } }

        [XmlAttribute(DataType = "double", AttributeName = "value")]
        public Double Value { get; set; }

        public PhysicalValue(Double value, TDimension unit)
            : this()
        {
            Value = value;
            Unit = unit;
        }

        public PhysicalValue(Double value, String unit ) : this ( value, Dimension<TDimension>.FromString(unit) )
        {

        }

        public bool Equals(PhysicalValue<TDimension> rhs)
        {
            return this == rhs;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is PhysicalValue<TDimension>))
            {
                return false;
            }

            return Equals((PhysicalValue<TDimension>)obj);
        }

        public int CompareTo(PhysicalValue<TDimension> rhs)
        {
            return (int)Math.Round((this - rhs).Value, 0);
        }

        public bool IsSatisfied(Specification<PhysicalValue<TDimension>> specification)
        {
            return specification.IsSatisfiedBy(this);
        }

        public bool EqualsApproximately(PhysicalValue<TDimension> expected, PhysicalValue<TDimension> tolerance)
        {
            if (double.IsNaN(expected.Value))
            {
                return double.IsNaN(Value);
            }

            if (!double.IsInfinity(expected.Value))
            {
                return Math.Abs(Value - expected.ScaleTo(Unit).Value) <= tolerance.ToDefault().Value;
            }

            return expected.Value.Equals(Value);
        }

        public PhysicalValue<TDimension> ScaleTo(TDimension unit)
        {
            if (Unit == unit) return this;

            return new PhysicalValue<TDimension>(unit.ScaleTo(ToDefault().Value), unit);
        }

        public PhysicalValue<TDimension> ToDefault()
        {
            return new PhysicalValue<TDimension>(Unit.ToDefault(Value), new TDimension());
        }

        public override int GetHashCode()
        {
            return unchecked(Value.GetHashCode() * 83 + Unit.GetHashCode());
        }

        public static Boolean operator ==(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            if (lhs.Unit == rhs.Unit) return lhs.Value == rhs.Value;

            return lhs.Value == rhs.ScaleTo(lhs.Unit).Value;
        }

        public static Boolean operator !=(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            return !(lhs == rhs);
        }

        public static Boolean operator >(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            if (lhs.Unit == rhs.Unit) return lhs.Value > rhs.Value;

            return lhs.Value > rhs.ScaleTo(lhs.Unit).Value;
        }

        public static Boolean operator <(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            if (lhs.Unit == rhs.Unit) return lhs.Value < rhs.Value;

            return lhs.Value < rhs.ScaleTo(lhs.Unit).Value;
        }

        public static Boolean operator >=(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            return lhs > rhs || lhs == rhs;
        }

        public static Boolean operator <=(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            return lhs < rhs || lhs == rhs;
        }

        public static PhysicalValue<TDimension> operator *(PhysicalValue<TDimension> lhs, Double rhs)
        {
            return new PhysicalValue<TDimension>(Decimal.ToDouble(Decimal.Multiply(new Decimal(lhs.Value), new Decimal(rhs))), lhs.Unit);
        }

        public static PhysicalValue<TDimension> operator /(PhysicalValue<TDimension> lhs, Double rhs)
        {
            return new PhysicalValue<TDimension>(Decimal.ToDouble(Decimal.Divide(new Decimal(lhs.Value), new Decimal(rhs))), lhs.Unit);
        }

        public static PhysicalValue<TDimension> operator %(PhysicalValue<TDimension> lhs, Double rhs)
        {
            return new PhysicalValue<TDimension>(Decimal.ToDouble(Decimal.Remainder(new Decimal(lhs.Value), new Decimal(rhs))), lhs.Unit);
        }

        public static PhysicalValue<TDimension> operator +(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            return new PhysicalValue<TDimension>(Decimal.ToDouble(Decimal.Add(new Decimal(lhs.Value), new Decimal(rhs.ScaleTo(lhs.Unit).Value))), lhs.Unit);
        }

        public static PhysicalValue<TDimension> operator -(PhysicalValue<TDimension> lhs, PhysicalValue<TDimension> rhs)
        {
            return new PhysicalValue<TDimension>(Decimal.ToDouble(Decimal.Subtract(new Decimal(lhs.Value), new Decimal(rhs.ScaleTo(lhs.Unit).Value))), lhs.Unit);
        }

        public static PhysicalValue<TDimension> operator -(PhysicalValue<TDimension> lhs)
        {
            return new PhysicalValue<TDimension>(-lhs.Value, lhs.Unit);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", Value, Unit);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
            info.AddValue("Unit", Unit.ToString());
        }
                
        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
        private PhysicalValue (SerializationInfo info, StreamingContext context) : this ( )
        {
            Value = info.GetDouble("Value");
            try 
            {
                Unit = Dimension<TDimension>.FromString ( info.GetString("Unit") );
            }
            catch (SerializationException) 
            {
                Unit = new TDimension();
            }
        }
    }
}
