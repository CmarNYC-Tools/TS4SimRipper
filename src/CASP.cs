using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TS4SimRipper
{
    public class CASP       //Sims 4 CASP resource
    {
        uint version;	// 0x2C is current
        uint offset;	// to resource reference table from end of header (ie offset + 8)
        int presetCount; // Not used for TS4
        string partname;		// UnicodeBE - part name
        float sortPriority;	// CAS sorts on this value, from largest to smallest

        ushort swatchOrder;   // mSecondaryDisplayIndex - swatch order
        uint outfitID;    // Used to group CASPs
        uint materialHash;
        byte parameterFlags;  //parameter flags: 
        // 1 bit RestrictOppositeGender
        // 1 bit AllowForLiveRandom
        // 1 bit Show in CAS Demo
        // 1 bit ShowInSimInfoPanel
        // 1 bit ShowInUI
        // 1 bit AllowForCASRandom : 1;
        // 1 bit DefaultThumbnailPart
        // 1 bit Deprecated
        byte parameterFlags2; // additional parameter flags:
        // 5 bits unused
        // 1 bit DefaultForBodyTypeFemale
        // 1 bit DefaultForBodyTypeMale
        // 1 bit RestrictOppositeFrame
        ulong excludePartFlags; // parts removed
        ulong excludePartFlags2;         // v0x29
        ulong excludeModifierRegionFlags;
        int tagCount;
        PartTag[] categoryTags; // [tagCount] PartTags

        uint price;             //deprecated
        uint titleKey;
        uint partDescKey;
        uint unknown;         // added in v 0x2B
        byte textureSpace;
        uint bodyType;
        uint bodySubType;    // usually 8, not used before version 37
        AgeGender ageGender;
        uint species;          // added in version 0x20
        ushort packID;       // added in version 0x25
        byte packFlags;      // added in version 0x25
        // bit 7 - reserved, set to 0
        // bit 1 - hide pack icon
        byte[] Reserved2Set0;  // added in version 0x25, nine bytes set to 0
        byte Unused2;        //usually 1
        byte Unused3;        //if Unused2 > 0; usually 0
        byte usedColorCount;
        uint[] colorData;    // [usedColorCount] color code
        byte buffResKey;     // index to data file with custom icon and text info
        byte swatchIndex;    // index to swatch image
        ulong VoiceEffect;   // added in version 0x1C -  mVoiceEffectHash, a hash of a sound effect
        byte usedMaterialCount;       // added in version 0x1e - if not 0, should be 3
        uint materialSetUpperBodyHash;       // added in version 0x1e
        uint materialSetLowerBodyHash;       // added in version 0x1e
        uint materialSetShoesHash;       // added in version 0x1e 
        uint occultBitField;            // added in version 0x1f - disabled for occult types
        // 30 bits reserved
        //  1 bit alien
        //  1 bit human
        ulong unknown1;                 // Version 0x2E
        UInt64 oppositeGenderPart;      // Version 0x28 - If the current part is not compatible with the Sim due to frame/gender
        // restrictions, use this part instead. Maxis convention is to use this
        // to specify the opposite gender version of the part. Set to 0 for none.
        UInt64 fallbackPart;            // Version 0x28 - If the current part is not compatible with the Sim due to frame/gender
                                        // restrictions, and there is no mOppositeGenderPart specified, use this part.
                                        // Maxis convention is to use this to specify a replacement part which is not
                                        // necessarily the opposite gendered version of the part. Set to 0 for none.
        OpacitySettings opacitySlider;     //V 0x2C
        SliderSettings hueSlider;           // "
        SliderSettings saturationSlider;    // "
        SliderSettings brightnessSlider;    // "
        byte unknownCount;                      //Version 0x2E
        byte[] unknown2;                    //Version 0x2E - unknownCount bytes
        byte nakedKey;
        byte parentKey;
        int sortLayer;
        byte lodCount;
        MeshDesc[] lods;      // [count] mesh lod and part indexes 
        byte numSlotKeys;
        byte[] slotKeys;      // [numSlotKeys] bytes
        byte textureIndex;    // index to texture TGI (diffuse)
        byte shadowIndex;     // index to 'shadow' texture/overlay
        byte compositionMethod;
        byte regionMapIndex;  // index to RegionMap file
        byte numOverrides;
        Override[] overrides; // [numOverrides] Override
        byte normalMapIndex;
        byte specularIndex;   // DDSRLES 
        uint UVoverride;      //added in version 0x1b, so far same values as bodyType
        byte emissionIndex;   // added in version 0x1d, for alien glow 
        byte reserved;        // added in version 0x2A
        byte reserved2;        // added in version 49
        byte IGTcount;        // Resource reference table in I64GT format (not TGI64)
        // --repeat(count)
        TGI[] IGTtable;

        public TGI tgi;
        public bool notBaseGame;
        public string package;

        public string PartName
        {
            get { return this.partname; }
            set { this.partname = value; }
        }
        public float SortPriority
        {
            get { return this.sortPriority; }
            set { this.sortPriority = value; }
        }
        public ushort SwatchOrder
        {
            get { return this.swatchOrder; }
            set { this.swatchOrder = value; }
        }
        public byte CASparameterFlags
        {
            get { return this.parameterFlags; }
            set { this.parameterFlags = value; }
        }
        public byte CASparameterFlags2
        {
            get { return this.parameterFlags2; }
            set { this.parameterFlags2 = value; }
        }
        public ulong ExcludePartFlags
        {
            get { return this.excludePartFlags; }
            set { this.excludePartFlags = value; }
        }
        public ulong ExcludePartFlags2
        {
            get { return this.excludePartFlags2; }
            set { this.excludePartFlags2 = value; }
        }
        public ulong ExcludeModifierRegionFlags
        {
            get { return this.excludeModifierRegionFlags; }
            set { this.excludeModifierRegionFlags = value; }
        }
        public BodyType BodyType
        {
            get { return (BodyType)this.bodyType; }
            set { this.bodyType = (uint)value; }
        }
        public uint BodyTypeNumeric
        {
            get { return this.bodyType; }
            set { this.bodyType = value; this.UVoverride = value; }
        }
        public BodyType SharedUVSpace
        {
            get { return (BodyType)this.UVoverride; }
            set { this.UVoverride = (uint)value; }
        }
        public uint SharedUVNumeric
        {
            get { return this.UVoverride; }
            set { this.UVoverride = value; }
        }
        public uint BodySubTypeNumeric
        {
            get { return this.bodySubType; }
            set { this.bodySubType = value; }
        }
        public List<uint[]> CategoryTags
        {
            get
            {
                List<uint[]> tmp = new List<uint[]>();
                foreach (PartTag t in this.categoryTags)
                {
                    tmp.Add(new uint[] { t.FlagCategory, t.FlagValue });
                }
                return tmp;
            }
            set
            {
                PartTag[] tmp = new PartTag[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    tmp[i] = new PartTag((ushort)value[i][0], value[i][1]);
                }
                this.categoryTags = tmp;
                this.tagCount = value.Count;
            }
        }

        public Species Species
        {
            get { return (Species)this.species; }
            set { this.species = (uint)value; }
        }
        public int SpeciesNumeric
        {
            get { return (int)this.species; }
        }

        public AgeGender age
        {
            get { return (AgeGender)((uint)this.ageGender & 0xFF); }
        }
        public AgeGender gender
        {
            get { return (AgeGender)((uint)this.ageGender & 0xFF00); }
        }

        public uint PackID
        {
            get { return this.packID; }
        }

        public bool DisallowOppositeGender
        {
            get { return (this.parameterFlags & (byte)CASParamFlag.RestrictOppositeGender) > 0;  }
        }
        public bool DisallowOppositeFrame
        {
            get { return (this.parameterFlags2 & (byte)CASParamFlag2.RestrictOppositeFrame) > 0; }
        }

        public bool HasMesh
        {
            get { return this.lods != null && this.lods.Length > 0; }
        }

        public TGI[] MeshParts(int LOD)
        {
            List<TGI> tgi = new List<TGI>();
            foreach (MeshDesc m in lods)
            {
                if (m.lod == LOD)
                {
                    for (int i = 0; i < m.indexes.Length; i++)
                    {
                        tgi.Add(IGTtable[m.indexes[i]]);
                    }
                }
            }
            return tgi.ToArray();
        }

        public int getLOD(TGI tgi)
        {
            for (int i = 0; i < lods.Length; i++)
            {
                for (int j = 0; j < lods[i].indexes.Length; j++)
                {
                    if (IGTtable[lods[i].indexes[j]].Equals(tgi))
                    {
                        return lods[i].lod;
                    }
                }
            }
            return -1;
        }

        public int[] getLODandPart(TGI tgi)
        {
            for (int i = 0; i < lods.Length; i++)
            {
                for (int j = 0; j < lods[i].indexes.Length; j++)
                {
                    if (IGTtable[lods[i].indexes[j]].Equals(tgi))
                    {
                        return new int[] { lods[i].lod, j };
                    }
                }
            }
            return null;
        }

        public bool MultipleMeshParts
        {
            get
            {
                for (int i = 0; i < lods.Length; i++)
                {
                    if (this.lods[i].indexes.Length > 1) return true;
                }
                return false;
            }
        }

        public void RemoveMeshLink(TGI meshTGI)
        {
            foreach (MeshDesc md in this.lods)
            {
                md.removeMeshLink(meshTGI, this.LinkList);
            }
        }

        public void AddMeshLink(TGI meshTGI, int lod)
        {
            foreach (MeshDesc md in this.lods)
            {
                if (md.lod == lod)
                {
                    md.addMeshLink(meshTGI, this.LinkList);
                }
            }
        }

        public uint[] ColorList
        {
            get
            {
                uint[] tmp = new uint[this.colorData.Length];
                Array.Copy(this.colorData, tmp, this.colorData.Length);
                return tmp;
            }
            set
            {
                uint[] tmp = new uint[value.Length];
                Array.Copy(value, tmp, value.Length);
                this.colorData = tmp;
                usedColorCount = (byte)value.Length;
            }
        }

        public int CompositionMethod
        {
            get
            {
                return (int)this.compositionMethod;
            }
            set
            {
                this.compositionMethod = (byte)value;
            }
        }

        public int SortLayer
        {
            get
            {
                return this.sortLayer;
            }
            set
            {
                this.sortLayer = value;
            }
        }

        public TGI[] LinkList
        {
            get { return IGTtable; }
            set { IGTtable = value; }
        }
        public void setLink(int Index, TGI tgi)
        {
            this.IGTtable[Index] = new TGI(tgi);
        }
        /// <summary>
        /// Adds a TGI to the TGI list
        /// </summary>
        /// <param name="tgi">tgi to add</param>
        /// <returns>Index of added TGI</returns>
        public byte addLink(TGI tgi)
        {
            List<TGI> newLinks = new List<TGI>(this.IGTtable);
            newLinks.Add(tgi);
            this.IGTtable = newLinks.ToArray();
            return (byte)(this.IGTtable.Length - 1);
        }
        public bool replaceLink(TGI oldtgi, TGI newtgi)
        {
            for (int i = 0; i < this.IGTtable.Length; i++)
            {
                if (IGTtable[i].Equals(oldtgi))
                {
                    IGTtable[i] = new TGI(newtgi);
                    return true;
                }
            }
            return false;
        }
        public int EmptyLink
        {
            get
            {
                TGI empty = new TGI(0, 0, 0);
                for (int i = 0; i < this.LinkList.Length; i++)
                {
                    if (this.LinkList[i].Equals(empty)) return i;
                }
                return -1;
            }
        }

        public byte BuffResKey
        {
            get { return this.buffResKey; }
            set { this.buffResKey = (byte)value; }
        }
        public byte SwatchIndex
        {
            get { return this.swatchIndex; }
            set { this.swatchIndex = (byte)value; }
        }
        public byte NakedKey
        {
            get { return this.nakedKey; }
            set { this.nakedKey = (byte)value; }
        }
        public byte ParentKey
        {
            get { return this.parentKey; }
            set { this.parentKey = (byte)value; }
        }
        public byte[] SlotKeys
        {
            get { return this.slotKeys; }
            set { this.slotKeys = value; }
        }
        public byte TextureIndex
        {
            get { return this.textureIndex; }
            set { this.textureIndex = (byte)value; }
        }
        public byte ShadowIndex
        {
            get { return this.shadowIndex; }
            set { this.shadowIndex = (byte)value; }
        }
        public byte RegionMapIndex
        {
            get { return this.regionMapIndex; }
            set { this.regionMapIndex = (byte)value; }
        }
        public byte NormalMapIndex
        {
            get { return this.normalMapIndex; }
            set { this.normalMapIndex = (byte)value; }
        }
        public byte SpecularIndex
        {
            get { return this.specularIndex; }
            set { this.specularIndex = (byte)value; }
        }
        public byte EmissionIndex
        {
            get { if (this.version >= 30) { return this.emissionIndex; } else { return (byte)this.EmptyLink; } }
            set { this.emissionIndex = (byte)value; }
        }

        public void RemoveSpecular()
        {
            this.specularIndex = RemoveKey(this.specularIndex);
            this.RebuildLinkList();
        }
        public void RemoveEmission()
        {
            this.emissionIndex = RemoveKey(this.emissionIndex);
            this.RebuildLinkList();
        }

        public uint OutfitID
        {
            get { return this.outfitID; }
            set { this.outfitID = value; }
        }

        public UInt64 OppositeGenderPart
        {
            get { return this.oppositeGenderPart; }
            set { this.oppositeGenderPart = value; }
        }
        public UInt64 FallbackPart
        {
            get { return this.fallbackPart; }
            set { this.fallbackPart = value; }
        }

        public CASP(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            if (br.BaseStream.Length < 32) throw new CASPEmptyException("Attempt to read empty CASP");
            version = br.ReadUInt32();
            offset = br.ReadUInt32();
            presetCount = br.ReadInt32();
            partname = new BinaryReader(br.BaseStream, Encoding.BigEndianUnicode).ReadString();
            sortPriority = br.ReadSingle();
            swatchOrder = br.ReadUInt16();
            outfitID = br.ReadUInt32();
            materialHash = br.ReadUInt32();
            parameterFlags = br.ReadByte();
            if (this.version >= 39) parameterFlags2 = br.ReadByte();

            if (this.version >= 50)
            {
                this.LayerID = br.ReadUInt16();
            }
            excludePartFlags = br.ReadUInt64();
            if (version >= 41)
            {
                excludePartFlags2 = br.ReadUInt64();
            }
            if (version > 36)
            {
                excludeModifierRegionFlags = br.ReadUInt64();
            }
            else
            {
                excludeModifierRegionFlags = br.ReadUInt32();
            }
            tagCount = br.ReadInt32();
            categoryTags = new PartTag[tagCount];
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i] = new PartTag(br, version >= 37 ? 4 : 2);
            }
            price = br.ReadUInt32();
            titleKey = br.ReadUInt32();
            partDescKey = br.ReadUInt32();
            if (version >= 0x2B) unknown = br.ReadUInt32();
            textureSpace = br.ReadByte();
            bodyType = br.ReadUInt32();
            bodySubType = br.ReadUInt32();
            ageGender = (AgeGender)br.ReadUInt32();
            if (this.version >= 32) species = br.ReadUInt32();
            if (this.version >= 34)
            {
                packID = br.ReadUInt16();
                packFlags = br.ReadByte();
                Reserved2Set0 = br.ReadBytes(9);
            }
            else
            {
                Unused2 = br.ReadByte();
                if (Unused2 > 0) Unused3 = br.ReadByte();
            }
            usedColorCount = br.ReadByte();
            colorData = new uint[usedColorCount];
            for (int i = 0; i < usedColorCount; i++)
            {
                colorData[i] = br.ReadUInt32();
            }
            buffResKey = br.ReadByte();
            swatchIndex = br.ReadByte();
            if (version >= 28)
            {
                VoiceEffect = br.ReadUInt64();
            }
            if (version >= 30)
            {
                usedMaterialCount = br.ReadByte();
                if (usedMaterialCount > 0)
                {
                    materialSetUpperBodyHash = br.ReadUInt32();
                    materialSetLowerBodyHash = br.ReadUInt32();
                    materialSetShoesHash = br.ReadUInt32();
                }
            }
            if (version >= 31)
            {
                occultBitField = br.ReadUInt32();
            }
            if (version >= 0x2E)
            {
                unknown1 = br.ReadUInt64();
            }
            if (version >= 38)
            {
                oppositeGenderPart = br.ReadUInt64();
            }
            if (version >= 39)
            {
                fallbackPart = br.ReadUInt64();
            }
            if (version >= 44)
            {
                opacitySlider = new OpacitySettings(br);
                hueSlider = new SliderSettings(br);
                saturationSlider = new SliderSettings(br);
                brightnessSlider = new SliderSettings(br);
            }
            if (version >= 0x2E)
            {
                unknownCount = br.ReadByte();
                unknown2 = br.ReadBytes(unknownCount);
            }
            nakedKey = br.ReadByte();
            parentKey = br.ReadByte();
            sortLayer = br.ReadInt32();
            lodCount = br.ReadByte();
            lods = new MeshDesc[lodCount];
            for (int i = 0; i < lodCount; i++)
            {
                lods[i] = new MeshDesc(br);
            }
            numSlotKeys = br.ReadByte();
            slotKeys = br.ReadBytes(numSlotKeys);
            textureIndex = br.ReadByte();
            shadowIndex = br.ReadByte();
            compositionMethod = br.ReadByte();
            regionMapIndex = br.ReadByte();
            numOverrides = br.ReadByte();
            overrides = new Override[numOverrides];
            for (int i = 0; i < numOverrides; i++)
            {
                overrides[i] = new Override(br);
            }
            normalMapIndex = br.ReadByte();
            specularIndex = br.ReadByte();
            if (version >= 27)
            {
                UVoverride = br.ReadUInt32();
            }
            if (version >= 29)
            {
                emissionIndex = br.ReadByte();
            }
            if (version >= 42)
            {
                reserved = br.ReadByte();
            }
            if (version >= 49)
            {
                reserved2 = br.ReadByte();
            }
            IGTcount = br.ReadByte();
            IGTtable = new TGI[IGTcount];
            for (int i = 0; i < IGTcount; i++)
            {
                IGTtable[i] = new TGI(br, TGI.TGIsequence.IGT);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(version);
            long offsetPos = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(presetCount);
            new BinaryWriter(bw.BaseStream, Encoding.BigEndianUnicode).Write(partname);
            bw.Write(sortPriority);
            bw.Write(swatchOrder);
            bw.Write(outfitID);
            bw.Write(materialHash);
            bw.Write(parameterFlags);
            if (this.version >= 39) bw.Write(parameterFlags2);
            if(this.version >= 50){
                bw.Write(this.LayerID);
            }
            bw.Write(excludePartFlags);
            if (version >= 41)
            {
                bw.Write(excludePartFlags2);
            }
            if (version > 36)
            {
                bw.Write(excludeModifierRegionFlags);
            }
            else
            {
                bw.Write((uint)excludeModifierRegionFlags);
            }
            bw.Write(tagCount);
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i].Write(bw, version >= 37 ? 4 : 2);
            }
            bw.Write(price);
            bw.Write(titleKey);
            bw.Write(partDescKey);
            if (version >= 0x2B) bw.Write(unknown);
            bw.Write(textureSpace);
            bw.Write(bodyType);
            bw.Write(bodySubType);
            bw.Write((uint)this.ageGender);
            if (this.version >= 32) bw.Write(species);
            if (this.version >= 34)
            {
                bw.Write(packID);
                bw.Write(packFlags);
                bw.Write(Reserved2Set0);
            }
            else
            {
                bw.Write(Unused2);
                if (Unused2 > 0) bw.Write(Unused3);
            }
            bw.Write(usedColorCount);
            for (int i = 0; i < usedColorCount; i++)
            {
                bw.Write(colorData[i]);
            }
            bw.Write(buffResKey);
            bw.Write(swatchIndex);
            if (version >= 28)
            {
                bw.Write(VoiceEffect);
            }
            if (version >= 30)
            {
                bw.Write(usedMaterialCount);
                if (usedMaterialCount > 0)
                {
                    bw.Write(materialSetUpperBodyHash);
                    bw.Write(materialSetLowerBodyHash);
                    bw.Write(materialSetShoesHash);
                }
            }
            if (version >= 31)
            {
                bw.Write(occultBitField);
            }
            if (version >= 0x2E)
            {
                bw.Write(unknown1);
            }
            if (version >= 38)
            {
                bw.Write(oppositeGenderPart);
            }
            if (version >= 39)
            {
                bw.Write(fallbackPart);
            }
            if (version >= 44)
            {
                opacitySlider.Write(bw);
                hueSlider.Write(bw);
                saturationSlider.Write(bw);
                brightnessSlider.Write(bw);
            }
            if (version >= 0x2E)
            {
                bw.Write((byte)unknown2.Length);
                if (unknown2.Length > 0) bw.Write(unknown2);
            }
            bw.Write(nakedKey);
            bw.Write(parentKey);
            bw.Write(sortLayer);
            bw.Write(lodCount);
            for (int i = 0; i < lodCount; i++)
            {
                lods[i].Write(bw);
            }
            bw.Write(numSlotKeys);
            bw.Write(slotKeys);
            bw.Write(textureIndex);
            bw.Write(shadowIndex);
            bw.Write(compositionMethod);
            bw.Write(regionMapIndex);
            bw.Write(numOverrides);
            for (int i = 0; i < numOverrides; i++)
            {
                overrides[i].Write(bw);
            }
            bw.Write(normalMapIndex);
            bw.Write(specularIndex);
            if (version >= 27)
            {
                bw.Write(UVoverride);
            }
            if (version >= 29)
            {
                bw.Write(emissionIndex);
            }
            if (version >= 42)
            {
                bw.Write(reserved);
            }
            if (version >= 49)
            {
                bw.Write(reserved2);
            }
            long tablePos = bw.BaseStream.Position;
            bw.BaseStream.Position = offsetPos;
            bw.Write((uint)(tablePos - 8));
            bw.BaseStream.Position = tablePos;
            bw.Write((byte)IGTtable.Length);
            for (int i = 0; i < IGTtable.Length; i++)
            {
                IGTtable[i].Write(bw, TGI.TGIsequence.IGT);
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

        public ushort LayerID { get; private set; }

        internal class PartTag
        {
            ushort flagCategory;
            uint flagValue;

            internal ushort FlagCategory
            {
                get { return this.flagCategory; }
                set { this.flagCategory = value; }
            }
            internal uint FlagValue
            {
                get { return this.flagValue; }
                set { this.flagValue = value; }
            }

            internal PartTag(BinaryReader br, int valueLength)
            {
                flagCategory = br.ReadUInt16();
                if (valueLength == 4)
                {
                    flagValue = br.ReadUInt32();
                }
                else
                {
                    flagValue = br.ReadUInt16();
                }
            }

            internal PartTag(ushort category, uint flagVal)
            {
                this.flagCategory = category;
                this.flagValue = flagVal;
            }

            internal void Write(BinaryWriter bw, int valueLength)
            {
                bw.Write(flagCategory);
                if (valueLength == 4)
                {
                    bw.Write(flagValue);
                }
                else
                {
                    bw.Write((ushort)flagValue);
                }
            }
        }

        internal class Override
        {
            byte region;
            float layer;

            internal Override(BinaryReader br)
            {
                region = br.ReadByte();
                layer = br.ReadSingle();
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(region);
                bw.Write(layer);
            }
        }

        internal class MeshDesc
        {
            internal byte lod;
            internal uint Unused1;
            LODasset[] assets;
            internal byte[] indexes;

            internal int Length
            {
                get
                {
                    return 7 + (12 * assets.Length) + indexes.Length;
                }
            }

            internal void removeMeshLink(TGI meshTGI, TGI[] caspTGIlist)
            {
                List<byte> tmp = new List<byte>();
                foreach (byte i in indexes)
                {
                    if (!meshTGI.Equals(caspTGIlist[i])) tmp.Add(i);
                }
                this.indexes = tmp.ToArray();
            }

            internal void addMeshLink(TGI meshTGI, TGI[] caspTGIlist)
            {
                List<byte> tmp = new List<byte>(this.indexes);
                for (byte i = 0; i < caspTGIlist.Length; i++)
                {
                    if (meshTGI.Equals(caspTGIlist[i]))
                    {
                        tmp.Add(i);
                        break;
                    }
                }
                this.indexes = tmp.ToArray();
            }

            internal MeshDesc(BinaryReader br)
            {
                lod = br.ReadByte();
                Unused1 = br.ReadUInt32();
                byte numAssets = br.ReadByte();
                assets = new LODasset[numAssets];
                for (int i = 0; i < numAssets; i++)
                {
                    assets[i] = new LODasset(br);
                }
                byte indexCount = br.ReadByte();
                indexes = new byte[indexCount];
                for (int i = 0; i < indexCount; i++)
                {
                    indexes[i] = br.ReadByte();
                }
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(lod);
                bw.Write(Unused1);
                bw.Write((byte)assets.Length);
                for (int i = 0; i < assets.Length; i++)
                {
                    assets[i].Write(bw);
                }
                bw.Write((byte)indexes.Length);
                for (int i = 0; i < indexes.Length; i++)
                {
                    bw.Write(indexes[i]);
                }
            }

            internal class LODasset
            {
                internal int sorting;
                internal int specLevel;
                internal int castShadow;

                internal LODasset(BinaryReader br)
                {
                    this.sorting = br.ReadInt32();
                    this.specLevel = br.ReadInt32();
                    this.castShadow = br.ReadInt32();
                }

                internal void Write(BinaryWriter bw)
                {
                    bw.Write(this.sorting);
                    bw.Write(this.specLevel);
                    bw.Write(this.castShadow);
                }
            }
        }

        public class OpacitySettings
        {
            internal float minimum;
            internal float increment;

            public float Minimum
            {
                get { return this.minimum; }
                set { this.minimum = value; }
            }
            public float Increment
            {
                get { return this.increment; }
                set { this.increment = value; }
            }

            public OpacitySettings()
            {
                this.minimum = .2f;
                this.increment = .05f;
            }

            public OpacitySettings(float minimum, float increment)
            {
                this.minimum = minimum;
                this.increment = increment;
            }

            internal OpacitySettings(BinaryReader br)
            {
                this.minimum = br.ReadSingle();
                this.increment = br.ReadSingle();
            }

            internal virtual void Write(BinaryWriter bw)
            {
                bw.Write(this.minimum);
                bw.Write(this.increment);
            }
        }

        public class SliderSettings : OpacitySettings
        {
            internal float maximum;

            public float Maximum
            {
                get { return this.maximum; }
                set { this.maximum = value; }
            }

            public SliderSettings()
            {
                this.minimum = -.5f;
                this.maximum = .5f;
                this.increment = .05f;
            }

            public SliderSettings(float minimum, float maximum, float increment)
            {
                this.minimum = minimum;
                this.maximum = maximum;
                this.increment = increment;
            }

            internal SliderSettings(BinaryReader br)
            {
                this.minimum = br.ReadSingle();
                this.maximum = br.ReadSingle();
                this.increment = br.ReadSingle();
            }

            internal override void Write(BinaryWriter bw)
            {
                bw.Write(this.minimum);
                bw.Write(this.maximum);
                bw.Write(this.increment);
            }
        }

        internal byte RemoveKey(byte index)
        {
            TGI empty_tgi = new TGI(0, 0, 0);
            if (this.IGTtable[index].Equals(empty_tgi) || index < 0 || index > this.IGTtable.Length - 1) return index;  //if not needed or invalid index, don't do anything.
            byte empty_index = 0;
            bool found = false;
            for (int i = 0; i < this.IGTtable.Length; i++)
            {
                if (this.IGTtable[i].Equals(empty_tgi))
                {
                    empty_index = (byte)i;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return empty_index;
            }
            else
            {
                TGI[] newTGIlist = new TGI[this.IGTtable.Length + 1];
                Array.Copy(this.IGTtable, newTGIlist, this.IGTtable.Length);
                newTGIlist[this.IGTtable.Length] = empty_tgi;
                this.IGTtable = newTGIlist;
                return (byte)(this.IGTtable.Length - 1);
            }
        }

        public void RebuildLinkList()
        {
            List<TGI> newLinks = new List<TGI>();
            newLinks.Add(new TGI(0, 0, 0));

            this.buffResKey = AddLink(this.buffResKey, newLinks);
            this.swatchIndex = AddLink(this.swatchIndex, newLinks);
            this.nakedKey = AddLink(this.nakedKey, newLinks);
            this.parentKey = AddLink(this.parentKey, newLinks);
            for (int i = 0; i < this.lodCount; i++)
            {
                for (int j = 0; j < this.lods[i].indexes.Length; j++)
                {
                    this.lods[i].indexes[j] = AddLink(this.lods[i].indexes[j], newLinks);
                }
            }
            for (int i = 0; i < this.numSlotKeys; i++)
            {
                this.slotKeys[i] = AddLink(this.slotKeys[i], newLinks);
            }
            this.textureIndex = AddLink(this.textureIndex, newLinks);
            this.shadowIndex = AddLink(this.shadowIndex, newLinks);
            this.regionMapIndex = AddLink(this.regionMapIndex, newLinks);
            this.normalMapIndex = AddLink(this.normalMapIndex, newLinks);
            this.specularIndex = AddLink(this.specularIndex, newLinks);
            this.emissionIndex = AddLink(this.emissionIndex, newLinks);
            this.IGTtable = newLinks.ToArray();
        }

        internal byte AddLink(byte indexIn, List<TGI> linkList)
        {
            if (this.IGTtable[indexIn].Equals(new TGI(0, 0, 0)) | indexIn < 0 | indexIn > IGTtable.Length) // correct invalid links
            {
                return (byte)0;
            }
            else
            {
                byte tmp = (byte)linkList.Count;
                linkList.Add(this.IGTtable[indexIn]);
                return tmp;
            }
        }

        [global::System.Serializable]
        public class CASPEmptyException : ApplicationException
        {
            public CASPEmptyException() { }
            public CASPEmptyException(string message) : base(message) { }
            public CASPEmptyException(string message, Exception inner) : base(message, inner) { }
            protected CASPEmptyException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

    }
}
