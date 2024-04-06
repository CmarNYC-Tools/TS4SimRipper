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
using s4pi.ImageResource;
using s4pi.Interfaces;
using s4pi.Package;

namespace TS4SimRipper
{
    public class Sculpt
    {
        public uint contextVersion;
        public TGI[] publicKey;
        public TGI[] externalKey;
        public TGI[] BGEOKey;
        public ObjectData[] objectKey;

        private uint version;
        public AgeGender ageGender;
        public SimRegion region;
        public SimSubRegion subRegion;
        public BgeoLinkTag linkTag;
        public TGI textureRef;
        public TGI specularRef;
        public TGI bumpmapRef;
        private byte unknown7;
        public TGI dmapShapeRef;
        public TGI dmapNormalRef;
        public TGI boneDeltaRef;
        private uint unknown8;

        public string SculptName;
        public bool isDefaultReplacement;
        public BGEO bgeo;
        public RLEResource texture;
        public DSTResource bumpmap;
        public RLEResource specular;
        public DMap shape;
        public DMap normals;
        public BOND bond;
        public string package;

        public Sculpt(BinaryReader br)
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
            if (version > 0x60) this.subRegion = (SimSubRegion)br.ReadUInt32();
            this.linkTag = (BgeoLinkTag)br.ReadUInt32();
            this.textureRef = new TGI(br, TGI.TGIsequence.ITG);
            if (version > 0x60)
            {
                this.specularRef = new TGI(br, TGI.TGIsequence.ITG);
                this.bumpmapRef = new TGI(br, TGI.TGIsequence.ITG);
            }
            this.unknown7 = br.ReadByte();
            this.dmapShapeRef = new TGI(br, TGI.TGIsequence.ITG);
            this.dmapNormalRef = new TGI(br, TGI.TGIsequence.ITG);
            if (version > 0x60)
            {
                this.boneDeltaRef = new TGI(br, TGI.TGIsequence.ITG);
                this.unknown8 = br.ReadUInt32();
            }
        }

        public Sculpt()
        {
            this.contextVersion = 3;
            this.publicKey = new TGI[] { new TGI(0, 0, 0) };
            this.externalKey = new TGI[0];
            this.BGEOKey = new TGI[0];
            this.linkTag = BgeoLinkTag.NoBGEO;
            this.objectKey = new ObjectData[] { new ObjectData(44U, 121) };
            this.version = 0xB0U;
            this.ageGender = AgeGender.None;
            this.region = SimRegion.EYES;
            this.subRegion = SimSubRegion.None;
            this.textureRef = new TGI((uint)ResourceTypes.RLE2, 0U, 0L);
            this.specularRef = new TGI((uint)ResourceTypes.RLES, 0U, 0L);
            this.bumpmapRef = new TGI((uint)ResourceTypes.DDS, 0U, 0L);
            this.unknown7 = 0;
            this.dmapShapeRef = new TGI();
            this.dmapNormalRef = new TGI();
            this.boneDeltaRef = new TGI();
            this.unknown8 = 0;
        }

        public Sculpt(Sculpt other)
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
            this.textureRef = new TGI(other.textureRef);
            this.specularRef = new TGI(other.specularRef);
            this.bumpmapRef = new TGI(other.bumpmapRef);
            this.unknown7 = other.unknown7;
            this.dmapShapeRef = new TGI(other.dmapShapeRef);
            this.dmapNormalRef = new TGI(other.dmapNormalRef);
            this.boneDeltaRef = new TGI(other.boneDeltaRef);
            this.unknown8 = other.unknown8;
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
                                            121) };
            for (int i = 0; i < objectKey.Length; i++) objectKey[i].Write(bw);
            bw.Write(this.version);
            bw.Write((uint)this.ageGender);
            bw.Write((uint)this.region);
            if (version > 0x60) bw.Write((uint)this.subRegion);
            bw.Write(this.BGEOKey.Length > 0 ? (uint)BgeoLinkTag.UseBGEO : (uint)BgeoLinkTag.NoBGEO);
            this.textureRef.Write(bw, TGI.TGIsequence.ITG);
            if (version > 0x60)
            {
                this.specularRef.Write(bw, TGI.TGIsequence.ITG);
                this.bumpmapRef.Write(bw, TGI.TGIsequence.ITG);
            }
            bw.Write(this.unknown7);
            this.dmapShapeRef.Write(bw, TGI.TGIsequence.ITG);
            this.dmapNormalRef.Write(bw, TGI.TGIsequence.ITG);
            if (version > 0x60)
            {
                this.boneDeltaRef.Write(bw, TGI.TGIsequence.ITG);
                bw.Write(this.unknown8);
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
    }
}
