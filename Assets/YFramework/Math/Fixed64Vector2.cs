using System;

namespace YFramework.Math
{
    public class Fixed64Vector2 : IEquatable<Fixed64Vector2>
    {
        public Fixed64 x;
        public Fixed64 y;
        public Fixed64Vector2(Fixed64 x,Fixed64 y)
        {
            this.x = x;
            this.y = y;
        }
        public Fixed64 this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else 
                    return y;
            }
            set
            {
                if (index == 0)
                    x=value;
                else
                    y=value;
            }
        }
        public static Fixed64Vector2 Zero
        {
            get
            {
                return new Fixed64Vector2(Fixed64.Zero,Fixed64.Zero);
            }
        }

        public static Fixed64Vector2 operator +(Fixed64Vector2 a, Fixed64Vector2 b)
        {
            Fixed64 x=a.x+b.x;
            Fixed64 y=a.y+b.y;
            return new Fixed64Vector2(x,y);
        }
        public static Fixed64Vector2 operator -(Fixed64Vector2 a, Fixed64Vector2 b)
        {
            Fixed64 x = a.x - b.x;
            Fixed64 y = a.y - b.y;
            return new Fixed64Vector2(x, y);
        }
        public static Fixed64Vector2 operator *(Fixed64 a, Fixed64Vector2 b)
        {
            Fixed64 x = a * b.x;
            Fixed64 y = a * b.y;
            return new Fixed64Vector2(x, y);
        }
        public static Fixed64Vector2 operator *(Fixed64Vector2 a, Fixed64 b)
        {
            Fixed64 x = a.x * b;
            Fixed64 y = a.y * b;
            return new Fixed64Vector2(x, y);
        }
        public static Fixed64Vector2 operator /(Fixed64Vector2 a, Fixed64 b)
        {
            Fixed64 x = a.x / b;
            Fixed64 y = a.y / b;
            return new Fixed64Vector2(x, y);
        }
        public static bool operator ==(Fixed64Vector2 a, Fixed64Vector2 b)
        {
            return a.x==b.x&& a.y==b.y;
        }
        public static bool operator !=(Fixed64Vector2 a, Fixed64Vector2 b)
        {
            return !(a==b);
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode()>>2);
        }
        public override bool Equals(object obj)
        {
            if (obj is Fixed64Vector2)
            {
                return (Fixed64Vector2)obj == this;
            }
            else
            {
                return false;
            }
        }
        public override string ToString()
        {
            return string.Format("x:{0},y:{1}", x, y);
        }
        public bool Equals(Fixed64Vector2 other)
        {
            return other == this;
        }

    }
}