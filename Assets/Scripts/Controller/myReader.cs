using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class myReader
{
    public sbyte[] buffer;

    private int posRead;

    private int posMark;

    private static string fileName;

    private static int status;

    public myReader()
    {
    }

    public myReader(sbyte[] data)
    {
        buffer = data;
    }

    public myReader(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        buffer = convertToSbyte(textAsset.bytes);
    }

    public sbyte[] convertToSbyte(byte[] scr)
    {
        sbyte[] array = new sbyte[scr.Length];
        for (int i = 0; i < scr.Length; i++)
        {
            array[i] = (sbyte)scr[i];
        }
        return array;
    }
    public sbyte[] ReadAudio(int length)
    {
        sbyte[] result = new sbyte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = ReadSByteUncheck();
        }
        return result;
    }

    public sbyte ReadSByteUncheck()
    {
        return (sbyte)buffer[posRead++];
    }

    public sbyte readSByte()
    {
        if (posRead < buffer.Length)
        {
            return buffer[posRead++];
        }
        posRead = buffer.Length;
        throw new Exception(" loi doc sbyte eof ");
    }
    public void CheckLenght(int ltemp)
    {
        if (posRead + ltemp >= posMark)
        {
            sbyte[] array = new sbyte[posMark + 1024 + ltemp];
            for (int i = 0; i < posMark; i++)
            {
                array[i] = buffer[i];
            }
            buffer = null;
            buffer = array;
            posMark += 1024 + ltemp;
        }
    }
    public byte[] ConvertSbyteToByte(sbyte[] var)
    {
        byte[] array = new byte[var.Length];
        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] > 0)
            {
                array[i] = (byte)var[i];
            }
            else
            {
                array[i] = (byte)(var[i] + 256);
            }
        }
        return array;
    }
    public float[] ConvertShortToFloatArray(byte[] byteArray)
    {
        int sampleCount = byteArray.Length / 2;
        float[] floatArray = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            short value = (short)(byteArray[i * 2] | (byteArray[i * 2 + 1] << 8));
            floatArray[i] = value / (float)short.MaxValue;
        }

        return floatArray;
    }
    public byte[] ReadBytes(int length)
    {
        CheckLenght(length);
        // Tạo mảng byte để lưu dữ liệu đã đọc
        byte[] byteArray = new byte[length];

        for (int i = 0; i < length; i++)
        {
            byteArray[i] = convertSbyteToByte(readSByte());
        }

        return byteArray;
    }
    public sbyte readsbyte()
    {
        return readSByte();
    }

    public sbyte readByte()
    {
        return readSByte();
    }

    public void mark(int readlimit)
    {
        posMark = posRead;
    }

    public void reset()
    {
        posRead = posMark;
    }

    public byte readUnsignedByte()
    {
        return convertSbyteToByte(readSByte());
    }

    public short readShort()
    {
        short num = 0;
        for (int i = 0; i < 2; i++)
        {
            num <<= 8;
            num |= (short)(0xFF & buffer[posRead++]);
        }
        return num;
    }

    public float readFloat()
    {
        // Đảm bảo buffer có đủ 4 byte để đọc
        if (posRead + 4 > buffer.Length)
        {
            throw new IndexOutOfRangeException("Buffer không đủ dữ liệu để đọc float.");
        }

        // Đọc 4 byte và chuyển thành float
        byte[] floatBytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            floatBytes[i] = (byte)buffer[posRead++];
        }

        // Chuyển đổi từ mảng byte thành float
        return BitConverter.ToSingle(floatBytes, 0);
    }

    public ushort readUnsignedShort()
    {
        ushort num = 0;
        for (int i = 0; i < 2; i++)
        {
            num <<= 8;
            num |= (ushort)(0xFFu & (uint)buffer[posRead++]);
        }
        return num;
    }

    public int readInt()
    {
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            num <<= 8;
            num |= 0xFF & buffer[posRead++];
        }
        return num;
    }

    public long readLong()
    {
        long num = 0L;
        for (int i = 0; i < 8; i++)
        {
            num <<= 8;
            num |= (uint)(0xFF & buffer[posRead++]);
        }
        return num;
    }

    public bool readBool()
    {
        return readSByte() > 0;
    }
    private byte ConvertAudio(sbyte value)
    {
        return value >= 0 ? (byte)value : (byte)(value + 256);
    }
    public bool readBoolean()
    {
        return readSByte() > 0;
    }

    public string readString()
    {
        short num = readShort();
        byte[] array = new byte[num];
        for (int i = 0; i < num; i++)
        {
            array[i] = convertSbyteToByte(readSByte());
        }
        UTF8Encoding uTF8Encoding = new UTF8Encoding();
        return uTF8Encoding.GetString(array);
    }

    public string readStringUTF()
    {
        short num = readShort();
        byte[] array = new byte[num];
        for (int i = 0; i < num; i++)
        {
            array[i] = convertSbyteToByte(readSByte());
        }
        UTF8Encoding uTF8Encoding = new UTF8Encoding();
        return uTF8Encoding.GetString(array);
    }

    public string readUTF()
    {
        return readStringUTF();
    }

    public int read()
    {
        if (posRead < buffer.Length)
        {
            return readSByte();
        }
        return -1;
    }

    public int read(ref sbyte[] data)
    {
        if (data == null)
        {
            return 0;
        }
        int num = 0;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = readSByte();
            if (posRead > buffer.Length)
            {
                return -1;
            }
            num++;
        }
        return num;
    }

    public void readFully(ref byte[] data)
    {
        if (data != null && data.Length + posRead <= buffer.Length)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = convertSbyteToByte(readSByte());
            }
        }
    }

    public int available()
    {
        return buffer.Length - posRead;
    }

    public byte convertSbyteToByte(sbyte var)
    {
        if (var > 0)
        {
            return (byte)var;
        }
        return (byte)(var + 256);
    }

    public static byte[] convertSbyteToByte(sbyte[] var)
    {
        byte[] array = new byte[var.Length];
        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] > 0)
            {
                array[i] = (byte)var[i];
            }
            else
            {
                array[i] = (byte)(var[i] + 256);
            }
        }
        return array;
    }

    public void Close()
    {
        buffer = null;
    }

    public void close()
    {
        buffer = null;
    }

    public void read(ref sbyte[] data, int arg1, int arg2)
    {
        if (data == null)
        {
            return;
        }
        for (int i = 0; i < arg2; i++)
        {
            data[i + arg1] = readSByte();
            if (posRead > buffer.Length)
            {
                break;
            }
        }
    }

    public void CoppyData(myReader myReaderCoppy)
    {
        CoppyArray(myReaderCoppy.buffer);
        SetPoint(myReaderCoppy.posRead);
        SetMask(myReaderCoppy.posMark);
    }

    private void SetPoint(int point)
    {
        this.posRead = point;
    }

    private void SetMask(int point)
    {
        this.posMark = point;
    }

    private void CoppyArray(sbyte[] buffer)
    {
        this.buffer = buffer.ToArray();
    }
}
