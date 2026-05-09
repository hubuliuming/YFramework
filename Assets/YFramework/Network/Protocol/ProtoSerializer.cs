using System;
using System.Collections.Generic;

namespace YFramework.Network.Protocol
{

public sealed class ProtoSerializer
{
    public readonly struct ProtoPacket
    {
        public ProtoPacket(int cmd, byte[] body)
        {
            Cmd = cmd;
            Body = CloneBytes(body);
        }

        public int Cmd { get; }
        public byte[] Body { get; }
    }

    private readonly Dictionary<Type, Delegate> m_encoders = new Dictionary<Type, Delegate>();
    private readonly Dictionary<Type, Delegate> m_decoders = new Dictionary<Type, Delegate>();
    private Func<int, byte[], byte[]> m_packetEncoder = DefaultPacketEncoder;
    private Func<byte[], ProtoPacket> m_packetDecoder = DefaultPacketDecoder;

    public void Register<T>(Func<T, byte[]> encoder, Func<byte[], T> decoder)
    {
        if (encoder == null)
        {
            throw new ArgumentNullException(nameof(encoder));
        }

        if (decoder == null)
        {
            throw new ArgumentNullException(nameof(decoder));
        }

        Type messageType = typeof(T);
        m_encoders[messageType] = encoder;
        m_decoders[messageType] = decoder;
    }

    public bool CanSerialize<T>()
    {
        return m_encoders.ContainsKey(typeof(T));
    }

    public bool CanDeserialize<T>()
    {
        return m_decoders.ContainsKey(typeof(T));
    }

    public byte[] Serialize<T>(T message)
    {
        if (!m_encoders.TryGetValue(typeof(T), out Delegate encoderDelegate))
        {
            throw new InvalidOperationException($"ProtoSerializer missing encoder for '{typeof(T).Name}'.");
        }

        return CloneBytes(((Func<T, byte[]>)encoderDelegate).Invoke(message));
    }

    public T Deserialize<T>(byte[] payload)
    {
        if (!m_decoders.TryGetValue(typeof(T), out Delegate decoderDelegate))
        {
            throw new InvalidOperationException($"ProtoSerializer missing decoder for '{typeof(T).Name}'.");
        }

        return ((Func<byte[], T>)decoderDelegate).Invoke(CloneBytes(payload));
    }

    public void RegisterPacketCodec(Func<int, byte[], byte[]> encoder, Func<byte[], ProtoPacket> decoder)
    {
        if (encoder == null)
        {
            throw new ArgumentNullException(nameof(encoder));
        }

        if (decoder == null)
        {
            throw new ArgumentNullException(nameof(decoder));
        }

        // Packet shape is still not finalized. Keep the hook replaceable so business code stays stable.
        m_packetEncoder = encoder;
        m_packetDecoder = decoder;
    }

    public byte[] EncodePacket(int cmd, byte[] body)
    {
        return CloneBytes(m_packetEncoder.Invoke(cmd, CloneBytes(body)));
    }

    public ProtoPacket DecodePacket(byte[] payload)
    {
        return m_packetDecoder.Invoke(CloneBytes(payload));
    }

    public void Clear()
    {
        m_encoders.Clear();
        m_decoders.Clear();
        m_packetEncoder = DefaultPacketEncoder;
        m_packetDecoder = DefaultPacketDecoder;
    }

    private static byte[] DefaultPacketEncoder(int cmd, byte[] body)
    {
        return CloneBytes(body);
    }

    private static ProtoPacket DefaultPacketDecoder(byte[] payload)
    {
        return new ProtoPacket(0, payload);
    }

    private static byte[] CloneBytes(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] copy = new byte[bytes.Length];
        Buffer.BlockCopy(bytes, 0, copy, 0, bytes.Length);
        return copy;
    }
}

}
