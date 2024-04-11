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
using System.Globalization;
using System.Linq;

namespace TS4SimRipper
{
    public class OBJ
    {
        List<Position> positionList;
        List<UV> uvList;
        List<Normal> normalList;
        List<Group> groupList;
        List<int> idList;

        public OBJ.Position[] positionArray
        {
            get { return this.positionList.ToArray(); }
            set { this.positionList = new List<Position>(value); }
        }

        public OBJ.UV[] uvArray
        {
            get { return this.uvList.ToArray(); }
            set { this.uvList = new List<UV>(value); }
        }

        public OBJ.Normal[] normalArray
        {
            get { return this.normalList.ToArray(); }
            set { this.normalList = new List<Normal>(value); }
        }

        public bool hasIDs
        {
            get { return (this.idList != null & this.idList.Count > 0); }
        }

        public int numberGroups
        {
            get { return this.groupList.Count; }
        }

        public OBJ.Group[] groupArray
        {
            get { return this.groupList.ToArray(); }
        }

        public string getGroupName(int groupIndex)
        {
            return this.groupList[groupIndex].groupName;
        }

        public string[] getGroupNames()
        {
            string[] tmp = new string[this.numberGroups];
            for (int i = 0; i < this.numberGroups; i++)
            {
                tmp[i] = this.groupList[i].groupName;
            }
            return tmp;
        }

        public int totalFaces
        {
            get
            {
                int tot = 0;
                foreach (OBJ.Group g in this.groupArray)
                {
                    tot += g.numberFaces;
                }
                return tot;
            }
        }

        public Face[] FaceArray
        {
            get
            {
                List<Face> tmp = new List<Face>();
                foreach (OBJ.Group g in this.groupArray)
                {
                    tmp.AddRange(g.facesList);
                }
                return tmp.ToArray();
            }
        }

        public int[] GroupFacePositionIndices(int group)
        {
            List<int> tmp = new List<int>();
            foreach (Face f in groupArray[group].facesList)
            {
                tmp.AddRange(f.facePositions);
            }
            return tmp.ToArray();
        }

        public int[] GroupFaceNormalsIndices(int group)
        {
            List<int> tmp = new List<int>();
            foreach (Face f in groupArray[group].facesList)
            {
                tmp.AddRange(f.faceNormals);
            }
            return tmp.ToArray();
        }

        public Point[] GroupPointArray(int group)
        {
            List<Point> tmp = new List<Point>();
            OBJ.Group g = this.groupArray[group];
            foreach (OBJ.Face f in g.facesList)
            {
                if (this.idList != null && this.idList.Count > 0)
                {
                    OBJ.Point v = new Point(f.p1[0] - 1, group, this.positionArray[f.p1[0] - 1].Coordinates,
                        this.uvArray[f.p1[1] - 1].Coordinates, this.normalArray[f.p1[2] - 1].Coordinates, this.idList[f.p1[0] - 1]);
                    if (!tmp.Contains(v)) tmp.Add(v);
                    v = new Point(f.p2[0] - 1, group, this.positionArray[f.p2[0] - 1].Coordinates,
                       this.uvArray[f.p2[1] - 1].Coordinates, this.normalArray[f.p2[2] - 1].Coordinates, this.idList[f.p2[0] - 1]);
                    if (!tmp.Contains(v)) tmp.Add(v);
                    v = new Point(f.p3[0] - 1, group, this.positionArray[f.p3[0] - 1].Coordinates,
                        this.uvArray[f.p3[1] - 1].Coordinates, this.normalArray[f.p3[2] - 1].Coordinates, this.idList[f.p3[0] - 1]);
                    if (!tmp.Contains(v)) tmp.Add(v);
                }
                else
                {
                    OBJ.Point v = new Point(f.p1[0] - 1, group, this.positionArray[f.p1[0] - 1].Coordinates,
                        this.uvArray[f.p1[1] - 1].Coordinates, this.normalArray[f.p1[2] - 1].Coordinates);
                    if (!tmp.Contains(v)) tmp.Add(v);
                    v = new Point(f.p2[0] - 1, group, this.positionArray[f.p2[0] - 1].Coordinates,
                        this.uvArray[f.p2[1] - 1].Coordinates, this.normalArray[f.p2[2] - 1].Coordinates);
                    if (!tmp.Contains(v)) tmp.Add(v);
                    v = new Point(f.p3[0] - 1, group, this.positionArray[f.p3[0] - 1].Coordinates,
                        this.uvArray[f.p3[1] - 1].Coordinates, this.normalArray[f.p3[2] - 1].Coordinates);
                    if (!tmp.Contains(v)) tmp.Add(v);
                }
            }
            tmp.Sort();
            return tmp.ToArray();
        }
        
        public Point[] PointArray
        {
            get
            {
                List<Point> tmp = new List<Point>();
                for (int i = 0; i < this.groupArray.Length; i++)
                {
                    tmp.AddRange(this.GroupPointArray(i));
                }
                return tmp.ToArray();
            }
        }

        public OBJ() { }

        public OBJ(StreamReader sr)
        {
            sr.BaseStream.Position = 0;
            positionList = new List<Position>();
            uvList = new List<UV>();
            normalList = new List<Normal>();
            groupList = new List<Group>();
            idList = new List<int>();
            string str;
            string[] sep = new string[] { " " };
            string[] slash = new string[] { "/" };
            int grpInd = 0;
            while ((str = sr.ReadLine()) != null)
            {
                if (str.StartsWith("v "))
                {
                    string[] v = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        positionList.Add(new Position(float.Parse(v[1], CultureInfo.InvariantCulture),
                            float.Parse(v[2], CultureInfo.InvariantCulture), float.Parse(v[3], CultureInfo.InvariantCulture)));
                    }
                    catch
                    {
                        DialogResult res = MessageBox.Show("Error in .obj text: " + str + System.Environment.NewLine +
                            "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.Cancel) return;
                        positionList.Add(new Position(0f, 0f, 0f));
                    }
                }
                else if (str.StartsWith("vt "))
                {
                    string[] t = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        uvList.Add(new UV(float.Parse(t[1], CultureInfo.InvariantCulture), float.Parse(t[2], CultureInfo.InvariantCulture)));
                    }
                    catch
                    {
                        DialogResult res = MessageBox.Show("Error in .obj text: " + str + System.Environment.NewLine +
                            "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.Cancel) return;
                        uvList.Add(new UV(0f, 0f));
                    }
                }
                else if (str.StartsWith("vn "))
                {
                    string[] n = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        normalList.Add(new Normal(float.Parse(n[1], CultureInfo.InvariantCulture),
                            float.Parse(n[2], CultureInfo.InvariantCulture), float.Parse(n[3], CultureInfo.InvariantCulture)));
                    }
                    catch
                    {
                        DialogResult res = MessageBox.Show("Error in .obj text: " + str + System.Environment.NewLine +
                            "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.Cancel) return;
                        normalList.Add(new Normal(0f, 0f, 0f));
                    }
                }
                else if (str.StartsWith("#vid "))       //custom vertex id
                {
                    string[] n = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        idList.Add(int.Parse(n[1], CultureInfo.InvariantCulture));
                    }
                    catch
                    {
                        DialogResult res = MessageBox.Show("Error in .obj text: " + str + System.Environment.NewLine +
                            "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.Cancel) return;
                        idList.Add(0);
                    }
                }
                else if (str.StartsWith("f "))
                {
                    if (groupList.Count == 0)
                    {
                        groupList.Add(new Group());
                        grpInd = 0;
                    }
                    try
                    {
                        string[] f = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                        int[][] points = new int[4][];
                        for (int i = 1; i < f.Length; i++)
                        {
                            string[] ptmp = f[i].Split(slash, System.StringSplitOptions.None);
                            int[] itmp = new int[ptmp.Length];
                            for (int j = 0; j < itmp.Length; j++)
                            {
                                if (ptmp[j] == String.Empty)
                                {
                                    itmp[j] = 0;
                                }
                                else
                                {
                                    itmp[j] = Int32.Parse(ptmp[j], CultureInfo.InvariantCulture);
                                }
                                if (itmp[j] < 0)
                                {
                                    if (j == 0)
                                    {
                                        itmp[j] = positionList.Count + itmp[j];
                                    }
                                    else if (j == 1)
                                    {
                                        itmp[j] = uvList.Count + itmp[j];
                                    }
                                    else if (j == 2)
                                    {
                                        itmp[j] = normalList.Count + itmp[j];
                                    }
                                }
                            }
                            points[i - 1] = itmp;
                        }
                        groupList[grpInd].addFace(new Face(points[0], points[1], points[2]));
                        if (f.Length > 4)
                        {
                            groupList[grpInd].addFace(new Face(points[2], points[3], points[0]));
                        }
                    }
                    catch
                    {
                        DialogResult res = MessageBox.Show("Error in .obj text: " + str + System.Environment.NewLine +
                            "Skip and continue anyway?", "OBJ Error", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.Cancel) return;
                    }
                }
                else if (str.StartsWith("g ") || str.StartsWith("o "))
                {
                    string[] g = str.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                    if (g.Length > 1)
                    {
                        bool found = false;
                        for (int i = 0; i < groupList.Count; i++)
                        {
                            if (String.CompareOrdinal(groupList[i].groupName, g[1]) == 0)
                            {
                                grpInd = i;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            groupList.Add(new Group(g[1]));
                            grpInd = groupList.Count - 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert geom to obj
        /// </summary>
        /// <param name="geom">geom mesh</param>
        /// <param name="uvSet">uv set to transfer</param>
        /// <param name="includeVertexID">if true, write vertex IDs in comments</param>
        /// <param name="groupnames">base name for obj group(s)</param>
        public OBJ(GEOM[] geom, int uvSet, bool includeVertexID, string[] groupnames)
        {
            positionList = new List<Position>();
            normalList = new List<Normal>();
            uvList = new List<UV>();
            groupList = new List<Group>();
            idList = new List<int>();

            int vertOffset = 1;
            int groupNum = 0;
            for (int m = 0; m < geom.Length; m++)
            {
                if (includeVertexID)
                {
                    if (!geom[m].hasVertexIDs)
                    {
                        MessageBox.Show("Input GEOM does not have vertex IDs!");
                        return;
                    }
                }
                else if (!geom[m].hasUVset(uvSet))
                {
                    DialogResult res = MessageBox.Show("Input GEOM does not have UV set " + uvSet.ToString() + "! Continue using UV0?", "No UV" + uvSet.ToString(), MessageBoxButtons.OKCancel);
                    if (res == DialogResult.Cancel) return;
                    else uvSet = 0;
                }
                for (int i = 0; i < geom[m].numberVertices; i++)
                {
                    positionList.Add(new Position(geom[m].getPosition(i)));
                    normalList.Add(new Normal(geom[m].getNormal(i)));
                    float[] uv = geom[m].getUV(i, uvSet);
                    if (uvSet > 0) uv[0] = (uv[0] + 1f) / 2f;
                    uvList.Add(new UV(uv, true));
                    if (includeVertexID) idList.Add(geom[m].getVertexID(i));
                }
                this.groupList.Add(new Group(groupnames[m]));
                for (int i = 0; i < geom[m].numberFaces; i++)
                {
                    this.groupList[groupNum].addFace(new Face(geom[m].getFaceIndices(i), vertOffset));
                }
                vertOffset += geom[m].numberVertices;
                groupNum++;
            }
        }

        public void Write(StreamWriter sw)
        {
            if (idList == null) idList = new List<int>();
            sw.WriteLine("# Generated by XMODS on " + System.DateTime.Now.ToString("MM/dd/yyyy"));
            sw.WriteLine();
            foreach (Position v in positionList)
            {
                sw.WriteLine("v " + v.ToString());
            }
            sw.WriteLine();
            foreach (UV vt in uvList)
            {
                sw.WriteLine("vt " + vt.ToString());
            }
            sw.WriteLine();
            foreach (Normal vn in normalList)
            {
                sw.WriteLine("vn " + vn.ToString());
            }
            sw.WriteLine();
            foreach (int i in idList)
            {
                sw.WriteLine("#vid " + i.ToString());
            }
            sw.WriteLine();
            foreach (Group g in groupList)
            {
                sw.WriteLine("g " + g.groupName);
                foreach (Face f in g.facesList)
                {
                    sw.WriteLine("f " + f.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("# Group " + g.groupName + " Total Faces: " + g.numberFaces);
                sw.WriteLine();
            }
            sw.WriteLine("# Total groups: " + groupList.Count.ToString() + ", Total vertices: " + positionList.Count.ToString() +
                ", Total uvs: " + uvList.Count.ToString() + ", Total normals: " + normalList.Count.ToString() +
                ((idList != null & idList.Count > 0) ? ", Total IDs: " + idList.Count.ToString() : ""));

            sw.Flush();
        }

        public static OBJ DeltaOBJ(OBJ baseOBJ, OBJ morphOBJ)
        {
            OBJ delta = new OBJ();
            OBJ.Face[] baseFaces = baseOBJ.FaceArray;
            OBJ.Face[] morphFaces = morphOBJ.FaceArray;
            if (baseFaces.Length != morphFaces.Length) 
                throw new ApplicationException("The base and morph mesh number of faces do not match!");
            OBJ.Position[] pos = new Position[baseOBJ.positionArray.Length];
            for (int i = 0; i < baseOBJ.positionArray.Length; i++)
            {
                pos[i] = new Position(baseOBJ.positionArray[i].X - morphOBJ.positionArray[i].X,
                                        baseOBJ.positionArray[i].Y - morphOBJ.positionArray[i].Y,
                                        baseOBJ.positionArray[i].Z - morphOBJ.positionArray[i].Z);
            }
            delta.positionArray = pos;
            OBJ.UV[] uv = new UV[baseOBJ.uvArray.Length];
            for (int i = 0; i < baseOBJ.uvArray.Length; i++)
            {
                uv[i] = new UV(baseOBJ.uvArray[i].U, baseOBJ.uvArray[i].V);
            }
            delta.uvArray = uv;
            OBJ.Normal[] norm = new Normal[baseOBJ.normalArray.Length];
            if (baseOBJ.normalArray.Length != morphOBJ.normalArray.Length)
            {
                OBJ.Normal[] temp = new Normal[baseOBJ.normalArray.Length];
                for (int i = 0; i < baseFaces.Length; i++)
                {
                    if (!baseFaces[i].facePositionListSorted.SequenceEqual(morphFaces[i].facePositionListSorted))
                        throw new ApplicationException("The base and morph mesh faces do not match!");
                    foreach (int[] pt in morphFaces[i].facePoints)
                    {
                        temp[pt[0] - 1] = new Normal(morphOBJ.normalArray[pt[2] - 1]);
                    }
                }
                morphOBJ.normalArray = temp;
            }
            for (int i = 0; i < baseOBJ.normalArray.Length; i++)
            {
                norm[i] = new Normal(baseOBJ.normalArray[i].X - morphOBJ.normalArray[i].X,
                                        baseOBJ.normalArray[i].Y - morphOBJ.normalArray[i].Y,
                                        baseOBJ.normalArray[i].Z - morphOBJ.normalArray[i].Z);
            }
            delta.normalArray = norm;
            OBJ.Group[] grp = new Group[baseOBJ.groupArray.Length];
            for (int i = 0; i < baseOBJ.groupArray.Length; i++)
            {
                grp[i] = new Group();
                grp[i].facesList = new List<Face>(baseOBJ.groupArray[i].facesList);
            }
            delta.groupList = new List<Group>(grp);
            return delta;
        }

        public List<Triangle2D> FacesUV1(float mapWidth, float mapHeight)
        {
            List<Triangle2D> tmp = new List<Triangle2D>();
            foreach (OBJ.Face face in this.FaceArray)
            {
                Vector2[] points = new Vector2[3];
                bool drop = false;
                for (int i = 0; i < 3; i++)
                {
                    float x = (float)(((this.uvList[face.faceUVs[i] - 1].U * 2d) - 1d) * mapWidth);
                    if (x < 0f)
                    {
                        drop = true;
                        continue;
                    }
                    float y = (this.uvList[face.faceUVs[i] - 1].V) * mapHeight;
                    points[i] = new Vector2(x, y);
                }
                if (!drop) tmp.Add(new Triangle2D(points));
            }
            return tmp;
        }

        public List<GroupTriangle> TrianglesUV1(float mapWidth, float mapHeight)
        {
            List<GroupTriangle> tmp = new List<GroupTriangle>();
            for (int i = 0; i < this.groupArray.Length; i++)
            {
                foreach (OBJ.Face face in this.groupArray[i].facesList)
                {
                    Vector2[] points = new Vector2[3];
                    Vector3[] deltaPosition = new Vector3[3];
                    Vector3[] deltaNormals = new Vector3[3];
                    bool drop = false;
                    for (int j = 0; j < 3; j++)
                    {
                        float x = (float)(((this.uvList[face.faceUVs[j] - 1].U * 2d) - 1d) * mapWidth);
                        if (x < 0f)
                        {
                            drop = true;
                            continue;
                        }
                        float y = (this.uvList[face.faceUVs[j] - 1].V) * mapHeight;
                        points[j] = new Vector2(x, y);
                        deltaPosition[j] = this.positionList[face.facePositions[j] - 1].Vector;
                        deltaNormals[j] = this.normalList[face.faceNormals[j] - 1].Vector;
                    }
                    if (!drop) tmp.Add(new GroupTriangle(i, points, deltaPosition, deltaNormals));
                }
            }
            return tmp;
        }

        /// <summary>
        /// Adds custom ID numbers to an obj mesh. These are written as obj comments.
        /// </summary>
        /// <param name="startNumber">number to start with</param>
        /// <returns>next number in sequence</returns>
        public int AddIDnumbers(int startNumber)
        {
            this.idList = new List<int>();
            Face[] faces = this.FaceArray;
            int nextNumber = startNumber;
            for (int i = 0; i < this.positionList.Count; i++)
            {
                bool foundMatch = false;
                int match = 0;
                for (int j = 0; j < i; j++)
                {
                     if (this.positionList[i].Equals(this.positionList[j]))
                    {
                        Normal iNormal = null, jNormal = null;
                        bool iFound = false, jFound = false;
                        for (int f = 0; f < faces.Length; f++)
                        {
                            for (int v = 0; v < 3; v++)
                            {
                                if (faces[f].facePositions[v] == i)
                                {
                                    iNormal = this.normalList[faces[f].faceNormals[v]];
                                    iFound = true;
                                    break;
                                }
                                if (faces[f].facePositions[v] == j)
                                {
                                    jNormal = this.normalList[faces[f].faceNormals[v]];
                                    jFound = true;
                                    break;
                                }
                            }
                            if (iFound && jFound && iNormal.Equals(jNormal))
                            {
                                foundMatch = true;
                                break;
                            }
                        }
                        if (foundMatch)
                        {
                            match = j;
                            break;
                        }
                    }
                }
                if (foundMatch)
                {
                    idList.Add(idList[match]);
                }
                else
                {
                    idList.Add(nextNumber);
                    nextNumber++;
                }
            }
            return nextNumber;
        }

        public void Sort()
        {
            this.groupList.Sort();
        }

        public void AddEmptyUV()
        {
            this.uvList.Add(new UV(0f, 0f));
            for (int i = 0; i < this.numberGroups; i++)
            {
                for (int j = 0; j < this.groupList[i].numberFaces; j++)
                {
                    this.groupList[i].facesList[j].p1[1] = 1;
                    this.groupList[i].facesList[j].p2[1] = 1;
                    this.groupList[i].facesList[j].p3[1] = 1;
                }
            }
        }

        public void FlipUV(bool verticalFlip, bool horizontalFlip)
        {
            OBJ.UV[] tmp = new OBJ.UV[this.uvArray.Length];
            for (int i = 0; i < this.uvArray.Length; i++)
            {
                tmp[i] = new OBJ.UV(this.uvArray[i].Coordinates, verticalFlip, horizontalFlip);
            }
            this.uvArray = tmp;
        }

        public class Group : IComparable<Group>
        {
            string grpName;
            List<Face> faceList;
            public string groupName
            {
                get { return grpName; }
                set { this.grpName = value; }
            }
            public List<Face> facesList
            {
                get { return this.faceList; }
                set { this.faceList = value; }
            }
            public int numberFaces
            {
                get { return this.faceList.Count; }
            }
            internal Group()
            {
                grpName = "default";
                faceList = new List<Face>();
            }
            internal Group(string groupName)
            {
                grpName = groupName;
                faceList = new List<Face>();
            }
            internal void addFace(Face f)
            {
                faceList.Add(f);
            }

            public int CompareTo(Group other)
            {
                int thisLOD = -1, otherLOD = -1;
                if (this.groupName.Contains("lod0") || this.groupName.Contains("LOD0")) thisLOD = 0;
                else if (this.groupName.Contains("lod1") || this.groupName.Contains("LOD1")) thisLOD = 1;
                else if (this.groupName.Contains("lod2") || this.groupName.Contains("LOD2")) thisLOD = 2;
                else if (this.groupName.Contains("lod3") || this.groupName.Contains("LOD3")) thisLOD = 3;

                if (other.groupName.Contains("lod0") || other.groupName.Contains("LOD0")) otherLOD = 0;
                else if (other.groupName.Contains("lod1") || other.groupName.Contains("LOD1")) otherLOD = 1;
                else if (other.groupName.Contains("lod2") || other.groupName.Contains("LOD2")) otherLOD = 2;
                else if (other.groupName.Contains("lod3") || other.groupName.Contains("LOD3")) otherLOD = 3;

                return thisLOD.CompareTo(otherLOD);
            }
        }

        public class Position : IEquatable<Position>
        {
            float x, y, z;
            public float[] Coordinates
            {
                get { return new float[] { x, y, z }; }
            }
            public Vector3 Vector
            {
                get { return new Vector3(x, y, z); }
            }
            public float X
            {
                get { return this.x; }
            }
            public float Y
            {
                get { return this.y; }
            }
            public float Z
            {
                get { return this.z; }
            }
            internal Position() { }
            internal Position(Position other)
            {
                this.x = other.x;
                this.y = other.y;
                this.z = other.z;
            }
            internal Position(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            internal Position(float[] coordinates)
            {
                this.x = coordinates[0];
                this.y = coordinates[1];
                this.z = coordinates[2];
            }
            public override bool Equals(object other)
            {
                if (other is Position)
                {
                    return this.Equals((Position)other);
                }
                else
                {
                    return false;
                }
            }
            public bool Equals (Position other)
            {
                return (new Vector3(this.Coordinates).Equals(new Vector3(other.Coordinates)));
            }
            public override string ToString()
            {
                return this.x.ToString("N6", CultureInfo.InvariantCulture) + " " +
                    this.y.ToString("N6", CultureInfo.InvariantCulture) + " " +
                    this.z.ToString("N6", CultureInfo.InvariantCulture);
            }
        }

        public class UV : IEquatable<UV>
        {
            float x, y;
            public float[] Coordinates
            {
                get { return new float[] { x, y }; }
            }
            public Vector2 Vector
            {
                get { return new Vector2(x, y); }
            }
            public float U
            {
                get { return this.x; }
            }
            public float V
            {
                get { return this.y; }
            }
            internal UV() { }
            internal UV(UV other)
            {
                this.x = other.x;
                this.y = other.y;
            }
            internal UV(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
            internal UV(float[] coordinates, bool verticalFlip)
            {
                this.x = coordinates[0];
                if (verticalFlip)
                {
                    this.y = 1f - coordinates[1];
                }
                else
                {
                    this.y = coordinates[1];
                }
            }
            internal UV(float[] coordinates, bool verticalFlip, bool horizontalFlip)
            {
                if (horizontalFlip)
                {
                    this.x = 1f - coordinates[0];
                }
                else
                {
                    this.x = coordinates[0];
                }
                if (verticalFlip)
                {
                    this.y = 1f - coordinates[1];
                }
                else
                {
                    this.y = coordinates[1];
                }
            }
            public override bool Equals(object other)
            {
                if (other is UV)
                {
                    return this.Equals((UV)other);
                }
                else
                {
                    return false;
                }
            }
            public bool Equals(UV other)
            {
                return (new Vector2(this.Coordinates).Equals(new Vector2(other.Coordinates)));
            }

            public override string ToString()
            {
                return this.x.ToString("N6", CultureInfo.InvariantCulture) + " " +
                    this.y.ToString("N6", CultureInfo.InvariantCulture);
            }
        }

        public class Normal : IEquatable<Normal>
        {
            float x, y, z;
            public float[] Coordinates
            {
                get { return new float[] { x, y, z }; }
            }
            public Vector3 Vector
            {
                get { return new Vector3(x, y, z); }
            }
            public float X
            {
                get { return this.x; }
            }
            public float Y
            {
                get { return this.y; }
            }
            public float Z
            {
                get { return this.z; }
            }
            internal Normal() { }
            internal Normal(Normal other)
            {
                this.x = other.x;
                this.y = other.y;
                this.z = other.z;
            }
            internal Normal(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            internal Normal(float[] coordinates)
            {
                this.x = coordinates[0];
                this.y = coordinates[1];
                this.z = coordinates[2];
            }
            public override bool Equals(object other)
            {
                if (other is Normal)
                {
                    return this.Equals((Normal)other);
                }
                else
                {
                    return false;
                }
            }
            public bool Equals(Normal other)
            {
                return (new Vector3(this.Coordinates).Equals(new Vector3(other.Coordinates)));
            }

            public override string ToString()
            {
                return this.x.ToString("N6", CultureInfo.InvariantCulture) + " " +
                    this.y.ToString("N6", CultureInfo.InvariantCulture) + " " +
                    this.z.ToString("N6", CultureInfo.InvariantCulture);
            }
        }

        public class Face
        {
            internal int[] p1, p2, p3;

            internal int[] facePositions
            {
                get { return new int[] { p1[0], p2[0], p3[0] }; }
            }

            internal List<int> facePositionListSorted
            {
                get { List<int> tmp = new List<int> { p1[0], p2[0], p3[0] }; tmp.Sort(); return tmp; }
            }

            internal int[] faceUVs
            {
                get { return new int[] { p1[1], p2[1], p3[1] }; }
            }

            internal int[] faceNormals
            {
                get { return new int[] { p1[2], p2[2], p3[2] }; }
            }

            internal int[][] facePoints
            {
                get
                {
                    int[][] tmp = new int[3][];
                    tmp[0] = new int[3];
                    tmp[1] = new int[3];
                    tmp[2] = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        tmp[0][i] = p1[i];
                        tmp[1][i] = p2[i];
                        tmp[2][i] = p3[i];
                    }
                    return tmp;
                }
            }

            internal Face() { }
            internal Face(int[] point1, int[] point2, int[] point3)
            {
                p1 = new int[3] { 0, 0, 0 };
                p2 = new int[3] { 0, 0, 0 };
                p3 = new int[3] { 0, 0, 0 };
                for (int i = 0; i < 3; i++)
                {
                    if (i < point1.Length) p1[i] = point1[i];
                    if (i < point2.Length) p2[i] = point2[i];
                    if (i < point3.Length) p3[i] = point3[i];
                }
            }
            internal Face(int[] points, int offset)
            {
                p1 = new int[3] { 0, 0, 0 };
                p2 = new int[3] { 0, 0, 0 };
                p3 = new int[3] { 0, 0, 0 };
                for (int i = 0; i < 3; i++)
                {
                    p1[i] = points[0] + offset;
                    p2[i] = points[1] + offset;
                    p3[i] = points[2] + offset;
                }
            }
            public override string ToString()
            {
                return pointString(p1) + " " + pointString(p2) + " " + pointString(p3);
            }
            internal string pointString(int[] point)
            {
                string str = point[0].ToString();
                for (int i = 1; i < point.Length; i++)
                {
                    str += "/";
                    if (point[i] > 0) str += point[i].ToString();
                }
                return str;
            }
        }

        public class GroupTriangle : IEquatable<GroupTriangle>, IComparable<GroupTriangle>
        {
            int group;
            Vector2[] uv1;
            Triangle2D triangle;
            Vector3[] position;
            Vector3[] normal;

            public int Group
            {
                get { return this.group; }
            }
            public Triangle2D Triangle
            {
                get { return this.triangle; }
            }
            public Vector2[] UV1
            {
                get { return this.uv1; }
            }
            public Vector3[] Positions
            {
                get { return this.position; }
            }
            public Vector3[] Normals
            {
                get { return this.normal; }
            }

            public GroupTriangle(GroupTriangle other)
            {
                this.group = other.group;
                this.uv1 = new Vector2[3];
                this.position = new Vector3[3];
                this.normal = new Vector3[3];
                this.triangle = new Triangle2D(other.uv1);
                for (int i = 0; i < 3; i++)
                {
                    this.uv1[i] = new Vector2(other.uv1[i]);
                    this.position[i] = new Vector3(other.position[i]);
                    this.normal[i] = new Vector3(other.normal[i]);
                }
            }
            
            public GroupTriangle(int groupNum, Vector2[] UVpoints, Vector3[] positions, Vector3[] normals)
            {
                this.group = groupNum;
                this.uv1 = new Vector2[3];
                this.position = new Vector3[3];
                this.normal = new Vector3[3];
                this.triangle = new Triangle2D(UVpoints);
                for (int i = 0; i < 3; i++)
                {
                    this.uv1[i] = new Vector2(UVpoints[i]);
                    this.position[i] = new Vector3(positions[i]);
                    this.normal[i] = new Vector3(normals[i]);
                }
            }

            public override bool Equals(Object other)
            {
                if (other is GroupTriangle)
                {
                    GroupTriangle otherTri = other as GroupTriangle;
                    return Equals(otherTri);
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(GroupTriangle other)
            {
                if (other == null) return false;
                return this.group == other.group && Enumerable.SequenceEqual<Vector2>(this.uv1, other.uv1) &&
                                                    Enumerable.SequenceEqual<Vector3>(this.position, other.position) &&
                                                    Enumerable.SequenceEqual<Vector3>(this.normal, other.normal);
            }

            public int CompareTo(GroupTriangle other)
            {
                return this.group.CompareTo(other.group);
            }
        }

        public class Point : IEquatable<Point>, IComparable<Point>
        {
            int index;
            int group;
            Vector3 position;
            Vector2 uv;
            Vector3 normal;
            int id;

            public int Group
            {
                get { return this.group; }
            }
            public Vector3 Position
            {
                get { return this.position; }
            }
            public Vector2 UV
            {
                get { return this.uv; }
            }
            public Vector3 Normal
            {
                get { return this.normal; }
            }
            public int ID
            {
                get { return this.id; }
            }

            public Point(int index, int groupNum, Vector3 position, Vector2 uv, Vector3 normal)
            {
                this.index = index;
                this.group = groupNum;
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
            }

            public Point(int index, int groupNum, Vector3 position, Vector2 uv, Vector3 normal, int id)
            {
                this.index = index;
                this.group = groupNum;
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
                this.id = id;
            }

            public Point(int index, int groupNum, float[] position, float[] uv, float[] normal)
            {
                this.index = index;
                this.group = groupNum;
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
            }

            public Point(int index, int groupNum, float[] position, float[] uv, float[] normal, int id)
            {

                this.index = index;
                this.group = groupNum;
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
                this.id = id;
            }

            public override bool Equals(Object other)
            {
                if (other is Point)
                {
                    Point otherVert = other as Point;
                    return Equals(otherVert);
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(Point other)
            {
                if (other == null) return false;
                return this.index == other.index;
            }

            public int CompareTo(Point other)
            {
                return this.index.CompareTo(other.index);
            }
        }

        public class Vertex : IEquatable<Vertex>
        {
            Vector3 position;
            Vector2 uv;
            Vector3 normal;

            public Vector3 Position
            {
                get { return this.position; }
            }
            public Vector2 UV
            {
                get { return this.uv; }
            }
            public Vector3 Normal
            {
                get { return this.normal; }
            }

            public Vertex(int index, Vector3 position, Vector2 uv, Vector3 normal)
            {
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
            }

            public Vertex(int index, float[] position, float[] uv, float[] normal)
            {
                this.position = new Vector3(position);
                this.uv = new Vector2(uv);
                this.normal = new Vector3(normal);
            }

            public override bool Equals(Object other)
            {
                if (other is Vertex)
                {
                    Vertex otherVert = other as Vertex;
                    return Equals(otherVert);
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(Vertex other)
            {
                if (other == null) return false;
                return this.position.Equals(other.position) && this.uv.Equals(other.uv) && this.normal.Equals(other.normal);
            }
        }
            
        internal float[] addArrays(float[] v1, float[] v2)
        {
            float[] res = new float[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                res[i] = v1[i] + v2[i];
            }
            return res;
        }

        internal bool foundVert(int[] p, List<int[]> verts, out int vertInd, bool cleanModel)
        {
            for (int i = 0; i < verts.Count; i++)       //for each vertex
            {
                bool found = true;
                if (cleanModel)
                {
                    for (int k = 0; k < 3; k++)         //for position X, Y, Z
                    {
                        if (this.positionArray[p[0] - 1].Coordinates[k] != this.positionArray[verts[i][0] - 1].Coordinates[k])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        for (int k = 0; k < 2; k++)         //for UV X, Y
                        {
                            if (this.uvArray[p[1] - 1].Coordinates[k] != this.uvArray[verts[i][1] - 1].Coordinates[k])
                            {
                                found = false;
                                break;
                            }
                        }
                    }
                    if (found)
                    {
                        for (int k = 0; k < 3; k++)         //for normals X, Y, Z
                        {
                            if (this.normalArray[p[2] - 1].Coordinates[k] != this.normalArray[verts[i][2] - 1].Coordinates[k])
                            {
                                found = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < verts[i].Length; j++)
                    {
                        if (p[j] != verts[i][j]) found = false;
                    }
                }

                if (found)
                {
                    vertInd = i;
                    return true;
                }
            }
            vertInd = -1;
            return false;
        }
    }
}
