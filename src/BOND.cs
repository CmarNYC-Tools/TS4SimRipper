/* TS4 SimRipper, a tool for creating custom content for The Sims 4,
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
   The author may be contacted at modthesims.info, username cmarNYC.
  
   BoneDelta format originally from s3pe by Peter Jones*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TS4SimRipper
{
    public class BOND
    {
        public uint contextVersion = 3;
        public TGI[] publicKey;
        public TGI[] externalKey;
        public TGI[] delayLoadKey;
        public ObjectData[] objectKey;
        uint version = 1;
        public BoneAdjust[] adjustments;

        public float weight = 1f;
        public string package;
        public ulong instance;

        public void RemoveBoneAdjust(int index)
        {
            BoneAdjust[] tmp = new BoneAdjust[this.adjustments.Length - 1];
            Array.Copy(this.adjustments, 0, tmp, 0, index);
            Array.Copy(this.adjustments, index + 1, tmp, index, tmp.Length - index);
            this.adjustments = tmp;
        }

        public void AddBoneAdjust(BoneAdjust adjust)
        {
            BoneAdjust[] tmp = new BoneAdjust[this.adjustments.Length + 1];
            Array.Copy(this.adjustments, tmp, this.adjustments.Length);
            tmp[tmp.Length - 1] = new BoneAdjust(adjust);
            this.adjustments = tmp;
        }

        public BOND Multiply(float factor)
        {
            BOND b = new BOND();
            b.publicKey[0] = new TGI(this.publicKey[0]);
            b.adjustments = new BoneAdjust[this.adjustments.Length];
            for (int i = 0; i < this.adjustments.Length; i++)
            {
                b.adjustments[i] = new BoneAdjust()
                {
                    slotHash = this.adjustments[i].slotHash,
                    offsetX = this.adjustments[i].offsetX * factor,
                    offsetY = this.adjustments[i].offsetY * factor,
                    offsetZ = this.adjustments[i].offsetZ * factor,
                    scaleX = this.adjustments[i].scaleX * factor,
                    scaleY = this.adjustments[i].scaleY * factor,
                    scaleZ = this.adjustments[i].scaleZ * factor
                };
                Quaternion q = new Quaternion(this.adjustments[i].quatX, this.adjustments[i].quatY, this.adjustments[i].quatZ, this.adjustments[i].quatW);
                q *= factor;
                b.adjustments[i].quatX = q.X;
                b.adjustments[i].quatY = q.Y;
                b.adjustments[i].quatZ = q.Z;
                b.adjustments[i].quatW = q.W;
            }
            return b;
        }

        public BOND(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.contextVersion = br.ReadUInt32();
            uint publicKeyCount = br.ReadUInt32();
            uint externalKeyCount = br.ReadUInt32();
            uint delayLoadKeyCount = br.ReadUInt32();
            uint objectKeyCount = br.ReadUInt32();
            this.publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++) publicKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            if (objectKeyCount - publicKeyCount > 0)
            {
                TGI[] privateKey = new TGI[objectKeyCount - publicKeyCount];
                for (int i = 0; i < objectKeyCount - publicKeyCount; i++) privateKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            this.externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++) externalKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.delayLoadKey = new TGI[delayLoadKeyCount];
            for (int i = 0; i < delayLoadKeyCount; i++) delayLoadKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.objectKey = new ObjectData[objectKeyCount];
            for (int i = 0; i < objectKeyCount; i++) objectKey[i] = new ObjectData(br);
            version = br.ReadUInt32();
            uint boneAdjustCount = br.ReadUInt32();
            adjustments = new BoneAdjust[boneAdjustCount];
            for (uint i = 0; i < boneAdjustCount; i++)
            {
                adjustments[i] = new BoneAdjust(br);
            }
        }

        public BOND()
        {
            this.publicKey = new TGI[] { new TGI() };
            this.adjustments = new BoneAdjust[0];
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(this.contextVersion);
            if (this.publicKey == null) this.publicKey = new TGI[0];
            bw.Write(publicKey.Length);
            if (this.externalKey == null) this.externalKey = new TGI[0];
            bw.Write(externalKey.Length);
            if (this.delayLoadKey == null) this.delayLoadKey = new TGI[0];
            bw.Write(delayLoadKey.Length);
            bw.Write(1);
            for (int i = 0; i < publicKey.Length; i++) publicKey[i].Write(bw, TGI.TGIsequence.ITG);
            for (int i = 0; i < externalKey.Length; i++) externalKey[i].Write(bw, TGI.TGIsequence.ITG);
            for (int i = 0; i < delayLoadKey.Length; i++) delayLoadKey[i].Write(bw, TGI.TGIsequence.ITG);
            this.objectKey = new ObjectData[] { new ObjectData((uint)(20 + (publicKey.Length * 16) + (externalKey.Length * 16) + (delayLoadKey.Length * 16) + 8),
                                            (uint)(4 + (adjustments.Length * 44))) };
            for (int i = 0; i < objectKey.Length; i++) objectKey[i].Write(bw);
            bw.Write(version);
            if (adjustments == null) adjustments = new BoneAdjust[0];
            bw.Write(adjustments.Length);
            for (uint i = 0; i < adjustments.Length; i++)
            {
                adjustments[i].Write(bw);
            }
        }

        public Stream Stream
        {
            get
            {
                Stream s = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(s);
                this.Write(bw);
                s.Position = 0;
                return s;
            }
        }

        public class ObjectData
        {
            internal uint position;
            internal uint length;

            internal ObjectData(BinaryReader br)
            {
                this.position = br.ReadUInt32();
                this.length = br.ReadUInt32();
            }

            internal ObjectData(uint position, uint length)
            {
                this.position = position;
                this.length = length;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.position);
                bw.Write(this.length);
            }
        }

        public class BoneAdjust
        {
            public uint slotHash;
            public float offsetX;
            public float offsetY;
            public float offsetZ;
            public float scaleX;
            public float scaleY;
            public float scaleZ;
            public float quatX;
            public float quatY;
            public float quatZ;
            public float quatW;

            public BoneAdjust(BinaryReader br)
            {
                slotHash = br.ReadUInt32();
                offsetX = br.ReadSingle();
                offsetY = br.ReadSingle();
                offsetZ = br.ReadSingle();
                scaleX = br.ReadSingle();
                scaleY = br.ReadSingle();
                scaleZ = br.ReadSingle();
                quatX = br.ReadSingle();
                quatY = br.ReadSingle();
                quatZ = br.ReadSingle();
                quatW = br.ReadSingle();
            }

            public BoneAdjust() {}
           
            public BoneAdjust(BoneAdjust other)
            {
                slotHash = other.slotHash;
                offsetX = other.offsetX;
                offsetY = other.offsetY;
                offsetZ = other.offsetZ;
                scaleX = other.scaleX;
                scaleY = other.scaleY;
                scaleZ = other.scaleZ;
                quatX = other.quatX;
                quatY = other.quatY;
                quatZ = other.quatZ;
                quatW = other.quatW;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(slotHash);
                bw.Write(offsetX);
                bw.Write(offsetY);
                bw.Write(offsetZ);
                bw.Write(scaleX);
                bw.Write(scaleY);
                bw.Write(scaleZ);
                bw.Write(quatX);
                bw.Write(quatY);
                bw.Write(quatZ);
                if (quatX > 0f || quatY > 0f || quatZ > 0f)
                {
                    bw.Write(quatW);
                }
                else
                {
                    bw.Write(0f);
                }
            }
        }
    }
}
