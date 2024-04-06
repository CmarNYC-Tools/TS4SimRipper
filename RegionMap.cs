using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TS4SimRipper
{
    public class RegionMap
    {
        uint contextVersion;                 //should be 3
        uint publicKeyCount, externalKeyCount, delayLoadKeyCount, objectCount;
        TGI[] publicKey;                     //TGI of self, in ITG format
        TGI[] externalKey, delayLoadKey;    //normally not used
        ObjectData[] objData;               //position and lengths of MeshBlocks
        int version;                        //should be 1
        MeshBlock[] meshBlocks;

        internal int MeshBlockDataLength
        {
            get
            {
                int tmp = 0;
                foreach (MeshBlock mb in this.meshBlocks)
                {
                    tmp += mb.BlockSize;
                }
                return tmp;
            }
        }

        public uint[] GetMeshRegions(TGI meshTGI)
        {
            List<uint> tmp = new List<uint>();
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                for (int j = 0; j < this.meshBlocks[i].ITGtable.Length; j++)
                {
                    if (this.meshBlocks[i].ITGtable[j].Equals(meshTGI))
                    {
                        if (this.meshBlocks[i].region == (uint)CASPartRegionTS4.Base) return new uint[] { (uint)CASPartRegionTS4.Base };
                        tmp.Add(this.meshBlocks[i].region);
                    }
                }
            }
            return tmp.ToArray();
        }

        public float[] GetMeshLayers(TGI meshTGI)
        {
            List<float> tmp = new List<float>();
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                for (int j = 0; j < this.meshBlocks[i].ITGtable.Length; j++)
                {
                    if (this.meshBlocks[i].ITGtable[j].Equals(meshTGI))
                    {
                        if (this.meshBlocks[i].region == (uint)CASPartRegionTS4.Base) return new float[] { 0 };
                        tmp.Add(this.meshBlocks[i].layer);
                    }
                }
            }
            return tmp.ToArray();
        }

        public float GetLayer(CASPartRegionTS4 region)
        {
            float layer = -1f;
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                if (this.meshBlocks[i].region == (uint)region) layer = Math.Max(layer, this.meshBlocks[i].layer); 
            }
            return layer;
        }

        public void replaceLink(TGI oldtgi, TGI newtgi)
        {
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                for (int j = 0; j < this.meshBlocks[i].ITGtable.Length; j++)
                {
                    if (this.meshBlocks[i].ITGtable[j].Equals(oldtgi))
                    {
                        this.meshBlocks[i].ITGtable[j] = new TGI(newtgi);
                    }
                }
            }
        }

        public void RemoveLink(TGI meshTGI)
        {
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                List<TGI> linkList = new List<TGI>(this.meshBlocks[i].ITGtable);
                for (int j = 0; j < linkList.Count; j++)
                {
                    if (linkList[j].Equals(meshTGI))
                    {
                        linkList.RemoveAt(j);
                    }
                }
                this.meshBlocks[i].ITGtable = linkList.ToArray();
            }
            List<int> removeRegions = new List<int>();
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                if (this.meshBlocks[i].ITGtable.Length == 0) removeRegions.Add(i);
            }
            foreach (int i in removeRegions)
            {
                this.RemoveRegion(i);
            }
        }

        public void setInternalLink(TGI tgi)
        {
            this.publicKey = new TGI[] { tgi };
        }

        public int NumberRegions
        {
            get { return this.meshBlocks.Length; }
        }

        public void RemoveRegion(int index)
        {
            List<MeshBlock> tmp = new List<MeshBlock>();
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                if (i != index) tmp.Add(this.meshBlocks[i]);
            }
            this.meshBlocks = tmp.ToArray();
        }

        public int AddRegion(uint region, float layer, byte isReplacement, TGI[] meshTGIlist)
        {
            List<MeshBlock> tmp = new List<MeshBlock>(this.meshBlocks);
            tmp.Add(new MeshBlock(region, layer, isReplacement, meshTGIlist));
            this.meshBlocks = tmp.ToArray();
            return this.meshBlocks.Length - 1;
        }

        public uint GetRegion(int index)
        {
            return this.meshBlocks[index].region;
        }

        public float GetLayer(int index)
        {
            return this.meshBlocks[index].layer;
        }

        public byte GetIsReplacement(int index)
        {
            return this.meshBlocks[index].isReplacement;
        }

        public TGI[] GetTGIlist(int index)
        {
            return this.meshBlocks[index].ITGtable;
        }

        public void SetRegion(int index, uint region)
        {
            this.meshBlocks[index].region = region;
        }

        public void SetLayer(int index, float layer)
        {
            this.meshBlocks[index].layer = layer;
        }

        public void SetIsReplacement(int index, byte isReplacement)
        {
            this.meshBlocks[index].isReplacement = isReplacement;
        }

        public void SetTGIlist(int index, TGI[] meshTGIlist)
        {
            this.meshBlocks[index].ITGtable = meshTGIlist;
        }

        public void SetMeshRegions(TGI meshTGI, int[] regionList, float layer)
        {
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                if (Array.IndexOf(this.meshBlocks[i].ITGtable, meshTGI) >= 0 &&
                    ((Array.IndexOf(regionList, (int)this.meshBlocks[i].region) < 0) || (layer != this.meshBlocks[i].layer)))
                {
                    List<TGI> tmpTGI = new List<TGI>(this.meshBlocks[i].ITGtable);
                    tmpTGI.Remove(meshTGI);
                    this.meshBlocks[i].ITGtable = tmpTGI.ToArray();
                }
            }
            foreach (uint r in regionList)
            {
                bool wasFound = false;
                foreach (MeshBlock b in this.meshBlocks)
                {
                    if (r == b.region && layer == b.layer)
                    {
                        if (Array.IndexOf(b.ITGtable, meshTGI) < 0)
                        {
                            List<TGI> tmpTGI = new List<TGI>(b.ITGtable);
                            tmpTGI.Add(meshTGI);
                            b.ITGtable = tmpTGI.ToArray();
                        }
                        wasFound = true;
                        break;
                    }
                }
                if (!wasFound)
                {
                    List<MeshBlock> tmp = new List<MeshBlock>(this.meshBlocks);
                    tmp.Add(new MeshBlock(r, layer, 0, new TGI[] { meshTGI }));
                    this.meshBlocks = tmp.ToArray();
                }
            }
            List<int> removeRegions = new List<int>();
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                if (this.meshBlocks[i].ITGtable.Length == 0) removeRegions.Add(i);
            }
            foreach (int i in removeRegions)
            {
                this.RemoveRegion(i);
            }
        }

        public void SortLODs(CASP casp)
        {
            for (int i = 0; i < this.meshBlocks.Length; i++)
            {
                this.meshBlocks[i].SortLODs(casp);
            }
        }

        public RegionMap(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            contextVersion = br.ReadUInt32();
            publicKeyCount = br.ReadUInt32();
            externalKeyCount = br.ReadUInt32();
            delayLoadKeyCount = br.ReadUInt32();
            objectCount = br.ReadUInt32();
            publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++)
            {
                publicKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++)
            {
                externalKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            delayLoadKey = new TGI[delayLoadKeyCount];
            for (int i = 0; i < delayLoadKeyCount; i++)
            {
                delayLoadKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
            objData = new ObjectData[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                objData[i] = new ObjectData(br);
            }
            version = br.ReadInt32();
            int numMeshBlocks = br.ReadInt32();
            meshBlocks = new MeshBlock[numMeshBlocks];
            for (int i = 0; i < numMeshBlocks; i++)
            {
                meshBlocks[i] = new MeshBlock(br);
            }
        }

        public RegionMap(RegionMap regionMapToClone)
        {
            this.contextVersion = regionMapToClone.contextVersion;
            this.publicKeyCount = regionMapToClone.publicKeyCount;
            this.externalKeyCount = regionMapToClone.externalKeyCount;
            this.delayLoadKeyCount = regionMapToClone.delayLoadKeyCount;
            this.objectCount = regionMapToClone.objectCount;
            this.publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++)
            {
                this.publicKey[i] = new TGI(regionMapToClone.publicKey[i].Type, regionMapToClone.publicKey[i].Group, regionMapToClone.publicKey[i].Instance);
            }
            this.externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++)
            {
                this.externalKey[i] = new TGI(regionMapToClone.externalKey[i].Type, regionMapToClone.externalKey[i].Group, regionMapToClone.externalKey[i].Instance);
            }
            this.delayLoadKey = new TGI[delayLoadKeyCount];
            for (int i = 0; i < delayLoadKeyCount; i++)
            {
                this.delayLoadKey[i] = new TGI(regionMapToClone.delayLoadKey[i].Type, regionMapToClone.delayLoadKey[i].Group, regionMapToClone.delayLoadKey[i].Instance);
            }
            this.objData = new ObjectData[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                this.objData[i] = new ObjectData(regionMapToClone.objData[i]);
            }
            this.version = regionMapToClone.version;
            int numMeshBlocks = regionMapToClone.NumberRegions;
            this.meshBlocks = new MeshBlock[numMeshBlocks];
            for (int i = 0; i < numMeshBlocks; i++)
            {
                this.meshBlocks[i] = new MeshBlock(regionMapToClone.meshBlocks[i]);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(contextVersion);
            bw.Write(publicKeyCount);
            bw.Write(externalKeyCount);
            bw.Write(delayLoadKeyCount);
            bw.Write(objectCount);
            for (int i = 0; i < publicKeyCount; i++)
            {
                publicKey[i].Write(bw, TGI.TGIsequence.ITG);
            }
            for (int i = 0; i < objectCount; i++)
            {
                objData[i].Write(bw, this.MeshBlockDataLength);
            }
            bw.Write(version);
            bw.Write(meshBlocks.Length);
            for (int i = 0; i < meshBlocks.Length; i++)
            {
                meshBlocks[i].Write(bw);
            }
        }
    }

    internal class ObjectData
    {
        uint objPosition;              //absolute position of mesh data block, usually 44
        uint objLength;

        internal ObjectData(BinaryReader br)
        {
            this.objPosition = br.ReadUInt32();
            this.objLength = br.ReadUInt32();
        }

        internal ObjectData(ObjectData objectDataToClone)
        {
            this.objPosition = objectDataToClone.objPosition;
            this.objLength = objectDataToClone.objLength;
        }

        internal void Write(BinaryWriter bw, int meshBlocksSize)
        {
            bw.Write(this.objPosition);
            bw.Write(8 + meshBlocksSize);
        }
    }

    internal class MeshBlock        //Asset Pairing Info
    {
        internal uint region;
        internal float layer;
        internal byte isReplacement;         //true or false
        internal TGI[] ITGtable;

        internal int BlockSize { get { return 13 + (this.ITGtable.Length * 16); } }

        internal void SortLODs(CASP casp)
        {
            for (int i = this.ITGtable.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (casp.getLOD(this.ITGtable[j]) > casp.getLOD(this.ITGtable[j + 1]))
                    {
                        TGI tmp = new TGI(this.ITGtable[j]);
                        this.ITGtable[j] = new TGI(this.ITGtable[j + 1]);
                        this.ITGtable[j + 1] = tmp;
                    }
                }
            }
        }

        internal MeshBlock(uint region, float layer, byte isReplacement, TGI[] meshTGIlist)
        {
            this.region = region;
            this.layer = layer;
            this.isReplacement = isReplacement;
            this.ITGtable = meshTGIlist;
        }

        internal MeshBlock(BinaryReader br)
        {
            region = br.ReadUInt32();
            layer = br.ReadSingle();
            isReplacement = br.ReadByte();
            uint numKeys = br.ReadUInt32();
            ITGtable = new TGI[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                ITGtable[i] = new TGI(br, TGI.TGIsequence.ITG);
            }
        }

        internal MeshBlock(MeshBlock meshBlockToClone)
        {
            this.region = meshBlockToClone.region;
            this.layer = meshBlockToClone.layer;
            this.isReplacement = meshBlockToClone.isReplacement;
            this.ITGtable = new TGI[meshBlockToClone.ITGtable.Length];
            for (int i = 0; i < ITGtable.Length; i++)
            {
                this.ITGtable[i] = new TGI(meshBlockToClone.ITGtable[i].Type, meshBlockToClone.ITGtable[i].Group, meshBlockToClone.ITGtable[i].Instance);
            }
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(region);
            bw.Write(layer);
            bw.Write(isReplacement);
            bw.Write((uint)ITGtable.Length);
            for (int i = 0; i < ITGtable.Length; i++)
            {
                ITGtable[i].Write(bw, TGI.TGIsequence.ITG);
            }
        }
    }
}
