using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AdapterDecorator
{
    public class MyStringBuilder
    {
        StringBuilder _sb;

        //=============================================

        public static implicit operator MyStringBuilder(string s)
        {
            var msb = new MyStringBuilder();
            msb._sb.Append(s);
            return msb;
        }

        public static MyStringBuilder operator +(MyStringBuilder msb, string s)
        {
            msb.Append(s);
            return msb;
        }
        public MyStringBuilder()
        {
            _sb = new StringBuilder();
        }

        public MyStringBuilder(string s)
        {
            _sb = new StringBuilder(s);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        //=============================================

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_sb).GetObjectData(info, context);
        }

        public int EnsureCapacity(int capacity)
        {
            return _sb.EnsureCapacity(capacity);
        }

        public string ToString(int startIndex, int length)
        {
            return _sb.ToString(startIndex, length);
        }

        public StringBuilder Clear()
        {
            return _sb.Clear();
        }

        public StringBuilder Append(char value, int repeatCount)
        {
            return _sb.Append(value, repeatCount);
        }

        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            return _sb.Append(value, startIndex, charCount);
        }

        public StringBuilder Append(string value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(string value, int startIndex, int count)
        {
            return _sb.Append(value, startIndex, count);
        }

        public StringBuilder AppendLine()
        {
            return _sb.AppendLine();
        }

        public StringBuilder AppendLine(string value)
        {
            return _sb.AppendLine(value);
        }

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _sb.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public StringBuilder Insert(int index, string value, int count)
        {
            return _sb.Insert(index, value, count);
        }

        public StringBuilder Remove(int startIndex, int length)
        {
            return _sb.Remove(startIndex, length);
        }

        public StringBuilder Append(bool value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(sbyte value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(byte value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(char value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(short value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(int value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(long value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(float value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(double value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(decimal value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(ushort value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(uint value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(ulong value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(object value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Append(char[] value)
        {
            return _sb.Append(value);
        }

        public StringBuilder Insert(int index, string value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, bool value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, sbyte value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, byte value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, short value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, char value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, char[] value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            return _sb.Insert(index, value, startIndex, charCount);
        }

        public StringBuilder Insert(int index, int value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, long value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, float value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, double value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, decimal value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, ushort value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, uint value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, ulong value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder Insert(int index, object value)
        {
            return _sb.Insert(index, value);
        }

        public StringBuilder AppendFormat(string format, object arg0)
        {
            return _sb.AppendFormat(format, arg0);
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            return _sb.AppendFormat(format, arg0, arg1);
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            return _sb.AppendFormat(format, arg0, arg1, arg2);
        }

        public StringBuilder AppendFormat(string format, params object[] args)
        {
            return _sb.AppendFormat(format, args);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            return _sb.AppendFormat(provider, format, arg0);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
          object arg1)
        {
            return _sb.AppendFormat(provider, format, arg0, arg1);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
          object arg1, object arg2)
        {
            return _sb.AppendFormat(provider, format, arg0, arg1, arg2);
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            return _sb.AppendFormat(provider, format, args);
        }

        public StringBuilder Replace(string oldValue, string newValue)
        {
            return _sb.Replace(oldValue, newValue);
        }

        public bool Equals(StringBuilder sb)
        {
            return this._sb.Equals(sb);
        }

        public StringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            return _sb.Replace(oldValue, newValue, startIndex, count);
        }

        public StringBuilder Replace(char oldChar, char newChar)
        {
            return _sb.Replace(oldChar, newChar);
        }

        public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            return _sb.Replace(oldChar, newChar, startIndex, count);
        }

        public int Capacity
        {
            get => _sb.Capacity;
            set => _sb.Capacity = value;
        }

        public int MaxCapacity => _sb.MaxCapacity;

        public int Length
        {
            get => _sb.Length;
            set => _sb.Length = value;
        }

        public char this[int index]
        {
            get => _sb[index];
            set => _sb[index] = value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyStringBuilder s = new MyStringBuilder("hello");
            s = s + "world";
            WriteLine(s);
        }
    }
}
