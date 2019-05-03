using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EasyBehaviorTree
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BBValue : IBlackBoardData
    {
        [FieldOffset(0)]
        public float floatValue;
        public static implicit operator float(BBValue value)
        {
            return value.floatValue;
        }
        public static implicit operator BBValue(float value)
        {
            return new BBValue
            {
                floatValue = value
            };
        }

        [FieldOffset(0)]
        public int intValue;
        public static implicit operator int(BBValue value)
        {
            return value.intValue;
        }
        public static implicit operator BBValue(int value)
        {
            return new BBValue
            {
                intValue = value
            };
        }

        [FieldOffset(0)]
        public double doubleValue;
        public static implicit operator double(BBValue value)
        {
            return value.doubleValue;
        }
        public static implicit operator BBValue(double value)
        {
            return new BBValue
            {
                doubleValue = value
            };
        }

        [FieldOffset(0)]
        public long longValue;
        public static implicit operator long(BBValue value)
        {
            return value.longValue;
        }
        public static implicit operator BBValue(long value)
        {
            return new BBValue
            {
                longValue = value
            };
        }

        [FieldOffset(0)]
        public bool boolValue;
        public static implicit operator bool(BBValue value)
        {
            return value.boolValue;
        }
        public static implicit operator BBValue(bool value)
        {
            return new BBValue
            {
                boolValue = value
            };
        }
    }

    public struct BBString : IBlackBoardData
    {
        public string strValue;

        public override string ToString()
        {
            return strValue;
        }
        public static implicit operator string(BBString value)
        {
            return value.strValue;
        }
        public static implicit operator BBString(string value)
        {
            return new BBString(value);
        }
        public BBString(string value)
        {
            strValue = value;
        }
    }
}
