using System;

namespace YFramework.Math
{
    public struct Fixed64Vector3 : IEquatable<Fixed64Vector3>
    {
        public Fixed64 x;
        public Fixed64 y;
        public Fixed64 z;

        public Fixed64Vector3(Fixed64 x, Fixed64 y, Fixed64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Fixed64 this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else if (index == 1)
                    return y;
                else
                    return z;
            }
            set
            {
                if (index == 0)
                    x = value;
                else if (index == 1)
                    y = value;
                else
                    y = value;
            }
        }

        public static Fixed64Vector3 Zero
        {
            get { return new Fixed64Vector3(Fixed64.Zero, Fixed64.Zero, Fixed64.Zero); }
        }

        public static Fixed64Vector3 operator +(Fixed64Vector3 a, Fixed64Vector3 b)
        {
            Fixed64 x = a.x + b.x;
            Fixed64 y = a.y + b.y;
            Fixed64 z = a.z + b.z;
            return new Fixed64Vector3(x, y, z);
        }

        public static Fixed64Vector3 operator -(Fixed64Vector3 a, Fixed64Vector3 b)
        {
            Fixed64 x = a.x - b.x;
            Fixed64 y = a.y - b.y;
            Fixed64 z = a.z - b.z;
            return new Fixed64Vector3(x, y, z);
        }

        public static Fixed64Vector3 operator *(Fixed64 d, Fixed64Vector3 a)
        {
            Fixed64 x = a.x * d;
            Fixed64 y = a.y * d;
            Fixed64 z = a.z * d;
            return new Fixed64Vector3(x, y, z);
        }

        public static Fixed64Vector3 operator *(Fixed64Vector3 a, Fixed64 d)
        {
            Fixed64 x = a.x * d;
            Fixed64 y = a.y * d;
            Fixed64 z = a.z * d;
            return new Fixed64Vector3(x, y, z);
        }

        public static Fixed64Vector3 operator /(Fixed64Vector3 a, Fixed64 d)
        {
            Fixed64 x = a.x / d;
            Fixed64 y = a.y / d;
            Fixed64 z = a.z / d;
            return new Fixed64Vector3(x, y, z);
        }

        public static bool operator ==(Fixed64Vector3 lhs, Fixed64Vector3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(Fixed64Vector3 lhs, Fixed64Vector3 rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        public override bool Equals(object obj)
        {
            return ((Fixed64Vector3) obj) == this;
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", x, y, z);
        }

        public bool Equals(Fixed64Vector3 other)
        {
            return this == other;
        }
    }
}