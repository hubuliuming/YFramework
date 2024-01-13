using System;
using System.Collections;
using System.Collections.Generic;
using MirrorOfWitch;
using UnityEngine;
using UnityEngine.Profiling;
using Convert = YFramework.Kit.Convert.Convert;

public class Test1 : MonoBehaviour
{
    private GameObject go;
    private void Start()
    {
        //IEEE754();
        BitConverter.ToString(YFramework.Kit.Convert.Convert.Convert16Byte("33"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
        }
    }

    private void IEEE754()
    {
        float v = 3.25f;
        var bytes = BitConverter.GetBytes(v);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        var s = BitConverter.ToString(bytes);
        Debug.Log(s);
    }
}


/// <summary>
/// 64位定点数
/// </summary>
[Serializable]
public struct Fixed64 : IEquatable<Fixed64>,IComparable,IComparable<Fixed64>
{
    public long value;
    /// <summary>
    /// 小数部分
    /// </summary>
    private const int FRACTIONLITS = 12;

    private const long ONE = 1L << FRACTIONLITS;
    public static Fixed64 Zero = new Fixed64(0);

    Fixed64(long v)
    {
        this.value = v;
    }
    public Fixed64(int value)
    {
        this.value = ONE * value;
    }

    #region 重载操作运算符

    public static Fixed64 operator +(Fixed64 a, Fixed64 b) => new Fixed64(a.value + b.value);
    public static Fixed64 operator -(Fixed64 a, Fixed64 b) => new Fixed64(a.value - b.value);
    public static Fixed64 operator *(Fixed64 a, Fixed64 b) => new Fixed64((a.value * b.value) >> FRACTIONLITS); //乘法会多出一倍的小数点
    public static Fixed64 operator /(Fixed64 a, Fixed64 b) => new Fixed64((a.value << FRACTIONLITS) / b.value);
    public static bool operator ==(Fixed64 a, Fixed64 b) => a.value == b.value;
    public static bool operator !=(Fixed64 a, Fixed64 b) =>!(a == b);
    public static bool operator >(Fixed64 a, Fixed64 b) => a.value > b.value;
    public static bool operator <(Fixed64 a, Fixed64 b) => a.value < b.value;
    public static bool operator >=(Fixed64 a, Fixed64 b) => a.value >= b.value;
    public static bool operator <=(Fixed64 a, Fixed64 b) => a.value <= b.value;
    
    #endregion

    #region 强制转换重载操作

    public static explicit operator long(Fixed64 value) => value.value >> FRACTIONLITS;
    public static explicit operator Fixed64(long value) => new Fixed64(value);
    public static explicit operator float(Fixed64 value) => (float) value / ONE;
    public static explicit operator Fixed64(float value) => new Fixed64((long) value * ONE);

    #endregion

    public bool Equals(Fixed64 other)
    {
        return other.value == value;
    }

    public int CompareTo(object obj)
    {
        return value.CompareTo(obj);
    }

    public int CompareTo(Fixed64 other)
    {
        return value.CompareTo(other.value);
    }

    public override bool Equals(object obj)
    {
        return obj is Fixed64 && obj.Equals(obj);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return ((float)this).ToString();
    }
}