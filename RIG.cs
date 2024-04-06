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
    public class RIG
    {
        int version;                //3-4 - TS4 always 3?
        int minorVersion;           //1-2 - TS4 always 1?
        int boneCount;
        Bone[] bones;
        int IKchainCount;
        IKchain[] IKchains;

        Quaternion rootRotation = Quaternion.Identity;

        public int NumberBones { get { return this.boneCount; } }

        public string ListBonesByFlags(uint flag)
        {
            string s = "";
            foreach (Bone b in this.bones)
            {
                if (b.flags == flag) s += b.BoneName + Environment.NewLine;
            }
            return s;
        }

        public Bone[] Bones { get { return this.bones; } }

        public Quaternion RootBindRotation
        {
            get { return this.rootRotation; }
            set { this.rootRotation = value; }
        }

        public int GetIndex(uint boneHash)
        {
            for (int i = 0; i < this.NumberBones; i++)
            {
                if (boneHash == this.bones[i].BoneHash) return i;
            }
            return -1;
        }

        public RIG.Bone GetBone(uint boneHash)
        {
            foreach (Bone b in bones)
            {
                if (boneHash == b.BoneHash)
                {
                    return b;
                }
            }
            return null;
        }

        public bool GetPosition(uint boneHash, out Vector3 position)
        {
            foreach (Bone b in bones)
            {
                if (boneHash == b.BoneHash)
                {
                    position = b.PositionVector;
                    return true;
                }
            }
            position = new Vector3();
            return false;
        }

        public bool GetPosition(int index, out Vector3 position)
        {
            if (index >= 0 && index < this.NumberBones)
            {
                position = bones[index].PositionVector;
                return true;
            }
            position = new Vector3();
            return false;
        }

        public bool GetWorldPosition(uint boneHash, out Vector3 position)
        {
            foreach (Bone b in bones)
            {
                if (boneHash == b.BoneHash)
                {
                    position = b.WorldPosition;
                    return true;
                }
            }
            position = new Vector3();
            return false;
        }

        public bool GetWorldPosition(int index, out Vector3 position)
        {
            if (index >= 0 && index < this.NumberBones)
            {
                position = bones[index].WorldPosition;
                return true;
            }
            position = new Vector3();
            return false;
        }

        public Bone[] GetChildren(uint boneHash)
        {
            int index = this.GetIndex(boneHash);
            if (index < 0) return null;
            int[] childIndexes = this.GetChildIndexes(index);
            List<Bone> childBones = new List<Bone>();
            foreach (int i in childIndexes)
            {
                childBones.Add(this.bones[i]);
            }
            return childBones.ToArray();
        }

        public int[] GetChildIndexes(int index)
        {
            List<int> childList = new List<int>();
            if (index >= 0 && index < this.NumberBones)
            {
                for (int i = 0; i < this.NumberBones; i++)
                {
                    if (bones[i].ParentBoneIndex == index) childList.Add(i);
                }
            }
            return childList.ToArray();
        }

        public int[] GetParentIndexes(int index)
        {
            List<int> parentList = new List<int>();
            int i = index;
            while (this.bones[i].ParentBoneIndex >= 0 && this.bones[i].ParentBoneIndex < this.NumberBones)
            {
                parentList.Add(this.bones[i].ParentBoneIndex);
                i = this.bones[i].ParentBoneIndex;
            }
            parentList.Reverse();
            return parentList.ToArray();
        }

        public RIG(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            version = br.ReadInt32();
            minorVersion = br.ReadInt32();
            boneCount = br.ReadInt32();
            bones = new Bone[boneCount];
            for (int i = 0; i < boneCount; i++)
            {
                bones[i] = new Bone(br, this, i);
            }
            IKchainCount = br.ReadInt32();
            IKchains = new IKchain[IKchainCount];
            for (int i = 0; i < IKchainCount; i++)
            {
                IKchains[i] = new IKchain(br, this);
            }
        }

        public RIG(RIG other)
        {
            this.version = other.version;
            this.minorVersion = other.minorVersion;
            this.boneCount = other.boneCount;
            this.bones = new Bone[boneCount];
            for (int i = 0; i < boneCount; i++)
            {
                bones[i] = new Bone(other.bones[i], this);
            }
            IKchainCount = other.IKchainCount;
            IKchains = new IKchain[IKchainCount];
            for (int i = 0; i < IKchainCount; i++)
            {
                IKchains[i] = new IKchain(other.IKchains[i], this);
            }
        }

        public void GetDescendants(uint boneHash, ref List<uint> allBones)
        {
            int index = this.GetIndex(boneHash);
            if (index < 0) return;
            allBones.Add(boneHash);
            int[] childIndexes = this.GetChildIndexes(index);
            foreach (int i in childIndexes)
            {
                GetDescendants(this.bones[i].BoneHash, ref allBones);
            }
        }

        //internal void BoneMorpher(Bone bone, float weight, Vector3 scale, Vector3 offset, Quaternion rotation)
        //{
        //  //  RIG.Bone bone = GetBone(boneHash);
        //    bone.UpdateLocalData(bone.WorldPosition, weight, offset, rotation, scale);
        //    for (int i = GetIndex(bone.BoneHash); i < this.bones.Length; i++)
        //    {
        //        this.bones[i].CalculateTransforms();
        //    }
        //}

        internal void BoneMorpher(Bone bone, float weight, Vector3 scale, Vector3 offset, Quaternion rotation)
        {
            bone.UpdateLocalData(weight, offset, rotation, scale);
            ScaleBone(bone.BoneHash, scale, weight);
            for (int i = GetIndex(bone.BoneHash); i < this.bones.Length; i++)
            {
                this.bones[i].CalculateTransforms();
            }
        }

        internal void ScaleBone(uint boneHash, Vector3 scale, float weight)
        {
            RIG.Bone[] childBones = this.GetChildren(boneHash);
            foreach (RIG.Bone child in childBones)
            {
                child.ScaleBone(scale, weight);
                ScaleBone(child.BoneHash, scale, weight);
            }
        }

        public override string ToString()
        {
            string tmp = "Version: " + version.ToString() + ", Minor Version: " + minorVersion.ToString() + Environment.NewLine;
            tmp += boneCount.ToString() + " Bones:" + Environment.NewLine + Environment.NewLine;
            for (int i = 0; i < boneCount; i++)
            {
                tmp += bones[i].ToString() + Environment.NewLine + Environment.NewLine;
            }
            tmp += IKchainCount.ToString() + " IK Chains:" + Environment.NewLine + Environment.NewLine;
            for (int i = 0; i < IKchainCount; i++)
            {
                tmp += IKchains[i].ToString() + Environment.NewLine + Environment.NewLine;
            }
            return tmp;
        }

        public class Bone
        {
            float[] position = new float[3];
            float[] orientation = new float[4];         //Quaternion 
            float[] scaling = new float[3];
            string boneName;
            int opposingBoneIndex;      // Same as the bone's index except in the case of Left/Right mirrored bones it is the index of its opposite
            int parentBoneIndex;
            uint boneHash;
            internal uint flags;

            RIG rig;
            int index;
            Quaternion localRotation;
            Quaternion globalRotation;
            Matrix4D localTransform;
            Matrix4D globalTransform;
            Vector3 worldPosition;

            public string BoneName { get { return this.boneName; } }
            public int ParentBoneIndex { get { return this.parentBoneIndex; } }
            public RIG.Bone ParentBone { get { if (this.parentBoneIndex >= 0) { return this.rig.bones[parentBoneIndex]; } else { return null; } } }
            public string ParentName { get { if (this.parentBoneIndex >= 0) { return rig.bones[this.parentBoneIndex].boneName; } else { return ""; } } }
            public string OpposingBoneName
            {
                get
                {
                    if (this.opposingBoneIndex != this.index) { return rig.bones[this.opposingBoneIndex].boneName; }
                    else { return ""; }
                }
            }
            public uint BoneHash { get { return this.boneHash; } }
            /// <summary>
            /// Returns position relative to parent bone
            /// </summary>
            public Vector3 PositionVector { get { return new Vector3(this.position); } set { this.position = value.Coordinates; } }
            public Vector3 ScalingVector { get { return new Vector3(this.scaling); } }
            public Quaternion LocalRotation { get { return new Quaternion(this.localRotation.Coordinates); } }
            public Quaternion GlobalRotation { get { return new Quaternion(this.globalRotation.Coordinates); } }
            public Vector3 WorldPosition
            {
                get { return this.worldPosition; }
                set { this.worldPosition = new Vector3(value); }
            }
            public Quaternion MorphRotation
            {
                get
                {
                    return (parentBoneIndex >= 0) ? new Quaternion(this.rig.bones[parentBoneIndex].globalRotation.Coordinates) : this.globalRotation;
                }
            }
            // public Quaternion GlobalRotation { get { return new Quaternion(this.globalRotation.Coordinates); } }

            public Matrix4D GlobalTransform { get { return new Matrix4D(this.globalTransform.Matrix); } }
            public Matrix4D LocalTransform { get { return new Matrix4D(this.localTransform.Matrix); } }
            public RIG Rig { get { return this.rig; } }

            internal Bone(BinaryReader br, RIG r, int index)
            {
                this.rig = r;
                this.index = index;
                for (int i = 0; i < 3; i++)
                {
                    position[i] = br.ReadSingle();
                }
                for (int i = 0; i < 4; i++)
                {
                    orientation[i] = br.ReadSingle();
                }
                for (int i = 0; i < 3; i++)
                {
                    scaling[i] = br.ReadSingle();
                }
                int boneNameLength = br.ReadInt32();
                char[] tmp = br.ReadChars(boneNameLength);
                boneName = new string(tmp);
                opposingBoneIndex = br.ReadInt32();
                parentBoneIndex = br.ReadInt32();
                boneHash = br.ReadUInt32();
                flags = br.ReadUInt32();
                CalculateTransforms();
            }

            internal void CalculateTransforms()
            {
                this.localRotation = new Quaternion(this.orientation);
                if (this.localRotation.isEmpty) localRotation = Quaternion.Identity;
                if (!this.localRotation.isNormalized) this.localRotation.Balance();
                localTransform = this.localRotation.toMatrix4D(new Vector3(position), new Vector3(scaling));
                if (this.boneName.Contains("ROOT_bind")) rig.rootRotation = this.localRotation;

                if (this.parentBoneIndex >= 0 && this.parentBoneIndex < rig.NumberBones)
                {
                    this.globalTransform = rig.bones[this.parentBoneIndex].globalTransform * localTransform;
                    this.globalRotation = rig.bones[this.parentBoneIndex].globalRotation * this.localRotation;
                }
                else    //no parents
                {
                    this.globalTransform = localTransform;
                    this.globalRotation = localRotation;
                }
                this.worldPosition = this.globalTransform * new Vector3();
            }

            internal void UpdateLocalData(Vector3 origin, float weight, Vector3 morphOffset, Quaternion morphRotation, Vector3 morphScale)
            {
                Vector3 weightedScale = (morphScale * weight) + new Vector3(1f, 1f, 1f);
                Vector3 weightedOffset = morphOffset * weight;
                Quaternion weightedRotation = morphRotation * weight;

                this.position = (this.PositionVector + weightedOffset).Coordinates;
                this.orientation = (this.localRotation * weightedRotation).Coordinates;
                this.scaling = this.ScalingVector.Scale(weightedScale).Coordinates;
            }

            internal Bone(Bone other, RIG r)
            {
                this.rig = other.rig;
                this.index = other.index;
                position = new float[] { other.position[0], other.position[1], other.position[2] };
                orientation = new float[] { other.orientation[0], other.orientation[1], other.orientation[2], other.orientation[3] };
                scaling = new float[] { other.scaling[0], other.scaling[1], other.scaling[2] };
                boneName = other.boneName;
                opposingBoneIndex = other.opposingBoneIndex;
                parentBoneIndex = other.parentBoneIndex;
                boneHash = other.boneHash;
                flags = other.flags;

                rig = r;
                int index = other.index;
                localRotation = new Quaternion(other.localRotation.Coordinates);
                globalRotation = new Quaternion(other.globalRotation.Coordinates);
                localTransform = new Matrix4D(other.localTransform.Matrix);
                globalTransform = new Matrix4D(other.globalTransform.Matrix);
                worldPosition = new Vector3(other.worldPosition);
            }

            public override string ToString()
            {
                string tmp = "Position: " + position[0].ToString() + "," + position[1].ToString() + "," + position[2].ToString() + Environment.NewLine;
                tmp += "Rotation: " + orientation[0].ToString() + "," + orientation[1].ToString() + "," + orientation[2].ToString() + Environment.NewLine;
                tmp += "Scaling: " + scaling[0].ToString() + "," + scaling[1].ToString() + "," + scaling[2].ToString() + Environment.NewLine;
                tmp += "Bone Name: " + boneName + Environment.NewLine;
                tmp += "Opposing Bone Index: " + opposingBoneIndex.ToString() + " (" + rig.bones[opposingBoneIndex].boneName + ")" + Environment.NewLine;
                tmp += "Parent Bone Index: " + parentBoneIndex.ToString() + ((parentBoneIndex >= 0 && parentBoneIndex < rig.boneCount) ? " (" + rig.bones[parentBoneIndex].boneName + ")" : "") + Environment.NewLine;
                tmp += "Bone Hash : " + boneHash.ToString("X8") + Environment.NewLine;
                tmp += "Flags : " + flags.ToString("X8");
                return tmp;
            }

            internal void UpdateLocalData(float weight, Vector3 morphOffset, Quaternion morphRotation, Vector3 morphScale)
            {
                //  Vector3 weightedScale = (morphScale * weight) + new Vector3(1f, 1f, 1f);
                Vector3 weightedOffset = morphOffset * weight;
                Quaternion weightedRotation = morphRotation * weight;

                this.position = (this.PositionVector + weightedOffset).Coordinates;
                this.orientation = (this.localRotation * weightedRotation).Coordinates;
                //  this.scaling = this.ScalingVector.Scale(weightedScale).Coordinates;
            }

            internal void ScaleBone(Vector3 scale, float weight)
            {
                Vector3 pos = new Vector3(this.position);
                Vector3 tmp = pos.Scale((scale * weight) + new Vector3(1f, 1f, 1f));
                // Vector3 diff = tmp - pos;
                this.position = tmp.Coordinates;
            }

            /// <summary>
            /// Moves a bone when its parent bone is scaled, moved, and/or rotated
            /// </summary>
            /// <param name="parentPosition">Point of origin for scaling and rotation</param>
            /// <param name="scale">Scale vector of parent</param>
            /// <param name="offset">Offset of parent</param>
            /// <param name="rotation">Rotation of parent</param>
            /// <returns>Offset vector from old position to new position</returns>
            internal void BoneMover(Vector3 scale, Vector3 offset, Quaternion rotation, float weight)
            {
                Vector3 weightedScale = (scale * weight) + new Vector3(1f, 1f, 1f);
                Vector3 weightedOffset = offset * weight;
                Quaternion weightedRotation = rotation * weight;
                Matrix4D transform = weightedRotation.toMatrix4D(weightedOffset, weightedScale);
                Vector3 bonePos = this.WorldPosition;
                if (this.ParentBone != null) bonePos -= this.ParentBone.WorldPosition;
                bonePos = transform * bonePos;
                if (this.ParentBone != null) bonePos += this.ParentBone.WorldPosition;
                this.WorldPosition = bonePos;

                RIG.Bone[] childBones = rig.GetChildren(this.boneHash);
                foreach (RIG.Bone child in childBones)
                {
                    child.BoneMover(scale, offset, rotation, weight);
                }
            }
        }

        internal class IKchain
        {
            int boneListLength;
            int[] boneIndex;            //bone index, boneListLength times
            int poleVectorIndex;        //bone index 
            int slotOffsetIndex;        //bone index 
            int rootIndex;              //bone index 
            RIG rig;

            internal IKchain(BinaryReader br, RIG r)
            {
                boneListLength = br.ReadInt32();
                boneIndex = new int[boneListLength];
                for (int i = 0; i < boneListLength; i++)
                {
                    boneIndex[i] = br.ReadInt32();
                }
                poleVectorIndex = br.ReadInt32();
                slotOffsetIndex = br.ReadInt32();
                rootIndex = br.ReadInt32();
                this.rig = r;
            }

            public IKchain(IKchain other, RIG r)
            {
                this.boneListLength = other.boneListLength;
                this.boneIndex = new int[boneListLength];
                Array.Copy(other.boneIndex, this.boneIndex, boneListLength);
                this.poleVectorIndex = other.poleVectorIndex;
                this.slotOffsetIndex = other.slotOffsetIndex;
                this.rootIndex = other.rootIndex;
                this.rig = r;
            }

            public override string ToString()
            {
                string tmp = "Bones: ";
                for (int i = 0; i < boneListLength; i++)
                {
                    tmp += rig.bones[boneIndex[i]].BoneName + " ";
                }
                tmp += Environment.NewLine + "PoleVectorIndex: " + poleVectorIndex.ToString() + Environment.NewLine;
                tmp += "SlotOffsetIndex: " + slotOffsetIndex.ToString() + Environment.NewLine;
                tmp += "RootIndex: " + rootIndex.ToString();
                return tmp;
            }
        }
    }
}
