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
   The author may be contacted at modthesims.info, username cmarNYC.
   with thanks to EA/SimGuruModSquad for file description and code,
   and Kuree for initial translation into C# */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.Linq;

namespace TS4SimRipper
{
    public class DMap
    {
        uint version;       // current is 7
        uint doubledWidth;
        uint height;
        uint ageGender;
        uint species; 
        byte physique;
        ShapeOrNormals shapeOrNormals;
        uint minCol;
        uint maxCol;
        uint minRow;
        uint maxRow;
        RobeChannel robeChannel;
        float skinTightMinVal;
        float skinTightDelta;
        float robeMinVal;
        float robeDelta;
        int totalBytes;
        ScanLine[] scanLines;

        public float weight = 1f;
        public string package;
        public ulong instance;
        public SimRegion region = SimRegion.ALL;

        public uint Version { get { return this.version; } }
        public uint Width { get { return this.doubledWidth / 2; } }
        public uint Height { get { return this.height; } }
        public float SkinTightMinVal { get { return (this.version >= 7) ? this.skinTightMinVal : ((this.shapeOrNormals == 0) ? -0.2f : -0.75f); } }
        public float SkinTightDelta { get { return (this.version >= 7) ? this.skinTightDelta : ((this.shapeOrNormals == 0) ? 0.4f : 1.5f); } }
        public float RobeMinVal { get { return (this.version >= 7) ? this.robeMinVal : ((this.shapeOrNormals == 0) ? -0.2f : -0.75f); } }
        public float RobeDelta { get { return (this.version >= 7) ? this.robeDelta : ((this.shapeOrNormals == 0) ? 0.4f : 1.5f); ; } }
        public Species Species { get { return (Species)this.species; } }
        public AgeGender AgeGender { get { return (AgeGender)this.ageGender; } }
        public Physiques Physique { get { return (Physiques)this.physique; } }
        public ShapeOrNormals ShapeOrNormal { get { return (ShapeOrNormals)this.shapeOrNormals; } }
        public bool RobeDataPresent { get { return (this.robeChannel == 0); } }

        public uint MinCol { get { return this.minCol; } }
        public uint MaxCol { get { return this.maxCol; } }
        public uint MinRow { get { return this.minRow; } }
        public uint MaxRow { get { return this.maxRow; } }
        public bool UseRobeChannel { get { return this.robeChannel != RobeChannel.ROBECHANNEL_DROPPED; } }

        public bool HasData
        {
            get { return this.scanLines.Length > 0; }
        }

        public DMap(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            this.version = br.ReadUInt32();
            this.doubledWidth = br.ReadUInt32();
            this.height = br.ReadUInt32();
            this.ageGender = br.ReadUInt32();
            if (version > 5) this.species = br.ReadUInt32();
            this.physique = br.ReadByte();
            this.shapeOrNormals = (ShapeOrNormals)br.ReadByte();
            this.minCol = br.ReadUInt32();
            this.maxCol = br.ReadUInt32();
            this.minRow = br.ReadUInt32();
            this.maxRow = br.ReadUInt32();
            this.robeChannel = (RobeChannel)br.ReadByte();
            if (version > 6)
            {
                this.skinTightMinVal = br.ReadSingle();
                this.skinTightDelta = br.ReadSingle();
                if ((RobeChannel)robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    this.robeMinVal = br.ReadSingle();
                    this.robeDelta = br.ReadSingle();
                }
                else
                {
                    this.robeMinVal = this.skinTightMinVal;
                    this.robeDelta = this.skinTightDelta;
                }
            }
            this.totalBytes = br.ReadInt32();
            if (this.totalBytes == 0)
            {
                scanLines = new ScanLine[0];
            }
            else
            {
                int width = (int)(maxCol - minCol + 1);
                uint numScanLines = maxRow - minRow + 1;
                scanLines = new ScanLine[numScanLines];
                for (int i = 0; i < numScanLines; i++)
                {
                    scanLines[i] = new ScanLine(width, br);
                }
            }
        }

        /// <summary>
        /// Make a DMap from an array of delta vectors: skintightDeltas[w, h], robeDeltas[w, h] (robeDeltas may be null)
        /// </summary>
        /// <param name="ageGender">AgeGender</param>
        /// <param name="physique">Physique</param>
        /// <param name="shapeNormals">Shape or Normals</param>
        /// <param name="minColumn">first column containing data</param>
        /// <param name="maxColumn">last column containing data</param>
        /// <param name="minRow">first row containing data</param>
        /// <param name="maxRow">last row containing data</param>
        /// <param name="skinDeltas">Array of Delta Vectors, [row][column]</param>
        /// <param name="robeDeltas">Array of Delta Vectors, [row][column], may be null if there is no robe data</param>
        public DMap(Species species, AgeGender ageGender, Physiques physique, RobeChannel hasRobeData, ShapeOrNormals shapeNormals,
                    uint minColumn, uint maxColumn, uint minRow, uint maxRow,
                    Vector3[][] skinDeltas, Vector3[][] robeDeltas, bool verticalFlip)
        {
            this.version = 7;
            this.height = (uint)skinDeltas.Length;
            this.doubledWidth = (uint)(skinDeltas[0].Length * 2);
            this.ageGender = (uint)ageGender;
            this.species = (uint)species;
            this.physique = (byte)physique;
            this.shapeOrNormals = shapeNormals;
            this.minCol = minColumn;
            this.maxCol = maxColumn;
            this.minRow = minRow;
            this.maxRow = maxRow;
            this.robeChannel = hasRobeData;

            float minVal = float.MaxValue;
            float maxVal = float.MinValue;
            for (uint h = minRow; h <= maxRow; h++)
            {
                for (uint w = minCol; w <= maxCol; w++)
                {
                    Vector3 v = skinDeltas[h][w];
                    if (v.X < minVal) minVal = v.X;
                    if (v.Y < minVal) minVal = v.Y;
                    if (v.Z < minVal) minVal = v.Z;
                    if (v.X > maxVal) maxVal = v.X;
                    if (v.Y > maxVal) maxVal = v.Y;
                    if (v.Z > maxVal) maxVal = v.Z;
                    if (hasRobeData == RobeChannel.ROBECHANNEL_PRESENT)
                    {
                        v = robeDeltas[h][w];
                        if (v.X < minVal) minVal = v.X;
                        if (v.Y < minVal) minVal = v.Y;
                        if (v.Z < minVal) minVal = v.Z;
                        if (v.X > maxVal) maxVal = v.X;
                        if (v.Y > maxVal) maxVal = v.Y;
                        if (v.Z > maxVal) maxVal = v.Z;
                    }
                }
            }
            float tmp = Math.Max(Math.Abs(minVal), Math.Abs(maxVal)) + 0.05f;
          //  minVal = Math.Min((ShapeOrNormal == ShapeOrNormals.SHAPE_DEFORMER ? -0.2f : -.75f), -tmp);
            minVal = -tmp;
            float deltaVal = Math.Abs(minVal) * 2.0f;

            //minVal = -0.75f;
            //deltaVal = 1.50f;

            if (minCol >= maxCol)
            {
                this.skinTightMinVal = -0.75f;
                this.skinTightDelta = 1.5f;
            }
            else
            {
                this.skinTightMinVal = minVal;
                this.skinTightDelta = deltaVal;
            }
            if (hasRobeData == RobeChannel.ROBECHANNEL_PRESENT)
            {
                this.robeMinVal = this.skinTightMinVal;
                this.robeDelta = this.skinTightDelta;
            }

            int width = (int)(maxCol - minCol + 1);
            List<ScanLine> scanners = new List<ScanLine>();
            byte zero = (byte)0x80;
            Vector3 vZero = new Vector3();
            for (uint r = minRow; r < maxRow + 1; r++)
            {
                RobeChannel robeDataLine = hasRobeData;
                if (hasRobeData == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    Vector3[] skinRow = skinDeltas[r];
                    Vector3[] robeRow = robeDeltas[r];
                    if (skinRow.SequenceEqual(robeRow))
                    {
                        robeDataLine = RobeChannel.ROBECHANNEL_ISCOPY;
                        bool drop = true;
                        foreach (Vector3 v in robeRow)
                        {
                            if (v != vZero)
                            {
                                drop = false;
                                break;
                            }
                        }
                        if (drop) robeDataLine = RobeChannel.ROBECHANNEL_DROPPED;
                    }
                }
                List<byte> pixels = new List<byte>();
                for (uint c = minCol; c < maxCol + 1 ; c++)
                {
                    Vector3 v = skinDeltas[r][c];
                    pixels.Add(Math.Abs(v.X) > .0001f ? (byte)((((double)v.X + minVal) * 255.0d) / deltaVal) : zero);
                    pixels.Add(Math.Abs(v.Y) > .0001f ? (byte)((((double)v.Y + minVal) * 255.0d) / deltaVal) : zero);
                    pixels.Add(Math.Abs(v.Z) > .0001f ? (byte)((((double)v.Z + minVal) * 255.0d) / deltaVal) : zero);
                    if (robeDataLine == RobeChannel.ROBECHANNEL_PRESENT)
                    {
                        Vector3 vr = robeDeltas[r][c];
                        pixels.Add(Math.Abs(vr.X) > .0001f ? (byte)((((double)vr.X + minVal) * 255.0d) / deltaVal) : zero);
                        pixels.Add(Math.Abs(vr.Y) > .0001f ? (byte)((((double)vr.Y + minVal) * 255.0d) / deltaVal) : zero);
                        pixels.Add(Math.Abs(vr.Z) > .0001f ? (byte)((((double)vr.Z + minVal) * 255.0d) / deltaVal) : zero);
                    }
                }
                scanners.Add(new ScanLine(width, robeDataLine, pixels.ToArray()));
            }
            if (verticalFlip)
            {
                scanners.Reverse();
                uint h = this.maxRow - this.minRow;
                this.minRow = height - this.maxRow - 1;
                this.maxRow = this.minRow + h;
            }
            this.scanLines = scanners.ToArray();            
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.version);
            bw.Write(this.doubledWidth);
            bw.Write(this.height);
            bw.Write(this.ageGender);
            if (version > 5) bw.Write(this.species);
            bw.Write(this.physique);
            bw.Write((byte)this.shapeOrNormals);
            bw.Write(this.minCol);
            bw.Write(this.maxCol);
            bw.Write(this.minRow);
            bw.Write(this.maxRow);
            bw.Write((byte)this.robeChannel);
            if (version > 6)
            {
                bw.Write(this.skinTightMinVal);
                bw.Write(this.skinTightDelta);
                if (robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    bw.Write(this.robeMinVal);
                    bw.Write(this.robeDelta);
                }
            }
            if (this.scanLines == null) this.scanLines = new ScanLine[0];
            uint totalBytes = 0;
            for (int i = 0; i < this.scanLines.Length; i++)
            {
                totalBytes += this.scanLines[i].scanLineDataSize;
            }
            bw.Write(totalBytes);
            for (int i = 0; i < this.scanLines.Length; i++)
            {
                this.scanLines[i].Write(bw);
            }
        }

        //public void Smooth()
        //{
        //    for (int i = 0; i < scanLines.Length; i++)
        //    {
        //        int pixelLength = scanLines[i].robeChannel == RobeChannel.ROBECHANNEL_PRESENT ? 6 : 3;
        //        byte[] tmp = new byte[scanLines[i].uncompressedPixels.Length];
        //        for (int j = 2 * pixelLength; j <= scanLines[i].uncompressedPixels.Length - (3 * pixelLength); j += pixelLength)
        //        {
        //            int tot1 = 0; int tot2 = 0; int tot3 = 0;
        //            int totr1 = 0; int totr2 = 0; int totr3 = 0;
        //            for (int k = j - (2 * pixelLength); k < j + (3 * pixelLength); k += pixelLength)
        //            {
        //                tot1 += scanLines[i].uncompressedPixels[k];
        //                tot2 += scanLines[i].uncompressedPixels[k + 1];
        //                tot3 += scanLines[i].uncompressedPixels[k + 2];
        //                if (pixelLength == 6)
        //                {
        //                    totr1 += scanLines[i].uncompressedPixels[k + 3];
        //                    totr2 += scanLines[i].uncompressedPixels[k + 4];
        //                    totr3 += scanLines[i].uncompressedPixels[k + 5];
        //                }
        //            }
        //            tmp[j] = (byte)((tot1 / 5f) + .5);
        //            tmp[j + 1] = (byte)((tot2 / 5) + .5);
        //            tmp[j + 2] = (byte)((tot3 / 5) + .5);
        //            if (pixelLength == 6)
        //            {
        //                tmp[j + 3] = (byte)((totr1 / 5f) + .5);
        //                tmp[j + 4] = (byte)((totr2 / 5) + .5);
        //                tmp[j + 5] = (byte)((totr3 / 5) + .5);
        //            }
        //        }
        //        Array.Copy(tmp, 2 * pixelLength, scanLines[i].uncompressedPixels, 2 * pixelLength, scanLines[i].uncompressedPixels.Length - (2 * pixelLength));
        //    }
        //}

        public class ScanLine
        {
            internal UInt16 scanLineDataSize;
            internal CompressionType isCompressed;
            internal RobeChannel robeChannel;
            internal byte[] uncompressedPixels;
            internal byte numIndexes;
            internal UInt16[] pixelPosIndexes;
            internal UInt16[] dataPosIndexes;
            internal byte[] RLEArrayOfPixels;
            int width;

            public ScanLine(int width, BinaryReader br)
            {
                this.width = width;
                this.scanLineDataSize = br.ReadUInt16();
                this.isCompressed = (CompressionType)br.ReadByte();
                if (isCompressed == CompressionType.NoData)
                {
                    this.robeChannel = RobeChannel.ROBECHANNEL_DROPPED;
                }
                else
                {
                    this.robeChannel = (RobeChannel)br.ReadByte();
                }

                if (isCompressed == CompressionType.None)
                {
                    if (robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                    {
                        this.uncompressedPixels = br.ReadBytes(width * 6);
                    }
                    else
                    {
                        this.uncompressedPixels = br.ReadBytes(width * 3);
                    }
                }
                else if (isCompressed == CompressionType.RLE)
                {
                    this.numIndexes = br.ReadByte();
                    this.pixelPosIndexes = new UInt16[numIndexes];
                    this.dataPosIndexes = new UInt16[numIndexes];
                    for (int i = 0; i < numIndexes; i++) this.pixelPosIndexes[i] = br.ReadUInt16();
                    for (int i = 0; i < numIndexes; i++) this.dataPosIndexes[i] = br.ReadUInt16();
                    uint headerdatasize = 4U + 1U + (4U * numIndexes);
                    this.RLEArrayOfPixels = br.ReadBytes((int)(scanLineDataSize - headerdatasize));
                }
            }

            public ScanLine(ScanLine other)
            {
                this.width = other.width;
                this.scanLineDataSize = other.scanLineDataSize;
                this.isCompressed = other.isCompressed;
                this.robeChannel = (RobeChannel)(byte)other.robeChannel;

                if (isCompressed == CompressionType.None)
                {
                    this.uncompressedPixels = new byte[other.uncompressedPixels.Length];
                    Array.Copy(other.uncompressedPixels, this.uncompressedPixels, other.uncompressedPixels.Length);
                }
                else if (isCompressed == CompressionType.RLE)
                {
                    this.numIndexes = other.numIndexes;
                    this.pixelPosIndexes = new UInt16[numIndexes];
                    this.dataPosIndexes = new UInt16[numIndexes];
                    Array.Copy(other.pixelPosIndexes, this.pixelPosIndexes, numIndexes);
                    Array.Copy(other.dataPosIndexes, this.dataPosIndexes, numIndexes);
                    uint headerdatasize = 4U + 1U + (4U * numIndexes);
                    this.RLEArrayOfPixels = new byte[other.RLEArrayOfPixels.Length];
                    Array.Copy(other.RLEArrayOfPixels, this.RLEArrayOfPixels, other.RLEArrayOfPixels.Length);
                }
            }

            public ScanLine(int width, RobeChannel hasRobeData, byte[] pixels)
            {
                this.width = width;
                this.scanLineDataSize = (ushort)(pixels.Length + 4);
                this.isCompressed = CompressionType.None;
                this.robeChannel = hasRobeData;

                if (isCompressed == CompressionType.None)
                {
                    this.uncompressedPixels = new byte[pixels.Length];
                    Array.Copy(pixels, this.uncompressedPixels, pixels.Length);
                }
                //else
                //{
                //    this.numIndexes = br.ReadByte();
                //    this.pixelPosIndexes = new UInt16[numIndexes];
                //    this.dataPosIndexes = new UInt16[numIndexes];
                //    for (int i = 0; i < numIndexes; i++) this.pixelPosIndexes[i] = br.ReadUInt16();
                //    for (int i = 0; i < numIndexes; i++) this.dataPosIndexes[i] = br.ReadUInt16();
                //    uint headerdatasize = 4U + 1U + (4U * numIndexes);
                //    this.RLEArrayOfPixels = br.ReadBytes((int)(scanLineDataSize - headerdatasize));
                //}

            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(this.scanLineDataSize);
                bw.Write((byte)this.isCompressed);
                if (this.isCompressed != CompressionType.NoData) bw.Write((byte)this.robeChannel);

                if (isCompressed == CompressionType.None)
                {
                    bw.Write(this.uncompressedPixels);
                }
                else if (isCompressed == CompressionType.RLE)
                {
                    bw.Write(this.numIndexes);
                    for (int i = 0; i < numIndexes; i++) bw.Write(this.pixelPosIndexes[i]);
                    for (int i = 0; i < numIndexes; i++) bw.Write(this.dataPosIndexes[i]);
                    bw.Write(this.RLEArrayOfPixels);
                }
            }
        }

        public enum OutputType
        {
            Skin,
            Robe
        }

        public enum CompressionType : byte
        {
            None = 0,
            RLE = 1,
            NoData = 2
        }

        public Stream ToBitMap(OutputType type)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter w = new BinaryWriter(ms);
            if (maxCol == 0) return null;
            int height = (int)(maxRow - minRow + 1);
            int width = (int)(this.maxCol - this.minCol + 1);

            byte[] pixelArraySkinTight = new byte[width * height * 3];
            byte[] pixelArrayRobe = new byte[width * height * 3];

            int destIndexRobe = 0;
            int destSkinTight = 0;

            int pixelsize = 0;

            for (int i = 0; i < height; i++)
            {
                if (scanLines[i].robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    pixelsize = 6;
                }
                else
                {
                    pixelsize = 3;
                }

                var scan = scanLines[i];
                if (scan.isCompressed == CompressionType.None)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 0];
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 1];
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 2];

                        switch (scan.robeChannel)
                        {
                            case RobeChannel.ROBECHANNEL_PRESENT:
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 3];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 4];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 5];
                                break;
                            case RobeChannel.ROBECHANNEL_DROPPED:
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                break;
                            case RobeChannel.ROBECHANNEL_ISCOPY:
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 0];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 1];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 2];
                                break;
                        }
                    }
                }
                else if (scan.isCompressed == CompressionType.RLE)
                {

                    // Look up each pixel using index tables
                    for (int j = 0; j < width; j++)
                    {
                        // To get pointer to the RLE encoded data we need first find 
                        // proper RLE run in the buffer. Use index for this:

                        // Cache increment for indexing in pixel space?
                        int step = 1 + width / (scan.numIndexes - 1); // 1 entry was added for the remainder of the division

                        // Find index into the positions and data table:
                        int idx = j / step;

                        // This is location of the run first covering this interval.
                        int pixelPosX = scan.pixelPosIndexes[idx];

                        // Position of the RLE data of the place where need to unwind to the pixel. 
                        int dataPos = scan.dataPosIndexes[idx] * (pixelsize + 1); // +1 for run length byte

                        // This is run length for the RLE entry found at 
                        int runLength = scan.RLEArrayOfPixels[dataPos];

                        // Loop forward unwinding RLE data from the found indexed position. 
                        // Continue until the pixel position in question is not covered 
                        // by the current run interval. By design the loop should execute 
                        // only few times until we find the value we are looking for.
                        while (j >= pixelPosX + runLength)
                        {
                            pixelPosX += runLength;
                            dataPos += (1 + pixelsize); // 1 for run length, +pixelSize for the run value

                            runLength = scan.RLEArrayOfPixels[dataPos];
                        }

                        // After breaking out of the cycle, we have the current run length interval
                        // covering the pixel position x we are interested in. So just return the pointer
                        // to the pixel data we were after:
                        int pixelStart = dataPos + 1;

                        //
                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 0];
                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 1];
                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 2];
                        switch (scan.robeChannel)
                        {
                            case RobeChannel.ROBECHANNEL_PRESENT:
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 3];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 4];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 5];
                                break;
                            case RobeChannel.ROBECHANNEL_DROPPED:
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                break;
                            case RobeChannel.ROBECHANNEL_ISCOPY:
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 0];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 1];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 2];
                                break;
                        }
                    }
                }
                else if (scan.isCompressed == CompressionType.NoData)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                    }
                }

            }

            w.Write((ushort)0x4d42);
            w.Write(0);
            w.Write(0);
            w.Write(54);
            w.Write(40);
            w.Write(width);
            w.Write(height);
            w.Write((ushort)1);
            w.Write((ushort)24);
            for (int i = 0; i < 6; i++) w.Write(0);

            int bytesPerLine = (int)Math.Ceiling(width * 24.0 / 8.0);
            int padding = 4 - bytesPerLine % 4;
            if (padding == 4) padding = 0;
            long sourcePosition = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * 3; j++)
                {
                    w.Write(type == OutputType.Robe ? pixelArrayRobe[sourcePosition++] : pixelArraySkinTight[sourcePosition++]);
                }

                for (int j = 0; j < padding; j++)
                {
                    w.Write((byte)0);
                }
            }

            return ms;
        }

        public void Uncompress()
        {
            if (maxCol == 0) return;
            int height = (int)(maxRow - minRow + 1);
            int width = (int)(this.maxCol - this.minCol + 1);

            int pixelsize = 0;

            for (int i = 0; i < height; i++)
            {
                if (scanLines[i].robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    pixelsize = 6;
                }
                else
                {
                    pixelsize = 3;
                }
                byte[] pixelArray = new byte[width * pixelsize];
                int dest = 0;

                var scan = scanLines[i];
                if (scan.isCompressed == CompressionType.RLE)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int step = 1 + width / (scan.numIndexes - 1); // 1 entry was added for the remainder of the division
                        int idx = j / step;
                        int pixelPosX = scan.pixelPosIndexes[idx];
                        int dataPos = scan.dataPosIndexes[idx] * (pixelsize + 1); // +1 for run length byte
                        int runLength = scan.RLEArrayOfPixels[dataPos];
                        while (j >= pixelPosX + runLength)
                        {
                            pixelPosX += runLength;
                            dataPos += (1 + pixelsize); // 1 for run length, +pixelSize for the run value
                            runLength = scan.RLEArrayOfPixels[dataPos];
                        }
                        int pixelStart = dataPos + 1;
                        pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 0];
                        pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 1];
                        pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 2];
                        if (scan.robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                        {
                            pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 3];
                            pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 4];
                            pixelArray[dest++] = scan.RLEArrayOfPixels[pixelStart + 5];
                        }
                    }
                    scan.uncompressedPixels = pixelArray;
                    scan.isCompressed = 0;
                    scan.scanLineDataSize = (ushort)(pixelArray.Length + 4);
                }
                else if (scan.isCompressed == CompressionType.NoData)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pixelArray[dest++] = 0x80;
                        pixelArray[dest++] = 0x80;
                        pixelArray[dest++] = 0x80;
                        scan.uncompressedPixels = pixelArray;
                        scan.isCompressed = 0;
                        scan.robeChannel = RobeChannel.ROBECHANNEL_DROPPED;
                        scan.scanLineDataSize = 3;
                    }
                }
            }
        }

        public void Compress()
        {
            if (maxCol == 0) return;
            int height = (int)(maxRow - minRow + 1);
            int width = (int)(this.maxCol - this.minCol + 1);

            int pixelsize = 0;

            for (int i = 0; i < height; i++)
            {
                if (scanLines[i].robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    pixelsize = 6;
                }
                else
                {
                    pixelsize = 3;
                }

                if (scanLines[i].isCompressed == CompressionType.None)
                {
                    var scan = new ScanLine(scanLines[i]);
                    bool noData = true;
                    foreach (byte b in scan.uncompressedPixels)
                    {
                        if (b != 0x80)
                        {
                            noData = false;
                            break;
                        }
                    }
                    if (noData)
                    {
                        scan.scanLineDataSize = 3;
                        scan.isCompressed = CompressionType.NoData;
                        scan.robeChannel = RobeChannel.ROBECHANNEL_DROPPED;
                    }
                    else
                    {
                        List<byte> compressed = new List<byte>();
                        List<ushort> pixelInd = new List<ushort>();
                        List<ushort> dataInd = new List<ushort>();
                        byte[] currentVal = new byte[pixelsize];
                        for (int p = 0; p < pixelsize; p++)             //copy first pixel set to start
                        {
                            currentVal[p] = scan.uncompressedPixels[p];
                        }
                        int pixInd = 0;
                        int runSize = 0;
                        for (int j = 0; j < width; j++)
                        {
                            byte[] testVal = new byte[pixelsize];
                            for (int p = 0; p < pixelsize; p++)    //copy next pixel set
                            {
                                testVal[p] = scan.uncompressedPixels[(j * pixelsize) + p];
                            }
                            if (currentVal.SequenceEqual(testVal) && runSize < 255)  //test for different value
                            {
                                runSize++;
                            }
                            else                                //if run of same values is over, write the count and values
                            {
                                pixelInd.Add((ushort)pixInd);
                                dataInd.Add((ushort)(compressed.Count / (pixelsize + 1)));
                                compressed.Add((byte)runSize);
                                for (int p = 0; p < pixelsize; p++)
                                {
                                    compressed.Add(currentVal[p]);
                                }
                                pixInd = j;
                                runSize = 1;
                                Array.Copy(testVal, currentVal, pixelsize);
                            }
                        }
                        pixelInd.Add((ushort)pixInd);                   //add last set
                        dataInd.Add((ushort)(compressed.Count / (pixelsize + 1)));
                        compressed.Add((byte)runSize);
                        for (int p = 0; p < pixelsize; p++)
                        {
                            compressed.Add(currentVal[p]);
                        }

                        pixelInd.Add(pixelInd[pixelInd.Count - 1]);     //duplicate last index
                        dataInd.Add(dataInd[dataInd.Count - 1]);

                        int step = width / pixelInd.Count;
                        for (int s = 1; s < pixelInd.Count; s++)
                        {
                            if (pixelInd[s] > s * step)
                            {
                                pixelInd[s] = pixelInd[s - 1];
                                dataInd[s] = dataInd[s - 1];
                            }
                        }

                        scan.numIndexes = (byte)pixelInd.Count;
                        scan.pixelPosIndexes = pixelInd.ToArray();
                        scan.dataPosIndexes = dataInd.ToArray();
                        scan.RLEArrayOfPixels = compressed.ToArray();
                        scan.isCompressed = CompressionType.RLE;
                        scan.scanLineDataSize = (ushort)((scan.numIndexes * 4) + scan.RLEArrayOfPixels.Length + 5);
                    }
                    if (noData || (scan.pixelPosIndexes.Length <= byte.MaxValue && scan.scanLineDataSize < scanLines[i].uncompressedPixels.Length + 5)) scanLines[i] = scan;
                }
            }
        }

        public MorphMap ToMorphMap()
        {
            if (maxCol == 0) return null;
            int height = (int)(maxRow - minRow + 1);
            int width = (int)(this.maxCol - this.minCol + 1);

            byte[] pixelArraySkinTight = new byte[width * height * 3];
            byte[] pixelArrayRobe = new byte[width * height * 3];
            bool[] robeDataRow = new bool[height];

            int destIndexRobe = 0;
            int destSkinTight = 0;

            int pixelsize = 0;

            for (int i = 0; i < height; i++)
            {
                if (scanLines[i].robeChannel == RobeChannel.ROBECHANNEL_PRESENT)
                {
                    pixelsize = 6;
                    robeDataRow[i] = true;
                }
                else
                {
                    pixelsize = 3;
                    robeDataRow[i] = false;
                }

                var scan = scanLines[i];
                if (scan.isCompressed == CompressionType.None)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 0];
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 1];
                        pixelArraySkinTight[destSkinTight++] = scan.uncompressedPixels[(j * pixelsize) + 2];

                        switch (scan.robeChannel)
                        {
                            case RobeChannel.ROBECHANNEL_PRESENT:
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 3];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 4];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 5];
                                break;
                            case RobeChannel.ROBECHANNEL_DROPPED:
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                break;
                            case RobeChannel.ROBECHANNEL_ISCOPY:
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 0];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 1];
                                pixelArrayRobe[destIndexRobe++] = scan.uncompressedPixels[(j * pixelsize) + 2];
                                break;
                        }
                    }
                }
                else if (scan.isCompressed == CompressionType.RLE)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int step = 1 + width / (scan.numIndexes - 1); // 1 entry was added for the remainder of the division
                        int idx = j / step;
                        int pixelPosX = scan.pixelPosIndexes[idx];
                        int dataPos = scan.dataPosIndexes[idx] * (pixelsize + 1); // +1 for run length byte
                        int runLength = scan.RLEArrayOfPixels[dataPos];
                        while (j >= pixelPosX + runLength)
                        {
                            pixelPosX += runLength;
                            dataPos += (1 + pixelsize); // 1 for run length, +pixelSize for the run value

                            runLength = scan.RLEArrayOfPixels[dataPos];
                        }
                        int pixelStart = dataPos + 1;

                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 0];
                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 1];
                        pixelArraySkinTight[destSkinTight++] = scan.RLEArrayOfPixels[pixelStart + 2];
                        switch (scan.robeChannel)
                        {
                            case RobeChannel.ROBECHANNEL_PRESENT:
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 3];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 4];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 5];
                                break;
                            case RobeChannel.ROBECHANNEL_DROPPED:
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                pixelArrayRobe[destIndexRobe++] = 0x80;
                                break;
                            case RobeChannel.ROBECHANNEL_ISCOPY:
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 0];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 1];
                                pixelArrayRobe[destIndexRobe++] = scan.RLEArrayOfPixels[pixelStart + 2];
                                break;
                        }
                    }
                }
                else if (scan.isCompressed == CompressionType.NoData)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArraySkinTight[destSkinTight++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                        pixelArrayRobe[destIndexRobe++] = 0x80;
                    }
                }
            }

            long sourcePosition = 0;
            Vector3[][] deltaSkin = new Vector3[height][];
            Vector3[][] deltaRobe = new Vector3[height][];
            for (int i = 0; i < height; i++)
            {
                deltaSkin[i] = new Vector3[width];
                deltaRobe[i] = new Vector3[width];
                for (int j = 0; j < width; j++)
                {
                    deltaSkin[i][j] = new Vector3(pixelArraySkinTight[sourcePosition] == 0x80 ? 0f : (float)(((pixelArraySkinTight[sourcePosition] * (double)this.SkinTightDelta) / 255d) + this.SkinTightMinVal),
                                                  pixelArraySkinTight[sourcePosition + 1] == 0x80 ? 0f : (float)(((pixelArraySkinTight[sourcePosition + 1] * (double)this.SkinTightDelta) / 255d) + this.SkinTightMinVal),
                                                  pixelArraySkinTight[sourcePosition + 2] == 0x80 ? 0f : (float)(((pixelArraySkinTight[sourcePosition + 2] * (double)this.SkinTightDelta) / 255d) + this.SkinTightMinVal));
                    deltaRobe[i][j] = new Vector3(pixelArrayRobe[sourcePosition] == 0x80 ? 0f : (float)(((pixelArrayRobe[sourcePosition] * (double)this.RobeDelta) / 255d) + this.RobeMinVal),
                                                  pixelArrayRobe[sourcePosition + 1] == 0x80 ? 0f : (float)(((pixelArrayRobe[sourcePosition + 1] * (double)this.RobeDelta) / 255d) + this.RobeMinVal),
                                                  pixelArrayRobe[sourcePosition + 2] == 0x80 ? 0f : (float)(((pixelArrayRobe[sourcePosition + 2] * (double)this.RobeDelta) / 255d) + this.RobeMinVal));
                    sourcePosition += 3;
                }
            }

            return new MorphMap(this.Width, this.height, this.shapeOrNormals, this.minCol, this.maxCol,
                                this.minRow, this.maxRow, deltaSkin, deltaRobe, robeDataRow, this.weight, this.AgeGender,
                                this.package, this.instance, this.region);
        }
    }

    public class MorphMap
    {
        uint mapWidth;
        uint mapHeight;
        ShapeOrNormals shapeOrNormals;
        uint minCol;
        uint maxCol;
        uint minRow;
        uint maxRow;
        Vector3[][] skinDeltas;
        Vector3[][] robeDeltas;

        public uint MapWidth { get { return this.mapWidth; } }
        public uint MapHeight { get { return this.mapHeight; } }
        public uint MinCol { get { return this.minCol; } }
        public uint MaxCol { get { return this.maxCol; } }
        public uint MinRow { get { return this.minRow; } }
        public uint MaxRow { get { return this.maxRow; } }

        public float weight = 1f;
        public AgeGender age;
        public string package;
        public ulong instance;
        public SimRegion region;

        public Vector3 GetAdjustedDelta(int x, int y, bool mirrorX, byte robeBlend)
        {
            if (y < 0 || y > skinDeltas.Length) return new Vector3();
            if (Math.Abs(x) > skinDeltas[y].Length) return new Vector3();
            float robeMult = (float)robeBlend / 63f;
            Vector3 tmpS = new Vector3(skinDeltas[y][Math.Abs(x)]);
            if (mirrorX || x < 0) tmpS.X = -tmpS.X;
            Vector3 tmpR = new Vector3(robeDeltas[y][Math.Abs(x)]);
            if (mirrorX || x < 0) tmpR.X = -tmpR.X;
            Vector3 delta;
            delta = new Vector3((tmpS.X * (1f - robeMult)) + (tmpR.X * robeMult),
                                (tmpS.Y * (1f - robeMult)) + (tmpR.Y * robeMult),
                                (tmpS.Z * (1f - robeMult)) + (tmpR.Z * robeMult));
            return delta;
        }

        public bool HasData
        {
            get { return this.skinDeltas.Length > 0; }
        }

        public MorphMap(uint mapWidth, uint mapHeight, ShapeOrNormals shapeOrNormals, uint minCol, uint maxCol,
            uint minRow, uint maxRow, Vector3[][] skinDeltas, Vector3[][] robeDeltas, bool[] robeDataRow, float weight, AgeGender age,
            string package, ulong instance, SimRegion region)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.shapeOrNormals = shapeOrNormals;
            this.minCol = minCol;
            this.maxCol = maxCol;
            this.minRow = minRow;
            this.maxRow = maxRow;
            this.skinDeltas = skinDeltas;
            this.robeDeltas = robeDeltas;
            this.weight = weight;
            this.age = age;
            this.package = package;
            this.instance = instance;
            this.region = region;
        }
    }
}
