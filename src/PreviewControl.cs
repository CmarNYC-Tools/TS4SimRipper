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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Resources;
using s4pi.ImageResource;
using s4pi.Package;

namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
        ResourceManager rm = Properties.Resources.ResourceManager;
        GEOM[] CurrentModel = null;
        GEOM[] BaseModel = null;
        GEOM[] GlassModel = null;
        string currentName;
        string[] partNames = Enum.GetNames(typeof(BodyType));
        Species currentSpecies;
        AgeGender currentAge;
        AgeGender currentGender;
        AgeGender currentFrame;
        SimOccult currentOccult;
        RIG currentRig;
        RIG baseRig;
        List<OutfitInfo> currentOutfits;
        int outfitIndex;
        Image currentTexture;
        Image currentSpecular;
        Image currentOverlay;
        Image currentClothing;
        Image currentClothingSpecular;
        Image currentMakeup;
        Image currentMakeupSpecular;
        Image currentBump;
        Image currentEmission;
        Image currentGlassTexture;
        Image currentGlassSpecular;
        Image currentSkin;
        Image currentSculptOverlay;
        Image currentOutfitOverlay;
        List<BGEO> morphBGEO;
        List<MorphMap> morphShape;
        List<MorphMap> morphNormals;
        List<BOND> morphBOND;

        public static void LoadBONDMorph(FileStream log, GEOM baseMesh, BOND boneDelta, RIG rig)
        {
            LoadBONDMorph(log, new GEOM[] { baseMesh }, boneDelta, rig, true);
        }
        public static void LoadBONDMorph(FileStream log, GEOM baseMesh, BOND boneDelta, RIG rig, bool updateRig)
        {
            LoadBONDMorph(log, new GEOM[] { baseMesh }, boneDelta, rig, updateRig);
        }
        public static void LoadBONDMorph(FileStream log, GEOM[] baseMeshes, BOND boneDelta, RIG rig)
        {
            LoadBONDMorph(log, baseMeshes, boneDelta, rig, true);
        }
        public static void LoadBONDMorph(FileStream log, GEOM[] baseMeshes, BOND boneDelta, RIG rig, bool updateRig)
        {
            if (baseMeshes == null) return;
            if (boneDelta == null) return;
            if (boneDelta.weight == 0f) return;
            LogMe(log, "Applying BOND 0x" + boneDelta.instance.ToString("X16") + " (" + boneDelta.package + ") to GEOMs:");
            Vector3 unit = new Vector3(1f, 1f, 1f);
            string missingBones = "";
            float weight = boneDelta.weight;

            foreach (GEOM morphMesh in baseMeshes)
            {
                if (morphMesh == null) continue;
                LogMe(log, "--- GEOM 0x" + morphMesh.Instance + " (" + morphMesh.package + ")");
                morphMesh.SetupDeltas();

                // for (int i = boneDelta.adjustments.Length - 1; i >= 0; i--)
                foreach (BOND.BoneAdjust delta in boneDelta.adjustments)
                {
                    //    BOND.BoneAdjust delta = boneDelta.adjustments[i];

                    RIG.Bone bone = rig.GetBone(delta.slotHash);
                    if (bone == null)
                    {
                        missingBones += "Bone not found: " + delta.slotHash.ToString("X8") + ", ";
                        continue;
                    }
                    Vector3 localScale = new Vector3(delta.scaleX, delta.scaleY, delta.scaleZ);
                    Vector3 localOffset = new Vector3(delta.offsetX, delta.offsetY, delta.offsetZ);
                    Quaternion localRotation = new Quaternion(delta.quatX, delta.quatY, delta.quatZ, delta.quatW);
                    if (localRotation.isEmpty) localRotation = Quaternion.Identity;
                    if (!localRotation.isNormalized) localRotation.Balance();

                    Vector3 worldScale = (bone.MorphRotation.toMatrix3D() * Matrix3D.FromScale(localScale + unit)).Scale - unit;
                    Vector3 worldOffset = (bone.MorphRotation * localOffset * bone.MorphRotation.Conjugate()).toVector3();
                    Quaternion worldRotation = bone.MorphRotation * localRotation * bone.MorphRotation.Conjugate();

                    morphMesh.BoneMorpher(bone, weight, worldOffset, worldScale, worldRotation);
                    //BoneMorpher2(true, morphMesh, rig, bone, weight, localOffset, localScale, localRotation);
                    //rig.BoneMorpher(bone, weight, localScale, localOffset, localRotation);
                    // BoneMorpher(morphMesh, rig, bone, weight, worldOffset, worldScale, worldRotation);
                }

                morphMesh.UpdatePositions();
            }

            if (missingBones.Length > 0)
            {
                MessageBox.Show(missingBones + Environment.NewLine + Environment.NewLine + "Are you using the right rig?");
            }

            if (!updateRig) return;

            LogMe(log, "--- Updating rig");
            foreach (BOND.BoneAdjust delta in boneDelta.adjustments)
            {
                RIG.Bone bone = rig.GetBone(delta.slotHash);
                if (bone == null)
                {
                    continue;
                }
                Vector3 localScale = new Vector3(delta.scaleX, delta.scaleY, delta.scaleZ);
                Vector3 localOffset = new Vector3(delta.offsetX, delta.offsetY, delta.offsetZ);
                Quaternion localRotation = new Quaternion(delta.quatX, delta.quatY, delta.quatZ, delta.quatW);
                if (localRotation.isEmpty) localRotation = Quaternion.Identity;
                if (!localRotation.isNormalized) localRotation.Balance();

                rig.BoneMorpher(bone, weight, localScale, localOffset, localRotation);
            }
        }

        public static GEOM LoadBGEOMorph(FileStream log, GEOM baseMesh, BGEO morph, int lod)
        {
            if (baseMesh == null || !baseMesh.hasVertexIDs) return baseMesh;
            if (morph == null) return new GEOM(baseMesh);
            if (morph.weight == 0f) return new GEOM(baseMesh);
            LogMe(log, "Applying BGEO 0x" + morph.instance + " (" + morph.package + ") to GEOM 0x" + baseMesh.Instance + " (" + baseMesh.package + ")");
            GEOM morphMesh = new GEOM(baseMesh);
            float weight = morph.weight;
            uint startID = morph.LodData[lod].IndexBase;
            uint startIndex = 0;
            for (int i = 0; i < lod; i++)
            {
                startIndex += morph.LodData[i].NumberVertices;
            }
            for (int i = 0; i < morphMesh.numberVertices; i++)
            {
                if (morphMesh.getVertexID(i) >= morph.LodData[lod].IndexBase &&
                        morphMesh.getVertexID(i) < morph.LodData[lod].IndexBase + morph.LodData[lod].NumberVertices)
                {
                    float vertWeight = 1f;
                    if (morphMesh.copyFaceMorphs)
                        vertWeight = ((morphMesh.getTagval(i) & 0x003F0000) >> 16) / 63f;
                    BGEO.Blend blend = morph.BlendMap[startIndex + (morphMesh.getVertexID(i) - startID)];
                    if (blend.PositionDelta)
                    {
                        Vector3 pos = new Vector3(morphMesh.getPosition(i));
                        Vector3 delta = new Vector3(morph.VectorData[blend.Index].TranslatedVector);
                        morphMesh.setPosition(i, (pos + (delta * weight * vertWeight)).Coordinates);
                    }
                    if (blend.NormalDelta)
                    {
                        Vector3 norm = new Vector3(morphMesh.getNormal(i));
                        Vector3 delta = new Vector3(morph.VectorData[blend.Index + (blend.PositionDelta ? 1 : 0)].TranslatedVector);
                        morphMesh.setNormal(i, (norm + (delta * weight * vertWeight)).Coordinates);
                    }
                }
            }
            return morphMesh;
        }

        public static GEOM LoadDMapMorph(FileStream log, GEOM baseMesh, DMap dmapShape, DMap dmapNormals)
        {
            MorphMap mapShape = dmapShape != null ? dmapShape.ToMorphMap() : null;
            MorphMap mapNormals = dmapNormals != null ? dmapNormals.ToMorphMap() : null;
            return LoadDMapMorph(log, baseMesh, mapShape, mapNormals);
        }

        public static GEOM LoadDMapMorph(FileStream log, GEOM baseMesh, MorphMap morphShape, MorphMap morphNormals)
        {
            if (baseMesh == null) return null;
            if (morphShape == null) return new GEOM(baseMesh);
            if (morphShape.weight == 0) return new GEOM(baseMesh);
            if (!baseMesh.hasTags || !baseMesh.hasUVset(1)) return new GEOM(baseMesh);
            LogMe(log, "Applying DMap 0x" + morphShape.instance.ToString("X16") + " (" + morphShape.package + ") to GEOM 0x" + baseMesh.Instance + " (" + baseMesh.package + ")");
            GEOM morphMesh = new GEOM(baseMesh);
            Vector3 empty = new Vector3(0, 0, 0);

            if (morphShape != null && morphMesh.hasUVset(1))
            {
                string tmp = "";
                for (int i = 0; i < morphMesh.numberVertices; i++)
                {
                    float[] pos = morphMesh.getPosition(i);
                    float[] origPos = morphMesh.getOriginalPosition(i);
                    float[] norm = morphMesh.getNormal(i);
                    List<float[]> stitchList = morphMesh.GetStitchUVs(i);
                    int x, y;
                    Vector3 shapeVector = new Vector3();
                    Vector3 normVector = new Vector3();
                    if (stitchList.Count > 0)
                    {
                        float[] uv1 = stitchList[0];
                        x = (int)(Math.Abs(morphShape.MapWidth * uv1[0]) - morphShape.MinCol - 0.5f);
                        y = (int)((morphShape.MapHeight * uv1[1]) - morphShape.MinRow - 0.5f);

                        //Vector2 uv1 = new Vector2();
                        //Vector2 uvTest = new Vector2(stitchList[stitchList.Count - 1]);
                        //foreach (float[] stitch in stitchList)
                        //{
                        //    uv1 += new Vector2(Math.Abs(stitch[0]), stitch[1]);
                        //}
                        //uv1 = uv1 * (1f / stitchList.Count);
                        ////if (!uv1.CloseTo(uvTest, 0.1f))
                        ////{
                        ////    tmp += "Vert: " + i.ToString() + " Single: " + uvTest.ToString() + " Averaged: " + uv1.ToString() + Environment.NewLine;
                        ////}
                        //x = (int)(Math.Abs(morphShape.MapWidth * uv1.X) - morphShape.MinCol - 0.5f);
                        //y = (int)((morphShape.MapHeight * uv1.Y) - morphShape.MinRow - 0.5f);
                    }
                    else
                    {
                        float[] uv1 = morphMesh.getUV(i, 1);
                        x = (int)(Math.Abs(morphShape.MapWidth * uv1[0]) - morphShape.MinCol - 0.5f);
                        y = (int)((morphShape.MapHeight * uv1[1]) - morphShape.MinRow - 0.5f);
                    }

                    if (y > morphShape.MaxRow - morphShape.MinRow) y = (int)(morphShape.MaxRow - morphShape.MinRow - 0.5f); //not sure about this

                    if (x >= 0 && x <= (morphShape.MaxCol - morphShape.MinCol) &&
                        y >= 0 && y <= (morphShape.MaxRow - morphShape.MinRow))
                    {
                        //x = Math.Max(x, 0);
                        //y = Math.Max(y, 0);
                        //x = (int)Math.Min(x, morphShape.MaxCol - morphShape.MinCol);
                        //y = (int)Math.Min(y, morphShape.MaxRow - morphShape.MinRow);
                        shapeVector = morphShape.GetAdjustedDelta(x, y, origPos[0] < 0, (byte)(morphMesh.getTagval(i) & 0x3F));
                        if (morphNormals != null)
                        {
                            normVector = morphNormals.GetAdjustedDelta(x, y, origPos[0] < 0, (byte)(morphMesh.getTagval(i) & 0x3F));
                        }
                    }

                    if (shapeVector != empty)
                    {
                        // float vertWeight = ((morphMesh.getTagval(i) & 0xFF00) >> 8) / 255f;
                        float vertWeight = Math.Min(((morphMesh.getTagval(i) & 0xFF00) >> 8) / 64f, 1f);
                        pos[0] -= shapeVector.X * morphShape.weight * vertWeight;
                        pos[1] -= shapeVector.Y * morphShape.weight * vertWeight;
                        pos[2] -= shapeVector.Z * morphShape.weight * vertWeight;
                        if(morphNormals != null)
                        {
                            norm[0] -= normVector.X * morphNormals.weight * vertWeight;
                            norm[1] -= normVector.Y * morphNormals.weight * vertWeight;
                            norm[2] -= normVector.Z * morphNormals.weight * vertWeight;
                        }
                        morphMesh.setPosition(i, pos);
                        morphMesh.setNormal(i, norm);
                    }
                }
               // MessageBox.Show(tmp);
            }

            return morphMesh;
        }

        //public static GEOM LoadDMapMorph(GEOM baseMesh, MorphMap morphShape, MorphMap morphNormals)
        //{
        //    if (baseMesh == null) return null;
        //    if (morphShape == null & morphNormals == null)
        //    {
        //        return new GEOM(baseMesh);
        //    }
        //    if (!baseMesh.hasTags || !baseMesh.hasUVset(1)) return new GEOM(baseMesh);

        //    GEOM g = new GEOM(baseMesh);

        //    for (int i = 0; i < g.numberVertices; i++)
        //    {
        //        float[] pos = g.getPosition(i);
        //        float[] norm = g.getNormal(i);
        //        if (morphShape != null && g.hasUVset(1))
        //        {
        //            List<float[]> stitchList = g.GetStitchUVs(i);
        //            Vector3 shapeVector = new Vector3();
        //            Vector3 normVector = new Vector3();
        //            if (stitchList.Count > 0)
        //            {
        //                foreach (float[] stitch in stitchList)
        //                {
        //                    int x = (int)(Math.Abs((morphShape.MapWidth - 1) * stitch[0]) - morphShape.MinCol - 0.5f);
        //                    int y = (int)(((morphShape.MapHeight - 1) * stitch[1]) - morphShape.MinRow - 0.5f);
        //                    if (x >= 0 && x < (morphShape.MaxCol - morphShape.MinCol) &&
        //                        y >= 0 && y < (morphShape.MaxRow - morphShape.MinRow))
        //                    {
        //                        // Vector3 deltaShape = morphShape.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0xFF));
        //                        Vector3 deltaShape = morphShape.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0x3F));  //as of Cats & Dogs
        //                        shapeVector += deltaShape;
        //                        if (morphNormals != null)
        //                        {
        //                            //  Vector3 deltaNorm = morphNormals.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0xFF));
        //                            Vector3 deltaNorm = morphNormals.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0x3F));
        //                            normVector += deltaNorm;
        //                        }
        //                    }
        //                }
        //                shapeVector = shapeVector / (float)stitchList.Count;
        //                normVector = normVector / (float)stitchList.Count;
        //            }
        //            else
        //            {
        //                float[] uv1 = g.getUV(i, 1);
        //                int x = (int)(Math.Abs(morphShape.MapWidth * uv1[0]) - morphShape.MinCol - 0.5f);
        //                int y = (int)((morphShape.MapHeight * uv1[1]) - morphShape.MinRow - 0.5f);
        //                if (x >= 0 && x < (morphShape.MaxCol - morphShape.MinCol) &&
        //                    y >= 0 && y < (morphShape.MaxRow - morphShape.MinRow))
        //                {
        //                    shapeVector = morphShape.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0x3F));
        //                    if (morphNormals != null)
        //                    {
        //                        normVector = morphNormals.GetAdjustedDelta(x, y, pos[0] < 0, (byte)(g.getTagval(i) & 0x3F));
        //                    }
        //                }
        //            }
        //            pos[0] -= shapeVector.X;
        //            pos[1] -= shapeVector.Y;
        //            pos[2] -= shapeVector.Z;
        //            norm[0] -= normVector.X;
        //            norm[1] -= normVector.Y;
        //            norm[2] -= normVector.Z;
        //            g.setPosition(i, pos);
        //            g.setNormal(i, norm);
        //        }
        //    }

        //    return g;
        //}

        private void GetCurrentModel()
        {
            GetCurrentModel(false);
        }
        private void GetCurrentModel(bool skinOnly)
        {
            LogMe(log, " ");
            LogMe(log, "**********");
            LogMe(log, " ");
            LogMe(log, "GetCurrentModel start, Outfit: " + Outfits_comboBox.SelectedItem.ToString());
            string fullInfo = "";
            TroubleshootPackageOutfit = (Package)Package.NewPackage(1);
            if (HQSize_radioButton.Checked) currentSize = currentSpecies == Species.Human ? humanTextureSizeHQ : petTextureSizeHQ;
            else currentSize = currentSpecies == Species.Human ? humanTextureSize : petTextureSize;
            if (currentSculptOverlay != null && currentSize != currentSculptOverlay.Size) currentSculptOverlay = new Bitmap(currentSculptOverlay, currentSize);

            if (!skinOnly)
            {
                List<ImageStack> imageStack = new List<ImageStack>();
                List<ImageStack> glassStack = new List<ImageStack>();
                AgeGender adjustedAge = currentAge >= AgeGender.Teen && currentAge <= AgeGender.Elder ? AgeGender.Adult : currentAge;
                string prefix = GetBodyCompletePrefix(currentSpecies, adjustedAge, currentGender);
                currentRig = new RIG(baseRig);
                GEOM head = null;
                //GEOM collar = null;
                CASP[] outfit = currentOutfits[outfitIndex].casps;
                foreach (CASP c in outfit)
                {
                    if (c.notBaseGame) SaveStream(c.tgi, c.Stream, TroubleshootPackageOutfit);
                }
                BaseModel = new GEOM[partNames.Length];
                CurrentModel = new GEOM[partNames.Length];
                GlassModel = new GEOM[partNames.Length];
                AgeGender[] partGenders = new AgeGender[partNames.Length];
                for (int i = 0; i < partGenders.Length; i++) partGenders[i] = AgeGender.None;
                string[] packNames = currentOutfits[outfitIndex].packages;
                ulong[] colorShifts = currentOutfits[outfitIndex].colorShifts;
                uint[] layerIds = currentOutfits[outfitIndex].layerIds;
                ulong excludeFlags = 0, excludeFlags2 = 0;
                List<MeshInfo> meshRegions = new List<MeshInfo>();
                float shoesKneeLayer = 0, shoesCalfLayer = 0;
                bool shoesOnTop = false;

                LogMe(log, "Setting up frame modifiers");
                if (currentGender == AgeGender.Male)
                {
                    if (currentFrame == AgeGender.Male)     //needed to change female parts to male frame
                    {
                        ulong shapeID = frameIDFtM[0];
                        DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                        if (shape == null) shape = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.yfBody_Male_Shape)));
                        ulong normalID = frameIDFtM[1];
                        DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                        if (normals == null) normals = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.yfBody_Male_Normals)));
                        if (shape != null && normals != null) frameModifier = new MorphMap[] { shape.ToMorphMap(), normals.ToMorphMap() };
                    }
                    else if (currentFrame == AgeGender.Female)  //needed to change male parts to female frame
                    {
                        ulong shapeID = frameIDMtF4male[0];
                        DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                        if (shape == null) shape = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.ymBody_Female_Shape)));
                        ulong normalID = frameIDMtF4male[1];
                        DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                        if (normals == null) normals = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.ymBody_Female_Normals)));
                        if (shape != null && normals != null) frameModifier = new MorphMap[] { shape.ToMorphMap(), normals.ToMorphMap() };
                    }
                }
                else if (currentGender == AgeGender.Female)
                {
                    if (currentFrame == AgeGender.Female)       //needed to change male parts to female frame
                    {
                        ulong shapeID = frameIDMtF4female[0];
                        DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                        ulong normalID = frameIDMtF4female[1];
                        DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                        if (shape != null && normals != null) frameModifier = new MorphMap[] { shape.ToMorphMap(), normals.ToMorphMap() };
                    }
                    else if (currentFrame == AgeGender.Male)    //needed to change female parts to male frame
                    {
                        ulong shapeID = frameIDFtM[0];
                        DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                        ulong normalID = frameIDFtM[1];
                        DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                        if (shape != null && normals != null) frameModifier = new MorphMap[] { shape.ToMorphMap(), normals.ToMorphMap() };
                    }
                }

                LogMe(log, "First pass through outfits");
                for (int i = 0; i < outfit.Length; i++)
                {
                    string packname = packNames[i];
                    if ((outfit[i].DisallowOppositeGender && (currentGender & outfit[i].gender) == 0) ||
                        (outfit[i].DisallowOppositeFrame && (currentFrame & outfit[i].gender) == 0))
                    {
                        if (outfit[i].OppositeGenderPart > 0)
                        {
                            CASP casp = FetchGameCASP(new TGI((uint)ResourceTypes.CASP, 0, outfit[i].OppositeGenderPart), out packname, outfit[i].BodyType, outfitIndex, ref errorList, true);
                            if (casp != null) outfit[i] = casp;
                        }
                        else if (outfit[i].FallbackPart > 0)
                        {
                            CASP casp = FetchGameCASP(new TGI((uint)ResourceTypes.CASP, 0, outfit[i].FallbackPart), out packname, outfit[i].BodyType, outfitIndex, ref errorList, true);
                            if (casp != null) outfit[i] = casp;
                        }
                    }
                    if (outfit[i].BodyType == BodyType.Head)
                    {
                        TGI[] headTgis = outfit[i].MeshParts(0);
                        foreach (TGI tgi in headTgis)
                        {
                            GEOM geom = FetchGameGEOM(tgi, packNames[i], outfitIndex, ref errorList);
                            if (geom != null && geom.ShaderHash != (uint)SimShader.Phong)
                            {
                                LogMe(log, "Loading head geom: " + tgi.ToString() + " linked from package: " + outfit[i].package);
                                if ((currentFrame & outfit[i].gender) == 0)
                                {
                                    geom = LoadDMapMorph(log, geom, frameModifier[0], frameModifier[1]);
                                }
                                head = geom;
                            }
                        }
                    }
                        //else if (currentSpecies != Species.Human && outfit[i].BodyType == BodyType.Necklace)
                        //{
                        //    collar = geom;
                        //}
                    if (outfit[i].BodyType == BodyType.Shoes)
                    {
                        RegionMap shoes = FetchGameRMap(outfit[i].LinkList[outfit[i].RegionMapIndex], packNames[i], outfitIndex, ref errorList);
                        LogMe(log, "Setting shoes layer: RMAP: " + outfit[i].LinkList[outfit[i].RegionMapIndex].ToString() + " Package: " + packNames[i]);
                        if (shoes != null)
                        {
                            shoesKneeLayer = shoes.GetLayer(CASPartRegionTS4.Knee);
                            shoesCalfLayer = shoes.GetLayer(CASPartRegionTS4.Calf);
                        }
                    }
                    packNames[i] = packname;
                    excludeFlags = excludeFlags | outfit[i].ExcludePartFlags;
                    excludeFlags2 = excludeFlags2 | outfit[i].ExcludePartFlags2;
                    fullInfo += $"{outfit[i].PartName} ({outfit[i].BodyType.ToString()}) LayerID: {layerIds[i]:X8} Colorshift:  0x{colorShifts[i].ToString("X16")} ({packname}){Environment.NewLine}{Environment.NewLine}";
                }

                for (int i = 0; i < outfit.Length; i++)
                {
                    LogMe(log, "Processing outfit CASP: 0x" + outfit[i].tgi.Instance.ToString("X16") + " Package: " + outfit[i].package);
                    ulong test = (outfit[i].BodyTypeNumeric < 64) ? 1ul << (int)outfit[i].BodyTypeNumeric : 1ul << (int)(outfit[i].BodyTypeNumeric - 64);
                    if (outfit[i].BodyTypeNumeric < 64 && (excludeFlags & test) > 0 ||
                        outfit[i].BodyTypeNumeric >= 64 && (excludeFlags2 & test) > 0) continue;

                    TGI[] tgis = outfit[i].MeshParts(0);
                    if (tgis.Length == 0) tgis = outfit[i].MeshParts(1);
                    TGI bumpTGI = null;
                    TGI emissionTGI = null;
                    bool gotGlass = false;
                    bool rejected = false;
                    if (tgis.Length > 0)
                    {
                        RegionMap rmap = FetchGameRMap(outfit[i].LinkList[outfit[i].RegionMapIndex], packNames[i], outfitIndex, ref errorList);
                        rejected = true;
                        foreach (TGI tgi in tgis)
                        {
                            uint[] regions = rmap != null ? rmap.GetMeshRegions(tgi) : new uint[0];
                            float[] layers = rmap != null ? rmap.GetMeshLayers(tgi) : new float[0];
                            if (rmap != null)
                            {
                                bool reject = false;
                                if ((outfit[i].BodyType == BodyType.Body || outfit[i].BodyType == BodyType.Bottom))
                                {
                                    if (rmap.GetLayer(CASPartRegionTS4.Knee) < shoesKneeLayer)
                                    {
                                        for (int r = 0; r < regions.Length; r++)
                                        {
                                            if (regions[r] == (uint)CASPartRegionTS4.Calf) reject = true;
                                            else if (regions[r] == (uint)CASPartRegionTS4.Ankle) reject = true;
                                        }
                                        shoesOnTop = true;
                                    }
                                    else if (rmap.GetLayer(CASPartRegionTS4.Calf) < shoesCalfLayer)
                                    {
                                        for (int r = 0; r < regions.Length; r++)
                                        {
                                            if (regions[r] == (uint)CASPartRegionTS4.Ankle) reject = true;
                                        }
                                        shoesOnTop = true;
                                    }
                                }
                                if (reject) continue;
                                foreach (MeshInfo meshReg in meshRegions)
                                {
                                    if (meshReg.rejectMesh(regions, layers))
                                    {
                                        reject = true;
                                        break;
                                    }
                                }
                                if (reject) continue;
                            }

                            rejected = false;
                            GEOM geom = FetchGameGEOM(tgi, packNames[i], outfitIndex, ref errorList);
                            if (geom != null)
                            {
                                meshRegions.Add(new MeshInfo(outfit[i].BodyType, outfit[i].Species > 0 ? outfit[i].Species : Species.Human,
                                        outfit[i].age, outfit[i].gender, geom, regions, layers));
                                if (bumpTGI == null && geom.Shader.normalIndex >= 0) bumpTGI = geom.TGIList[geom.Shader.normalIndex];
                                if (emissionTGI == null && geom.Shader.emissionIndex >= 0) emissionTGI = geom.TGIList[geom.Shader.emissionIndex];
                                if (geom.ShaderHash == (uint)SimShader.SimGlass) gotGlass = true;
                            }
                        }
                    }

                    if (rejected && !(outfit[i].BodyType == BodyType.Hair)) continue;

                    partGenders[(int)outfit[i].BodyType] = outfit[i].gender;

                    LogMe(log, "Processing outfit textures");
                    Image texture = FetchGameTexture(outfit[i].LinkList[outfit[i].TextureIndex], outfitIndex, ref errorList, false);
                    Image shadow = FetchGameImageFromRLE(outfit[i].LinkList[outfit[i].ShadowIndex], outfitIndex, ref errorList);
                    Image specular = FetchGameImageFromRLE(outfit[i].LinkList[outfit[i].SpecularIndex], true, outfitIndex, ref errorList, true);
                    Image bumpmap = null;
                    Image emissionmap = null;
                    currentOutfitOverlay = new Bitmap(currentSize.Width, currentSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    bumpmap = bumpTGI != null ? FetchGameImageFromDST(bumpTGI, outfitIndex, ref errorList) : null;
                    emissionmap = emissionTGI != null ? FetchGameImageFromDST(emissionTGI, outfitIndex, ref errorList) : null;

                    int sortLayer = outfit[i].BodyType == BodyType.Shoes && shoesOnTop ? 17000 : outfit[i].SortLayer;
                    //if (outfit[i].BodyType != BodyType.Head)
                    //{
                        imageStack.Add(new ImageStack(sortLayer, outfit[i].CompositionMethod, colorShifts[i], outfit[i].BodyType, outfit[i].HasMesh, texture, shadow, specular, bumpmap, emissionmap));
                        if (OverlaySort_comboBox.SelectedIndex == 0 && outfit[i].CompositionMethod == 3)
                        {
                            using (Graphics g = Graphics.FromImage(currentOutfitOverlay))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.DrawImage(texture, new Point(0, 0));
                            }
                        }
                        else if (OverlaySort_comboBox.SelectedIndex == 1 && outfit[i].CompositionMethod == 3)
                        {
                            using (Graphics g = Graphics.FromImage(texture))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.DrawImage(currentOutfitOverlay, new Point(0, 0));
                            }
                            currentOutfitOverlay = texture;
                        }
                        if (gotGlass) glassStack.Add(new ImageStack(outfit[i].SortLayer, outfit[i].CompositionMethod, colorShifts[i], outfit[i].BodyType, outfit[i].HasMesh, texture, shadow, specular, bumpmap, emissionmap));
                   // }
                }
                if(tattooTracker?.body_type_tattoo_data?.Any()==true){
                    var sl = 3000;
                    foreach(var t in tattooTracker.body_type_tattoo_data){
                        if(t.body_part_custom_texture >0){
                            Bitmap texture = FetchCustomTattoo(new TGI(0xF8E1457A, 0x00800000, t.body_part_custom_texture), ref errorList);
                            imageStack.Add(new ImageStack(++sl, 0, 0x4000000000000000, (BodyType)t.body_type, false, texture, null, null, null, null));
                        }
                    }
                }

                LogMe(log, "Processing outfit mesh regions");
                for (int m = 0; m < meshRegions.Count; m++)
                {
                    int ind = (int)meshRegions[m].partType;
                    bool skip = false;
                    for (int g = 0; g < meshRegions.Count; g++)
                    {
                        if (g == m) continue;
                        for (int r = 0; r < meshRegions[m].regions.Count; r++)
                        {
                            for (int r2 = 0; r2 < meshRegions[g].regions.Count; r2++)
                            {
                                if ((meshRegions[m].regions[r] > 0) &&
                                    (meshRegions[m].regions[r] == meshRegions[g].regions[r2]) &&
                                    (meshRegions[m].layers[r] < meshRegions[g].layers[r2]))
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (skip) break;
                        }
                        if (skip) break;
                    }
                    if (skip) continue;
                    GEOM geom = meshRegions[m].geom;
                    if (geom == null) continue;
                    if (!geom.hasSeamStitches) geom.AutoSeamStitches(currentSpecies, currentAge, currentGender, 0);
                    if (currentSpecies == Species.Human)
                    {
                        if (geom.hasTags && geom.hasBlueVertexColor)
                        {
                            if (head != null && !geom.hasVertexIDs) geom.AutoVertexID(head);
                        }
                       // if (head != null && meshRegions[m].partType == BodyType.Hair) geom.SnapVerticesToHead(head);
                        
                        if (geom.hasVertexIDs)
                        {
                            foreach (BGEO b in morphBGEO)
                            {
                                geom = LoadBGEOMorph(log, geom, b, 0);
                            }
                        }
                    }

                    geom.StandardizeFormat();
                    geom.FixBoneWeights();

                    // geom.MatchPartSeamStitches();
                    // geom.SnapVertices();

                    if (geom.ShaderHash == (uint)SimShader.SimGlass)
                    {
                        if (GlassModel[ind] == null) GlassModel[ind] = geom;
                        else GlassModel[ind].AppendMesh(geom);
                    }
                    else
                    {
                        if (BaseModel[ind] == null) BaseModel[ind] = geom;
                        else BaseModel[ind].AppendMesh(geom);
                    }
                }

                if (BaseModel != null && currentSpecies == Species.Human && currentAge > AgeGender.Child && currentAge != AgeGender.Infant)
                {
                    //if (partGenders[(int)BodyType.Top] == AgeGender.Female && partGenders[(int)BodyType.Bottom] == AgeGender.Male) BaseModel[(int)BodyType.Top].FillWaistGap(BaseModel[(int)BodyType.Bottom]);
                    if (partGenders[(int)BodyType.Top] == AgeGender.Female && partGenders[(int)BodyType.Bottom] == AgeGender.Male)
                    {
                        LogMe(log, "Adding waist filler");
                        BaseModel[(int)BodyType.Top].AutoSeamStitches(Species.Human, AgeGender.Adult, AgeGender.Female, 0);
                        BaseModel[(int)BodyType.Bottom].AutoSeamStitches(Species.Human, AgeGender.Adult, AgeGender.Male, 0);
                        Stream s = new MemoryStream(Properties.Resources.WaistFiller);
                        BinaryReader br = new BinaryReader(s);
                        BaseModel[(int)BodyType.Top].AppendMesh(new GEOM(br));
                    }

                    LogMe(log, "Applying frame morphs");
                    if (currentGender == AgeGender.Female && currentFrame == AgeGender.Male)
                    {
                        if (partGenders[(int)BodyType.Top] == AgeGender.Male)
                        {
                            BaseModel[(int)BodyType.Top] = LoadDMapMorph(log, BaseModel[(int)BodyType.Top], frameBoobs[0], frameBoobs[1]);
                            GlassModel[(int)BodyType.Top] = LoadDMapMorph(log, GlassModel[(int)BodyType.Top], frameBoobs[0], frameBoobs[1]);
                            partGenders[(int)BodyType.Top] = AgeGender.Female;
                        }
                        if (partGenders[(int)BodyType.Body] == AgeGender.Male)
                        {
                            BaseModel[(int)BodyType.Body] = LoadDMapMorph(log, BaseModel[(int)BodyType.Body], frameBoobs[0], frameBoobs[1]);
                            GlassModel[(int)BodyType.Body] = LoadDMapMorph(log, GlassModel[(int)BodyType.Body], frameBoobs[0], frameBoobs[1]);
                            partGenders[(int)BodyType.Body] = AgeGender.Female;
                        }
                    }

                    for (int i = 0; i < BaseModel.Length; i++)
                    {
                        if ((currentFrame & partGenders[i]) == 0)
                        {
                            BaseModel[i] = LoadDMapMorph(log, BaseModel[i], frameModifier[0], frameModifier[1]);
                            GlassModel[i] = LoadDMapMorph(log, GlassModel[i], frameModifier[0], frameModifier[1]);
                        }
                    }

                    if (currentGender == AgeGender.Male)
                    {
                        if (partGenders[(int)BodyType.Top] == AgeGender.Female)
                        {
                            LoadBONDMorph(log, BaseModel[(int)BodyType.Top], frameModifierFlat, currentRig);
                            LoadBONDMorph(log, GlassModel[(int)BodyType.Top], frameModifierFlat, currentRig);
                        }
                        if (partGenders[(int)BodyType.Body] == AgeGender.Female)
                        {
                            LoadBONDMorph(log, BaseModel[(int)BodyType.Body], frameModifierFlat, currentRig);
                            LoadBONDMorph(log, GlassModel[(int)BodyType.Body], frameModifierFlat, currentRig);
                        }
                    }
                }

                for (int d = 0; d < morphShape.Count; d++)
                {
                    for (int i = 0; i < GlassModel.Length; i++)
                    {
                        if (GlassModel[i] != null && morphShape[d] != null && isBodyTypeForRegion((BodyType)i, morphShape[d].region))
                        {
                            GlassModel[i] = LoadDMapMorph(log, GlassModel[i], morphShape[d], morphNormals.ElementAtOrDefault(d));
                        }
                    }
                    for (int i = 0; i < BaseModel.Length; i++)
                    {
                        if (BaseModel[i] != null && morphShape[d] != null && isBodyTypeForRegion((BodyType)i, morphShape[d].region))
                        {
                            BaseModel[i] = LoadDMapMorph(log, BaseModel[i], morphShape[d], morphNormals.ElementAtOrDefault(d));
                        }
                    }
                }

                foreach (BOND b in morphBOND)
                {
                    foreach (BOND.BoneAdjust adjust in b.adjustments)
                    {
                        if (adjust.slotHash == 0x77F97B14 || adjust.slotHash == 0x57912F4F)
                        {
                            int dummy = 1;
                        }
                    }
                    float weight = b.weight;
                    if (currentGender == AgeGender.Female && (partGenders[(int)BodyType.Top] == AgeGender.Male || partGenders[(int)BodyType.Body] == AgeGender.Male))
                    {
                        Dictionary<ulong, string> dmapConvert = CASTuning.DmapConversions(currentSpecies, currentOccult, currentAge, currentGender,
                            partGenders[(int)BodyType.Top] == AgeGender.Male ? BodyType.Top : BodyType.Body, AgeGender.Male);
                        string dmapName;
                        if (dmapConvert != null && dmapConvert.TryGetValue(b.publicKey[0].Instance, out dmapName))
                        {
                            b.weight = 0;
                            ulong shapeInstance = FNVhash.FNV64(dmapName + "_shape");
                            DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeInstance), ref errorList);
                            ulong normalInstance = FNVhash.FNV64(dmapName + "_normals");
                            DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalInstance), ref errorList);
                            shape.weight = weight;
                            normals.weight = weight;
                            for (int i = 0; i < GlassModel.Length; i++) GlassModel[i] = LoadDMapMorph(log, GlassModel[i], shape, normals);
                            for (int i = 0; i < BaseModel.Length; i++) BaseModel[i] = LoadDMapMorph(log, BaseModel[i], shape, normals);
                        }
                    }
                    if (b.weight > 0)
                    {
                        LoadBONDMorph(log, GlassModel, b, currentRig, false);
                        LoadBONDMorph(log, BaseModel, b, currentRig);
                    }
                    b.weight = weight;
                }

                LogMe(log, "Matching seams");
                for (int i = 0; i < BaseModel.Length; i++)
                {
                    if (BaseModel[i] == null) continue;
                    if (BaseModel[i].hasSeamStitches)
                    {
                        for (int j = 0; j < BaseModel.Length; j++)
                        {
                            if (i == j || BaseModel[j] == null) continue;
                            if (BaseModel[j].hasSeamStitches)
                            {
                                GEOM.MatchSeamStitches(BaseModel[i], BaseModel[j]);
                            }
                        }
                    }
                    //BaseModel[i].MatchPartSeamStitches();
                    //if (BaseModel[i] != null) BaseModel[i].SnapVertices();
                }
                for (int i = 0; i < GlassModel.Length; i++)
                {
                    if (GlassModel[i] == null) continue;
                    if (GlassModel[i].hasSeamStitches)
                    {
                        for (int j = 0; j < GlassModel.Length; j++)
                        {
                            if (i == j || GlassModel[j] == null) continue;
                            if (GlassModel[j].hasSeamStitches)
                            {
                                GEOM.MatchSeamStitches(GlassModel[i], GlassModel[j]);
                            }
                        }
                    }
                }

                if (pregnancyProgress > 0f)
                {
                    if (currentSpecies == Species.Human)
                    {
                        LogMe(log, "Applying pregnancy morph");
                        if (pregnantModifier[0] != null) pregnantModifier[0].weight = pregnancyProgress / 1.25f;
                        if (pregnantModifier[1] != null) pregnantModifier[1].weight = pregnancyProgress / 1.25f;
                        for (int i = 0; i < BaseModel.Length; i++)
                        {
                            if (i == (int)BodyType.Body || i == (int)BodyType.Top || i == (int)BodyType.Bottom)
                                CurrentModel[i] = LoadDMapMorph(log, BaseModel[i], pregnantModifier[0], pregnantModifier[1]);
                            else
                                CurrentModel[i] = BaseModel[i] != null ? new GEOM(BaseModel[i]) : null;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < BaseModel.Length; i++)
                    {
                        CurrentModel[i] = BaseModel[i] != null ? new GEOM(BaseModel[i]) : null;
                    }
                }

                UpdateSlotTargets(ref errorList);

                // imageStack.Sort((x, y) => x.sortLayer.CompareTo(y.sortLayer));
                // imageStack.OrderByDescending(s => s.compositionMethod).ThenBy(s => s.sortLayer).ToList();
                LogMe(log, "Compositing textures");
                imageStack.Sort((x, y) => {
                    int ret = -x.compositionMethod.CompareTo(y.compositionMethod);
                    return ret != 0 ? ret : x.sortLayer.CompareTo(y.sortLayer);
                });
                Bitmap image = new Bitmap(currentSize.Width, currentSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                Bitmap spec = new Bitmap(currentSize.Width / 2, currentSize.Height / 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Bitmap clothing = new Bitmap(currentSize.Width, currentSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                Bitmap clothingSpec = new Bitmap(currentSize.Width / 2, currentSize.Height / 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Bitmap makeup = new Bitmap(currentSize.Width, currentSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Bitmap makeupSpec = new Bitmap(currentSize.Width / 2, currentSize.Height / 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Bitmap bump = new Bitmap(1024, 2048, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gs = Graphics.FromImage(bump))
                {
                    gs.Clear(Color.Gray);
                }
                Bitmap alpha = new Bitmap(bump);
                bump.SetAlphaFromImage(alpha);
                alpha.Dispose();
                Bitmap emit = new Bitmap(1024, 2048, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bool gotBump = false;
                bool gotEmit = false;
                float[][] alphaMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
                };
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    Point origin = new Point(0, 0);
                    for (int i = 0; i < imageStack.Count; i++)
                    {
                        if (imageStack[i].shadow != null)
                        {
                            g.DrawImage(DisplayableShadow(imageStack[i].shadow), origin);
                            using (Graphics go = Graphics.FromImage(clothing))
                            {
                                go.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                go.DrawImage(DisplayableShadow(imageStack[i].shadow), origin);
                            }
                        }

                        // composition 0 = straight overlay
                        // 1 = hard light?
                        // 2 = regular makeup
                        // 3 = grayscale shading
                        // 4 = special makeup?             
                        if (imageStack[i].image != null && imageStack[i].compositionMethod != 3)
                        {
                            ShiftTexture((Bitmap)imageStack[i].image, imageStack[i].HueShift, imageStack[i].SaturationShift, imageStack[i].BrightnessShift);
                            if (imageStack[i].compositionMethod == 2)
                            {
                                alphaMatrix[3][3] = currentTONE.SkinSets[0].MakeupOpacity * imageStack[i].Opacity;
                            }
                            else if (imageStack[i].compositionMethod == 4)
                            {
                                alphaMatrix[3][3] = currentTONE.SkinSets[0].MakeupOpacity2 * imageStack[i].Opacity;
                            }
                            else
                            {
                                alphaMatrix[3][3] = imageStack[i].Opacity;
                            }
                            ColorMatrix convert = new ColorMatrix(alphaMatrix);
                            ImageAttributes attributes = new ImageAttributes();
                            attributes.SetColorMatrix(convert);
                            ColorMatrix convertMakeup = new ColorMatrix(alphaMatrix);
                            convertMakeup.Matrix33 = alphaMatrix[3][3] * 1.2f;
                            ImageAttributes attributes2 = new ImageAttributes();
                            attributes2.SetColorMatrix(convertMakeup);
                            if (imageStack[i].image.Width != image.Width || imageStack[i].image.Height != image.Height)
                            {
                                errorList += "Image dimensions don't match!" +
                                    "Main image: " + image.Width.ToString() + "x" + image.Height.ToString() +
                                    " - CASP image: " + imageStack[i].image.Width.ToString() + "x" + imageStack[i].image.Height.ToString() +
                                    Environment.NewLine;
                                continue;
                            }
                            g.DrawImage(imageStack[i].image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, imageStack[i].image.Width, imageStack[i].image.Height, GraphicsUnit.Pixel, attributes);
                            if (isMakeup(imageStack[i].partType, imageStack[i].isMesh))
                            {
                                using (Graphics go = Graphics.FromImage(makeup))
                                {
                                    go.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    go.DrawImage(imageStack[i].image, new Rectangle(0, 0, makeup.Width, makeup.Height), 0, 0, imageStack[i].image.Width, imageStack[i].image.Height, GraphicsUnit.Pixel, attributes2);
                                }
                            }
                            else
                            {
                                using (Graphics go = Graphics.FromImage(clothing))
                                {
                                    go.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    go.DrawImage(imageStack[i].image, new Rectangle(0, 0, clothing.Width, clothing.Height), 0, 0, imageStack[i].image.Width, imageStack[i].image.Height, GraphicsUnit.Pixel, attributes);
                                }
                            }
                        }
                        else if (OverlaySort_comboBox.SelectedIndex == 2 && imageStack[i].image != null && imageStack[i].compositionMethod == 3)
                        {
                            //  imageStack[i].image.Save("F:\\Sims4Workspace\\" + imageStack[i].partType.ToString());
                            using (Graphics go = Graphics.FromImage(currentOutfitOverlay))
                            {
                                go.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                go.DrawImage(imageStack[i].image, new Rectangle(0, 0, currentOutfitOverlay.Width, currentOutfitOverlay.Height), 0, 0, imageStack[i].image.Width, imageStack[i].image.Height, GraphicsUnit.Pixel);
                            }
                        }

                        if (imageStack[i].specular != null & imageStack[i].image != null)
                        {
                            spec = LayerWithMask(spec, DisplayableSpecular(imageStack[i].specular), imageStack[i].image, imageStack[i].isMesh);
                            if (isMakeup(imageStack[i].partType, imageStack[i].isMesh))
                                makeupSpec = LayerWithMask(makeupSpec, DisplayableSpecular(imageStack[i].specular), imageStack[i].image, imageStack[i].isMesh);
                            else
                                clothingSpec = LayerWithMask(clothingSpec, DisplayableSpecular(imageStack[i].specular), imageStack[i].image, imageStack[i].isMesh);
                        }
                        else if (imageStack[i].specular != null)
                        {
                            using (Graphics gs = Graphics.FromImage(spec))
                            {
                                gs.DrawImage(DisplayableSpecular(imageStack[i].specular), origin);
                            }
                            if (isMakeup(imageStack[i].partType, imageStack[i].isMesh))
                            {
                                using (Graphics gs = Graphics.FromImage(makeupSpec))
                                {
                                    gs.DrawImage(DisplayableSpecular(imageStack[i].specular), origin);
                                }
                            }
                            else
                            {
                                using (Graphics gs = Graphics.FromImage(clothingSpec))
                                {
                                    gs.DrawImage(DisplayableSpecular(imageStack[i].specular), origin);
                                }
                            }
                        }

                        if (imageStack[i].bumpmap != null & imageStack[i].image != null)
                        {
                            bump = LayerWithMask(bump, ExpandPartialImage(imageStack[i].bumpmap, imageStack[i].partType, currentSpecies),
                                imageStack[i].image, imageStack[i].isMesh);
                            gotBump = true;
                        }
                        else if (imageStack[i].bumpmap != null)
                        {
                            using (Graphics gs = Graphics.FromImage(bump))
                            {
                                gs.DrawImage(imageStack[i].bumpmap, GetImageStartPoint(imageStack[i].bumpmap, imageStack[i].partType, currentSpecies));
                            }
                            gotBump = true;
                        }
                        if (imageStack[i].emission != null & imageStack[i].image != null)
                        {
                            emit = LayerWithMask(emit, ExpandPartialImage(imageStack[i].emission, imageStack[i].partType, currentSpecies),
                                imageStack[i].image, imageStack[i].isMesh);
                            gotEmit = true;
                        }
                        else if (imageStack[i].emission != null)
                        {
                            using (Graphics gs = Graphics.FromImage(emit))
                            {
                                gs.DrawImage(imageStack[i].emission, GetImageStartPoint(imageStack[i].emission, imageStack[i].partType, currentSpecies));
                            }
                            gotEmit = true;
                        }
                    }
                }

                currentOverlay = new Bitmap(image);
                currentSpecular = new Bitmap(spec);
                currentClothing = new Bitmap(clothing);
                currentClothingSpecular = new Bitmap(clothingSpec);
                currentMakeup = new Bitmap(makeup);
                currentMakeupSpecular = new Bitmap(makeupSpec);
                currentBump = gotBump ? bump : null;
                currentEmission = gotEmit ? emit : null;

                currentGlassTexture = null;
                currentGlassSpecular = null;

                LogMe(log, "Compositing glass textures");
                if (glassStack.Count > 0)
                {
                    glassStack.Sort((x, y) => x.sortLayer.CompareTo(y.sortLayer));
                    image = new Bitmap(currentSize.Width, currentSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    spec = new Bitmap(currentSize.Width / 2, currentSize.Height / 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        Point origin = new Point(0, 0);
                        for (int i = 0; i < glassStack.Count; i++)
                        {
                            if (glassStack[i].image != null && glassStack[i].compositionMethod != 3)
                            {
                                if (glassStack[i].compositionMethod == 2)
                                {
                                    alphaMatrix[3][3] = currentTONE.SkinSets[0].MakeupOpacity * imageStack[i].Opacity; ;
                                }
                                else if (glassStack[i].compositionMethod == 4)
                                {
                                    alphaMatrix[3][3] = currentTONE.SkinSets[0].MakeupOpacity2 * imageStack[i].Opacity; ;
                                }
                                else
                                {
                                    alphaMatrix[3][3] = 1f;
                                }
                                ColorMatrix convert = new ColorMatrix(alphaMatrix);
                                ImageAttributes attributes = new ImageAttributes();
                                attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                g.DrawImage(glassStack[i].image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, glassStack[i].image.Width, glassStack[i].image.Height, GraphicsUnit.Pixel, attributes);
                            }
                            else if (glassStack[i].image != null && glassStack[i].compositionMethod == 3)
                            {
                                using (Graphics go = Graphics.FromImage(currentOutfitOverlay))
                                {
                                    go.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    go.DrawImage(glassStack[i].image, new Rectangle(0, 0, currentOutfitOverlay.Width, currentOutfitOverlay.Height), 0, 0, glassStack[i].image.Width, glassStack[i].image.Height, GraphicsUnit.Pixel);
                                }
                            }
                            if (glassStack[i].specular != null & glassStack[i].image != null)
                                spec = LayerWithMask(spec, DisplayableSpecular(glassStack[i].specular), glassStack[i].image, glassStack[i].isMesh);
                            else if (glassStack[i].specular != null)
                            {
                                using (Graphics gs = Graphics.FromImage(spec))
                                {
                                    gs.DrawImage(DisplayableSpecular(glassStack[i].specular), origin);
                                }
                            }
                        }
                    }
                    currentGlassTexture = image;
                    currentGlassSpecular = spec;
                }

                LogMe(log, "Blending skin/pelt texture");
                currentSkin = null;
                if (currentSpecies == Species.Human && currentOccult != SimOccult.Werewolf)
                {
                    if (Skinblend37_radioButton.Checked)
                    {
                        currentSkin = DisplayableSkintone37(currentTONE, currentSkinSet, currentTanLines, currentAge, currentGender, currentPhysique,
                            pregnancyProgress, currentSculptOverlay, currentOutfitOverlay);
                    }
                    else
                    {
                        currentSkin = DisplayableSkintone(currentTONE, currentSkinShift, currentSkinSet, currentTanLines, currentAge, currentGender, currentPhysique,
                            pregnancyProgress, currentSculptOverlay, currentOutfitOverlay);
                    }
                }
                else
                {
                    currentSkin = DisplayablePelt(currentPelt, currentSculptOverlay, currentPhysique);
                }
                if (currentSkin == null)
                {
                    currentSkin = new Bitmap(currentSize.Width, currentSize.Height);
                    using (Graphics g = Graphics.FromImage(currentSkin))
                        g.Clear(Color.LightGray);
                }

                currentTexture = new Bitmap(currentSkin.Width, currentSkin.Height);
                using (Graphics g = Graphics.FromImage(currentTexture))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    if (currentSkin != null)
                    {
                        if (currentSpecies == Species.Human)
                        {
                            int cutoff = currentSkin.Height / 4;
                            g.DrawImage(currentSkin, new Rectangle(0, cutoff, currentSkin.Width, currentSkin.Height - cutoff), 0, cutoff, currentTexture.Width, currentTexture.Height - cutoff, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage(currentSkin, new Point(0, 0));
                        }
                    }
                    if (currentOverlay != null) g.DrawImage(currentOverlay, new Point(0, 0));
                }

                SimInfo_button.Tag = simDesc + "Outfit: " + Outfits_comboBox.SelectedItem.ToString() + Environment.NewLine + Environment.NewLine + fullInfo;
                SimError_button.Tag = startupErrors + Environment.NewLine + errorList;
                SimError_button.Enabled = startupErrors.Length + errorList.Length > 0;
            }
            else
            {
                LogMe(log, "Blending skin/pelt texture");
                currentSkin = null;
                if (currentSpecies == Species.Human)
                {
                    if (Skinblend37_radioButton.Checked)
                    {
                        currentSkin = DisplayableSkintone37(currentTONE, currentSkinSet, currentTanLines, currentAge, currentGender, currentPhysique,
                            pregnancyProgress, currentSculptOverlay, currentOutfitOverlay);
                    }
                    else
                    {
                        currentSkin = DisplayableSkintone(currentTONE, currentSkinShift, currentSkinSet, currentTanLines, currentAge, currentGender, currentPhysique,
                        pregnancyProgress, currentSculptOverlay, currentOutfitOverlay);
                    }
                }
                else
                {
                    currentSkin = DisplayablePelt(currentPelt, currentSculptOverlay, currentPhysique);
                }
                if (currentSkin == null)
                {
                    currentSkin = new Bitmap(currentSize.Width, currentSize.Height);
                    using (Graphics g = Graphics.FromImage(currentSkin))
                        g.Clear(Color.LightGray);
                }

                currentTexture = new Bitmap(currentSkin.Width, currentSkin.Height);
                using (Graphics g = Graphics.FromImage(currentTexture))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    if (currentSkin != null)
                    {
                        if (currentSpecies == Species.Human)
                        {
                            int cutoff = currentSkin.Height / 4;
                            g.DrawImage(currentSkin, new Rectangle(0, cutoff, currentSkin.Width, currentSkin.Height - cutoff), 0, cutoff, currentTexture.Width, currentTexture.Height - cutoff, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage(currentSkin, new Point(0, 0));
                        }
                    }
                    if (currentOverlay != null) g.DrawImage(currentOverlay, new Point(0, 0));
                }
            }
        }

        private bool isBodyTypeForRegion(BodyType bodyType, SimRegion region)
        {
            if (region == SimRegion.ALL)
            {
                return true;
            }
            //else if (region > SimRegion.FULLFACE && (bodyType == BodyType.Head || bodyType == BodyType.Face))
            //{
            //    return false;
            //}
            else if (region == SimRegion.EARS && bodyType != BodyType.Ears)
            {
                return false;
            }
            else if (region == SimRegion.TAIL && bodyType != BodyType.Tail)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateSlotTargets(ref string errorList)            //slot adjustment written by thepancakeone
        {
            Dictionary<RIG.Bone, Vector3> morphedSlotBasePosition = new Dictionary<RIG.Bone, Vector3>();
            Dictionary<RIG.Bone, Vector3> morphedSlotBasePositionOffsets = new Dictionary<RIG.Bone, Vector3>();

            foreach (GEOM morphMesh in CurrentModel)
            {
                if (morphMesh == null)
                    continue;
                int slotRayIdx = 0;
                try
                {
                    foreach (GEOM.SlotrayIntersection slotRayIntersection in morphMesh.SlotrayAdjustments)
                    {
                        //How Slot Ray Intersections work is simple, they specify three vertices specifying a face and barycentric coordinates.
                        //The barycentric coordinates specify the exact point on the face where the slot's base position should be.
                        //Then, we add offsetFromIntersectionOSForExport to the slot base position.
                        //Note that these two positions are in world space!

                        //Next, we check if currentBone already exists in morphedSlotBasePosition. If it exists already, then it will compare
                        //the two slots' magnitude to determine what is further away from the sim.
                        //This is necessary because multiple cas assets can affect the same slot.
                        //If the currentBone doesn't exist, it just sets the position and the offsets as usual.

                        //get bone hash of slot

                        //get verts specified for the intersection
                        int[] vertIndices = slotRayIntersection.TrianglePointIndices;
                        //get the barycentric coordinates for the intersection
                        Vector2 coordinates = slotRayIntersection.Coordinates;
                        //offset from intersection OS is the offset of the slot from the point on the face.
                        Vector3 offsetFromIntersectionOS = slotRayIntersection.OffsetFromIntersectionOS;
                        //get vertex positions AFTER dmaps and bonedeltas
                        Vector3[] vertexPositions = morphMesh.SlotrayTrianglePositions(slotRayIdx);
                        //calculate the point on the face from the barycentric coordinates
                        float xPos = (1 - coordinates.X - coordinates.Y) * vertexPositions[0].X + (coordinates.X * vertexPositions[1].X) + (coordinates.Y * vertexPositions[2].X);
                        float yPos = (1 - coordinates.X - coordinates.Y) * vertexPositions[0].Y + (coordinates.X * vertexPositions[1].Y) + (coordinates.Y * vertexPositions[2].Y);
                        float zPos = (1 - coordinates.X - coordinates.Y) * vertexPositions[0].Z + (coordinates.X * vertexPositions[1].Z) + (coordinates.Y * vertexPositions[2].Z);
                        Vector3 slotBasePosition = new Vector3(xPos, yPos, zPos);
                        Vector3 offsetFromIntersectionOSForExport = new Vector3(offsetFromIntersectionOS.X, offsetFromIntersectionOS.Y, offsetFromIntersectionOS.Z);
                        try
                        {
                            RIG.Bone currentBone = null;
                            if (slotRayIntersection.SlotHash != 0xFFFFFFFF)
                            {
                                uint slotHash = 0;
                                try
                                {
                                    slotHash = slotRayIntersection.SlotHash;
                                    currentBone = currentRig.GetBone(slotHash);
                                }

                                catch
                                {
                                    errorList += "Slot Assignment specified a hash that doesn't exist on the rig. " + slotHash.ToString() + System.Environment.NewLine;
                                    continue;

                                }

                            }
                            else
                            {
                                uint slotIdx = 0;

                                slotIdx = slotRayIntersection.SlotIndex;
                                if (slotIdx != 0xFFFFFFFF)
                                {
                                    try
                                    {

                                        currentBone = currentRig.Bones[slotIdx];
                                    }
                                    catch
                                    {
                                        errorList += "Slot Index is out of bounds of the bones of the rig. " + slotIdx.ToString() + System.Environment.NewLine;
                                        continue;

                                    }
                                }
                            }

                            if (currentBone == null)
                            {
                                continue;
                            }

                            try
                            {
                                if (morphedSlotBasePosition.ContainsKey(currentBone))
                                {
                                    if (morphedSlotBasePosition[currentBone].Magnitude < slotBasePosition.Magnitude)
                                    {
                                        morphedSlotBasePosition[currentBone] = slotBasePosition;
                                        morphedSlotBasePositionOffsets[currentBone] = offsetFromIntersectionOSForExport;
                                    }
                                }
                                else
                                {
                                    morphedSlotBasePosition[currentBone] = slotBasePosition;
                                    morphedSlotBasePositionOffsets[currentBone] = offsetFromIntersectionOSForExport;

                                }
                            }
                            catch (Exception ex)
                            {
                                errorList += ex.ToString() + Environment.NewLine;
                            }
                            finally
                            {
                                slotRayIdx += 1;

                            }
                        }
                        catch (Exception ex)
                        {
                            errorList += ex.ToString() + " " + Environment.NewLine;

                        }
                    }
                }

                catch (Exception ex)
                {
                    errorList += ex.ToString() + Environment.NewLine;
                }
            }
            //Finally, it adds the base slot position and the offset together. It also has to be converted to local space as the position is in world space.
            // NOTE! I had to make a setter for PositionVector. I didn't see any functions for updating a bone's position but not it's rotation.
            // Finally, calculate transforms.

            foreach (RIG.Bone slot in morphedSlotBasePosition.Keys)
            {
                Vector3 basePosition = morphedSlotBasePosition[slot];
                Vector3 offsetForBasePosition = morphedSlotBasePositionOffsets[slot];
                if(slot.ParentBone !=null){
                    Vector3 pos = (slot.ParentBone.GlobalTransform.Inverse() * slot.LocalRotation.toMatrix4D(basePosition + offsetForBasePosition)).Offset;
                    slot.PositionVector = pos;
                    slot.CalculateTransforms();
                }
            }
        }

        //public Bitmap CompositeTexture()
        //{
        //    if (currentSkin == null && currentMakeup == null && currentClothing == null) return null;
        //    Bitmap currentTexture = new Bitmap(currentSkin.Width, currentSkin.Height);
        //    using (Graphics g = Graphics.FromImage(currentTexture))
        //    {
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        if (currentSkin != null)
        //        {
        //            if (currentSpecies == Species.Human && (SeparateMeshes_comboBox.SelectedIndex < 2))
        //            {
        //                int cutoff = currentSkin.Height / 4;
        //                g.DrawImage(currentSkin, new Rectangle(0, cutoff, currentSkin.Width, currentSkin.Height), 0, cutoff, currentTexture.Width, currentTexture.Height, GraphicsUnit.Pixel);
        //            }
        //            else
        //            {
        //                g.DrawImage(currentSkin, new Point(0, 0));
        //            }
        //        }
        //        if (currentMakeup != null) g.DrawImage(currentMakeup, new Point(0, 0));
        //        if (currentClothing != null) g.DrawImage(currentClothing, new Point(0, 0));
        //    }
        //    return currentTexture;
        //}

        //public Image CompositeSpecular()
        //{
        //    if (currentMakeupSpecular == null && currentClothingSpecular == null) return null;
        //    Image currentSpecular = new Bitmap(currentSkin.Width / 2, currentSkin.Height / 2);
        //    using (Graphics g = Graphics.FromImage(currentSpecular))
        //    {
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        if (currentMakeupSpecular != null) g.DrawImage(currentMakeupSpecular, new Point(0, 0));
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        if (currentClothingSpecular != null) g.DrawImage(currentClothingSpecular, new Point(0, 0));
        //    }
        //    return currentSpecular;
        //}

        public bool isMakeup(BodyType part, bool isMesh)
        {
            if (part == BodyType.Socks || part == BodyType.Tights || part == BodyType.Toenails) return false;
            if (part == BodyType.Eyecolor || part == BodyType.SecondaryEyeColor) return true;
            return !isMesh;
        }

        internal class MeshInfo
        {
            internal BodyType partType;
            internal Species species;
            internal AgeGender age;
            internal AgeGender gender;
            internal GEOM geom;
            internal List<uint> regions;
            internal List<float> layers;
            internal MeshInfo(BodyType partType, Species species, AgeGender age, AgeGender gender, GEOM mesh, uint[] regions, float[] layers)
            {
                this.partType = partType;
                this.species = species;
                this.age = age;
                this.gender = gender;
                this.geom = mesh;
                this.regions = new List<uint>(regions);
                this.layers = new List<float>(layers);
            }
            /// <summary>
            /// Returns bool indicating whether this mesh is overridden by a mesh in the same region with a higher layer
            /// </summary>
            /// <param name="meshRegions">Array of regions of the mesh being tested</param>
            /// <param name="layers">Array of layers of the mesh being tested</param>
            /// <returns></returns>
            internal bool rejectMesh(uint[] meshRegions, float[] meshLayers)
            {
                for (int i = 0; i < meshRegions.Length; i++)
                {
                    if (meshLayers[i] == 0) return false;
                    int index = this.regions.IndexOf(meshRegions[i]);
                    if (index >= 0 && this.layers[index] > meshLayers[i]) return true;
                }
                return false;
            }
        }

        //internal class RegionInfo
        //{
        //    internal uint[] region;
        //    internal float[] layer;
        //    internal RegionInfo()
        //    {
        //        int count = Enum.GetNames(typeof(CASPartRegionTS4)).Length;
        //        this.region = new uint[count];
        //        this.layer = new float[count];
        //    }
        //    internal RegionInfo(uint[] regions, float[] layers)
        //    {
        //        bool suppressCalf = false, suppressAnkle = false;
        //        for (int i = 0; i < regions.Length; i++)
        //        {
        //            CASPartRegionTS4 tmpRegion = (CASPartRegionTS4)regions[i];
        //            if (tmpRegion == CASPartRegionTS4.Knee && layers[i] < this.layer[(uint)CASPartRegionTS4.Knee])
        //            {
        //                suppressCalf = true;
        //                suppressAnkle = true;
        //            }
        //            else if (tmpRegion == CASPartRegionTS4.Calf && layers[i] < this.layer[(uint)CASPartRegionTS4.Calf])
        //            {
        //                suppressAnkle = true;
        //            }
        //        }
        //        for (int i = 0; i < regions.Length; i++)
        //        {
        //            CASPartRegionTS4 tmpRegion = (CASPartRegionTS4)regions[i];
        //            if (tmpRegion == CASPartRegionTS4.Calf && suppressCalf)
        //            {

        //            }
        //            if (tmpRegion == CASPartRegionTS4.Calf && layers[i] < this.layer[(uint)CASPartRegionTS4.Calf]) suppressUnderCalf = true;
        //        }
        //    }
        //}

        internal class ImageStack
        {
            internal int sortLayer;
            internal int compositionMethod;
            internal byte[] colorShifts;
            internal BodyType partType;
            internal bool isMesh;
            internal Image image;
            internal Image shadow;
            internal Image specular;
            internal Image bumpmap;
            internal Image emission;
            internal ImageStack(int sortLayer, int compositionMethod, ulong colorShifts, BodyType partType, bool isMesh, Image texture, Image shadow, Image specular, Image bumpmap, Image emissionmap)
            {
                this.sortLayer = sortLayer;
                this.compositionMethod = compositionMethod;
                this.colorShifts = BitConverter.GetBytes(colorShifts);
                this.partType = partType;
                this.isMesh = isMesh;
                this.image = texture;
                this.shadow = shadow;
                this.specular = specular;
                this.bumpmap = bumpmap;
                this.emission = emissionmap;
            }
            internal float Opacity
            {
                get
                {
                    short tmp = BitConverter.ToInt16(this.colorShifts, 6);
                    return (float)tmp / 0x4000;
                }
            }
            internal float HueShift
            {
                get
                {
                    short tmp = BitConverter.ToInt16(this.colorShifts, 4);
                    return (float)tmp / 0x4000;
                }
            }
            internal float SaturationShift
            {
                get
                {
                    short tmp = BitConverter.ToInt16(this.colorShifts, 2);
                    return (float)tmp / 0x4000;
                }
            }
            internal float BrightnessShift
            {
                get
                {
                    short tmp = BitConverter.ToInt16(this.colorShifts, 0);
                    return (float)tmp / 0x4000;
                }
            }
        }

        private void SaveModelMorph(MeshFormat format)
        {
            string savename = "";
            Working_label.Visible = true;
            Working_label.Refresh();
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            List<GEOM> geomList = new List<GEOM>();
            List<string> nameList = new List<string>();
            if (SeparateMeshes_comboBox.SelectedIndex == 0)     //single mesh
            {
                GEOM tmp = null;
                for (int i = CurrentModel.Length - 1; i >= 0; i--)   
                {
                    if (CurrentModel[i] != null)
                    {
                        if (tmp == null) tmp = new GEOM(CurrentModel[i]);
                        else tmp.AppendMesh(CurrentModel[i]);
                    }
                    if (GlassModel[i] != null)
                    {
                        if (tmp == null) tmp = new GEOM(GlassModel[i]);
                        else tmp.AppendMesh(GlassModel[i]);
                    }
                }
                //if (CurrentModel[(int)BodyType.Hair] != null) tmp.AppendMesh(CurrentModel[(int)BodyType.Hair]);       //hair 
                //if (GlassModel[(int)BodyType.Hair] != null) tmp.AppendMesh(GlassModel[(int)BodyType.Hair]);
                //if (CurrentModel[(int)BodyType.Hat] != null) tmp.AppendMesh(CurrentModel[(int)BodyType.Hat]);
                //if (GlassModel[(int)BodyType.Hat] != null) tmp.AppendMesh(GlassModel[(int)BodyType.Hat]);
                geomList.Add(tmp);
                nameList.Add(basename);
            }
            else if (SeparateMeshes_comboBox.SelectedIndex == 1)                                             //all separate meshes
            {
                for (int i = CurrentModel.Length - 1; i >= 0; i--)
                {
                    if (CurrentModel[i] != null || GlassModel[i] != null)
                    {
                        GEOM tmp = CurrentModel[i] != null ? new GEOM(CurrentModel[i]) : new GEOM(GlassModel[i]);
                        if (CurrentModel[i] != null && GlassModel[i] != null) tmp.AppendMesh(GlassModel[i]);
                        geomList.Add(tmp);
                        nameList.Add(partNames[i]);
                    }
                }
                //if (CurrentModel[(int)BodyType.Hair] != null || GlassModel[(int)BodyType.Hair] != null)
                //{
                //    GEOM tmp = CurrentModel[(int)BodyType.Hair] != null ? new GEOM(CurrentModel[(int)BodyType.Hair]) : new GEOM(GlassModel[(int)BodyType.Hair]);
                //    if (CurrentModel[(int)BodyType.Hair] != null && GlassModel[(int)BodyType.Hair] != null) tmp.AppendMesh(GlassModel[(int)BodyType.Hair]);
                //    geomList.Add(tmp);      //hair
                //    nameList.Add(partNames[(int)BodyType.Hair]);
                //}
                //if (CurrentModel[(int)BodyType.Hat] != null || GlassModel[(int)BodyType.Hat] != null)
                //{
                //    GEOM tmp = CurrentModel[(int)BodyType.Hat] != null ? new GEOM(CurrentModel[(int)BodyType.Hat]) : new GEOM(GlassModel[(int)BodyType.Hat]);
                //    if (CurrentModel[(int)BodyType.Hat] != null && GlassModel[(int)BodyType.Hat] != null) tmp.AppendMesh(GlassModel[(int)BodyType.Hat]);
                //    geomList.Add(tmp);      //hat
                //    nameList.Add(partNames[(int)BodyType.Hat]);
                //}
            }
            else                                                    //solid mesh and glass mesh
            {
                GEOM solid = null;
                GEOM glass = null;
                for (int i = CurrentModel.Length - 1; i >= 0; i--)
                {
                    if (CurrentModel[i] != null)
                    {
                        if (solid == null) solid = new GEOM(CurrentModel[i]);
                        else solid.AppendMesh(CurrentModel[i]);
                    }
                    if (GlassModel[i] != null)
                    {
                        if (glass == null) glass = new GEOM(GlassModel[i]);
                        else glass.AppendMesh(GlassModel[i]);
                    }
                }
                //if (CurrentModel[(int)BodyType.Hair] != null) solid.AppendMesh(CurrentModel[(int)BodyType.Hair]);       //hair 
                //if (GlassModel[(int)BodyType.Hair] != null) glass.AppendMesh(GlassModel[(int)BodyType.Hair]);
                //if (CurrentModel[(int)BodyType.Hat] != null) solid.AppendMesh(CurrentModel[(int)BodyType.Hat]);
                //if (GlassModel[(int)BodyType.Hat] != null) glass.AppendMesh(GlassModel[(int)BodyType.Hat]);
                if (solid != null) geomList.Add(solid);
                if (glass != null) geomList.Add(glass);
                if (solid != null) nameList.Add(basename);
                if (glass != null) nameList.Add(basename + "_glass");
            }

            if (format == MeshFormat.OBJ)
            {
                OBJ obj = new OBJ(geomList.ToArray(), 0, false, nameList.ToArray());
                Working_label.Visible = false;
                savename = WriteOBJFile("Save OBJ of morphed model", obj, path + "\\" + basename);
                basename = Path.GetFileNameWithoutExtension(savename);
            }
            else if (format == MeshFormat.MS3D)
            {
                MS3D ms3d = new MS3D(geomList.ToArray(), currentRig, 0, nameList.ToArray());
                Working_label.Visible = false;
                savename = WriteMS3D("Save MS3D of morphed model", ms3d, path + "\\" + basename);
                basename = Path.GetFileNameWithoutExtension(savename);
            }
            else if (format == MeshFormat.DAE)
            {
                DAE dae = new DAE(geomList.ToArray(), nameList.ToArray(), currentRig, false, CleanDAE_checkBox.Checked, ref errorList);
                Working_label.Text = "Saving....";
                savename = WriteDAEFile("Save Collada DAE of morphed model", dae, false, path, basename);
                Working_label.Text = "Working....";
                Working_label.Visible = false;
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = Imagefilter;
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.Title = "Save Textures";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "png";
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.FileName = path + "\\" + basename;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    savename = saveFileDialog1.FileName;
                }
            }
            if (savename.Length == 0) return;
            savename = Path.GetDirectoryName(savename) + '\\' + Path.GetFileNameWithoutExtension(savename).Replace(" ", "_");
            string testname = "";
            try
            {
                testname = savename + "_diffuse.png";
                //Image currentTexture = new Bitmap(CurrentTexture());
                //if (currentTexture != null) currentTexture.Save(savename + "_diffuse.png");
                if (currentTexture != null) SaveImagePng(currentTexture, testname);
                testname = savename + "_specular.png";
                //if (currentSpecular != null) currentSpecular.Save(savename + "_specular.png");
                //Image currentSpecular = new Bitmap(CurrentSpecular());
                if (currentSpecular != null) SaveImagePng(currentSpecular, testname);
                if (SeparateMeshes_comboBox.SelectedIndex == 2)
                {
                    testname = savename + "_glass_diffuse.png";
                    if (currentGlassTexture != null) SaveImagePng(currentGlassTexture, testname);
                    testname = savename + "_glass_specular.png";
                    if (currentGlassSpecular != null) SaveImagePng(currentGlassSpecular, testname);
                    //if (currentGlassTexture != null) currentGlassTexture.Save(savename + "_glass_diffuse.png");
                    //if (currentGlassSpecular != null) currentGlassSpecular.Save(savename + "_glass_specular.png");
                }
                testname = savename + "_normalmap.png";
                //if (currentBump != null) currentBump.Save(savename + "_normalmap.png");
                if (currentBump != null) SaveImagePng(NormalConvert_checkBox.Checked ? ConvertNormalMap(currentBump) : currentBump, testname);
                testname = savename + "_emissionmap.png";
                //if (currentEmission != null) currentEmission.Save(savename + "_emissionmap.png");
                if (currentEmission != null) SaveImagePng(currentEmission, testname);
            }
            catch (Exception e)
            {
                bool dirExist = Directory.Exists(Path.GetDirectoryName(testname));
                MessageBox.Show("Unable to save image to file: " + testname + Environment.NewLine 
                    + "Directory " + Path.GetDirectoryName(testname) + " exists: " + dirExist.ToString() + Environment.NewLine
                    + e.ToString());
            }
            //if (currentSculptOverlay != null) currentSculptOverlay.Save(savename + "_sculpt.png");
            //if (currentOutfitOverlay != null) currentOutfitOverlay.Save(savename + "_outfit.png");
            //if (currentSkin != null) currentSkin.Save(savename + "_skin.png");
            Working_label.Visible = false;
            Properties.Settings.Default.LastSavePath = Path.GetDirectoryName(savename);
            Properties.Settings.Default.Save();
        }

        private void SaveTextures_button_Click(object sender, EventArgs e)
        {
            //Image currentTexture = new Bitmap(CurrentTexture());
            if (currentTexture == null)
            {
                MessageBox.Show("There is no diffuse texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Save Combined Texture";
            saveFileDialog1.FileName = path + "\\" + basename + "_Diffuse.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                if (currentTexture != null) SaveImagePng(currentTexture, saveFileDialog1.FileName);
            }
            //Image currentSpecular = new Bitmap(CurrentSpecular());
            if (currentSpecular != null)
            {
                saveFileDialog1.Title = "Save Combined Specular";
                saveFileDialog1.FileName = path + "\\" + basename + "_Specular.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName.Length == 0) return;
                    SaveImagePng(currentSpecular, saveFileDialog1.FileName);
                }
            }
        }

        private void SaveGlass_button_Click(object sender, EventArgs e)
        {
            if (currentGlassTexture == null)
            {
                MessageBox.Show("There is no glass texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Save Glass Texture";
            saveFileDialog1.FileName = path + "\\" + basename + "_Glass_Diffuse.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                //if (currentGlassTexture != null) currentGlassTexture.Save(saveFileDialog1.FileName);
                if (currentGlassTexture != null) SaveImagePng(currentGlassTexture, saveFileDialog1.FileName);
            }
            if (currentGlassSpecular != null)
            {
                saveFileDialog1.Title = "Save Glass Specular";
                saveFileDialog1.FileName = path + "\\" + basename + "_Glass_Specular.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName.Length == 0) return;
                    SaveImagePng(currentGlassSpecular, saveFileDialog1.FileName);
                }
            }
        }

        private void SaveSkin_button_Click(object sender, EventArgs e)
        {
            if (currentSkin == null)
            {
                MessageBox.Show("There is no skin texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Save Skin Texture";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = path + "\\" + basename + "_Skin_Texture.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                if (currentSkin != null) SaveImagePng(currentSkin, saveFileDialog1.FileName);
            }
        }

        private void SaveClothing_button_Click(object sender, EventArgs e)
        {
            if (currentClothing == null)
            {
                MessageBox.Show("There is no clothing texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Save Clothing Texture";
            saveFileDialog1.FileName = path + "\\" + basename + "_Clothing_Diffuse.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                //if (currentGlassTexture != null) currentGlassTexture.Save(saveFileDialog1.FileName);
                if (currentClothing != null) SaveImagePng(currentClothing, saveFileDialog1.FileName);
            }
            if (currentClothingSpecular != null)
            {
                saveFileDialog1.Title = "Save Clothing Specular";
                saveFileDialog1.FileName = path + "\\" + basename + "_Clothing_Specular.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName.Length == 0) return;
                    SaveImagePng(currentClothingSpecular, saveFileDialog1.FileName);
                }
            }
        }

        private void SaveMakeup_button_Click(object sender, EventArgs e)
        {
            if (currentMakeup == null)
            {
                MessageBox.Show("There is no makeup texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Save Makeup Texture";
            saveFileDialog1.FileName = path + "\\" + basename + "_Makeup_Diffuse.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                if (currentMakeup != null) SaveImagePng(currentMakeup, saveFileDialog1.FileName);
            }
            if (currentMakeupSpecular != null)
            {
                saveFileDialog1.Title = "Save Makeup Specular";
                saveFileDialog1.FileName = path + "\\" + basename + "_Makeup_Specular.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName.Length == 0) return;
                    SaveImagePng(currentMakeupSpecular, saveFileDialog1.FileName);
                }
            }
        }

        private void SaveEmission_button_Click(object sender, EventArgs e)
        {
            if (currentEmission == null)
            {
                MessageBox.Show("There is no emission/glow map texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Save Emission Map Texture";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = path + "\\" + basename + "_EmissionMap.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                if (currentEmission != null) SaveImagePng(currentEmission, saveFileDialog1.FileName);
            }
        }

        private void SaveNormals_button_Click(object sender, EventArgs e)
        {
            if (currentBump == null)
            {
                MessageBox.Show("There is no bump/normals map texture for this model!");
                return;
            }
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Save Normal Map Texture";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = path + "\\" + basename + "_NormalMap.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length == 0) return;
                if (currentBump != null) SaveImagePng(NormalConvert_checkBox.Checked ? ConvertNormalMap(currentBump) : currentBump, saveFileDialog1.FileName);
            }
        }

        private void SimTrouble_button_Click(object sender, EventArgs e)
        {
            string path = Properties.Settings.Default.LastSavePath;
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(currentName);
            string basename = System.Text.Encoding.UTF8.GetString(tempBytes).Replace(" ", "");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Save zip file with sim CC";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "zip";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = path + "\\" + basename + "_TroubleShoot.zip";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFileDialog1.FileName)) File.Delete(saveFileDialog1.FileName);
                using (ZipArchive zip = ZipFile.Open(saveFileDialog1.FileName, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry info = zip.CreateEntry("Sim Information.txt");
                    using (Stream zipEntryStream = info.Open())
                    {
                        MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes((string)SimInfo_button.Tag));
                        ms.CopyTo(zipEntryStream);
                    }
                    if (errorList.Length > 0)
                    {
                        ZipArchiveEntry err = zip.CreateEntry("Sim Errors.txt");
                        using (Stream zipEntryStream = err.Open())
                        {
                            MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(errorList));
                            ms.CopyTo(zipEntryStream);
                        }
                    }
                    if (currentSaveGame != null)
                    {
                        ZipArchiveEntry save = zip.CreateEntry(currentSaveName + ".save");
                        using (Stream zipEntryStream = save.Open())
                        {
                            Stream s = new MemoryStream();
                            currentSaveGame.SaveAs(s);
                            s.Position = 0;
                            s.CopyTo(zipEntryStream);
                        }
                    }
                    if (TroubleshootPackageTuning != null && TroubleshootPackageTuning.GetResourceList.Count > 0)
                    {
                        ZipArchiveEntry tune = zip.CreateEntry("CASModifierCCTuning.package");
                        using (Stream zipEntryStream = tune.Open())
                        {
                            Stream s = new MemoryStream();
                            TroubleshootPackageTuning.SaveAs(s);
                            s.Position = 0;
                            s.CopyTo(zipEntryStream);
                        }
                    }
                    if (TroubleshootPackageBasic != null)
                    {
                        ZipArchiveEntry entry = zip.CreateEntry("Basic Sim Resources.package");
                        using (Stream zipEntryStream = entry.Open())
                        {
                            Stream s = new MemoryStream();
                            TroubleshootPackageBasic.SaveAs(s);
                            s.Position = 0;
                            s.CopyTo(zipEntryStream);
                        }
                    }
                    if (TroubleshootPackageOutfit != null)
                    {
                        ZipArchiveEntry outfit = zip.CreateEntry(Outfits_comboBox.SelectedItem.ToString() + " Outfit Resources.package");
                        using (Stream zipEntryStream = outfit.Open())
                        {
                            Stream s = new MemoryStream();
                            TroubleshootPackageOutfit.SaveAs(s);
                            s.Position = 0;
                            s.CopyTo(zipEntryStream);
                        }
                    }
                }
            }
        }

        public static string GetBodyCompletePrefix(Species species, AgeGender age, AgeGender gender)
        {
            string specifier = "";
            if (age == AgeGender.Toddler) specifier = (species == Species.Human ? "p" : "c");
            else if (age == AgeGender.Child) specifier = "c";
            else specifier = (species == Species.Human ? "y" : "a");
            if (species != Species.Human) specifier +=
                (age == AgeGender.Child && species == Species.LittleDog) ? "d" :
                species.ToString().Substring(0, 1).ToLower();
            else if (age <= AgeGender.Child) specifier += "u";
            else specifier += (gender == AgeGender.Male || gender == AgeGender.Female) ? gender.ToString().Substring(0, 1).ToLower() : "f";
            return specifier;
        }

        public static string GetPhysiquePrefix(Species species, AgeGender age, AgeGender gender)
        {
            string specifier = "";
            if (age == AgeGender.Toddler) specifier = (species == Species.Human ? "p" : "c");
            else if (species == Species.Human && age >= AgeGender.Teen && age <= AgeGender.Adult) specifier = "y";
            else if (species != Species.Human && age >= AgeGender.Teen && age <= AgeGender.Elder) specifier = "a";
            else specifier = age.ToString().Substring(0, 1).ToLower();
            if (species != Species.Human) specifier +=
                (age == AgeGender.Child && species == Species.LittleDog) ? "d" :
                species.ToString().Substring(0, 1).ToLower();
            else if (age <= AgeGender.Child) specifier += "u";
            else specifier += (gender == AgeGender.Male || gender == AgeGender.Female) ? gender.ToString().Substring(0, 1).ToLower() : "f";
            return specifier;
        }

        public static string GetRigPrefix(Species species, AgeGender age, AgeGender gender)
        {
            string specifier = "";
            if (age == AgeGender.Infant) specifier = (species == Species.Human ? "i" : "c");
            else if (age == AgeGender.Toddler) specifier = (species == Species.Human ? "p" : "c");
            else if (age == AgeGender.Child) specifier = "c";
            else specifier = "a";
            if (species != Species.Human) specifier +=
                (age == AgeGender.Child && species == Species.LittleDog) ? "d" :
                species.ToString().Substring(0, 1).ToLower();
            else specifier += "u";
            return specifier;
        }

        private Image DisplayableShadow(Image shadowImage)
        {
            if (shadowImage == null) return null;
            Bitmap shadow = new Bitmap(shadowImage);
            Rectangle rect = new Rectangle(0, 0, shadow.Width, shadow.Height);
            System.Drawing.Imaging.BitmapData bmpData = shadow.LockBits(rect, ImageLockMode.ReadWrite,
                shadow.PixelFormat);

            IntPtr ptr;
            if (bmpData.Stride > 0)
            {
                ptr = bmpData.Scan0;
            }
            else
            {
                ptr = bmpData.Scan0 + bmpData.Stride * (shadow.Height - 1);
            }

            int bytes = Math.Abs(bmpData.Stride) * shadow.Height;
            byte[] argbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, bytes);

            for (int i = 0; i < argbValues.Length; i += 4)
            {
                if (argbValues[i + 3] > 0)
                {
                    int tmp = (int)(((((int)argbValues[i + 2] + (int)argbValues[i]) / 2.0) - 150) * 3.5);
                    tmp = Math.Min(tmp, 255);
                    tmp = Math.Max(tmp, 0);
                    int a = (255 - tmp) / 2;
                    argbValues[i + 3] = (byte)a;        //alpha
                    argbValues[i + 2] = 0;              //red
                    argbValues[i + 1] = 0;              //green
                    argbValues[i] = 0;                  //blue
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);
            shadow.UnlockBits(bmpData);

            return shadow;
        }

        private Image DisplayableSpecular(Image specularImage)
        {
            if (specularImage == null) return null;
            //Bitmap specular = new Bitmap(specularImage);

            //Rectangle rect = new Rectangle(0, 0, specular.Width, specular.Height);
            //System.Drawing.Imaging.BitmapData bmpData = specular.LockBits(rect, ImageLockMode.ReadWrite, specular.PixelFormat);
            //IntPtr ptr;
            //if (bmpData.Stride > 0) ptr = bmpData.Scan0;
            //else ptr = bmpData.Scan0 + bmpData.Stride * (specular.Height - 1);
            //int bytes = Math.Abs(bmpData.Stride) * specular.Height;
            //byte[] argbValues = new byte[bytes];
            //System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, bytes);

            //for (int i = 0; i < argbValues.Length; i += 4)
            //{
            //    argbValues[i + 2] = 225;          //red
            //    argbValues[i + 1] = 225;          //green
            //    argbValues[i] = 225;              //blue
            //}
            //System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);
            //specular.UnlockBits(bmpData);

            Bitmap specular = new Bitmap(specularImage.Width, specularImage.Height);
            float[][] specMatrix = {
                   new float[] {0, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 0, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 0, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {255, 255, 255, 0, 1}        // increments for R, G, B, A
                };
            using (Graphics gr = Graphics.FromImage(specular))
            {
                ColorMatrix spec = new ColorMatrix(specMatrix);
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(spec, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                gr.DrawImage(specularImage, new Rectangle(0, 1, specular.Width, specular.Height), 0, 0, specularImage.Width, specularImage.Height, GraphicsUnit.Pixel, attributes);
            }

            return specular;
        }

        private Bitmap LayerWithMask(Image baseImage, Image layerImage, Image maskImage, bool isMesh)
        {
            if (baseImage == null) return (Bitmap)layerImage;
            if (layerImage == null || maskImage == null) return (Bitmap)baseImage;
            Bitmap baseLayer = (Bitmap)baseImage;
            Bitmap topLayer = (Bitmap)layerImage;
            Bitmap mask = new Bitmap(maskImage, baseLayer.Width, baseLayer.Height);

            Rectangle rect = new Rectangle(0, 0, baseLayer.Width, baseLayer.Height);
            System.Drawing.Imaging.BitmapData bmpData = baseLayer.LockBits(rect, ImageLockMode.ReadWrite, baseLayer.PixelFormat);
            IntPtr ptr;
            if (bmpData.Stride > 0) ptr = bmpData.Scan0;
            else ptr = bmpData.Scan0 + bmpData.Stride * (baseLayer.Height - 1);
            int bytes = Math.Abs(bmpData.Stride) * baseLayer.Height;
            byte[] baseValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, baseValues, 0, bytes);

            Rectangle rect2 = new Rectangle(0, 0, topLayer.Width, topLayer.Height);
            System.Drawing.Imaging.BitmapData bmpData2 = topLayer.LockBits(rect2, ImageLockMode.ReadOnly, topLayer.PixelFormat);
            IntPtr ptr2;
            if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
            else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (topLayer.Height - 1);
            int bytes2 = Math.Abs(bmpData2.Stride) * topLayer.Height;
            byte[] layerValues = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, layerValues, 0, bytes2);

            Rectangle rect3 = new Rectangle(0, 0, mask.Width, mask.Height);
            System.Drawing.Imaging.BitmapData bmpData3 = mask.LockBits(rect3, ImageLockMode.ReadOnly, mask.PixelFormat);
            IntPtr ptr3;
            if (bmpData3.Stride > 0) ptr3 = bmpData3.Scan0;
            else ptr3 = bmpData3.Scan0 + bmpData3.Stride * (mask.Height - 1);
            int bytes3 = Math.Abs(bmpData3.Stride) * mask.Height;
            byte[] maskValues = new byte[bytes3];
            System.Runtime.InteropServices.Marshal.Copy(ptr3, maskValues, 0, bytes3);

            for (int i = 0; i < baseValues.Length; i += 4)
            {
                float maskAlpha;
                if (isMesh) maskAlpha = Math.Min((maskValues[i + 3] / 255) * 3f, 1f);
                else maskAlpha = maskValues[i + 3] > 5 ? 1 : 0;
                baseValues[i + 3] = (byte)Math.Min(((layerValues[i + 3] * maskAlpha) + (baseValues[i + 3] * (1f - maskAlpha))), 255);  //alpha
                baseValues[i + 2] = (byte)Math.Min(((layerValues[i + 2] * maskAlpha) + (baseValues[i + 2] * (1f - maskAlpha))), 255);  //red
                baseValues[i + 1] = (byte)Math.Min(((layerValues[i + 1] * maskAlpha) + (baseValues[i + 1] * (1f - maskAlpha))), 255);  //green
                baseValues[i] = (byte)Math.Min(((layerValues[i] * maskAlpha) + (baseValues[i] * (1f - maskAlpha))), 255);  //blue
            }

            System.Runtime.InteropServices.Marshal.Copy(baseValues, 0, ptr, bytes);
            baseLayer.UnlockBits(bmpData);
            topLayer.UnlockBits(bmpData2);
            mask.UnlockBits(bmpData3);

            return baseLayer;
        }

        private Point GetImageStartPoint(Image myImage, BodyType partType, Species species)
        {
            if (myImage == null) return new Point();
            int x = 0, y = 0;
            if (species == Species.Human)
            {
                if (partType == BodyType.Body | partType == BodyType.Top | partType == BodyType.Bottom)
                {
                    x = 0;
                    y = 1025;
                }
                else if (partType == BodyType.Shoes)
                {
                    x = 0;
                    y = 1537;
                }
                else if (partType == BodyType.Hair)
                {
                    x = 0;
                    y = 0;
                }
                else if (partType == BodyType.Hat)
                {
                    x = 513;
                    y = 257;
                }
                else
                {
                    if (myImage.Width <= 512)
                    {
                        x = 513;
                    }
                    else
                    {
                        x = 0;
                    }
                    y = 0;
                }
            }
            else
            {
                if (partType == BodyType.Body || partType == BodyType.Hat || partType == BodyType.Necklace)
                {
                    x = 1537;
                    y = 0;
                }
            }
            return new Point(x, y);
        }

        private Image ExpandPartialImage(Image myImage, BodyType partType, Species species)
        {
            if (myImage == null) return null;
            if (species == Species.Human)
            {
                Bitmap final = new Bitmap(1024, 2048);
                using (Graphics g = Graphics.FromImage(final))
                {
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    int x, y;
                    int width, height;
                    if (partType == BodyType.Body | partType == BodyType.Top | partType == BodyType.Bottom)
                    {
                        x = 0;
                        y = 1025;
                        width = 1024;
                        height = 1024;
                    }
                    else if (partType == BodyType.Shoes)
                    {
                        x = 0;
                        y = 1537;
                        width = 1024;
                        height = 512;
                    }
                    else if (partType == BodyType.Hair)
                    {
                        x = 0;
                        y = 0;
                        width = 1024;
                        height = 1024;
                    }
                    else if (partType == BodyType.Hat)
                    {
                        x = 513;
                        y = 257;
                        width = 512;
                        height = 256;
                    }
                    else
                    {
                        if (myImage.Width <= 512)
                        {
                            x = 513;
                        }
                        else
                        {
                            x = 0;
                        }
                        y = 0;
                        width = myImage.Width;
                        height = myImage.Height;
                    }
                    g.DrawImage(myImage, x, y, width, height);
                }
                Image tmp = (Image)final;
                return tmp;
            }
            else
            {
                Bitmap final = new Bitmap(2048, 1024);
                using (Graphics g = Graphics.FromImage(final))
                {
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    int x = 0, y = 0;
                    int width = 2048, height = 1024;
                    if (myImage.Width >= 2048)
                    {
                        x = 0;
                        y = 0;
                        width = 2048;
                        height = 1024;
                    }
                    else if (partType == BodyType.Body)
                    {
                        x = 1537;
                        y = 0;
                        width = 512;
                        height = 1024;
                    }
                    else if (partType == BodyType.Hat)
                    {
                        x = 0;
                        y = 0;
                        width = 2048;
                        height = 1024;
                    }
                    else if (partType == BodyType.Necklace)
                    {
                        x = 1537;
                        y = 0;
                        width = 512;
                        height = 128;
                    }

                    g.DrawImage(myImage, x, y, width, height);
                }
                Image tmp = (Image)final;
                return tmp;
            }
        }

        private Image ConvertNormalMap(Image normal)
        {
            if (normal == null) return null;
            Bitmap n = new Bitmap(normal);

            Rectangle rect = new Rectangle(0, 0, n.Width, n.Height);
            System.Drawing.Imaging.BitmapData bmpData = n.LockBits(rect, ImageLockMode.ReadWrite, n.PixelFormat);
            IntPtr ptr;
            if (bmpData.Stride > 0) ptr = bmpData.Scan0;
            else ptr = bmpData.Scan0 + bmpData.Stride * (n.Height - 1);
            int bytes = Math.Abs(bmpData.Stride) * n.Height;
            byte[] argbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, bytes);

            for (int i = 0; i < argbValues.Length; i += 4)
            {
                argbValues[i + 2] = argbValues[i + 3];          //red = alpha
                argbValues[i] = 255;              //blue
                argbValues[i + 3] = 255;          //alpha
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);
            n.UnlockBits(bmpData);
            return n;
        }

        private void SeparateMeshes_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentSkin == null) return;
            morphPreview1.Stop_Mesh();
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
        }
    }
}
