using System;
using System.Collections.Generic;
using System.Text;

namespace YFramework.Network.Protocol
{
    public static class ProtoWireCodec
    {
        public const int VarintWireType = 0;
        public const int Fixed64WireType = 1;
        public const int LengthDelimitedWireType = 2;
        public const int Fixed32WireType = 5;

        public delegate void FieldReader(int fieldNumber, int wireType, byte[] data, ref int index);

        public static void ReadFields(byte[] payload, FieldReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            byte[] data = payload ?? Array.Empty<byte>();
            int index = 0;
            while (index < data.Length)
            {
                ulong tag = ReadVarint(data, ref index);
                if (tag == 0)
                {
                    break;
                }

                int fieldNumber = (int)(tag >> 3);
                int wireType = (int)(tag & 0x07);
                reader(fieldNumber, wireType, data, ref index);
            }
        }

        public static void WriteRequiredString(List<byte> buffer, int fieldNumber, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Field {fieldNumber} is required.");
            }

            WriteString(buffer, fieldNumber, value);
        }

        public static void WriteString(List<byte> buffer, int fieldNumber, string value)
        {
            if (buffer == null || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteTag(buffer, fieldNumber, LengthDelimitedWireType);
            WriteVarint(buffer, (ulong)bytes.Length);
            buffer.AddRange(bytes);
        }

        public static string ReadString(byte[] data, ref int index, int wireType)
        {
            EnsureWireType(LengthDelimitedWireType, wireType);
            ulong length = ReadVarint(data, ref index);
            int byteLength = checked((int)length);
            EnsureReadable(data, index, byteLength);
            string value = Encoding.UTF8.GetString(data, index, byteLength);
            index += byteLength;
            return value;
        }

        public static void WriteInt32(List<byte> buffer, int fieldNumber, int value)
        {
            if (buffer == null)
            {
                return;
            }

            WriteTag(buffer, fieldNumber, VarintWireType);
            WriteVarint(buffer, unchecked((ulong)(long)value));
        }

        public static void WriteNullableInt32(List<byte> buffer, int fieldNumber, int? value)
        {
            if (value.HasValue)
            {
                WriteInt32(buffer, fieldNumber, value.Value);
            }
        }

        public static void WriteNullableEnum<TEnum>(List<byte> buffer, int fieldNumber, TEnum? value) where TEnum : struct, Enum
        {
            if (value.HasValue)
            {
                WriteInt32(buffer, fieldNumber, Convert.ToInt32(value.Value));
            }
        }

        public static void WriteInt32IfPositive(List<byte> buffer, int fieldNumber, int value)
        {
            if (value > 0)
            {
                WriteInt32(buffer, fieldNumber, value);
            }
        }

        public static int ReadInt32(byte[] data, ref int index, int wireType)
        {
            EnsureWireType(VarintWireType, wireType);
            return unchecked((int)ReadVarint(data, ref index));
        }

        public static void WriteInt64(List<byte> buffer, int fieldNumber, long value)
        {
            if (buffer == null)
            {
                return;
            }

            WriteTag(buffer, fieldNumber, VarintWireType);
            WriteVarint(buffer, unchecked((ulong)value));
        }

        public static void WriteInt64IfPositive(List<byte> buffer, int fieldNumber, long value)
        {
            if (value > 0)
            {
                WriteInt64(buffer, fieldNumber, value);
            }
        }

        public static long ReadInt64(byte[] data, ref int index, int wireType)
        {
            EnsureWireType(VarintWireType, wireType);
            return unchecked((long)ReadVarint(data, ref index));
        }

        public static void WriteMessage(List<byte> buffer, int fieldNumber, byte[] payload)
        {
            if (buffer == null || payload == null || payload.Length == 0)
            {
                return;
            }

            WriteTag(buffer, fieldNumber, LengthDelimitedWireType);
            WriteVarint(buffer, (ulong)payload.Length);
            buffer.AddRange(payload);
        }

        public static byte[] ReadMessage(byte[] data, ref int index, int wireType)
        {
            EnsureWireType(LengthDelimitedWireType, wireType);
            ulong length = ReadVarint(data, ref index);
            int byteLength = checked((int)length);
            EnsureReadable(data, index, byteLength);
            byte[] payload = new byte[byteLength];
            Buffer.BlockCopy(data, index, payload, 0, byteLength);
            index += byteLength;
            return payload;
        }

        public static void SkipField(byte[] data, ref int index, int wireType)
        {
            switch (wireType)
            {
                case VarintWireType:
                    ReadVarint(data, ref index);
                    break;
                case Fixed64WireType:
                    EnsureReadable(data, index, 8);
                    index += 8;
                    break;
                case LengthDelimitedWireType:
                    ulong length = ReadVarint(data, ref index);
                    int skipLength = checked((int)length);
                    EnsureReadable(data, index, skipLength);
                    index += skipLength;
                    break;
                case Fixed32WireType:
                    EnsureReadable(data, index, 4);
                    index += 4;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported protobuf wire type: {wireType}");
            }
        }

        public static void WriteTag(List<byte> buffer, int fieldNumber, int wireType)
        {
            WriteVarint(buffer, (ulong)((fieldNumber << 3) | wireType));
        }

        public static void WriteVarint(List<byte> buffer, ulong value)
        {
            if (buffer == null)
            {
                return;
            }

            while (value >= 0x80)
            {
                buffer.Add((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }

            buffer.Add((byte)value);
        }

        public static ulong ReadVarint(byte[] data, ref int index)
        {
            ulong value = 0;
            int shift = 0;
            while (true)
            {
                EnsureReadable(data, index, 1);
                byte current = data[index++];
                value |= (ulong)(current & 0x7F) << shift;
                if ((current & 0x80) == 0)
                {
                    return value;
                }

                shift += 7;
                if (shift > 63)
                {
                    throw new InvalidOperationException("Invalid protobuf varint payload.");
                }
            }
        }

        public static void EnsureWireType(int expected, int actual)
        {
            if (expected != actual)
            {
                throw new InvalidOperationException($"Unexpected protobuf wire type. Expected {expected}, got {actual}.");
            }
        }

        public static void EnsureReadable(byte[] data, int index, int count)
        {
            if (data == null || index < 0 || count < 0 || index + count > data.Length)
            {
                throw new InvalidOperationException("Protobuf payload ended unexpectedly.");
            }
        }
    }
}
