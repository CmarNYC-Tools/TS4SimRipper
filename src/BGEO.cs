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
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TS4SimRipper
{
    public class BGEO
    {
        uint contextVersion;
        TGI[] publicKey;
        TGI[] externalKey;
        TGI[] delayloadKey;
        ObjectData[] objectData;
        uint tag = 0x4F454742;     //BGEO ‭‬
        uint version = 0x00000600;
        LOD[] lodData;
        Blend[] blendMap;
        Vector[] vectorData;

        public float weight = 1f;
        public string package;
        public ulong instance;

        public TGI PublicKey { get { return publicKey[0]; } set { this.publicKey[0] = value; } }
        public LOD[] LodData { get { return lodData; } set { this.lodData = value; } }
        public Blend[] BlendMap { get { return blendMap; } set { this.blendMap = value; } }
        public Vector[] VectorData { get { return vectorData; } set { this.vectorData = value; } }

        public BGEO Multiply(float factor)
        {
            BGEO b = new BGEO(this);
            for (int i = 0; i < b.vectorData.Length; i++)
            {
                float[] v = b.vectorData[i].TranslatedVector;
                v[0] *= factor;
                v[1] *= factor;
                v[2] *= factor;
                b.vectorData[i] = new Vector(v);
            }
            return b;
        }

        public BGEO(BGEO basis)
            : this(basis.contextVersion, basis.publicKey, basis.externalKey, basis.delayloadKey, basis.objectData, 
            basis.version, basis.lodData, basis.blendMap, basis.vectorData) { }
        public BGEO(uint contextVersion, TGI[] publicKey, TGI[] externalKey, TGI[] delayloadKey, ObjectData[] objectData, 
            uint version, LOD[] lodData, Blend[] blendMap, Vector[] vectorData)
        {
            this.contextVersion = contextVersion;
            this.publicKey = new TGI[publicKey != null ? publicKey.Length : 0];
            for (int i = 0; i < this.publicKey.Length; i++)
            {
                this.publicKey[i] = new TGI(publicKey[i]);
            }
            this.externalKey = new TGI[externalKey != null ? externalKey.Length : 0];
            for (int i = 0; i < this.externalKey.Length; i++)
            {
                this.externalKey[i] = new TGI(externalKey[i]);
            }
            this.delayloadKey = new TGI[delayloadKey != null ? delayloadKey.Length : 0];
            for (int i = 0; i < this.delayloadKey.Length; i++)
            {
                this.delayloadKey[i] = new TGI(delayloadKey[i]);
            }
            this.objectData = new ObjectData[objectData.Length];
            for (int i = 0; i < objectData.Length; i++)
            {
                this.objectData[i] = new ObjectData(objectData[i]);
            }
            this.version = version;
            this.lodData = lodData;
            this.blendMap = blendMap;
            this.vectorData = vectorData;
        }

        public BGEO(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.contextVersion = br.ReadUInt32();
            uint publicKeyCount = br.ReadUInt32();
            uint externalKeyCount = br.ReadUInt32();
            uint delayloadKeyCount = br.ReadUInt32();
            uint objectCount = br.ReadUInt32();
            this.publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++)
            {
                this.publicKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            this.externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++)
            {
                this.externalKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            this.delayloadKey = new TGI[delayloadKeyCount];
            for (int i = 0; i < delayloadKeyCount; i++)
            {
                this.delayloadKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            this.objectData = new ObjectData[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                this.objectData[i] = new ObjectData(br);
            }
            this.tag = br.ReadUInt32();
            if (tag != BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BGEO"), 0))
            {
                throw new ApplicationException("Not a valid BGEO file!");
            }
            version = br.ReadUInt32();
            if (version != 0x00000600)
            {
                throw new ApplicationException("Invalid version of BGEO file!");
            }
            uint lodCount = br.ReadUInt32();
            uint totalVertexCount = br.ReadUInt32();
            uint totalVectorCount = br.ReadUInt32();
            lodData = new LOD[lodCount];
            for (int i = 0; i < lodCount; i++)
            {
                lodData[i] = new LOD(br);
            }
            blendMap = new Blend[totalVertexCount];
            int runningIndex = 0;
            int lodCounter = 0;
            int previousLODnumVerts = 0;
            for (int i = 0; i < totalVertexCount; i++)
            {
                if (i == lodData[lodCounter].NumberVertices + previousLODnumVerts)
                {
                    runningIndex += (int)lodData[lodCounter].NumberDeltaVectors;
                    previousLODnumVerts += (int)lodData[lodCounter].NumberVertices;
                    lodCounter++;
                    if (lodCounter > 3) lodCounter = 3;
                }
                blendMap[i] = new Blend(br, runningIndex);
                runningIndex += blendMap[i].Offset;
            }
            vectorData = new Vector[totalVectorCount];
            for (int i = 0; i < totalVectorCount; i++)
            {
                vectorData[i] = new Vector(br);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(contextVersion);
            if (publicKey == null) publicKey = new TGI[0];
            bw.Write(publicKey.Length);
            if (externalKey == null) externalKey = new TGI[0];
            bw.Write(externalKey.Length);
            if (delayloadKey == null) delayloadKey = new TGI[0];
            bw.Write(delayloadKey.Length);
            bw.Write(objectData.Length);
            for (int i = 0; i < publicKey.Length; i++)
            {
                this.publicKey[i].Write(bw, TGI.TGIsequence.ITG);
            }
            for (int i = 0; i < externalKey.Length; i++)
            {
                this.externalKey[i].Write(bw, TGI.TGIsequence.ITG);
            }
            for (int i = 0; i < delayloadKey.Length; i++)
            {
                this.delayloadKey[i].Write(bw, TGI.TGIsequence.ITG);
            }
            for (int i = 0; i < objectData.Length; i++)
            {
                this.objectData[i].Write(bw);
            }
            bw.Write(tag);
            bw.Write(version);
            if (lodData == null) lodData = new LOD[0];
            bw.Write(lodData.Length);
            if (blendMap == null) blendMap = new Blend[0];
            bw.Write(blendMap.Length);
            if (vectorData == null) vectorData = new Vector[0];
            bw.Write(vectorData.Length);
            for (int i = 0; i < lodData.Length; i++)
            {
                if (lodData[i] != null) { lodData[i].Write(bw); } else { (new LOD()).Write(bw); }
            }
            for (int i = 0; i < blendMap.Length; i++)
            {
                if (blendMap[i] != null) { blendMap[i].Write(bw); } else { (new Blend()).Write(bw); }
            }
            for (int i = 0; i < vectorData.Length; i++)
            {
                if (vectorData[i] != null) { vectorData[i].Write(bw); } else { (new Vector()).Write(bw); }
            }
        }

        public enum BlendMapFlags
        {
            // Apply a position delta to this vertex.
            FlagPosDelta = 1,
            // Apply a normal delta to this vertex. It follows the position delta if both
            // are present.
            FlagNorDelta = 2,
            FlagAll      = 3,
            PackIndexScale = 4,
        };
        const byte packIndexShift = 2;

        public class ObjectData
        {
            uint position;
            uint length;
            internal ObjectData(ObjectData other)
            {
                this.position = other.position;
                this.length = other.length;
            }
            internal ObjectData(uint position, uint length)
            {
                this.position = position;
                this.length = length;
            }
            internal ObjectData(BinaryReader br)
            {
                this.position = br.ReadUInt32();
                this.length = br.ReadUInt32();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.position);
                bw.Write(this.length);
            }
        }

        public class LOD : IEquatable<LOD>
        {
            uint indexBase;
            uint numVerts;
            uint numDeltaVectors;

            public uint IndexBase { get { return indexBase; } }
            public uint NumberVertices { get { return numVerts; } }
            public uint NumberDeltaVectors { get { return numDeltaVectors; } }

            public LOD()
            {
                this.indexBase = 0;
                this.numVerts = 0;
                this.numDeltaVectors = 0;
            }

            public LOD(LOD basis)
                : this(basis.indexBase, basis.numVerts, basis.numDeltaVectors) { }

            public LOD(uint indexBase, uint numVerts, uint numDeltaVectors)
            {
                this.indexBase = indexBase;
                this.numVerts = numVerts;
                this.numDeltaVectors = numDeltaVectors;
            }

            internal LOD(BinaryReader br)
            {
                this.indexBase = br.ReadUInt32();
                this.numVerts = br.ReadUInt32();
                this.numDeltaVectors = br.ReadUInt32();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.indexBase);
                bw.Write(this.numVerts);
                bw.Write(this.numDeltaVectors);
            }

            public bool Equals(LOD other)
            {
                return this.indexBase.Equals(other.indexBase)
                    && this.numVerts.Equals(other.numVerts)
                    && this.numDeltaVectors.Equals(other.numDeltaVectors);
            }

            public override bool Equals(object obj) { return obj is LOD && Equals(obj as LOD); }

            public override int GetHashCode() { return indexBase.GetHashCode() ^ numVerts.GetHashCode() ^ numDeltaVectors.GetHashCode(); }
        }

        public class Blend : IEquatable<Blend>
        {
            bool positionDelta, normalDelta;
            short offset;
            internal int index;

            public bool PositionDelta { get { return positionDelta; } }
            public bool NormalDelta { get { return normalDelta; } }
            public int Offset { get { return offset; } }
            public int Index { get { return index; } }

            public Blend()
            {
                this.positionDelta = false;
                this.normalDelta = false;
                this.offset = 0;
                this.index = 0;
            }

            public Blend(Blend basis)
                : this(basis.positionDelta, basis.normalDelta, basis.offset, basis.index) { }

            public Blend(bool posDelta, bool normDelta, short off, int ind)
            {
                this.positionDelta = posDelta;
                this.normalDelta = normDelta;
                this.offset = off;
                this.index = ind;
            }

            public Blend(bool posDelta, bool normDelta, short off)
            {
                this.positionDelta = posDelta;
                this.normalDelta = normDelta;
                this.offset = off;
                this.index = 0;
            }

            internal Blend(BinaryReader br, int lastIndex)
            {
                short tmp = br.ReadInt16();
                positionDelta = (tmp & (ushort)BlendMapFlags.FlagPosDelta) > 0;
                normalDelta = (tmp & (ushort)BlendMapFlags.FlagNorDelta) > 0;
                offset = (short)(tmp >> packIndexShift);
                index = offset + lastIndex;
            }
            internal void Write(BinaryWriter bw)
            {
                short tmp = (short)((offset << packIndexShift) + (positionDelta ? (short)BlendMapFlags.FlagPosDelta : 0) + 
                    (normalDelta ? (short)BlendMapFlags.FlagNorDelta : 0));
                bw.Write(tmp);
            }

            public bool Equals(Blend other)
            {
                return (this.positionDelta == other.positionDelta) & (this.normalDelta == other.normalDelta) &
                    (this.offset == other.offset);
            }

            public override bool Equals(object obj) { return obj is Blend && Equals(obj as Blend); }
        }

        public class Vector : IEquatable<Vector>
        {
            ushort[] vector;
            const float scaleFactor = 8000f;

            public float[] TranslatedVector
            {
                get
                {
                    float[] translated = new float[3];
                    for (int i = 0; i < 3; i++)
                    {
                        int tmp = ((vector[i] ^ 0x8000) << 16) >> 16;   //flip sign bit and move it to high bit
                        translated[i] = tmp / scaleFactor;
                    }
                    return translated;
                }
            }

            public Vector()
            {
                this.vector = new ushort[] { 0, 0, 0 };
            }

            public Vector(Vector basis)
                : this(basis.vector) { }
            
            public Vector(ushort[] vector)
            {
                this.vector = new ushort[vector.Length];
                for (int i = 0; i < vector.Length; i++)
                {
                    this.vector[i] = vector[i];
                }
            }
            
            public Vector(float[] vector)
            {
                this.vector = new ushort[vector.Length];
                for (int i = 0; i < vector.Length; i++)
                {
                    int tmp = (Convert.ToInt32(vector[i] * scaleFactor)) ^ 0x8000;
                    this.vector[i] = BitConverter.ToUInt16(BitConverter.GetBytes(tmp), 0);
                }
            }

            public Vector(BinaryReader br)
            {
                this.vector = new ushort[3];
                this.vector[0] = br.ReadUInt16();
                this.vector[1] = br.ReadUInt16();
                this.vector[2] = br.ReadUInt16();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.vector[0]);
                bw.Write(this.vector[1]);
                bw.Write(this.vector[2]);
            }

            public bool Equals(Vector other)
            {
                return this.vector.SequenceEqual(other.vector);
            }

            public override bool Equals(object obj) { return obj is Vector && Equals(obj as Vector); }

        }
    }
}
