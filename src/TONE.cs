using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace TS4SimRipper
{
    public class TONE
    {
        uint version;                                  // currently 6, now 7, now 8, now 10 (0x0A), now 11 (0x0B)76
        SkinSetDesc[] skinSets;                         // v10 and up only
        OverlayDesc[] overlayList;
        UInt16 saturation;
        UInt16 hue;
        uint opacity;
        CASP.PartTag[] categoryTags;                  //Same as CASP flags
        Color[] colorList;
        float sortOrder;
        ulong tuningInstance;
        SkinPanel skinPanel;
        float sliderLow;
        float sliderHigh;
        float sliderIncrement;

        public uint Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public TONE.SkinSetDesc[] SkinSets
        {
            get { return new SkinSetDesc[] { new SkinSetDesc(this.skinSets[0]), new SkinSetDesc(this.skinSets[1]), new SkinSetDesc(this.skinSets[2]) }; }
            set { this.skinSets = new SkinSetDesc[] { new SkinSetDesc(value[0]), new SkinSetDesc(value[1]), new SkinSetDesc(value[2]) }; }
        }

        public ulong GetSkinSetTextureInstance(int index)
        {
            return this.skinSets[index].textureInstance;
        }

        public void SetSkinSetTextureInstance(int index, ulong instance)
        {
            this.skinSets[index].textureInstance = instance;
        }

        public ulong GetSkinSetOverlayInstance(int index)
        {
            return this.skinSets[index].overlayInstance;
        }

        public void SetSkinSetOverlayInstance(int index, ulong instance)
        {
            this.skinSets[index].overlayInstance = instance;
        }

        public List<OverlayDesc> OverlayList
        {
            get
            {
                List<OverlayDesc> overlays = new List<OverlayDesc>();
                for (int i = 0; i < this.NumberOverlays; i++)
                {
                    overlays.Add(new OverlayDesc(this.overlayList[i]));
                }
                return overlays;
            }
            set
            {
                List<OverlayDesc> overlays = new List<OverlayDesc>();
                for (int i = 0; i < value.Count; i++)
                {
                    overlays.Add(new OverlayDesc(value[i]));
                }
                this.overlayList = overlays.ToArray();
            }
        }
        public int NumberOverlays
        {
            get { return this.overlayList.Length; }
        }
        public AgeGender GetOverLayAge(int index)
        {
            return this.overlayList[index].flags;
        }
        public AgeGender GetOverLayGender(int index)
        {
            return this.overlayList[index].flags;
        }
        public ulong GetOverLayInstance(int index)
        {
            return this.overlayList[index].textureInstance;
        }
        public void SetOverLayInstance(int index, ulong instanceID)
        {
            this.overlayList[index].textureInstance = instanceID;
        }
        public ulong GetOverlayInstance(AgeGender ageGender)
        {
            for (int i = 0; i < this.NumberOverlays; i++)
            {
                if (this.overlayList[i].flags == ageGender) return this.overlayList[i].textureInstance;
            }
            return 0;
        }

        public UInt16 Hue
        {
            get { return this.hue; }
            set { this.hue = value; }
        }

        public UInt16 Saturation
        {
            get { return this.saturation; }
            set { this.saturation = value; }
        }

        public uint Opacity
        {
            get { return this.opacity; }
            set { this.opacity = value; }
        }

        public List<uint[]> CategoryTags
        {
            get
            {
                List<uint[]> tmp = new List<uint[]>();
                foreach (CASP.PartTag t in this.categoryTags)
                {
                    tmp.Add(new uint[] { t.FlagCategory, t.FlagValue });
                }
                return tmp;
            }
            set
            {
                CASP.PartTag[] tmp = new CASP.PartTag[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    tmp[i] = new CASP.PartTag((ushort)value[i][0], value[i][1]);
                }
                this.categoryTags = tmp;
            }
        }

        public Color[] ColorList
        {
            get
            {
                Color[] tmp = new Color[this.colorList.Length];
                Array.Copy(this.colorList, tmp, this.colorList.Length);
                return tmp;
            }
            set
            {
                Color[] tmp = new Color[value.Length];
                Array.Copy(value, tmp, value.Length);
                this.colorList = tmp;
            }
        }

        public float SortOrder
        {
            get { return this.sortOrder; }
            set { this.sortOrder = value; }
        }

        public ulong TuningInstance
        {
            get { return this.tuningInstance; }
            set { this.tuningInstance = value; }
        }

        public TONE()
        {
            this.version = 10;
            this.skinSets = new SkinSetDesc[3] { new TONE.SkinSetDesc(), new TONE.SkinSetDesc(), new TONE.SkinSetDesc() };
            this.overlayList = new OverlayDesc[0];
            this.saturation = 0;
            this.hue = 0;
            this.opacity = 0;
            this.categoryTags = new CASP.PartTag[0];
            this.colorList = new Color[1];
            this.colorList[0] = new Color();
            this.sortOrder = 100f;
            this.tuningInstance = 0x00000000000260A5U;          //human skintone tuning
            this.skinPanel = SkinPanel.Miscellaneous;
            this.sliderLow = -.05f;
            this.sliderHigh = .05f;
            this.sliderIncrement = .005f;
        }

        public TONE(uint version)
        {
            this.version = version;
            this.skinSets = new SkinSetDesc[] { new TONE.SkinSetDesc(), new TONE.SkinSetDesc(), new TONE.SkinSetDesc() };
            this.overlayList = new OverlayDesc[0];
            this.saturation = 0;
            this.hue = 0;
            this.opacity = 0;
            this.categoryTags = new CASP.PartTag[0];
            this.colorList = new Color[1];
            this.colorList[0] = new Color();
            this.sortOrder = 100f;
            this.tuningInstance = 0x00000000000260A5U;          //human skintone tuning
            this.skinPanel = SkinPanel.Miscellaneous;
            this.sliderLow = -.05f;
            this.sliderHigh = .05f;
            this.sliderIncrement = .005f;
        }

        public TONE(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.version = br.ReadUInt32();
            ulong tmpInstance = 0ul;
            float tmpOpacity = .5f, tmpOpacity2 = .5f;
            if (this.version >= 10)
            {
                byte count = br.ReadByte();
                List<SkinSetDesc> skinStateList = new List<SkinSetDesc>();
                for (int i = 0; i < count; i++) { skinStateList.Add(new SkinSetDesc(br)); }
                while (count < 3) { skinStateList.Add(new SkinSetDesc()); count++; }
                this.skinSets = skinStateList.ToArray();
            }
            else
            {
                this.skinSets = new SkinSetDesc[3];
                tmpInstance = br.ReadUInt64();
            }
            int overlayCount = br.ReadInt32();
            this.overlayList = new OverlayDesc[overlayCount];
            for (int i = 0; i < overlayCount; i++)
            {
                this.overlayList[i] = new OverlayDesc(br);
            }
            this.saturation = br.ReadUInt16();
            this.hue = br.ReadUInt16();
            this.opacity = br.ReadUInt32();
            int tagCount = br.ReadInt32();
            this.categoryTags = new CASP.PartTag[tagCount];
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i] = new CASP.PartTag(br, version >= 7 ? 4 : 2);
            }
            if (this.version < 10) tmpOpacity = br.ReadSingle();
            byte colorCount = br.ReadByte();
            this.colorList = new Color[colorCount];
            for (int i = 0; i < colorCount; i++)
            {
                this.colorList[i] = Color.FromArgb(br.ReadInt32());
            }
            this.sortOrder = br.ReadSingle();
            if (this.version < 10) tmpOpacity2 = br.ReadSingle();
            if (this.version >= 8)
            {
                this.tuningInstance = br.ReadUInt64();
            }
            if (this.version < 10)
            {
                this.skinSets[0] = new SkinSetDesc(tmpInstance, 0ul, 1f, tmpOpacity, tmpOpacity2);
                this.skinSets[1] = new SkinSetDesc(0ul, 0ul, 1f, tmpOpacity, tmpOpacity2);
                this.skinSets[2] = new SkinSetDesc(0ul, 0ul, 1f, tmpOpacity, tmpOpacity2);
            }
            if (this.version > 10)
            {
                this.skinPanel = (SkinPanel)br.ReadUInt16();
                this.sliderLow = br.ReadSingle();
                this.sliderHigh = br.ReadSingle();
                this.sliderIncrement = br.ReadSingle();
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.version);
            if (this.skinSets == null) this.skinSets = new SkinSetDesc[0];
            if (this.version >= 10)
            {
                bw.Write((byte)this.skinSets.Length);
                for (int i = 0; i < this.skinSets.Length; i++) { this.skinSets[i].Write(bw); }
            }
            else
            {
                bw.Write(this.skinSets[0].textureInstance);
            }
            bw.Write(this.overlayList.Length);
            for (int i = 0; i < overlayList.Length; i++)
            {
                this.overlayList[i].Write(bw);
            }
            bw.Write(this.saturation);
            bw.Write(this.hue);
            bw.Write(this.opacity);
            if (this.categoryTags == null) this.categoryTags = new CASP.PartTag[0];
            bw.Write(categoryTags.Length);
            for (int i = 0; i < categoryTags.Length; i++)
            {
                categoryTags[i].Write(bw, version >= 7 ? 4 : 2);
            }
            if (this.version < 10) bw.Write(this.skinSets[0].MakeupOpacity);
            if (this.colorList == null) this.colorList = new Color[0];
            bw.Write((byte)colorList.Length);
            for (int i = 0; i < colorList.Length; i++)
            {
                bw.Write(this.colorList[i].ToArgb());
            }
            bw.Write(this.sortOrder);
            if (this.version < 10) bw.Write(this.skinSets[0].MakeupOpacity2);
            if (this.version >= 8)
            {
                bw.Write(this.tuningInstance);
            }
            if (this.version > 10)
            {
                bw.Write((ushort)this.skinPanel);
                bw.Write(this.sliderLow);
                bw.Write(this.sliderHigh);
                bw.Write(this.sliderIncrement);
            }
        }

        public class SkinSetDesc
        {
            internal ulong textureInstance;
            internal ulong overlayInstance;
            float overlayMultiplier, makeupOpacity, makeupOpacity2;

            public ulong TextureInstance { get { return this.textureInstance; } set { this.textureInstance = value; } }
            public ulong OverlayInstance { get { return this.overlayInstance; } set { this.overlayInstance = value; } }
            public float OverlayMultiplier { get { return this.overlayMultiplier; } set { this.overlayMultiplier = value; } }
            public float MakeupOpacity { get { return this.makeupOpacity; } set { this.makeupOpacity = value; } }
            public float MakeupOpacity2 { get { return this.makeupOpacity2; } set { this.makeupOpacity2 = value; } }

            public SkinSetDesc()
            {
                this.textureInstance = 0ul;
                this.overlayInstance = 0ul;
                this.overlayMultiplier = 1f;
                this.makeupOpacity = 0.5f;
                this.makeupOpacity2 = 0.5f;
            }
            public SkinSetDesc(ulong textureInstance, ulong overlayInstance, float overlayMultiplier, float makeupOpacity, float makeupOpacity2)
            {
                this.textureInstance = textureInstance;
                this.overlayInstance = overlayInstance;
                this.overlayMultiplier = overlayMultiplier;
                this.makeupOpacity = makeupOpacity;
                this.makeupOpacity2 = makeupOpacity2;
            }
            public SkinSetDesc(SkinSetDesc other)
            {
                this.textureInstance = other.textureInstance;
                this.overlayInstance = other.overlayInstance;
                this.overlayMultiplier = other.overlayMultiplier;
                this.makeupOpacity = other.makeupOpacity;
                this.makeupOpacity2 = other.makeupOpacity2;
            }
            internal SkinSetDesc(BinaryReader br)
            {
                this.textureInstance = br.ReadUInt64();
                this.overlayInstance = br.ReadUInt64();
                this.overlayMultiplier = br.ReadSingle();
                this.makeupOpacity = br.ReadSingle();
                this.makeupOpacity2 = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.textureInstance);
                bw.Write(this.overlayInstance);
                bw.Write(this.overlayMultiplier);
                bw.Write(this.makeupOpacity);
                bw.Write(this.makeupOpacity2);
            }
        }

        public class OverlayDesc
        {
            internal AgeGender flags;
            internal ulong textureInstance;
            internal OverlayDesc(OverlayDesc other)
            {
                this.flags = other.flags;
                this.textureInstance = other.textureInstance;
            }
            internal OverlayDesc(BinaryReader br)
            {
                this.flags = (AgeGender)br.ReadUInt32();
                this.textureInstance = br.ReadUInt64();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write((uint)this.flags);
                bw.Write(this.textureInstance);
            }
        }

        public enum SkinPanel : ushort
        {
            Warm = 1,
            Neutral = 2,
            Cool = 3,
            Miscellaneous = 4
        }
    }
}
