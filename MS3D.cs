/* TS4 MorphMaker, a tool for creating custom content for The Sims 4,
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
using System.Globalization;

namespace TS4SimRipper
{
    public class MS3D
    {
        // max values
        //
        const int MAX_VERTICES = 65535;
        const int MAX_TRIANGLES = 65535;
        const int MAX_GROUPS = 128;
        const int MAX_MATERIALS = 128;
        const int MAX_JOINTS = 128;
        const int MAX_KEYFRAMES = 216;     // increase when needed

        // flags
        //
        const int SELECTED = 1;
        const int HIDDEN = 2;
        const int SELECTED2 = 4;
        const int DIRTY = 8;

        char[] id = new char[10];                       // always "MS3D000000"
        int version;                                    // 4
        ushort numVertices;                             // number of vertices
        Vertex[] verts;                                 // numVertices times
        ushort numTriangles;                            // number faces
        Face[] faces;                                   // numTriangles times
        ushort numGroups;
        Group[] groups;                                 // numGroups times
        ushort numMaterials;
        Material[] materials;                           // numMaterials times
        float fAnimationFPS;
        float fCurrentTime;
        int iTotalFrames;
        ushort numJoints;
        Joint[] joints;                                 // numJoints times
        int commentSubversion;                          // 1
        uint numGroupComments;
        Comment[] groupComments;                        //numGroupComments times
        uint numMaterialComments;
        Comment[] materialComments;                     //numMaterialComments times
        uint numJointComments;
        Comment[] jointComments;                        //numJointComments times
        uint hasModelComment;                           // one or zero
        Comment modelComments;                          //numModelComments times
        int vertexExtraSubversion;                      // 1, 2, or 3
        VertexExtra[] vertexExtras;                     //numVertices times
        int jointExtraSubversion;                       // 1
        float[][] color;                                //numJoints times
        int modelExtraSubversion;                       // 1
        ModelExtra modelExtra;

        public int NumberVertices { get { return numVertices; } }

        public int NumberFaces { get { return numTriangles; } }

        public int NumberGroups { get { return numGroups; } }

        public int NumberMaterials { get { return numMaterials; } }

        public int NumberJoints { get { return numJoints; } }

        public MS3D.Vertex[] VertexArray { get { return this.verts; } }

        public MS3D.VertexExtra[] VertexExtraArray { get { return this.vertexExtras; } }

        public MS3D.Face[] FaceArray { get { return this.faces; } }

        public MS3D.Joint[] JointArray { get { return this.joints; } }

        /// <summary>
        /// Returns the starting and ending vertex indices for the faces in a group.
        /// </summary>
        /// <param name="groupIndex">Index of the requested group.</param>
        /// <returns>Starting and ending vertex indices.</returns>
        public int[] GroupVertexRange(int groupIndex)
        {
            int minVert = int.MaxValue, maxVert = -1;
            foreach (Face f in this.faces)
            {
                if (f.GroupIndex != groupIndex) continue;
                foreach (ushort vertInd in f.VertexIndices)
                {
                    if (vertInd < minVert) minVert = vertInd;
                    if (vertInd > maxVert) maxVert = vertInd;
                }
            }
            return new int[] { minVert, maxVert };
        }
        /// <summary>
        /// Returns the number of vertices in the requested group.
        /// </summary>
        /// <param name="groupIndex">Index of the requested group.</param>
        /// <returns>Number of vertices in the group.</returns>
        public int GroupNumberOfVertices(int groupIndex)
        {
            int[] grpRange = this.GroupVertexRange(groupIndex);
            return grpRange[1] - grpRange[0] + 1;
        }

        public ushort[] GroupFaceIndices(int groupIndex)
        {
            return this.groups[groupIndex].FacesIndices;
        }

        public string[] GroupComments
        {
            get
            {
                string[] tmp = new string[numGroupComments];
                for (int i = 0; i < numGroupComments; i++)
                {
                    tmp[i] = this.groupComments[i].ToString();
                }
                return tmp;
            }
        }

        public Comment GetGroupComment(int index)
        {
            return this.groupComments[index];
        }


        public string JointsList
        {
            get
            {
                string tmp = "";
                for (int i = 0; i < numJoints; i++)
                {
                    tmp += i.ToString() + ". " + this.joints[i].ToString() + Environment.NewLine;
                    //tmp += i.ToString() + ". " + this.joints[i].ToString() + " Color: " + this.color[i][0].ToString() + ", " +
                    //    this.color[i][1].ToString() + ", " + this.color[i][2].ToString() + Environment.NewLine;
                }
                return tmp;
            }
        }

        public uint MaxVertexID
        {
            get
            {
                uint tmp = 0;
                foreach (VertexExtra v in vertexExtras)
                {
                    if (v.VertexID > tmp) tmp = v.VertexID;
                }
                return tmp;
            }
        }

        public Vertex getVertex(int vertexIndex)
        {
            return verts[vertexIndex];
        }

        public VertexExtra getVertexExtra(int vertexIndex)
        {
            return vertexExtras[vertexIndex];
        }

        public Face getFace(int faceIndex)
        {
            return faces[faceIndex];
        }

        public sbyte[] getBones(int vertexIndex)
        {
            sbyte[] tmp = new sbyte[4];
            tmp[0] = verts[vertexIndex].Bone0;
            if (vertexExtras.Length > vertexIndex)
            {
                tmp[1] = vertexExtras[vertexIndex].Bone1;
                tmp[2] = vertexExtras[vertexIndex].Bone2;
                tmp[3] = vertexExtras[vertexIndex].Bone3;
            }
            return tmp;
        }

        public byte[] getBoneWeights(int vertexIndex)
        {
            byte[] tmp = new byte[4];
            if (vertexExtras.Length > vertexIndex)
            {
                tmp[0] = vertexExtras[vertexIndex].Weight0;
                tmp[1] = vertexExtras[vertexIndex].Weight1;
                tmp[2] = vertexExtras[vertexIndex].Weight2;
                tmp[3] = vertexExtras[vertexIndex].Weight3;
            }
            else
            {
                tmp[0] = 100;
            }
            return tmp;
        }

        public string getGroupName(int groupIndex)
        {
            return groups[groupIndex].Name;
        }
        public string[] getGroupNames()
        {
            string[] tmp = new string[this.numGroups];
            for (int i = 0; i < this.numGroups; i++)
            {
                tmp[i] = groups[i].Name;
            }
            return tmp;
        }

        public MS3D(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            id = br.ReadChars(10);
            version = br.ReadInt32();
            numVertices = br.ReadUInt16();
            verts = new Vertex[numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                verts[i] = new Vertex(br);
            }
            numTriangles = br.ReadUInt16();
            faces = new Face[numTriangles];
            for (int i = 0; i < numTriangles; i++)
            {
                faces[i] = new Face(br);
            }
            numGroups = br.ReadUInt16();
            groups = new Group[numGroups];
            for (int i = 0; i < numGroups; i++)
            {
                groups[i] = new Group(br);
            }
            numMaterials = br.ReadUInt16();
            materials = new Material[numMaterials];
            for (int i = 0; i < numMaterials; i++)
            {
                materials[i] = new Material(br);
            }
            fAnimationFPS = br.ReadSingle();
            fCurrentTime = br.ReadSingle();
            iTotalFrames = br.ReadInt32();
            numJoints = br.ReadUInt16();
            joints = new Joint[numJoints];
            for (int i = 0; i < numJoints; i++)
            {
                joints[i] = new Joint(br);
            }
            try
            {
                commentSubversion = br.ReadInt32();
                if (commentSubversion > 0)
                {
                    numGroupComments = br.ReadUInt32();
                    groupComments = new Comment[numGroupComments];
                    for (int i = 0; i < numGroupComments; i++)
                    {
                        groupComments[i] = new Comment(br);
                    }
                    numMaterialComments = br.ReadUInt32();
                    materialComments = new Comment[numMaterialComments];
                    for (int i = 0; i < numMaterialComments; i++)
                    {
                        materialComments[i] = new Comment(br);
                    }
                    numJointComments = br.ReadUInt32();
                    jointComments = new Comment[numJointComments];
                    for (int i = 0; i < numJointComments; i++)
                    {
                        jointComments[i] = new Comment(br);
                    }
                    hasModelComment = br.ReadUInt32();
                    if (hasModelComment > 0)
                    {
                        modelComments = new Comment(br);
                    }
                }
                vertexExtraSubversion = br.ReadInt32();
                if (vertexExtraSubversion > 0)
                {
                    vertexExtras = new VertexExtra[numVertices];
                    for (int i = 0; i < numVertices; i++)
                    {
                        vertexExtras[i] = new VertexExtra(br, vertexExtraSubversion);
                    }
                }
                jointExtraSubversion = br.ReadInt32();
                if (jointExtraSubversion > 0)
                {
                    color = new float[numJoints][];
                    for (int i = 0; i < numJoints; i++)
                    {
                        color[i] = new float[3];
                        for (int j = 0; j < 3; j++)
                        {
                            color[i][j] = br.ReadSingle();
                        }
                    }
                }
                modelExtraSubversion = br.ReadInt32();
                if (modelExtraSubversion > 0)
                {
                    modelExtra = new ModelExtra(br);
                }
            }
            catch (EndOfStreamException)
            {
                if (groupComments == null) groupComments = new Comment[0];
                if (materialComments == null) materialComments = new Comment[0];
                if (jointComments == null) jointComments = new Comment[0];
                if (modelComments == null) modelComments = new Comment();
                if (vertexExtras == null) vertexExtras = new VertexExtra[0];
                if (color == null) color = new float[0][];
                if (modelExtra == null) modelExtra = new ModelExtra();
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(version);
            bw.Write(numVertices);
            for (int i = 0; i < numVertices; i++)
            {
                verts[i].Write(bw);
            }
            bw.Write(numTriangles);
            for (int i = 0; i < numTriangles; i++)
            {
                faces[i].Write(bw);
            }
            bw.Write(numGroups);
            for (int i = 0; i < numGroups; i++)
            {
                groups[i].Write(bw);
            }
            bw.Write(numMaterials);
            for (int i = 0; i < numMaterials; i++)
            {
                materials[i].Write(bw);
            }
            bw.Write(fAnimationFPS);
            bw.Write(fCurrentTime);
            bw.Write(iTotalFrames);
            bw.Write(numJoints);
            for (int i = 0; i < numJoints; i++)
            {
                joints[i].Write(bw);
            }
            bw.Write(commentSubversion);
            if (commentSubversion > 0)
            {
                bw.Write(numGroupComments);
                for (int i = 0; i < numGroupComments; i++)
                {
                    groupComments[i].Write(bw);
                }
                bw.Write(numMaterialComments);
                for (int i = 0; i < numMaterialComments; i++)
                {
                    materialComments[i].Write(bw);
                }
                bw.Write(numJointComments);
                for (int i = 0; i < numJointComments; i++)
                {
                    jointComments[i].Write(bw);
                }
                bw.Write(hasModelComment);
                if (hasModelComment > 0)
                {
                    modelComments.Write(bw);
                }
            }
            bw.Write(vertexExtraSubversion);
            if (vertexExtraSubversion > 0)
            {
                for (int i = 0; i < numVertices; i++)
                {
                    vertexExtras[i].Write(bw, vertexExtraSubversion);
                }
            }
            bw.Write(jointExtraSubversion);
            if (jointExtraSubversion > 0)
            {
                for (int i = 0; i < numJoints; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        bw.Write(color[i][j]);
                    }
                }
            }
            bw.Write(modelExtraSubversion);
            if (modelExtraSubversion > 0)
            {
                modelExtra.Write(bw);
            }
        }

        public MS3D(GEOM[] geomArray, RIG rig, int uvSet, string[] groupnames)
        {
            List<RIG.Bone> filterBones = new List<RIG.Bone>();
            foreach (RIG.Bone b in rig.Bones)
            {
                if (b.BoneName.IndexOf("_slot") >= 0 || b.BoneName.IndexOf("ExportPole") >= 0 ||
                    b.BoneName.IndexOf("Fur") >= 0 ||
                    (b.BoneName.IndexOf("CAS_") >= 0 && (b.BoneName.IndexOf("Lip") >= 0 || b.BoneName.IndexOf("Brow") >= 0))) continue;
                filterBones.Add(b);
            }
            RIG.Bone[] rigBones = filterBones.ToArray();

            id = new char[] { 'M', 'S', '3', 'D', '0', '0', '0', '0', '0', '0' };
            version = 4;
            List<int>[] groupTriangleIndexes = new List<int>[geomArray.Length];
            int tmpVertices = 0, tmpTriangles = 0;
            for (int i = 0; i < geomArray.Length; i++)
            {
                int tmp = geomArray[i].numberVertices;
                if (tmp > MAX_VERTICES) throw new MeshException("Mesh number " + i.ToString() + " has too many vertices and cannot be converted to MS3D!");
                tmpVertices += tmp;
                tmp = geomArray[i].numberFaces;
                if (tmp > MAX_TRIANGLES) throw new MeshException("Mesh number " + i.ToString() + " has too many triangles and cannot be converted to MS3D!");
                tmpTriangles += tmp;
                groupTriangleIndexes[i] = new List<int>();
            }
            if (tmpVertices > MAX_VERTICES) throw new MeshException("This set of meshes has too many vertices and cannot be converted to MS3D! You can convert each mesh separately.");
            numVertices = (ushort)tmpVertices;
            verts = new Vertex[numVertices];
            vertexExtras = new VertexExtra[numVertices];
            if (tmpTriangles > MAX_TRIANGLES) throw new MeshException("This set of meshes has too many triangles and cannot be converted to MS3D! You can convert each mesh separately.");
            numTriangles = (ushort)tmpTriangles;
            faces = new Face[numTriangles];

            int vertCounter = 0, faceCounter = 0, runningVertOffset = 0;
            for (int m = 0; m < geomArray.Length; m++)
            {
                for (int i = 0; i < geomArray[m].numberVertices; i++)
                {
                    byte tmp = geomArray[m].getBones(i)[0];
                    if (tmp < geomArray[m].numberBones)
                    {
                        verts[vertCounter] = new Vertex(geomArray[m].getPosition(i), TranslateBone(geomArray[m].BoneHashList[tmp], rigBones));
                    }
                    else
                    {
                        verts[vertCounter] = new Vertex(geomArray[m].getPosition(i), (sbyte)-1);
                    }

                    byte[] bones = geomArray[m].getBones(i);
                    sbyte[] bonesMS = new sbyte[3];
                    byte[] weights = geomArray[m].getBoneWeights(i);
                    byte[] weightsMS = new byte[3];
                    int totWeights = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        weightsMS[j] = (byte)(Math.Round((double)weights[j] / 255.0 * 100.0));
                        totWeights += weightsMS[j];
                    }
                    if (totWeights > 100) weightsMS[0] += (byte)(100 - totWeights);
                    else if (totWeights < 100 && weights[3] == 0) weightsMS[0] += (byte)(100 - totWeights);
                    //if (totWeights < 100)
                    //{
                    //    if (weightsMS[1] == 0 | weightsMS[2] == 0) weightsMS[0] += (byte)(100 - totWeights);
                    //}
                    for (int j = 1; j < 3; j++)
                    {
                        if (weightsMS[j] > 0 & bones[j] < geomArray[m].numberBones)
                        {
                            bonesMS[j - 1] = TranslateBone(geomArray[m].BoneHashList[bones[j]], rigBones);
                        }
                        else
                        {
                            bonesMS[j - 1] = (sbyte)-1;
                        }
                    }
                    if (100 - totWeights > 0 & bones[3] < geomArray[m].numberBones)
                    {
                        bonesMS[2] = TranslateBone(geomArray[m].BoneHashList[bones[3]], rigBones);
                    }
                    else
                    {
                        bonesMS[2] = (sbyte)-1;
                    }
                    if (geomArray[m].hasTags)
                    {
                        vertexExtraSubversion = 3;
                        int vertexID = geomArray[m].hasVertexIDs ? geomArray[m].getVertexID(i) : i;
                        vertexExtras[vertCounter] = new VertexExtra(bonesMS, weightsMS, new uint[] { (uint)vertexID, geomArray[m].getTagval(i) });
                    }
                    else
                    {
                        vertexExtraSubversion = 2;
                        int vertexID = geomArray[m].hasVertexIDs ? geomArray[m].getVertexID(i) : i;
                        vertexExtras[vertCounter] = new VertexExtra(bonesMS, weightsMS, new uint[] { (uint)vertexID });
                    }

                    vertCounter++;
                }

                for (int i = 0; i < geomArray[m].numberFaces; i++)
                {
                    int[] f = geomArray[m].getFaceIndices(i);
                    float[][] ntmp = new float[3][];
                    ntmp[0] = geomArray[m].getNormal(f[0]);
                    ntmp[1] = geomArray[m].getNormal(f[1]);
                    ntmp[2] = geomArray[m].getNormal(f[2]);
                    float[][] uvtmp = new float[3][];
                    if (geomArray[m].hasUVset(uvSet))
                    {
                        uvtmp[0] = geomArray[m].getUV(f[0], uvSet);
                        uvtmp[1] = geomArray[m].getUV(f[1], uvSet);
                        uvtmp[2] = geomArray[m].getUV(f[2], uvSet);
                        if (uvSet > 0)
                        {
                            uvtmp[0][0] = (uvtmp[0][0] + 1f) / 2f;
                            uvtmp[1][0] = (uvtmp[1][0] + 1f) / 2f;
                            uvtmp[2][0] = (uvtmp[2][0] + 1f) / 2f;
                        }
                    }
                    else
                    {
                        uvtmp[0] = new float[] { 0f, 0f };
                        uvtmp[1] = new float[] { 0f, 0f };
                        uvtmp[2] = new float[] { 0f, 0f };
                    }
                    f[0] += runningVertOffset;
                    f[1] += runningVertOffset;
                    f[2] += runningVertOffset;
                    faces[faceCounter] = new Face(f, ntmp, uvtmp, m);
                    groupTriangleIndexes[m].Add(faceCounter);
                    faceCounter++;
                }

                runningVertOffset = vertCounter;
            }

            for (int i = 0; i < numVertices; i++)               // set vertex reference counts
            {
                int refCount = 0;
                for (int j = 0; j < numTriangles; j++)
                {
                    ushort[] f = faces[j].VertexIndices;
                    for (int k = 0; k < 3; k++)
                    {
                        if (f[k] == i) refCount++;
                    }
                }
                verts[i].ReferenceCount = (byte)refCount;
            }

            numGroups = (ushort)geomArray.Length;
            groups = new Group[numGroups];
            commentSubversion = 1;
            numGroupComments = numGroups;
            groupComments = new Comment[numGroups];
            for (int m = 0; m < numGroups; m++)
            {
                groups[m] = new Group(groupnames[m], groupTriangleIndexes[m].ToArray());
                bool[] FVF = new bool[11];
                for (int i = 0; i < 11; i++) { FVF[i] = false; }
                foreach (GEOM.vertexForm vf in geomArray[m].vertexFormat)
                {
                    FVF[vf.datatype] = true;
                }
                int FVFitems = 0;
                foreach (bool b in FVF) { if (b) FVFitems++; }
                groupComments[m] = new Comment(m, FVFitems, geomArray[m].hasTags, 0, geomArray[m].TGIList, geomArray[m].Shader, geomArray[m].ShaderHash);
            }
            numMaterials = (ushort)0;
            materials = new Material[0];
            fAnimationFPS = 24f;
            fCurrentTime = 0f;
            iTotalFrames = 0;
            numJoints = (ushort)rigBones.Length;
            joints = new Joint[numJoints];
            for (int i = 0; i < numJoints; i++)
            {
                joints[i] = new Joint(rigBones[i].BoneName, rigBones[i].ParentName, rigBones[i].LocalRotation, rigBones[i].PositionVector);
            }

            numMaterialComments = 0;
            materialComments = new Comment[0];
            numJointComments = 0;
            jointComments = new Comment[0];
            hasModelComment = 0;
            modelComments = null;

            jointExtraSubversion = 1;
            color = new float[numJoints][];
            int ind = 0;
            for (float r = 1f; r >= 0f; r -= 0.2f)
            {
                for (float g = 1f; g >= 0f; g -= 0.2f)
                {
                    for (float b = 1f; b >= 0f; b -= 0.2f)
                    {
                        color[ind] = new float[] { r, g, b };
                        ind++;
                        if (ind >= numJoints) break;
                    }
                    if (ind >= numJoints) break;
                }
                if (ind >= numJoints) break;
            }
            modelExtraSubversion = 1;
            modelExtra = new ModelExtra();
        }

        public class Vertex
        {
            byte flags;                              // SELECTED | SELECTED2 | HIDDEN
            float[] position = new float[3];
            sbyte boneId;                             // -1 = no bone
            byte referenceCount;

            public byte Flags { get { return flags; } }

            public float[] Position { get { return position; } }

            public sbyte Bone0 { get { return boneId; } }

            public byte ReferenceCount
            {
                get { return this.referenceCount; }
                internal set { this.referenceCount = value; }
            }

            internal Vertex(float[] position, sbyte boneID)
            {
                this.flags = (byte)0;
                this.position = position;
                this.boneId = boneID;
                this.referenceCount = (byte)0;
            }

            internal Vertex(BinaryReader br)
            {
                flags = br.ReadByte();
                for (int i = 0; i < 3; i++)
                {
                    position[i] = br.ReadSingle();
                }
                boneId = br.ReadSByte();
                referenceCount = br.ReadByte();
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(flags);
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(position[i]);
                }
                bw.Write(boneId);
                bw.Write(referenceCount);
            }
        }

        public class Face
        {
            ushort flags;                                       // SELECTED | SELECTED2 | HIDDEN
            ushort[] vertexIndices = new ushort[3];
            float[][] vertexNormals = new float[3][];           // 3 x 3
            float[] s = new float[3];
            float[] t = new float[3];
            byte smoothingGroup;                             // 1 - 32
            byte groupIndex;

            public int Flags { get { return flags; } }

            public ushort[] VertexIndices { get { return vertexIndices; } }

            public float[][] VertexNormals { get { return vertexNormals; } }

            public float[] S { get { return s; } }

            public float[] T { get { return t; } }

            public byte SmoothingGroup { get { return smoothingGroup; } }

            public byte GroupIndex { get { return groupIndex; } }

            internal Face(int[] VertexIndices, float[][] VertexNormals, float[][] VertexUVs, int GroupIndex)
            {
                flags = (ushort)0;
                vertexIndices = new ushort[] { (ushort)VertexIndices[0], (ushort)VertexIndices[1], (ushort)VertexIndices[2] };
                vertexNormals = VertexNormals;
                s = new float[] { VertexUVs[0][0], VertexUVs[1][0], VertexUVs[2][0] };
                t = new float[] { VertexUVs[0][1], VertexUVs[1][1], VertexUVs[2][1] };
                smoothingGroup = (byte)0;
                groupIndex = (byte)GroupIndex;
            }

            internal Face(BinaryReader br)
            {
                flags = br.ReadUInt16();
                for (int i = 0; i < 3; i++)
                {
                    vertexIndices[i] = br.ReadUInt16();
                }
                for (int i = 0; i < 3; i++)
                {
                    vertexNormals[i] = new float[3];
                    for (int j = 0; j < 3; j++)
                    {
                        vertexNormals[i][j] = br.ReadSingle();
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    s[i] = br.ReadSingle();
                }
                for (int i = 0; i < 3; i++)
                {
                    t[i] = br.ReadSingle();
                }
                smoothingGroup = br.ReadByte();
                groupIndex = br.ReadByte();
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(flags);
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(vertexIndices[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        bw.Write(vertexNormals[i][j]);
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(s[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(t[i]);
                }
                bw.Write(smoothingGroup);
                bw.Write(groupIndex);
            }
        }

        public class Group
        {
            byte flags;                              // SELECTED | HIDDEN
            char[] name = new char[32];
            ushort numtriangles;
            ushort[] triangleIndices;                  // the groups group the triangles - numtriangles times
            sbyte materialIndex;                      // -1 = no material

            public byte Flags { get { return flags; } }

            public string Name { get { return TrimUnprintable(name); } }

            public int NumberFaces { get { return (int)numtriangles; } }

            public ushort[] FacesIndices
            {
                get
                {
                    ushort[] tmp = new ushort[this.triangleIndices.Length];
                    Array.Copy(this.triangleIndices, tmp, this.triangleIndices.Length);
                    return tmp;
                }
            }

            internal Group(string groupName, int[] triangleIndexArray)
            {
                flags = (byte)0;
                char[] tmp = groupName.ToCharArray();
                name = new char[32];
                int len = Math.Min(32, tmp.Length);
                for (int i = 0; i < len; i++)
                {
                    name[i] = tmp[i];
                }
                numtriangles = (ushort)triangleIndexArray.Length;
                triangleIndices = new ushort[numtriangles];
                for (int i = 0; i < numtriangles; i++)
                {
                    triangleIndices[i] = (ushort)triangleIndexArray[i];
                }
                materialIndex = (sbyte)-1;
            }

            internal Group(BinaryReader br)
            {
                flags = br.ReadByte();
                name = br.ReadChars(32);
                numtriangles = br.ReadUInt16();
                triangleIndices = new ushort[numtriangles];
                for (int i = 0; i < numtriangles; i++)
                {
                    triangleIndices[i] = br.ReadUInt16();
                }
                materialIndex = br.ReadSByte();
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(flags);
                bw.Write(name);
                bw.Write(numtriangles);
                for (int i = 0; i < numtriangles; i++)
                {
                    bw.Write(triangleIndices[i]);
                }
                bw.Write(materialIndex);
            }
        }

        internal class Material
        {
            char[] name = new char[32];
            float[] ambient = new float[4];
            float[] diffuse = new float[4];
            float[] specular = new float[4];
            float[] emissive = new float[4];
            float shininess;                          // 0.0f - 128.0f
            float transparency;                       // 0.0f - 1.0f
            byte mode;                               // 0, 1, 2 is unused now
            byte[] texture = new byte[128];            // texture.bmp
            byte[] alphamap = new byte[128];           // alpha.bmp

            internal Material(BinaryReader br)
            {
                name = br.ReadChars(32);
                for (int i = 0; i < 4; i++)
                {
                    ambient[i] = br.ReadSingle();
                }
                for (int i = 0; i < 4; i++)
                {
                    diffuse[i] = br.ReadSingle();
                }
                for (int i = 0; i < 4; i++)
                {
                    specular[i] = br.ReadSingle();
                }
                for (int i = 0; i < 4; i++)
                {
                    emissive[i] = br.ReadSingle();
                }
                shininess = br.ReadSingle();
                transparency = br.ReadSingle();
                mode = br.ReadByte();
                texture = br.ReadBytes(128);
                alphamap = br.ReadBytes(128);
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(name);
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(ambient[i]);
                }
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(diffuse[i]);
                }
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(specular[i]);
                }
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(emissive[i]);
                }
                bw.Write(shininess);
                bw.Write(transparency);
                bw.Write(mode);
                bw.Write(texture);
                bw.Write(alphamap);
            }
        }

        public class Joint
        {
            byte flags;                                         // SELECTED | DIRTY
            char[] name = new char[32];
            char[] parentName = new char[32];
            float[] rotation = new float[3];                        // local reference matrix
            float[] position = new float[3];
            ushort numKeyFramesRot;
            ushort numKeyFramesTrans;
            Keyframe[] keyFramesRot;      // local animation matrices - numKeyFramesRot times - time and angles
            Keyframe[] keyFramesTrans;    // local animation matrices - numKeyFramesTrans times - time and local position

            internal class Keyframe
            {
                float time;                                     // time in seconds
                float x, y, z;
                internal Keyframe(BinaryReader br)
                {
                    time = br.ReadSingle();
                    x = br.ReadSingle();
                    y = br.ReadSingle();
                    z = br.ReadSingle();
                }
                internal void Write(BinaryWriter bw)
                {
                    bw.Write(time);
                    bw.Write(x);
                    bw.Write(y);
                    bw.Write(z);
                }
            }

            public string JointName { get { return TrimUnprintable(this.name); } }

            internal Joint(string boneName, string parentName, Quaternion orientationQuaternion, Vector3 position)
            {
                flags = (byte)0;
                name = new char[32];
                char[] tmp = boneName.ToCharArray();
                for (int i = 0; i < tmp.Length; i++)
                {
                    name[i] = tmp[i];
                }
                this.parentName = new char[32];
                tmp = parentName.ToCharArray();
                for (int i = 0; i < tmp.Length; i++)
                {
                    this.parentName[i] = tmp[i];
                }
                float[] d = orientationQuaternion.toEuler().xyzRotation;
                this.rotation = new float[] { d[0], d[1], d[2] };
                this.position = position.Coordinates;
                numKeyFramesRot = (byte)0;
                numKeyFramesTrans = (byte)0;
                keyFramesRot = new Keyframe[0];
                keyFramesTrans = new Keyframe[0];
            }

            internal Joint(BinaryReader br)
            {
                flags = br.ReadByte();
                name = br.ReadChars(32);
                parentName = br.ReadChars(32);
                for (int i = 0; i < 3; i++)
                {
                    rotation[i] = br.ReadSingle();
                }
                for (int i = 0; i < 3; i++)
                {
                    position[i] = br.ReadSingle();
                }
                numKeyFramesRot = br.ReadUInt16();
                numKeyFramesTrans = br.ReadUInt16();
                keyFramesRot = new Keyframe[numKeyFramesRot];
                for (int i = 0; i < numKeyFramesRot; i++)
                {
                    keyFramesRot[i] = new Keyframe(br);
                }
                keyFramesTrans = new Keyframe[numKeyFramesTrans];
                for (int i = 0; i < numKeyFramesTrans; i++)
                {
                    keyFramesTrans[i] = new Keyframe(br);
                }
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(flags);
                bw.Write(name);
                bw.Write(parentName);
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(rotation[i]);
                }
                for (int i = 0; i < 3; i++)
                {
                    bw.Write(position[i]);
                }
                bw.Write(numKeyFramesRot);
                bw.Write(numKeyFramesTrans);
                for (int i = 0; i < numKeyFramesRot; i++)
                {
                    keyFramesRot[i].Write(bw);
                }
                keyFramesTrans = new Keyframe[numKeyFramesTrans];
                for (int i = 0; i < numKeyFramesTrans; i++)
                {
                    keyFramesTrans[i].Write(bw);
                }
            }

            public override string ToString()
            {
                return TrimUnprintable(this.name) + ": Parent " + TrimUnprintable(this.parentName) +
                        " Position=" + this.position[0].ToString() + "," + this.position[1].ToString() + "," + this.position[2] +
                        " Rotation=" + this.rotation[0].ToString() + "," + this.rotation[1].ToString() + "," + this.rotation[2];
            }
        }

        public class Comment
        {
            int index;						// index of group, material or joint
            //int commentLength;				// length of comment (terminating '\0' is not saved), "MC" has comment length of 2 (not 3)
            //char[] comment;                 // comment[commentLength]
            string comment;

            internal Comment()
            {
                index = 0;
                comment = "";
            }

            internal Comment(BinaryReader br)
            {
                index = br.ReadInt32();
                int commentLength = br.ReadInt32();
                char[] tmp = br.ReadChars(commentLength);
                comment = new string(tmp);
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(index);
                bw.Write(comment.Length);
                bw.Write(comment.ToCharArray());
            }

            internal Comment(int index, int vertexFormat, bool hasTagval, int tableType, TGI[] tgiList, GEOM.MTNF shader, uint shaderHash)
            {
                this.index = index;
                comment = "FVFItems: " + vertexFormat.ToString() + Environment.NewLine +
                    "HasTagVal: " + (hasTagval ? "1" : "0") + Environment.NewLine +
                    "TableType: " + tableType.ToString() + Environment.NewLine +
                    "References: " + tgiList.Length.ToString() + Environment.NewLine;
                for (int i = 0; i < tgiList.Length; i++)
                {
                    comment += "TGIRef" + i.ToString("D2") + ": " + tgiList[i].ToString().Remove(0, 2).Replace("-0x", " ") + Environment.NewLine;
                }
                comment += "EmbeddedType: " + shaderHash.ToString("X8") + Environment.NewLine +
                    "EmbeddedSize: " + shader.chunkSize.ToString() + Environment.NewLine;
                uint[] longArray = shader.toDataArray();
                for (int i = 0; i < longArray.Length; i++)
                {
                    comment += "EmbeddedLong" + i.ToString("D3") + ": " + longArray[i].ToString("X8") + Environment.NewLine;
                }
            }

            public int VertexFormat
            {
                get
                {
                    int val;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("FVFItems: ") + 10, 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            public bool HasTagValue
            {
                get
                {
                    int val;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("HasTagVal: ") + 11, 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out val))
                    {
                        return (val == 1);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public int TableType
            {
                get
                {
                    int val;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("TableType: ") + 11, 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            public TGI[] TGIlist
            {
                get
                {
                    //MessageBox.Show(this.comment.Substring(this.comment.IndexOf("References: ") + 12, 1));
                    int len;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("References: ") + 12, 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out len))
                    {
                        List<TGI> tlist = new List<TGI>();
                        for (int i = 0; i < len; i++)
                        {
                            //MessageBox.Show(this.comment.Substring(this.comment.IndexOf("TGIRef" + i.ToString("D2") + ": " + 10, 34)));
                            tlist.Add(new TGI(this.comment.Substring(this.comment.IndexOf("TGIRef" + i.ToString("D2") + ": ") + 10, 34)));
                        }
                        return tlist.ToArray();
                    }
                    return null;
                }
            }

            public uint ShaderHash
            {
                get
                {
                    try
                    {
                        uint val = UInt32.Parse(this.comment.Substring(this.comment.IndexOf("EmbeddedType: ") + 14, 8), System.Globalization.NumberStyles.HexNumber);
                        return val;
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }

            public uint[] ShaderDataArray
            {
                get
                {
                    int len;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("EmbeddedSize: ") + 14, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out len))
                    {
                        List<uint> slist = new List<uint>();
                        len = len / 4;
                        for (int i = 0; i < len; i++)
                        {
                            try
                            {
                                slist.Add(UInt32.Parse(this.comment.Substring(this.comment.IndexOf("EmbeddedLong" + i.ToString("D3") + ": ") + 17, 8),
                                    System.Globalization.NumberStyles.HexNumber));
                            }
                            catch
                            {
                                return null;
                            }
                        }
                        return slist.ToArray();
                    }
                    return null;
                }
            }

            public int Region
            {
                get
                {
                    int val;
                    if (Int32.TryParse(this.comment.Substring(this.comment.IndexOf("Region: ") + 8, 2), NumberStyles.Integer , CultureInfo.InvariantCulture, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            public override string ToString()
            {
                return this.comment;
            }
        }

        // ms3d_vertex_ex_t for subVersion == 1
        public class VertexExtra
        {
            sbyte[] boneIds;				// [3] - index of joint or -1, if -1, then that weight is ignored, since subVersion 1
            byte[] weights;				    // [3] - vertex weight ranging from 0 - 100, last weight is computed by 100 - sum(all weights), since subVersion 1
            // weight[0] is the weight for boneId in ms3d_vertex_t
            // weight[1] is the weight for boneIds[0]
            // weight[2] is the weight for boneIds[1]
            // 1.0f - weight[0] - weight[1] - weight[2] is the weight for boneIds[2]
            uint[] extra;					// vertex extra, which can be used as color or anything else. subVersion 2: [1], subVersion 3: [2]
            // extra[0] = vertex ID, extra[1] = vertex color/tagval

            public sbyte Bone1 { get { return boneIds[0]; } }
            public sbyte Bone2 { get { return boneIds[1]; } }
            public sbyte Bone3 { get { return boneIds[2]; } }

            public byte Weight0 { get { return weights[0]; } }
            public byte Weight1 { get { return weights[1]; } }
            public byte Weight2 { get { return weights[2]; } }
            public byte Weight3 { get { return (byte)(100 - (weights[0] + weights[1] + weights[2])); } }

            public uint VertexID { get { return extra[0]; } }

            public bool hasVertexColor { get { return (extra.Length == 2); } }
            public uint VertexColor { get { return extra[1]; } }

            internal VertexExtra(BinaryReader br, int version)
            {
                boneIds = new sbyte[3];
                boneIds[0] = br.ReadSByte();
                boneIds[1] = br.ReadSByte();
                boneIds[2] = br.ReadSByte();
                weights = br.ReadBytes(3);
                if (version == 2)
                {
                    extra = new uint[1];
                    extra[0] = br.ReadUInt32();
                }
                else if (version == 3)
                {
                    extra = new uint[2];
                    extra[0] = br.ReadUInt32();
                    extra[1] = br.ReadUInt32();
                }
            }

            internal VertexExtra(sbyte[] bones, byte[] boneWeights, uint[] extraInfo)
            {
                boneIds = bones;
                weights = boneWeights;
                extra = extraInfo;
            }

            internal void Write(BinaryWriter bw, int version)
            {
                bw.Write(boneIds[0]);
                bw.Write(boneIds[1]);
                bw.Write(boneIds[2]);
                bw.Write(weights);
                if (version == 2)
                {
                    bw.Write(extra[0]);
                }
                else if (version == 3)
                {
                    bw.Write(extra[0]);
                    bw.Write(extra[1]);
                }
            }
        }

        internal class ModelExtra
        {
            float jointSize;	    // joint size, since subVersion == 1
            int transparencyMode;   // 0 = simple, 1 = depth buffered with alpha ref, 2 = depth sorted triangles, since subVersion == 1
            float alphaRef;         // alpha reference value for transparencyMode = 1, since subVersion == 1

            internal ModelExtra(BinaryReader br)
            {
                jointSize = br.ReadSingle();
                transparencyMode = br.ReadInt32();
                alphaRef = br.ReadSingle();
            }

            internal ModelExtra()
            {
                jointSize = 0.012f;
                transparencyMode = 1;
                alphaRef = 0.5f;
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(jointSize);
                bw.Write(transparencyMode);
                bw.Write(alphaRef);
            }
        }

        public static string TrimUnprintable(char[] str)
        {
            int begin = 0, end = str.Length - 1;
            bool gotbegin = false, gotend = false;

            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsLetterOrDigit(str[i]) | Char.IsPunctuation(str[i]) | Char.IsSeparator(str[i]))
                {
                    begin = i;
                    gotbegin = true;
                    break;
                }
            }
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (Char.IsLetterOrDigit(str[i]) | Char.IsPunctuation(str[i]) | Char.IsSeparator(str[i]))
                {
                    end = i;
                    gotend = true;
                    break;
                }
            }
            if (gotbegin & gotend)
            {
                return new string(str, begin, end - begin + 1);
            }
            else
            {
                return "(none)";
            }
        }

        internal sbyte TranslateBone(uint boneHash, RIG.Bone[] rigBones)
        {
            for (int i = 0; i < rigBones.Length; i++)
            {
                if (boneHash == rigBones[i].BoneHash) return (sbyte)i;
            }
            return (sbyte)-1;
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
    }
}
