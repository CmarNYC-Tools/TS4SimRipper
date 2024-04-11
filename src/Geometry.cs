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
using System.Windows.Forms;
using System.Globalization;

// Parts adapted from article "A Vector Type for C#" by R. Potter on codeproject.com

namespace TS4SimRipper
{

    //                      VECTOR3

    public struct Vector3 : IEquatable<Vector3>
    {
        private float x, y, z;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public float[] Coordinates
        {
            get { return new float[] { x, y, z }; }
            set
            {
                x = value[0];
                y = value[1];
                z = value[2];
            }
        }

        public float Magnitude
        {
            get
            {
                double tmp = (x * x) + (y * y) + (z * z);
                return (float)Math.Sqrt(tmp);
            }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(float[] coordinates)
        {
            this.x = coordinates[0];
            this.y = coordinates[1];
            this.z = coordinates[2];
        }

        public Vector3(Vector3 vector)
        {
            this.x = vector.X;
            this.y = vector.Y;
            this.z = vector.Z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return
            (
               new Vector3
               (
                  v1.X + v2.X,
                  v1.Y + v2.Y,
                  v1.Z + v2.Z
               )
            );
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return
            (
               new Vector3
               (
                   v1.X - v2.X,
                   v1.Y - v2.Y,
                   v1.Z - v2.Z
               )
            );
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return
            (
               (AlmostEquals(v1.X, v2.X)) &&
               (AlmostEquals(v1.Y, v2.Y)) &&
               (AlmostEquals(v1.Z, v2.Z))
            );
        }

        public override bool Equals(object obj)
        {
            // Check object other is a Vector3 object
            if (obj is Vector3)
            {
                // Convert object to Vector3
                Vector3 otherVector = (Vector3)obj;

                // Check for equality
                return otherVector == this;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Vector3 obj)
        {
            return obj == this;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static Vector3 operator *(Vector3 v1, float s2)
        {
            return
            (
               new Vector3
               (
                  v1.X * s2,
                  v1.Y * s2,
                  v1.Z * s2
               )
            );
        }

        public static Vector3 operator *(float s1, Vector3 v2)
        {
            return v2 * s1;
        }

        public static Vector3 operator /(Vector3 v1, float s2)
        {
            return
            (
               new Vector3
               (
                  v1.X / s2,
                  v1.Y / s2,
                  v1.Z / s2
               )
            );
        }

        public static Vector3 Scale(Vector3 v1, Vector3 v2)
        {
            return
            (
                new Vector3
                (
                    v1.X * v2.X,
                    v1.Y * v2.Y,
                    v1.Z * v2.Z
                )
            );
        }

        public static Vector3 AbsoluteValue(Vector3 v)
        {
            return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
        }

        public Vector3 Scale(Vector3 scalingVector)
        {
            return Scale(this, scalingVector);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return
            (
               new Vector3
               (
                  v1.Y * v2.Z - v1.Z * v2.Y,
                  v1.Z * v2.X - v1.X * v2.Z,
                  v1.X * v2.Y - v1.Y * v2.X
               )
            );
        }

        public Vector3 Cross(Vector3 other)
        {
            return Cross(this, other);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return
            (
               v1.X * v2.X +
               v1.Y * v2.Y +
               v1.Z * v2.Z
            );
        }

        public float Dot(Vector3 other)
        {
            return Dot(this, other);
        }

        public static Vector3 Normalize(Vector3 v1)
        {
            // Check for divide by zero errors
            if (v1.Magnitude == 0)
            {
                return v1;
            }
            else
            {
                // find the inverse of the vector's magnitude
                float inverse = 1 / v1.Magnitude;
                return
                (
                   new Vector3
                   (
                      // multiply each component by the inverse of the magnitude
                      v1.X * inverse,
                      v1.Y * inverse,
                      v1.Z * inverse
                   )
                );
            }
        }

        public void Normalize()
        {
            Vector3 n = Normalize(this);
            this.x = n.x;
            this.y = n.y;
            this.z = n.z;
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return
            (
               (float)Math.Sqrt
               (
                   (v1.X - v2.X) * (v1.X - v2.X) +
                   (v1.Y - v2.Y) * (v1.Y - v2.Y) +
                   (v1.Z - v2.Z) * (v1.Z - v2.Z)
               )
            );
        }

        public float Distance(Vector3 other)
        {
            return Distance(this, other);
        }

        public static float Angle(Vector3 v1, Vector3 v2)
        {
            return
            (
               (float)Math.Acos
               (
                  Normalize(v1).Dot(Normalize(v2))
               )
            );
        }

        public float Angle(Vector3 other)
        {
            return Angle(this, other);
        }

        public static Vector3 Centroid(Vector3 P1, Vector3 P2, Vector3 P3)
        {
            return new Vector3((P1.x + P2.x + P3.x) / 3f, (P1.y + P2.y + P3.y) / 3f, (P1.z + P2.z + P3.z) / 3f);
        }

        public Vector3 ProjectToLine(Vector3 Point1, Vector3 Point2)
        {
            Vector3 tmp = Point2 - Point1;
            tmp.Normalize();
            Vector3 tmp2 = this - Point1;
            Vector3 tmp3 = Vector3.Dot(tmp, tmp2) * tmp;
            return new Vector3(Point1 + tmp3);
        }

        public bool Between(Vector3 Point1, Vector3 Point2)
        {
            float min = Math.Min(Point1.X, Point2.X);
            float max = Math.Max(Point1.X, Point2.X);
            if (min > this.X | this.X > max)
            {
                return false;
            }
            min = Math.Min(Point1.Y, Point2.Y);
            max = Math.Max(Point1.Y, Point2.Y);
            if (min > this.Y | this.Y > max)
            {
                return false;
            }
            min = Math.Min(Point1.Z, Point2.Z);
            max = Math.Max(Point1.Z, Point2.Z);
            if (min > this.Z | this.Z > max)
            {
                return false;
            }
            return true;
        }

        public float[] GetInterpolationWeights(Vector3[] points, float weightingFactor)
        {
            float[] weights = new float[points.Length];

            if (points.Length == 1)
            {
                weights[0] = 1f;
                return weights;
            }
            for (int i = 0; i < points.Length; i++)
            {
                if (Vector3.Distance(points[i], this) == 0f)
                {
                    weights[i] = 1f;
                    return weights;
                }
            }

            float[] d = new float[points.Length];
            float dt = 0;
            for (int i = 0; i < points.Length; i++)
            {
                d[i] = 1f / (float)Math.Pow(Vector3.Distance(points[i], this), weightingFactor);
                dt += d[i];
            }

            for (int i = 0; i < points.Length; i++)
            {
                weights[i] = d[i] / dt;
            }
            //string a = "Distance: ";
            //string b = "Weights: ";
            //string x = "Positions: ";
            //for (int i = 0; i < weights.Length; i++)
            //{
            //    a += d[i].ToString() + ", ";
            //    b += weights[i].ToString() + ", ";
            //    x += points[i].x.ToString() + "," + points[i].y.ToString() + "," + points[i].z.ToString() + ", ";
            //}
            //MessageBox.Show(a + System.Environment.NewLine + b + System.Environment.NewLine + x);
            return weights;
        }

        public int NearestPointIndexSimple(Vector3[] RefPointsArray)
        {
            float minDistance = float.MaxValue;
            int ind = 0;
            for (int i = 0; i < RefPointsArray.Length; i++)
            {
                if (this.Distance(RefPointsArray[i]) < minDistance)
                {
                    minDistance = this.Distance(RefPointsArray[i]);
                    ind = i;
                }
            }
            return ind;
        }

        public Vector3 NearestPointSimple(Vector3[] RefPointsArray)
        {
            int ind = this.NearestPointIndexSimple(RefPointsArray);
            return RefPointsArray[ind];
        }

        public int NearestPointIndex(Vector3 thisFacesCentroid, Vector3[] RefPointsArray, Vector3[] refPointsFacesCentroids)
        {
            float minDistance = float.MaxValue;
            List<int> workingIndexes = new List<int>();
            for (int i = 0; i < RefPointsArray.Length; i++)
            {
                if (this.Distance(RefPointsArray[i]) < minDistance)
                {
                    workingIndexes.Clear();
                    workingIndexes.Add(i);
                    minDistance = this.Distance(RefPointsArray[i]);
                }
                else if (this.Distance(RefPointsArray[i]) == minDistance)
                {
                    workingIndexes.Add(i);
                }
            }
            float minFaceDistance = float.MaxValue;
            int ind = 0;
            for (int i = 0; i < workingIndexes.Count; i++)
            {
                if (thisFacesCentroid.Distance(refPointsFacesCentroids[workingIndexes[i]]) < minFaceDistance)
                {
                    ind = workingIndexes[i];
                    minFaceDistance = thisFacesCentroid.Distance(refPointsFacesCentroids[workingIndexes[i]]);
                }
            }
            return ind;
        }

        public Vector3 NearestPoint(Vector3 thisFacesCentroid, Vector3[] RefPointsArray, Vector3[] refPointsFacesCentroids)
        {
            int ind = this.NearestPointIndex(thisFacesCentroid, RefPointsArray, refPointsFacesCentroids);
            return RefPointsArray[ind];
        }

        internal int ArrayMinimumIndex(float[] array)
        {
            int tmp = -1;
            float tmpVal = float.MaxValue;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < tmpVal) tmp = i;
            }
            return tmp;
        }

        public override int GetHashCode()
        {
            return
            (
               (int)((X + Y + Z) % Int32.MaxValue)
            );
        }

        public override string ToString()
        {
            return this.X.ToString() + ", " + this.Y.ToString() + ", " + this.Z.ToString();
        }

        public static Vector3 Parse(string coordinateString)
        {
            string[] coordsStr = coordinateString.Split(new char[] { ',' });
            if (coordsStr.Length != 3) throw new FormatException("Input not in correct format");
            float[] coords = new float[3];
            for (int i = 0; i < 3; i++)
            {
                if (!float.TryParse(coordsStr[i], NumberStyles.Float, CultureInfo.InvariantCulture, out coords[i])) throw new FormatException("Input not in correct format");
            }
            return new Vector3(coords);
        }

        internal static bool AlmostEquals(float f1, float f2)
        {
            const float EPSILON = 0.00005f;
            return (Math.Abs(f1 - f2) < EPSILON);
        }

        internal bool positionMatches(float[] other)
        {
            return this.positionMatches(new Vector3(other));
        }
        internal bool positionMatches(float x, float y, float z)
        {
            return this.positionMatches(new Vector3(x, y, z));
        }
        internal bool positionMatches(Vector3 other)
        {
            const float EPSILON = 0.0005f;
            if (Math.Abs(this.x - other.x) < EPSILON && Math.Abs(this.y - other.y) < EPSILON && Math.Abs(this.z - other.z) < EPSILON) return true;
            return false;
        }

        internal bool positionClose(float[] other)
        {
            return this.positionClose(new Vector3(other));
        }
        internal bool positionClose(Vector3 other)
        {
            const float EPSILON = 0.005f;
            if (Math.Abs(this.x - other.x) < EPSILON && Math.Abs(this.y - other.y) < EPSILON && Math.Abs(this.z - other.z) < EPSILON) return true;
            return false;
        }
    }

    // VECTOR2

    public struct Vector2
    {
        private float x, y;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float[] Coordinates
        {
            get { return new float[] { x, y }; }
            set
            {
                x = value[0];
                y = value[1];
            }
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(float[] coordinates)
        {
            this.x = coordinates[0];
            this.y = coordinates[1];
        }

        public Vector2(Vector2 vector)
        {
            this.x = vector.X;
            this.y = vector.Y;
        }

        public double Magnitude
        {
            get { return Math.Sqrt((this.x * this.x) + (this.y * this.y)); }
        }

        public float Distance(Vector2 other)
        {
            double deltaX = this.x - other.x;
            double deltaY = this.y - other.y;
            return (float)Math.Sqrt(Math.Pow(deltaX, 2d) + Math.Pow(deltaY, 2d));
        }

        public bool CloseTo(Vector2 v2, float epsilon)
        {
            return
            (
               (Math.Abs(this.X - v2.X) < epsilon) &&
               (Math.Abs(this.Y - v2.Y) < epsilon)
            );
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            const float EPSILON = 0.00005f;
            return
            (
               (Math.Abs(v1.X - v2.X) < EPSILON) &&
               (Math.Abs(v1.Y - v2.Y) < EPSILON)
            );
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                Vector2 other = (Vector2)obj;
                return (other == this);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Vector2 obj)
        {
            return obj == this;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator *(Vector2 v1, float m2)
        {
            return new Vector2(v1.x * m2, v1.y * m2);
        }

        public static Vector2 operator *(float m1, Vector2 v2)
        {
            return v2 * m1;
        }

        public static Vector2 Normalize(Vector2 v1)
        {
            // Check for divide by zero errors
            if (v1.Magnitude == 0)
            {
                throw new DivideByZeroException("Cannot normalize a vector with magnitude of zero!");
            }
            else
            {
                // find the inverse of the vector's magnitude
                float inverse = (float)(1f / v1.Magnitude);
                return new Vector2(v1.X * inverse, v1.Y * inverse);
            }
        }

        public override int GetHashCode()
        {
            return
            (
               (int)((X + Y) % Int32.MaxValue)
            );
        }

        public override string ToString()
        {
            return this.X.ToString() + ", " + this.Y.ToString();
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y);
        }

        public float Dot(Vector2 other)
        {
            return Dot(this, other);
        }

        public bool DistanceFromLineRestricted(Vector2 P1, Vector2 P2, out float distance, out int endpointIndex)
        //returns whether point project to line segment falls within endpoints, 
        //distance = distance point to projected point or to nearest endpoint,
        //endPontIndex = 0 if nearest endpoint is P1 or 1 if P2
        {
            Vector2 tmp = this - P1;
            Vector2 line = P2 - P1;

            float lineLenSq = (line.x * line.x) + (line.y * line.y);
            float distanceOnSegment = Vector2.Dot(tmp, line) / lineLenSq;

            endpointIndex = 0;
            if (distanceOnSegment >= 0f && distanceOnSegment <= 1f)         //within segment
            {
                Vector2 projectedPoint = P1 + (line * distanceOnSegment);
                distance = this.Distance(projectedPoint);
                return true;
            }
            else
            {
                if (distanceOnSegment > 1f) endpointIndex = 1;
                distance = this.Distance(endpointIndex == 0 ? P1 : P2);
                return false;
            }
        }

        public float DistanceFromLine(Vector2 P1, Vector2 P2)
        {
            return Math.Abs(((P2.y - P1.y) * this.x) - ((P2.x - P1.x) * this.y) + (P2.x * P1.y) - (P2.y * P1.x)) / P1.Distance(P2);
        }

        public Vector2 ProjectToLine(Vector2 A, Vector2 B, out bool withinLine)
        {
            Vector2 AP = this - A;       //Vector from A to P   
            Vector2 AB = B - A;       //Vector from A to B  

            float magnitudeAB = (AB.x * AB.x) + (AB.y * AB.y);     //Magnitude of AB vector (its length squared)
            float ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
            float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

            withinLine = (distance >= 0f && distance <= 1f);
            return A + AB * distance;
        }

        public bool ProjectsWithinLine(Vector2 v1, Vector2 v2)
        {
            bool tmp;
            this.ProjectToLine(v1, v2, out tmp);
            return tmp;
        }
    }

    // Triangles

    public struct Triangle2D
    {
        private Vector2 p1;
        private Vector2 p2;
        private Vector2 p3;

        public Vector2 Point1
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Vector2 Point2
        {
            get { return p2; }
            set { p2 = value; }
        }
        public Vector2 Point3
        {
            get { return p3; }
            set { p3 = value; }
        }

        public Vector2[] TrianglePoints
        {
            get { return new Vector2[] { this.p1, this.p2, this.p3 }; }
        }

        public Triangle2D(Vector2 Pnt1, Vector2 Pnt2, Vector2 Pnt3)
        {
            this.p1 = new Vector2(Pnt1);
            this.p2 = new Vector2(Pnt2);
            this.p3 = new Vector2(Pnt3);
        }

        public Triangle2D(Vector2[] Points)
        {
            this.p1 = new Vector2(Points[0]);
            this.p2 = new Vector2(Points[1]);
            this.p3 = new Vector2(Points[2]);
        }

        public Triangle2D(float[] Point1, float[] Point2, float[] Point3)
        {
            this.p1 = new Vector2(Point1);
            this.p2 = new Vector2(Point2);
            this.p3 = new Vector2(Point3);
        }

        public Triangle2D(Triangle2D other)
        {
            this.p1 = new Vector2(other.p1);
            this.p2 = new Vector2(other.p2);
            this.p3 = new Vector2(other.p3);
        }

        public bool PointInUV1Triangle(Vector2 p, out float[] weights)   //return whether point p is inside UV1 face, weights = barycentric weights
        {
            float padding = 0.0001f;      //compensate for rounding etc. errors
            float lowval = 0f - padding;
            float highval = 1f + padding;
            float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            float w1 = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) / denominator;
            float w2 = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) / denominator;
            float w3 = 1f - w1 - w2;

            weights = new float[] { w1, w2, w3 };
            return lowval <= w1 && w1 <= highval && lowval <= w2 && w2 <= highval && lowval <= w3 && w3 <= highval;
        }

        public Vector2 Centroid
        {
            get { return new Vector2((p1.X + p2.X + p3.X) / 3f, (p1.Y + p2.Y + p3.Y) / 3f); }
        }

        public override string ToString()
        {
            return "(" + this.Point1.ToString() + "), (" + this.Point2.ToString() + "), (" + this.Point3.ToString() + ")";
        }
    }

    public struct Triangle3D
    {
        private Vector3 p1;
        private Vector3 p2;
        private Vector3 p3;

        public Vector3 Point1
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Vector3 Point2
        {
            get { return p2; }
            set { p2 = value; }
        }
        public Vector3 Point3
        {
            get { return p3; }
            set { p3 = value; }
        }
        public Vector3[] TrianglePoints
        {
            get { return new Vector3[] { this.p1, this.p2, this.p3 }; }
        }

        public Triangle3D(Vector3 Pnt1, Vector3 Pnt2, Vector3 Pnt3)
        {
            this.p1 = new Vector3(Pnt1);
            this.p2 = new Vector3(Pnt2);
            this.p3 = new Vector3(Pnt3);
        }

        public Triangle3D(Vector3[] Points)
        {
            this.p1 = new Vector3(Points[0]);
            this.p2 = new Vector3(Points[1]);
            this.p3 = new Vector3(Points[2]);
        }

        public Triangle3D(float[] Point1, float[] Point2, float[] Point3)
        {
            this.p1 = new Vector3(Point1);
            this.p2 = new Vector3(Point2);
            this.p3 = new Vector3(Point3);
        }

        public Triangle3D(Triangle3D other)
        {
            this.p1 = new Vector3(other.p1);
            this.p2 = new Vector3(other.p2);
            this.p3 = new Vector3(other.p3);
        }

        public Vector3 Centroid
        {
            get { return new Vector3((p1.X + p2.X + p3.X) / 3f, (p1.Y + p2.Y + p3.Y) / 3f, (p1.Z + p2.Z + p3.Z) / 3f); }
        }

        public Vector3 BarycentricCoordinates(Vector3 p)   //return point barycentric coordinates
        {
            float denominatorInverse = 1f / ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            float w1 = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) * denominatorInverse;
            float w2 = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) * denominatorInverse;
            float w3 = 1f - w1 - w2;
            return new Vector3(new float[] { w1, w2, w3 });
        }

        public Vector3 WorldCoordinates(Vector3 barycentricCoordinates)   //return point Cartesian coordinates
        {
            return barycentricCoordinates.X * this.p1 + barycentricCoordinates.Y * this.p2 + barycentricCoordinates.Z * this.p3;
        }

        public override string ToString()
        {
            return "(" + this.Point1.ToString() + "), (" + this.Point2.ToString() + "), (" + this.Point3.ToString() + ")";
        }
    }

    // Rotation quaternions and matrices

    public class Quaternion
    {
        double x, y, z, w;

        public float X { get { return (float)this.x; } }
        public float Y { get { return (float)this.y; } }
        public float Z { get { return (float)this.z; } }
        public float W { get { return (float)this.w; } }
        public float[] Coordinates { get { return new float[] { (float)this.x, (float)this.y, (float)this.z, (float)this.w }; } }

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion(float[] quat)
        {
            this.x = quat[0];
            this.y = quat[1];
            this.z = quat[2];
            this.w = quat[3];
        }

        public static Quaternion Identity
        {
            get { return new Quaternion(0, 0, 0, 1); }
        }

        public static Quaternion RotateYUpToZUp
        {
            get { return new Quaternion(0.7071068f, 0, 0, 0.7071068f);  }
        }

        public bool isEmpty
        {
            get { return this.x == 0d && this.y == 0d && this.z == 0d && (this.w == 1d || this.w == 0d); }
        }

        public bool isIdentity
        {
            get { return this.x == 0d && this.y == 0d && this.z == 0d && this.w == 1d; }
        }

        public bool isNormalized
        {
            get
            {
                double magnitude = this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
                return (Math.Abs(magnitude - 1d) <= .000001d);
            }
        }

        public void Normalize()
        {
            double magnitude = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w);
            this.x = (float)(this.x / magnitude);
            this.y = (float)(this.y / magnitude);
            this.z = (float)(this.z / magnitude);
            this.w = (float)(this.w / magnitude);
        }

        public void Balance()
        {
            double m = this.x * this.x - this.y * this.y - this.z * this.z;
            if (m <= 1d)
            {
                this.w = Math.Sqrt(1d - m);
            }
            else
            {
                this.Normalize();
            }
        }

        public static Quaternion operator *(Quaternion q, Quaternion r)
        {
            return new Quaternion(r.w * q.x + r.x * q.w - r.y * q.z + r.z * q.y,
                                  r.w * q.y + r.x * q.z + r.y * q.w - r.z * q.x,
                                  r.w * q.z - r.x * q.y + r.y * q.x + r.z * q.w,
                                  r.w * q.w - r.x * q.x - r.y * q.y - r.z * q.z);
        }

        public static Quaternion operator *(Quaternion q, float f)
        {
            Quaternion tmp = new Quaternion(q.x * f, q.y * f, q.z * f, q.w);
            tmp.Normalize();
            return tmp;
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            Quaternion tmp = new Quaternion(v.X, v.Y, v.Z, 0);
            return q * tmp;
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-this.x, -this.y, -this.z, this.w);
        }

        public Quaternion Inverse()
        {
            double norm = this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
            if (norm > 0f)
            {
                Quaternion q = new Quaternion(-this.x / norm, -this.y / norm, -this.z / norm, this.w / norm);
                q.Normalize();
                return q;
            }
            else
            {
                return Quaternion.Identity;
            }
        }

        public Euler toEuler()
        {
            Quaternion q = this;
            float[] res = new float[3];
            res[0] = (float)Math.Atan2(2 * (q.y * q.z + q.w * q.x), q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z);
            double r21 = -2 * (q.x * q.z - q.w * q.y);
            if (r21 < -1d) r21 = -1;
            if (r21 > 1d) r21 = 1;
            res[1] = (float)Math.Asin(r21);
            res[2] = (float)Math.Atan2(2 * (q.x * q.y + q.w * q.z), q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z);
            Euler tmp = new Euler(res[0], res[1], res[2]);
            return tmp;
        }

        public Matrix3D toMatrix3D()
        {
            double[,] matrix = new double[3, 3];
            matrix[0, 0] = 1f - (2f * this.y * this.y) - (2f * this.z * this.z);
            matrix[0, 1] = (2f * this.x * this.y) - (2f * this.z * this.w);
            matrix[0, 2] = (2f * this.x * this.z) + (2f * this.y * this.w);
            matrix[1, 0] = (2f * this.x * this.y) + (2f * this.z * this.w);
            matrix[1, 1] = 1f - (2f * this.x * this.x) - (2f * this.z * this.z);
            matrix[1, 2] = (2f * this.y * this.z) - (2f * this.x * this.w);
            matrix[2, 0] = (2f * this.x * this.z) - (2f * this.y * this.w);
            matrix[2, 1] = (2f * this.y * this.z) + (2f * this.x * this.w);
            matrix[2, 2] = 1f - (2f * this.x * this.x) - (2f * this.y * this.y);
            return new Matrix3D(matrix);
        }

        public Matrix4D toMatrix4D()
        {
            return this.toMatrix4D(new Vector3(0, 0, 0));
        }

        public Matrix4D toMatrix4D(Vector3 offset)
        {
            double[,] matrix = new double[4, 4];
            matrix[0, 0] = 1d - (2d * this.y * this.y) - (2d * this.z * this.z);
            matrix[0, 1] = (2d * this.x * this.y) - (2d * this.z * this.w);
            matrix[0, 2] = (2d * this.x * this.z) + (2d * this.y * this.w);
            matrix[0, 3] = offset.X;
            matrix[1, 0] = (2d * this.x * this.y) + (2d * this.z * this.w);
            matrix[1, 1] = 1d - (2d * this.x * this.x) - (2d * this.z * this.z);
            matrix[1, 2] = (2d * this.y * this.z) - (2d * this.x * this.w);
            matrix[1, 3] = offset.Y;
            matrix[2, 0] = (2d * this.x * this.z) - (2d * this.y * this.w);
            matrix[2, 1] = (2d * this.y * this.z) + (2d * this.x * this.w);
            matrix[2, 2] = 1d - (2d * this.x * this.x) - (2d * this.y * this.y);
            matrix[2, 3] = offset.Z;
            matrix[3, 0] = 0d;
            matrix[3, 1] = 0d;
            matrix[3, 2] = 0d;
            matrix[3, 3] = 1d;

            return new Matrix4D(matrix);
        }

        public Matrix4D toMatrix4D(Vector3 offset, Vector3 scale)
        {
            double[,] matrix = new double[4, 4];
            matrix[0, 0] = scale.X - (2d * this.y * this.y) - (2d * this.z * this.z);
            matrix[0, 1] = (2d * this.x * this.y) - (2d * this.z * this.w);
            matrix[0, 2] = (2d * this.x * this.z) + (2d * this.y * this.w);
            matrix[0, 3] = offset.X;
            matrix[1, 0] = (2d * this.x * this.y) + (2d * this.z * this.w);
            matrix[1, 1] = scale.Y - (2d * this.x * this.x) - (2d * this.z * this.z);
            matrix[1, 2] = (2d * this.y * this.z) - (2d * this.x * this.w);
            matrix[1, 3] = offset.Y;
            matrix[2, 0] = (2d * this.x * this.z) - (2d * this.y * this.w);
            matrix[2, 1] = (2d * this.y * this.z) + (2d * this.x * this.w);
            matrix[2, 2] = scale.Z - (2d * this.x * this.x) - (2d * this.y * this.y);
            matrix[2, 3] = offset.Z;
            matrix[3, 0] = 0d;
            matrix[3, 1] = 0d;
            matrix[3, 2] = 0d;
            matrix[3, 3] = 1d;

            return new Matrix4D(matrix);
        }

        public Vector3 toVector3()
        {
            return new Vector3((float)this.x, (float)this.y, (float)this.z);
        }

        public override string ToString()
        {
            return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString() + ", " + this.w.ToString();
        }

        public string ToString(string format)
        {
            return this.x.ToString(format) + ", " + this.y.ToString(format) + ", " + this.z.ToString(format) + ", " + this.w.ToString(format);
        }
    }

    public class Euler
    {
        float x, y, z;

        public float X { get { return this.x; } }
        public float Y { get { return this.y; } }
        public float Z { get { return this.z; } }

        public float[] xyzRotation { get { return new float[] { this.x, this.y, this.z }; } }

        public Euler(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class AxisAngle
    {
        double x, y, z, angle;

        public double X { get { return this.x; } }
        public double Y { get { return this.y; } }
        public double Z { get { return this.z; } }
        public double Angle { get { return this.angle; } }
        public Vector3 Axis { get { return new Vector3((float)this.x, (float)this.y, (float)this.z); } }

        public AxisAngle(float[] values)
        {
            this.x = values[0];
            this.y = values[1];
            this.z = values[2];
            this.angle = values[3];
        }

        public AxisAngle(double[] values)
        {
            this.x = values[0];
            this.y = values[1];
            this.z = values[2];
            this.angle = values[3];
        }

        public AxisAngle(double angle, double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angle = angle;
        }

        public AxisAngle(float angle, Vector3 axis)
        {
            this.x = axis.X;
            this.y = axis.Y;
            this.z = axis.Z;
            this.angle = angle;
        }

        public AxisAngle(float angle, float[] axis)
        {
            this.x = axis[0];
            this.y = axis[1];
            this.z = axis[2];
            this.angle = angle;
        }

        public void Normalize()
        {
            double magnitude = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            if (magnitude == 0) throw new ApplicationException("Cannot normalize AxisAngle: " + this.x.ToString() + " " + this.y.ToString() + " " + this.z.ToString());
            this.x /= magnitude;
            this.y /= magnitude;
            this.z /= magnitude;
        }

        public Matrix3D ToMatrix()
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);
            double t = 1.0 - c;
            this.Normalize();
            double[,] m = new double[3,3];

            m[0,0] = c + x * x * t;
            m[1,1] = c + y * y * t;
            m[2,2] = c + z * z * t;

            double tmp1 = x * y * t;
            double tmp2 = z * s;
            m[1,0] = tmp1 + tmp2;
            m[0,1] = tmp1 - tmp2;
            tmp1 = x * z * t;
            tmp2 = y * s;
            m[2,0] = tmp1 - tmp2;
            m[0,2] = tmp1 + tmp2;
            tmp1 = y * z * t;
            tmp2 = x * s;
            m[2,1] = tmp1 + tmp2;
            m[1,2] = tmp1 - tmp2;

            return new Matrix3D(m);
        }
    }

    public class Matrix3D
    {
        double[,] matrix;

        public double[,] Matrix
        {
            get
            {
                return new double[,] { { this.matrix[0,0], this.matrix[0,1], this.matrix[0,2] },
                                      { this.matrix[1,0], this.matrix[1,1], this.matrix[1,2] },
                                      { this.matrix[2,0], this.matrix[2,1], this.matrix[2,2] } };
            }
        }

        public Matrix3D()
        {
            this.matrix = new double[,] { { 0, 0, 0 },
                                         { 0, 0, 0 },
                                         { 0, 0, 0 } };
        }

        public Matrix3D(double[,] matrix)
        {
            this.matrix = new double[,] { { matrix[0,0], matrix[0,1], matrix[0,2] },
                                         { matrix[1,0], matrix[1,1], matrix[1,2] },
                                         { matrix[2,0], matrix[2,1], matrix[2,2] } };
        }

        public static Vector3 operator *(Matrix3D m, Vector3 v)
        {
            double x1 = 0, y1 = 0, z1 = 0;
            for (int i = 0; i < 3; i++)
            {
                x1 += m.matrix[0, i] * v.Coordinates[i];
                y1 += m.matrix[1, i] * v.Coordinates[i];
                z1 += m.matrix[2, i] * v.Coordinates[i];
            }
            return new Vector3((float)x1, (float)y1, (float)z1);
        }

        public static Matrix3D operator *(Matrix3D m, float f)
        {
            double[,] res = new double[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    res[r, c] = m.matrix[r, c] * f;
                }
            }
            return new Matrix3D(res);
        }

        public static double[] operator *(Matrix3D m, double[] v)
        {
            double[] tmp = new double[3];
            for (int i = 0; i < 3; i++)
            {
                tmp[0] += m.matrix[0, i] * v[i];
                tmp[1] += m.matrix[1, i] * v[i];
                tmp[2] += m.matrix[2, i] * v[i];
            }
            return tmp;
        }

        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
        {
            double[][] v = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                v[i] = m1 * new double[] { m2.matrix[0, i], m2.matrix[1, i], m2.matrix[2, i] };
            }
            return new Matrix3D(new double[,] { { v[0][0], v[1][0], v[2][0] },
                                               { v[0][1], v[1][1], v[2][1] },
                                               { v[0][2], v[1][2], v[2][2] } });
        }

        public static Matrix3D Identity
        {
            get { return new Matrix3D(new double[,] { { 1f, 0f, 0f }, { 0f, 1f, 0f }, { 0f, 0f, 1f } }); }
        }

        public static Matrix3D FromScale(Vector3 scale)
        {
            return new Matrix3D(new double[,] { { scale.X, 0f, 0f }, { 0f, scale.Y, 0f }, { 0f, 0f, scale.Z } });
        }

        public Vector3 Scale
        {
            get
            {
                Vector3 sx = new Vector3((float)this.matrix[0, 0], (float)this.matrix[0, 1], (float)this.matrix[0, 2]);
                Vector3 sy = new Vector3((float)this.matrix[1, 0], (float)this.matrix[1, 1], (float)this.matrix[1, 2]);
                Vector3 sz = new Vector3((float)this.matrix[2, 0], (float)this.matrix[2, 1], (float)this.matrix[2, 2]);
                return new Vector3(sx.Magnitude, sy.Magnitude, sz.Magnitude);
            }
        }

        public static Matrix3D RotateZupToYup
        {
            get { return new Matrix3D(new double[,] { { 1f, 0f, 0f }, { 0f, 0f, 1f }, { 0f, -1f, 0f } }); }
        }

        public static Matrix3D RotateYupToZup
        {
            get { return new Matrix3D(new double[,] { { 1f, 0f, 0f }, { 0f, 0f, -1f }, { 0f, 1f, 0f } }); }
        }

        public Matrix3D Inverse()
        {
            // computes the inverse of a matrix
            double det = this.matrix[0, 0] * (this.matrix[1, 1] * this.matrix[2, 2] - this.matrix[2, 1] * this.matrix[1, 2]) -
                         this.matrix[0, 1] * (this.matrix[1, 0] * this.matrix[2, 2] - this.matrix[1, 2] * this.matrix[2, 0]) +
                         this.matrix[0, 2] * (this.matrix[1, 0] * this.matrix[2, 1] - this.matrix[1, 1] * this.matrix[2, 0]);

            double invdet = 1f / det;

            double[,] minv = new double[3, 3];
            minv[0, 0] = (this.matrix[1, 1] * this.matrix[2, 2] - this.matrix[2, 1] * this.matrix[1, 2]) * invdet;
            minv[0, 1] = (this.matrix[0, 2] * this.matrix[2, 1] - this.matrix[0, 1] * this.matrix[2, 2]) * invdet;
            minv[0, 2] = (this.matrix[0, 1] * this.matrix[1, 2] - this.matrix[0, 2] * this.matrix[1, 1]) * invdet;
            minv[1, 0] = (this.matrix[1, 2] * this.matrix[2, 0] - this.matrix[1, 0] * this.matrix[2, 2]) * invdet;
            minv[1, 1] = (this.matrix[0, 0] * this.matrix[2, 2] - this.matrix[0, 2] * this.matrix[2, 0]) * invdet;
            minv[1, 2] = (this.matrix[1, 0] * this.matrix[0, 2] - this.matrix[0, 0] * this.matrix[1, 2]) * invdet;
            minv[2, 0] = (this.matrix[1, 0] * this.matrix[2, 1] - this.matrix[2, 0] * this.matrix[1, 1]) * invdet;
            minv[2, 1] = (this.matrix[2, 0] * this.matrix[0, 1] - this.matrix[0, 0] * this.matrix[2, 1]) * invdet;
            minv[2, 2] = (this.matrix[0, 0] * this.matrix[1, 1] - this.matrix[1, 0] * this.matrix[0, 1]) * invdet;

            return new Matrix3D(minv);
        }

        public Matrix3D Transpose()
        {
            double[,] mt = new double[,] { { this.matrix[0,0], this.matrix[1,0], this.matrix[2,0] },
                                           { this.matrix[0,1], this.matrix[1,1], this.matrix[2,1] },
                                           { this.matrix[0,2], this.matrix[1,2], this.matrix[2,2] } };
            return new Matrix3D(mt);
        }

        public override string ToString()
        {
            string str = "";
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    str += this.matrix[r, c].ToString();
                    if (c != 2 || r != 2) str += ", ";
                }
                str += Environment.NewLine;
            }
            return str;
        }
    }

    /// <summary>
    /// Supports 4x4 matrixes only
    /// </summary>
    public struct Matrix4D
    {
        double[,] matrix;

        public double[,] Matrix
        {
            get
            {
                return new double[,] { { this.matrix[0,0], this.matrix[0,1], this.matrix[0,2], this.matrix[0,3] },
                                       { this.matrix[1,0], this.matrix[1,1], this.matrix[1,2], this.matrix[1,3] },
                                       { this.matrix[2,0], this.matrix[2,1], this.matrix[2,2], this.matrix[2,3] },
                                       { this.matrix[3,0], this.matrix[3,1], this.matrix[3,2], this.matrix[3,3] } };
            }
        }

        public double[] Values
        {
            get
            {
                return new double[] { this.matrix[0,0], this.matrix[0,1], this.matrix[0,2], this.matrix[0,3],
                                      this.matrix[1,0], this.matrix[1,1], this.matrix[1,2], this.matrix[1,3],
                                      this.matrix[2,0], this.matrix[2,1], this.matrix[2,2], this.matrix[2,3],
                                      this.matrix[3,0], this.matrix[3,1], this.matrix[3,2], this.matrix[3,3] };
            }
        }

        public Matrix4D(double[,] array4x4)
        {
            this.matrix = new double[,] { { array4x4[0,0], array4x4[0,1], array4x4[0,2], array4x4[0,3] },
                                          { array4x4[1,0], array4x4[1,1], array4x4[1,2], array4x4[1,3] },
                                          { array4x4[2,0], array4x4[2,1], array4x4[2,2], array4x4[2,3] },
                                          { array4x4[3,0], array4x4[3,1], array4x4[3,2], array4x4[3,3] } };
        }

        public Matrix4D(double[] array)
        {
            this.matrix = new double[,] { { array[0], array[1], array[2], array[3] },
                                          { array[4], array[5], array[6], array[7] },
                                          { array[8], array[9], array[10], array[11] },
                                          { array[12], array[13], array[14], array[15] } };
        }

        public static Matrix4D Identity
        {
            get { return new Matrix4D(new double[,] { { 1d, 0d, 0d, 0d }, { 0d, 1d, 0d, 0d }, { 0d, 0d, 1d, 0d }, { 0d, 0d, 0d, 1d } }); }
        }

        public static Matrix4D FromOffset(Vector3 offset)
        {
            return new Matrix4D(new double[,] { { 1d, 0d, 0d, offset.X }, { 0d, 1d, 0d, offset.Y }, { 0d, 0d, 1d, offset.Z }, { 0d, 0d, 0d, 1d } });
        }

        public static Matrix4D FromOffset(double[] offset)
        {
            return new Matrix4D(new double[,] { { 1d, 0d, 0d, offset[0] }, { 0d, 1d, 0d, offset[1] }, { 0d, 0d, 1d, offset[2] }, { 0d, 0d, 0d, 1d } });
        }

        public static Matrix4D FromScale(Vector3 scale)
        {
            return new Matrix4D(new double[,] { { scale.X, 0d, 0d, 0d }, { 0d, scale.Y, 0d, 0d }, { 0d, 0d, scale.Z, 0d }, { 0d, 0d, 0d, 1d } });
        }

        public static Matrix4D FromScale(double[] scale)
        {
            return new Matrix4D(new double[,] { { scale[0], 0d, 0d, 0d }, { 0d, scale[1], 0d, 0d }, { 0d, 0d, scale[2], 0d }, { 0d, 0d, 0d, 1d } });
        }

        public static Matrix4D FromAxisAngle(AxisAngle aa)
        {
            aa.Normalize();
            Matrix4D m = new Matrix4D();
            m.matrix = new double[,] { { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 },
                                       { 0, 0, 0, 0 } };
            double c = Math.Cos(aa.Angle);
            double s = Math.Sin(aa.Angle);
            double t = 1.0 - c;

            m.matrix[0, 0] = c + aa.X * aa.X * t;
            m.matrix[1, 1] = c + aa.Y * aa.Y * t;
            m.matrix[2, 2] = c + aa.Z * aa.Z * t;

            double tmp1 = aa.X * aa.Y * t;
            double tmp2 = aa.Z * s;
            m.matrix[1, 0] = tmp1 + tmp2;
            m.matrix[0, 1] = tmp1 - tmp2;
            tmp1 = aa.X * aa.Z * t;
            tmp2 = aa.Y * s;
            m.matrix[2, 0] = tmp1 - tmp2;
            m.matrix[0, 2] = tmp1 + tmp2; tmp1 = aa.Y * aa.Z * t;
            tmp2 = aa.X * s;
            m.matrix[2, 1] = tmp1 + tmp2;
            m.matrix[1, 2] = tmp1 - tmp2;

            m.matrix[3, 3] = 1;

            return m;
        }

        public AxisAngle toAxisAngle()
        {
            double angle, x, y, z; // variables for result
            double epsilon = 0.01; // margin to allow for rounding errors
            double epsilon2 = 0.1; // margin to distinguish between 0 and 180 degrees
                                   // optional check that input is pure rotation, 'isRotationMatrix' is defined at:
                                   // https://www.euclideanspace.com/maths/algebra/matrix/orthogonal/rotation/

            if ((Math.Abs(this.matrix[0, 1] - this.matrix[1, 0]) < epsilon)
              && (Math.Abs(this.matrix[0, 2] - this.matrix[2, 0]) < epsilon)
              && (Math.Abs(this.matrix[1, 2] - this.matrix[2, 1]) < epsilon))
            {
                // singularity found
                // first check for identity matrix which must have +1 for all terms
                //  in leading diagonaland zero in other terms
                if ((Math.Abs(this.matrix[0, 1] + this.matrix[1, 0]) < epsilon2)
                  && (Math.Abs(this.matrix[0, 2] + this.matrix[2, 0]) < epsilon2)
                  && (Math.Abs(this.matrix[1, 2] + this.matrix[2, 1]) < epsilon2)
                  && (Math.Abs(this.matrix[0, 0] + this.matrix[1, 1] + this.matrix[2, 2] - 3) < epsilon2))
                {
                    // this singularity is identity matrix so angle = 0
                    return new AxisAngle(0, 1, 0, 0); // zero angle, arbitrary axis
                }
                // otherwise this singularity is angle = 180
                angle = Math.PI;
                double xx = (this.matrix[0, 0] + 1) / 2;
                double yy = (this.matrix[1, 1] + 1) / 2;
                double zz = (this.matrix[2, 2] + 1) / 2;
                double xy = (this.matrix[0, 1] + this.matrix[1, 0]) / 4;
                double xz = (this.matrix[0, 2] + this.matrix[2, 0]) / 4;
                double yz = (this.matrix[1, 2] + this.matrix[2, 1]) / 4;
                if ((xx > yy) && (xx > zz))
                { // m[0][0] is the largest diagonal term
                    if (xx < epsilon)
                    {
                        x = 0;
                        y = 0.7071;
                        z = 0.7071;
                    }
                    else
                    {
                        x = Math.Sqrt(xx);
                        y = xy / x;
                        z = xz / x;
                    }
                }
                else if (yy > zz)
                { // m[1][1] is the largest diagonal term
                    if (yy < epsilon)
                    {
                        x = 0.7071;
                        y = 0;
                        z = 0.7071;
                    }
                    else
                    {
                        y = Math.Sqrt(yy);
                        x = xy / y;
                        z = yz / y;
                    }
                }
                else
                { // m[2][2] is the largest diagonal term so base result on this
                    if (zz < epsilon)
                    {
                        x = 0.7071;
                        y = 0.7071;
                        z = 0;
                    }
                    else
                    {
                        z = Math.Sqrt(zz);
                        x = xz / z;
                        y = yz / z;
                    }
                }
                return new AxisAngle(angle, x, y, z); // return 180 deg rotation
            }
            // as we have reached here there are no singularities so we can handle normally
            double s = Math.Sqrt((this.matrix[2, 1] - this.matrix[1, 2]) * (this.matrix[2, 1] - this.matrix[1, 2])
                + (this.matrix[0, 2] - this.matrix[2, 0]) * (this.matrix[0, 2] - this.matrix[2, 0])
                + (this.matrix[1, 0] - this.matrix[0, 1]) * (this.matrix[1, 0] - this.matrix[0, 1])); // used to normalise
            if (Math.Abs(s) < 0.001) s = 1;
            // prevent divide by zero, should not happen if matrix is orthogonal and should be
            // caught by singularity test above, but I've left it in just in case
            angle = Math.Acos((this.matrix[0, 0] + this.matrix[1, 1] + this.matrix[2, 2] - 1) / 2);
            x = (this.matrix[2, 1] - this.matrix[1, 2]) / s;
            y = (this.matrix[0, 2] - this.matrix[2, 0]) / s;
            z = (this.matrix[1, 0] - this.matrix[0, 1]) / s;
            return new AxisAngle(angle, x, y, z);
        }

        public static Matrix4D RotateZupToYup
        {
            get { return new Matrix4D(new double[,] { { 1f, 0f, 0f, 0f }, { 0f, 0f, 1f, 0f }, { 0f, -1f, 0f, 0f }, { 0f, 0f, 0f, 1f } }); }
        }

        public static Matrix4D RotateYupToZup
        {
            get { return new Matrix4D(new double[,] { { 1f, 0f, 0f, 0f }, { 0f, 0f, -1f, 0f }, { 0f, 1f, 0f, 0f }, { 0f, 0f, 0f, 1f } }); }
        }

        public Matrix3D ToMatrix3D()
        {
            return new Matrix3D(new double[,] { { this.matrix[0,0], this.matrix[0,1], this.matrix[0,2] },
                                               { this.matrix[1,0], this.matrix[1,1], this.matrix[1,2] },
                                               { this.matrix[2,0], this.matrix[2,1], this.matrix[2,2] } });
        }

        /// <summary>
        /// Rounds values close to zero
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Math.Abs(this.matrix[i, j]) < .0000002d) this.matrix[i, j] = 0d;
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                Vector3 sx = new Vector3((float)this.matrix[0, 0], (float)this.matrix[0, 1], (float)this.matrix[0, 2]);
                Vector3 sy = new Vector3((float)this.matrix[1, 0], (float)this.matrix[1, 1], (float)this.matrix[1, 2]);
                Vector3 sz = new Vector3((float)this.matrix[2, 0], (float)this.matrix[2, 1], (float)this.matrix[2, 2]);
                return new Vector3(sx.Magnitude, sy.Magnitude, sz.Magnitude);
            }
        }

        public Vector3 Offset
        {
            get
            {
                return new Vector3((float)this.matrix[0, 3], (float)this.matrix[1, 3], (float)this.matrix[2, 3]);
            }
        }

        public Matrix4D RemoveOffset()
        {
            double[,] d = new double[4, 4];
            Array.Copy(this.matrix, d, 16);
            d[0, 3] = 0d;
            d[1, 3] = 0d;
            d[2, 3] = 0d;
            return new Matrix4D(d);
        }

        public Matrix4D Inverse()
        {
            double[,] m = this.matrix;
            double det = m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0] - m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0] +
                         m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] - m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] +
                         m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1] - m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] +
                         m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] - m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] +
                         m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] - m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3] +
                         m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3] - m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3];
            double invdet = 1d / det;

            double[,] minv = new double[4, 4];
            minv[0, 0] = (m[1, 2] * m[2, 3] * m[3, 1] - m[1, 3] * m[2, 2] * m[3, 1] + m[1, 3] * m[2, 1] * m[3, 2] - m[1, 1] * m[2, 3] * m[3, 2] - m[1, 2] * m[2, 1] * m[3, 3] + m[1, 1] * m[2, 2] * m[3, 3]) * invdet;
            minv[0, 1] = (m[0, 3] * m[2, 2] * m[3, 1] - m[0, 2] * m[2, 3] * m[3, 1] - m[0, 3] * m[2, 1] * m[3, 2] + m[0, 1] * m[2, 3] * m[3, 2] + m[0, 2] * m[2, 1] * m[3, 3] - m[0, 1] * m[2, 2] * m[3, 3]) * invdet;
            minv[0, 2] = (m[0, 2] * m[1, 3] * m[3, 1] - m[0, 3] * m[1, 2] * m[3, 1] + m[0, 3] * m[1, 1] * m[3, 2] - m[0, 1] * m[1, 3] * m[3, 2] - m[0, 2] * m[1, 1] * m[3, 3] + m[0, 1] * m[1, 2] * m[3, 3]) * invdet;
            minv[0, 3] = (m[0, 3] * m[1, 2] * m[2, 1] - m[0, 2] * m[1, 3] * m[2, 1] - m[0, 3] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 3] * m[2, 2] + m[0, 2] * m[1, 1] * m[2, 3] - m[0, 1] * m[1, 2] * m[2, 3]) * invdet;
            minv[1, 0] = (m[1, 3] * m[2, 2] * m[3, 0] - m[1, 2] * m[2, 3] * m[3, 0] - m[1, 3] * m[2, 0] * m[3, 2] + m[1, 0] * m[2, 3] * m[3, 2] + m[1, 2] * m[2, 0] * m[3, 3] - m[1, 0] * m[2, 2] * m[3, 3]) * invdet;
            minv[1, 1] = (m[0, 2] * m[2, 3] * m[3, 0] - m[0, 3] * m[2, 2] * m[3, 0] + m[0, 3] * m[2, 0] * m[3, 2] - m[0, 0] * m[2, 3] * m[3, 2] - m[0, 2] * m[2, 0] * m[3, 3] + m[0, 0] * m[2, 2] * m[3, 3]) * invdet;
            minv[1, 2] = (m[0, 3] * m[1, 2] * m[3, 0] - m[0, 2] * m[1, 3] * m[3, 0] - m[0, 3] * m[1, 0] * m[3, 2] + m[0, 0] * m[1, 3] * m[3, 2] + m[0, 2] * m[1, 0] * m[3, 3] - m[0, 0] * m[1, 2] * m[3, 3]) * invdet;
            minv[1, 3] = (m[0, 2] * m[1, 3] * m[2, 0] - m[0, 3] * m[1, 2] * m[2, 0] + m[0, 3] * m[1, 0] * m[2, 2] - m[0, 0] * m[1, 3] * m[2, 2] - m[0, 2] * m[1, 0] * m[2, 3] + m[0, 0] * m[1, 2] * m[2, 3]) * invdet;
            minv[2, 0] = (m[1, 1] * m[2, 3] * m[3, 0] - m[1, 3] * m[2, 1] * m[3, 0] + m[1, 3] * m[2, 0] * m[3, 1] - m[1, 0] * m[2, 3] * m[3, 1] - m[1, 1] * m[2, 0] * m[3, 3] + m[1, 0] * m[2, 1] * m[3, 3]) * invdet;
            minv[2, 1] = (m[0, 3] * m[2, 1] * m[3, 0] - m[0, 1] * m[2, 3] * m[3, 0] - m[0, 3] * m[2, 0] * m[3, 1] + m[0, 0] * m[2, 3] * m[3, 1] + m[0, 1] * m[2, 0] * m[3, 3] - m[0, 0] * m[2, 1] * m[3, 3]) * invdet;
            minv[2, 2] = (m[0, 1] * m[1, 3] * m[3, 0] - m[0, 3] * m[1, 1] * m[3, 0] + m[0, 3] * m[1, 0] * m[3, 1] - m[0, 0] * m[1, 3] * m[3, 1] - m[0, 1] * m[1, 0] * m[3, 3] + m[0, 0] * m[1, 1] * m[3, 3]) * invdet;
            minv[2, 3] = (m[0, 3] * m[1, 1] * m[2, 0] - m[0, 1] * m[1, 3] * m[2, 0] - m[0, 3] * m[1, 0] * m[2, 1] + m[0, 0] * m[1, 3] * m[2, 1] + m[0, 1] * m[1, 0] * m[2, 3] - m[0, 0] * m[1, 1] * m[2, 3]) * invdet;
            minv[3, 0] = (m[1, 2] * m[2, 1] * m[3, 0] - m[1, 1] * m[2, 2] * m[3, 0] - m[1, 2] * m[2, 0] * m[3, 1] + m[1, 0] * m[2, 2] * m[3, 1] + m[1, 1] * m[2, 0] * m[3, 2] - m[1, 0] * m[2, 1] * m[3, 2]) * invdet;
            minv[3, 1] = (m[0, 1] * m[2, 2] * m[3, 0] - m[0, 2] * m[2, 1] * m[3, 0] + m[0, 2] * m[2, 0] * m[3, 1] - m[0, 0] * m[2, 2] * m[3, 1] - m[0, 1] * m[2, 0] * m[3, 2] + m[0, 0] * m[2, 1] * m[3, 2]) * invdet;
            minv[3, 2] = (m[0, 2] * m[1, 1] * m[3, 0] - m[0, 1] * m[1, 2] * m[3, 0] - m[0, 2] * m[1, 0] * m[3, 1] + m[0, 0] * m[1, 2] * m[3, 1] + m[0, 1] * m[1, 0] * m[3, 2] - m[0, 0] * m[1, 1] * m[3, 2]) * invdet;
            minv[3, 3] = (m[0, 1] * m[1, 2] * m[2, 0] - m[0, 2] * m[1, 1] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1] - m[0, 0] * m[1, 2] * m[2, 1] - m[0, 1] * m[1, 0] * m[2, 2] + m[0, 0] * m[1, 1] * m[2, 2]) * invdet;

            return new Matrix4D(minv);
        }

        public Matrix4D Transpose()
        {
            double[,] mt = new double[,] { { this.matrix[0,0], this.matrix[1,0], this.matrix[2,0], this.matrix[3,0] },
                                           { this.matrix[0,1], this.matrix[1,1], this.matrix[2,1], this.matrix[3,1] },
                                           { this.matrix[0,2], this.matrix[1,2], this.matrix[2,2], this.matrix[3,2] },
                                           { this.matrix[0,3], this.matrix[1,3], this.matrix[2,3], this.matrix[3,3] } };
            return new Matrix4D(mt);
        }

        public static Matrix4D operator *(Matrix4D m, float f)
        {
            double[,] res = new double[4, 4];
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    res[r, c] = m.matrix[r, c] * f;
                }
            }
            return new Matrix4D(res);
        }

        public static Vector3 operator *(Matrix4D m, Vector3 v)
        {
            double x1 = 0, y1 = 0, z1 = 0, ex = 0; ;
            double[] tmp = new double[] { v.X, v.Y, v.Z, 1f };
            for (int i = 0; i < 4; i++)
            {
                x1 += m.matrix[0, i] * tmp[i];
                y1 += m.matrix[1, i] * tmp[i];
                z1 += m.matrix[2, i] * tmp[i];
                ex += m.matrix[3, i] * tmp[i];
            }
            return new Vector3((float)x1, (float)y1, (float)z1);
        }

        public static Matrix4D operator *(Matrix4D m1, Matrix4D m2)
        {
            double[][] v = new double[4][];

            for (int i = 0; i < 4; i++)
            {
                v[i] = m1 * new double[] { m2.matrix[0, i], m2.matrix[1, i], m2.matrix[2, i], m2.matrix[3, i] };
            }
            return new Matrix4D(new double[,] { { v[0][0], v[1][0], v[2][0], v[3][0] },
                                                 { v[0][1], v[1][1], v[2][1], v[3][1] },
                                                 { v[0][2], v[1][2], v[2][2], v[3][2] },
                                                 { v[0][3], v[1][3], v[2][3], v[3][3] } });
        }

        public static double[] operator *(Matrix4D m, double[] v)
        {
            double[] tmp = new double[4];
            for (int i = 0; i < 4; i++)
            {
                tmp[0] += m.matrix[0, i] * v[i];
                tmp[1] += m.matrix[1, i] * v[i];
                tmp[2] += m.matrix[2, i] * v[i];
                tmp[3] += m.matrix[3, i] * v[i];
            }
            return tmp;
        }

        public override string ToString()
        {
            string str = "";
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    str += this.matrix[r, c].ToString("G7");
                    if (c != 3 || r != 3) str += ", ";
                }
                if (r < 4) str += Environment.NewLine;
            }
            return str;
        }

        public string ToUnpunctuatedString()
        {
            string str = "";
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    str += this.matrix[r, c].ToString("G7", System.Globalization.CultureInfo.InvariantCulture) + " ";
                }
            }
            return str;
        }
    }

    public partial class Form1 : Form
    {
        //port of the updated C function from armature.c
        //https://developer.blender.org/T39470
        //note that C accesses columns first, so all matrix indices are swapped compared to the C version
        public static Matrix3D vec_roll_to_mat3(Vector3 vec, float roll)
        {
            vec.Normalize();
            float[] nor = vec.Coordinates;
            double THETA_THRESHOLD_NEGY = 1.0e-9;
            double THETA_THRESHOLD_NEGY_CLOSE = 1.0e-5;

            // create a 3x3 matrix
            double[,] bMatrix = new double[3, 3];

            double theta = 1.0f + nor[1];
            double test = nor[0] > 0f ? nor[0] : nor[2];
            double test2 = test > 0f ? theta : test;
            if ((theta > THETA_THRESHOLD_NEGY_CLOSE) || (test2 > THETA_THRESHOLD_NEGY))
            {
                bMatrix[1, 0] = -nor[0];
                bMatrix[0, 1] = nor[0];
                bMatrix[1, 1] = nor[1];
                bMatrix[2, 1] = nor[2];
                bMatrix[1, 2] = -nor[2];
                if (theta > THETA_THRESHOLD_NEGY_CLOSE)
                {
                    //If nor is far enough from -Y, apply the general case.
                    bMatrix[0, 0] = 1 - nor[0] * nor[0] / theta;
                    bMatrix[2, 2] = 1 - nor[2] * nor[2] / theta;
                    bMatrix[0, 2] = bMatrix[2, 0] = -nor[0] * nor[2] / theta;
                }
                else
                {
                    // If nor is too close to -Y, apply the special case.
                    theta = nor[0] * nor[0] + nor[2] * nor[2];
                    bMatrix[0, 0] = (nor[0] + nor[2]) * (nor[0] - nor[2]) / -theta;
                    bMatrix[2, 2] = -bMatrix[0, 0];
                    bMatrix[0, 2] = bMatrix[2, 0] = 2.0f * nor[0] * nor[2] / theta;
                }
            }
            else
            {
                // If nor is -Y, simple symmetry by Z axis.
                bMatrix[0, 0] = bMatrix[1, 1] = -1.0f;
            }

            // Make Roll matrix
            // rMatrix = mathutils.Matrix.Rotation(roll, 3, nor)
            AxisAngle aa = new AxisAngle(roll, nor);
            Matrix3D rMatrix = aa.ToMatrix();

            // Combine and output result
            Matrix3D mat = rMatrix * new Matrix3D(bMatrix);
            return mat;
        }

        public static AxisAngle mat3_to_vec_roll(Matrix3D mat)
        {
            // this hasn't changed
            double[,] matrix = mat.Matrix;
            Vector3 vec = new Vector3((float)matrix[0, 1], (float)matrix[1, 1], (float)matrix[2, 1]);
            Matrix3D vecmat = vec_roll_to_mat3(vec, 0f);
            Matrix3D vecmatinv = vecmat.Inverse();
            Matrix3D rollmatrix = vecmatinv * mat;
            double[,] rollmat = rollmatrix.Matrix;
            double roll = Math.Atan2(rollmat[0, 2], rollmat[2, 2]);
            if (roll == double.NaN) roll = 0;
            return new AxisAngle((float)roll, vec);
        }
    }
}