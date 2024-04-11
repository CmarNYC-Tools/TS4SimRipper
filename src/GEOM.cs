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
using System.Windows.Forms;
using System.Linq;

namespace TS4SimRipper
{
    public class GEOM : IComparable<GEOM>
    {
        private int version1, count, ind3, extCount, intCount;
        private TGI dummyTGI;
        private int abspos, meshsize;
        private char[] magic;
        private int version, TGIoff, TGIsize;       // versions: base game 12, current 14
        private uint shaderHash;
        private int MTNFsize;
        private MTNF meshMTNF;
        private int mergeGrp, sortOrd, numVerts, Fcount;
        private vertexForm[] vertform = null;
        private position[] vPositions = null;
        private position[] originalPositions = null;
        private normal[] vNormals = null;
        private uv[][] vUVs = null;
        private Bones[] vBones = null;
        private tangent[] vTangents = null;
        private tagval[] vTags = null;
        private int[] vertID = null;
        private int numSubMeshes;
        private byte bytesperfacepnt;
        private int numfacepoints;
        private Face[] meshfaces = null;
        private int skconIndex;  // index to skcon, unused
        private int uvStitchCount;
        private UVStitch[] uvStitches = null;
        private int seamStitchCount;
        private SeamStitch[] seamStitches = null;
        private int slotCount;
        private SlotrayIntersection[] slotrayIntersections = null;
        private int bonehashcount;
        private uint[] bonehasharray = null;
        private int numtgi;
        private TGI[] meshTGIs = null;

        public Vector3[] deltaPosition;
        public bool copyFaceMorphs = false;
        public string package;
        public ulong[] instance;
        public string Instance
        {
            get
            {
                string tmp = "";
                for (int i = 0; i < instance.Length; i++)
                {
                    tmp += "0x" + instance[i].ToString("X16");
                    if (i < instance.Length - 1) tmp += ", ";
                }
                return tmp;
            }
        }

        public static Vector3[][][][][] MeshSeamVerts = SetupSeamVertexPositions();
        public static List<int> femaleWaistSequence = new List<int> { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 1, 0x0B, 0x0A, 9, 8, 7, 6, 2, 3, 5, 4, 0, 0x0E, 0x0F, 0x0D, 0x0C };
        public static List<int> maleWaistSequence = new List<int> { 0x13, 0x12, 0x14, 0x15, 0x10, 0x11, 0x0B, 6, 5, 0x0A, 9, 7, 8, 0, 1, 3, 2, 4, 0x0E, 0x0F, 0x0D, 0x0C };

        public bool UpdateToLatestVersion()         //latest version is 14 but meshes included here are V13
        {
            if (this.version < 13)
            {
                this.SetVersion(13);
                return true;
            }
            return false;
        }

        public void SetVersion(int newVersion)
        {
            if (newVersion == 5 && this.version > 5)
            {
                for (int i = 0; i < this.Fcount; i++)
                {
                    if (this.vertform[i].datatype == 5)
                    {
                        this.vertform[i].subtype = 1;
                        this.vertform[i].bytesper = 16;
                    }
                }
            }
            if (newVersion >= 12 & this.version == 5)
            {
                for (int i = 0; i < this.Fcount; i++)
                {
                    if (this.vertform[i].datatype == 5)
                    {
                        this.vertform[i].subtype = 2;
                        this.vertform[i].bytesper = 4;
                    }
                }
            }
            this.version = newVersion;
        }

        public int meshVersion
        {
            get { return this.version; }
        }
        public int numberVertices
        {
            get { return numVerts; }
        }
        public int mergeGroup
        {
            get { return this.mergeGrp; }
        }
        public int sortOrder
        {
            get { return this.sortOrd; }
        }
        public int numberSubMeshes
        {
            get { return this.numSubMeshes; }
        }
        public int bytesPerFacePoint
        {
            get { return this.bytesperfacepnt; }
        }
        public UVStitch[] UVStitches
        {
            get { return this.uvStitches; }
            set
            {
                if (value != null)
                {
                    this.uvStitches = value;
                    this.uvStitchCount = this.uvStitches.Length;
                }
                else
                {
                    this.uvStitches = new UVStitch[0];
                    this.uvStitchCount = 0;
                }
            }
        }
        internal int UVStitches_size
        {
            get
            {
                int tmp = 0;
                foreach (UVStitch a in this.uvStitches)
                {
                    tmp += a.Size;
                }
                return tmp;
            }
        }
        public SeamStitch[] SeamStitches
        {
            get { return this.seamStitches; }
            set
            {
                if (value != null)
                {
                    this.seamStitches = value;
                    this.seamStitchCount = this.seamStitches.Length;
                }
                else
                {
                    this.seamStitches = new SeamStitch[0];
                    this.seamStitchCount = 0;
                }
            }
        }
        public SlotrayIntersection[] SlotrayAdjustments
        {
            get { return this.slotrayIntersections; }
            set
            {
                if (value != null)
                {
                    this.slotrayIntersections = value;
                    this.slotCount = this.slotrayIntersections.Length;
                }
                else
                {
                    this.slotrayIntersections = new SlotrayIntersection[0];
                    this.slotCount = 0;
                }
            }
        }
        public Vector3[] SlotrayTrianglePositions(int slotrayAdjustmentIndex)
        {
            int[] vertexIndices = this.SlotrayAdjustments[slotrayAdjustmentIndex].TrianglePointIndices;
            return new Vector3[] { new Vector3(this.vPositions[vertexIndices[0]].Coordinates), 
                new Vector3(this.vPositions[vertexIndices[1]].Coordinates), new Vector3(this.vPositions[vertexIndices[2]].Coordinates) };
        }
        internal int slotrayAdjustments_size
        {
            get { return this.slotrayIntersections.Length * 63; }
        }

        public bool isValid
        {
            get
            {
                bool b = false;
                if (new string(this.magic) == "GEOM" && this.numVerts > 0 && (this.version == 5 || this.version == 12 || this.version == 13 || this.version == 14) && this.Fcount > 2)
                {
                    b = true;
                    int uvInd = 0;
                    for (int i = 0; i < this.vertexFormatList.Length; i++)
                    {
                        switch (this.vertexFormatList[i])
                        {
                            case (1):
                                if (this.vPositions.Length != this.numVerts) b = false;
                                break;
                            case (2):
                                if (this.vNormals.Length != this.numVerts) b = false;
                                break;
                            case (3):
                                if (this.vUVs[uvInd].Length != this.numVerts) b = false;
                                uvInd += 1;
                                break;
                            case (4):
                                if (this.vBones.Length != this.numVerts) b = false;
                                break;
                            case (5):
                                if (this.vBones.Length != this.numVerts) b = false;
                                break;
                            case (6):
                                if (this.vTangents.Length != this.numVerts) b = false;
                                break;
                            case (7):
                                if (this.vTags.Length != this.numVerts) b = false;
                                break;
                            case (10):
                                if (this.vertID.Length != this.numVerts) b = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
                return b;
            }
        }

        public bool isBase
        {
            get { return (this.Fcount > 3); }
        }
        public bool isMorph
        {
            get
            {
                bool b = false;
                if (this.Fcount == 3)
                {
                    if (((this.vertform[0].datatype == 1) && (this.vertform[0].subtype == 1) && (this.vertform[0].bytesper == 12)) &
                        ((this.vertform[1].datatype == 2) && (this.vertform[1].subtype == 1) && (this.vertform[1].bytesper == 12)) &
                        ((this.vertform[2].datatype == 10) && (this.vertform[2].subtype == 4) && (this.vertform[2].bytesper == 4)))
                        b = true;
                }
                return b;
            }
        }
        public bool hasVertexIDs
        {
            get
            {
                if (Array.IndexOf(this.vertexFormatList, 10) > -1 && this.vertID.Length > 0)
                {
                    for (uint i = 0; i < this.numVerts; i++)
                    {
                        if (this.vertID[i] > 0) return true;
                    }
                }
                return false;
            }
        }
        public bool vertexIDsAreSequential
        {
            get
            {
                for (uint i = 0; i < this.numVerts; i++)
                {
                    if (this.vertID[i] != i) return false;
                }
                return true;
            }
        }
        public bool hasPositions
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 1) > -1 && this.vPositions.Length > 0);
            }
        }
        public bool hasNormals
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 2) > -1 && this.vNormals.Length > 0);
            }
        }
        public bool hasUVs
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 3) > -1 && this.vUVs.Length > 0);
            }
        }
        public bool hasBones
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 4) > -1 && this.vBones.Length > 0);
            }
        }
        public bool hasTangents
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 6) > -1 && this.vTangents.Length > 0);
            }
        }
        public bool hasTags
        {
            get
            {
                return (Array.IndexOf(this.vertexFormatList, 7) > -1 && this.vTags.Length > 0);
            }
        }
        public bool hasSeamStitches
        {
            get
            {
                return this.seamStitches != null && this.seamStitches.Length > 0;
            }
        }
        public int[] seamStitchTypes
        {
            get
            {
                List<int> seams = new List<int>();
                if (this.seamStitches == null) return seams.ToArray();
                foreach (SeamStitch s in this.seamStitches)
                {
                    if (!seams.Contains(s.SeamType)) seams.Add(s.SeamType);
                }
                return seams.ToArray();
            }
        }

        public int numberUVsets
        {
            get
            {
                if (vUVs == null)
                {
                    return 0;
                }
                else
                {
                    return vUVs.Length;
                }
            }
        }

        public vertexForm[] vertexFormat
        {
            get
            {
                return this.vertform;
            }
        }
        public int[] vertexFormatList
        {
            get
            {
                int[] tmp = new int[Fcount];
                for (int i = 0; i < Fcount; i++)
                {
                    tmp[i] = this.vertform[i].datatype;
                }
                return tmp;
            }
        }
        public int vertexDataLength
        {
            get
            {
                int vl = 0;
                for (int i = 0; i < Fcount; i++)
                {
                    vl = vl + vertform[i].bytesper;
                }
                return vl;
            }
        }
        public int minVertexID
        {
            get
            {
                if (vertID == null)
                {
                    return -1;
                }
                int m = vertID[0];
                for (int i = 1; i < this.vertID.Length; i++)
                    if (m > vertID[i]) m = vertID[i];
                return m;
            }
        }
        public int maxVertexID
        {
            get
            {
                if (vertID == null)
                {
                    return -1;
                }
                int m = 0;
                for (int i = 0; i < this.vertID.Length; i++)
                    if (m < vertID[i]) m = vertID[i];
                return m;
            }
        }
        public int numberFaces
        {
            get { return numfacepoints / 3; }
        }
        public int numberBones
        {
            get { return bonehasharray.Length; }
        }
        public uint[] BoneHashList
        {
            get { return bonehasharray; }
        }
        public int GetBoneIndex(uint boneHash)
        {
            for (int i = 0; i < this.numberBones; i++)
            {
                if (boneHash == this.bonehasharray[i]) return i;
            }
            return -1;
        }

        public List<vertexData> GetVertexList()
        {
            List<vertexData> vertList = new List<vertexData>();
            for (int i = 0; i < this.vPositions.Length; i++)
            {
                vertList.Add(new vertexData(vPositions[i], vNormals[i], new uv[] { vUVs[0][i], vUVs[1][i] }, vBones[i], 
                    vTags[i] != null ? vTags[i] : new tagval(), vTangents[i] != null ? vTangents[i] : new tangent(),
                    vertID.Length > 1 ? vertID[i] : 0));
            }
            return vertList;
        }

        public void SetVertexList(List<vertexData> vertexList)
        {
            position[] pos = new position[vertexList.Count];
            normal[] norm = new normal[vertexList.Count];
            uv[][] uvs = new uv[2][];
            uvs[0] = new uv[vertexList.Count];
            uvs[1] = new uv[vertexList.Count];
            Bones[] bone = new Bones[vertexList.Count];
            tagval[] color = new tagval[vertexList.Count];
            tangent[] tan = new tangent[vertexList.Count];
            int[] id = new int[vertexList.Count];
            for (int i = 0; i < vertexList.Count; i++)
            {
                pos[i] = vertexList[i].p;
                norm[i] = vertexList[i].n;
                uvs[0][i] = vertexList[i].uvs[0];
                uvs[1][i] = vertexList[i].uvs[1];
                bone[i] = vertexList[i].b;
                color[i] = vertexList[i].t;
                tan[i] = vertexList[i].tan;
                id[i] = vertexList[i].id;
            }
            this.vPositions = pos;
            this.vNormals = norm;
            this.vUVs = uvs;
            this.vBones = bone;
            this.vTags = color;
            this.vTangents = tan;
            this.vertID = id;
        }

        public int[] GetVertexIndicesAssignedtoBone(uint boneHash)
        {
            int index = GetBoneIndex(boneHash);
            if (index < 0) return new int[] {};
            List<int> verts = new List<int>();
            for (int i = 0; i < this.numberVertices; i++)
            {
                byte[] bones = this.getBones(i);
                byte[] weights = this.getBoneWeights(i);
                for (int j = 0; j < 4; j++)
                {
                    if (weights[j] > 0 && bones[j] == index) verts.Add(i);
                } 
            }
            return verts.ToArray();
        }
        public float GetBoneWeightForVertex(int vertexIndex, uint boneHash)
        {
            byte[] vertBones = this.getBones(vertexIndex);
            byte[] vertWeights = this.getBoneWeights(vertexIndex);
            for (int i = 0; i < 4; i++)
            {
                if (this.bonehasharray[vertBones[i]] == boneHash) return vertWeights[i] / 255f;
            }
            return 0f;
        }
        public bool validBones(int vertexSequenceNumber)
        {
            byte[] abones = this.getBones(vertexSequenceNumber);
            byte[] wbones = this.getBoneWeights(vertexSequenceNumber);
            if (version == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (wbones[i] > 0 & (abones[i] < 0 | abones[i] >= this.numberBones)) return false;
                }
            }
            else if (version >= 12)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (abones[i] < 0 | abones[i] >= this.numberBones) return false;
                }
            }

            return true;
        }
        public string[] TGIListStr
        {
            get
            {
                string[] tmp = new string[numtgi];
                for (int i = 0; i < numtgi; i++)
                    tmp[i] = meshTGIs[i].ToString();
                return tmp;
            }
        }
        public TGI[] TGIList
        {
            get
            {
                TGI[] tmp = new TGI[numtgi];
                for (int i = 0; i < numtgi; i++)
                    tmp[i] = meshTGIs[i];
                return tmp;
            }
            set
            {
                this.numtgi = value.Length;
                TGI[] tmp = new TGI[this.numtgi];
                for (int i = 0; i < numtgi; i++)
                    tmp[i] = value[i];
                this.meshTGIs = tmp;
            }
        }

        public uint ShaderHash
        {
            get
            {
                return this.shaderHash;
            }
        }
        public MTNF Shader
        {
            get
            {
                return this.meshMTNF;
            }
        }
        public int skeletonIndex
        {
            get { return this.skconIndex; }
        }

        public void setShader(uint shaderHash)
        {
            this.shaderHash = shaderHash;
        }

        public void setShader(uint shaderHash, GEOM.MTNF shader)
        {
            this.shaderHash = shaderHash;
            this.meshMTNF = shader;
        }

        public void setTGI(int index, TGI tgi)
        {
            this.meshTGIs[index] = new TGI(tgi);
        }

        public bool vertexFormatEquals(int[] testFormatList)
        {
            int[] tmp = this.vertexFormatList;
            if (tmp.Length != testFormatList.Length) return false;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] != testFormatList[i]) return false;
            }
            return true;
        }

        public bool hasUVset(int UVsequence)
        {
            return (Array.IndexOf(this.vertexFormatList, 3) > -1 && UVsequence < this.vUVs.Length && this.vUVs[UVsequence] != null);
        }

        public string[] DataString(int lineno)
        {
            int[] f = this.vertexFormatList;
            string[] str = new string[f.Length];
            int uvInd = 0;
            for (int i = 0; i < f.Length; i++)
            {
                switch (f[i])
                {
                    case (1):
                        str[i] = "Position: " + vPositions[lineno].ToString();
                        break;
                    case (2):
                        str[i] = "Normals: " + vNormals[lineno].ToString();
                        break;
                    case (3):
                        str[i] = "UV: " + vUVs[uvInd][lineno].ToString();
                        uvInd += 1;
                        break;
                    case (4):
                        str[i] = "Bones: " + vBones[lineno].ToString();
                        break;
                    case (6):
                        str[i] = "Tangents: " + vTangents[lineno].ToString();
                        break;
                    case (7):
                        str[i] = "TagVals: " + vTags[lineno].ToString();
                        break;
                    case (10):
                        str[i] = "Vertex ID: " + vertID[lineno].ToString();
                        break;
                    default:
                        break;
                }
            }
            return str;
        }
        public string VertexDataString(int vertexSequenceNumber)
        {
            int[] f = this.vertexFormatList;
            string str = "";
            int uvInd = 0;
            string s = " | ";
            for (int i = 0; i < f.Length; i++)
            {
                switch (f[i])
                {
                    case (1):
                        str = str + vPositions[vertexSequenceNumber].ToString() + s;
                        break;
                    case (2):
                        str = str + vNormals[vertexSequenceNumber].ToString() + s;
                        break;
                    case (3):
                        str = str + vUVs[uvInd][vertexSequenceNumber].ToString() + s;
                        uvInd += 1;
                        break;
                    case (4):
                        str = str + vBones[vertexSequenceNumber].ToString() + s;
                        break;
                    case (6):
                        str = str + vTangents[vertexSequenceNumber].ToString() + s;
                        break;
                    case (7):
                        str = str + vTags[vertexSequenceNumber].ToString() + s;
                        break;
                    case (10):
                        str = str + vertID[vertexSequenceNumber].ToString() + s;
                        break;
                    default:
                        break;
                }
            }
            int ind = str.LastIndexOf(s);
            str = str.Remove(ind);
            return str;
        }

        public int getVertexID(int vertexSequenceNumber)
        {
            return (int)this.vertID[vertexSequenceNumber];
        }
        public float[] getOriginalPosition(int vertexSequenceNumber)
        {
            return this.originalPositions[vertexSequenceNumber].Data();
        }
        public float[] getPosition(int vertexSequenceNumber)
        {
            return this.vPositions[vertexSequenceNumber].Data();
        }
        public float[] getNormal(int vertexSequenceNumber)
        {
            return this.vNormals[vertexSequenceNumber].Data();
        }
        public float[] getUV(int vertexSequenceNumber, int UVset)
        {
            return this.vUVs[UVset][vertexSequenceNumber].Data();
        }
        public byte[] getBones(int vertexSequenceNumber)
        {
            return this.vBones[vertexSequenceNumber].boneAssignments;
        }
        public float[] getBoneWeightsV5(int vertexSequenceNumber)
        {
            return this.vBones[vertexSequenceNumber].boneWeightsV5;
        }
        public byte[] getBoneWeights(int vertexSequenceNumber)
        {
            return this.vBones[vertexSequenceNumber].boneWeights;
        }
        public float[] getTangent(int vertexSequenceNumber)
        {
            return this.vTangents[vertexSequenceNumber].Data();
        }
        public uint getTagval(int vertexSequenceNumber)
        {
            return this.vTags[vertexSequenceNumber].Data();
        }
        public int[] getFaceIndices(int faceSequenceNumber)
        {
            return new int[] { (int)this.meshfaces[faceSequenceNumber].meshface[0], (int)this.meshfaces[faceSequenceNumber].meshface[1], (int)this.meshfaces[faceSequenceNumber].meshface[2] };
        }
        public uint[] getFaceIndicesUint(int faceSequenceNumber)
        {
            return new uint[] { (uint)this.meshfaces[faceSequenceNumber].meshface[0], (uint)this.meshfaces[faceSequenceNumber].meshface[1], (uint)this.meshfaces[faceSequenceNumber].meshface[2] };
        }
        public Vector3[] getFacePoints(int faceSequenceNumber)
        {
            return new Vector3[] { new Vector3(this.vPositions[this.meshfaces[faceSequenceNumber].meshface[0]].Coordinates), 
                new Vector3(this.vPositions[this.meshfaces[faceSequenceNumber].meshface[1]].Coordinates), 
                new Vector3(this.vPositions[this.meshfaces[faceSequenceNumber].meshface[2]].Coordinates) };
        }

        public void setVertexID(int vertexSequenceNumber, int newVertexID)
        {
            this.vertID[vertexSequenceNumber] = newVertexID;
        }
        public void setPosition(int vertexSequenceNumber, float[] newPosition)
        {
            this.vPositions[vertexSequenceNumber] = new position(newPosition);
        }
        public void setPosition(int vertexSequenceNumber, float X, float Y, float Z)
        {
            this.vPositions[vertexSequenceNumber] = new position(X, Y, Z);
        }
        public void setNormal(int vertexSequenceNumber, float[] newNormal)
        {
            this.vNormals[vertexSequenceNumber] = new normal(newNormal);
        }
        public void setNormal(int vertexSequenceNumber, float X, float Y, float Z)
        {
            this.vNormals[vertexSequenceNumber] = new normal(X, Y, Z);
        }
        public void setUV(int vertexSequenceNumber, int UVset, float[] newUV)
        {
            this.vUVs[UVset][vertexSequenceNumber] = new uv(newUV[0], newUV[1]);
        }
        public void setUV(int vertexSequenceNumber, int UVset, float U, float V)
        {
            this.vUVs[UVset][vertexSequenceNumber] = new uv(U, V);
        }
        public void setBoneList(uint[] newBoneHashList)
        {
            this.bonehasharray = newBoneHashList;
            this.bonehashcount = newBoneHashList.Length;
        }
        public void setBones(int vertexSequenceNumber, byte[] newBones)
        {
            this.vBones[vertexSequenceNumber].boneAssignments = newBones;
        }
        public void setBones(int vertexSequenceNumber, byte bone0, byte bone1, byte bone2, byte bone3)
        {
            this.vBones[vertexSequenceNumber].boneAssignments = new byte[] { bone0, bone1, bone2, bone3 };
        }
        public void setBoneWeightsV5(int vertexSequenceNumber, float[] newWeights)
        {
            this.vBones[vertexSequenceNumber].boneWeightsV5 = newWeights;
        }
        public void setBoneWeightsV5(int vertexSequenceNumber, float weight0, float weight1, float weight2, float weight3)
        {
            this.vBones[vertexSequenceNumber].boneWeightsV5 = new float[] { weight0, weight1, weight2, weight3 };
        }
        public void setBoneWeights(int vertexSequenceNumber, byte[] newWeights)
        {
            this.vBones[vertexSequenceNumber].boneWeights = newWeights;
        }
        public void setBoneWeights(int vertexSequenceNumber, byte weight0, byte weight1, byte weight2, byte weight3)
        {
            this.vBones[vertexSequenceNumber].boneWeights = new byte[] { weight0, weight1, weight2, weight3 };
        }
        public void setTangent(int vertexSequenceNumber, float[] newTangent)
        {
            this.vTangents[vertexSequenceNumber] = new tangent(newTangent);
        }
        public void setTangent(int vertexSequenceNumber, float X, float Y, float Z)
        {
            this.vTangents[vertexSequenceNumber] = new tangent(X, Y, Z);
        }
        public void setTagval(int vertexSequenceNumber, uint newTag)
        {
            this.vTags[vertexSequenceNumber] = new tagval(newTag);
        }
        public void setBoneHashList(uint[] boneHashList)
        {
            this.bonehasharray = boneHashList;
            this.bonehashcount = boneHashList.Length;
        }

        public List<float[]> GetStitchUVs(int vertexIndex)
        {
            List<float[]> uvList = new List<float[]>();
            foreach (UVStitch s in this.uvStitches)
            {
                if (s.Index == vertexIndex)
                {
                    uvList.AddRange(s.UV1Coordinates);
                    break;
                }
            }
            return uvList;
        }

        public float[] GetHeightandDepth()
        {
            float yMax = 0, zMax = 0;
            foreach (position p in vPositions)
            {
                if (p.Y > yMax) yMax = p.Y;
                if (p.Z > zMax) zMax = p.Z;
            }
            return new float[] { yMax, zMax };
        }

        public GEOM() { }

        public GEOM(GEOM sourceMesh)
        {
            //if (!sourceMesh.isValid)
            //{
            //    throw new MeshException("Invalid source mesh, cannot construct new mesh!");
            //}
            this.package = sourceMesh.package;
            this.instance = new ulong[sourceMesh.instance.Length];
            Array.Copy(sourceMesh.instance, this.instance, sourceMesh.instance.Length);
            this.version1 = sourceMesh.version1;
            this.count = sourceMesh.count;
            this.ind3 = sourceMesh.ind3;
            this.extCount = sourceMesh.extCount;
            this.intCount = sourceMesh.intCount;
            this.dummyTGI = new TGI(sourceMesh.dummyTGI);
            this.abspos = sourceMesh.abspos;
            this.magic = sourceMesh.magic;
            this.version = sourceMesh.version;
            this.shaderHash = sourceMesh.shaderHash;
            this.MTNFsize = sourceMesh.MTNFsize;
            if (this.shaderHash > 0)
            {
                this.meshMTNF = new MTNF(sourceMesh.meshMTNF);
            }
            this.mergeGrp = sourceMesh.mergeGrp;
            this.sortOrd = sourceMesh.sortOrd;
            this.numVerts = sourceMesh.numberVertices;
            this.Fcount = sourceMesh.Fcount;
            this.vertform = new vertexForm[sourceMesh.Fcount];
            for (int i = 0; i < sourceMesh.Fcount; i++)
            {
                this.vertform[i] = new vertexForm(sourceMesh.vertform[i]);
            }
            if (sourceMesh.hasBones) this.vBones = new Bones[this.numVerts];
            for (int i = 0; i < this.vertform.Length; i++)
            {
                switch (this.vertform[i].datatype)
                {
                    case (1):
                        this.vPositions = new position[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.vPositions[j] = new position(sourceMesh.vPositions[j]);
                        }
                        this.originalPositions = new position[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.originalPositions[j] = new position(sourceMesh.originalPositions[j]);
                        }
                        break;
                    case (2):
                        this.vNormals = new normal[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.vNormals[j] = new normal(sourceMesh.vNormals[j]);
                        }
                        break;
                    case (3):
                        this.vUVs = new uv[sourceMesh.vUVs.Length][];
                        for (int j = 0; j < sourceMesh.vUVs.Length; j++)
                        {
                            this.vUVs[j] = new uv[this.numVerts];
                            for (int k = 0; k < this.numVerts; k++)
                            {
                                this.vUVs[j][k] = new uv(sourceMesh.vUVs[j][k]);
                            }
                        }
                        break;
                    case (4):
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            if (this.vBones[j] == null) this.vBones[j] = new Bones();
                            byte[] tmp = new byte[] { sourceMesh.vBones[j].boneAssignments[0], sourceMesh.vBones[j].boneAssignments[1], 
                                sourceMesh.vBones[j].boneAssignments[2], sourceMesh.vBones[j].boneAssignments[3] };
                            this.vBones[j].boneAssignments = tmp;
                        }
                        break;
                    case (5):
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            if (this.vBones[j] == null) this.vBones[j] = new Bones();
                            byte[] tmp = new byte[] { sourceMesh.vBones[j].boneWeights[0], sourceMesh.vBones[j].boneWeights[1], 
                                sourceMesh.vBones[j].boneWeights[2], sourceMesh.vBones[j].boneWeights[3] };
                            this.vBones[j].boneWeights = tmp;
                        }
                        break;
                    case (6):
                        this.vTangents = new tangent[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.vTangents[j] = new tangent(sourceMesh.vTangents[j]);
                        }
                        break;
                    case (7):
                        this.vTags = new tagval[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.vTags[j] = new tagval(sourceMesh.vTags[j]);
                        }
                        break;
                    case (10):
                        this.vertID = new int[this.numVerts];
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            this.vertID[j] = sourceMesh.vertID[j];
                        }
                        break;
                    default:
                        break;
                }
            }

            this.numSubMeshes = sourceMesh.numSubMeshes;
            this.bytesperfacepnt = sourceMesh.bytesperfacepnt;
            this.numfacepoints = sourceMesh.numfacepoints;
            this.meshfaces = new Face[sourceMesh.meshfaces.Length];
            for (int i = 0; i < sourceMesh.meshfaces.Length; i++)
            {
                this.meshfaces[i] = new Face(sourceMesh.meshfaces[i]);
            }
            if (sourceMesh.version == 5)
            {
                this.skconIndex = sourceMesh.skconIndex;
            }
            else if (sourceMesh.version >= 12)
            {
                this.uvStitchCount = sourceMesh.uvStitchCount;
                uvStitches = new UVStitch[this.uvStitchCount];
                for (int i = 0; i < this.uvStitchCount; i++)
                {
                    uvStitches[i] = new UVStitch(sourceMesh.uvStitches[i]);
                }
                if (sourceMesh.version >= 13)
                {
                    this.seamStitchCount = sourceMesh.seamStitchCount;
                    seamStitches = new SeamStitch[this.seamStitchCount];
                    for (int i = 0; i < this.seamStitchCount; i++)
                    {
                        seamStitches[i] = new SeamStitch(sourceMesh.seamStitches[i]);
                    }
                }
                this.slotCount = sourceMesh.slotCount;
                slotrayIntersections = new SlotrayIntersection[this.slotCount];
                for (int i = 0; i < this.slotCount; i++)
                {
                    slotrayIntersections[i] = new SlotrayIntersection(sourceMesh.slotrayIntersections[i]);
                }
            }
            this.bonehashcount = sourceMesh.bonehashcount;
            this.bonehasharray = new uint[sourceMesh.bonehasharray.Length];
            for (int i = 0; i < sourceMesh.bonehasharray.Length; i++)
            {
                this.bonehasharray[i] = sourceMesh.bonehasharray[i];
            }
            this.numtgi = sourceMesh.numtgi;
            this.meshTGIs = new TGI[sourceMesh.meshTGIs.Length];
            for (int i = 0; i < sourceMesh.meshTGIs.Length; i++)
            {
                this.meshTGIs[i] = new TGI(sourceMesh.meshTGIs[i]);
            }

            this.copyFaceMorphs = sourceMesh.copyFaceMorphs;
        }

        public GEOM(GEOM basemesh, Vector3[] delta_positions, Vector3[] delta_normals)
        {
            if (!basemesh.isValid | !basemesh.isBase | basemesh.numVerts <= 0)
            {
                throw new MeshException("Invalid base mesh, cannot construct new mesh!");
            }
            if (basemesh.numberVertices != delta_positions.Length | basemesh.numberVertices != delta_normals.Length)
            {
                throw new MeshException("Lists of positions and normals do not match number of base mesh vertices!");
            }
            this.package = basemesh.package;
            this.instance = new ulong[basemesh.instance.Length];
            Array.Copy(basemesh.instance, this.instance, basemesh.instance.Length);
            this.version1 = basemesh.version1;
            this.count = basemesh.count;
            this.ind3 = basemesh.ind3;
            this.extCount = basemesh.extCount;
            this.intCount = basemesh.intCount;
            this.dummyTGI = new TGI(basemesh.dummyTGI.Type, 0u, 0ul);
            this.abspos = basemesh.abspos;
            this.magic = basemesh.magic;
            this.version = basemesh.version;
            this.shaderHash = 0;
            this.MTNFsize = 0;
            this.mergeGrp = basemesh.mergeGrp;
            this.sortOrd = basemesh.sortOrd;
            this.numVerts = basemesh.numberVertices;
            this.Fcount = 3;
            this.vertform = new vertexForm[3] { new vertexForm(1, 1, 12), new vertexForm(2, 1, 12), new vertexForm(10, 4, 4) };
            this.vPositions = new position[this.numVerts];
            this.originalPositions = new position[this.numVerts];
            this.vNormals = new normal[this.numVerts];
            this.vertID = basemesh.vertID;
            this.numSubMeshes = basemesh.numSubMeshes;
            this.bytesperfacepnt = basemesh.bytesperfacepnt;
            this.numfacepoints = basemesh.numfacepoints;
            this.meshfaces = basemesh.meshfaces;
            this.skconIndex = 0;
            this.bonehashcount = basemesh.bonehashcount;
            this.bonehasharray = basemesh.bonehasharray;
            this.numtgi = 1;
            this.meshTGIs = new TGI[1] { new TGI(0, 0, 0L) };

            for (int i = 0; i < this.numVerts; i++)
            {
                this.vPositions[i] = new position(delta_positions[i].X, delta_positions[i].Y, delta_positions[i].Z);
                this.originalPositions[i] = new position(basemesh.originalPositions[i]);
                this.vNormals[i] = new normal(delta_normals[i].X, delta_normals[i].Y, delta_normals[i].Z);
            }
        }

        public GEOM(BinaryReader br)
        {
            this.ReadFile(br);
        }

        public void ReadFile(BinaryReader br)
        {
            if (br.BaseStream.Length < 12) return;
            this.package = "none";
            this.instance = new ulong[1];
            br.BaseStream.Position = 0;
            this.version1 = br.ReadInt32();
            this.count = br.ReadInt32();
            this.ind3 = br.ReadInt32();
            this.extCount = br.ReadInt32();
            this.intCount = br.ReadInt32();
            this.dummyTGI = new TGI(br);
            this.abspos = br.ReadInt32();
            this.meshsize = br.ReadInt32();
            this.magic = br.ReadChars(4);
            if (new string(this.magic) != "GEOM")
            {
                throw new MeshException("Not a valid GEOM file.");
            }
            this.version = br.ReadInt32();
            this.TGIoff = br.ReadInt32();
            this.TGIsize = br.ReadInt32();
            this.shaderHash = br.ReadUInt32();
            if (this.shaderHash != 0)
            {
                this.MTNFsize = br.ReadInt32();
                this.meshMTNF = new MTNF(br);
            }
            this.mergeGrp = br.ReadInt32();
            this.sortOrd = br.ReadInt32();
            this.numVerts = br.ReadInt32();
            this.Fcount = br.ReadInt32();
            this.vertform = new vertexForm[this.Fcount];
            for (int i = 0; i < this.Fcount; i++)
            {
                this.vertform[i] = new vertexForm(br);
            }
            int[] f = this.vertexFormatList;
            int uvInd = 0;
            for (int i = 0; i < f.Length; i++)
            {
                switch (f[i])
                {
                    case (1):
                        this.vPositions = new position[this.numVerts];
                        this.originalPositions = new position[this.numVerts];
                        break;
                    case (2):
                        this.vNormals = new normal[this.numVerts];
                        break;
                    case (3):
                        uvInd += 1;
                        break;
                    case (4):
                        this.vBones = new Bones[this.numVerts];
                        break;
                    case (6):
                        this.vTangents = new tangent[this.numVerts];
                        break;
                    case (7):
                        this.vTags = new tagval[this.numVerts];
                        break;
                    case (10):
                        this.vertID = new int[this.numVerts];
                        break;
                    default:
                        break;
                }
            }
            if (uvInd > 0) this.vUVs = new uv[uvInd][];
            for (int i = 0; i < uvInd; i++)
            {
                this.vUVs[i] = new uv[this.numVerts];
            }
            for (int i = 0; i < this.numVerts; i++)
            {
                int UVind = 0;
                for (int j = 0; j < this.Fcount; j++)
                {
                    switch (this.vertform[j].datatype)
                    {
                        case (1):
                            this.vPositions[i] = new position(br);
                            this.originalPositions[i] = new position(this.vPositions[i]);
                            break;
                        case (2):
                            this.vNormals[i] = new normal(br);
                            break;
                        case (3):
                            this.vUVs[UVind][i] = new uv(br);
                            UVind += 1;
                            break;
                        case (4):
                            if (this.vBones[i] == null) this.vBones[i] = new Bones();
                            this.vBones[i].ReadAssignments(br);
                            break;
                        case (5):
                            if (this.vBones[i] == null) this.vBones[i] = new Bones();
                            this.vBones[i].ReadWeights(br, this.vertform[j].subtype);
                            break;
                        case (6):
                            this.vTangents[i] = new tangent(br);
                            break;
                        case (7):
                            tagval t = new tagval(br);
                            if (this.vTags[i] == null) this.vTags[i] = t;
                            break;
                        case (10):
                            this.vertID[i] = br.ReadInt32();
                            break;
                        default:
                            break;
                    }
                }
            }
            this.numSubMeshes = br.ReadInt32();
            this.bytesperfacepnt = br.ReadByte();
            this.numfacepoints = br.ReadInt32();
            this.meshfaces = new Face[this.numfacepoints / 3];
            for (int i = 0; i < this.numfacepoints / 3; i++)
            {
                this.meshfaces[i] = new Face(br, this.bytesperfacepnt);
            }
            if (this.version == 5)
            {
                this.skconIndex = br.ReadInt32();
            }
            else if (this.version >= 12)
            {
                this.uvStitchCount = br.ReadInt32();
                uvStitches = new UVStitch[this.uvStitchCount];
                for (int i = 0; i < this.uvStitchCount; i++)
                {
                    uvStitches[i] = new UVStitch(br);
                }
                if (this.version >= 13)
                {
                    this.seamStitchCount = br.ReadInt32();
                    seamStitches = new SeamStitch[this.seamStitchCount];
                    for (int i = 0; i < this.seamStitchCount; i++)
                    {
                        seamStitches[i] = new SeamStitch(br, this.vUVs[0]);
                    }
                }
                this.slotCount = br.ReadInt32();
                slotrayIntersections = new SlotrayIntersection[this.slotCount];
                for (int i = 0; i < this.slotCount; i++)
                {
                    slotrayIntersections[i] = new SlotrayIntersection(br, this.version);
                }
            }
            this.bonehashcount = br.ReadInt32();
            this.bonehasharray = new uint[this.bonehashcount];
            for (int i = 0; i < this.bonehashcount; i++)
            {
                this.bonehasharray[i] = br.ReadUInt32();
            }
            if (br.BaseStream.Length <= br.BaseStream.Position) return;
            this.numtgi = br.ReadInt32();
            this.meshTGIs = new TGI[this.numtgi];
            for (int i = 0; i < this.numtgi; i++)
            {
                this.meshTGIs[i] = new TGI(br);
            }
            if (this.isMorph & this.skconIndex >= this.numtgi) this.skconIndex = 0;

            return;
        }

        public void WriteFile(BinaryWriter bw)
        {
            int tmp = 0;
            if (this.meshMTNF != null)
            {
                this.MTNFsize = this.meshMTNF.chunkSize;
            }
            else
            {
                this.MTNFsize = 0;
            }
            if (this.shaderHash != 0) tmp = this.MTNFsize + 4;
            this.TGIoff = 37 + (this.Fcount * 9) + tmp + (this.numVerts * this.vertexDataLength) + (this.numfacepoints * this.bytesperfacepnt) + (this.bonehashcount * 4);
            if (this.version == 5)
            {
                this.TGIoff += 4;
            }
            else if (this.version >= 12)
            {
                this.TGIoff += 8 + this.UVStitches_size + this.slotrayAdjustments_size;
                if (this.version >= 13) this.TGIoff += 4 + (this.seamStitchCount * 6);
            }
            this.meshsize = this.TGIoff + 16 + (this.numtgi * 16);
            this.TGIsize = 4 + (this.numtgi * 16);

            bw.Write(this.version1);
            bw.Write(this.count);
            bw.Write(this.ind3);
            bw.Write(this.extCount);
            bw.Write(this.intCount);
            dummyTGI.Write(bw);
            bw.Write(this.abspos);
            bw.Write(this.meshsize);
            bw.Write(this.magic);
            bw.Write(this.version);
            bw.Write(this.TGIoff);
            bw.Write(this.TGIsize);
            bw.Write(this.shaderHash);
            if (this.shaderHash != 0)
            {
                bw.Write(this.MTNFsize);
                this.meshMTNF.Write(bw);
            }
            bw.Write(this.mergeGrp);
            bw.Write(this.sortOrd);
            bw.Write(this.numVerts);
            bw.Write(this.Fcount);
            for (int i = 0; i < this.Fcount; i++)
            {
                this.vertform[i].vertexformatWrite(bw);
            }

            for (int i = 0; i < this.numVerts; i++)
            {
                int UVind = 0;
                for (int j = 0; j < this.Fcount; j++)
                {
                    switch (this.vertform[j].datatype)
                    {
                        case (1):
                            this.vPositions[i].Write(bw);
                            break;
                        case (2):
                            this.vNormals[i].Write(bw);
                            break;
                        case (3):
                            this.vUVs[UVind][i].Write(bw);
                            UVind += 1;
                            break;
                        case (4):
                            this.vBones[i].WriteAssignments(bw, this.version, this.bonehashcount - 1);
                            break;
                        case (5):
                            this.vBones[i].WriteWeights(bw, this.version);
                            break;
                        case (6):
                            this.vTangents[i].Write(bw);
                            break;
                        case (7):
                            this.vTags[i].Write(bw);
                            break;
                        case (10):
                            bw.Write(this.vertID[i]);
                            break;
                        default:
                            break;
                    }
                }
            }

            bw.Write(this.numSubMeshes);
            bw.Write(this.bytesperfacepnt);
            bw.Write(this.meshfaces.Length * 3);
            for (int i = 0; i < this.meshfaces.Length; i++)
            {
                this.meshfaces[i].Write(bw, this.bytesperfacepnt);
            }
            if (this.version == 5)
            {
                bw.Write(this.skconIndex);
            }
            else if (this.version >= 12)
            {
                bw.Write(this.uvStitchCount);
                for (int i = 0; i < this.uvStitchCount; i++)
                {
                    uvStitches[i].Write(bw);
                }
                if (this.version >= 13)
                {
                    bw.Write(this.seamStitchCount);
                    for (int i = 0; i < this.seamStitchCount; i++)
                    {
                        seamStitches[i].Write(bw);
                    }
                }
                bw.Write(this.slotCount);
                for (int i = 0; i < this.slotCount; i++)
                {
                    slotrayIntersections[i].Write(bw);
                }
            }
            bw.Write(this.bonehashcount);
            for (int i = 0; i < this.bonehashcount; i++)
            {
                bw.Write(this.bonehasharray[i]);
            }
            bw.Write(this.numtgi);
            for (int i = 0; i < this.numtgi; i++)
            {
                this.meshTGIs[i].Write(bw);
            }

            return;
        }

        public Stream Stream
        {
            get
            {
                Stream s = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(s);
                this.WriteFile(bw);
                s.Position = 0;
                return s;
            }
        }

        public void StandardizeFormat()
        {
            if (this.version < 14) this.version = 14;
            if (this.seamStitches == null) this.seamStitches = new SeamStitch[0];
            if (this.uvStitches == null) this.uvStitches = new UVStitch[0];
            if (this.SlotrayAdjustments == null) this.SlotrayAdjustments = new SlotrayIntersection[0];
            if (this.bonehasharray == null) this.bonehasharray = new uint[0];
            if (this.meshTGIs == null) this.meshTGIs = new TGI[0];
            if (!this.hasUVset(1))
            {
                this.InsertFormatDescriptor(MeshFormatFlag.UV);
                uv[] tmp = new uv[this.vPositions.Length];
                for (int i = 0; i < tmp.Length; i++) tmp[i] = new uv();
                uv[][] newuv = new uv[2][];
                newuv[0] = this.vUVs[0];
                newuv[1] = tmp;
                this.vUVs = newuv;
            }
            if (this.numberUVsets > 2)
            {
                for (int i = 2; i < this.numberUVsets; i++)
                {
                    if (this.hasUVset(i))
                    {
                        vertexForm[] newFormat = new vertexForm[this.vertexFormatList.Length - 1];
                        int uv2 = Array.LastIndexOf(this.vertexFormatList, (int)MeshFormatFlag.UV);
                        Array.Copy(this.vertform, newFormat, uv2);
                        Array.Copy(this.vertform, uv2 + 1, newFormat, uv2, this.vertform.Length - uv2 - 1);
                        this.vertform = newFormat;
                        this.Fcount--;
                    }
                }
                uv[][] newuv = new uv[2][];
                newuv[0] = this.vUVs[0];
                newuv[1] = this.vUVs[1];
                this.vUVs = newuv;
            }
            if (!this.hasNormals)
            {
                this.InsertFormatDescriptor(MeshFormatFlag.Normals);
                this.vNormals = new normal[this.vPositions.Length];
                for (int i = 0; i < this.vNormals.Length; i++) this.vNormals[i] = new normal();
            }
            if (!this.hasBones)
            {
                this.InsertFormatDescriptor(MeshFormatFlag.BoneIndices);
                this.InsertFormatDescriptor(MeshFormatFlag.BoneWeights);
                this.vBones = new Bones[this.vPositions.Length];
                for (int i = 0; i < this.vBones.Length; i++) this.vBones[i] = new Bones();
            }
            if (!this.hasTangents)
            {
                this.InsertFormatDescriptor(MeshFormatFlag.Tangents);
                this.vTangents = new tangent[this.vPositions.Length];
                for (int i = 0; i < this.vTangents.Length; i++) this.vTangents[i] = new tangent();
            }
            if (!this.hasTags)
            {
                this.InsertFormatDescriptor(MeshFormatFlag.Color);
                this.vTags = new tagval[this.vPositions.Length];
                for (int i = 0; i < this.vTags.Length; i++) this.vTags[i] = new tagval();
            }
            if (!this.hasVertexIDs)
            {
                this.InsertFormatDescriptor(MeshFormatFlag.VertexID);
                this.vertID = new int[this.vPositions.Length];
                for (int i = 0; i < this.vertID.Length; i++) this.vertID[i] = -1;
            }
        }

        internal void InsertFormatDescriptor(MeshFormatFlag flag)
        {
           // if (Array.IndexOf(this.vertexFormatList, (int)flag) >= 0) return;
            MeshFormatDatatype datatype = 0;
            byte byteLength = 0;
            switch (flag)
            {
                case MeshFormatFlag.Position:
                    datatype = MeshFormatDatatype.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.Normals:
                    datatype = MeshFormatDatatype.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.UV:
                    datatype = MeshFormatDatatype.Float;
                    byteLength = 8;
                    break;
                case MeshFormatFlag.BoneIndices:
                    datatype = MeshFormatDatatype.Byte4;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.BoneWeights:
                    datatype = MeshFormatDatatype.Byte4;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.Tangents:
                    datatype = MeshFormatDatatype.Float;
                    byteLength = 12;
                    break;
                case MeshFormatFlag.Color:
                    datatype = MeshFormatDatatype.Color;
                    byteLength = 4;
                    break;
                case MeshFormatFlag.VertexID:
                    datatype = MeshFormatDatatype.Uint;
                    byteLength = 4;
                    break;
                default:
                    datatype = MeshFormatDatatype.Float;
                    byteLength = 12;
                    break;
            }
            vertexForm[] newFormat = new vertexForm[this.vertexFormatList.Length + 1];
            if (flag == MeshFormatFlag.UV && Array.IndexOf(this.vertexFormatList, (int)flag) >= 0)
            {
                int uv0 = Array.IndexOf(this.vertexFormatList, (int)flag);
                Array.Copy(this.vertform, newFormat, uv0 + 1);
                newFormat[uv0 + 1] = new vertexForm((int)flag, (int)datatype, byteLength);
                Array.Copy(this.vertform, uv0 + 1, newFormat, uv0 + 2, this.vertform.Length - uv0 - 1);
            }
            else
            {
                Array.Copy(this.vertform, newFormat, this.vertexFormatList.Length);
                newFormat[this.vertexFormatList.Length] = new vertexForm((int)flag, (int)datatype, byteLength);
            }
            this.vertform = newFormat;
            this.Fcount += 1;
        }

        public void MatchPartSeamStitches()
        {
            if (this.seamStitches == null) return;
            List<ushort> ids = new List<ushort>();
            List<int> numHits = new List<int>();
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            foreach (SeamStitch s in this.seamStitches)
            {
                int i = ids.IndexOf(s.VertID);
                if (i < 0)
                {
                    Vector3 pos = new Vector3(this.vPositions[s.Index].Coordinates);
                    Vector3 norm = new Vector3(this.vNormals[s.Index].Coordinates);
                    ids.Add(s.VertID);
                    positions.Add(pos);
                    normals.Add(norm);
                    numHits.Add(1);
                }
                else
                {
                    Vector3 pos = new Vector3(this.vPositions[s.Index].Coordinates);
                    Vector3 norm = new Vector3(this.vNormals[s.Index].Coordinates);
                    Vector3 newPos = (pos + (numHits[i] * positions[i])) / (numHits[i] + 1);
                    Vector3 newNorm = (norm + (numHits[i] * normals[i])) / (numHits[i] + 1);
                    positions[i] = newPos;
                    normals[i] = newNorm;
                    numHits[i]++;
                }
            }

            foreach (SeamStitch s in this.seamStitches)
            {
                int i = ids.IndexOf(s.VertID);
                if (i >= 0)
                {
                    this.vPositions[s.Index] = new position(positions[i].Coordinates);
                    this.vNormals[s.Index] = new normal(normals[i].Coordinates);
                }
            }
        }

        public static void MatchSeamStitches(GEOM geom1, GEOM geom2)
        {
            MatchSeamStitches(geom1, geom2, null);
        }
        public static void MatchSeamStitches(GEOM geom1, GEOM geom2, GEOM geom3)
        {
            MatchSeamStitches(geom1, geom2, geom3, null);
        }
        public static void MatchSeamStitches(GEOM geom1, GEOM geom2, GEOM geom3, GEOM geom4)
        {
            if (geom1.seamStitches == null || geom2.seamStitches == null) return;
            List<ushort> ids = new List<ushort>();
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            foreach (SeamStitch s1 in geom1.seamStitches)
            {
                foreach (SeamStitch s2 in geom2.seamStitches)
                {
                    if (s1.VertID == s2.VertID)
                    {
                        int i = ids.IndexOf(s1.VertID);
                        if (i < 0)
                        {
                            Vector3 pos1 = new Vector3(geom1.vPositions[s1.Index].Coordinates);
                            Vector3 pos2 = new Vector3(geom2.vPositions[s2.Index].Coordinates);
                            Vector3 pos = (pos1 + pos2) / 2f;
                            Vector3 norm1 = new Vector3(geom1.vNormals[s1.Index].Coordinates);
                            Vector3 norm2 = new Vector3(geom2.vNormals[s2.Index].Coordinates);
                            Vector3 norm = (norm1 + norm2) / 2f;
                            ids.Add(s1.VertID);
                            positions.Add(pos);
                            normals.Add(norm);
                        }
                        continue;
                    }
                }
            }

            foreach (SeamStitch s1 in geom1.seamStitches)
            {
                int i = ids.IndexOf(s1.VertID);
                if (i >= 0)
                {
                    geom1.vPositions[s1.Index] = new position(positions[i].Coordinates);
                    geom1.vNormals[s1.Index] = new normal(normals[i].Coordinates);
                }
            }
            foreach (SeamStitch s2 in geom2.seamStitches)
            {
                int i = ids.IndexOf(s2.VertID);
                if (i >= 0)
                {
                    geom2.vPositions[s2.Index] = new position(positions[i].Coordinates);
                    geom2.vNormals[s2.Index] = new normal(normals[i].Coordinates);
                }
            }
            if (geom3 != null)
            {
                foreach (SeamStitch s3 in geom3.seamStitches)
                {
                    int i = ids.IndexOf(s3.VertID);
                    if (i >= 0)
                    {
                        geom3.vPositions[s3.Index] = new position(positions[i].Coordinates);
                        geom3.vNormals[s3.Index] = new normal(normals[i].Coordinates);
                    }
                }
            }
            if (geom4 != null)
            {
                foreach (SeamStitch s4 in geom4.seamStitches)
                {
                    int i = ids.IndexOf(s4.VertID);
                    if (i >= 0)
                    {
                        geom4.vPositions[s4.Index] = new position(positions[i].Coordinates);
                        geom4.vNormals[s4.Index] = new normal(normals[i].Coordinates);
                    }
                }
            }

            //foreach (SeamStitch s1 in geom1.seamStitches)
            //{
            //    foreach (SeamStitch s2 in geom2.seamStitches)
            //    {
            //        if (s1.VertID == s2.VertID)
            //        {
            //            int i = ids.IndexOf(s1.VertID);
            //            if (i >= 0)
            //            {
            //                geom1.vPositions[s1.Index] = new position(positions[i].Coordinates);
            //                geom2.vPositions[s2.Index] = new position(positions[i].Coordinates);
            //                geom1.vNormals[s1.Index] = new normal(normals[i].Coordinates);
            //                geom2.vNormals[s2.Index] = new normal(normals[i].Coordinates);
            //            }
            //            else
            //            {
            //                Vector3 pos1 = new Vector3(geom1.vPositions[s1.Index].Coordinates);
            //                Vector3 pos2 = new Vector3(geom2.vPositions[s2.Index].Coordinates);
            //                Vector3 pos = (pos1 + pos2) / 2f;
            //                geom1.vPositions[s1.Index] = new position(pos.Coordinates);
            //                geom2.vPositions[s2.Index] = new position(pos.Coordinates);
            //                Vector3 norm1 = new Vector3(geom1.vNormals[s1.Index].Coordinates);
            //                Vector3 norm2 = new Vector3(geom2.vNormals[s2.Index].Coordinates);
            //                Vector3 norm = (norm1 + norm2) / 2f;
            //                geom1.vNormals[s1.Index] = new normal(norm.Coordinates);
            //                geom2.vNormals[s2.Index] = new normal(norm.Coordinates);
            //                ids.Add(s1.VertID);
            //                positions.Add(pos);
            //                normals.Add(norm);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Fills gap at waist when wearing a female top and male bottom
        /// </summary>
        //public void FillWaistGap()
        //{
        //    uint femaleWaist = (uint)SeamType.WaistAdultFemale;
        //    uint maleWaist = (uint)SeamType.WaistAdultMale;
        //    List<SeamStitch> femaleVerts = new List<SeamStitch>();
        //    List<SeamStitch> maleVerts = new List<SeamStitch>();
        //    for (int i = 0; i < this.seamStitches.Length; i++)
        //    {
        //        //if (this.seamStitches[i].SeamType == femaleWaist && femaleVerts.IndexOf(this.seamStitches[i]) < 0) femaleVerts.Add(this.seamStitches[i]);
        //        //if (this.seamStitches[i].SeamType == maleWaist && maleVerts.IndexOf(this.seamStitches[i]) < 0) maleVerts.Add(this.seamStitches[i]);
        //        if (this.seamStitches[i].SeamType == femaleWaist && !femaleVerts.Contains(this.seamStitches[i])) femaleVerts.Add(this.seamStitches[i]);
        //        if (this.seamStitches[i].SeamType == maleWaist && !femaleVerts.Contains(this.seamStitches[i])) maleVerts.Add(this.seamStitches[i]);
        //    }
        //    if (femaleVerts.Count == 0 || maleVerts.Count == 0) return;
        //    //femaleVerts.Distinct().ToList();
        //    //maleVerts.Distinct().ToList();
        //    femaleVerts.Sort();
        //    maleVerts.Sort();
        //    SeamStitch femaleLast = femaleVerts[0];
        //    femaleVerts.RemoveAt(0);
        //    femaleVerts.Add(femaleLast);
        //    SeamStitch maleLast = maleVerts[0];
        //    maleVerts.RemoveAt(0);
        //    maleVerts.Add(maleLast);
        //    if (femaleVerts.Count != maleVerts.Count) return;
        //    List<Face> newFaces = new List<Face>(this.meshfaces);
        //    for (int i = 0; i < femaleVerts.Count - 1; i++)
        //    {
        //        Face face = new Face(femaleVerts[i].Index, femaleVerts[i + 1].Index, maleVerts[i].Index);
        //        newFaces.Add(face);
        //        Face face2 = new Face(maleVerts[i].Index, femaleVerts[i + 1].Index, maleVerts[i + 1].Index);
        //        newFaces.Add(face2);
        //    }
        //    //Face face3 = new Face(femaleVerts[femaleVerts.Count - 1].Index, femaleVerts[0].Index, maleVerts[maleVerts.Count - 1].Index);
        //    //newFaces.Add(face3);
        //    //Face face4 = new Face(maleVerts[maleVerts.Count - 1].Index, femaleVerts[0].Index, maleVerts[0].Index);
        //    //newFaces.Add(face4);

        //    this.meshfaces = newFaces.ToArray();
        //    this.numfacepoints = this.meshfaces.Length * 3;
        //}
        
        /// <summary>
        /// Fills gap at waist when wearing a female top and male bottom by adding a row of faces to the top
        /// </summary>
        /// <param name="geom">Male bottom mesh to match with</param>
        //public void FillWaistGap(GEOM bottom)
        //{
        //    uint femaleWaist = (uint)SeamType.WaistAdultFemale;
        //    uint maleWaist = (uint)SeamType.WaistAdultMale;
        //    List<SeamStitch> femaleVerts = new List<SeamStitch>();
        //    List<SeamStitch> maleVerts = new List<SeamStitch>();
        //    List<vertexData> femaleVertData = this.GetVertexList();
        //    List<vertexData> maleVertData = bottom.GetVertexList();
        //    for (int i = 0; i < this.seamStitches.Length; i++)
        //    {
        //        if (this.seamStitches[i].SeamType == femaleWaist) femaleVerts.Add(this.seamStitches[i]);
        //    }
        //    for (int i = 0; i < bottom.seamStitches.Length; i++)
        //    {        
        //        if (bottom.seamStitches[i].SeamType == maleWaist)
        //        {
        //            int vertInd = (int)bottom.seamStitches[i].Index;
        //            int newInd = femaleVertData.Count;
        //            femaleVertData.Add(maleVertData[vertInd]);
        //            SeamStitch newStitch = new SeamStitch(bottom.seamStitches[i]);
        //            newStitch.Index = (uint)newInd;
        //            maleVerts.Add(newStitch);
        //        }
        //    }
        //    if (femaleVerts.Count == 0 || maleVerts.Count == 0) return;
        //    SetVertexList(femaleVertData);
        //    this.numVerts = femaleVertData.Count;
        //    femaleVerts.Sort();
        //    maleVerts.Sort();
        //    SeamStitch femaleLast = femaleVerts[0];
        //    femaleVerts.RemoveAt(0);
        //    femaleVerts.Add(femaleLast);
        //    SeamStitch maleLast = maleVerts[0];
        //    maleVerts.RemoveAt(0);
        //    maleVerts.Add(maleLast);
        //    if (femaleVerts.Count != maleVerts.Count) return;
        //    List<Face> newFaces = new List<Face>(this.meshfaces);

        //    GEOM tmp = new GEOM(this);
        //    List<vertexData> tmpVertData = new List<vertexData>();
        //    List<SeamStitch> tmpSeams = new List<SeamStitch>();
        //    List<Face> tmpFaces = new List<Face>();

        //    for (int i = 0; i < femaleVerts.Count - 1; i++)
        //    {
        //        Face face = new Face(femaleVerts[i].Index, femaleVerts[i + 1].Index, maleVerts[i].Index);
        //        newFaces.Add(face);
        //        Face face2 = new Face(maleVerts[i].Index, femaleVerts[i + 1].Index, maleVerts[i + 1].Index);
        //        newFaces.Add(face2);

        //        int tmpInd1 = tmpVertData.Count;
        //        SeamStitch tmpStitch1 = new SeamStitch(femaleVerts[i]);
        //        tmpStitch1.Index = (uint)tmpInd1;
        //        tmpSeams.Add(tmpStitch1);
        //        tmpVertData.Add(femaleVertData[(int)femaleVerts[i].Index]);
        //        SeamStitch tmpStitch2 = new SeamStitch(femaleVerts[i + 1]);
        //        tmpStitch2.Index = (uint)tmpInd1 + 2;
        //        tmpSeams.Add(tmpStitch2);
        //        int tmpInd3 = tmpVertData.Count;
        //        SeamStitch tmpStitch3 = new SeamStitch(maleVerts[i]);
        //        tmpStitch3.Index = (uint)tmpInd3;
        //        tmpSeams.Add(tmpStitch3);
        //        tmpVertData.Add(femaleVertData[(int)maleVerts[i].Index]);
        //        SeamStitch tmpStitch4 = new SeamStitch(maleVerts[i + 1]);
        //        tmpStitch4.Index = (uint)tmpInd3 + 2;
        //        tmpSeams.Add(tmpStitch4);
        //        Face face3 = new Face(tmpStitch1.Index, tmpStitch2.Index, tmpStitch3.Index);
        //        tmpFaces.Add(face3);
        //        Face face4 = new Face(tmpStitch3.Index, tmpStitch2.Index, tmpStitch4.Index);
        //        tmpFaces.Add(face4);
        //    }
        //    tmpVertData.Add(femaleVertData[(int)femaleVerts[femaleVerts.Count - 1].Index]);
        //    tmpVertData.Add(femaleVertData[(int)maleVerts[femaleVerts.Count - 1].Index]);
        //    tmp.SetVertexList(tmpVertData);
        //    tmp.numVerts = tmpVertData.Count;
        //    tmp.meshfaces = tmpFaces.ToArray();
        //    tmp.numfacepoints = tmp.meshfaces.Length * 3;
        //    tmp.seamStitchCount = tmpSeams.Count;
        //    tmp.seamStitches = tmpSeams.ToArray();
        //    tmp.uvStitchCount = 0;
        //    tmp.uvStitches = new UVStitch[0];
        //    tmp.slotCount = 0;
        //    tmp.slotrayIntersections = new SlotrayIntersection[0];
        //    tmp.FixUnusedBones();
        //    Form1.WriteGEOM("Save waist filler GEOM", tmp, "");

        //    this.numVerts = this.vPositions.Length;
        //    this.meshfaces = newFaces.ToArray();
        //    this.numfacepoints = this.meshfaces.Length * 3;
        //}

        public static void MatchSeamVerts(GEOM geom1, GEOM geom2)
        {
            List<UVStitch> geom1Stitches = new List<UVStitch>(geom1.uvStitches);
            List<UVStitch> geom2Stitches = new List<UVStitch>(geom2.uvStitches);
            for (int i = 0; i < geom1.numVerts; i++)
            {
                for (int j = 0; j < geom2.numVerts; j++)
                {
                    if (new Vector3(geom1.getPosition(i)).positionMatches(geom2.getPosition(j)))
                    {
                        byte[] sourceBones = geom1.getBones(i);
                        byte[] targetBones = new byte[sourceBones.Length];
                        byte[] sourceWeights = geom1.getBoneWeights(i);
                        for (int k = 0; k < sourceBones.Length; k++)
                        {
                            if (!TranslateBone(geom1.bonehasharray[sourceBones[k]], geom2.bonehasharray, out targetBones[k]) && sourceWeights[k] > 0)
                            {
                                List<uint> tmp = new List<uint>(geom2.bonehasharray);
                                tmp.Add(geom1.bonehasharray[sourceBones[k]]);
                                targetBones[k] = (byte)(tmp.Count - 1);
                            }
                        }
                        geom2.setBones(j, targetBones);
                        geom2.setBoneWeights(j, sourceWeights);
                        geom2.setNormal(j, geom1.getNormal(i));
                        List<Vector2> geom1Stitch = new List<Vector2>();
                        List<Vector2> geom2Stitch = new List<Vector2>();
                        int geom1Ind = -1;
                        int geom2Ind = -1;
                        for (int k = 0; k < geom1.uvStitches.Length; k++)
                        {
                            if (geom1.uvStitches[k].Index == i)
                            {
                                geom1Stitch.AddRange(geom1Stitches[k].UV1Vectors);
                                geom1Ind = k;
                                break;
                            }
                        }
                        for (int k = 0; k < geom2.uvStitches.Length; k++)
                        {
                            if (geom2.uvStitches[k].Index == j)
                            {
                                geom2Stitch.AddRange(geom2Stitches[k].UV1Vectors);
                                geom2Ind = k;
                                break;
                            }
                        }
                        Vector2 geom1UV1 = new Vector2(geom1.getUV(i, 1));
                        geom1UV1.X = Math.Abs(geom1UV1.X);
                        if (geom2Stitch.IndexOf(geom1UV1) < 0) geom2Stitch.Add(geom1UV1);
                        Vector2 geom2UV1 = new Vector2(geom2.getUV(j, 1));
                        geom2UV1.X = Math.Abs(geom2UV1.X);
                        if (geom1Stitch.IndexOf(geom2UV1) < 0) geom1Stitch.Add(geom2UV1);
                        if (geom1Ind >= 0)
                        {
                            geom1Stitches[geom1Ind] = new UVStitch(i, geom1Stitch.ToArray());
                        }
                        else
                        {
                            geom1Stitches.Add(new UVStitch(i, geom1Stitch.ToArray()));
                        }
                        if (geom2Ind >= 0)
                        {
                            geom2Stitches[geom2Ind] = new UVStitch(j, geom2Stitch.ToArray());
                        }
                        else
                        {
                            geom2Stitches.Add(new UVStitch(j, geom2Stitch.ToArray()));
                        }
                    }
                }
            }
            geom1.uvStitches = geom1Stitches.ToArray();
            geom2.uvStitches = geom2Stitches.ToArray();
        }

        public static void MatchSeamVerts(GEOM geom)
        {
            List<UVStitch> geomStitches = new List<UVStitch>(geom.uvStitches);
            for (int i = 0; i < geom.numVerts; i++)
            {
                for (int j = 0; j < geom.numVerts; j++)
                {
                    if (new Vector3(geom.getPosition(i)).positionMatches(geom.getPosition(j)))
                    {
                        if (i == j) continue;
                        byte[] sourceBones = geom.getBones(i);
                        byte[] targetBones = new byte[sourceBones.Length];
                        byte[] sourceWeights = geom.getBoneWeights(i);
                        geom.setBones(j, sourceBones);
                        geom.setBoneWeights(j, sourceWeights);
                        geom.setNormal(j, geom.getNormal(i));
                        UVStitch stitch = null;
                        for (int k = 0; k < geom.uvStitches.Length; k++)
                        {
                            if (geom.uvStitches[k].Index == i)
                            {
                                stitch = new UVStitch(geom.uvStitches[k]);
                                break;
                            }
                        }
                        for (int k = 0; k < geom.uvStitches.Length; k++)
                        {
                            if (geom.uvStitches[k].Index == j)
                            {
                                if (stitch != null) geom.uvStitches[k] = stitch;
                                break;
                            }
                        }
                    }
                }
            }
        }

        internal static bool TranslateBone(uint boneHash, uint[] bonehashArray, out byte bone)
        {
            for (int i = 0; i < bonehashArray.Length; i++)
            {
                if (boneHash == bonehashArray[i])
                {
                    bone = (byte)i;
                    return true;
                }
            }
            bone = 0;
            return false;
        }

        public void MatchFormats(vertexForm[] vertexFormatToMatch)
        {
            int uvInd = 0;
            for (int i = 0; i < vertexFormatToMatch.Length; i++)
            {
                switch (vertexFormatToMatch[i].datatype)
                {
                    case (1):
                        if (this.vPositions == null || this.vPositions.Length != this.numVerts)
                        {
                            this.vPositions = new position[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vPositions[j] = new position();
                            }
                        }
                        break;
                    case (2):
                        if (this.vNormals == null || this.vNormals.Length != this.numVerts)
                        {
                            this.vNormals = new normal[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vNormals[j] = new normal();
                            }
                        }
                        break;
                    case (3):
                        uvInd += 1;
                        break;
                    case (4):
                        if (this.vBones == null || this.vBones.Length != this.numVerts)
                        {
                            this.vBones = new Bones[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vBones[j] = new Bones();
                            }
                        }
                        break;
                    case (5):
                        if (this.vBones == null || this.vBones.Length != this.numVerts)
                        {
                            this.vBones = new Bones[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vBones[j] = new Bones();
                            }
                        }
                        break;
                    case (6):
                        if (!this.hasTangents) this.vTangents = new tangent[this.numVerts];
                        break;
                    case (7):
                        if (this.vTags == null || this.vTags.Length != this.numVerts)
                        {
                            this.vTags = new tagval[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vTags[j] = new tagval();
                            }
                        }
                        break;
                    case (10):
                        if (this.vertID == null || this.vertID.Length != this.numVerts)
                        {
                            this.vertID = new int[this.numVerts];
                            for (int j = 0; j < this.numVerts; j++)
                            {
                                this.vertID[j] = -1;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (uvInd > 0 && this.numberUVsets != uvInd)
            {
                uv[][] newUV = new uv[uvInd][];
                for (int i = 0; i < uvInd; i++)
                {
                    newUV[i] = new uv[this.numVerts];
                    if (this.hasUVset(i))
                    {
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            newUV[i][j] = this.vUVs[i][j];
                        }
                    }
                    else
                    {
                        for (int j = 0; j < this.numVerts; j++)
                        {
                            newUV[i][j] = new uv();
                        }
                    }
                }
                this.vUVs = newUV;
            }
            vertexForm[] newFormat = new vertexForm[vertexFormatToMatch.Length];
            for (int i = 0; i < vertexFormatToMatch.Length; i++)
            {
                newFormat[i] = new vertexForm(vertexFormatToMatch[i].formatDataType, vertexFormatToMatch[i].formatSubType, vertexFormatToMatch[i].formatDataLength);
            }
            this.vertform = newFormat;
            this.Fcount = this.vertform.Length;
        }

        public void AppendMesh(GEOM meshToAppend)
        {
            if (meshToAppend == null) return;
            if (!this.isValid) throw new MeshException("Not a valid mesh!");
            if (!meshToAppend.isValid) throw new MeshException("The mesh to be appended is not a valid mesh!");

            this.package += ", " + meshToAppend.package;
            ulong[] tmp = new ulong[this.instance.Length + meshToAppend.instance.Length];
            Array.Copy(this.instance, tmp, this.instance.Length);
            Array.Copy(meshToAppend.instance, 0, tmp, this.instance.Length, meshToAppend.instance.Length);
            this.instance = tmp;

            int uvInd = 0;
            for (int i = 0; i < this.vertexFormatList.Length; i++)
            {
                switch (this.vertexFormatList[i])
                {
                    case (1):
                        position[] newPos = new position[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.vPositions, 0, newPos, 0, this.numVerts);
                        Array.Copy(meshToAppend.vPositions, 0, newPos, this.numVerts, meshToAppend.numVerts);
                        this.vPositions = newPos;
                        position[] newOrig = new position[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.originalPositions, 0, newOrig, 0, this.numVerts);
                        Array.Copy(meshToAppend.originalPositions, 0, newOrig, this.numVerts, meshToAppend.numVerts);
                        this.originalPositions = newOrig;
                        break;
                    case (2):
                        normal[] newNorm = new normal[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.vNormals, 0, newNorm, 0, this.numVerts);
                        Array.Copy(meshToAppend.vNormals, 0, newNorm, this.numVerts, meshToAppend.numVerts);
                        this.vNormals = newNorm;
                        break;
                    case (3):
                        uvInd += 1;
                        break;
                    case (4):
                        Bones[] newBones = new Bones[this.numVerts + meshToAppend.numVerts];
                        List<uint> tmpBoneHash = new List<uint>(this.bonehasharray);
                        foreach (uint h in meshToAppend.bonehasharray)
                        {
                            if (tmpBoneHash.IndexOf(h) < 0)
                            {
                                tmpBoneHash.Add(h);         //add bones from second mesh
                            }
                        }
                        uint[] newbonehasharray = tmpBoneHash.ToArray();

                        for (int j = 0; j < this.numVerts; j++)                 //add updated bone assignments for this mesh
                        {
                            byte[] oldBones = this.getBones(j);
                            byte[] oldWeights = this.getBoneWeights(j);
                            byte[] tmpBones = new byte[oldBones.Length];
                            for (int k = 0; k < oldBones.Length; k++)
                            {
                                if (oldWeights[k] > 0 & oldBones[k] < this.bonehasharray.Length)                         // if it's a valid bone
                                {                                               // find index of bone hash in new bone list
                                    tmpBones[k] = (byte)Array.IndexOf(newbonehasharray, this.bonehasharray[oldBones[k]]);
                                }
                                else
                                {
                                    tmpBones[k] = 0;
                                }
                            }
                            newBones[j] = new Bones(tmpBones, oldWeights);
                        }
                        for (int j = 0; j < meshToAppend.numVerts; j++)        //add updated bone assignments for appended mesh
                        {
                            byte[] oldBones = meshToAppend.getBones(j);
                            byte[] oldWeights = meshToAppend.getBoneWeights(j);
                            byte[] tmpBones = new byte[oldBones.Length];
                            for (int k = 0; k < oldBones.Length; k++)
                            {
                                if (oldWeights[k] > 0 & oldBones[k] < meshToAppend.bonehasharray.Length)
                                {
                                    tmpBones[k] = (byte)Array.IndexOf(newbonehasharray, meshToAppend.bonehasharray[oldBones[k]]);
                                }
                                else
                                {
                                    tmpBones[k] = 0;
                                }
                            }
                            newBones[j + this.numVerts] = new Bones(tmpBones, oldWeights);
                        }
                        this.vBones = newBones;
                        this.bonehasharray = newbonehasharray;
                        this.bonehashcount = newbonehasharray.Length;
                        break;
                    case (6):
                        tangent[] newTan = new tangent[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.vTangents, 0, newTan, 0, this.numVerts);
                        if (meshToAppend.hasTangents)
                        {
                            Array.Copy(meshToAppend.vTangents, 0, newTan, this.numVerts, meshToAppend.numVerts);
                        }
                        else
                        {
                            for (int v = this.numVerts; v < newTan.Length; v++) newTan[v] = new tangent();
                        }
                        this.vTangents = newTan;
                        break;
                    case (7):
                        tagval[] newTag = new tagval[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.vTags, 0, newTag, 0, this.numVerts);
                        if (meshToAppend.hasTags)
                        {
                            Array.Copy(meshToAppend.vTags, 0, newTag, this.numVerts, meshToAppend.numVerts);
                        }
                        else
                        {
                            for (int v = this.numVerts; v < newTag.Length; v++) newTag[v] = new tagval();
                        }
                        this.vTags = newTag;
                        break;
                    case (10):
                        int[] newIDs = new int[this.numVerts + meshToAppend.numVerts];
                        Array.Copy(this.vertID, 0, newIDs, 0, this.numVerts);
                        if (meshToAppend.hasVertexIDs) Array.Copy(meshToAppend.vertID, 0, newIDs, this.numVerts, meshToAppend.numVerts);
                        this.vertID = newIDs;
                        break;
                    default:
                        break;
                }
            }
            if (uvInd > 0)
            {
                uv[][] newUV = new uv[uvInd][];
                for (int i = 0; i < uvInd; i++)
                {
                    newUV[i] = new uv[this.numVerts + meshToAppend.numVerts];
                    for (int j = 0; j < this.numVerts; j++)
                    {
                        newUV[i][j] = this.vUVs[i][j];
                    }
                    if (meshToAppend.hasUVset(i))
                    {
                        for (int j = 0; j < meshToAppend.numVerts; j++)
                        {
                            newUV[i][j + this.numVerts] = meshToAppend.vUVs[i][j];
                        }
                    }
                    else
                    {
                        for (int j = 0; j < meshToAppend.numVerts; j++)
                        {
                            newUV[i][j + this.numVerts] = new uv();
                        }
                    }
                }
                this.vUVs = newUV;
            }

            Face[] newFaces = new Face[this.numberFaces + meshToAppend.numberFaces];
            Array.Copy(this.meshfaces, 0, newFaces, 0, this.numberFaces);
            for (int i = 0; i < meshToAppend.numberFaces; i++)
            {
                newFaces[i + this.numberFaces] = new Face(meshToAppend.meshfaces[i].facePoint0 + this.numVerts,
                    meshToAppend.meshfaces[i].facePoint1 + this.numVerts, meshToAppend.meshfaces[i].facePoint2 + this.numVerts);
            }
            this.meshfaces = newFaces;

            if (this.version >= 12)
            {
                UVStitch[] adj = new UVStitch[(this.uvStitches != null ? this.uvStitches.Length : 0) + 
                    (meshToAppend.uvStitches != null ? meshToAppend.uvStitches.Length : 0)];
                if (this.uvStitches != null) Array.Copy(this.uvStitches, 0, adj, 0, this.uvStitches.Length);
                if (meshToAppend.uvStitches != null)
                {
                    for (int i = 0; i < meshToAppend.uvStitches.Length; i++)
                    {
                        adj[i + this.uvStitches.Length] = new UVStitch(meshToAppend.uvStitches[i]);
                        adj[i + this.uvStitches.Length].Index += this.numVerts;
                    }
                }
                this.uvStitches = adj;
                this.uvStitchCount = adj.Length;
                if (this.version >= 13)
                {
                    SeamStitch[] seam = new SeamStitch[(this.seamStitches != null ? this.seamStitches.Length : 0) + 
                        (meshToAppend.seamStitches != null ? meshToAppend.seamStitches.Length : 0)];
                    if (this.seamStitches != null) Array.Copy(this.seamStitches, 0, seam, 0, this.seamStitches.Length);
                    if (meshToAppend.seamStitches != null)
                    {
                        for (int i = 0; i < meshToAppend.seamStitches.Length; i++)
                        {
                            seam[i + this.seamStitches.Length] = new SeamStitch(meshToAppend.seamStitches[i]);
                            seam[i + this.seamStitches.Length].Index += (uint)this.numVerts;
                        }
                    }
                    this.seamStitches = seam;
                    this.seamStitchCount = seam.Length;
                }
                SlotrayIntersection[] adj2 = new SlotrayIntersection[(this.slotrayIntersections != null ? this.slotrayIntersections.Length : 0) + 
                    (meshToAppend.slotrayIntersections != null ? meshToAppend.slotrayIntersections.Length : 0)];
                if (this.slotrayIntersections != null) Array.Copy(this.slotrayIntersections, 0, adj2, 0, this.slotrayIntersections.Length);
                if (meshToAppend.slotrayIntersections != null)
                {
                    for (int i = 0; i < meshToAppend.slotrayIntersections.Length; i++)
                    {
                        adj2[i + this.slotrayIntersections.Length] = new SlotrayIntersection(meshToAppend.slotrayIntersections[i]);
                        int[] f = meshToAppend.slotrayIntersections[i].TrianglePointIndices;
                        for (int j = 0; j < f.Length; j++)
                        {
                            f[j] += this.numVerts;
                        }
                        adj2[i + this.slotrayIntersections.Length].TrianglePointIndices = f;
                    }
                }
                this.slotrayIntersections = adj2;
                this.slotCount = adj2.Length;
            }
            this.numVerts += meshToAppend.numVerts;
            this.numfacepoints += meshToAppend.numfacepoints;
        }

        public bool hasBlueVertexColor
        {
            get
            {
                for (int i = 0; i < this.numberVertices; i++)
                {
                    if ((this.getTagval(i) & 0x00FF0000) > 0) return true;
                }
                return false;
            }
        }

        public void AutoVertexID(GEOM refMesh)
        {
            if (!this.hasVertexIDs)
            {
                this.insertVertexIDinFormatList();
                this.vertID = new int[this.numberVertices];
            }
            Vector3[] refVerts = new Vector3[refMesh.numberVertices];
            for (int i = 0; i < refMesh.numberVertices; i++)
            {
                refVerts[i] = new Vector3(refMesh.getPosition(i));
            }
            for (int i = 0; i < this.numberVertices; i++)
            {
                Vector3 tmpVert = new Vector3(this.getPosition(i));
                int ind = tmpVert.NearestPointIndexSimple(refVerts);
                this.vertID[i] = refMesh.vertID[ind];
            }
            this.copyFaceMorphs = true;
        }

        internal void insertVertexIDinFormatList()
        {
            if (Array.IndexOf(this.vertexFormatList, 10) >= 0) return;
            vertexForm[] newFormat = new vertexForm[this.vertexFormatList.Length + 1];
            if (Array.IndexOf(this.vertexFormatList, 5) >= 0)
            {
                int ind = 0;
                for (int i = 0; i < this.vertexFormatList.Length; i++)
                {
                    newFormat[ind] = this.vertform[i];
                    ind += 1;
                    if (this.vertform[i].formatDataType == 5)
                    {
                        newFormat[ind] = new vertexForm(10, 4, 4);
                        ind += 1;
                    }
                }
            }
            else
            {
                Array.Copy(this.vertform, newFormat, this.vertexFormatList.Length);
                newFormat[this.vertexFormatList.Length] = new vertexForm(10, 4, 4);
            }
            this.vertform = newFormat;
            this.Fcount += 1;
        }

        public void FixBoneWeights()
        {
            if (this.bonehasharray == null || this.bonehasharray.Length == 0) return;
            for (int i = 0; i < this.numVerts; i++)
            {
                byte[] wbones = this.getBoneWeights(i);
                byte[] bones = this.getBones(i);
                int totweight = 0;
                int maxbone = 0;
                for (int j = 0; j < 4; j++)
                {
                    totweight += wbones[j];
                    if (wbones[j] > 0) maxbone = j;
                }
                if (totweight > 255)
                {
                    wbones[maxbone] += (byte)(255 - totweight);
                }
                else if (totweight < 255)
                {
                    wbones[0] += (byte)(255 - totweight);
                }
                for (int j = 0; j < 3; j++)
                {
                    if (wbones[j] < wbones[j + 1])
                    {
                        byte tmp = bones[j];
                        bones[j] = bones[j + 1];
                        bones[j + 1] = tmp;
                        tmp = wbones[j];
                        wbones[j] = wbones[j + 1];
                        wbones[j + 1] = tmp;
                    }
                }
                this.setBones(i, bones);
                this.setBoneWeights(i, wbones);
            }
        }

        public void FixUnusedBones()
        {
            if (this.bonehasharray == null || this.bonehasharray.Length == 0) return;
            List<byte> usedBones = new List<byte>();
            for (int i = 0; i < this.numVerts; i++)
            {
                byte[] abones = this.getBones(i);
                byte[] wbones = this.getBoneWeights(i);
                for (int j = 0; j < 4; j++)
                {
                    if (wbones[j] > 0 & !(usedBones.IndexOf(abones[j]) >= 0))
                        usedBones.Add(abones[j]);
                }
            }
            List<uint> usedBoneHash = new List<uint>();
            uint[] oldBoneHash = this.BoneHashList;
            foreach (byte b in usedBones)
            {
                usedBoneHash.Add(oldBoneHash[b]);
            }
            for (int i = 0; i < this.numVerts; i++)
            {
                byte[] tmpBones = this.getBones(i);
                byte[] wbones = this.getBoneWeights(i);
                byte[] abones = new byte[tmpBones.Length];
                for (int j = 0; j < 4; j++)
                {
                    if (wbones[j] > 0)
                    {
                        abones[j] = (byte)usedBoneHash.IndexOf(oldBoneHash[tmpBones[j]]);
                    }
                    else
                    {
                        abones[j] = 0;
                    }
                }
                this.setBones(i, abones);
            }
            this.bonehashcount = usedBoneHash.Count;
            this.bonehasharray = usedBoneHash.ToArray();
        }

        //internal void BoneMorpher(RIG.Bone bone, RIG.Bone pivot, float weight, Vector3 offset, Vector3 scale, Quaternion rotation)
        //{
        //    Vector3 unit = new Vector3(1f, 1f, 1f);
        //    for (int i = 0; i < this.vPositions.Length; i++)
        //    {
        //        float boneWeight = GetBoneWeightForVertex(i, bone.BoneHash);
        //        if (boneWeight == 0f || weight == 0f) continue;
        //        Vector3 weightedScale = (scale * boneWeight * weight) + unit;
        //        Vector3 weightedOffset = offset * boneWeight * weight;
        //        Quaternion weightedRotation = rotation * boneWeight * weight;
        //        Matrix4D weightedTransform = weightedRotation.toMatrix4D(weightedOffset, weightedScale);
        //        Vector3 vertPos = new Vector3(this.getPosition(i)) - pivot.WorldPosition;
        //        vertPos = weightedTransform * vertPos;
        //        vertPos += pivot.WorldPosition;
        //        this.setPosition(i, vertPos.Coordinates);
        //        Vector3 vertNorm = new Vector3(this.getNormal(i)) - pivot.WorldPosition;
        //        vertNorm = (weightedRotation * vertNorm * weightedRotation.Conjugate()).toVector3();
        //        vertNorm += pivot.WorldPosition;
        //        this.setNormal(i, vertNorm.Coordinates);
        //    }
        //}

        public float GetTotalBoneWeight(int vertexIndex, List<uint> boneHashes)
        {
            byte[] vertBones = this.getBones(vertexIndex);
            byte[] vertWeights = this.getBoneWeights(vertexIndex);
            float weight = 0f;
            for (int i = 0; i < 4; i++)
            {
                if (vertWeights[i] > 0)
                {
                    try
                    {
                        if (boneHashes.Contains(this.bonehasharray[vertBones[i]])) weight += vertWeights[i] / 255f;
                    }
                    catch
                    {
                        return 0f;
                    }
                }
            }
            return weight;
        }

        internal void SetupDeltas()
        {
            this.deltaPosition = new Vector3[this.vPositions.Length];
            for (int i = 0; i < this.vPositions.Length; i++)
            {
                this.deltaPosition[i] = new Vector3();
            }
        }

        internal void UpdatePositions()
        {
            for (int i = 0; i < this.vPositions.Length; i++)
            {
                this.vPositions[i] = new position((new Vector3(this.getPosition(i)) + this.deltaPosition[i]).Coordinates);
            }
        }

        internal void BoneMorpher(RIG.Bone bone, float weight, Vector3 offset, Vector3 scale, Quaternion rotation)
        {
            if (weight == 0) return;
            Vector3 unit = new Vector3(1f, 1f, 1f);
            List<uint> boneHashes = new List<uint>();
            bone.Rig.GetDescendants(bone.BoneHash, ref boneHashes);

            for (int i = 0; i < this.vPositions.Length; i++)
            {
                float boneWeight = GetTotalBoneWeight(i, boneHashes);
                if (boneWeight == 0f) continue;
                float adjustedWeight = boneWeight * weight;
                Vector3 weightedScale = (scale * adjustedWeight) + unit;
                Vector3 weightedOffset = offset * adjustedWeight;
                Quaternion weightedRotation = rotation * adjustedWeight;
                Matrix4D weightedTransform = weightedRotation.toMatrix4D(weightedOffset, weightedScale);
                Vector3 vertPos = new Vector3(this.getPosition(i)) - bone.WorldPosition;
                vertPos = weightedTransform * vertPos;
                vertPos += bone.WorldPosition;

                this.deltaPosition[i] += vertPos - new Vector3(this.getPosition(i));

                //  this.setPosition(i, vertPos.Coordinates);

                Matrix4D normalTransform = weightedTransform.Inverse().Transpose();
                Vector3 vertNorm = new Vector3(this.getNormal(i));
                //  vertNorm = (weightedRotation * vertNorm * weightedRotation.Conjugate()).toVector3();
                vertNorm = (normalTransform * vertNorm);
                vertNorm.Normalize();
                //  vertNorm += bone.WorldPosition;
                this.setNormal(i, vertNorm.Coordinates);
            }
        }

        public class vertexData
        {
            internal position p;
            internal normal n;
            internal uv[] uvs;
            internal Bones b;
            internal tagval t;
            internal tangent tan;
            internal int id;
            internal vertexData(position pos, normal norm, uv[] uvs, Bones bones, tagval color, tangent tan, int vertID)
            {
                this.p = pos;
                this.n = norm;
                this.uvs = uvs;
                this.b = bones;
                this.t = color;
                this.tan = tan;
                this.id = vertID;
            }
            public bool Equals(vertexData other)
            {
                return (this.p.Equals(other.p) && this.n.Equals(other.n) && this.uvs[0].Equals(other.uvs[0]) && this.uvs[1].Equals(other.uvs[1]));
            }
        }

        internal int vertexIDsearch(uint vertexID)
        {
            return Array.IndexOf(this.vertID, vertexID);
        }

        public void SnapVerticesToHead(GEOM head)
        {
            for (int i = 0; i < this.numVerts; i++)
            {
                Vector3 iPos = new Vector3(this.getPosition(i));
                Vector3 iNorm = new Vector3(this.getNormal(i));
                for (int j = 0; j < head.numVerts; j++)
                {
                    if (iPos.positionClose(head.getPosition(j)))
                    {
                        this.setPosition(i, head.getPosition(j));
                        this.setNormal(i, head.getNormal(j));
                        uint tag = this.getTagval(i);
                        this.setTagval(i, tag | 0x00630000);     
                    }
                }
            }
        }

        public void SnapVertices()
        {
            for (int i = 0; i < this.numVerts - 1; i++)
            {
                Vector3 iPos = new Vector3(this.getPosition(i));
                Vector3 iNorm = new Vector3(this.getNormal(i));
                for (int j = i + 1; j < this.numVerts; j++)
                {
                    if (iPos.positionMatches(this.getPosition(j)))
                    {
                        this.setPosition(j, iPos.Coordinates);
                        if (iNorm.positionMatches(this.getNormal(j))) this.setNormal(j, iNorm.Coordinates);
                    }
                }
            }
        }

        public void Clean()
        {
            int[] indexTrans = new int[this.numVerts];
            for (int i = 0; i < indexTrans.Length; i++)
            {
                indexTrans[i] = i;
            }

            for (int i = 0; i < this.numVerts - 1; i++)
            {
                if (indexTrans[i] != i) continue;
                for (int j = i + 1; j < this.numVerts; j++)
                {
                    if (this.vPositions[i].Equals(this.vPositions[j]) & this.vNormals[i].Equals(this.vNormals[j]))
                    {
                        bool match = true;
                        for (int u = 0; u < this.numberUVsets; u++)
                        {
                            if (!this.vUVs[u][i].CloseTo(this.vUVs[u][j])) match = false;
                        }
                        if (match) indexTrans[j] = i;
                    }
                }
            }

            List<int> indexTrans2 = new List<int>();
            for (int i = 0; i < indexTrans.Length; i++)
            {
                if (indexTrans[i] == i) indexTrans2.Add(i);
            }
            for (int i = 0; i < indexTrans.Length; i++)
            {
                indexTrans[i] = indexTrans2.IndexOf(indexTrans[i]);
            }

            List<SlotrayIntersection> newSlots = new List<SlotrayIntersection>();
            for (int i = 0; i < this.meshfaces.Length; i++)
            {
                int fp0 = indexTrans[this.meshfaces[i].facePoint0];
                int fp1 = indexTrans[this.meshfaces[i].facePoint1];
                int fp2 = indexTrans[this.meshfaces[i].facePoint2];
                for (int j = 0; j < this.SlotrayAdjustments.Length; j++)
                {
                    int[] slotVertIndices = this.slotrayIntersections[j].TrianglePointIndices;
                    if ((this.meshfaces[i].facePoint0 == slotVertIndices[0]) && (this.meshfaces[i].facePoint1 == slotVertIndices[1]) &&
                        (this.meshfaces[i].facePoint2 == slotVertIndices[2]))
                    {
                        SlotrayIntersection tmp = new SlotrayIntersection(this.slotrayIntersections[j]);
                        tmp.TrianglePointIndices = new int[] { fp0, fp1, fp2 };
                        newSlots.Add(tmp);
                    }
                }
                this.meshfaces[i] = new Face(fp0, fp1, fp2);
            }
            this.SlotrayAdjustments = newSlots.ToArray();
            this.slotCount = newSlots.Count;

            List<position> newPos = new List<position>();
            List<normal> newNorm = new List<normal>();
            List<uv>[] newUV = new List<uv>[this.numberUVsets];
            for (int i = 0; i < this.numberUVsets; i++)
            {
                newUV[i] = new List<uv>();
            }
            List<Bones> newBones = new List<Bones>();
            List<tangent> newTan = new List<tangent>();
            List<tagval> newTag = new List<tagval>();
            List<int> newID = new List<int>();
            for (int i = 0; i < indexTrans2.Count; i++)
            {
                newPos.Add(new position(this.vPositions[indexTrans2[i]]));
                newNorm.Add(new normal(this.vNormals[indexTrans2[i]]));
                for (int j = 0; j < this.numberUVsets; j++)
                {
                    newUV[j].Add(new uv(this.vUVs[j][indexTrans2[i]]));
                }
                newBones.Add(new Bones(this.vBones[indexTrans2[i]]));
                if (this.hasTangents) newTan.Add(new tangent(this.vTangents[indexTrans2[i]]));
                if (this.hasTags) newTag.Add(new tagval(this.vTags[indexTrans2[i]]));
                if (this.hasVertexIDs) newID.Add(this.vertID[indexTrans2[i]]);
            }

            this.vPositions = newPos.ToArray();
            this.vNormals = newNorm.ToArray();
            for (int i = 0; i < this.numberUVsets; i++)
            {
                this.vUVs[i] = newUV[i].ToArray();
            }
            this.vBones = newBones.ToArray();
            if (this.hasTangents) this.vTangents = newTan.ToArray();
            if (this.hasTags) this.vTags = newTag.ToArray();
            if (this.hasVertexIDs) this.vertID = newID.ToArray();
            this.numVerts = indexTrans2.Count;

            List<SeamStitch> newSeam = new List<SeamStitch>();
            for (int i = 0; i < indexTrans2.Count; i++)
            {
                for (int j = 0; j < this.seamStitches.Length; j++)
                {
                    if (indexTrans2[i] == this.seamStitches[j].Index)
                    {
                        SeamStitch s = new SeamStitch(this.seamStitches[j]);
                        s.Index = (uint)i;
                        newSeam.Add(s);
                    }
                }
            }
            this.seamStitches = newSeam.ToArray();
            this.seamStitchCount = newSeam.Count;
            if (this.hasUVset(1)) this.AutoUV1Stitches();
        }

        public void AutoUV1Stitches()
        {
            List<UVStitch> newStitch = new List<UVStitch>();
            for (int i = 0; i < this.numVerts; i++)
            {
                Vector3 iPos = new Vector3(this.getPosition(i));
                List<Vector2> iStitches = new List<Vector2>();
                for (int j = 0; j < this.numVerts; j++)
                {
                    if (iPos.positionMatches(this.getPosition(j)))
                    {
                        float[] tmpUV = this.getUV(j, 1);
                        tmpUV[0] = Math.Abs(tmpUV[0]);
                        Vector2 tmp = new Vector2(tmpUV);
                        if (iStitches.IndexOf(tmp) < 0) iStitches.Add(tmp);
                    }
                }
                if (iStitches.Count > 0) newStitch.Add(new UVStitch(i, iStitches.ToArray()));
            }
            this.uvStitches = newStitch.ToArray();
            this.uvStitchCount = this.uvStitches.Length;
        }

        public void AutoSeamStitches(Species species, AgeGender age, AgeGender gender, int lod)
        {
            List<SeamStitch> ss = new List<SeamStitch>();
            foreach (GEOM.SeamType seam in Enum.GetValues(typeof(GEOM.SeamType)))
            {
                Vector3[] verts = GetSeamVertexPositions(species, age, gender, lod, seam);
                for (int i = 0; i < this.numVerts; i++)
                {
                    for (int v = 0; v < verts.Length; v++)
                    {
                        if (verts[v].positionMatches(this.vPositions[i].Coordinates))
                        {
                            ss.Add(new SeamStitch(i, seam, v, this.vUVs[0]));
                        }
                    }
                }
            }
            this.seamStitches = ss.ToArray();
            this.seamStitchCount = this.seamStitches.Length;
        }

        internal static Vector3[] GetSeamVertexPositions(Species species, AgeGender age, AgeGender gender, int lod, GEOM.SeamType seam)
        {
            int speciesIndex = (int)species - 1;
            int ageGenderIndex;
            if (species == Species.Human)
            {
                if (age >= AgeGender.Teen)
                {
                    if (gender == AgeGender.Male) ageGenderIndex = 0;
                    else ageGenderIndex = 1;
                }
                else if (age == AgeGender.Child) ageGenderIndex = 2;
                else ageGenderIndex = 3;
            }
            else if (species == Species.LittleDog && age <= AgeGender.Child)
            {
                speciesIndex = 1;       //little dogs only have adult form so go to dog/child
                ageGenderIndex = 1;
            }
            else ageGenderIndex = age > AgeGender.Child ? 0 : 1;
            return MeshSeamVerts[speciesIndex][ageGenderIndex][lod][(int)seam];
        }

        internal static Vector3[][][][][] SetupSeamVertexPositions()
        {
            Vector3[][][][][] meshSeamVerts = new Vector3[6][][][][];   //indices: species, age/gender, lod, seam, verts
            //  dimension 0: 0 = human, 1 = dog, 2 = cat, 3 = little dog, 4 =fox, 5= horse
            //  dimension 1: human: 0 = male, 1 = female, 2 = child, 3 = toddler; little dog: 0 = adult; dog/cat: 0 = adult, 1 = child; fox: 0 = adult; horse: 0 = adult, 1 = child; 
            //  dimension 2: lod
            //  dimension 3: Ankles = 0, Pet tail = 1, Pet ears = 2, Neck = 3, Waist = 4, WaistAdultFemale = 5,  WaistAdultMale = 6
            meshSeamVerts[0] = new Vector3[4][][][];        //ageGenders
            meshSeamVerts[0][0] = new Vector3[4][][];         //Adult Male
            meshSeamVerts[0][0][0] = new Vector3[7][];         //Adult Male LOD0 seams
            meshSeamVerts[0][0][0][0] = new Vector3[] { new Vector3(0.10318f, 0.16812f, 0.01464f), new Vector3(0.08145f, 0.16812f, 0.006759f), new Vector3(0.12142f, 0.16812f, 0.002309f), new Vector3(0.1301f, 0.16812f, -0.02376f), new Vector3(0.12235f, 0.16812f, -0.04518f), new Vector3(0.10225f, 0.16812f, -0.06237f), new Vector3(0.06959f, 0.16812f, -0.01289f), new Vector3(0.07157f, 0.16812f, -0.03863f), new Vector3(0.08476f, 0.16812f, -0.05846f), new Vector3(-0.10318f, 0.168119f, 0.01464f), new Vector3(-0.08145f, 0.168119f, 0.00676f), new Vector3(-0.12142f, 0.168119f, 0.00231f), new Vector3(-0.1301f, 0.168119f, -0.02376f), new Vector3(-0.12235f, 0.168119f, -0.04518f), new Vector3(-0.10225f, 0.168119f, -0.06237f), new Vector3(-0.06959f, 0.168119f, -0.01289f), new Vector3(-0.07157f, 0.168119f, -0.03863f), new Vector3(-0.08476f, 0.168119f, -0.05846f) };     //Ankles
            meshSeamVerts[0][0][0][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][0][0][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][0][0][3] = new Vector3[] { new Vector3(0.04994f, 1.65732f, -0.04331f), new Vector3(0.05748f, 1.65212f, -0.02185f), new Vector3(0.02016f, 1.62796f, 0.02991f), new Vector3(0f, 1.62329f, 0.03646f), new Vector3(0.02658f, 1.65984f, -0.06291001f), new Vector3(0.04268f, 1.63725f, 0.01346f), new Vector3(0.03073f, 1.63173f, 0.02297f), new Vector3(0.05114f, 1.64436f, -0.00103f), new Vector3(0f, 1.66001f, -0.07078f), new Vector3(-0.04994f, 1.65732f, -0.04331f), new Vector3(-0.05748f, 1.65212f, -0.02185f), new Vector3(-0.02016f, 1.62796f, 0.02991f), new Vector3(-0.02658f, 1.65984f, -0.06291001f), new Vector3(-0.04268f, 1.63725f, 0.01346f), new Vector3(-0.03074f, 1.63173f, 0.02296f), new Vector3(-0.05114f, 1.64436f, -0.00103f) };     //Neck
            meshSeamVerts[0][0][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][0][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][0][0][6] = new Vector3[] { new Vector3(0.13477f, 1.10102f, 0.05168f), new Vector3(0.09531f, 1.09588f, 0.08789f), new Vector3(0.0283f, 1.09001f, 0.11046f), new Vector3(0.06203f, 1.0929f, 0.10109f), new Vector3(0f, 1.08875f, 0.11338f), new Vector3(0.0537f, 1.10483f, -0.07900001f), new Vector3(0.02362f, 1.10363f, -0.08015f), new Vector3(0.12888f, 1.10734f, -0.03361f), new Vector3(0.14252f, 1.10484f, 0.00736f), new Vector3(0.10903f, 1.10822f, -0.05758001f), new Vector3(0.08691f, 1.10752f, -0.0717f), new Vector3(0f, 1.10245f, -0.07855f), new Vector3(-0.13477f, 1.10102f, 0.05168f), new Vector3(-0.09531f, 1.09588f, 0.08789f), new Vector3(-0.0283f, 1.09001f, 0.11046f), new Vector3(-0.06203f, 1.0929f, 0.10109f), new Vector3(-0.0537f, 1.10483f, -0.07900001f), new Vector3(-0.02362f, 1.10363f, -0.08015f), new Vector3(-0.12888f, 1.10734f, -0.03361f), new Vector3(-0.14252f, 1.10484f, 0.00736f), new Vector3(-0.10903f, 1.10822f, -0.05758001f), new Vector3(-0.08691f, 1.10752f, -0.0717f) };     //WaistAdultMale
            meshSeamVerts[0][0][1] = new Vector3[7][];         //Adult Male LOD1 seams
            meshSeamVerts[0][0][1][0] = new Vector3[] { new Vector3(0.10318f, 0.16812f, 0.01464f), new Vector3(0.08145f, 0.16812f, 0.006759f), new Vector3(0.12142f, 0.16812f, 0.002309f), new Vector3(0.1301f, 0.16812f, -0.02376f), new Vector3(0.12235f, 0.16812f, -0.04518f), new Vector3(0.09351f, 0.16812f, -0.06042f), new Vector3(0.06959f, 0.16812f, -0.01289f), new Vector3(0.07157f, 0.16812f, -0.03863f), new Vector3(-0.10318f, 0.168119f, 0.01464f), new Vector3(-0.08145f, 0.168119f, 0.00676f), new Vector3(-0.12142f, 0.168119f, 0.00231f), new Vector3(-0.1301f, 0.168119f, -0.02376f), new Vector3(-0.12235f, 0.168119f, -0.04518f), new Vector3(-0.09351f, 0.168119f, -0.06042f), new Vector3(-0.06959f, 0.168119f, -0.01289f), new Vector3(-0.07157f, 0.168119f, -0.03863f) };     //Ankles
            meshSeamVerts[0][0][1][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][0][1][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][0][1][3] = new Vector3[] { new Vector3(0.04994f, 1.65732f, -0.04331f), new Vector3(0.05748f, 1.65212f, -0.02185f), new Vector3(0.02016f, 1.62796f, 0.02991f), new Vector3(0f, 1.62329f, 0.03646f), new Vector3(0.02658f, 1.65984f, -0.06291001f), new Vector3(0.04268f, 1.63725f, 0.01346f), new Vector3(0.03074f, 1.63173f, 0.02297f), new Vector3(0.05114f, 1.64436f, -0.00103f), new Vector3(0f, 1.66001f, -0.07078f), new Vector3(-0.04994f, 1.65732f, -0.04331f), new Vector3(-0.05748f, 1.65212f, -0.02185f), new Vector3(-0.02016f, 1.62796f, 0.02991f), new Vector3(-0.02658f, 1.65984f, -0.06291001f), new Vector3(-0.04268f, 1.63725f, 0.01346f), new Vector3(-0.03074f, 1.63173f, 0.02296f), new Vector3(-0.05114f, 1.64436f, -0.00103f) };     //Neck
            meshSeamVerts[0][0][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][0][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][0][1][6] = new Vector3[] { new Vector3(0.13477f, 1.10102f, 0.05168f), new Vector3(0.07867001f, 1.09439f, 0.09449001f), new Vector3(0f, 1.08875f, 0.11338f), new Vector3(0.03866f, 1.10423f, -0.07958f), new Vector3(0.12888f, 1.10734f, -0.03361f), new Vector3(0.14252f, 1.10484f, 0.00736f), new Vector3(0.09797f, 1.10787f, -0.06464f), new Vector3(0f, 1.10245f, -0.07855f), new Vector3(-0.13477f, 1.10102f, 0.05168f), new Vector3(-0.07867001f, 1.09439f, 0.09449001f), new Vector3(-0.03866f, 1.10423f, -0.07958f), new Vector3(-0.12888f, 1.10734f, -0.03361f), new Vector3(-0.14252f, 1.10484f, 0.00736f), new Vector3(-0.09797f, 1.10787f, -0.06464f) };     //WaistAdultMale
            meshSeamVerts[0][0][2] = new Vector3[7][];         //Adult Male LOD2 seams
            meshSeamVerts[0][0][2][0] = new Vector3[] { new Vector3(0.1123f, 0.16812f, 0.008479001f), new Vector3(0.08145f, 0.16812f, 0.006759f), new Vector3(0.1301f, 0.16812f, -0.02376f), new Vector3(0.12235f, 0.16812f, -0.04518f), new Vector3(0.08254f, 0.16812f, -0.04952f), new Vector3(0.06959f, 0.16812f, -0.01289f), new Vector3(-0.1123f, 0.168119f, 0.00848f), new Vector3(-0.08145f, 0.168119f, 0.00676f), new Vector3(-0.1301f, 0.168119f, -0.02376f), new Vector3(-0.12235f, 0.168119f, -0.04518f), new Vector3(-0.08254f, 0.168119f, -0.04952f), new Vector3(-0.06959f, 0.168119f, -0.01289f) };     //Ankles
            meshSeamVerts[0][0][2][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][0][2][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][0][2][3] = new Vector3[] { new Vector3(0.04994f, 1.65732f, -0.04331f), new Vector3(0.05748f, 1.65212f, -0.02185f), new Vector3(0.02016f, 1.62796f, 0.02991f), new Vector3(0f, 1.62329f, 0.03646f), new Vector3(0.02658f, 1.65984f, -0.06291001f), new Vector3(0.04268f, 1.63725f, 0.01346f), new Vector3(0.03074f, 1.63173f, 0.02297f), new Vector3(0.05114f, 1.64436f, -0.00103f), new Vector3(0f, 1.66001f, -0.07078f), new Vector3(-0.04994f, 1.65732f, -0.04331f), new Vector3(-0.05748f, 1.65212f, -0.02185f), new Vector3(-0.02016f, 1.62796f, 0.02991f), new Vector3(-0.02658f, 1.65984f, -0.06291001f), new Vector3(-0.04268f, 1.63725f, 0.01346f), new Vector3(-0.03074f, 1.63173f, 0.02296f), new Vector3(-0.05114f, 1.64436f, -0.00103f) };     //Neck
            meshSeamVerts[0][0][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][0][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][0][2][6] = new Vector3[] { new Vector3(0.13477f, 1.10102f, 0.05168f), new Vector3(0.07867001f, 1.09439f, 0.09449001f), new Vector3(0f, 1.08875f, 0.11338f), new Vector3(0.06831001f, 1.10605f, -0.07211f), new Vector3(0.12888f, 1.10734f, -0.03361f), new Vector3(0.14252f, 1.10484f, 0.00736f), new Vector3(0f, 1.10245f, -0.07855f), new Vector3(-0.13477f, 1.10102f, 0.05168f), new Vector3(-0.07867001f, 1.09439f, 0.09449001f), new Vector3(-0.06831001f, 1.10605f, -0.07211f), new Vector3(-0.12888f, 1.10734f, -0.03361f), new Vector3(-0.14252f, 1.10484f, 0.00736f) };     //WaistAdultMale
            meshSeamVerts[0][0][3] = new Vector3[7][];         //Adult Male LOD3 seams
            meshSeamVerts[0][0][3][0] = new Vector3[] { new Vector3(0.10318f, 0.16812f, 0.01464f), new Vector3(0.1301f, 0.16812f, -0.02376f), new Vector3(0.09351f, 0.16812f, -0.06042f), new Vector3(0.06959f, 0.16812f, -0.01289f), new Vector3(-0.10318f, 0.168119f, 0.01464f), new Vector3(-0.1301f, 0.168119f, -0.02376f), new Vector3(-0.09351f, 0.168119f, -0.06042f), new Vector3(-0.06959f, 0.168119f, -0.01289f) };     //Ankles
            meshSeamVerts[0][0][3][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][0][3][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][0][3][3] = new Vector3[] { new Vector3(0.05748f, 1.65212f, -0.02185f), new Vector3(0f, 1.62329f, 0.03646f), new Vector3(0.03826f, 1.65858f, -0.05311f), new Vector3(0.03074f, 1.63173f, 0.02297f), new Vector3(0f, 1.66001f, -0.07078f), new Vector3(-0.05748f, 1.65212f, -0.02185f), new Vector3(-0.03826f, 1.65858f, -0.05311f), new Vector3(-0.03074f, 1.63173f, 0.02296f) };     //Neck
            meshSeamVerts[0][0][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][0][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][0][3][6] = new Vector3[] { new Vector3(0.10672f, 1.09771f, 0.07308f), new Vector3(0f, 1.08875f, 0.11338f), new Vector3(0.0986f, 1.1067f, -0.05286f), new Vector3(0.14252f, 1.10484f, 0.00736f), new Vector3(0f, 1.10245f, -0.07855f), new Vector3(-0.10672f, 1.09771f, 0.07308f), new Vector3(-0.0986f, 1.1067f, -0.05286f), new Vector3(-0.14252f, 1.10484f, 0.00736f) };     //WaistAdultMale

            meshSeamVerts[0][1] = new Vector3[4][][];         //Adult Female
            meshSeamVerts[0][1][0] = new Vector3[7][];         //Adult Female LOD0 seams
            meshSeamVerts[0][1][0][0] = new Vector3[] { new Vector3(0.10061f, 0.17831f, 0.01385f), new Vector3(0.08411001f, 0.17831f, 0.01062f), new Vector3(0.11955f, 0.17831f, -0.00055f), new Vector3(0.12174f, 0.17831f, -0.02315f), new Vector3(0.11393f, 0.17831f, -0.04578f), new Vector3(0.1008f, 0.17831f, -0.05814f), new Vector3(0.07353f, 0.17831f, -0.01651f), new Vector3(0.07809f, 0.17831f, -0.04168f), new Vector3(0.08404f, 0.17831f, -0.05354f), new Vector3(-0.10061f, 0.17831f, 0.01385f), new Vector3(-0.08411001f, 0.17831f, 0.01062f), new Vector3(-0.11955f, 0.17831f, -0.00055f), new Vector3(-0.12174f, 0.17831f, -0.02315f), new Vector3(-0.11393f, 0.17831f, -0.04578f), new Vector3(-0.10078f, 0.17831f, -0.05815f), new Vector3(-0.07353f, 0.17831f, -0.01651f), new Vector3(-0.07809f, 0.17831f, -0.04168f), new Vector3(-0.08404f, 0.17831f, -0.05354f) };     //Ankles
            meshSeamVerts[0][1][0][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][1][0][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][1][0][3] = new Vector3[] { new Vector3(0.04228f, 1.65728f, -0.03741f), new Vector3(0.04676f, 1.65358f, -0.01843f), new Vector3(0.01541f, 1.62769f, 0.02782f), new Vector3(0f, 1.62476f, 0.03136f), new Vector3(0.02362f, 1.65786f, -0.05106f), new Vector3(0.03554f, 1.6385f, 0.013f), new Vector3(0.02586f, 1.6321f, 0.02275f), new Vector3(0.04565f, 1.64751f, -0.00218f), new Vector3(0f, 1.65824f, -0.05823f), new Vector3(-0.04228f, 1.65728f, -0.03741f), new Vector3(-0.04676f, 1.65358f, -0.01843f), new Vector3(-0.01541f, 1.62769f, 0.02782f), new Vector3(-0.02361f, 1.65786f, -0.05106f), new Vector3(-0.03554f, 1.6385f, 0.013f), new Vector3(-0.02586f, 1.6321f, 0.02275f), new Vector3(-0.04565f, 1.64751f, -0.00218f) };     //Neck
            meshSeamVerts[0][1][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][1][0][5] = new Vector3[] { new Vector3(0f, 1.16153f, 0.10832f), new Vector3(0f, 1.17486f, -0.05726f), new Vector3(0.11117f, 1.17097f, 0.05975f), new Vector3(0.08326f, 1.16772f, 0.08886f), new Vector3(0.02036f, 1.1624f, 0.10652f), new Vector3(0.05003f, 1.16448f, 0.10046f), new Vector3(0.11914f, 1.17389f, 0.01728f), new Vector3(0.11197f, 1.17412f, -0.01527f), new Vector3(0.09584f, 1.17388f, -0.04124f), new Vector3(0.07065f, 1.1729f, -0.05077f), new Vector3(0.0456f, 1.17315f, -0.05503f), new Vector3(0.01515f, 1.17431f, -0.05672f), new Vector3(-0.11117f, 1.17097f, 0.05975f), new Vector3(-0.08326f, 1.16772f, 0.08886f), new Vector3(-0.02036f, 1.1624f, 0.10652f), new Vector3(-0.05003f, 1.16448f, 0.10046f), new Vector3(-0.11914f, 1.17389f, 0.01728f), new Vector3(-0.11197f, 1.17412f, -0.01527f), new Vector3(-0.09584f, 1.17388f, -0.04124f), new Vector3(-0.07065f, 1.1729f, -0.05077f), new Vector3(-0.0456f, 1.17315f, -0.05503f), new Vector3(-0.01515f, 1.17431f, -0.05672f) };     //WaistAdultFemale
            meshSeamVerts[0][1][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][1][1] = new Vector3[7][];         //Adult Female LOD1 seams
            meshSeamVerts[0][1][1][0] = new Vector3[] { new Vector3(0.10061f, 0.17831f, 0.01385f), new Vector3(0.08411001f, 0.17831f, 0.01062f), new Vector3(0.11955f, 0.17831f, -0.00055f), new Vector3(0.12174f, 0.17831f, -0.02315f), new Vector3(0.11393f, 0.17831f, -0.04578f), new Vector3(0.09242f, 0.17831f, -0.05584f), new Vector3(0.07353f, 0.17831f, -0.01651f), new Vector3(0.07809f, 0.17831f, -0.04168f), new Vector3(-0.10061f, 0.17831f, 0.01385f), new Vector3(-0.08411001f, 0.17831f, 0.01062f), new Vector3(-0.11955f, 0.17831f, -0.00055f), new Vector3(-0.12174f, 0.17831f, -0.02315f), new Vector3(-0.11393f, 0.17831f, -0.04578f), new Vector3(-0.09241f, 0.17831f, -0.05585f), new Vector3(-0.07353f, 0.17831f, -0.01651f), new Vector3(-0.07809f, 0.17831f, -0.04168f) };     //Ankles
            meshSeamVerts[0][1][1][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][1][1][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][1][1][3] = new Vector3[] { new Vector3(0.04228f, 1.65729f, -0.03741f), new Vector3(0.04676f, 1.65359f, -0.01843f), new Vector3(0.01541f, 1.6277f, 0.02782f), new Vector3(0f, 1.62477f, 0.03136f), new Vector3(0.02362f, 1.65786f, -0.05105f), new Vector3(0.03554f, 1.63851f, 0.013f), new Vector3(0.02586f, 1.63211f, 0.02275f), new Vector3(0.04565f, 1.64751f, -0.00218f), new Vector3(0f, 1.65824f, -0.05822f), new Vector3(-0.04228f, 1.65729f, -0.03741f), new Vector3(-0.04676f, 1.65359f, -0.01843f), new Vector3(-0.01541f, 1.6277f, 0.02782f), new Vector3(-0.02361f, 1.65786f, -0.05105f), new Vector3(-0.03554f, 1.63851f, 0.013f), new Vector3(-0.02586f, 1.63211f, 0.02275f), new Vector3(-0.04565f, 1.64751f, -0.00218f) };     //Neck
            meshSeamVerts[0][1][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][1][1][5] = new Vector3[] { new Vector3(0f, 1.16153f, 0.10832f), new Vector3(0f, 1.17486f, -0.05726f), new Vector3(0.11117f, 1.17097f, 0.05975f), new Vector3(0.06664f, 1.1661f, 0.09466001f), new Vector3(0.11914f, 1.17389f, 0.01728f), new Vector3(0.11197f, 1.17412f, -0.01527f), new Vector3(0.08324f, 1.17339f, -0.04601f), new Vector3(0.0456f, 1.17315f, -0.05503f), new Vector3(-0.11117f, 1.17097f, 0.05975f), new Vector3(-0.06664f, 1.1661f, 0.09466001f), new Vector3(-0.11914f, 1.17389f, 0.01728f), new Vector3(-0.11197f, 1.17412f, -0.01527f), new Vector3(-0.08324f, 1.17339f, -0.04601f), new Vector3(-0.0456f, 1.17315f, -0.05503f) };     //WaistAdultFemale
            meshSeamVerts[0][1][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][1][2] = new Vector3[7][];         //Adult Female LOD2 seams
            meshSeamVerts[0][1][2][0] = new Vector3[] { new Vector3(0.11008f, 0.17831f, 0.00665f), new Vector3(0.08411001f, 0.17831f, 0.01062f), new Vector3(0.12174f, 0.17831f, -0.02315f), new Vector3(0.11393f, 0.17831f, -0.04578f), new Vector3(0.08525001f, 0.17831f, -0.04876f), new Vector3(0.07353f, 0.17831f, -0.01651f), new Vector3(-0.11008f, 0.17831f, 0.00665f), new Vector3(-0.08411001f, 0.17831f, 0.01062f), new Vector3(-0.12174f, 0.17831f, -0.02315f), new Vector3(-0.11393f, 0.17831f, -0.04578f), new Vector3(-0.08525001f, 0.17831f, -0.04876f), new Vector3(-0.07353f, 0.17831f, -0.01651f) };     //Ankles
            meshSeamVerts[0][1][2][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][1][2][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][1][2][3] = new Vector3[] { new Vector3(0.04228f, 1.65729f, -0.03742f), new Vector3(0.04676f, 1.65358f, -0.01844f), new Vector3(0.01541f, 1.6277f, 0.02782f), new Vector3(0f, 1.62477f, 0.03136f), new Vector3(0.02362f, 1.65786f, -0.05105f), new Vector3(0.03554f, 1.6385f, 0.013f), new Vector3(0.02586f, 1.63212f, 0.02275f), new Vector3(0.04565f, 1.64751f, -0.00218f), new Vector3(0f, 1.65824f, -0.05822f), new Vector3(-0.04228f, 1.65729f, -0.03742f), new Vector3(-0.04676f, 1.65358f, -0.01844f), new Vector3(-0.01541f, 1.6277f, 0.02782f), new Vector3(-0.02361f, 1.65786f, -0.05105f), new Vector3(-0.03554f, 1.63851f, 0.013f), new Vector3(-0.02586f, 1.63212f, 0.02275f), new Vector3(-0.04565f, 1.64751f, -0.00218f) };     //Neck
            meshSeamVerts[0][1][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][1][2][5] = new Vector3[] { new Vector3(0f, 1.16153f, 0.10832f), new Vector3(0f, 1.17486f, -0.05726f), new Vector3(0.08891001f, 1.16853f, 0.07720001f), new Vector3(0.11914f, 1.17389f, 0.01728f), new Vector3(0.11197f, 1.17412f, -0.01527f), new Vector3(0.06442f, 1.17327f, -0.05052f), new Vector3(-0.08891001f, 1.16853f, 0.07720001f), new Vector3(-0.11914f, 1.17389f, 0.01728f), new Vector3(-0.11197f, 1.17412f, -0.01527f), new Vector3(-0.06442f, 1.17327f, -0.05052f) };     //WaistAdultFemale
            meshSeamVerts[0][1][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][1][3] = new Vector3[7][];         //Adult Female LOD3 seams
            meshSeamVerts[0][1][3][0] = new Vector3[] { new Vector3(0.10061f, 0.17831f, 0.01385f), new Vector3(0.12174f, 0.17831f, -0.02315f), new Vector3(0.09242f, 0.17831f, -0.05584f), new Vector3(0.07353f, 0.17831f, -0.01651f), new Vector3(-0.10061f, 0.17831f, 0.01385f), new Vector3(-0.12174f, 0.17831f, -0.02315f), new Vector3(-0.09241f, 0.17831f, -0.05585f), new Vector3(-0.07353f, 0.17831f, -0.01651f) };     //Ankles
            meshSeamVerts[0][1][3][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][1][3][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][1][3][3] = new Vector3[] { new Vector3(0.04676f, 1.65358f, -0.01844f), new Vector3(0f, 1.62477f, 0.03136f), new Vector3(0.03295f, 1.65757f, -0.04423f), new Vector3(0.02583f, 1.63211f, 0.02276f), new Vector3(0f, 1.65824f, -0.05822f), new Vector3(-0.04676f, 1.65358f, -0.01844f), new Vector3(-0.03295f, 1.65757f, -0.04423f), new Vector3(-0.02586f, 1.63212f, 0.02275f) };     //Neck
            meshSeamVerts[0][1][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[0][1][3][5] = new Vector3[] { new Vector3(0f, 1.16153f, 0.10832f), new Vector3(0f, 1.17486f, -0.05726f), new Vector3(0.08891001f, 1.16853f, 0.07720001f), new Vector3(0.11914f, 1.17389f, 0.01728f), new Vector3(0.0882f, 1.17369f, -0.03289f), new Vector3(-0.08891001f, 1.16853f, 0.07720001f), new Vector3(-0.11914f, 1.17389f, 0.01728f), new Vector3(-0.0882f, 1.17369f, -0.03289f) };     //WaistAdultFemale
            meshSeamVerts[0][1][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[0][2] = new Vector3[4][][];         //Child
            meshSeamVerts[0][2][0] = new Vector3[7][];         //Child LOD0 seams
            meshSeamVerts[0][2][0][0] = new Vector3[] { new Vector3(0.07243f, 0.11592f, 0.01694f), new Vector3(0.05307f, 0.11592f, 0.0112f), new Vector3(0.09189f, 0.11592f, 0.00385f), new Vector3(0.0952f, 0.11592f, -0.01715f), new Vector3(0.08587f, 0.11592f, -0.03915f), new Vector3(0.07431f, 0.11592f, -0.04292f), new Vector3(0.04333f, 0.11592f, -0.00869f), new Vector3(0.04767f, 0.11593f, -0.03418f), new Vector3(0.06141f, 0.11592f, -0.04133f), new Vector3(-0.05307f, 0.11592f, 0.0112f), new Vector3(-0.07243f, 0.11592f, 0.01694f), new Vector3(-0.09189f, 0.11592f, 0.00385f), new Vector3(-0.0952f, 0.11592f, -0.01715f), new Vector3(-0.08587f, 0.11592f, -0.03915f), new Vector3(-0.07431f, 0.11592f, -0.04292f), new Vector3(-0.04333f, 0.11592f, -0.00869f), new Vector3(-0.04767f, 0.11593f, -0.03418f), new Vector3(-0.06141f, 0.11592f, -0.04133f) };     //Ankles
            meshSeamVerts[0][2][0][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][2][0][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][2][0][3] = new Vector3[] { new Vector3(-0.03752f, 1.12267f, -0.03273f), new Vector3(-0.02065f, 1.12468f, -0.04379001f), new Vector3(0f, 1.12563f, -0.04968f), new Vector3(0.02065f, 1.12477f, -0.04379001f), new Vector3(0.03752f, 1.12268f, -0.03273f), new Vector3(-0.04215f, 1.11826f, -0.01802f), new Vector3(-0.03877f, 1.11206f, -0.00206f), new Vector3(-0.03276f, 1.10712f, 0.00977f), new Vector3(-0.01436f, 1.09967f, 0.02605f), new Vector3(0f, 1.09771f, 0.029229f), new Vector3(-0.02296f, 1.10289f, 0.01801f), new Vector3(0.04215f, 1.11824f, -0.01801f), new Vector3(0.03877f, 1.11205f, -0.00205f), new Vector3(0.03276f, 1.10712f, 0.00977f), new Vector3(0.01436f, 1.09965f, 0.02605f), new Vector3(0.02295f, 1.10288f, 0.01802f) };     //Neck
            meshSeamVerts[0][2][0][4] = new Vector3[] { new Vector3(0.0914f, 0.7533001f, 0.03828f), new Vector3(0.06577f, 0.7498001f, 0.06943001f), new Vector3(0.01949f, 0.74579f, 0.08594f), new Vector3(0.04228f, 0.74777f, 0.08012f), new Vector3(0f, 0.74493f, 0.08794f), new Vector3(0.03662f, 0.7558801f, -0.06594001f), new Vector3(0.01616f, 0.7548701f, -0.06698f), new Vector3(0.09703f, 0.75579f, 0.00505f), new Vector3(0.08787f, 0.75756f, -0.02397f), new Vector3(0.07434f, 0.75819f, -0.04569f), new Vector3(0.05926f, 0.75773f, -0.05923f), new Vector3(0f, 0.75421f, -0.06595f), new Vector3(-0.0914f, 0.7533001f, 0.03828f), new Vector3(-0.06577f, 0.7498001f, 0.06944f), new Vector3(-0.04228f, 0.74777f, 0.08014001f), new Vector3(-0.01949f, 0.74579f, 0.08594f), new Vector3(-0.01616f, 0.7548701f, -0.06698f), new Vector3(-0.03662f, 0.7558801f, -0.06594001f), new Vector3(-0.09703f, 0.75579f, 0.00505f), new Vector3(-0.08787f, 0.75756f, -0.02397f), new Vector3(-0.07434f, 0.7582f, -0.04569f), new Vector3(-0.05926f, 0.75773f, -0.05923f) };     //Waist
            meshSeamVerts[0][2][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][2][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][2][1] = new Vector3[7][];         //Child LOD1 seams
            meshSeamVerts[0][2][1][0] = new Vector3[] { new Vector3(0.053066f, 0.115924f, 0.011202f), new Vector3(0.043329f, 0.115923f, -0.008687f), new Vector3(0.072431f, 0.115924f, 0.016942f), new Vector3(0.09188901f, 0.115924f, 0.003849f), new Vector3(0.095204f, 0.115924f, -0.017147f), new Vector3(0.08587401f, 0.115924f, -0.039149f), new Vector3(0.067858f, 0.115924f, -0.042117f), new Vector3(0.047673f, 0.115932f, -0.034183f), new Vector3(-0.053066f, 0.115924f, 0.011202f), new Vector3(-0.043329f, 0.115923f, -0.008687f), new Vector3(-0.072431f, 0.115924f, 0.016942f), new Vector3(-0.09188901f, 0.115924f, 0.003849f), new Vector3(-0.095204f, 0.115924f, -0.017147f), new Vector3(-0.08587401f, 0.115924f, -0.039149f), new Vector3(-0.067858f, 0.115924f, -0.042117f), new Vector3(-0.047673f, 0.115932f, -0.034183f) };     //Ankles
            meshSeamVerts[0][2][1][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][2][1][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][2][1][3] = new Vector3[] { new Vector3(-0.03752f, 1.12268f, -0.03273f), new Vector3(-0.02065f, 1.12477f, -0.04379001f), new Vector3(0f, 1.12563f, -0.04968f), new Vector3(0.02065f, 1.12477f, -0.04379001f), new Vector3(0.03752f, 1.12268f, -0.03273f), new Vector3(-0.03276f, 1.10712f, 0.00977f), new Vector3(-0.03877f, 1.11205f, -0.002049f), new Vector3(-0.04215f, 1.11824f, -0.01801f), new Vector3(0.04215f, 1.11824f, -0.01801f), new Vector3(0.03877f, 1.11205f, -0.002049f), new Vector3(0.03276f, 1.10712f, 0.00977f), new Vector3(0f, 1.09771f, 0.02923f), new Vector3(-0.01436f, 1.09965f, 0.02605f), new Vector3(-0.02295f, 1.10288f, 0.01802f), new Vector3(0.01436f, 1.09965f, 0.02605f), new Vector3(0.02295f, 1.10288f, 0.01802f) };     //Neck
            meshSeamVerts[0][2][1][4] = new Vector3[] { new Vector3(0.0914f, 0.7533001f, 0.03828f), new Vector3(0.05402f, 0.74878f, 0.07478f), new Vector3(0f, 0.74493f, 0.08794f), new Vector3(0.09703f, 0.75579f, 0.00505f), new Vector3(0.08787f, 0.75756f, -0.02397f), new Vector3(0.0668f, 0.75796f, -0.05246f), new Vector3(0.02639f, 0.75537f, -0.06646f), new Vector3(0f, 0.75421f, -0.06595f), new Vector3(-0.0914f, 0.7533001f, 0.03828f), new Vector3(-0.05402f, 0.74878f, 0.07478f), new Vector3(-0.09703f, 0.75579f, 0.00505f), new Vector3(-0.08787f, 0.75756f, -0.02397f), new Vector3(-0.0668f, 0.75796f, -0.05246f), new Vector3(-0.02639f, 0.75537f, -0.06646f) };     //Waist
            meshSeamVerts[0][2][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][2][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][2][2] = new Vector3[7][];         //Child LOD2 seams
            meshSeamVerts[0][2][2][0] = new Vector3[] { new Vector3(0.053066f, 0.115924f, 0.016333f), new Vector3(), new Vector3(0.043329f, 0.115923f, -0.007374f), new Vector3(0.08216001f, 0.115924f, 0.015517f), new Vector3(0.095204f, 0.115924f, -0.017455f), new Vector3(0.08587401f, 0.115924f, -0.041221f), new Vector3(0.057831f, 0.115938f, -0.038038f), new Vector3(-0.053066f, 0.115924f, 0.016333f), new Vector3(-0.043329f, 0.115923f, -0.007374f), new Vector3(-0.08216001f, 0.115924f, 0.015517f), new Vector3(-0.095204f, 0.115924f, -0.017455f), new Vector3(-0.08587401f, 0.115924f, -0.041221f), new Vector3(-0.057831f, 0.115938f, -0.038038f) };     //Ankles
            meshSeamVerts[0][2][2][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][2][2][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][2][2][3] = new Vector3[] { new Vector3(-0.03752f, 1.12267f, -0.03273f), new Vector3(0f, 1.12563f, -0.04968f), new Vector3(-0.02065f, 1.12468f, -0.04379001f), new Vector3(0.02065f, 1.12468f, -0.04379001f), new Vector3(0.03752f, 1.12267f, -0.03273f), new Vector3(-0.04215f, 1.11826f, -0.01802f), new Vector3(-0.03877f, 1.11206f, -0.002059f), new Vector3(-0.03276f, 1.10712f, 0.00977f), new Vector3(0.03276f, 1.10712f, 0.00977f), new Vector3(0.03877f, 1.11206f, -0.002059f), new Vector3(0.04215f, 1.11826f, -0.01802f), new Vector3(0f, 1.09771f, 0.02923f), new Vector3(-0.01436f, 1.09967f, 0.02605f), new Vector3(-0.02296f, 1.10289f, 0.01801f), new Vector3(0.01436f, 1.09967f, 0.02605f), new Vector3(0.02296f, 1.10289f, 0.01801f) };     //Neck
            meshSeamVerts[0][2][2][4] = new Vector3[] { new Vector3(0.0914f, 0.7533001f, 0.03828f), new Vector3(0.05402f, 0.74878f, 0.07478f), new Vector3(0f, 0.74493f, 0.08794f), new Vector3(0.09703f, 0.75579f, 0.00505f), new Vector3(0.08787f, 0.75756f, -0.02397f), new Vector3(0.04613f, 0.75671f, -0.05946f), new Vector3(0f, 0.75421f, -0.06595f), new Vector3(-0.0914f, 0.7533001f, 0.03828f), new Vector3(-0.05402f, 0.74878f, 0.07479f), new Vector3(-0.09703f, 0.75579f, 0.00505f), new Vector3(-0.08787f, 0.75756f, -0.02397f), new Vector3(-0.04613f, 0.75672f, -0.05946f) };     //Waist
            meshSeamVerts[0][2][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][2][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][2][3] = new Vector3[7][];         //Child LOD3 seams
            meshSeamVerts[0][2][3][0] = new Vector3[] { new Vector3(0.07243f, 0.11592f, 0.01694f), new Vector3(0.04333f, 0.11592f, -0.00869f), new Vector3(0.0952f, 0.11592f, -0.01715f), new Vector3(0.06786f, 0.11592f, -0.04212f), new Vector3(-0.07243f, 0.11592f, 0.01694f), new Vector3(-0.04333f, 0.11592f, -0.00869f), new Vector3(-0.0952f, 0.11592f, -0.01715f), new Vector3(-0.06786f, 0.11592f, -0.04212f) };     //Ankles
            meshSeamVerts[0][2][3][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][2][3][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][2][3][3] = new Vector3[] { new Vector3(0.02908f, 1.12368f, -0.03826001f), new Vector3(0f, 1.12563f, -0.04968f), new Vector3(-0.02908f, 1.12368f, -0.03826001f), new Vector3(-0.04215f, 1.11826f, -0.01802f), new Vector3(0.02296f, 1.10289f, 0.01801f), new Vector3(0.04215f, 1.11826f, -0.01802f), new Vector3(0f, 1.09771f, 0.02923f), new Vector3(-0.02296f, 1.10289f, 0.01801f) };     //Neck
            meshSeamVerts[0][2][3][4] = new Vector3[] { new Vector3(0f, 0.74493f, 0.08794f), new Vector3(0.0721f, 0.7513f, 0.05675f), new Vector3(0.09703f, 0.75579f, 0.00505f), new Vector3(0f, 0.75421f, -0.06595f), new Vector3(-0.0721f, 0.7513f, 0.05675f), new Vector3(-0.09703f, 0.75579f, 0.00505f) };     //Waist
            meshSeamVerts[0][2][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][2][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[0][3] = new Vector3[4][][];         //Toddler
            meshSeamVerts[0][3][0] = new Vector3[7][];         //Toddler LOD0 seams
            meshSeamVerts[0][3][0][0] = new Vector3[] { new Vector3(0.0581f, 0.08125f, 0.02318f), new Vector3(0.04031f, 0.08124f, 0.01804f), new Vector3(0.07382f, 0.08125f, 0.01169f), new Vector3(0.07935f, 0.08125f, -0.00576f), new Vector3(0.07126f, 0.08125f, -0.02278f), new Vector3(0.05931f, 0.08124f, -0.02894f), new Vector3(0.03273f, 0.08124f, 0.00192f), new Vector3(0.03487f, 0.08125f, -0.0212f), new Vector3(0.04727f, 0.08125f, -0.02768f), new Vector3(-0.0581f, 0.08125f, 0.02318f), new Vector3(-0.04031f, 0.08124f, 0.01804f), new Vector3(-0.07382f, 0.08125f, 0.01169f), new Vector3(-0.07935f, 0.08125f, -0.00576f), new Vector3(-0.07126f, 0.08125f, -0.02278f), new Vector3(-0.05931f, 0.08124f, -0.02894f), new Vector3(-0.03273f, 0.08124f, 0.00192f), new Vector3(-0.03487f, 0.08125f, -0.0212f), new Vector3(-0.04727f, 0.08125f, -0.02768f) };     //Ankles
            meshSeamVerts[0][3][0][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][3][0][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][3][0][3] = new Vector3[] { new Vector3(0f, 0.72543f, 0.02841f), new Vector3(0f, 0.74598f, -0.04577f), new Vector3(-0.03502f, 0.74528f, -0.0298f), new Vector3(-0.03926f, 0.74341f, -0.0156f), new Vector3(-0.0366f, 0.73923f, -0.00225f), new Vector3(-0.0332f, 0.73407f, 0.01018f), new Vector3(-0.01506f, 0.72707f, 0.02594f), new Vector3(-0.02401f, 0.73009f, 0.01878f), new Vector3(-0.01907f, 0.7453601f, -0.0411f), new Vector3(0.03502f, 0.74528f, -0.0298f), new Vector3(0.03926f, 0.74341f, -0.0156f), new Vector3(0.0366f, 0.73923f, -0.00225f), new Vector3(0.0332f, 0.73407f, 0.01018f), new Vector3(0.01506f, 0.72707f, 0.02594f), new Vector3(0.02401f, 0.73009f, 0.01878f), new Vector3(0.01907f, 0.7453601f, -0.0411f) };     //Neck
            meshSeamVerts[0][3][0][4] = new Vector3[] { new Vector3(0.08580001f, 0.46483f, 0.03673f), new Vector3(0.064f, 0.46369f, 0.06124f), new Vector3(0.01813f, 0.46041f, 0.08473f), new Vector3(0.03817f, 0.46175f, 0.07651f), new Vector3(0f, 0.45983f, 0.08692f), new Vector3(0.03322f, 0.46701f, -0.06998001f), new Vector3(0.01455f, 0.46646f, -0.07039f), new Vector3(0.08139001f, 0.4675f, -0.02652f), new Vector3(0.09089f, 0.46698f, 0.00424f), new Vector3(0.06864001f, 0.46769f, -0.04676f), new Vector3(0.05398f, 0.46762f, -0.05926f), new Vector3(0f, 0.4661f, -0.0696f), new Vector3(-0.08580001f, 0.46483f, 0.0367f), new Vector3(-0.064f, 0.46369f, 0.0613f), new Vector3(-0.01813f, 0.46041f, 0.08473f), new Vector3(-0.03818f, 0.46175f, 0.07676001f), new Vector3(-0.03322f, 0.46701f, -0.06998001f), new Vector3(-0.01455f, 0.46646f, -0.07039f), new Vector3(-0.08139001f, 0.4675f, -0.02652f), new Vector3(-0.09089f, 0.46698f, 0.00424f), new Vector3(-0.06864001f, 0.46769f, -0.04676f), new Vector3(-0.05398f, 0.46762f, -0.05926f) };     //Waist
            meshSeamVerts[0][3][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][3][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][3][1] = new Vector3[7][];         //Toddler LOD1 seams
            meshSeamVerts[0][3][1][0] = new Vector3[] { new Vector3(0.058104f, 0.08125f, 0.023184f), new Vector3(0.040309f, 0.08123501f, 0.018042f), new Vector3(0.07382f, 0.08125f, 0.01169f), new Vector3(0.079346f, 0.08125f, -0.005758f), new Vector3(0.065284f, 0.081243f, -0.025861f), new Vector3(0.032729f, 0.081235f, 0.001915f), new Vector3(0.034873f, 0.081249f, -0.021203f), new Vector3(0.047273f, 0.08125f, -0.027681f), new Vector3(-0.058104f, 0.08125f, 0.023184f), new Vector3(-0.040309f, 0.08123501f, 0.018042f), new Vector3(-0.07382f, 0.08125f, 0.01169f), new Vector3(-0.079346f, 0.08125f, -0.005758f), new Vector3(-0.065284f, 0.081243f, -0.025861f), new Vector3(-0.032729f, 0.081235f, 0.001915f), new Vector3(-0.034873f, 0.081249f, -0.021203f), new Vector3(-0.047273f, 0.08125f, -0.027681f) };     //Ankles
            meshSeamVerts[0][3][1][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][3][1][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][3][1][3] = new Vector3[] { new Vector3(0f, 0.725429f, 0.028412f), new Vector3(0f, 0.745978f, -0.045768f), new Vector3(-0.035021f, 0.7452821f, -0.029798f), new Vector3(-0.039258f, 0.743412f, -0.015598f), new Vector3(-0.036595f, 0.739231f, -0.002247f), new Vector3(-0.033196f, 0.7340671f, 0.010182f), new Vector3(-0.024012f, 0.730088f, 0.018779f), new Vector3(-0.019068f, 0.745362f, -0.041102f), new Vector3(-0.015064f, 0.727072f, 0.025942f), new Vector3(0.035021f, 0.7452821f, -0.029798f), new Vector3(0.039258f, 0.743412f, -0.015598f), new Vector3(0.036595f, 0.739231f, -0.002247f), new Vector3(0.033196f, 0.7340671f, 0.010182f), new Vector3(0.024012f, 0.730088f, 0.018779f), new Vector3(0.019068f, 0.745362f, -0.041102f), new Vector3(0.015064f, 0.727072f, 0.025942f) };     //Neck
            meshSeamVerts[0][3][1][4] = new Vector3[] { new Vector3(0.074902f, 0.464262f, 0.048984f), new Vector3(0.038165f, 0.46175f, 0.076512f), new Vector3(0f, 0.459831f, 0.086915f), new Vector3(0.023888f, 0.466736f, -0.070186f), new Vector3(0.090889f, 0.466976f, 0.004244f), new Vector3(0.075017f, 0.467595f, -0.036638f), new Vector3(0.053979f, 0.467616f, -0.05926f), new Vector3(0f, 0.466101f, -0.069595f), new Vector3(-0.074902f, 0.464262f, 0.048999f), new Vector3(-0.038183f, 0.46175f, 0.076757f), new Vector3(-0.023888f, 0.466736f, -0.070186f), new Vector3(-0.090889f, 0.466976f, 0.004244f), new Vector3(-0.075017f, 0.467595f, -0.036638f), new Vector3(-0.053979f, 0.467616f, -0.05926f) };     //Waist
            meshSeamVerts[0][3][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][3][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][3][2] = new Vector3[7][];         //Toddler LOD2 seams
            meshSeamVerts[0][3][2][0] = new Vector3[] { new Vector3(0.065962f, 0.08125f, 0.017437f), new Vector3(0.040309f, 0.08123501f, 0.018042f), new Vector3(0.079346f, 0.08125f, -0.005758f), new Vector3(0.065284f, 0.081243f, -0.025861f), new Vector3(0.032729f, 0.081235f, 0.001915f), new Vector3(0.041073f, 0.08125f, -0.024442f), new Vector3(-0.065962f, 0.08125f, 0.017437f), new Vector3(-0.040309f, 0.08123501f, 0.018042f), new Vector3(-0.079346f, 0.08125f, -0.005758f), new Vector3(-0.065284f, 0.081243f, -0.025861f), new Vector3(-0.032729f, 0.081235f, 0.001915f), new Vector3(-0.041073f, 0.08125f, -0.024442f) };     //Ankles
            meshSeamVerts[0][3][2][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][3][2][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][3][2][3] = new Vector3[] { new Vector3(0f, 0.725429f, 0.028412f), new Vector3(0f, 0.745978f, -0.045768f), new Vector3(-0.039258f, 0.743412f, -0.015598f), new Vector3(-0.024012f, 0.730088f, 0.018779f), new Vector3(-0.019068f, 0.745362f, -0.041102f), new Vector3(-0.036595f, 0.739231f, -0.002247f), new Vector3(-0.033196f, 0.7340671f, 0.010182f), new Vector3(-0.035021f, 0.7452821f, -0.029798f), new Vector3(-0.015064f, 0.727072f, 0.025942f), new Vector3(0.039258f, 0.743412f, -0.015598f), new Vector3(0.024012f, 0.730088f, 0.018779f), new Vector3(0.019068f, 0.745362f, -0.041102f), new Vector3(0.036595f, 0.739231f, -0.002247f), new Vector3(0.033196f, 0.7340671f, 0.010182f), new Vector3(0.035021f, 0.7452821f, -0.029798f), new Vector3(0.015064f, 0.727072f, 0.025942f) };     //Neck
            meshSeamVerts[0][3][2][4] = new Vector3[] { new Vector3(0.074902f, 0.464262f, 0.048984f), new Vector3(0.038165f, 0.46175f, 0.076512f), new Vector3(0f, 0.459831f, 0.086915f), new Vector3(0.038933f, 0.467176f, -0.064723f), new Vector3(0.090889f, 0.466976f, 0.004244f), new Vector3(0.075017f, 0.467595f, -0.036638f), new Vector3(0f, 0.466101f, -0.069595f), new Vector3(-0.074902f, 0.464262f, 0.048999f), new Vector3(-0.038183f, 0.46175f, 0.076757f), new Vector3(-0.038933f, 0.467176f, -0.064723f), new Vector3(-0.090889f, 0.466976f, 0.004244f), new Vector3(-0.075017f, 0.467595f, -0.036638f) };     //Waist
            meshSeamVerts[0][3][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][3][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[0][3][3] = new Vector3[7][];         //Toddler LOD3 seams
            meshSeamVerts[0][3][3][0] = new Vector3[] { new Vector3(0.053136f, 0.081243f, 0.017739f), new Vector3(0.079346f, 0.08125f, -0.005758f), new Vector3(0.053179f, 0.081246f, -0.025151f), new Vector3(0.032729f, 0.081235f, 0.001915f), new Vector3(-0.053136f, 0.081243f, 0.017739f), new Vector3(-0.079346f, 0.08125f, -0.005758f), new Vector3(-0.053179f, 0.081246f, -0.025151f), new Vector3(-0.032729f, 0.081235f, 0.001915f) };     //Ankles
            meshSeamVerts[0][3][3][1] = new Vector3[0];     //Tail
            meshSeamVerts[0][3][3][2] = new Vector3[0];     //Ears
            meshSeamVerts[0][3][3][3] = new Vector3[] { new Vector3(0f, 0.725429f, 0.028412f), new Vector3(0f, 0.745978f, -0.045768f) };     //Neck
            meshSeamVerts[0][3][3][4] = new Vector3[] { new Vector3(0.056534f, 0.463006f, 0.062748f), new Vector3(0f, 0.459831f, 0.086915f), new Vector3(0.056975f, 0.467386f, -0.050681f), new Vector3(0.090889f, 0.466976f, 0.004244f), new Vector3(0f, 0.466101f, -0.069595f), new Vector3(-0.056542f, 0.463006f, 0.062878f), new Vector3(-0.056975f, 0.467386f, -0.050681f), new Vector3(-0.090889f, 0.466976f, 0.004244f) };     //Waist
            meshSeamVerts[0][3][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[0][3][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[1] = new Vector3[4][][][];        //ageSpecies
            meshSeamVerts[1][0] = new Vector3[4][][];         //Adult Dog
            meshSeamVerts[1][0][0] = new Vector3[7][];         //Adult Dog LOD0 seams
            meshSeamVerts[1][0][0][0] = new Vector3[] { new Vector3(0.091319f, 0.110002f, 0.100705f), new Vector3(0.089504f, 0.111499f, 0.110355f), new Vector3(0.083803f, 0.1126f, 0.11691f), new Vector3(0.072392f, 0.112377f, 0.120393f), new Vector3(0.059744f, 0.111575f, 0.116427f), new Vector3(0.052912f, 0.110878f, 0.108974f), new Vector3(0.050505f, 0.109698f, 0.09901001f), new Vector3(0.050042f, 0.107311f, 0.088836f), new Vector3(0.054359f, 0.104351f, 0.079998f), new Vector3(0.06345101f, 0.101799f, 0.071559f), new Vector3(0.077975f, 0.101195f, 0.071651f), new Vector3(0.087592f, 0.104716f, 0.081086f), new Vector3(0.091225f, 0.107681f, 0.090575f), new Vector3(-0.050505f, 0.109698f, 0.09901001f), new Vector3(-0.052912f, 0.110878f, 0.108974f), new Vector3(-0.059744f, 0.111575f, 0.116427f), new Vector3(-0.072392f, 0.112377f, 0.120393f), new Vector3(-0.083803f, 0.1126f, 0.11691f), new Vector3(-0.089504f, 0.111499f, 0.110355f), new Vector3(-0.091319f, 0.110002f, 0.100705f), new Vector3(-0.091225f, 0.107681f, 0.090575f), new Vector3(-0.087592f, 0.104716f, 0.081086f), new Vector3(-0.077975f, 0.101195f, 0.071651f), new Vector3(-0.06345101f, 0.101799f, 0.071559f), new Vector3(-0.054359f, 0.104351f, 0.079998f), new Vector3(-0.050042f, 0.107311f, 0.088836f), new Vector3(0.070713f, 0.101497f, 0.069976f), new Vector3(-0.070713f, 0.101497f, 0.069976f), new Vector3(0.072045f, 0.10763f, -0.502381f), new Vector3(0.07268f, 0.10763f, -0.512439f), new Vector3(0.069921f, 0.107654f, -0.5222141f), new Vector3(0.065669f, 0.107896f, -0.529511f), new Vector3(0.053849f, 0.107928f, -0.531096f), new Vector3(0.041362f, 0.107749f, -0.528099f), new Vector3(0.037166f, 0.107536f, -0.5213591f), new Vector3(0.035074f, 0.10762f, -0.512302f), new Vector3(0.035216f, 0.1077f, -0.5021471f), new Vector3(0.039253f, 0.107855f, -0.491577f), new Vector3(0.046065f, 0.10827f, -0.486399f), new Vector3(0.054622f, 0.108626f, -0.484898f), new Vector3(0.062579f, 0.108652f, -0.485451f), new Vector3(0.068362f, 0.108161f, -0.492384f), new Vector3(-0.041362f, 0.107749f, -0.528099f), new Vector3(-0.037166f, 0.107536f, -0.5213591f), new Vector3(-0.035074f, 0.10762f, -0.512302f), new Vector3(-0.035216f, 0.1077f, -0.5021471f), new Vector3(-0.039253f, 0.107855f, -0.491577f), new Vector3(-0.046065f, 0.10827f, -0.486399f), new Vector3(-0.054622f, 0.108626f, -0.484898f), new Vector3(-0.062579f, 0.108652f, -0.485451f), new Vector3(-0.068362f, 0.108161f, -0.492384f), new Vector3(-0.072045f, 0.10763f, -0.502381f), new Vector3(-0.07268f, 0.10763f, -0.512439f), new Vector3(-0.069921f, 0.107654f, -0.5222141f), new Vector3(-0.065669f, 0.107896f, -0.529511f), new Vector3(-0.053849f, 0.107928f, -0.531096f) };     //Ankles
            meshSeamVerts[1][0][0][1] = new Vector3[] { new Vector3(0.021073f, 0.577363f, -0.43746f), new Vector3(0.02086f, 0.592635f, -0.42631f), new Vector3(0.012646f, 0.600489f, -0.423659f), new Vector3(0f, 0.604671f, -0.423148f), new Vector3(0f, 0.562809f, -0.451407f), new Vector3(0.014413f, 0.569726f, -0.445067f), new Vector3(-0.021073f, 0.577363f, -0.437461f), new Vector3(-0.02086f, 0.592635f, -0.426311f), new Vector3(-0.012646f, 0.600489f, -0.423659f), new Vector3(-0.014413f, 0.569726f, -0.445067f) };     //Tail
            meshSeamVerts[1][0][0][2] = new Vector3[] { new Vector3(-0.067989f, 0.768746f, 0.27049f), new Vector3(-0.063373f, 0.779868f, 0.276023f), new Vector3(-0.068046f, 0.761078f, 0.258345f), new Vector3(-0.066053f, 0.759634f, 0.242935f), new Vector3(-0.06145401f, 0.769566f, 0.230691f), new Vector3(-0.057924f, 0.780925f, 0.229641f), new Vector3(-0.05198f, 0.798391f, 0.236866f), new Vector3(-0.057639f, 0.793634f, 0.27381f), new Vector3(-0.053146f, 0.803787f, 0.266705f), new Vector3(-0.06815f, 0.764306f, 0.265297f), new Vector3(-0.060771f, 0.786525f, 0.276518f), new Vector3(-0.055271f, 0.7991011f, 0.270695f), new Vector3(-0.050504f, 0.808143f, 0.255652f), new Vector3(-0.050653f, 0.803242f, 0.243345f), new Vector3(-0.053795f, 0.7924451f, 0.232829f), new Vector3(-0.066883f, 0.760161f, 0.251542f), new Vector3(0.067989f, 0.768746f, 0.27049f), new Vector3(0.063373f, 0.779868f, 0.276023f), new Vector3(0.068046f, 0.761078f, 0.258345f), new Vector3(0.066053f, 0.759634f, 0.242935f), new Vector3(0.06145401f, 0.769566f, 0.23069f), new Vector3(0.057924f, 0.780925f, 0.229641f), new Vector3(0.05198f, 0.798391f, 0.236866f), new Vector3(0.057638f, 0.793634f, 0.273811f), new Vector3(0.053146f, 0.803787f, 0.266704f), new Vector3(0.06815f, 0.764306f, 0.265296f), new Vector3(0.060771f, 0.786525f, 0.276519f), new Vector3(0.055271f, 0.7991011f, 0.270695f), new Vector3(0.050504f, 0.808143f, 0.255652f), new Vector3(0.050653f, 0.803242f, 0.243345f), new Vector3(0.053795f, 0.7924451f, 0.232828f), new Vector3(0.066883f, 0.760161f, 0.251542f) };     //Ears
            meshSeamVerts[1][0][0][3] = new Vector3[] { new Vector3(0f, 0.65108f, 0.263976f), new Vector3(-0.031086f, 0.663975f, 0.251473f), new Vector3(-0.040149f, 0.6724421f, 0.244208f), new Vector3(-0.052244f, 0.6996421f, 0.220295f), new Vector3(-0.048166f, 0.719901f, 0.202111f), new Vector3(-0.036981f, 0.735922f, 0.186963f), new Vector3(-0.029402f, 0.743723f, 0.179634f), new Vector3(-0.01039f, 0.752611f, 0.171274f), new Vector3(0f, 0.754215f, 0.169697f), new Vector3(-0.012513f, 0.6536471f, 0.261375f), new Vector3(-0.050186f, 0.690092f, 0.229349f), new Vector3(-0.019823f, 0.74883f, 0.174659f), new Vector3(-0.043192f, 0.7280371f, 0.194325f), new Vector3(0.031086f, 0.663975f, 0.251473f), new Vector3(0.040149f, 0.6724421f, 0.244208f), new Vector3(0.052244f, 0.6996421f, 0.220295f), new Vector3(0.048166f, 0.719901f, 0.202111f), new Vector3(0.036981f, 0.735922f, 0.186963f), new Vector3(0.029402f, 0.743723f, 0.179634f), new Vector3(0.01039f, 0.752611f, 0.171274f), new Vector3(0.012513f, 0.6536471f, 0.261375f), new Vector3(0.050186f, 0.690092f, 0.229349f), new Vector3(0.019823f, 0.74883f, 0.174659f), new Vector3(0.043192f, 0.7280371f, 0.194325f), new Vector3(0.051507f, 0.710206f, 0.210656f), new Vector3(0.046314f, 0.680477f, 0.237306f), new Vector3(0.022854f, 0.658232f, 0.256744f), new Vector3(-0.051507f, 0.710206f, 0.210656f), new Vector3(-0.046314f, 0.680477f, 0.237306f), new Vector3(-0.022854f, 0.658232f, 0.256744f) };     //Neck
            meshSeamVerts[1][0][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][0][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][0][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][0][1] = new Vector3[7][];         //Adult Dog LOD1 seams
            meshSeamVerts[1][0][1][0] = new Vector3[] { new Vector3(0.089504f, 0.111499f, 0.110355f), new Vector3(0.083803f, 0.1126f, 0.11691f), new Vector3(0.072392f, 0.112377f, 0.120393f), new Vector3(0.059744f, 0.111575f, 0.116427f), new Vector3(0.052912f, 0.110878f, 0.108974f), new Vector3(0.050273f, 0.108504f, 0.093923f), new Vector3(0.054359f, 0.104351f, 0.079998f), new Vector3(0.06345101f, 0.101799f, 0.071559f), new Vector3(0.077975f, 0.101195f, 0.071651f), new Vector3(0.087592f, 0.104716f, 0.081086f), new Vector3(0.091272f, 0.108841f, 0.09564f), new Vector3(-0.052912f, 0.110878f, 0.108974f), new Vector3(-0.059744f, 0.111575f, 0.116427f), new Vector3(-0.072392f, 0.112377f, 0.120393f), new Vector3(-0.083803f, 0.1126f, 0.11691f), new Vector3(-0.089504f, 0.111499f, 0.110355f), new Vector3(-0.091272f, 0.108841f, 0.09564f), new Vector3(-0.087592f, 0.104716f, 0.081086f), new Vector3(-0.077975f, 0.101195f, 0.071651f), new Vector3(-0.06345101f, 0.101799f, 0.071559f), new Vector3(-0.054359f, 0.104351f, 0.079998f), new Vector3(-0.050273f, 0.108504f, 0.093923f), new Vector3(0.070713f, 0.101497f, 0.069976f), new Vector3(-0.070713f, 0.101497f, 0.069976f), new Vector3(0.067952f, 0.119676f, -0.490161f), new Vector3(0.071998f, 0.120309f, -0.505297f), new Vector3(0.06767501f, 0.121354f, -0.524048f), new Vector3(0.053809f, 0.121686f, -0.530085f), new Vector3(0.039275f, 0.121442f, -0.522727f), new Vector3(0.035499f, 0.12054f, -0.504781f), new Vector3(0.039851f, 0.1193f, -0.489503f), new Vector3(0.046574f, 0.119315f, -0.484606f), new Vector3(0.054669f, 0.119597f, -0.483415f), new Vector3(0.062031f, 0.119659f, -0.483632f), new Vector3(-0.039275f, 0.121442f, -0.522727f), new Vector3(-0.035499f, 0.12054f, -0.504781f), new Vector3(-0.039851f, 0.1193f, -0.489503f), new Vector3(-0.046574f, 0.119315f, -0.484606f), new Vector3(-0.054669f, 0.119597f, -0.483415f), new Vector3(-0.062031f, 0.119659f, -0.483632f), new Vector3(-0.06767501f, 0.121354f, -0.524048f), new Vector3(-0.053809f, 0.121686f, -0.530085f), new Vector3(-0.067952f, 0.119676f, -0.490161f), new Vector3(-0.071998f, 0.120309f, -0.505297f) };     //Ankles
            meshSeamVerts[1][0][1][1] = new Vector3[] { new Vector3(0.021073f, 0.577363f, -0.43746f), new Vector3(0.016753f, 0.596562f, -0.424984f), new Vector3(0f, 0.604671f, -0.423148f), new Vector3(0f, 0.562809f, -0.451407f), new Vector3(0.014413f, 0.569726f, -0.445067f), new Vector3(-0.021073f, 0.577363f, -0.437461f), new Vector3(-0.016753f, 0.596562f, -0.424985f), new Vector3(-0.014413f, 0.569726f, -0.445067f) };     //Tail
            meshSeamVerts[1][0][1][2] = new Vector3[] { new Vector3(-0.067992f, 0.768754f, 0.270502f), new Vector3(-0.066058f, 0.759642f, 0.242946f), new Vector3(-0.061457f, 0.769576f, 0.230701f), new Vector3(-0.057926f, 0.780935f, 0.229652f), new Vector3(-0.051985f, 0.798399f, 0.236878f), new Vector3(-0.057639f, 0.793635f, 0.27381f), new Vector3(-0.053148f, 0.8037941f, 0.266716f), new Vector3(-0.068101f, 0.7626981f, 0.261832f), new Vector3(-0.060771f, 0.786526f, 0.276518f), new Vector3(-0.05527201f, 0.799109f, 0.270708f), new Vector3(-0.050583f, 0.8057f, 0.249509f), new Vector3(-0.0538f, 0.792453f, 0.232841f), new Vector3(-0.066888f, 0.760171f, 0.251553f), new Vector3(0.067992f, 0.768754f, 0.270502f), new Vector3(0.068101f, 0.7626981f, 0.261832f), new Vector3(0.066058f, 0.759642f, 0.242946f), new Vector3(0.061457f, 0.769576f, 0.230701f), new Vector3(0.057926f, 0.780935f, 0.229652f), new Vector3(0.051985f, 0.798399f, 0.236878f), new Vector3(0.057638f, 0.793635f, 0.27381f), new Vector3(0.053148f, 0.8037941f, 0.266716f), new Vector3(0.060771f, 0.786526f, 0.276518f), new Vector3(0.05527201f, 0.799109f, 0.270708f), new Vector3(0.050583f, 0.8057f, 0.249509f), new Vector3(0.0538f, 0.792453f, 0.232841f), new Vector3(0.066888f, 0.760171f, 0.251553f) };     //Ears
            meshSeamVerts[1][0][1][3] = new Vector3[] { new Vector3(0f, 0.65108f, 0.263976f), new Vector3(-0.031086f, 0.663975f, 0.251473f), new Vector3(-0.047474f, 0.719901f, 0.202111f), new Vector3(-0.039776f, 0.7319791f, 0.190644f), new Vector3(-0.029402f, 0.743723f, 0.179634f), new Vector3(-0.015106f, 0.75072f, 0.172967f), new Vector3(0f, 0.754215f, 0.169697f), new Vector3(-0.050186f, 0.690092f, 0.229349f), new Vector3(0.031086f, 0.663975f, 0.251473f), new Vector3(0.043231f, 0.67646f, 0.240757f), new Vector3(0.051443f, 0.704924f, 0.215476f), new Vector3(0.047474f, 0.719901f, 0.202111f), new Vector3(0.029402f, 0.743723f, 0.179634f), new Vector3(0.017683f, 0.65594f, 0.25906f), new Vector3(0.050186f, 0.690092f, 0.229349f), new Vector3(0.015106f, 0.75072f, 0.172967f), new Vector3(0.039776f, 0.7319791f, 0.190644f), new Vector3(-0.051443f, 0.704924f, 0.215476f), new Vector3(-0.043231f, 0.67646f, 0.240757f), new Vector3(-0.017683f, 0.65594f, 0.25906f) };     //Neck
            meshSeamVerts[1][0][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][0][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][0][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][0][2] = new Vector3[7][];         //Adult Dog LOD2 seams
            meshSeamVerts[1][0][2][0] = new Vector3[] { new Vector3(0.09131901f, 0.110002f, 0.100705f), new Vector3(0.083803f, 0.1126f, 0.11691f), new Vector3(0.059744f, 0.111575f, 0.116427f), new Vector3(0.050505f, 0.109698f, 0.09901f), new Vector3(0.054359f, 0.104351f, 0.079998f), new Vector3(0.087592f, 0.104716f, 0.081086f), new Vector3(-0.050505f, 0.109698f, 0.09901f), new Vector3(-0.059744f, 0.111575f, 0.116427f), new Vector3(-0.083803f, 0.1126f, 0.11691f), new Vector3(-0.09131901f, 0.110002f, 0.100705f), new Vector3(-0.087592f, 0.104716f, 0.081086f), new Vector3(-0.054359f, 0.104351f, 0.079998f), new Vector3(0.070713f, 0.101497f, 0.069976f), new Vector3(-0.070713f, 0.101497f, 0.069976f), new Vector3(0.071462f, 0.154987f, -0.495282f), new Vector3(0.069497f, 0.159743f, -0.5185031f), new Vector3(0.053665f, 0.162502f, -0.530194f), new Vector3(0.036587f, 0.159017f, -0.515529f), new Vector3(0.035549f, 0.154733f, -0.494399f), new Vector3(0.0472f, 0.151351f, -0.47717f), new Vector3(0.062333f, 0.151765f, -0.476295f), new Vector3(-0.036587f, 0.159017f, -0.515529f), new Vector3(-0.035549f, 0.154733f, -0.494399f), new Vector3(-0.0472f, 0.151351f, -0.47717f), new Vector3(-0.062333f, 0.151765f, -0.476295f), new Vector3(-0.071462f, 0.154987f, -0.495282f), new Vector3(-0.069497f, 0.159743f, -0.5185031f), new Vector3(-0.053665f, 0.162502f, -0.530194f) };     //Ankles
            meshSeamVerts[1][0][2][1] = new Vector3[] { new Vector3(0f, 0.604671f, -0.423148f), new Vector3(0f, 0.562809f, -0.451407f), new Vector3(0.019152f, 0.575161f, -0.439654f), new Vector3(-0.019152f, 0.575161f, -0.439654f) };     //Tail
            meshSeamVerts[1][0][2][2] = new Vector3[] { new Vector3(-0.067992f, 0.768753f, 0.270502f), new Vector3(-0.063376f, 0.779874f, 0.276034f), new Vector3(-0.068048f, 0.761084f, 0.258355f), new Vector3(-0.066058f, 0.759641f, 0.242946f), new Vector3(-0.057926f, 0.780934f, 0.229652f), new Vector3(-0.053148f, 0.8037931f, 0.266716f), new Vector3(-0.056455f, 0.796371f, 0.272259f), new Vector3(-0.050583f, 0.805699f, 0.24951f), new Vector3(-0.052892f, 0.7954251f, 0.23486f), new Vector3(0.067992f, 0.768753f, 0.270503f), new Vector3(0.063376f, 0.779874f, 0.276035f), new Vector3(0.068048f, 0.761084f, 0.258356f), new Vector3(0.066058f, 0.759641f, 0.242947f), new Vector3(0.057926f, 0.780934f, 0.229653f), new Vector3(0.052892f, 0.7954251f, 0.23486f), new Vector3(0.056455f, 0.796371f, 0.27226f), new Vector3(0.053148f, 0.8037931f, 0.266717f), new Vector3(0.050583f, 0.805699f, 0.24951f) };     //Ears
            meshSeamVerts[1][0][2][3] = new Vector3[] { new Vector3(0f, 0.65108f, 0.263976f), new Vector3(-0.040149f, 0.6724421f, 0.244208f), new Vector3(-0.051842f, 0.6996421f, 0.220295f), new Vector3(-0.047474f, 0.719901f, 0.202111f), new Vector3(-0.036981f, 0.735922f, 0.186963f), new Vector3(0f, 0.754215f, 0.169697f), new Vector3(-0.019823f, 0.74883f, 0.174659f), new Vector3(0.040149f, 0.6724421f, 0.244208f), new Vector3(0.051842f, 0.6996421f, 0.220295f), new Vector3(0.047474f, 0.719901f, 0.202111f), new Vector3(0.036981f, 0.735922f, 0.186963f), new Vector3(0.019823f, 0.74883f, 0.174659f), new Vector3(0.022854f, 0.658232f, 0.256744f), new Vector3(-0.022854f, 0.658232f, 0.256744f) };     //Neck
            meshSeamVerts[1][0][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][0][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][0][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][0][3] = new Vector3[7][];         //Adult Dog LOD3 seams
            meshSeamVerts[1][0][3][0] = new Vector3[] { new Vector3(0.083803f, 0.1126f, 0.11691f), new Vector3(0.059744f, 0.111575f, 0.116427f), new Vector3(0.054359f, 0.104351f, 0.079998f), new Vector3(0.087592f, 0.104716f, 0.081086f), new Vector3(-0.059744f, 0.111575f, 0.116427f), new Vector3(-0.083803f, 0.1126f, 0.11691f), new Vector3(-0.087592f, 0.104716f, 0.081086f), new Vector3(-0.054359f, 0.104351f, 0.079998f), new Vector3(0.070713f, 0.101497f, 0.069976f), new Vector3(-0.070713f, 0.101497f, 0.069976f), new Vector3(0.069497f, 0.159743f, -0.5185031f), new Vector3(0.053665f, 0.162502f, -0.530194f), new Vector3(0.036587f, 0.159017f, -0.515529f), new Vector3(0.0472f, 0.151351f, -0.47717f), new Vector3(0.062333f, 0.151765f, -0.476295f), new Vector3(-0.036587f, 0.159017f, -0.515529f), new Vector3(-0.0472f, 0.151351f, -0.47717f), new Vector3(-0.062333f, 0.151765f, -0.476295f), new Vector3(-0.069497f, 0.159743f, -0.5185031f), new Vector3(-0.053665f, 0.162502f, -0.530194f) };     //Ankles
            meshSeamVerts[1][0][3][1] = new Vector3[] { new Vector3(0f, 0.604671f, -0.423148f), new Vector3(0f, 0.562809f, -0.451407f), new Vector3(0.019152f, 0.575161f, -0.439654f), new Vector3(-0.019152f, 0.575161f, -0.439654f) };     //Tail
            meshSeamVerts[1][0][3][2] = new Vector3[] { new Vector3(-0.065684f, 0.774313f, 0.273268f), new Vector3(-0.06681f, 0.75988f, 0.25055f), new Vector3(-0.057926f, 0.780934f, 0.229652f), new Vector3(-0.054662f, 0.800348f, 0.269728f), new Vector3(-0.050898f, 0.80237f, 0.242183f), new Vector3(0.065684f, 0.774313f, 0.273269f), new Vector3(0.06681f, 0.75988f, 0.25055f), new Vector3(0.057926f, 0.780934f, 0.229653f), new Vector3(0.054662f, 0.800348f, 0.269728f), new Vector3(0.050898f, 0.80237f, 0.242183f) };     //Ears
            meshSeamVerts[1][0][3][3] = new Vector3[] { new Vector3(0f, 0.65108f, 0.263976f), new Vector3(-0.03184f, 0.664723f, 0.250976f), new Vector3(-0.051246f, 0.710773f, 0.211633f), new Vector3(0f, 0.754215f, 0.169697f), new Vector3(-0.029526f, 0.7441601f, 0.180104f), new Vector3(0.051246f, 0.710773f, 0.211633f), new Vector3(0.029526f, 0.7441601f, 0.180104f), new Vector3(0.03184f, 0.664723f, 0.250976f) };     //Neck
            meshSeamVerts[1][0][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][0][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][0][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[1][1] = new Vector3[4][][];         //Puppy
            meshSeamVerts[1][1][0] = new Vector3[7][];         //Puppy LOD0 seams
            meshSeamVerts[1][1][0][0] = new Vector3[] { new Vector3(0.036768f, 0.026227f, 0.013628f), new Vector3(0.03593601f, 0.02782f, 0.017807f), new Vector3(0.033462f, 0.028612f, 0.02055f), new Vector3(0.027855f, 0.029023f, 0.022005f), new Vector3(0.022458f, 0.028545f, 0.020437f), new Vector3(0.019409f, 0.027613f, 0.017329f), new Vector3(0.018312f, 0.026146f, 0.013026f), new Vector3(0.018188f, 0.024345f, 0.008862f), new Vector3(0.019965f, 0.022352f, 0.004917f), new Vector3(0.024243f, 0.020768f, 0.001611f), new Vector3(0.03077f, 0.020477f, 0.001638f), new Vector3(0.035116f, 0.022422f, 0.005462f), new Vector3(0.036745f, 0.024428f, 0.009618001f), new Vector3(-0.018312f, 0.026146f, 0.013026f), new Vector3(-0.019409f, 0.027613f, 0.017329f), new Vector3(-0.022458f, 0.028545f, 0.020437f), new Vector3(-0.027855f, 0.029023f, 0.022005f), new Vector3(-0.033462f, 0.028612f, 0.02055f), new Vector3(-0.03593601f, 0.02782f, 0.017807f), new Vector3(-0.036768f, 0.026227f, 0.013628f), new Vector3(-0.036745f, 0.024428f, 0.009618001f), new Vector3(-0.035116f, 0.022422f, 0.005462f), new Vector3(-0.03077f, 0.020477f, 0.001639f), new Vector3(-0.024243f, 0.020768f, 0.001611f), new Vector3(-0.019965f, 0.022352f, 0.004917f), new Vector3(-0.018188f, 0.024345f, 0.008862f), new Vector3(0.027483f, 0.020485f, 0.000959f), new Vector3(-0.027483f, 0.020485f, 0.000958f), new Vector3(0.035483f, 0.026637f, -0.156033f), new Vector3(0.035769f, 0.026894f, -0.160339f), new Vector3(0.034603f, 0.027203f, -0.1644f), new Vector3(0.032792f, 0.027444f, -0.167616f), new Vector3(0.02773f, 0.027534f, -0.16831f), new Vector3(0.022383f, 0.027477f, -0.16705f), new Vector3(0.020598f, 0.027186f, -0.164045f), new Vector3(0.019747f, 0.026911f, -0.160276f), new Vector3(0.019792f, 0.026623f, -0.155934f), new Vector3(0.021326f, 0.026195f, -0.151986f), new Vector3(0.024209f, 0.025911f, -0.149262f), new Vector3(0.02806f, 0.025941f, -0.148507f), new Vector3(0.031656f, 0.026052f, -0.148969f), new Vector3(0.03409901f, 0.026335f, -0.152187f), new Vector3(-0.022383f, 0.027477f, -0.16705f), new Vector3(-0.020598f, 0.027186f, -0.164045f), new Vector3(-0.019747f, 0.026911f, -0.160276f), new Vector3(-0.019792f, 0.026623f, -0.155934f), new Vector3(-0.021326f, 0.026195f, -0.151986f), new Vector3(-0.024209f, 0.025911f, -0.149262f), new Vector3(-0.02806f, 0.025941f, -0.148507f), new Vector3(-0.031656f, 0.026052f, -0.148969f), new Vector3(-0.03409901f, 0.026335f, -0.152187f), new Vector3(-0.035483f, 0.026637f, -0.156033f), new Vector3(-0.035769f, 0.026894f, -0.160339f), new Vector3(-0.034603f, 0.027203f, -0.1644f), new Vector3(-0.032792f, 0.027444f, -0.167616f), new Vector3(-0.02773f, 0.027534f, -0.16831f) };     //Ankles
            meshSeamVerts[1][1][0][1] = new Vector3[] { new Vector3(0.008108f, 0.141443f, -0.133774f), new Vector3(0.008026f, 0.147319f, -0.129635f), new Vector3(0.004865f, 0.150341f, -0.128202f), new Vector3(0f, 0.15195f, -0.127799f), new Vector3(0f, 0.135842f, -0.138938f), new Vector3(0.005545001f, 0.138504f, -0.136499f), new Vector3(-0.008108f, 0.141443f, -0.133774f), new Vector3(-0.008026f, 0.147319f, -0.129635f), new Vector3(-0.004865f, 0.150341f, -0.128202f), new Vector3(-0.005545001f, 0.138504f, -0.136499f) };     //Tail
            meshSeamVerts[1][1][0][2] = new Vector3[] { new Vector3(-0.03149f, 0.202056f, 0.044791f), new Vector3(-0.029873f, 0.206929f, 0.047409f), new Vector3(-0.032279f, 0.198817f, 0.03928f), new Vector3(-0.031583f, 0.198393f, 0.032406f), new Vector3(-0.029422f, 0.202985f, 0.0271f), new Vector3(-0.027024f, 0.208296f, 0.026351f), new Vector3(-0.023789f, 0.217317f, 0.030254f), new Vector3(-0.0268f, 0.213567f, 0.047074f), new Vector3(-0.024298f, 0.218782f, 0.043602f), new Vector3(-0.03232f, 0.200156f, 0.042419f), new Vector3(-0.028384f, 0.210043f, 0.047718f), new Vector3(-0.025254f, 0.216639f, 0.045571f), new Vector3(-0.023084f, 0.220917f, 0.038748f), new Vector3(-0.023264f, 0.219516f, 0.033205f), new Vector3(-0.02485f, 0.213747f, 0.027885f), new Vector3(-0.031973f, 0.198507f, 0.036242f), new Vector3(0.03149f, 0.202056f, 0.044791f), new Vector3(0.029873f, 0.206929f, 0.047409f), new Vector3(0.032279f, 0.198817f, 0.03928f), new Vector3(0.031583f, 0.198393f, 0.032406f), new Vector3(0.029422f, 0.202985f, 0.0271f), new Vector3(0.027024f, 0.208296f, 0.026351f), new Vector3(0.023789f, 0.217317f, 0.030254f), new Vector3(0.0268f, 0.213567f, 0.047074f), new Vector3(0.024298f, 0.218782f, 0.043602f), new Vector3(0.03232f, 0.200156f, 0.042419f), new Vector3(0.028384f, 0.210043f, 0.047718f), new Vector3(0.025254f, 0.216639f, 0.045571f), new Vector3(0.023084f, 0.220917f, 0.038748f), new Vector3(0.023264f, 0.219516f, 0.033205f), new Vector3(0.02485f, 0.213747f, 0.027885f), new Vector3(0.031973f, 0.198507f, 0.036242f) };     //Ears
            meshSeamVerts[1][1][0][3] = new Vector3[] { new Vector3(0f, 0.145662f, 0.05409f), new Vector3(-0.015691f, 0.152265f, 0.04751001f), new Vector3(-0.020114f, 0.156391f, 0.04372f), new Vector3(-0.026019f, 0.171842f, 0.030004f), new Vector3(-0.024575f, 0.181631f, 0.021221f), new Vector3(-0.019194f, 0.18951f, 0.014427f), new Vector3(-0.01529f, 0.192578f, 0.011842f), new Vector3(-0.005277f, 0.19623f, 0.008721f), new Vector3(0f, 0.197183f, 0.007899f), new Vector3(-0.005727f, 0.146799f, 0.052907f), new Vector3(-0.025166f, 0.166557f, 0.034826f), new Vector3(-0.010536f, 0.194739f, 0.009992f), new Vector3(-0.021994f, 0.185906f, 0.017399f), new Vector3(0.015691f, 0.152265f, 0.04751001f), new Vector3(0.020114f, 0.156391f, 0.04372f), new Vector3(0.026019f, 0.171842f, 0.030004f), new Vector3(0.024575f, 0.181631f, 0.021221f), new Vector3(0.019194f, 0.18951f, 0.014427f), new Vector3(0.01529f, 0.192578f, 0.011842f), new Vector3(0.005277f, 0.19623f, 0.008721f), new Vector3(0.005727f, 0.146799f, 0.052907f), new Vector3(0.025166f, 0.166557f, 0.034826f), new Vector3(0.010536f, 0.194739f, 0.009992f), new Vector3(0.021994f, 0.185906f, 0.017399f), new Vector3(0.025781f, 0.177038f, 0.025226f), new Vector3(0.023169f, 0.161847f, 0.038919f), new Vector3(0.010967f, 0.149102f, 0.050558f), new Vector3(-0.025781f, 0.177038f, 0.025226f), new Vector3(-0.023169f, 0.161847f, 0.038919f), new Vector3(-0.010967f, 0.149102f, 0.050558f) };     //Neck
            meshSeamVerts[1][1][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][1][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][1][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][1][1] = new Vector3[7][];         //Puppy LOD1 seams
            meshSeamVerts[1][1][1][0] = new Vector3[] { new Vector3(0.035936f, 0.027821f, 0.017807f), new Vector3(0.033462f, 0.028613f, 0.020549f), new Vector3(0.027855f, 0.029024f, 0.022004f), new Vector3(0.022458f, 0.028546f, 0.020436f), new Vector3(0.019409f, 0.027614f, 0.017328f), new Vector3(0.01825f, 0.025246f, 0.010943f), new Vector3(0.019965f, 0.022352f, 0.004917f), new Vector3(0.024243f, 0.020769f, 0.00161f), new Vector3(0.03077f, 0.020477f, 0.001638f), new Vector3(0.035116f, 0.022422f, 0.005462f), new Vector3(0.036756f, 0.025328f, 0.011623f), new Vector3(-0.019409f, 0.027614f, 0.017328f), new Vector3(-0.022458f, 0.028546f, 0.020436f), new Vector3(-0.027855f, 0.029024f, 0.022004f), new Vector3(-0.033462f, 0.028613f, 0.020549f), new Vector3(-0.035936f, 0.027821f, 0.017807f), new Vector3(-0.036756f, 0.025328f, 0.011623f), new Vector3(-0.035116f, 0.022422f, 0.005462f), new Vector3(-0.03077f, 0.020477f, 0.001638f), new Vector3(-0.024243f, 0.020769f, 0.00161f), new Vector3(-0.019965f, 0.022352f, 0.004917f), new Vector3(-0.01825f, 0.025246f, 0.010943f), new Vector3(0.027483f, 0.020485f, 0.000958f), new Vector3(-0.027483f, 0.020485f, 0.000958f), new Vector3(0.033966f, 0.02768f, -0.151808f), new Vector3(0.035505f, 0.028593f, -0.157844f), new Vector3(0.03365801f, 0.029292f, -0.165822f), new Vector3(0.02771f, 0.029659f, -0.168411f), new Vector3(0.021542f, 0.029336f, -0.165628f), new Vector3(0.019765f, 0.028546f, -0.157617f), new Vector3(0.021437f, 0.027542f, -0.151612f), new Vector3(0.024334f, 0.027091f, -0.148868f), new Vector3(0.028081f, 0.027075f, -0.148205f), new Vector3(0.031514f, 0.027209f, -0.148604f), new Vector3(-0.021542f, 0.029336f, -0.165628f), new Vector3(-0.019765f, 0.028546f, -0.157617f), new Vector3(-0.021437f, 0.027542f, -0.151612f), new Vector3(-0.024334f, 0.027091f, -0.148868f), new Vector3(-0.028081f, 0.027075f, -0.148205f), new Vector3(-0.031514f, 0.027209f, -0.148604f), new Vector3(-0.03365801f, 0.029292f, -0.165822f), new Vector3(-0.02771f, 0.029659f, -0.168411f), new Vector3(-0.033966f, 0.02768f, -0.151808f), new Vector3(-0.035505f, 0.028593f, -0.157844f) };     //Ankles
            meshSeamVerts[1][1][1][1] = new Vector3[] { new Vector3(0.008108f, 0.141444f, -0.133774f), new Vector3(0.006446f, 0.148831f, -0.128919f), new Vector3(0f, 0.151951f, -0.1278f), new Vector3(0f, 0.135843f, -0.138939f), new Vector3(0.005545f, 0.138505f, -0.136499f), new Vector3(-0.008108f, 0.141444f, -0.133774f), new Vector3(-0.006445f, 0.148831f, -0.128919f), new Vector3(-0.005545f, 0.138505f, -0.136499f) };     //Tail
            meshSeamVerts[1][1][1][2] = new Vector3[] { new Vector3(-0.03149001f, 0.202056f, 0.044792f), new Vector3(-0.031583f, 0.198393f, 0.032406f), new Vector3(-0.029422f, 0.202985f, 0.0271f), new Vector3(-0.027024f, 0.208296f, 0.026351f), new Vector3(-0.023789f, 0.217317f, 0.030254f), new Vector3(-0.0268f, 0.213567f, 0.047074f), new Vector3(-0.024298f, 0.218782f, 0.043602f), new Vector3(-0.032299f, 0.199486f, 0.04085f), new Vector3(-0.028384f, 0.210043f, 0.047718f), new Vector3(-0.025254f, 0.216639f, 0.045571f), new Vector3(-0.023174f, 0.220216f, 0.035976f), new Vector3(-0.02485f, 0.213747f, 0.027885f), new Vector3(-0.031973f, 0.198507f, 0.036242f), new Vector3(0.03149001f, 0.202056f, 0.04479101f), new Vector3(0.032299f, 0.199486f, 0.04085f), new Vector3(0.031583f, 0.198393f, 0.032406f), new Vector3(0.029422f, 0.202985f, 0.0271f), new Vector3(0.027024f, 0.208296f, 0.026351f), new Vector3(0.023789f, 0.217317f, 0.030254f), new Vector3(0.0268f, 0.213567f, 0.047074f), new Vector3(0.024298f, 0.218782f, 0.043602f), new Vector3(0.028384f, 0.210043f, 0.047718f), new Vector3(0.025254f, 0.216639f, 0.045571f), new Vector3(0.023174f, 0.220216f, 0.035976f), new Vector3(0.02485f, 0.213747f, 0.027885f), new Vector3(0.031973f, 0.198507f, 0.036242f) };     //Ears
            meshSeamVerts[1][1][1][3] = new Vector3[] { new Vector3(0f, 0.145662f, 0.05409f), new Vector3(-0.015691f, 0.152266f, 0.04751001f), new Vector3(-0.024575f, 0.181631f, 0.021221f), new Vector3(-0.020594f, 0.187709f, 0.015912f), new Vector3(-0.01529f, 0.192578f, 0.011842f), new Vector3(-0.007906f, 0.195485f, 0.009356001f), new Vector3(0f, 0.197183f, 0.007898f), new Vector3(-0.025166f, 0.166557f, 0.034826f), new Vector3(0.015691f, 0.152266f, 0.04751001f), new Vector3(0.021641f, 0.159119f, 0.04131901f), new Vector3(0.0259f, 0.17444f, 0.027614f), new Vector3(0.024575f, 0.181631f, 0.021221f), new Vector3(0.01529f, 0.192578f, 0.011842f), new Vector3(0.008347f, 0.147951f, 0.051732f), new Vector3(0.025166f, 0.166557f, 0.034826f), new Vector3(0.007906f, 0.195485f, 0.009356001f), new Vector3(0.020594f, 0.187709f, 0.015912f), new Vector3(-0.0259f, 0.17444f, 0.027614f), new Vector3(-0.021641f, 0.159119f, 0.04131901f), new Vector3(-0.008347f, 0.147951f, 0.051732f) };     //Neck
            meshSeamVerts[1][1][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][1][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][1][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][1][2] = new Vector3[7][];         //Puppy LOD2 seams
            meshSeamVerts[1][1][2][0] = new Vector3[] { new Vector3(0.036768f, 0.026228f, 0.013627f), new Vector3(0.033462f, 0.028613f, 0.020549f), new Vector3(0.022458f, 0.028546f, 0.020436f), new Vector3(0.018312f, 0.026146f, 0.013025f), new Vector3(0.019965f, 0.022352f, 0.004917f), new Vector3(0.035116f, 0.022422f, 0.005462f), new Vector3(-0.018312f, 0.026146f, 0.013025f), new Vector3(-0.022458f, 0.028546f, 0.020436f), new Vector3(-0.033462f, 0.028613f, 0.020549f), new Vector3(-0.036768f, 0.026228f, 0.013627f), new Vector3(-0.035116f, 0.022422f, 0.005462f), new Vector3(-0.019965f, 0.022352f, 0.004917f), new Vector3(0.027483f, 0.020485f, 0.000958f), new Vector3(-0.027483f, 0.020485f, 0.000958f), new Vector3(0.035221f, 0.033182f, -0.154798f), new Vector3(0.03444f, 0.035381f, -0.164102f), new Vector3(0.027588f, 0.036252f, -0.168875f), new Vector3(0.020287f, 0.035234f, -0.163587f), new Vector3(0.01974f, 0.033091f, -0.154437f), new Vector3(0.024645f, 0.031387f, -0.147424f), new Vector3(0.031218f, 0.031604f, -0.147082f), new Vector3(-0.020287f, 0.035234f, -0.163587f), new Vector3(-0.01974f, 0.033091f, -0.154437f), new Vector3(-0.024645f, 0.031387f, -0.147424f), new Vector3(-0.031218f, 0.031604f, -0.147082f), new Vector3(-0.035221f, 0.033182f, -0.154798f), new Vector3(-0.03444f, 0.035381f, -0.164102f), new Vector3(-0.027588f, 0.036252f, -0.168875f) };     //Ankles
            meshSeamVerts[1][1][2][1] = new Vector3[] { new Vector3(0f, 0.151951f, -0.1278f), new Vector3(0f, 0.135843f, -0.138939f), new Vector3(0.007262f, 0.14805f, -0.129288f), new Vector3(-0.007262f, 0.14805f, -0.129289f) };     //Tail
            meshSeamVerts[1][1][2][2] = new Vector3[] { new Vector3(-0.03149001f, 0.202056f, 0.04479101f), new Vector3(-0.029873f, 0.206929f, 0.047409f), new Vector3(-0.032279f, 0.198817f, 0.03928f), new Vector3(-0.031583f, 0.198393f, 0.032406f), new Vector3(-0.027024f, 0.208296f, 0.026351f), new Vector3(-0.024298f, 0.218782f, 0.043602f), new Vector3(-0.026027f, 0.215103f, 0.046323f), new Vector3(-0.023174f, 0.220216f, 0.035976f), new Vector3(-0.02432f, 0.215532f, 0.02907f), new Vector3(0.03149001f, 0.202056f, 0.04479101f), new Vector3(0.029873f, 0.206929f, 0.047409f), new Vector3(0.032279f, 0.198817f, 0.03928f), new Vector3(0.031583f, 0.198393f, 0.032406f), new Vector3(0.027024f, 0.208296f, 0.026351f), new Vector3(0.02432f, 0.215532f, 0.02907f), new Vector3(0.026027f, 0.215103f, 0.046323f), new Vector3(0.024298f, 0.218782f, 0.043602f), new Vector3(0.023174f, 0.220216f, 0.035976f) };     //Ears
            meshSeamVerts[1][1][2][3] = new Vector3[] { new Vector3(0f, 0.145662f, 0.05409f), new Vector3(-0.020114f, 0.156391f, 0.043719f), new Vector3(-0.026019f, 0.171842f, 0.030003f), new Vector3(-0.024575f, 0.181631f, 0.021221f), new Vector3(-0.019194f, 0.18951f, 0.014426f), new Vector3(0f, 0.197183f, 0.007898f), new Vector3(-0.010536f, 0.19474f, 0.009992f), new Vector3(0.020114f, 0.156391f, 0.043719f), new Vector3(0.026019f, 0.171842f, 0.030003f), new Vector3(0.024575f, 0.181631f, 0.021221f), new Vector3(0.019194f, 0.18951f, 0.014426f), new Vector3(0.010536f, 0.19474f, 0.009992f), new Vector3(0.010967f, 0.149103f, 0.050557f), new Vector3(-0.010967f, 0.149103f, 0.050557f) };     //Neck
            meshSeamVerts[1][1][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][1][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][1][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[1][1][3] = new Vector3[7][];         //Puppy LOD3 seams
            meshSeamVerts[1][1][3][0] = new Vector3[] { new Vector3(0.033462f, 0.028613f, 0.020549f), new Vector3(0.022458f, 0.028546f, 0.020436f), new Vector3(0.019965f, 0.022352f, 0.004917f), new Vector3(0.035116f, 0.022422f, 0.005462f), new Vector3(-0.022458f, 0.028546f, 0.020436f), new Vector3(-0.033462f, 0.028613f, 0.020549f), new Vector3(-0.035116f, 0.022422f, 0.005462f), new Vector3(-0.019965f, 0.022352f, 0.004917f), new Vector3(0.027483f, 0.020485f, 0.000958f), new Vector3(-0.027483f, 0.020485f, 0.000958f), new Vector3(0.03444f, 0.035381f, -0.164102f), new Vector3(0.027588f, 0.036252f, -0.168875f), new Vector3(0.020287f, 0.035234f, -0.163587f), new Vector3(0.024645f, 0.031387f, -0.147424f), new Vector3(0.031218f, 0.031604f, -0.147082f), new Vector3(-0.020287f, 0.035234f, -0.163587f), new Vector3(-0.024645f, 0.031387f, -0.147424f), new Vector3(-0.031218f, 0.031604f, -0.147082f), new Vector3(-0.03444f, 0.035381f, -0.164102f), new Vector3(-0.027588f, 0.036252f, -0.168875f) };     //Ankles
            meshSeamVerts[1][1][3][1] = new Vector3[] { new Vector3(0f, 0.151951f, -0.1278f), new Vector3(0f, 0.135843f, -0.138939f), new Vector3(0.007824f, 0.147512f, -0.129544f), new Vector3(-0.007824f, 0.147512f, -0.129544f) };     //Tail
            meshSeamVerts[1][1][3][2] = new Vector3[] { new Vector3(-0.030682f, 0.204493f, 0.0461f), new Vector3(-0.031915f, 0.198441f, 0.035844f), new Vector3(-0.027024f, 0.208296f, 0.026351f), new Vector3(-0.024986f, 0.217198f, 0.045089f), new Vector3(-0.023382f, 0.219023f, 0.032543f), new Vector3(0.030682f, 0.204493f, 0.0461f), new Vector3(0.031915f, 0.198441f, 0.035844f), new Vector3(0.027024f, 0.208296f, 0.026351f), new Vector3(0.024986f, 0.217198f, 0.045089f), new Vector3(0.023382f, 0.219023f, 0.032543f) };     //Ears
            meshSeamVerts[1][1][3][3] = new Vector3[] { new Vector3(0f, 0.145662f, 0.05409f), new Vector3(-0.01605f, 0.152601f, 0.047202f), new Vector3(-0.025785f, 0.176954f, 0.025303f), new Vector3(0f, 0.197183f, 0.007898f), new Vector3(-0.0154f, 0.192492f, 0.011914f), new Vector3(0.025785f, 0.176954f, 0.025303f), new Vector3(0.0154f, 0.192492f, 0.011914f), new Vector3(0.01605f, 0.152601f, 0.047202f) };     //Neck
            meshSeamVerts[1][1][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[1][1][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[1][1][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[2] = new Vector3[4][][][];        //ageSpecies
            meshSeamVerts[2][0] = new Vector3[4][][];         //Adult Cat
            meshSeamVerts[2][0][0] = new Vector3[7][];         //Adult Cat LOD0 seams
            meshSeamVerts[2][0][0][0] = new Vector3[] { new Vector3(0.03187f, 0.04709f, 0.04364f), new Vector3(0.0239f, 0.04833f, 0.05513f), new Vector3(0.02666f, 0.05041f, 0.06838f), new Vector3(0.03171f, 0.05114f, 0.07282f), new Vector3(0.04434f, 0.05157f, 0.07285f), new Vector3(0.04968f, 0.05119f, 0.06906f), new Vector3(0.05158f, 0.04802f, 0.04905f), new Vector3(0.04707f, 0.04709001f, 0.04262f), new Vector3(0.03326f, 0.052579f, -0.23745f), new Vector3(0.02857f, 0.052609f, -0.24304f), new Vector3(0.02676f, 0.052669f, -0.25031f), new Vector3(0.02741f, 0.052829f, -0.25657f), new Vector3(0.02854f, 0.052829f, -0.26258f), new Vector3(0.03167f, 0.052849f, -0.26771f), new Vector3(0.04803f, 0.052849f, -0.26868f), new Vector3(0.05164f, 0.052829f, -0.26331f), new Vector3(0.05357f, 0.052829f, -0.25688f), new Vector3(0.05385f, 0.052829f, -0.25001f), new Vector3(0.05187f, 0.052759f, -0.24285f), new Vector3(0.04686f, 0.052689f, -0.23775f), new Vector3(0.04009f, 0.052619f, -0.23539f), new Vector3(0.04026f, 0.052849f, -0.27037f), new Vector3(0.03794f, 0.05174f, 0.07458f), new Vector3(0.03962f, 0.04686f, 0.04151f), new Vector3(0.02381f, 0.04983f, 0.0623f), new Vector3(0.05228f, 0.05042f, 0.0631f), new Vector3(0.05311f, 0.04933f, 0.05638f), new Vector3(0.02697f, 0.04753f, 0.04869f), new Vector3(-0.03187f, 0.04709f, 0.04364f), new Vector3(-0.0239f, 0.04833f, 0.05513f), new Vector3(-0.02666f, 0.05041f, 0.06838f), new Vector3(-0.03171f, 0.05114f, 0.07282f), new Vector3(-0.04434f, 0.05157f, 0.07285f), new Vector3(-0.04968f, 0.05119f, 0.06906f), new Vector3(-0.05158f, 0.04802f, 0.04905f), new Vector3(-0.04707f, 0.04709001f, 0.04262f), new Vector3(-0.03326f, 0.052579f, -0.23745f), new Vector3(-0.02857f, 0.052609f, -0.24304f), new Vector3(-0.02676f, 0.052669f, -0.25031f), new Vector3(-0.02741f, 0.052829f, -0.25657f), new Vector3(-0.02854f, 0.052829f, -0.26258f), new Vector3(-0.03167f, 0.052849f, -0.26771f), new Vector3(-0.04803f, 0.052849f, -0.26868f), new Vector3(-0.05164f, 0.052829f, -0.26331f), new Vector3(-0.05357f, 0.052829f, -0.25688f), new Vector3(-0.05385f, 0.052829f, -0.25001f), new Vector3(-0.05187f, 0.052759f, -0.24285f), new Vector3(-0.04686f, 0.052689f, -0.23775f), new Vector3(-0.04009f, 0.052619f, -0.23539f), new Vector3(-0.04026f, 0.052849f, -0.27037f), new Vector3(-0.03794f, 0.05174f, 0.07458f), new Vector3(-0.03962f, 0.04686f, 0.04151f), new Vector3(-0.02381f, 0.04983f, 0.0623f), new Vector3(-0.05228f, 0.05042f, 0.0631f), new Vector3(-0.05311f, 0.04933f, 0.05638f), new Vector3(-0.02697f, 0.04753f, 0.04869f) };     //Ankles
            meshSeamVerts[2][0][0][1] = new Vector3[] { new Vector3(0.01765f, 0.26168f, -0.23504f), new Vector3(0f, 0.25412f, -0.23691f), new Vector3(0.0238f, 0.27703f, -0.22732f), new Vector3(0.01977f, 0.28779f, -0.22239f), new Vector3(0.01326f, 0.29217f, -0.2198f), new Vector3(0f, 0.29452f, -0.21842f), new Vector3(-0.01326f, 0.29217f, -0.2198f), new Vector3(-0.01977f, 0.28779f, -0.22239f), new Vector3(-0.0238f, 0.27703f, -0.22732f), new Vector3(-0.01765f, 0.26168f, -0.23504f) };     //Tail
            meshSeamVerts[2][0][0][2] = new Vector3[] { new Vector3(-0.01822f, 0.36544f, 0.157701f), new Vector3(-0.0239f, 0.36349f, 0.158141f), new Vector3(-0.05495f, 0.34042f, 0.133701f), new Vector3(-0.02872f, 0.36095f, 0.113401f), new Vector3(-0.01819f, 0.37046f, 0.146851f), new Vector3(-0.04298f, 0.34484f, 0.113721f), new Vector3(-0.03462f, 0.35361f, 0.109461f), new Vector3(-0.05576f, 0.33682f, 0.130501f), new Vector3(-0.05117f, 0.34445f, 0.137631f), new Vector3(-0.04628f, 0.34964f, 0.138691f), new Vector3(-0.03829f, 0.35765f, 0.144011f), new Vector3(-0.03082f, 0.36234f, 0.150591f), new Vector3(-0.02294f, 0.36797f, 0.122981f), new Vector3(-0.02f, 0.37197f, 0.134161f), new Vector3(-0.05104f, 0.33899f, 0.121241f), new Vector3(0.01822f, 0.36544f, 0.157701f), new Vector3(0.0239f, 0.36349f, 0.158141f), new Vector3(0.05495f, 0.34042f, 0.133701f), new Vector3(0.02872f, 0.36095f, 0.113401f), new Vector3(0.01819f, 0.37046f, 0.146851f), new Vector3(0.04298f, 0.34484f, 0.113721f), new Vector3(0.03462f, 0.35361f, 0.109461f), new Vector3(0.05576f, 0.33682f, 0.130501f), new Vector3(0.05117f, 0.34445f, 0.137631f), new Vector3(0.04628f, 0.34964f, 0.138691f), new Vector3(0.03829f, 0.35765f, 0.144011f), new Vector3(0.03082f, 0.36234f, 0.150591f), new Vector3(0.02294f, 0.36797f, 0.122981f), new Vector3(0.02f, 0.37197f, 0.134161f), new Vector3(0.05104f, 0.33899f, 0.121241f) };     //Ears
            meshSeamVerts[2][0][0][3] = new Vector3[] { new Vector3(0f, 0.27073f, 0.134301f), new Vector3(0f, 0.32871f, 0.075801f), new Vector3(0.042f, 0.31052f, 0.093541f), new Vector3(0.04632f, 0.30462f, 0.100251f), new Vector3(0.04845f, 0.29819f, 0.107681f), new Vector3(0.04489f, 0.29179f, 0.114121f), new Vector3(0.03882f, 0.28568f, 0.120121f), new Vector3(0.03178f, 0.28098f, 0.124551f), new Vector3(0.02408f, 0.27728f, 0.127931f), new Vector3(0.0159f, 0.27415f, 0.130781f), new Vector3(0.007960001f, 0.27215f, 0.132871f), new Vector3(0.01074f, 0.3273f, 0.077111f), new Vector3(0.02059f, 0.32455f, 0.079541f), new Vector3(0.02916f, 0.32087f, 0.083081f), new Vector3(0.03623001f, 0.31613f, 0.087741f), new Vector3(-0.042f, 0.31052f, 0.093541f), new Vector3(-0.04632f, 0.30462f, 0.100251f), new Vector3(-0.04845f, 0.29819f, 0.107681f), new Vector3(-0.04489f, 0.29179f, 0.114121f), new Vector3(-0.03882f, 0.28568f, 0.120121f), new Vector3(-0.03178f, 0.28098f, 0.124551f), new Vector3(-0.02408f, 0.27728f, 0.127931f), new Vector3(-0.0159f, 0.27415f, 0.130781f), new Vector3(-0.007960001f, 0.27215f, 0.132871f), new Vector3(-0.01074f, 0.3273f, 0.077111f), new Vector3(-0.02059f, 0.32455f, 0.079541f), new Vector3(-0.02916f, 0.32087f, 0.083081f), new Vector3(-0.03623001f, 0.31613f, 0.087741f) };     //Neck
            meshSeamVerts[2][0][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][0][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][0][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][0][1] = new Vector3[7][];         //Adult Cat LOD1 seams
            meshSeamVerts[2][0][1][0] = new Vector3[] { new Vector3(0.03187f, 0.04709f, 0.04364f), new Vector3(0.02666f, 0.05041f, 0.06838f), new Vector3(0.03171f, 0.05114f, 0.07282f), new Vector3(0.04114f, 0.051655f, 0.073715f), new Vector3(0.04968f, 0.05119f, 0.06906f), new Vector3(0.05158f, 0.04802f, 0.04905f), new Vector3(0.04707f, 0.04709001f, 0.04262f), new Vector3(0.03326f, 0.052579f, -0.23745f), new Vector3(0.02857f, 0.052609f, -0.24304f), new Vector3(0.027085f, 0.052749f, -0.25344f), new Vector3(0.03167f, 0.052849f, -0.26771f), new Vector3(0.049835f, 0.052839f, -0.265995f), new Vector3(0.05371f, 0.052829f, -0.253445f), new Vector3(0.049365f, 0.052724f, -0.2403f), new Vector3(0.04009f, 0.052619f, -0.23539f), new Vector3(0.04026f, 0.052849f, -0.27037f), new Vector3(0.03962f, 0.04686f, 0.04151f), new Vector3(0.023855f, 0.04908f, 0.058715f), new Vector3(0.052695f, 0.049875f, 0.05974f), new Vector3(0.02697f, 0.04753f, 0.04869f), new Vector3(-0.03187f, 0.04709f, 0.04364f), new Vector3(-0.02666f, 0.05041f, 0.06838f), new Vector3(-0.03171f, 0.05114f, 0.07282f), new Vector3(-0.04114f, 0.051655f, 0.073715f), new Vector3(-0.04968f, 0.05119f, 0.06906f), new Vector3(-0.05158f, 0.04802f, 0.04905f), new Vector3(-0.04707f, 0.04709001f, 0.04262f), new Vector3(-0.03326f, 0.052579f, -0.23745f), new Vector3(-0.02857f, 0.052609f, -0.24304f), new Vector3(-0.027085f, 0.052749f, -0.25344f), new Vector3(-0.03167f, 0.052849f, -0.26771f), new Vector3(-0.049835f, 0.052839f, -0.265995f), new Vector3(-0.05371f, 0.052829f, -0.253445f), new Vector3(-0.049365f, 0.052724f, -0.2403f), new Vector3(-0.04009f, 0.052619f, -0.23539f), new Vector3(-0.04026f, 0.052849f, -0.27037f), new Vector3(-0.03962f, 0.04686f, 0.04151f), new Vector3(-0.023855f, 0.04908f, 0.058715f), new Vector3(-0.052695f, 0.049875f, 0.05974f), new Vector3(-0.02697f, 0.04753f, 0.04869f) };     //Ankles
            meshSeamVerts[2][0][1][1] = new Vector3[] { new Vector3(0.01765f, 0.26168f, -0.23504f), new Vector3(0f, 0.25412f, -0.23691f), new Vector3(0.0238f, 0.27703f, -0.22732f), new Vector3(0.016515f, 0.28998f, -0.221095f), new Vector3(0f, 0.29452f, -0.21842f), new Vector3(-0.016515f, 0.28998f, -0.221095f), new Vector3(-0.0238f, 0.27703f, -0.22732f), new Vector3(-0.01765f, 0.26168f, -0.23504f) };     //Tail
            meshSeamVerts[2][0][1][2] = new Vector3[] { new Vector3(0.01822f, 0.36544f, 0.157701f), new Vector3(0.0239f, 0.36349f, 0.158141f), new Vector3(0.05495f, 0.34042f, 0.133701f), new Vector3(0.02583f, 0.36446f, 0.118191f), new Vector3(0.01819f, 0.37046f, 0.146851f), new Vector3(0.03462f, 0.35361f, 0.109461f), new Vector3(0.05576f, 0.33682f, 0.130501f), new Vector3(0.05117f, 0.34445f, 0.137631f), new Vector3(0.04628f, 0.34964f, 0.138691f), new Vector3(0.034555f, 0.359995f, 0.147301f), new Vector3(0.02f, 0.37197f, 0.134161f), new Vector3(0.04701f, 0.341915f, 0.117481f), new Vector3(-0.01822f, 0.36544f, 0.157701f), new Vector3(-0.0239f, 0.36349f, 0.158141f), new Vector3(-0.05495f, 0.34042f, 0.133701f), new Vector3(-0.02583f, 0.36446f, 0.118191f), new Vector3(-0.01819f, 0.37046f, 0.146851f), new Vector3(-0.03462f, 0.35361f, 0.109461f), new Vector3(-0.05576f, 0.33682f, 0.130501f), new Vector3(-0.05117f, 0.34445f, 0.137631f), new Vector3(-0.04628f, 0.34964f, 0.138691f), new Vector3(-0.034555f, 0.359995f, 0.147301f), new Vector3(-0.02f, 0.37197f, 0.134161f), new Vector3(-0.04701f, 0.341915f, 0.117481f) };     //Ears
            meshSeamVerts[2][0][1][3] = new Vector3[] { new Vector3(0f, 0.27073f, 0.134301f), new Vector3(0f, 0.32871f, 0.075801f), new Vector3(0.039115f, 0.313325f, 0.09064101f), new Vector3(0.04632f, 0.30462f, 0.100251f), new Vector3(0.04667f, 0.29499f, 0.110901f), new Vector3(0.03882f, 0.28568f, 0.120121f), new Vector3(0.02793f, 0.27913f, 0.126241f), new Vector3(0.01193f, 0.27315f, 0.131826f), new Vector3(0.015665f, 0.325925f, 0.078326f), new Vector3(0.02916f, 0.32087f, 0.083081f), new Vector3(-0.039115f, 0.313325f, 0.09064101f), new Vector3(-0.04632f, 0.30462f, 0.100251f), new Vector3(-0.04667f, 0.29499f, 0.110901f), new Vector3(-0.03882f, 0.28568f, 0.120121f), new Vector3(-0.02793f, 0.27913f, 0.126241f), new Vector3(-0.01193f, 0.27315f, 0.131826f), new Vector3(-0.015665f, 0.325925f, 0.078326f), new Vector3(-0.02916f, 0.32087f, 0.083081f) };     //Neck
            meshSeamVerts[2][0][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][0][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][0][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][0][2] = new Vector3[7][];         //Adult Cat LOD2 seams
            meshSeamVerts[2][0][2][0] = new Vector3[] { new Vector3(0.03187f, 0.04709f, 0.04364f), new Vector3(0.029185f, 0.050775f, 0.0706f), new Vector3(0.04114f, 0.051655f, 0.073715f), new Vector3(0.04968f, 0.05119f, 0.06906f), new Vector3(0.052137f, 0.048947f, 0.054395f), new Vector3(0.02857f, 0.052609f, -0.24304f), new Vector3(0.027085f, 0.052749f, -0.25344f), new Vector3(0.030105f, 0.052839f, -0.265145f), new Vector3(0.049835f, 0.052839f, -0.265995f), new Vector3(0.05371f, 0.052829f, -0.253445f), new Vector3(0.049365f, 0.052724f, -0.2403f), new Vector3(0.036675f, 0.05259901f, -0.23642f), new Vector3(0.04026f, 0.052849f, -0.27037f), new Vector3(0.043345f, 0.046975f, 0.042065f), new Vector3(0.025412f, 0.048305f, 0.053703f), new Vector3(-0.03187f, 0.04709f, 0.04364f), new Vector3(-0.029185f, 0.050775f, 0.0706f), new Vector3(-0.04114f, 0.051655f, 0.073715f), new Vector3(-0.04968f, 0.05119f, 0.06906f), new Vector3(-0.052137f, 0.048947f, 0.054395f), new Vector3(-0.02857f, 0.052609f, -0.24304f), new Vector3(-0.027085f, 0.052749f, -0.25344f), new Vector3(-0.030105f, 0.052839f, -0.265145f), new Vector3(-0.049835f, 0.052839f, -0.265995f), new Vector3(-0.05371f, 0.052829f, -0.253445f), new Vector3(-0.049365f, 0.052724f, -0.2403f), new Vector3(-0.036675f, 0.05259901f, -0.23642f), new Vector3(-0.04026f, 0.052849f, -0.27037f), new Vector3(-0.043345f, 0.046975f, 0.042065f), new Vector3(-0.025412f, 0.048305f, 0.053703f) };     //Ankles
            meshSeamVerts[2][0][2][1] = new Vector3[] { new Vector3(0.01765f, 0.26168f, -0.23504f), new Vector3(0f, 0.25412f, -0.23691f), new Vector3(0.020157f, 0.283505f, -0.224207f), new Vector3(0f, 0.29452f, -0.21842f), new Vector3(-0.020157f, 0.283505f, -0.224207f), new Vector3(-0.01765f, 0.26168f, -0.23504f) };     //Tail
            meshSeamVerts[2][0][2][2] = new Vector3[] { new Vector3(0.0239f, 0.36349f, 0.158141f), new Vector3(0.05495f, 0.34042f, 0.133701f), new Vector3(0.018205f, 0.36795f, 0.152276f), new Vector3(0.03167f, 0.35728f, 0.111431f), new Vector3(0.05576f, 0.33682f, 0.130501f), new Vector3(0.05117f, 0.34445f, 0.137631f), new Vector3(0.042285f, 0.353645f, 0.141351f), new Vector3(0.02147f, 0.36997f, 0.128571f), new Vector3(0.04701f, 0.341915f, 0.117481f), new Vector3(0.03082f, 0.36234f, 0.150591f), new Vector3(-0.0239f, 0.36349f, 0.158141f), new Vector3(-0.05495f, 0.34042f, 0.133701f), new Vector3(-0.018205f, 0.36795f, 0.152276f), new Vector3(-0.03167f, 0.35728f, 0.111431f), new Vector3(-0.05576f, 0.33682f, 0.130501f), new Vector3(-0.05117f, 0.34445f, 0.137631f), new Vector3(-0.042285f, 0.353645f, 0.141351f), new Vector3(-0.03082f, 0.36234f, 0.150591f), new Vector3(-0.02147f, 0.36997f, 0.128571f), new Vector3(-0.04701f, 0.341915f, 0.117481f) };     //Ears
            meshSeamVerts[2][0][2][3] = new Vector3[] { new Vector3(0f, 0.27073f, 0.134301f), new Vector3(0f, 0.32871f, 0.075801f), new Vector3(0.039115f, 0.313325f, 0.09064101f), new Vector3(0.04632f, 0.30462f, 0.100251f), new Vector3(0.04274501f, 0.290335f, 0.115511f), new Vector3(0.01993f, 0.27614f, 0.129034f), new Vector3(0.022412f, 0.323397f, 0.08070301f), new Vector3(-0.039115f, 0.313325f, 0.09064101f), new Vector3(-0.04632f, 0.30462f, 0.100251f), new Vector3(-0.04274501f, 0.290335f, 0.115511f), new Vector3(-0.01993f, 0.27614f, 0.129034f), new Vector3(-0.022412f, 0.323397f, 0.08070301f) };     //Neck
            meshSeamVerts[2][0][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][0][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][0][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][0][3] = new Vector3[7][];         //Adult Cat LOD3 seams
            meshSeamVerts[2][0][3][0] = new Vector3[] { new Vector3(0.04114f, 0.051655f, 0.073715f), new Vector3(0.04968f, 0.05119f, 0.06906f), new Vector3(0.052137f, 0.048947f, 0.054395f), new Vector3(0.02857f, 0.052609f, -0.24304f), new Vector3(0.027085f, 0.052749f, -0.25344f), new Vector3(0.030105f, 0.052839f, -0.265145f), new Vector3(0.05371f, 0.052829f, -0.253445f), new Vector3(0.04302f, 0.052661f, -0.23836f), new Vector3(0.045053f, 0.052844f, -0.268183f), new Vector3(0.043345f, 0.046975f, 0.042065f), new Vector3(0.028641f, 0.047697f, 0.048672f), new Vector3(-0.04114f, 0.051655f, 0.073715f), new Vector3(-0.04968f, 0.05119f, 0.06906f), new Vector3(-0.052137f, 0.048947f, 0.054395f), new Vector3(-0.02857f, 0.052609f, -0.24304f), new Vector3(-0.027085f, 0.052749f, -0.25344f), new Vector3(-0.030105f, 0.052839f, -0.265145f), new Vector3(-0.05371f, 0.052829f, -0.253445f), new Vector3(-0.04302f, 0.052661f, -0.23836f), new Vector3(-0.045053f, 0.052844f, -0.268183f), new Vector3(-0.043345f, 0.046975f, 0.042065f), new Vector3(-0.028641f, 0.047697f, 0.048672f) };     //Ankles
            meshSeamVerts[2][0][3][1] = new Vector3[] { new Vector3(0.01765f, 0.26168f, -0.23504f), new Vector3(0f, 0.25412f, -0.23691f), new Vector3(0.020157f, 0.283505f, -0.224207f), new Vector3(-0.020157f, 0.283505f, -0.224207f), new Vector3(-0.01765f, 0.26168f, -0.23504f), new Vector3(0f, 0.29452f, -0.21842f) };     //Tail
            meshSeamVerts[2][0][3][2] = new Vector3[] { new Vector3(0.0239f, 0.36349f, 0.158141f), new Vector3(0.05495f, 0.34042f, 0.133701f), new Vector3(0.018205f, 0.36795f, 0.152276f), new Vector3(0.03167f, 0.35728f, 0.111431f), new Vector3(0.036552f, 0.357993f, 0.145971f), new Vector3(0.021477f, 0.369976f, 0.128572f), new Vector3(0.04701f, 0.341915f, 0.117481f), new Vector3(-0.0239f, 0.36349f, 0.158141f), new Vector3(-0.05495f, 0.34042f, 0.133701f), new Vector3(-0.018205f, 0.36795f, 0.152276f), new Vector3(-0.03167f, 0.35728f, 0.111431f), new Vector3(-0.036552f, 0.357993f, 0.145971f), new Vector3(-0.021477f, 0.369976f, 0.128572f), new Vector3(-0.04701f, 0.341915f, 0.117481f) };     //Ears
            meshSeamVerts[2][0][3][3] = new Vector3[] { new Vector3(0f, 0.27073f, 0.134301f), new Vector3(0f, 0.32871f, 0.075801f), new Vector3(0.042717f, 0.308972f, 0.09544601f), new Vector3(0.04274501f, 0.290335f, 0.115511f), new Vector3(0.01993f, 0.27614f, 0.129034f), new Vector3(0.022412f, 0.323397f, 0.08070301f), new Vector3(-0.042717f, 0.308972f, 0.09544601f), new Vector3(-0.04274501f, 0.290335f, 0.115511f), new Vector3(-0.01993f, 0.27614f, 0.129034f), new Vector3(-0.022412f, 0.323397f, 0.08070301f) };     //Neck
            meshSeamVerts[2][0][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][0][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][0][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[2][1] = new Vector3[4][][];         //Kitten
            meshSeamVerts[2][1][0] = new Vector3[7][];         //Kitten LOD0 seams
            meshSeamVerts[2][1][0][0] = new Vector3[] { new Vector3(0.018497f, 0.026379f, 0.002009f), new Vector3(0.012387f, 0.028202f, 0.010199f), new Vector3(0.014981f, 0.030418f, 0.020039f), new Vector3(0.01866f, 0.031186f, 0.023475f), new Vector3(0.027665f, 0.03118f, 0.023433f), new Vector3(0.031687f, 0.030394f, 0.019928f), new Vector3(0.033337f, 0.027245f, 0.00572f), new Vector3(0.030188f, 0.026212f, 0.001113f), new Vector3(0.018861f, 0.029619f, -0.130957f), new Vector3(0.014863f, 0.029804f, -0.135698f), new Vector3(0.013112f, 0.02993f, -0.140976f), new Vector3(0.013336f, 0.030006f, -0.145075f), new Vector3(0.014588f, 0.03012f, -0.149169f), new Vector3(0.017892f, 0.030157f, -0.153162f), new Vector3(0.029672f, 0.030085f, -0.153938f), new Vector3(0.033145f, 0.030043f, -0.150467f), new Vector3(0.034879f, 0.029977f, -0.146234f), new Vector3(0.035254f, 0.02987f, -0.141454f), new Vector3(0.033931f, 0.029635f, -0.134763f), new Vector3(0.030054f, 0.029504f, -0.129609f), new Vector3(0.024389f, 0.029514f, -0.128336f), new Vector3(0.023928f, 0.0301f, -0.154478f), new Vector3(0.023195f, 0.031494f, 0.024832f), new Vector3(0.024602f, 0.02597f, 9.000001E-05f), new Vector3(0.012652f, 0.029413f, 0.015535f), new Vector3(0.033369f, 0.029321f, 0.015074f), new Vector3(0.033974f, 0.028346f, 0.010665f), new Vector3(0.014679f, 0.027247f, 0.005929f), new Vector3(-0.018497f, 0.026379f, 0.002009f), new Vector3(-0.012387f, 0.028202f, 0.010199f), new Vector3(-0.014981f, 0.030418f, 0.020039f), new Vector3(-0.01866f, 0.031186f, 0.023475f), new Vector3(-0.027665f, 0.03118f, 0.023433f), new Vector3(-0.031687f, 0.030394f, 0.019928f), new Vector3(-0.033337f, 0.027245f, 0.00572f), new Vector3(-0.030188f, 0.026212f, 0.001113f), new Vector3(-0.018861f, 0.029619f, -0.130957f), new Vector3(-0.014863f, 0.029804f, -0.135698f), new Vector3(-0.013112f, 0.02993f, -0.140976f), new Vector3(-0.013336f, 0.030006f, -0.145075f), new Vector3(-0.014588f, 0.03012f, -0.149169f), new Vector3(-0.017892f, 0.030157f, -0.153162f), new Vector3(-0.029672f, 0.030085f, -0.153938f), new Vector3(-0.033145f, 0.030043f, -0.150467f), new Vector3(-0.034879f, 0.029977f, -0.146234f), new Vector3(-0.035254f, 0.02987f, -0.141454f), new Vector3(-0.033931f, 0.029635f, -0.134763f), new Vector3(-0.030054f, 0.029504f, -0.129609f), new Vector3(-0.024389f, 0.029514f, -0.128336f), new Vector3(-0.023928f, 0.0301f, -0.154478f), new Vector3(-0.023195f, 0.031494f, 0.024832f), new Vector3(-0.024602f, 0.02597f, 9.000001E-05f), new Vector3(-0.012652f, 0.029413f, 0.015535f), new Vector3(-0.033369f, 0.029321f, 0.015074f), new Vector3(-0.033974f, 0.028346f, 0.010665f), new Vector3(-0.014679f, 0.027247f, 0.005929f) };     //Ankles
            meshSeamVerts[2][1][0][1] = new Vector3[] { new Vector3(0f, 0.149463f, -0.115957f), new Vector3(0f, 0.129288f, -0.128139f), new Vector3(0.010423f, 0.146481f, -0.117671f), new Vector3(0.00681f, 0.14851f, -0.116407f), new Vector3(0.008279f, 0.133257f, -0.127143f), new Vector3(0.012724f, 0.141664f, -0.121088f), new Vector3(-0.010423f, 0.146481f, -0.117671f), new Vector3(-0.00681f, 0.14851f, -0.116407f), new Vector3(-0.008279f, 0.133257f, -0.127143f), new Vector3(-0.012724f, 0.141664f, -0.121088f) };     //Tail
            meshSeamVerts[2][1][0][2] = new Vector3[] { new Vector3(0.013742f, 0.209132f, 0.067178f), new Vector3(0.017547f, 0.207469f, 0.067133f), new Vector3(0.021947f, 0.205727f, 0.062852f), new Vector3(0.027174f, 0.201811f, 0.058545f), new Vector3(0.032633f, 0.197125f, 0.056024f), new Vector3(0.03535f, 0.193716f, 0.056012f), new Vector3(0.037451f, 0.190713f, 0.053592f), new Vector3(0.038267f, 0.187306f, 0.05108f), new Vector3(0.035437f, 0.186991f, 0.04509f), new Vector3(0.029585f, 0.189462f, 0.039499f), new Vector3(0.023855f, 0.193845f, 0.036314f), new Vector3(0.019417f, 0.19881f, 0.038247f), new Vector3(0.015631f, 0.204683f, 0.043522f), new Vector3(0.013788f, 0.208724f, 0.050557f), new Vector3(0.013337f, 0.210111f, 0.059452f), new Vector3(-0.013742f, 0.209132f, 0.067178f), new Vector3(-0.017547f, 0.207469f, 0.067133f), new Vector3(-0.013337f, 0.210111f, 0.059452f), new Vector3(-0.013788f, 0.208724f, 0.050557f), new Vector3(-0.015631f, 0.204683f, 0.043522f), new Vector3(-0.019417f, 0.19881f, 0.038247f), new Vector3(-0.023855f, 0.193845f, 0.036314f), new Vector3(-0.029585f, 0.189462f, 0.039499f), new Vector3(-0.035437f, 0.186991f, 0.04509f), new Vector3(-0.038267f, 0.187306f, 0.05108f), new Vector3(-0.037451f, 0.190713f, 0.053592f), new Vector3(-0.03535f, 0.193716f, 0.056012f), new Vector3(-0.032633f, 0.197125f, 0.056024f), new Vector3(-0.027174f, 0.201811f, 0.058545f), new Vector3(-0.021947f, 0.205727f, 0.062852f) };     //Ears
            meshSeamVerts[2][1][0][3] = new Vector3[] { new Vector3(0f, 0.142284f, 0.059373f), new Vector3(0f, 0.176906f, 0.019761f), new Vector3(0.025839f, 0.16723f, 0.029918f), new Vector3(0.028225f, 0.163564f, 0.034268f), new Vector3(0.029034f, 0.159363f, 0.039308f), new Vector3(0.02783f, 0.155405f, 0.043854f), new Vector3(0.024934f, 0.151424f, 0.048448f), new Vector3(0.020576f, 0.148134f, 0.052331f), new Vector3(0.015405f, 0.145635f, 0.055386f), new Vector3(0.010212f, 0.143867f, 0.057427f), new Vector3(0.004888f, 0.14275f, 0.058746f), new Vector3(0.006122f, 0.176382f, 0.020261f), new Vector3(0.012127f, 0.174964f, 0.021662f), new Vector3(0.017547f, 0.172861f, 0.023772f), new Vector3(0.022247f, 0.170202f, 0.026496f), new Vector3(-0.025839f, 0.16723f, 0.029918f), new Vector3(-0.028225f, 0.163564f, 0.034268f), new Vector3(-0.029034f, 0.159363f, 0.039308f), new Vector3(-0.02783f, 0.155405f, 0.043854f), new Vector3(-0.024934f, 0.151424f, 0.048448f), new Vector3(-0.020576f, 0.148134f, 0.052331f), new Vector3(-0.015405f, 0.145635f, 0.055386f), new Vector3(-0.010212f, 0.143867f, 0.057427f), new Vector3(-0.004888f, 0.14275f, 0.058746f), new Vector3(-0.006122f, 0.176382f, 0.020261f), new Vector3(-0.012127f, 0.174964f, 0.021662f), new Vector3(-0.017547f, 0.172861f, 0.023772f), new Vector3(-0.022247f, 0.170202f, 0.026496f) };     //Neck
            meshSeamVerts[2][1][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][1][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][1][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][1][1] = new Vector3[7][];         //Kitten LOD1 seams
            meshSeamVerts[2][1][1][0] = new Vector3[] { new Vector3(0.018497f, 0.026379f, 0.002009f), new Vector3(0.014981f, 0.030418f, 0.020039f), new Vector3(0.01866f, 0.031186f, 0.023475f), new Vector3(0.02543f, 0.031337f, 0.024132f), new Vector3(0.031687f, 0.030394f, 0.019928f), new Vector3(0.033337f, 0.027245f, 0.00572f), new Vector3(0.030188f, 0.026212f, 0.001113f), new Vector3(0.018861f, 0.029619f, -0.130957f), new Vector3(0.014863f, 0.029804f, -0.135698f), new Vector3(0.013224f, 0.029968f, -0.143025f), new Vector3(0.017892f, 0.030157f, -0.153162f), new Vector3(0.031409f, 0.030064f, -0.152202f), new Vector3(0.035066f, 0.029924f, -0.143844f), new Vector3(0.031992f, 0.029569f, -0.132186f), new Vector3(0.024389f, 0.029514f, -0.128336f), new Vector3(0.023928f, 0.0301f, -0.154478f), new Vector3(0.024602f, 0.02597f, 9.000001E-05f), new Vector3(0.01252f, 0.028808f, 0.012867f), new Vector3(0.033671f, 0.028833f, 0.01287f), new Vector3(0.014679f, 0.027247f, 0.005929f), new Vector3(-0.018497f, 0.026379f, 0.002009f), new Vector3(-0.014981f, 0.030418f, 0.020039f), new Vector3(-0.01866f, 0.031186f, 0.023475f), new Vector3(-0.02543f, 0.031337f, 0.024132f), new Vector3(-0.031687f, 0.030394f, 0.019928f), new Vector3(-0.033337f, 0.027245f, 0.00572f), new Vector3(-0.030188f, 0.026212f, 0.001113f), new Vector3(-0.018861f, 0.029619f, -0.130957f), new Vector3(-0.014863f, 0.029804f, -0.135698f), new Vector3(-0.013224f, 0.029968f, -0.143025f), new Vector3(-0.017892f, 0.030157f, -0.153162f), new Vector3(-0.031409f, 0.030064f, -0.152202f), new Vector3(-0.035066f, 0.029924f, -0.143844f), new Vector3(-0.031992f, 0.029569f, -0.132186f), new Vector3(-0.024389f, 0.029514f, -0.128336f), new Vector3(-0.023928f, 0.0301f, -0.154478f), new Vector3(-0.024602f, 0.02597f, 9.000001E-05f), new Vector3(-0.01252f, 0.028808f, 0.012867f), new Vector3(-0.033671f, 0.028833f, 0.01287f), new Vector3(-0.014679f, 0.027247f, 0.005929f) };     //Ankles
            meshSeamVerts[2][1][1][1] = new Vector3[] { new Vector3(0f, 0.149463f, -0.115957f), new Vector3(0f, 0.129288f, -0.128139f), new Vector3(0.008616f, 0.147496f, -0.117039f), new Vector3(0.008279f, 0.133257f, -0.127143f), new Vector3(0.012724f, 0.141664f, -0.121088f), new Vector3(-0.008616f, 0.147496f, -0.117039f), new Vector3(-0.008279f, 0.133257f, -0.127143f), new Vector3(-0.012724f, 0.141664f, -0.121088f) };     //Tail
            meshSeamVerts[2][1][1][2] = new Vector3[] { new Vector3(0.013742f, 0.209132f, 0.067178f), new Vector3(0.017547f, 0.207469f, 0.067133f), new Vector3(0.024561f, 0.203769f, 0.060699f), new Vector3(0.032633f, 0.197125f, 0.056024f), new Vector3(0.03535f, 0.193716f, 0.056012f), new Vector3(0.037451f, 0.190713f, 0.053592f), new Vector3(0.038267f, 0.187306f, 0.05108f), new Vector3(0.032511f, 0.188226f, 0.042294f), new Vector3(0.023855f, 0.193845f, 0.036314f), new Vector3(0.017524f, 0.201747f, 0.040885f), new Vector3(0.013788f, 0.208724f, 0.050557f), new Vector3(0.013337f, 0.210111f, 0.059452f), new Vector3(-0.013742f, 0.209132f, 0.067178f), new Vector3(-0.017547f, 0.207469f, 0.067133f), new Vector3(-0.024561f, 0.203769f, 0.060699f), new Vector3(-0.032633f, 0.197125f, 0.056024f), new Vector3(-0.03535f, 0.193716f, 0.056012f), new Vector3(-0.037451f, 0.190713f, 0.053592f), new Vector3(-0.038267f, 0.187306f, 0.05108f), new Vector3(-0.032511f, 0.188226f, 0.042294f), new Vector3(-0.023855f, 0.193845f, 0.036314f), new Vector3(-0.017524f, 0.201747f, 0.040885f), new Vector3(-0.013788f, 0.208724f, 0.050557f), new Vector3(-0.013337f, 0.210111f, 0.059452f) };     //Ears
            meshSeamVerts[2][1][1][3] = new Vector3[] { new Vector3(0f, 0.142284f, 0.059373f), new Vector3(0f, 0.176906f, 0.019761f), new Vector3(0.024043f, 0.168716f, 0.028207f), new Vector3(0.028225f, 0.163564f, 0.034268f), new Vector3(0.028432f, 0.157384f, 0.041581f), new Vector3(0.024934f, 0.151424f, 0.048448f), new Vector3(0.01799f, 0.146885f, 0.053859f), new Vector3(0.00755f, 0.143309f, 0.058087f), new Vector3(0.009124f, 0.175673f, 0.020962f), new Vector3(0.017547f, 0.172861f, 0.023772f), new Vector3(-0.024043f, 0.168716f, 0.028207f), new Vector3(-0.028225f, 0.163564f, 0.034268f), new Vector3(-0.028432f, 0.157384f, 0.041581f), new Vector3(-0.024934f, 0.151424f, 0.048448f), new Vector3(-0.01799f, 0.146885f, 0.053859f), new Vector3(-0.00755f, 0.143309f, 0.058087f), new Vector3(-0.009124f, 0.175673f, 0.020962f), new Vector3(-0.017547f, 0.172861f, 0.023772f) };     //Neck
            meshSeamVerts[2][1][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][1][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][1][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][1][2] = new Vector3[7][];         //Kitten LOD2 seams
            meshSeamVerts[2][1][2][0] = new Vector3[] { new Vector3(0.018497f, 0.026379f, 0.002009f), new Vector3(0.01682f, 0.030802f, 0.021757f), new Vector3(0.02543f, 0.031337f, 0.024132f), new Vector3(0.031687f, 0.030394f, 0.019928f), new Vector3(0.033811f, 0.028065f, 0.009403f), new Vector3(0.014863f, 0.029804f, -0.135698f), new Vector3(0.013224f, 0.029968f, -0.143025f), new Vector3(0.01624f, 0.030138f, -0.151166f), new Vector3(0.031409f, 0.030064f, -0.152202f), new Vector3(0.035066f, 0.029924f, -0.143844f), new Vector3(0.031992f, 0.029569f, -0.132186f), new Vector3(0.021625f, 0.029566f, -0.129647f), new Vector3(0.023928f, 0.0301f, -0.154478f), new Vector3(0.027395f, 0.026091f, 0.000601f), new Vector3(0.012889f, 0.027994f, 0.009263f), new Vector3(-0.018497f, 0.026379f, 0.002009f), new Vector3(-0.01682f, 0.030802f, 0.021757f), new Vector3(-0.02543f, 0.031337f, 0.024132f), new Vector3(-0.031687f, 0.030394f, 0.019928f), new Vector3(-0.033811f, 0.028065f, 0.009403f), new Vector3(-0.014863f, 0.029804f, -0.135698f), new Vector3(-0.013224f, 0.029968f, -0.143025f), new Vector3(-0.01624f, 0.030138f, -0.151166f), new Vector3(-0.031409f, 0.030064f, -0.152202f), new Vector3(-0.035066f, 0.029924f, -0.143844f), new Vector3(-0.031992f, 0.029569f, -0.132186f), new Vector3(-0.021625f, 0.029566f, -0.129647f), new Vector3(-0.023928f, 0.0301f, -0.154478f), new Vector3(-0.027395f, 0.026091f, 0.000601f), new Vector3(-0.012889f, 0.027994f, 0.009263f) };     //Ankles
            meshSeamVerts[2][1][2][1] = new Vector3[] { new Vector3(0f, 0.149463f, -0.115957f), new Vector3(0f, 0.129288f, -0.128139f), new Vector3(0.011052f, 0.144775f, -0.119061f), new Vector3(0.008279f, 0.133257f, -0.127143f), new Vector3(-0.011052f, 0.144775f, -0.119061f), new Vector3(-0.008279f, 0.133257f, -0.127143f) };     //Tail
            meshSeamVerts[2][1][2][2] = new Vector3[] { new Vector3(0.017547f, 0.207469f, 0.067133f), new Vector3(0.037451f, 0.190713f, 0.053592f), new Vector3(0.013539f, 0.209622f, 0.063315f), new Vector3(0.021636f, 0.196327f, 0.03728f), new Vector3(0.038267f, 0.187306f, 0.05108f), new Vector3(0.03535f, 0.193716f, 0.056012f), new Vector3(0.029904f, 0.199468f, 0.057284f), new Vector3(0.021947f, 0.205727f, 0.06285201f), new Vector3(0.01471f, 0.206704f, 0.047039f), new Vector3(0.032511f, 0.188226f, 0.042294f), new Vector3(-0.017547f, 0.207469f, 0.067133f), new Vector3(-0.037451f, 0.190713f, 0.053592f), new Vector3(-0.013539f, 0.209622f, 0.063315f), new Vector3(-0.021636f, 0.196327f, 0.03728f), new Vector3(-0.038267f, 0.187306f, 0.05108f), new Vector3(-0.03535f, 0.193716f, 0.056012f), new Vector3(-0.029904f, 0.199468f, 0.057284f), new Vector3(-0.021947f, 0.205727f, 0.06285201f), new Vector3(-0.01471f, 0.206704f, 0.047039f), new Vector3(-0.032511f, 0.188226f, 0.042294f) };     //Ears
            meshSeamVerts[2][1][2][3] = new Vector3[] { new Vector3(0f, 0.142284f, 0.059373f), new Vector3(0f, 0.176906f, 0.019761f), new Vector3(0.024043f, 0.168716f, 0.028207f), new Vector3(0.028225f, 0.163564f, 0.034268f), new Vector3(0.027186f, 0.154554f, 0.045188f), new Vector3(0.01322f, 0.144772f, 0.056121f), new Vector3(0.013405f, 0.174547f, 0.022202f), new Vector3(-0.024043f, 0.168716f, 0.028207f), new Vector3(-0.028225f, 0.163564f, 0.034268f), new Vector3(-0.027186f, 0.154554f, 0.045188f), new Vector3(-0.01322f, 0.144772f, 0.056121f), new Vector3(-0.013405f, 0.174547f, 0.022202f) };     //Neck
            meshSeamVerts[2][1][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][1][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][1][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[2][1][3] = new Vector3[7][];         //Kitten LOD3 seams
            meshSeamVerts[2][1][3][0] = new Vector3[] { new Vector3(0.01483f, 0.03008f, 0.019832f), new Vector3(0.02543f, 0.031337f, 0.024132f), new Vector3(0.031687f, 0.030394f, 0.019928f), new Vector3(0.033811f, 0.028065f, 0.009403f), new Vector3(0.014863f, 0.029804f, -0.135698f), new Vector3(0.013224f, 0.029968f, -0.143025f), new Vector3(0.01624f, 0.030138f, -0.151166f), new Vector3(0.035066f, 0.029924f, -0.143844f), new Vector3(0.027232f, 0.029509f, -0.128975f), new Vector3(0.028032f, 0.030089f, -0.154092f), new Vector3(0.027395f, 0.026091f, 0.000601f), new Vector3(0.018497f, 0.026379f, 0.002009f), new Vector3(-0.01483f, 0.03008f, 0.019832f), new Vector3(-0.02543f, 0.031337f, 0.024132f), new Vector3(-0.031687f, 0.030394f, 0.019928f), new Vector3(-0.033811f, 0.028065f, 0.009403f), new Vector3(-0.014863f, 0.029804f, -0.135698f), new Vector3(-0.013224f, 0.029968f, -0.143025f), new Vector3(-0.01624f, 0.030138f, -0.151166f), new Vector3(-0.035066f, 0.029924f, -0.143844f), new Vector3(-0.027232f, 0.029509f, -0.128975f), new Vector3(-0.028032f, 0.030089f, -0.154092f), new Vector3(-0.027395f, 0.026091f, 0.000601f), new Vector3(-0.018497f, 0.026379f, 0.002009f) };     //Ankles
            meshSeamVerts[2][1][3][1] = new Vector3[] { new Vector3(0f, 0.149463f, -0.115957f), new Vector3(0f, 0.129288f, -0.128139f), new Vector3(0.011052f, 0.144775f, -0.119061f), new Vector3(0.008279001f, 0.133257f, -0.127143f), new Vector3(-0.011052f, 0.144775f, -0.119061f), new Vector3(-0.008279001f, 0.133257f, -0.127143f) };     //Tail
            meshSeamVerts[2][1][3][2] = new Vector3[] { new Vector3(0.017547f, 0.207469f, 0.067133f), new Vector3(0.037451f, 0.190713f, 0.053592f), new Vector3(0.013539f, 0.209622f, 0.063315f), new Vector3(0.021636f, 0.196327f, 0.03728f), new Vector3(0.025944f, 0.202331f, 0.059947f), new Vector3(0.01471f, 0.206704f, 0.047039f), new Vector3(0.032511f, 0.188226f, 0.042294f), new Vector3(-0.017547f, 0.207469f, 0.067133f), new Vector3(-0.037451f, 0.190713f, 0.053592f), new Vector3(-0.013539f, 0.209622f, 0.063315f), new Vector3(-0.021636f, 0.196327f, 0.03728f), new Vector3(-0.025944f, 0.202331f, 0.059947f), new Vector3(-0.01471f, 0.206704f, 0.047039f), new Vector3(-0.032511f, 0.188226f, 0.042294f) };     //Ears
            meshSeamVerts[2][1][3][3] = new Vector3[] { new Vector3(0f, 0.142284f, 0.059373f), new Vector3(0f, 0.176906f, 0.019761f), new Vector3(0.026663f, 0.166916f, 0.031727f), new Vector3(0.027186f, 0.154554f, 0.045188f), new Vector3(0.01322f, 0.144773f, 0.056121f), new Vector3(0.013405f, 0.174547f, 0.022202f), new Vector3(-0.026663f, 0.166916f, 0.031727f), new Vector3(-0.027186f, 0.154554f, 0.045188f), new Vector3(-0.01322f, 0.144773f, 0.056121f), new Vector3(-0.013405f, 0.174547f, 0.022202f) };     //Neck
            meshSeamVerts[2][1][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[2][1][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[2][1][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[3] = new Vector3[4][][][];        //ageSpecies
            meshSeamVerts[3][0] = new Vector3[4][][];         //Adult LittleDog
            meshSeamVerts[3][0][0] = new Vector3[7][];         //Adult LittleDog LOD0 seams
            meshSeamVerts[3][0][0][0] = new Vector3[] { new Vector3(0.053266f, 0.05217f, 0.048082f), new Vector3(0.052265f, 0.052938f, 0.05303301f), new Vector3(0.04917501f, 0.053503f, 0.056396f), new Vector3(0.043012f, 0.053388f, 0.058182f), new Vector3(0.036192f, 0.052977f, 0.056148f), new Vector3(0.032485f, 0.052619f, 0.052324f), new Vector3(0.031167f, 0.052014f, 0.047213f), new Vector3(0.031019f, 0.050789f, 0.042328f), new Vector3(0.033381f, 0.049271f, 0.037794f), new Vector3(0.038213f, 0.047961f, 0.03346501f), new Vector3(0.046057f, 0.047652f, 0.033513f), new Vector3(0.05128f, 0.049458f, 0.038353f), new Vector3(0.053238f, 0.050979f, 0.04322f), new Vector3(-0.031168f, 0.052014f, 0.047213f), new Vector3(-0.032486f, 0.052619f, 0.052324f), new Vector3(-0.036193f, 0.052977f, 0.056148f), new Vector3(-0.043012f, 0.053388f, 0.058182f), new Vector3(-0.04917501f, 0.053503f, 0.056396f), new Vector3(-0.052266f, 0.052938f, 0.05303301f), new Vector3(-0.053266f, 0.05217f, 0.048082f), new Vector3(-0.053238f, 0.050979f, 0.04322f), new Vector3(-0.051281f, 0.049458f, 0.038353f), new Vector3(-0.046057f, 0.047652f, 0.033513f), new Vector3(-0.038213f, 0.047961f, 0.03346501f), new Vector3(-0.033382f, 0.049271f, 0.037794f), new Vector3(-0.031019f, 0.050789f, 0.042328f), new Vector3(0.042106f, 0.047806f, 0.032653f), new Vector3(-0.042107f, 0.047806f, 0.032653f), new Vector3(0.043172f, 0.04736301f, -0.243378f), new Vector3(0.043527f, 0.047413f, -0.248714f), new Vector3(0.042082f, 0.047456f, -0.253793f), new Vector3(0.039837f, 0.0475f, -0.257573f), new Vector3(0.033562f, 0.047528f, -0.258435f), new Vector3(0.026935f, 0.047482f, -0.256842f), new Vector3(0.024721f, 0.047435f, -0.253338f), new Vector3(0.023628f, 0.047404f, -0.248632f), new Vector3(0.023723f, 0.047364f, -0.243251f), new Vector3(0.025867f, 0.04733f, -0.237971f), new Vector3(0.02946f, 0.047351f, -0.235291f), new Vector3(0.033971f, 0.047386f, -0.234519f), new Vector3(0.038166f, 0.047399f, -0.234816f), new Vector3(0.041217f, 0.047377f, -0.238406f), new Vector3(-0.026935f, 0.047482f, -0.256842f), new Vector3(-0.024721f, 0.047435f, -0.253338f), new Vector3(-0.023628f, 0.047404f, -0.248632f), new Vector3(-0.023723f, 0.047364f, -0.243251f), new Vector3(-0.025867f, 0.04733f, -0.237971f), new Vector3(-0.02946f, 0.047351f, -0.235291f), new Vector3(-0.033971f, 0.047386f, -0.234519f), new Vector3(-0.038166f, 0.047399f, -0.234816f), new Vector3(-0.041217f, 0.047377f, -0.238406f), new Vector3(-0.043172f, 0.04736301f, -0.243378f), new Vector3(-0.043527f, 0.047413f, -0.248714f), new Vector3(-0.042082f, 0.047456f, -0.253793f), new Vector3(-0.039837f, 0.0475f, -0.257573f), new Vector3(-0.033562f, 0.047528f, -0.258435f) };     //Ankles
            meshSeamVerts[3][0][0][1] = new Vector3[] { new Vector3(0.01081f, 0.278458f, -0.210725f), new Vector3(0.010701f, 0.286293f, -0.205004f), new Vector3(0.006487f, 0.290322f, -0.203094f), new Vector3(0f, 0.292468f, -0.202558f), new Vector3(0f, 0.270991f, -0.21788f), new Vector3(0.007394f, 0.27454f, -0.214627f), new Vector3(-0.01081f, 0.278458f, -0.210725f), new Vector3(-0.010701f, 0.286293f, -0.205005f), new Vector3(-0.006487f, 0.290322f, -0.203094f), new Vector3(-0.007394f, 0.27454f, -0.214627f) };     //Tail
            meshSeamVerts[3][0][0][2] = new Vector3[] { new Vector3(-0.037003f, 0.357684f, 0.114917f), new Vector3(-0.035077f, 0.363509f, 0.118049f), new Vector3(-0.037946f, 0.35381f, 0.108325f), new Vector3(-0.037114f, 0.353302f, 0.100102f), new Vector3(-0.034529f, 0.358795f, 0.09375401f), new Vector3(-0.032545f, 0.364857f, 0.093388f), new Vector3(-0.029207f, 0.374027f, 0.097527f), new Vector3(-0.032384f, 0.370868f, 0.117098f), new Vector3(-0.02986f, 0.376394f, 0.113495f), new Vector3(-0.037995f, 0.355409f, 0.112079f), new Vector3(-0.033635f, 0.36704f, 0.118419f), new Vector3(-0.031054f, 0.373833f, 0.11554f), new Vector3(-0.028377f, 0.378899f, 0.107687f), new Vector3(-0.028461f, 0.376499f, 0.101057f), new Vector3(-0.030227f, 0.370931f, 0.095279f), new Vector3(-0.03758f, 0.353438f, 0.104691f), new Vector3(0.037003f, 0.357684f, 0.114917f), new Vector3(0.035077f, 0.363509f, 0.118049f), new Vector3(0.037946f, 0.35381f, 0.108325f), new Vector3(0.037114f, 0.353302f, 0.100102f), new Vector3(0.034529f, 0.358795f, 0.093755f), new Vector3(0.032545f, 0.364857f, 0.093388f), new Vector3(0.029207f, 0.374027f, 0.097528f), new Vector3(0.032383f, 0.370868f, 0.117098f), new Vector3(0.02986f, 0.376394f, 0.113495f), new Vector3(0.037995f, 0.355409f, 0.11208f), new Vector3(0.033635f, 0.36704f, 0.118419f), new Vector3(0.031054f, 0.373833f, 0.11554f), new Vector3(0.028377f, 0.378899f, 0.107688f), new Vector3(0.028461f, 0.376499f, 0.101057f), new Vector3(0.030227f, 0.370931f, 0.09528001f), new Vector3(0.03758f, 0.353438f, 0.104691f) };     //Ears
            meshSeamVerts[3][0][0][3] = new Vector3[] { new Vector3(0f, 0.295948f, 0.115474f), new Vector3(-0.017193f, 0.303061f, 0.109489f), new Vector3(-0.022039f, 0.307239f, 0.105983f), new Vector3(-0.030306f, 0.322173f, 0.093075f), new Vector3(-0.029397f, 0.332197f, 0.084304f), new Vector3(-0.022961f, 0.341648f, 0.075985f), new Vector3(-0.018291f, 0.345885f, 0.07224901f), new Vector3(-0.006312001f, 0.350373f, 0.068202f), new Vector3(0f, 0.351544f, 0.067248f), new Vector3(-0.006851f, 0.297333f, 0.114314f), new Vector3(-0.028882f, 0.316425f, 0.098026f), new Vector3(-0.012603f, 0.34862f, 0.069776f), new Vector3(-0.02631f, 0.336917f, 0.080184f), new Vector3(0.017193f, 0.303061f, 0.109489f), new Vector3(0.022039f, 0.307239f, 0.105983f), new Vector3(0.030306f, 0.322173f, 0.093075f), new Vector3(0.029397f, 0.332197f, 0.084304f), new Vector3(0.022961f, 0.341648f, 0.075985f), new Vector3(0.018291f, 0.345885f, 0.07224901f), new Vector3(0.006312001f, 0.350373f, 0.068202f), new Vector3(0.006851f, 0.297333f, 0.114314f), new Vector3(0.028882f, 0.316425f, 0.098026f), new Vector3(0.012603f, 0.34862f, 0.069776f), new Vector3(0.02631f, 0.336917f, 0.080184f), new Vector3(0.03084f, 0.3272f, 0.088763f), new Vector3(0.025984f, 0.311629f, 0.1024f), new Vector3(0.012586f, 0.299943f, 0.112173f), new Vector3(-0.03084f, 0.3272f, 0.088763f), new Vector3(-0.025984f, 0.311629f, 0.1024f), new Vector3(-0.012586f, 0.299943f, 0.112173f) };     //Neck
            meshSeamVerts[3][0][0][4] = new Vector3[0];     //Waist
            meshSeamVerts[3][0][0][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[3][0][0][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[3][0][1] = new Vector3[7][];         //Adult LittleDog LOD1 seams
            meshSeamVerts[3][0][1][0] = new Vector3[] { new Vector3(0.052266f, 0.052938f, 0.053031f), new Vector3(0.04917501f, 0.053503f, 0.056394f), new Vector3(0.043012f, 0.05338901f, 0.058181f), new Vector3(0.036192f, 0.052977f, 0.056146f), new Vector3(0.032486f, 0.05262f, 0.052323f), new Vector3(0.031093f, 0.051402f, 0.044769f), new Vector3(0.033381f, 0.049271f, 0.037793f), new Vector3(0.038213f, 0.047962f, 0.033463f), new Vector3(0.046057f, 0.047652f, 0.033511f), new Vector3(0.05128f, 0.049459f, 0.038351f), new Vector3(0.053252f, 0.051575f, 0.045649f), new Vector3(-0.032486f, 0.05262f, 0.052323f), new Vector3(-0.036192f, 0.052977f, 0.056146f), new Vector3(-0.043012f, 0.05338901f, 0.058181f), new Vector3(-0.04917501f, 0.053503f, 0.056394f), new Vector3(-0.052266f, 0.052938f, 0.053031f), new Vector3(-0.053252f, 0.051575f, 0.045649f), new Vector3(-0.05128f, 0.049459f, 0.038351f), new Vector3(-0.046057f, 0.047652f, 0.033511f), new Vector3(-0.038213f, 0.047962f, 0.033463f), new Vector3(-0.033381f, 0.049271f, 0.037793f), new Vector3(-0.031093f, 0.051402f, 0.044769f), new Vector3(0.042107f, 0.047807f, 0.032652f), new Vector3(-0.042107f, 0.047807f, 0.032652f), new Vector3(0.041092f, 0.051627f, -0.237833f), new Vector3(0.043291f, 0.052228f, -0.245455f), new Vector3(0.041029f, 0.052701f, -0.255115f), new Vector3(0.033536f, 0.052772f, -0.258351f), new Vector3(0.025744f, 0.052608f, -0.254548f), new Vector3(0.023714f, 0.052139f, -0.24501f), new Vector3(0.026088f, 0.051489f, -0.237482f), new Vector3(0.02968f, 0.051407f, -0.234561f), new Vector3(0.03399801f, 0.051474f, -0.233948f), new Vector3(0.037926f, 0.051507f, -0.234069f), new Vector3(-0.025744f, 0.052608f, -0.254548f), new Vector3(-0.023714f, 0.052139f, -0.24501f), new Vector3(-0.026088f, 0.051489f, -0.237482f), new Vector3(-0.02968f, 0.051407f, -0.234561f), new Vector3(-0.03399801f, 0.051474f, -0.233948f), new Vector3(-0.037926f, 0.051507f, -0.234069f), new Vector3(-0.041029f, 0.052701f, -0.255115f), new Vector3(-0.033536f, 0.052772f, -0.258351f), new Vector3(-0.041092f, 0.051627f, -0.237833f), new Vector3(-0.043291f, 0.052228f, -0.245455f) };     //Ankles
            meshSeamVerts[3][0][1][1] = new Vector3[] { new Vector3(0.01081f, 0.278458f, -0.210725f), new Vector3(0.008594f, 0.288307f, -0.204049f), new Vector3(0f, 0.292468f, -0.202558f), new Vector3(0f, 0.270991f, -0.21788f), new Vector3(0.007394f, 0.27454f, -0.214627f), new Vector3(-0.01081f, 0.278458f, -0.210725f), new Vector3(-0.008594f, 0.288307f, -0.20405f), new Vector3(-0.007394f, 0.27454f, -0.214627f) };     //Tail
            meshSeamVerts[3][0][1][2] = new Vector3[] { new Vector3(-0.037003f, 0.357684f, 0.114915f), new Vector3(-0.037114f, 0.353302f, 0.1001f), new Vector3(-0.034529f, 0.358794f, 0.093752f), new Vector3(-0.032545f, 0.364857f, 0.09338601f), new Vector3(-0.029207f, 0.374026f, 0.097525f), new Vector3(-0.032383f, 0.370868f, 0.117096f), new Vector3(-0.02986f, 0.376394f, 0.113493f), new Vector3(-0.03797f, 0.354609f, 0.1102f), new Vector3(-0.033635f, 0.36704f, 0.118417f), new Vector3(-0.031054f, 0.373832f, 0.115538f), new Vector3(-0.028419f, 0.377698f, 0.10437f), new Vector3(-0.030226f, 0.37093f, 0.095277f), new Vector3(-0.03758f, 0.353438f, 0.104689f), new Vector3(0.037003f, 0.357684f, 0.114915f), new Vector3(0.03797f, 0.354609f, 0.1102f), new Vector3(0.037114f, 0.353302f, 0.1001f), new Vector3(0.034529f, 0.358794f, 0.093753f), new Vector3(0.032545f, 0.364857f, 0.09338601f), new Vector3(0.029207f, 0.374026f, 0.097526f), new Vector3(0.032383f, 0.370868f, 0.117096f), new Vector3(0.02986f, 0.376394f, 0.113493f), new Vector3(0.033635f, 0.36704f, 0.118417f), new Vector3(0.031054f, 0.373832f, 0.115538f), new Vector3(0.028419f, 0.377698f, 0.10437f), new Vector3(0.030226f, 0.37093f, 0.09527801f), new Vector3(0.03758f, 0.353438f, 0.104689f) };     //Ears
            meshSeamVerts[3][0][1][3] = new Vector3[] { new Vector3(0f, 0.295948f, 0.115472f), new Vector3(-0.017193f, 0.303061f, 0.109487f), new Vector3(-0.029397f, 0.332197f, 0.08430301f), new Vector3(-0.024635f, 0.339282f, 0.078083f), new Vector3(-0.018291f, 0.345885f, 0.072248f), new Vector3(-0.009458f, 0.349496f, 0.06898801f), new Vector3(0f, 0.351544f, 0.067247f), new Vector3(-0.028882f, 0.316425f, 0.098024f), new Vector3(0.017193f, 0.303061f, 0.109487f), new Vector3(0.024012f, 0.309434f, 0.10419f), new Vector3(0.030573f, 0.324687f, 0.09091701f), new Vector3(0.029397f, 0.332197f, 0.08430301f), new Vector3(0.018291f, 0.345885f, 0.072248f), new Vector3(0.009719001f, 0.298638f, 0.113242f), new Vector3(0.028882f, 0.316425f, 0.098024f), new Vector3(0.009458f, 0.349496f, 0.06898801f), new Vector3(0.024635f, 0.339282f, 0.078083f), new Vector3(-0.030573f, 0.324687f, 0.09091701f), new Vector3(-0.024012f, 0.309434f, 0.10419f), new Vector3(-0.009719001f, 0.298638f, 0.113242f) };     //Neck
            meshSeamVerts[3][0][1][4] = new Vector3[0];     //Waist
            meshSeamVerts[3][0][1][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[3][0][1][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[3][0][2] = new Vector3[7][];         //Adult LittleDog LOD2 seams
            meshSeamVerts[3][0][2][0] = new Vector3[] { new Vector3(0.053266f, 0.05217f, 0.04808f), new Vector3(0.049175f, 0.053503f, 0.056394f), new Vector3(0.036192f, 0.052977f, 0.056146f), new Vector3(0.031168f, 0.052014f, 0.047211f), new Vector3(0.033381f, 0.049271f, 0.037793f), new Vector3(0.05128f, 0.049459f, 0.038351f), new Vector3(-0.031168f, 0.052014f, 0.047211f), new Vector3(-0.036192f, 0.052977f, 0.056146f), new Vector3(-0.049175f, 0.053503f, 0.056394f), new Vector3(-0.053266f, 0.05217f, 0.04808f), new Vector3(-0.05128f, 0.049459f, 0.038351f), new Vector3(-0.033381f, 0.049271f, 0.037793f), new Vector3(0.042107f, 0.047807f, 0.032652f), new Vector3(-0.042107f, 0.047807f, 0.032652f), new Vector3(0.043424f, 0.066587f, -0.240959f), new Vector3(0.042368f, 0.068555f, -0.252335f), new Vector3(0.03344f, 0.069764f, -0.258f), new Vector3(0.023818f, 0.06823601f, -0.25111f), new Vector3(0.023287f, 0.066453f, -0.240506f), new Vector3(0.029835f, 0.065184f, -0.231667f), new Vector3(0.038287f, 0.065391f, -0.231218f), new Vector3(-0.023818f, 0.06823601f, -0.25111f), new Vector3(-0.023287f, 0.066453f, -0.240506f), new Vector3(-0.029835f, 0.065184f, -0.231667f), new Vector3(-0.038287f, 0.065391f, -0.231218f), new Vector3(-0.043424f, 0.066587f, -0.240959f), new Vector3(-0.042368f, 0.068555f, -0.252335f), new Vector3(-0.03344f, 0.069764f, -0.258f) };     //Ankles
            meshSeamVerts[3][0][2][1] = new Vector3[] { new Vector3(0f, 0.292468f, -0.202558f), new Vector3(0f, 0.270991f, -0.21788f), new Vector3(0.009682f, 0.287267f, -0.204543f), new Vector3(-0.009682f, 0.287267f, -0.204543f) };     //Tail
            meshSeamVerts[3][0][2][2] = new Vector3[] { new Vector3(-0.037003f, 0.357684f, 0.114915f), new Vector3(-0.035077f, 0.363509f, 0.118047f), new Vector3(-0.037946f, 0.353809f, 0.108323f), new Vector3(-0.037114f, 0.353302f, 0.1001f), new Vector3(-0.032545f, 0.364857f, 0.093386f), new Vector3(-0.02986f, 0.376394f, 0.113493f), new Vector3(-0.031718f, 0.37235f, 0.116317f), new Vector3(-0.028419f, 0.377698f, 0.10437f), new Vector3(-0.029717f, 0.372478f, 0.096401f), new Vector3(0.037003f, 0.357684f, 0.114915f), new Vector3(0.035077f, 0.363509f, 0.118047f), new Vector3(0.037946f, 0.353809f, 0.108323f), new Vector3(0.037114f, 0.353302f, 0.1001f), new Vector3(0.032545f, 0.364857f, 0.093386f), new Vector3(0.029717f, 0.372478f, 0.096402f), new Vector3(0.031718f, 0.37235f, 0.116317f), new Vector3(0.02986f, 0.376394f, 0.113493f), new Vector3(0.028419f, 0.377698f, 0.10437f) };     //Ears
            meshSeamVerts[3][0][2][3] = new Vector3[] { new Vector3(0f, 0.295948f, 0.115472f), new Vector3(-0.022039f, 0.307239f, 0.105982f), new Vector3(-0.030306f, 0.322173f, 0.093073f), new Vector3(-0.029397f, 0.332197f, 0.08430301f), new Vector3(-0.022961f, 0.341648f, 0.075984f), new Vector3(0f, 0.351544f, 0.067247f), new Vector3(-0.012603f, 0.348619f, 0.069774f), new Vector3(0.022039f, 0.307239f, 0.105982f), new Vector3(0.030306f, 0.322173f, 0.093073f), new Vector3(0.029397f, 0.332197f, 0.08430301f), new Vector3(0.022961f, 0.341648f, 0.075984f), new Vector3(0.012603f, 0.348619f, 0.069774f), new Vector3(0.012586f, 0.299942f, 0.112171f), new Vector3(-0.012586f, 0.299942f, 0.112171f) };     //Neck
            meshSeamVerts[3][0][2][4] = new Vector3[0];     //Waist
            meshSeamVerts[3][0][2][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[3][0][2][6] = new Vector3[0];     //WaistAdultMale
            meshSeamVerts[3][0][3] = new Vector3[7][];         //Adult LittleDog LOD3 seams
            meshSeamVerts[3][0][3][0] = new Vector3[] { new Vector3(0.049175f, 0.053503f, 0.056394f), new Vector3(0.036192f, 0.052977f, 0.056146f), new Vector3(0.033381f, 0.049271f, 0.037793f), new Vector3(0.05128f, 0.049459f, 0.038351f), new Vector3(-0.036192f, 0.052977f, 0.056146f), new Vector3(-0.049175f, 0.053503f, 0.056394f), new Vector3(-0.05128f, 0.049459f, 0.038351f), new Vector3(-0.033381f, 0.049271f, 0.037793f), new Vector3(0.042107f, 0.047807f, 0.032652f), new Vector3(-0.042107f, 0.047807f, 0.032652f), new Vector3(0.042368f, 0.068555f, -0.252335f), new Vector3(0.03344f, 0.069764f, -0.258f), new Vector3(0.023818f, 0.06823601f, -0.25111f), new Vector3(0.029835f, 0.065184f, -0.231667f), new Vector3(0.038287f, 0.065391f, -0.231218f), new Vector3(-0.023818f, 0.06823601f, -0.25111f), new Vector3(-0.029835f, 0.065184f, -0.231667f), new Vector3(-0.038287f, 0.065391f, -0.231218f), new Vector3(-0.042368f, 0.068555f, -0.252335f), new Vector3(-0.03344f, 0.069764f, -0.258f) };     //Ankles
            meshSeamVerts[3][0][3][1] = new Vector3[] { new Vector3(0f, 0.292468f, -0.202558f), new Vector3(0f, 0.270991f, -0.21788f), new Vector3(0.010432f, 0.28655f, -0.204883f), new Vector3(-0.010433f, 0.286549f, -0.204883f) };     //Tail
            meshSeamVerts[3][0][3][2] = new Vector3[] { new Vector3(-0.03604f, 0.360596f, 0.116481f), new Vector3(-0.03751f, 0.353353f, 0.104223f), new Vector3(-0.032545f, 0.364857f, 0.093386f), new Vector3(-0.030711f, 0.374507f, 0.115047f), new Vector3(-0.028628f, 0.375944f, 0.100264f), new Vector3(0.03604f, 0.360596f, 0.116481f), new Vector3(0.03751f, 0.353353f, 0.104223f), new Vector3(0.032545f, 0.364857f, 0.093386f), new Vector3(0.030711f, 0.374507f, 0.115047f), new Vector3(0.028628f, 0.375944f, 0.100264f) };     //Ears
            meshSeamVerts[3][0][3][3] = new Vector3[] { new Vector3(0f, 0.295948f, 0.115472f), new Vector3(-0.017586f, 0.3034f, 0.109202f), new Vector3(-0.030831f, 0.327118f, 0.08883201f), new Vector3(0f, 0.351544f, 0.067247f), new Vector3(-0.018422f, 0.345765f, 0.072353f), new Vector3(0.030831f, 0.327118f, 0.08883201f), new Vector3(0.018422f, 0.345765f, 0.072353f), new Vector3(0.017586f, 0.3034f, 0.109202f) };     //Neck
            meshSeamVerts[3][0][3][4] = new Vector3[0];     //Waist
            meshSeamVerts[3][0][3][5] = new Vector3[0];     //WaistAdultFemale
            meshSeamVerts[3][0][3][6] = new Vector3[0];     //WaistAdultMale

            meshSeamVerts[4] = new Vector3[4][][][];    //ageSpecies
            meshSeamVerts[4][0] = new Vector3[4][][];   //Adult Fox
            meshSeamVerts[4][0][0] = new Vector3[7][];  //Adult Fox LOD0 seams
            meshSeamVerts[4][0][0][0] = new Vector3[0]; //Ankles
            meshSeamVerts[4][0][0][1] = new Vector3[0]; //Tail
            meshSeamVerts[4][0][0][2] = new Vector3[0]; //Ears
            meshSeamVerts[4][0][0][3] = new Vector3[0]; //Neck
            meshSeamVerts[4][0][0][4] = new Vector3[0]; //Waist
            meshSeamVerts[4][0][0][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[4][0][0][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[4][0][1] = new Vector3[7][];  //Adult Fox LOD1 seams
            meshSeamVerts[4][0][1][0] = new Vector3[0]; //Ankles
            meshSeamVerts[4][0][1][1] = new Vector3[0]; //Tail
            meshSeamVerts[4][0][1][2] = new Vector3[0]; //Ears
            meshSeamVerts[4][0][1][3] = new Vector3[0]; //Neck
            meshSeamVerts[4][0][1][4] = new Vector3[0]; //Waist
            meshSeamVerts[4][0][1][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[4][0][1][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[4][0][2] = new Vector3[7][];  //Adult Fox LOD2 seams
            meshSeamVerts[4][0][2][0] = new Vector3[0]; //Ankles
            meshSeamVerts[4][0][2][1] = new Vector3[0]; //Tail
            meshSeamVerts[4][0][2][2] = new Vector3[0]; //Ears
            meshSeamVerts[4][0][2][3] = new Vector3[0]; //Neck
            meshSeamVerts[4][0][2][4] = new Vector3[0]; //Waist
            meshSeamVerts[4][0][2][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[4][0][2][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[4][0][3] = new Vector3[7][];  //Adult Fox LOD3 seams
            meshSeamVerts[4][0][3][0] = new Vector3[0]; //Ankles
            meshSeamVerts[4][0][3][1] = new Vector3[0]; //Tail
            meshSeamVerts[4][0][3][2] = new Vector3[0]; //Ears
            meshSeamVerts[4][0][3][3] = new Vector3[0]; //Neck
            meshSeamVerts[4][0][3][4] = new Vector3[0]; //Waist
            meshSeamVerts[4][0][3][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[4][0][3][6] = new Vector3[0]; //WaistAdultMale

            meshSeamVerts[5] = new Vector3[4][][][];    //ageSpecies
            meshSeamVerts[5][0] = new Vector3[4][][];   //Adult Horse
            meshSeamVerts[5][0][0] = new Vector3[7][];  //Adult Horse LOD0 seams
            meshSeamVerts[5][0][0][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][0][0][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][0][0][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][0][0][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][0][0][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][0][0][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[5][0][0][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[5][0][1] = new Vector3[7][];  //Adult Horse LOD1 seams
            meshSeamVerts[5][0][1][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][0][1][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][0][1][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][0][1][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][0][1][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][0][1][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[5][0][1][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[5][0][2] = new Vector3[7][];  //Adult Horse LOD2 seams
            meshSeamVerts[5][0][2][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][0][2][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][0][2][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][0][2][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][0][2][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][0][2][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[5][0][2][6] = new Vector3[0]; //WaistAdultMale
            meshSeamVerts[5][0][3] = new Vector3[7][]; //Adult Horse LOD3 seams
            meshSeamVerts[5][0][3][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][0][3][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][0][3][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][0][3][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][0][3][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][0][3][5] = new Vector3[0]; //WaistAdultFemale
            meshSeamVerts[5][0][3][6] = new Vector3[0]; //WaistAdultMale

            meshSeamVerts[5][1] = new Vector3[4][][];   //Child Horse
            meshSeamVerts[5][1][0] = new Vector3[7][];  //Child Horse LOD0 seams
            meshSeamVerts[5][1][0][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][1][0][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][1][0][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][1][0][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][1][0][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][1][0][5] = new Vector3[0]; //WaistChildFemale
            meshSeamVerts[5][1][0][6] = new Vector3[0]; //WaistChildMale
            meshSeamVerts[5][1][1] = new Vector3[7][];  //Child Horse LOD1 seams
            meshSeamVerts[5][1][1][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][1][1][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][1][1][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][1][1][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][1][1][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][1][1][5] = new Vector3[0]; //WaistChildFemale
            meshSeamVerts[5][1][1][6] = new Vector3[0]; //WaistChildMale
            meshSeamVerts[5][1][2] = new Vector3[7][];  //Child Horse LOD2 seams
            meshSeamVerts[5][1][2][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][1][2][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][1][2][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][1][2][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][1][2][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][1][2][5] = new Vector3[0]; //WaistChildFemale
            meshSeamVerts[5][1][2][6] = new Vector3[0]; //WaistChildMale
            meshSeamVerts[5][1][3] = new Vector3[7][]; //Child Horse LOD3 seams
            meshSeamVerts[5][1][3][0] = new Vector3[0]; //Ankles
            meshSeamVerts[5][1][3][1] = new Vector3[0]; //Tail
            meshSeamVerts[5][1][3][2] = new Vector3[0]; //Ears
            meshSeamVerts[5][1][3][3] = new Vector3[0]; //Neck
            meshSeamVerts[5][1][3][4] = new Vector3[0]; //Waist
            meshSeamVerts[5][1][3][5] = new Vector3[0]; //WaistChildFemale
            meshSeamVerts[5][1][3][6] = new Vector3[0]; //WaistChildMale

            return meshSeamVerts;
        }

        // helper classes

        public class vertexForm
        {
            internal int datatype, subtype;
            internal byte bytesper;

            public int formatDataType
            {
                get { return this.datatype; }
            }
            public int formatSubType
            {
                get { return this.subtype; }
            }
            public byte formatDataLength
            {
                get { return this.bytesper; }
            }

            public vertexForm() { }

            public vertexForm(vertexForm source)
            {
                this.datatype = source.datatype;
                this.subtype = source.subtype;
                this.bytesper = source.bytesper;
            }

            public vertexForm(int datatype, int subtype, byte bytesper)
            {
                this.datatype = datatype;
                this.subtype = subtype;
                this.bytesper = bytesper;
            }
            internal vertexForm(BinaryReader br)
            {
                this.datatype = br.ReadInt32();
                this.subtype = br.ReadInt32();
                this.bytesper = br.ReadByte();
            }
            internal void vertexformatWrite(BinaryWriter bw)
            {
                bw.Write(this.datatype);
                bw.Write(this.subtype);
                bw.Write(this.bytesper);
            }
            public override string ToString()
            {
                return Enum.GetName(typeof(VertexFormatNames), this.datatype);
            }
        }

        internal class position
        {
            float x, y, z;

            public float[] Coordinates
            {
                get { return new float[3] { this.x, this.y, this.z }; }
            }
            internal float X
            {
                get { return this.x; }
                set { this.x = value; }
            }
            internal float Y
            {
                get { return this.y; }
                set { this.y = value; }
            }
            internal float Z
            {
                get { return this.z; }
                set { this.z = value; }
            }

            internal position()
            {
                this.x = 0f;
                this.y = 0f;
                this.z = 0f;
            }

            internal position(position source)
            {
                this.x = source.x;
                this.y = source.y;
                this.z = source.z;
            }

            internal position(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            internal position(float[] newPosition)
            {
                this.x = newPosition[0];
                this.y = newPosition[1];
                this.z = newPosition[2];
            }
            internal position(BinaryReader br)
            {
                this.x = br.ReadSingle();
                this.y = br.ReadSingle();
                this.z = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.x);
                bw.Write(this.y);
                bw.Write(this.z);
            }
            public bool Equals(position comparePosition)
            {
                return (IsEqual(this.x, comparePosition.x) && IsEqual(this.y, comparePosition.y) && IsEqual(this.z, comparePosition.z));
            }
            public override string ToString()
            {
                return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
            }
            public float[] Data()
            {
                return new float[3] { this.x, this.y, this.z };
            }
            public void addDeltas(float[] deltas)
            {
                this.x += deltas[0];
                this.y += deltas[1];
                this.z += deltas[2];
            }
        }
        internal class normal
        {
            float x, y, z;

            public float[] Coordinates
            {
                get { return new float[3] { this.x, this.y, this.z }; }
            }
            internal float X
            {
                get { return this.x; }
                set { this.x = value; }
            }
            internal float Y
            {
                get { return this.y; }
                set { this.y = value; }
            }
            internal float Z
            {
                get { return this.z; }
                set { this.z = value; }
            }

            internal normal()
            {
                this.x = 0f;
                this.y = 0f;
                this.z = 0f;
            }
            internal normal(normal source)
            {
                this.x = source.x;
                this.y = source.y;
                this.z = source.z;
            }
            internal normal(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            internal normal(float[] newNormal)
            {
                this.x = newNormal[0];
                this.y = newNormal[1];
                this.z = newNormal[2];
            }
            internal normal(BinaryReader br)
            {
                this.x = br.ReadSingle();
                this.y = br.ReadSingle();
                this.z = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.x);
                bw.Write(this.y);
                bw.Write(this.z);
            }
            public bool Equals(normal compareNormal)
            {
                return (IsEqual(this.x, compareNormal.x) && IsEqual(this.y, compareNormal.y) && IsEqual(this.z, compareNormal.z));
            }
            public override string ToString()
            {
                return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
            }
            public float[] Data()
            {
                return new float[3] { this.x, this.y, this.z };
            }
            public void addDeltas(float[] deltas)
            {
                this.x += deltas[0];
                this.y += deltas[1];
                this.z += deltas[2];
            }
        }

        internal class uv
        {
            float x, y;

            internal float U
            {
                get { return this.x; }
                set { this.x = value; }
            }
            internal float V
            {
                get { return this.y; }
                set { this.y = value; }
            }

            internal uv() { }
            internal uv(uv source)
            {
                this.x = source.x;
                this.y = source.y;
            }
            internal uv(float u, float v)
            {
                this.x = u;
                this.y = v;
            }
            internal uv(float[] newUV)
            {
                this.x = newUV[0];
                this.y = newUV[1];
            }
            internal uv(float[] newUV, bool verticalFlip)
            {
                this.x = newUV[0];
                if (verticalFlip)
                {
                    this.y = 1f - newUV[1];
                }
                else
                {
                    this.y = newUV[1];
                }
            }
            internal uv(BinaryReader br)
            {
                this.x = br.ReadSingle();
                this.y = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.x);
                bw.Write(this.y);
            }
            public bool Equals(uv compareUV)
            {
                return (IsEqual(this.x, compareUV.x) && IsEqual(this.y, compareUV.y));
            }
            public bool CloseTo(uv other)
            {
                const float diff = 0.001f;
                return
                (
                   (Math.Abs(this.x - other.x) < diff) &&
                   (Math.Abs(this.y - other.y) < diff)
                );
            }
            public override string ToString()
            {
                return this.x.ToString() + ", " + this.y.ToString();
            }
            public float[] Data()
            {
                return new float[2] { this.x, this.y };
            }
        }
        internal class Bones
        {
            byte[] assignments = new byte[4];
            //float[] weights = new float[4];
            byte[] weights = new byte[4];
            internal Bones() { }
            internal Bones(Bones source)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = source.assignments[i];
                    this.weights[i] = source.weights[i];
                }
            }
            internal Bones(byte[] assignmentsIn, float[] weightsIn)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = assignmentsIn[i];
                    this.weights[i] = (byte)(Math.Min(weightsIn[i] * 255f, 255f));
                }
            }
            internal Bones(int[] assignmentsIn, float[] weightsIn)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = (byte)assignmentsIn[i];
                    this.weights[i] = (byte)(Math.Min(weightsIn[i] * 255f, 255f));
                }
            }
            internal Bones(byte[] assignmentsIn, byte[] weightsIn)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = assignmentsIn[i];
                    this.weights[i] = weightsIn[i];
                    //this.weights[i] = (float)weightsIn[i] / 255f;
                }
            }
            internal Bones(int[] assignmentsIn, byte[] weightsIn)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = (byte)assignmentsIn[i];
                    this.weights[i] = weightsIn[i];
                    //this.weights[i] = (float)weightsIn[i] / 255f;
                }
            }
            internal void ReadAssignments(BinaryReader br)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.assignments[i] = br.ReadByte();
                }
            }
            internal void ReadWeights(BinaryReader br, int subtype)
            {
                if (subtype == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float tmp = br.ReadSingle();
                        this.weights[i] = (byte)(Math.Min(tmp * 255f, 255f));
                    }
                }
                else if (subtype == 2)
                {
                    //for (int i = 2; i >= 0; i--)          // for CAS demo meshes only!
                    //{
                    //    byte tmp = br.ReadByte();
                    //    this.weights[i] = (float)tmp / 255f;
                    //}
                    //byte tmp2 = br.ReadByte();
                    //this.weights[3] = (float)tmp2 / 255f;
                    for (int i = 0; i < 4; i++)
                    {
                        this.weights[i] = br.ReadByte();
                        //this.weights[i] = (float)this.weightsNew[i] / 255f;
                    }
                }
            }
            internal void WriteAssignments(BinaryWriter bw, int version, int maxBoneIndex)
            {
                if (version == 5)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.weights[i] > 0f)
                        {
                            bw.Write(this.assignments[i]);
                        }
                        else
                        {
                            bw.Write((byte)2);
                        }
                    }
                }
                else if (version >= 12)
                {
                    //byte tmp = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        //if (this.weights[i] > 0)
                        //{
                        bw.Write(this.assignments[i]);
                        //}
                        //else
                        //{
                        //    bw.Write(tmp);
                        //    tmp = (byte)Math.Min(tmp++, maxBoneIndex);
                        //}
                    }
                }

            }
            internal void WriteWeights(BinaryWriter bw, int version)
            {
                if (version == 5)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float tmp = (float)this.weights[i] / 255f;
                        bw.Write(tmp);
                    }
                }
                else if (version >= 12)
                {
                    bw.Write(this.weights);
                }
            }
            public bool Equals(Bones compareBones)
            {
                return (this.assignments[0] == compareBones.assignments[0] && this.assignments[1] == compareBones.assignments[1] &&
                    this.assignments[2] == compareBones.assignments[2] && this.assignments[3] == compareBones.assignments[3] &&
                    this.weights[0] == compareBones.weights[0] && this.weights[1] == compareBones.weights[1] &&
                    this.weights[2] == compareBones.weights[2] && this.weights[3] == compareBones.weights[3]);
            }
            public override string ToString()
            {
                return this.assignments[0].ToString() + this.assignments[1].ToString() + this.assignments[2].ToString() + this.assignments[3].ToString() +
                    this.weights[0].ToString() + ", " + this.weights[1].ToString() + ", " + this.weights[2].ToString() + ", " + this.weights[3].ToString();
            }
            internal byte[] boneAssignments
            {
                get { return new byte[] { this.assignments[0], this.assignments[1], this.assignments[2], this.assignments[3] }; }
                set
                {
                    for (int i = 0; i < this.assignments.Length; i++)
                    {
                        this.assignments[i] = value[i];
                    }
                }
            }
            internal float[] boneWeightsV5
            {
                get { return new float[] { this.weights[0], this.weights[1], this.weights[2], this.weights[3] }; }
                set
                {
                    for (int i = 0; i < this.assignments.Length; i++)
                    {
                        //this.weights[i] = value[i];
                        this.weights[i] = (byte)(Math.Min(value[i] * 255f, 255f));
                    }
                }
            }
            internal byte[] boneWeights
            {
                get { return new byte[] { this.weights[0], this.weights[1], this.weights[2], this.weights[3] }; }
                set
                {
                    int tot = 0;
                    for (int i = 0; i < this.assignments.Length; i++)
                    {
                        this.weights[i] = value[i];
                        tot += value[i];
                        //this.weights[i] = (float)value[i] / 255f;
                    }
                    this.weights[0] += (byte)(255 - tot);
                }
            }
            internal void Sort(int version)
            {
                for (int i = this.assignments.Length - 2; i >= 0; i--)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        if (this.weights[j] < this.weights[j + 1])
                        {
                            byte tb = this.assignments[j];
                            this.assignments[j] = this.assignments[j + 1];
                            this.assignments[j + 1] = tb;
                            byte tw = this.weights[j];
                            this.weights[j] = this.weights[j + 1];
                            this.weights[j + 1] = tw;
                        }
                    }
                }
            }
        }

        internal class tangent
        {
            float x, y, z;
            internal tangent() { }
            internal float X
            {
                get { return this.x; }
                set { this.x = value; }
            }
            internal float Y
            {
                get { return this.y; }
                set { this.y = value; }
            }
            internal float Z
            {
                get { return this.z; }
                set { this.z = value; }
            }
            internal tangent(tangent source)
            {
                this.x = source.x;
                this.y = source.y;
                this.z = source.z;
            }
            internal tangent(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            internal tangent(float[] newTangent)
            {
                this.x = newTangent[0];
                this.y = newTangent[1];
                this.z = newTangent[2];
            }
            internal tangent(BinaryReader br)
            {
                this.x = br.ReadSingle();
                this.y = br.ReadSingle();
                this.z = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.x);
                bw.Write(this.y);
                bw.Write(this.z);
            }
            public bool Equals(tangent compareTangent)
            {
                return (IsEqual(this.x, compareTangent.x) && IsEqual(this.y, compareTangent.y) && IsEqual(this.z, compareTangent.z));
            }
            public override string ToString()
            {
                return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
            }
            public float[] Data()
            {
                return new float[3] { this.x, this.y, this.z };
            }
        }
        internal class tagval
        {
            uint tags;
            internal tagval() { }
            internal tagval(tagval source)
            {
                this.tags = source.tags;
            }
            internal tagval(uint tagValue)
            {
                this.tags = tagValue;
            }
            internal tagval(BinaryReader br)
            {
                this.tags = br.ReadUInt32();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.tags);
            }
            public bool Equals(tagval compareTagval)
            {
                return (this.tags == compareTagval.tags);
            }
            public override string ToString()
            {
                return Convert.ToString(this.tags, 16).ToUpper().PadLeft(8, '0');
            }
            public uint Data()
            {
                return this.tags;
            }
        }

        public class Face
        {
            uint[] face = new uint[3];
            public int facePoint0
            {
                get { return (int)face[0]; }
            }
            public int facePoint1
            {
                get { return (int)face[1]; }
            }

            public int facePoint2
            {
                get { return (int)face[2]; }
            }
            internal Face() { }
            internal Face(Face source)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.face[i] = source.face[i];
                }
            }
            internal uint[] meshface
            {
                get
                {
                    return new uint[] { face[0], face[1], face[2] };
                }
            }
            internal Face(byte[] face)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.face[i] = (uint)face[i];
                }
            }
            internal Face(ushort[] face)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.face[i] = (uint)face[i];
                }
            }
            internal Face(int[] face)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.face[i] = (uint)face[i];
                }
            }
            internal Face(uint[] face)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.face[i] = face[i];
                }
            }
            public Face(int FacePoint0, int FacePoint1, int FacePoint2)
            {
                this.face[0] = (uint)FacePoint0;
                this.face[1] = (uint)FacePoint1;
                this.face[2] = (uint)FacePoint2;
            }
            public Face(uint FacePoint0, uint FacePoint1, uint FacePoint2)
            {
                this.face[0] = FacePoint0;
                this.face[1] = FacePoint1;
                this.face[2] = FacePoint2;
            }
            public Face(short FacePoint0, short FacePoint1, short FacePoint2)
            {
                this.face[0] = (uint)FacePoint0;
                this.face[1] = (uint)FacePoint1;
                this.face[2] = (uint)FacePoint2;
            }

            internal Face(BinaryReader br, byte bytesperfacepnt)
            {
                for (int i = 0; i < 3; i++)
                {
                    switch (bytesperfacepnt)
                    {
                        case (1):
                            this.face[i] = br.ReadByte();
                            break;
                        case (2):
                            this.face[i] = br.ReadUInt16();
                            break;
                        case (4):
                            this.face[i] = br.ReadUInt32();
                            break;
                        default:
                            break;
                    }

                }
            }
            internal void Write(BinaryWriter bw, byte bytesperfacepnt)
            {
                for (int i = 0; i < 3; i++)
                {
                    switch (bytesperfacepnt)
                    {
                        case (1):
                            bw.Write((byte)this.face[i]);
                            break;
                        case (2):
                            bw.Write((ushort)this.face[i]);
                            break;
                        case (4):
                            bw.Write(this.face[i]);
                            break;
                        default:
                            break;
                    }
                }
            }
            public static Face Reverse(Face source)
            {
                return new Face(source.facePoint2, source.facePoint1, source.facePoint0);
            }
            public void Reverse()
            {
                uint tmp = this.face[0];
                this.face[0] = this.face[2];
                this.face[2] = tmp;
            }
            public bool Equals(Face f)
            {
                return ((this.face[0] == f.face[0]) && (this.face[1] == f.face[1]) && (this.face[2] == f.face[2]));
            }
            public override string ToString()
            {
                return this.face[0].ToString() + ", " + this.face[1].ToString() + ", " + this.face[2].ToString();
            }
        }

        public class UVStitch
        {
            int vertexIndex;
            int count;
            float[][] coordinates;

            public int Index
            {
                get { return this.vertexIndex; }
                set { this.vertexIndex = value; }
            }
            public int Count
            {
                get { return this.count; }
            }
            public List<float[]> UV1Coordinates
            {
                get
                {
                    List<float[]> pairs = new List<float[]>();
                    for (int i = 0; i < coordinates.GetLength(0); i++)
                    {
                        pairs.Add(coordinates[i]);
                    }
                    return pairs;
                }
            }
            public List<Vector2> UV1Vectors
            {
                get
                {
                    List<Vector2> pairs = new List<Vector2>();
                    for (int i = 0; i < coordinates.GetLength(0); i++)
                    {
                        pairs.Add(new Vector2(coordinates[i]));
                    }
                    return pairs;
                }
            }
            public int Size
            {
                get { return 8 + (this.count * 8); }
            }

            internal UVStitch(BinaryReader br)
            {
                this.vertexIndex = br.ReadInt32();
                this.count = br.ReadInt32();
                this.coordinates = new float[this.count][];
                for (int i = 0; i < this.count; i++)
                {
                    this.coordinates[i] = new float[2];
                    this.coordinates[i][0] = br.ReadSingle();
                    this.coordinates[i][1] = br.ReadSingle();
                }
            }

            public UVStitch(UVStitch adjustment)
            {
                this.vertexIndex = adjustment.vertexIndex;
                this.count = adjustment.count;
                this.coordinates = new float[adjustment.count][];
                for (int i = 0; i < this.count; i++)
                {
                    this.coordinates[i] = new float[2];
                    this.coordinates[i][0] = adjustment.coordinates[i][0];
                    this.coordinates[i][1] = adjustment.coordinates[i][1];
                }
            }

            public UVStitch(int vertexIndex, Vector2[] uv1Coordinates)
            {
                this.vertexIndex = vertexIndex;
                this.count = uv1Coordinates.Length;
                this.coordinates = new float[uv1Coordinates.Length][];
                for (int i = 0; i < uv1Coordinates.Length; i++)
                {
                    this.coordinates[i] = new float[2] { uv1Coordinates[i].X, uv1Coordinates[i].Y };
                }
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.vertexIndex);
                bw.Write(this.count);
                for (int i = 0; i < this.count; i++)
                {
                    bw.Write(this.coordinates[i][0]);
                    bw.Write(this.coordinates[i][1]);
                }
            }
        }

        public class SeamStitch : IComparable, IEquatable<SeamStitch>
        {
            uint index;
            UInt16 vertID;

            public int waistSequence;
            public float uvx;

            public uint Index
            {
                get { return this.index; }
                set { this.index = value; }
            }
            public UInt16 VertID
            {
                get { return this.vertID; }
                set { this.vertID = value; }
            }
            public int SeamType
            {
                get { return this.vertID >> 12; }
            }
            public int SeamIndex
            {
                get { return this.vertID & 0x0FFF; }
            }
            public int Size
            {
                get { return 6; }
            }

            internal SeamStitch(BinaryReader br, uv[] uvs)
            {
                this.index = br.ReadUInt32();
                this.vertID = br.ReadUInt16();
                if (this.SeamType == (int)GEOM.SeamType.WaistAdultFemale) this.waistSequence = femaleWaistSequence.IndexOf(this.SeamIndex);
                else if (this.SeamType == (int)GEOM.SeamType.WaistAdultMale) this.waistSequence = maleWaistSequence.IndexOf(this.SeamIndex);
                else this.waistSequence = this.SeamIndex;
                this.uvx = uvs[index].U;
            }

            internal SeamStitch(SeamStitch adjustment)
            {
                this.index = adjustment.index;
                this.vertID = adjustment.vertID;
                this.waistSequence = adjustment.waistSequence;
                this.uvx = adjustment.uvx;
            }

            internal SeamStitch(int index, GEOM.SeamType seam, int vertex, uv[] uvs)
            {
                this.index = (uint)index;
                this.vertID = (ushort)(((int)seam << 12) + vertex);
                if (this.SeamType == (int)GEOM.SeamType.WaistAdultFemale) this.waistSequence = femaleWaistSequence.IndexOf(this.SeamIndex);
                else if (this.SeamType == (int)GEOM.SeamType.WaistAdultMale) this.waistSequence = maleWaistSequence.IndexOf(this.SeamIndex);
                this.uvx = uvs[index].U;
            }

            internal SeamStitch(int index, int seam, int vertex, uv[] uvs)
            {
                this.index = (uint)index;
                this.vertID = (ushort)((seam << 12) + vertex);
                if (this.SeamType == (int)GEOM.SeamType.WaistAdultFemale) this.waistSequence = femaleWaistSequence.IndexOf(this.SeamIndex);
                else if (this.SeamType == (int)GEOM.SeamType.WaistAdultMale) this.waistSequence = maleWaistSequence.IndexOf(this.SeamIndex);
                this.uvx = uvs[index].U;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.index);
                bw.Write(this.vertID);
            }

            public override bool Equals(object obj)
            {
                if (obj is SeamStitch) return this.Equals(obj as SeamStitch);
                else return false;
            }

            public bool Equals(SeamStitch ss)
            {
                //return this.vertID == ss.vertID & this.uvx == ss.uvx;
                return this.SeamType == ss.SeamType && this.waistSequence == ss.waistSequence;
            }

            public int CompareTo(object obj)
            {
                if (obj is SeamStitch)
                {
                    SeamStitch ss = obj as SeamStitch;
                    if (this.waistSequence.Equals(ss.waistSequence)) return this.uvx.CompareTo(ss.uvx);
                    else return this.waistSequence.CompareTo(ss.waistSequence);
                }
                else return 0;
            }

            public int GetHashCode()
            {
                return (this.vertID + this.uvx).GetHashCode();
            }
        }

        public class SlotrayIntersection
        {
            uint slotBone;              //GEOM version < 14 index of slot bone, GEOM version >= 14 hash of slot bone
            short[] vertIndices;        // short[3] indices of vertices making up face
            float[] coordinates;        // float[2] Barycentric coordinates of the point of intersection
            float distance;             // distance from raycast origin to the intersection point
            float[] offsetFromIntersectionOS;  // Vector3 offset from the intersection point to the slot's average position (if outside geometry) in object space
            float[] slotAveragePosOS;   // Vector3 slot's average position in object space
            float[] transformToLS;     // Quaternion transform from object space to the slot's local space
            uint pivotBone;             // index or hash of the bone that this slot pivots around, 0xFF or 0x00000000 if pivot does not exist

            int parentVersion;

            public uint SlotIndex
            {
                get { return this.parentVersion < 14 ? this.slotBone : 0xFFFFFFFF; }
                set { this.slotBone = value; }
            }
            public uint SlotHash
            {
                get { return this.parentVersion >= 14 ? this.slotBone : 0xFFFFFFFF; }
                set { this.slotBone = value; }
            }
            public int[] TrianglePointIndices
            {
                get { return new int[] { this.vertIndices[0], this.vertIndices[1], this.vertIndices[2] }; }
                set { this.vertIndices[0] = (short)value[0]; this.vertIndices[1] = (short)value[1]; this.vertIndices[2] = (short)value[2]; }
            }
            public Vector2 Coordinates
            {
                get { return new Vector2(this.coordinates); }
                set { this.coordinates = value.Coordinates; }
            }
            public float Distance
            {
                get { return this.distance; }
                set { this.distance = value; }
            }
            public Vector3 OffsetFromIntersectionOS
            {
                get { return new Vector3(this.offsetFromIntersectionOS); }
                set { this.offsetFromIntersectionOS = value.Coordinates; }
            }
            public Vector3 SlotAveragePosOS
            {
                get { return new Vector3(this.slotAveragePosOS); }
                set { this.slotAveragePosOS = value.Coordinates; }
            }
            public Quaternion TransformToLS
            {
                get { return new Quaternion(this.transformToLS); }
                set { this.transformToLS = new float[] { (float)value.X, (float)value.Y, (float)value.Z, (float)value.W }; }
            }
            public uint PivotBone
            {
                get { return this.pivotBone; }
                set { this.pivotBone = value; }
            }

            internal SlotrayIntersection(BinaryReader br, int version)
            {
                this.parentVersion = version;
                this.slotBone = br.ReadUInt32();
                this.vertIndices = new short[3];
                for (int i = 0; i < 3; i++)
                {
                    this.vertIndices[i] = br.ReadInt16();
                }
                this.coordinates = new float[2];
                for (int i = 0; i < 2; i++)
                {
                    this.coordinates[i] = br.ReadSingle();
                }
                this.distance = br.ReadSingle();
                this.offsetFromIntersectionOS = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    this.offsetFromIntersectionOS[i] = br.ReadSingle();
                }
                this.slotAveragePosOS = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    this.slotAveragePosOS[i] = br.ReadSingle();
                }
                this.transformToLS = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    this.transformToLS[i] = br.ReadSingle();
                }
                if (parentVersion >= 14)
                {
                    this.pivotBone = br.ReadUInt32();
                }
                else
                {
                    this.pivotBone = br.ReadByte();
                }
            }

            public SlotrayIntersection(SlotrayIntersection faceAdjustment)
            {
                this.parentVersion = faceAdjustment.parentVersion;
                this.slotBone = faceAdjustment.slotBone;
                this.vertIndices = new short[3];
                for (int i = 0; i < 3; i++)
                {
                    this.vertIndices[i] = faceAdjustment.vertIndices[i];
                }
                this.coordinates = new float[2];
                for (int i = 0; i < 2; i++)
                {
                    this.coordinates[i] = faceAdjustment.coordinates[i];
                }
                this.distance = faceAdjustment.distance;
                this.offsetFromIntersectionOS = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    this.offsetFromIntersectionOS[i] = faceAdjustment.offsetFromIntersectionOS[i];
                }
                this.slotAveragePosOS = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    this.slotAveragePosOS[i] = faceAdjustment.slotAveragePosOS[i];
                }
                this.transformToLS = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    this.transformToLS[i] = faceAdjustment.transformToLS[i];
                }
                this.pivotBone = faceAdjustment.pivotBone;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.slotBone);
                for (int i = 0; i < this.vertIndices.Length; i++)
                {
                    bw.Write(this.vertIndices[i]);
                }
                for (int i = 0; i < this.coordinates.Length; i++)
                {
                    bw.Write(this.coordinates[i]);
                }
                bw.Write(this.distance);
                for (int i = 0; i < this.offsetFromIntersectionOS.Length; i++)
                {
                    bw.Write(this.offsetFromIntersectionOS[i]);
                }
                for (int i = 0; i < this.slotAveragePosOS.Length; i++)
                {
                    bw.Write(this.slotAveragePosOS[i]);
                }
                for (int i = 0; i < this.transformToLS.Length; i++)
                {
                    bw.Write(this.transformToLS[i]);
                }
                if (parentVersion >= 14)
                {
                    bw.Write(this.pivotBone);
                }
                else
                {
                    bw.Write((byte)this.pivotBone);
                }
            }

        }

        public class MTNF
        {
            char[] magic;
            int zero, datasize, paramCount;
            uint[][] paramList;
            object[][] dataList;

            public MTNF() { }

            internal int chunkSize
            {
                get { return 16 + (this.paramCount * 16) + this.datasize; }
            }

            public uint[] getParamsList()
            {
                uint[] tmp = new uint[this.paramCount];
                for (int i = 0; i < this.paramCount; i++)
                {
                    tmp[i] = this.paramList[i][0];
                }
                return tmp;
            }

            public object[] getParamValue(uint parameter, out int valueType)
            {
                object[] tmp = null;
                for (int i = 0; i < this.paramCount; i++)
                {
                    if (this.paramList[i][0] == parameter)
                    {
                        tmp = new object[this.paramList[i][2]];
                        for (int j = 0; j < tmp.Length; j++)
                        {
                            tmp[j] = this.dataList[i][j];
                        }
                        valueType = (int)this.paramList[i][1];
                        return tmp;
                    }
                }
                valueType = 0;
                return null;
            }

            internal MTNF(MTNF source)
            {
                this.magic = source.magic;
                this.zero = source.zero;
                this.datasize = source.datasize;
                this.paramCount = source.paramCount;
                this.paramList = new uint[source.paramList.Length][];
                for (int i = 0; i < source.paramList.Length; i++)
                {
                    this.paramList[i] = new uint[source.paramList[i].Length];
                    for (int j = 0; j < source.paramList[i].Length; j++)
                    {
                        this.paramList[i][j] = source.paramList[i][j];
                    }
                }
                this.dataList = new object[source.dataList.Length][];
                for (int i = 0; i < source.dataList.Length; i++)
                {
                    this.dataList[i] = new object[source.dataList[i].Length];
                    for (int j = 0; j < source.dataList[i].Length; j++)
                    {
                        this.dataList[i][j] = source.dataList[i][j];
                    }
                }
            }

            internal MTNF(BinaryReader br)
            {
                this.magic = br.ReadChars(4);
                this.zero = br.ReadInt32();
                this.datasize = br.ReadInt32();
                this.paramCount = br.ReadInt32();
                this.paramList = new uint[paramCount][];
                for (int i = 0; i < paramCount; i++)
                {
                    this.paramList[i] = new uint[4];
                    for (int j = 0; j < 4; j++)
                    {
                        this.paramList[i][j] = br.ReadUInt32();
                    }
                }
                this.dataList = new object[paramCount][];
                for (int i = 0; i < paramCount; i++)
                {
                    this.dataList[i] = new object[paramList[i][2]];
                    if (paramList[i][1] == 1)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            this.dataList[i][j] = br.ReadSingle();
                        }
                    }
                    else if (paramList[i][1] == 2)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            this.dataList[i][j] = br.ReadInt32();
                        }
                    }
                    else
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            this.dataList[i][j] = br.ReadUInt32();
                        }
                    }
                }
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(magic);
                bw.Write(zero);
                bw.Write(datasize);
                bw.Write(paramCount);
                for (int i = 0; i < paramCount; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bw.Write(paramList[i][j]);
                    }
                }
                for (int i = 0; i < paramCount; i++)
                {
                    if (paramList[i][1] == 1)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            bw.Write((float)dataList[i][j]);
                        }
                    }
                    else if (paramList[i][1] == 2)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            bw.Write((int)dataList[i][j]);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            bw.Write((uint)dataList[i][j]);
                        }
                    }
                }
            }

            public int normalIndex
            {
                get
                {
                    for (int i = 0; i < this.paramCount; i++)
                    {
                        if (this.paramList[i][0] == (uint)ShaderParam.NormalMap)
                        {
                            uint tmp = (uint)this.dataList[i][0];
                            return (int)tmp;
                        }
                    }
                    return -1;
                }
                set
                {
                    for (int i = 0; i < this.paramCount; i++)
                    {
                        if (this.paramList[i][0] == (uint)ShaderParam.NormalMap)
                        {
                            this.dataList[i][0] = (uint)value;
                        }
                    }

                }
            }

            public int emissionIndex
            {
                get
                {
                    for (int i = 0; i < this.paramCount; i++)
                    {
                        if (this.paramList[i][0] == (uint)ShaderParam.EmissionMap)
                        {
                            uint tmp = (uint)this.dataList[i][0];
                            return (int)tmp;
                        }
                    }
                    return -1;
                }
                set
                {
                    for (int i = 0; i < this.paramCount; i++)
                    {
                        if (this.paramList[i][0] == (uint)ShaderParam.EmissionMap)
                        {
                            this.dataList[i][0] = (uint)value;
                        }
                    }

                }
            }

            public MTNF(uint[] shaderDataArray)
            {
                magic = Encoding.UTF8.GetString(BitConverter.GetBytes(shaderDataArray[0])).ToCharArray();
                zero = (int)shaderDataArray[1];
                datasize = (int)shaderDataArray[2];
                paramCount = (int)shaderDataArray[3];
                paramList = new uint[paramCount][];
                dataList = new object[paramCount][];
                int ind = 4;
                for (int i = 0; i < paramCount; i++)
                {
                    paramList[i] = new uint[4];
                    for (int j = 0; j < 4; j++)
                    {
                        paramList[i][j] = shaderDataArray[ind];
                        ind++;
                    }
                }
                for (int i = 0; i < paramCount; i++)
                {
                    dataList[i] = new object[paramList[i][2]];
                    if (paramList[i][1] == 1)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            byte[] b = BitConverter.GetBytes(shaderDataArray[ind]);
                            dataList[i][j] = BitConverter.ToSingle(b, 0);
                            ind++;
                        }
                    }
                    else if (paramList[i][1] == 2)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            dataList[i][j] = (int)shaderDataArray[ind];
                            ind++;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            dataList[i][j] = shaderDataArray[ind];
                            ind++;
                        }
                    }
                }
            }

            public uint[] toDataArray()
            {
                List<uint> tmp = new List<uint>();
                tmp.Add(BitConverter.ToUInt32(Encoding.UTF8.GetBytes(magic), 0));
                tmp.Add((uint)zero);
                tmp.Add((uint)datasize);
                tmp.Add((uint)paramCount);
                for (int i = 0; i < paramCount; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tmp.Add(paramList[i][j]);
                    }
                }
                for (int i = 0; i < paramCount; i++)
                {
                    if (paramList[i][1] == 1)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            byte[] b = BitConverter.GetBytes((float)dataList[i][j]);
                            tmp.Add(BitConverter.ToUInt32(b, 0));
                        }
                    }
                    else if (paramList[i][1] == 2)
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            int t = (int)dataList[i][j];
                            tmp.Add((uint)t);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < paramList[i][2]; j++)
                        {
                            tmp.Add((uint)dataList[i][j]);
                        }
                    }
                }
                return tmp.ToArray();
            }

        }

        [global::System.Serializable]
        public class MeshException : ApplicationException
        {
            public MeshException() { }
            public MeshException(string message) : base(message) { }
            public MeshException(string message, Exception inner) : base(message, inner) { }
            protected MeshException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

        public enum VertexFormatNames
        {
            Position = 1,
            Normals = 2,
            UV = 3,
            BoneAssignment = 4,
            BoneWeight = 5,
            Tangents = 6,
            TagVal = 7,
            VertexID = 10
        }

        internal static bool IsEqual(float x, float y)
        {
            const float EPSILON = 1e-4f;
            return (Math.Abs(x - y) < EPSILON);
        }

        public enum SeamType
        {
            Ankles = 0,
            PetTail = 1,
            PetEars = 2,
            Neck = 3,
            Waist = 4,
            WaistAdultFemale = 5,
            WaistAdultMale = 6,
        }

        public int CompareTo(GEOM other)
        {
            if (this.numberVertices > other.numberVertices) return -1;
            else if (this.numberVertices < other.numberVertices) return 1;
            else return 0;
        }
    }
}
