using System;
using System.Buffers.Binary;
using System.IO;

namespace Skateboard3Server.Qos.Util;

public static class BinaryReaderExtensions
{
    public static short ReadInt16Be(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadInt16BigEndian(reader.ReadBytes(2));
    }

    public static ushort ReadUInt16Be(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadUInt16BigEndian(reader.ReadBytes(2));
    }

    public static int ReadInt32Be(this BinaryReader reader)
    {
        try
        {
            return BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(4));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("Malformed bytes?");
            return 0; 
        }
    }

    public static uint ReadUInt32Be(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadUInt32BigEndian(reader.ReadBytes(4));
    }

    public static long ReadInt64Be(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadInt64BigEndian(reader.ReadBytes(8));
    }

    public static ulong ReadUInt64Be(this BinaryReader reader)
    {
        return BinaryPrimitives.ReadUInt64BigEndian(reader.ReadBytes(8));
    }

}