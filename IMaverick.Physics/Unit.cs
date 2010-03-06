using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IMaverick.Physics
{
    public struct Unit
    {
        public Double Offset { get; set; }
        public Decimal Factor { get; set; }
        public String Name { get; set; }

        public override String ToString()
        {
            return Name;
        }

        public override Int32 GetHashCode()
        {
            return unchecked(Name.GetHashCode() * 83);
        }

        public override Boolean Equals(object obj)
        {
            if (obj is Unit)
            {
                var rhs = (Unit)obj;

                return this.Name == rhs.Name && this.Factor == rhs.Factor && this.Offset == rhs.Offset;
            }
            return false;
        }
    }
}
