/* Xmods Data Library, a library to support tools for The Sims 4,
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
using System.Text;
using System.IO;

namespace TS4SimRipper
{
    public class TGI
    {
        uint type;
        uint group;
        ulong instance;

        public uint Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public uint Group
        {
            get { return this.group; }
            set { this.group = value; }
        }

        public ulong Instance
        {
            get { return this.instance; }
            set { this.instance = value; }
        }

        public TGI()
        {
            type = 0U;
            group = 0U;
            instance = 0UL;
        }

        public TGI(uint typeID, uint groupID, ulong instanceID)
        {
            type = typeID;
            group = groupID;
            instance = instanceID;
        }

        public TGI(string tgi)
        {
            if (String.CompareOrdinal(tgi, " ") <= 0)
            {
                type = 0U;
                group = 0U;
                instance = 0LU;
                return;
            }
            string[] myTGI = tgi.Split('-', ':', '.', ' ', '_');
            for (int i = 0; i < myTGI.Length; i++)
            {
                if (String.CompareOrdinal(myTGI[i].Substring(0, 2), "0x") == 0)
                {
                    myTGI[i] = myTGI[i].Substring(2);
                }
            }
            try
            {
                type = UInt32.Parse(myTGI[0], System.Globalization.NumberStyles.HexNumber);
                group = UInt32.Parse(myTGI[1], System.Globalization.NumberStyles.HexNumber);
                instance = UInt64.Parse(myTGI[2], System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                throw new ApplicationException("Can't parse TGI string " + tgi);
            }
        }

        public TGI(TGI tgi)
        {
            this.type = tgi.Type;
            this.group = tgi.Group;
            this.instance = tgi.Instance;
        }

        public TGI(BinaryReader br)
        {
            this.type = br.ReadUInt32();
            this.group = br.ReadUInt32();
            this.instance = br.ReadUInt64();
        }

        public TGI(BinaryReader br, TGIsequence sequence)
        {
            if (sequence == TGIsequence.TGI)
            {
                this.type = br.ReadUInt32();
                this.group = br.ReadUInt32();
                this.instance = br.ReadUInt64();
            }
            if (sequence == TGIsequence.IGT)
            {
                this.instance = br.ReadUInt64();
                this.group = br.ReadUInt32();
                this.type = br.ReadUInt32();
            }
            if (sequence == TGIsequence.ITG)
            {
                this.instance = br.ReadUInt64();
                this.type = br.ReadUInt32();
                this.group = br.ReadUInt32();
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.type);
            bw.Write(this.group);
            bw.Write(this.instance);
        }

        public void Write(BinaryWriter bw, TGIsequence sequence)
        {
            if (sequence == TGIsequence.TGI)
            {
                bw.Write(this.type);
                bw.Write(this.group);
                bw.Write(this.instance);
            }
            if (sequence == TGIsequence.IGT)
            {
                bw.Write(this.instance);
                bw.Write(this.group);
                bw.Write(this.type);
            }
            if (sequence == TGIsequence.ITG)
            {
                bw.Write(this.instance);
                bw.Write(this.type);
                bw.Write(this.group);
            }
        }

        public bool Equals(TGI tgi)
        {
            if (this.type == (uint)ResourceTypes.CASP || this.type == (uint)ResourceTypes.Sculpt || this.type == (uint)ResourceTypes.SimModifier)
            {
                return (this.type == tgi.type & this.instance == tgi.instance);
            }
            else
            {
                return (this.type == tgi.type & this.group == tgi.group & this.instance == tgi.instance);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is TGI)
            {
                return this.Equals((TGI)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode() + this.group.GetHashCode() + this.instance.GetHashCode();
        }

        public override string ToString()
        {
            return "0x" + this.type.ToString("X8") + "-" + "0x" + this.group.ToString("X8") + "-" + "0x" + this.instance.ToString("X16");
        }

        public enum TGIsequence
        {
            TGI, ITG, IGT
        }

        internal static TGI[] CopyTGIArray(TGI[] source)
        {
            TGI[] tmp = new TGI[source.Length];
            Array.Copy(source, tmp, source.Length);
            return tmp;
        }
    }
}
