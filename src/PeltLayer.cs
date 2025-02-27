using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TS4SimRipper
{
    public class PeltLayer
    {
        uint version;
        uint unknown;
        float sortOrder;
        byte usage;
        ulong linkedPeltLayer;
        uint nameKey;
        ulong textureKey;
        ulong thumbnailKey;
        CASP.PartTag[] categoryTags;

        public ulong TextureKey { get { return this.textureKey; } }

        public byte[] BH_unknown { get; }

        public PeltLayer(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.version = br.ReadUInt32();
            this.unknown = br.ReadUInt32();
            this.sortOrder = br.ReadSingle();
            this.usage = br.ReadByte();
            if (version > 5)
            {
                this.linkedPeltLayer = br.ReadUInt64();
            }
            this.nameKey = br.ReadUInt32();
            if(this.version >= 8){
                this.BH_unknown = br.ReadBytes(5);
            }
            this.textureKey = br.ReadUInt64();
            this.thumbnailKey = br.ReadUInt64();
            uint tagCount = br.ReadUInt32();
            categoryTags = new CASP.PartTag[tagCount];
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i] = new CASP.PartTag(br, 4);
            }
        }
    }
}
