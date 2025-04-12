using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network
{
    internal class PacketBuffer : IDisposable
    {
        List<byte> Buff;
        byte[] readBuff;
        int readPosition;
        bool buffUpdate = false;

        public PacketBuffer()
        {
            Buff = new List<byte>();
            readPosition = 0;
        }

        public int GetReadPosition()
        {
            return readPosition;
        }

        public byte[] ToArray()
        {
            return Buff.ToArray();
        }

        public int Count()
        {
            return Buff.Count;
        }

        public int Length()
        {
            return Count() - readPosition;
        }

        public void Clear()
        {
            Buff.Clear();
            readPosition = 0;
        }

        //Write Data

        public void AddInteger(int Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;
        }

        public void AddFloat(float Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;
        }

        public void AddString(string Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input.Length));
            Buff.AddRange(Encoding.ASCII.GetBytes(Input));
            buffUpdate = true;
        }

        public void AddByte(byte Input)
        {
            Buff.Add(Input);
            buffUpdate = true;
        }

        public void AddByteArray(byte[] Input)
        {
            Buff.AddRange(Input);
            buffUpdate = true;
        }

        public void AddShort(short Input)
        {
            Buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;
        }

        //Read Data
        public int GetInteger(bool Peek = true)
        {
            if (Buff.Count > readPosition)
            {
                if (buffUpdate)
                {
                    readBuff = Buff.ToArray();
                    buffUpdate = false;
                }

                int bitToRead = BitConverter.ToInt32(readBuff, readPosition);

                if (Peek & Buff.Count > readPosition)
                {
                    readPosition += 4;
                }

                return bitToRead;
            }
            else
            {
                throw new Exception("Packet Buffer Passed limit");
            }
        }

        public float GetFloat(bool Peek = true)
        {
            if (Buff.Count > readPosition)
            {
                if (buffUpdate)
                {
                    readBuff = Buff.ToArray();
                    buffUpdate = false;
                }

                float bitToRead = BitConverter.ToSingle(readBuff, readPosition);

                if (Peek & Buff.Count > readPosition)
                {
                    readPosition += 4;
                }

                return bitToRead;
            }
            else
            {
                throw new Exception("Packet Buffer Passed limit");
            }
        }

        public string GetString(bool Peek = true)
        {
            int length = GetInteger(true);

            if (buffUpdate)
            {
                readBuff = Buff.ToArray();
                buffUpdate = false;
            }

            string bitToRead = Encoding.ASCII.GetString(readBuff, readPosition, length);

            if (Peek & Buff.Count > readPosition)
            {
                if (bitToRead.Length > 0)
                {
                    readPosition += length;
                }
            }

            return bitToRead;
        }

        public byte GetByte(bool Peek = true)
        {
            if (Buff.Count > readPosition)
            {
                if (buffUpdate)
                {
                    readBuff = Buff.ToArray();
                    buffUpdate = false;
                }

                byte bitToRead = readBuff[readPosition];

                if (Peek & Buff.Count > readPosition)
                {
                    readPosition += 1;
                }

                return bitToRead;
            }
            else
            {
                throw new Exception("Packet Buffer Passed limit");
            }
        }

        public byte[] GetByteArray(int Length, bool Peek = true)
        {
            if (buffUpdate)
            {
                readBuff = Buff.ToArray();
                buffUpdate = false;
            }

            byte[] bitsToRead = Buff.GetRange(readPosition, Length).ToArray();

            if (Peek)
            {
                readPosition += Length;
            }

            return bitsToRead;
        }

        public short GetShort(bool Peek = true)
        {
            if (Buff.Count > readPosition)
            {
                if (buffUpdate)
                {
                    readBuff = Buff.ToArray();
                    buffUpdate = false;
                }

                short bitToRead = BitConverter.ToInt16(readBuff, readPosition);

                if (Peek & Buff.Count > readPosition)
                {
                    readPosition += 2;
                }

                return bitToRead;
            }
            else
            {
                throw new Exception("Packet Buffer Passed limit");
            }
        }

        private bool disposedValue = false;

        //INTEFACE

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Buff.Clear();
                }

                readPosition = 0;
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public byte[] SerializeProto<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T DeserializeProto<T>(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
