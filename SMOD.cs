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
   The author may be contacted at modthesims.info, username cmarNYC. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using s4pi.Interfaces;
using s4pi.Package;

namespace TS4SimRipper
{
    public class SMOD
    {
        public uint contextVersion;
        public TGI[] publicKey;
        public TGI[] externalKey;
        public TGI[] BGEOKey;
        public ObjectData[] objectKey;
        public uint version;
        public AgeGender ageGender;
        public SimRegion region;
        public SimSubRegion subRegion;
        public BgeoLinkTag linkTag;
        public TGI bonePoseKey;
        public TGI deformerMapShapeKey;
        public TGI deformerMapNormalKey;
        public BoneEntry[] boneEntryList;

        public string smodName;
        public bool isDefaultReplacement;
        public BGEO bgeo;
        public DMap shape;
        public DMap normals;
        public BOND bond;
        public string package;

        public SMOD(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.contextVersion = br.ReadUInt32();
            uint publicKeyCount = br.ReadUInt32();
            uint externalKeyCount = br.ReadUInt32();
            uint delayLoadKeyCount = br.ReadUInt32();
            uint objectKeyCount = br.ReadUInt32();
            this.publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++) publicKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++) externalKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.BGEOKey = new TGI[delayLoadKeyCount];
            for (int i = 0; i < delayLoadKeyCount; i++) BGEOKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.objectKey = new ObjectData[objectKeyCount];
            for (int i = 0; i < objectKeyCount; i++) objectKey[i] = new ObjectData(br);
            this.version = br.ReadUInt32();
            this.ageGender = (AgeGender)br.ReadUInt32();
            this.region = (SimRegion)br.ReadUInt32();
            if (this.version >= 144) this.subRegion = (SimSubRegion)br.ReadUInt32();
            this.linkTag = (BgeoLinkTag)br.ReadUInt32();
            this.bonePoseKey = new TGI(br, TGI.TGIsequence.ITG);
            this.deformerMapShapeKey = new TGI(br, TGI.TGIsequence.ITG);
            this.deformerMapNormalKey = new TGI(br, TGI.TGIsequence.ITG);
            uint count = br.ReadUInt32();
            this.boneEntryList = new BoneEntry[count];
            for (int i = 0; i < count; i++)
            {
                this.boneEntryList[i] = new BoneEntry(br);
            }
        }

        public SMOD()
        {
            this.contextVersion = 3;
            this.publicKey = new TGI[] { new TGI(0, 0, 0) };
            this.externalKey = new TGI[0];
            this.BGEOKey = new TGI[0];
            this.linkTag = BgeoLinkTag.NoBGEO;
            this.objectKey = new ObjectData[] { new ObjectData(44U, 72) };
            this.version = 0x90U;
            this.ageGender = AgeGender.None;
            this.region = SimRegion.EYES;
            this.subRegion = SimSubRegion.None;
            this.bonePoseKey = new TGI(0, 0, 0);
            this.deformerMapShapeKey = new TGI(0, 0, 0);
            this.deformerMapNormalKey = new TGI(0, 0, 0);
            this.boneEntryList = new BoneEntry[0];
        }

        public SMOD(SMOD other)
        {
            this.contextVersion = other.contextVersion;
            this.publicKey = TGI.CopyTGIArray(other.publicKey);
            this.externalKey = TGI.CopyTGIArray(other.externalKey);
            this.BGEOKey = TGI.CopyTGIArray(other.BGEOKey);
            this.linkTag = other.linkTag;
            this.objectKey = new ObjectData[other.objectKey.Length];
            Array.Copy(other.objectKey, this.objectKey, other.objectKey.Length);
            this.version = other.version;
            this.ageGender = other.ageGender;
            this.region = other.region;
            this.subRegion = other.subRegion;
            this.bonePoseKey = new TGI(other.bonePoseKey);
            this.deformerMapShapeKey = new TGI(other.deformerMapShapeKey);
            this.deformerMapNormalKey = new TGI(other.deformerMapNormalKey);
            this.boneEntryList = new BoneEntry[other.boneEntryList.Length];
            Array.Copy(other.boneEntryList, this.boneEntryList, other.boneEntryList.Length);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.contextVersion);
            if (this.publicKey == null) this.publicKey = new TGI[0];
            bw.Write(publicKey.Length);
            if (this.externalKey == null) this.externalKey = new TGI[0];
            bw.Write(externalKey.Length);
            if (this.BGEOKey == null) this.BGEOKey = new TGI[0];
            bw.Write(BGEOKey.Length);
            bw.Write(1);
            for (int i = 0; i < publicKey.Length; i++) publicKey[i].Write(bw, TGI.TGIsequence.ITG);
            for (int i = 0; i < externalKey.Length; i++) externalKey[i].Write(bw, TGI.TGIsequence.ITG);
            for (int i = 0; i < BGEOKey.Length; i++) BGEOKey[i].Write(bw, TGI.TGIsequence.ITG);
            this.objectKey = new ObjectData[] { new ObjectData((uint)(20 + (publicKey.Length * 16) + (externalKey.Length * 16) + (BGEOKey.Length * 16) + 8),
                                            (uint)(16 + (this.version >= 144 ? 4 : 0) + 48 + 4 + (boneEntryList.Length * 8))) };
            for (int i = 0; i < objectKey.Length; i++) objectKey[i].Write(bw);
            bw.Write(this.version);
            bw.Write((uint)this.ageGender);
            bw.Write((uint)this.region);
            if (this.version >= 144) bw.Write((uint)this.subRegion);
            bw.Write(this.BGEOKey.Length > 0 ? (uint)BgeoLinkTag.UseBGEO : (uint)BgeoLinkTag.NoBGEO);
            this.bonePoseKey.Write(bw, TGI.TGIsequence.ITG);
            this.deformerMapShapeKey.Write(bw, TGI.TGIsequence.ITG);
            this.deformerMapNormalKey.Write(bw, TGI.TGIsequence.ITG);
            if (this.boneEntryList == null) this.boneEntryList = new BoneEntry[0];
            bw.Write(boneEntryList.Length);
            for (int i = 0; i < boneEntryList.Length; i++)
            {
                this.boneEntryList[i].Write(bw);
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

        public class BoneEntry
        {
            internal uint boneHash;
            internal float multiplier;

            internal BoneEntry(BinaryReader br)
            {
                this.boneHash = br.ReadUInt32();
                this.multiplier = br.ReadSingle();
            }

            internal BoneEntry(uint hash, float multiplier)
            {
                this.boneHash = hash;
                this.multiplier = multiplier;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.boneHash);
                bw.Write(this.multiplier);
            }

        }
    }
}
