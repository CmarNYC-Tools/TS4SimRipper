/* TS4 MorphMaker, a tool for creating custom content for The Sims 4,
   Copyright (C) 2014  C. Marinetti

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
   The author may be contacted at modthesims.info, username cmarNYC. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS4SimRipper
{
    public static class FNVhash
    {
        private static uint FNV_PRIME_32 = 16777619;
        private static uint FNV_OFFSET_32 = 2166136261;
        private static ulong FNV_PRIME_64 = 1099511628211;
        private static ulong FNV_OFFSET_64 = 14695981039346656037;

        public static ushort FNV16(string str)
        {
            uint hash = FNV_OFFSET_32;
            string s = str.ToLower();
            byte[] ASCII = Encoding.ASCII.GetBytes(s);
            foreach (byte b in ASCII)
            {
                unchecked { hash = hash * FNV_PRIME_32; }
                hash = hash ^ b;
            }
            uint shifted = hash >> 16;
            hash = hash ^ shifted;
            hash = hash & 65535;
            return (ushort)hash;
        }

        public static uint FNV24(string str)
        {
            uint hash = FNV_OFFSET_32;
            string s = str.ToLower();
            byte[] ASCII = Encoding.ASCII.GetBytes(s);
            foreach (byte b in ASCII)
            {
                unchecked { hash = hash * FNV_PRIME_32; }
                hash = hash ^ b;
            }
            uint shifted = hash >> 24;
            hash = hash ^ shifted;
            hash = hash & 16777215;
            return hash;
        }

        public static uint FNV32(string str)
        {
            uint hash = FNV_OFFSET_32;
            string s = str.ToLower();
            byte[] ASCII = Encoding.ASCII.GetBytes(s);
            foreach (byte b in ASCII)
            {
                unchecked { hash = hash * FNV_PRIME_32; }
                hash = hash ^ b;
            }
            return hash;
        }

        public static ulong FNV64(string str)
        {
            ulong hash = FNV_OFFSET_64;
            string s = str.ToLower();
            byte[] ASCII = Encoding.ASCII.GetBytes(s);
            foreach (byte b in ASCII)
            {
                unchecked { hash = hash * FNV_PRIME_64; }
                hash = hash ^ b;
            }
            return hash;
        }

        public static String FormatFNV(uint Hash)
        {
            return Convert.ToString(Hash, 16).ToUpper().PadLeft(8, '0');
        }
        public static String FormatFNV(ulong Hash)
        {
            ulong tmp = Hash;
            uint low64, hi64;
            unchecked
            {
                low64 = (uint)tmp;
                hi64 = (uint)(tmp >> 32);
            }
            return Convert.ToString(hi64, 16).ToUpper().PadLeft(8, '0') + Convert.ToString(low64, 16).ToUpper().PadLeft(8, '0');
        }
    }
}
