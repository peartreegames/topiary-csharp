using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PeartreeGames.Topiary
{
    public static class ByteCode
    {
        public static SortedSet<string> GetExterns(BinaryReader reader)
        {
            var globalSymbolsCount = reader.ReadUInt64();
            var result = new SortedSet<string>();
            var indexSize = Marshal.SizeOf<uint>();
            for (ulong i = 0; i < globalSymbolsCount; i++)
            {
                var nameLength = reader.ReadByte();
                var name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                reader.ReadBytes(indexSize); // skip globals index
                var isExtern = reader.ReadByte() == 1;
                _ = reader.ReadByte() == 1; // mutable
                if (isExtern) result.Add(name);
            }

            return result;
        }
    }
}