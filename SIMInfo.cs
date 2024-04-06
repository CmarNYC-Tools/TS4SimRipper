/* TS4 SimRipper Sim Information / Outfit resource,
   Copyright (C) 2023  C. Marinetti

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
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using ProtoBuf;

namespace TS4SimRipper
{
    public class SimInfo
    {
        uint version;
        float[] physique;  //physique_heavy, physique_fit, physique_lean, physique_bony, hips_wide, hips_narrow, waist_wide, waist_narrow
        AgeGender age;
        AgeGender gender;
        Species species;
        uint unknown1;
        Pronoun[] pronouns;
        ulong skintoneRef;
        float skintoneShift;
        PeltLayerData[] peltLayers;
        TGI[] sculpts;
        SimModifierData[] faceModifiers;
        SimModifierData[] bodyModifiers;
        uint voiceActor;
        float voicePitch;
        ulong voiceEffect;
        uint unknown2, unknown3;
        OutfitData[] simOutfits;
        TGI[] sculptsGenetic;
        SimModifierData[] faceModifiersGenetic;
        SimModifierData[] bodyModifiersGenetic;
        float[] genetic_physique;    //genetic_heavy, genetic_fit, genetic_lean, genetic_bony;
        PartEntryGenetic[] CASPartsGenetic;
        PartEntryGenetic[] GrowthPartsGenetic;
        uint voiceActorGenetic;
        float voicePitchGenetic;
        byte flags;
        ulong aspirationRef;
        byte[] unknown4;
        ulong[] traitRefs;
        TGI[] linkList;

        public SimInfo(BinaryReader br)
        {
            this.version = br.ReadUInt32();
            uint offset = br.ReadUInt32();
            long position = br.BaseStream.Position;
            br.BaseStream.Position = position + offset;
            byte linksNum = br.ReadByte();
            this.linkList = new TGI[linksNum];
            for (int i = 0; i < linksNum; i++)
            {
                this.linkList[i] = new TGI(br, TGI.TGIsequence.IGT);
            }
            br.BaseStream.Position = position;
            this.physique = new float[8];
            for (int i = 0; i < physique.Length; i++)
            {
                this.physique[i] = br.ReadSingle();
            }
            this.age = (AgeGender)br.ReadUInt32();
            this.gender = (AgeGender)br.ReadUInt32();
            if (this.version > 18)
            {
                this.species = (Species)br.ReadUInt32();
                this.unknown1 = br.ReadUInt32();
            }
            if (this.version >= 32)
            {
                int num2 = br.ReadInt32();
                this.pronouns = new Pronoun[num2];
                for (int i = 0; i < num2; i++)
                {
                    this.pronouns[i] = new Pronoun(br);
                }
            }
            this.skintoneRef = br.ReadUInt64();
            if (this.version >= 28)
                this.skintoneShift = br.ReadSingle();
            if (this.version > 19)
            {
                byte num3 = br.ReadByte();
                this.peltLayers = new PeltLayerData[num3];
                for (int i = 0; i < num3; i++)
                {
                    this.peltLayers[i] = new PeltLayerData(br);
                }
            }
            byte num = br.ReadByte();
            this.sculpts = new TGI[num];
            for (int i = 0; i < num; i++)
            {
                byte index = br.ReadByte();
                this.sculpts[i] = this.linkList[index];
            }
            num = br.ReadByte();
            this.faceModifiers = new SimModifierData[num];
            for (int i = 0; i < num; i++)
            {
                this.faceModifiers[i] = new SimModifierData(br, this.linkList);
            }
            num = br.ReadByte();
            this.bodyModifiers = new SimModifierData[num];
            for (int i = 0; i < num; i++)
            {
                this.bodyModifiers[i] = new SimModifierData(br, this.linkList);
            }
            this.voiceActor = br.ReadUInt32();
            this.voicePitch = br.ReadSingle();
            this.voiceEffect = br.ReadUInt64();
            this.unknown2 = br.ReadUInt32();
            this.unknown3 = br.ReadUInt32();
            uint tmp = br.ReadUInt32();
            this.simOutfits = new OutfitData[tmp];
            for (int i = 0; i < tmp; i++)
            {
                this.simOutfits[i] = new OutfitData(br, this.linkList);
            }
            num = br.ReadByte();
            this.sculptsGenetic = new TGI[num];
            for (int i = 0; i < num; i++)
            {
                byte index = br.ReadByte();
                this.sculptsGenetic[i] = this.linkList[index];
            }
            num = br.ReadByte();
            this.faceModifiersGenetic = new SimModifierData[num];
            for (int i = 0; i < num; i++)
            {
                this.faceModifiersGenetic[i] = new SimModifierData(br, this.linkList);
            }
            num = br.ReadByte();
            this.bodyModifiersGenetic = new SimModifierData[num];
            for (int i = 0; i < num; i++)
            {
                this.bodyModifiersGenetic[i] = new SimModifierData(br, this.linkList);
            }
            this.genetic_physique = new float[4];
            for (int i = 0; i < 4; i++)
            {
                this.genetic_physique[i] = br.ReadSingle();
            }
            num = br.ReadByte();
            this.CASPartsGenetic = new PartEntryGenetic[num];
            for (int i = 0; i < num; i++)
            {
                this.CASPartsGenetic[i] = new PartEntryGenetic(br, this.linkList);
            }
            if (this.version >= 32)
            {
                num = br.ReadByte();
                this.GrowthPartsGenetic = new PartEntryGenetic[num];
                for (int i = 0; i < num; i++)
                {
                    this.GrowthPartsGenetic[i] = new PartEntryGenetic(br, this.linkList);
                }
            }
            this.voiceActorGenetic = br.ReadUInt32();
            this.voicePitchGenetic = br.ReadSingle();
            this.flags = br.ReadByte();
            this.aspirationRef = br.ReadUInt64();
            if (this.version >= 32U)
            {
                this.unknown4 = new byte[3];
                this.unknown4[0] = br.ReadByte();
                this.unknown4[1] = br.ReadByte();
                this.unknown4[2] = br.ReadByte();
            }
            num = br.ReadByte();
            this.traitRefs = new ulong[num];
            for (int i = 0; i < num; i++)
            {
                this.traitRefs[i] = br.ReadUInt64();
            }
        }

        public SimInfo(TS4SaveGame.SimData sim, Dictionary<TGI, uint> GameTGIs)
        {
            this.version = 0x20;
            string[] physique = sim.physique.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.physique = new float[8];
            for (int i = 0; i < 8; i++)
            {
                float.TryParse(physique[i], NumberStyles.Float, CultureInfo.InvariantCulture, out this.physique[i]);
            }            
            this.age = (AgeGender)sim.age;
            this.gender = (AgeGender)sim.gender;
            this.species = (Species)sim.extended_species;
            if (sim.extended_species == 0) this.species = Species.Human;
            this.unknown1 = 1;
            if (sim.pronouns != null & sim.pronouns.pronouns != null)
            {
                this.pronouns = new Pronoun[sim.pronouns.pronouns.Length];
                for (int i = 0; i < sim.pronouns.pronouns.Length; i++)
                {
                    this.pronouns[i] = new Pronoun((GrammaticalCase)((uint)sim.pronouns.pronouns[i].thecase), sim.pronouns.pronouns[i].pronoun);
                }
            }
            else
            {
                this.pronouns = new Pronoun[0];
            }
            this.skintoneRef = sim.skin_tone;
            this.skintoneShift = sim.skin_tone_val_shift;
            if (sim.pelt_layers != null && sim.pelt_layers.layers != null)
            {
                List<PeltLayerData> pelts = new List<PeltLayerData>();
                for (int i = 0; i < sim.pelt_layers.layers.Length; i++)
                {
                    pelts.Add(new PeltLayerData(sim.pelt_layers.layers[i].layer_id, sim.pelt_layers.layers[i].color));
                }
                this.peltLayers = pelts.ToArray();
            }
            else
            {
                this.peltLayers = new PeltLayerData[0];
            }
            Stream s = new MemoryStream(sim.facial_attr);
            TS4SaveGame.BlobSimFacialCustomizationData morphs = Serializer.Deserialize<TS4SaveGame.BlobSimFacialCustomizationData>(s);
            ulong[] simSculpts = morphs.sculpts != null ? morphs.sculpts : new ulong[0];
            this.sculpts = new TGI[simSculpts.Length];
            for (int i = 0; i < simSculpts.Length; i++)
            {
                this.sculpts[i] = new TGI((uint)ResourceTypes.Sculpt, 0, simSculpts[i]);
            }
            TS4SaveGame.Modifier[] faceMods = morphs.face_modifiers != null ? morphs.face_modifiers : new TS4SaveGame.Modifier[0];
            this.faceModifiers = new SimModifierData[faceMods.Length];
            for (int i = 0; i < faceMods.Length; i++)
            {
                this.faceModifiers[i] = new SimModifierData(new TGI((uint)ResourceTypes.SimModifier, 0, faceMods[i].key), faceMods[i].amount);
            }
            TS4SaveGame.Modifier[] bodyMods = morphs.body_modifiers != null ? morphs.body_modifiers : new TS4SaveGame.Modifier[0];
            this.bodyModifiers = new SimModifierData[bodyMods.Length];
            for (int i = 0; i < bodyMods.Length; i++)
            {
                this.bodyModifiers[i] = new SimModifierData(new TGI((uint)ResourceTypes.SimModifier, 0, bodyMods[i].key), bodyMods[i].amount);
            }
            this.voiceActor = sim.voice_actor;
            this.voiceEffect = sim.voice_effect;
            this.voicePitch = sim.voice_pitch;
            this.unknown2 = this.species == Species.Human ? 0 : 0xFFFFFFFF;
            this.unknown3 = this.species == Species.Human ? 0 : 0x8003FC57;

            List<OutfitData> outfitList = new List<OutfitData>();
            for (int i = 0; i < sim.outfits.outfits.Length; i++)
            {
                List<OutfitData.OutfitDesc.PartEntry> parts = new List<OutfitData.OutfitDesc.PartEntry>();
                for (int j = 0; j < sim.outfits.outfits[i].parts.ids.Length; j++)
                {
                    uint group = 0;
                    var tmp = GameTGIs.Where(p => p.Key.Type == (uint)ResourceTypes.CASP & p.Key.Instance == sim.outfits.outfits[i].parts.ids[j]);
                    foreach (var c in tmp.ToList())
                    {
                        group = c.Key.Group;
                        break;
                    }
                    TGI tgi = new TGI((uint)ResourceTypes.CASP, group, sim.outfits.outfits[i].parts.ids[j]);
                    parts.Add(new OutfitData.OutfitDesc.PartEntry(tgi, (BodyType)sim.outfits.outfits[i].body_types_list.body_types[j], sim.outfits.outfits[i].part_shifts.color_shift[j]));                  
                }
                OutfitData.OutfitDesc[] desc = new OutfitData.OutfitDesc[] { new OutfitData.OutfitDesc(sim.outfits.outfits[i].outfit_id,
                    sim.outfits.outfits[i].outfit_flags, sim.outfits.outfits[i].created, sim.outfits.outfits[i].match_hair_style, parts.ToArray()) };
                outfitList.Add(new OutfitData((OutfitCategory)sim.outfits.outfits[i].category, 0, desc));
            }
            this.simOutfits = outfitList.ToArray();

            Stream s2 = new MemoryStream(sim.genetic_data.sculpts_and_mods_attr);
            TS4SaveGame.BlobSimFacialCustomizationData gmorphs = Serializer.Deserialize<TS4SaveGame.BlobSimFacialCustomizationData>(s2);
            ulong[] geneticSculpts = gmorphs.sculpts != null ? gmorphs.sculpts : new ulong[0];
            this.sculptsGenetic = new TGI[geneticSculpts.Length];
            for (int i = 0; i < geneticSculpts.Length; i++)
            {
                this.sculptsGenetic[i] = new TGI((uint)ResourceTypes.Sculpt, 0, geneticSculpts[i]);
            }
            TS4SaveGame.Modifier[] faceGenetic = gmorphs.face_modifiers != null ? gmorphs.face_modifiers : new TS4SaveGame.Modifier[0];
            this.faceModifiersGenetic = new SimModifierData[faceGenetic.Length];
            for (int i = 0; i < faceGenetic.Length; i++)
            {
                this.faceModifiersGenetic[i] = new SimModifierData(new TGI((uint)ResourceTypes.SimModifier, 0, faceGenetic[i].key), faceGenetic[i].amount);
            }
            TS4SaveGame.Modifier[] bodyGenetic = gmorphs.body_modifiers != null ? gmorphs.body_modifiers : new TS4SaveGame.Modifier[0];
            this.bodyModifiersGenetic = new SimModifierData[bodyGenetic.Length];
            for (int i = 0; i < bodyGenetic.Length; i++)
            {
                this.bodyModifiersGenetic[i] = new SimModifierData(new TGI((uint)ResourceTypes.SimModifier, 0, bodyGenetic[i].key), bodyGenetic[i].amount);
            }
            string[] physiqueGenetic = sim.genetic_data.physique.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.genetic_physique = new float[4];
            for (int i = 0; i < 4; i++)
            {
                float.TryParse(physiqueGenetic[i], NumberStyles.Float, CultureInfo.InvariantCulture, out this.genetic_physique[i]);
            }
            List<PartEntryGenetic> caspGenetic = new List<PartEntryGenetic>();
            for (int i = 0; i < sim.genetic_data.parts_list.parts.Length; i++)
            {
                uint group = 0;
                var tmp = GameTGIs.Where(p => p.Key.Type == (uint)ResourceTypes.CASP & p.Key.Instance == sim.genetic_data.parts_list.parts[i].id);
                foreach (var c in tmp.ToList())
                {
                    group = c.Key.Group;
                    break;
                }
                TGI tgi = new TGI((uint)ResourceTypes.CASP, group, sim.genetic_data.parts_list.parts[i].id);
                caspGenetic.Add(new PartEntryGenetic(tgi, (BodyType)sim.genetic_data.parts_list.parts[i].body_type));
            }
            this.CASPartsGenetic = caspGenetic.ToArray();

            List<PartEntryGenetic> caspGrowth = new List<PartEntryGenetic>();
            if (sim.genetic_data.growth_parts_list != null && sim.genetic_data.growth_parts_list.parts != null)
            {
                for (int i = 0; i < sim.genetic_data.growth_parts_list.parts.Length; i++)
                {
                    uint group = 0;
                    var tmp = GameTGIs.Where(p => p.Key.Type == (uint)ResourceTypes.CASP & p.Key.Instance == sim.genetic_data.growth_parts_list.parts[i].id);
                    foreach (var c in tmp.ToList())
                    {
                        group = c.Key.Group;
                        break;
                    }
                    TGI tgi = new TGI((uint)ResourceTypes.CASP, group, sim.genetic_data.growth_parts_list.parts[i].id);
                    caspGrowth.Add(new PartEntryGenetic(tgi, (BodyType)sim.genetic_data.growth_parts_list.parts[i].body_type));
                }
            }
            this.GrowthPartsGenetic = caspGrowth.ToArray();
            this.voiceActorGenetic = sim.genetic_data.voice_actor;
            this.voicePitchGenetic = sim.genetic_data.voice_pitch;

            this.flags = (byte)sim.flags;
            this.aspirationRef = sim.primary_aspiration;
            this.unknown4 = new byte[3];
            this.traitRefs = sim.attributes.trait_tracker.trait_ids;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.version);
            List<TGI> links = new List<TGI>();
            long offsetPos = bw.BaseStream.Position;
            bw.Write(0);
            for (int i = 0; i < physique.Length; i++)
            {
                bw.Write(this.physique[i]);
            }
            bw.Write((uint)this.age);
            bw.Write((uint)this.gender);
            if (this.version > 18)
            {
                bw.Write((uint)this.species);
                bw.Write(this.unknown1);
            }
            if (this.version >= 32)
            {
                if (this.pronouns.Length == 0) this.pronouns = new Pronoun[] { new Pronoun(), new Pronoun(), new Pronoun(), new Pronoun(), new Pronoun() };
                bw.Write(this.pronouns.Length);
                for (int i = 0; i < this.pronouns.Length; i++)
                {
                    this.pronouns[i].Write(bw);
                }
            }
            bw.Write(this.skintoneRef);
            if (this.version >= 28) bw.Write(this.skintoneShift);
            if (this.version >= 24)
            {
                bw.Write((byte)this.peltLayers.Length);
                for (int i = 0; i < this.peltLayers.Length; i++)
                {
                    this.peltLayers[i].Write(bw);
                }
            }
            bw.Write((byte)this.sculpts.Length);
            for (int i = 0; i < this.sculpts.Length; i++)
            {
                int index = links.Count;
                links.Add(this.sculpts[i]);
                bw.Write((byte)index);
            }
            bw.Write((byte)this.faceModifiers.Length);
            for (int i = 0; i < this.faceModifiers.Length; i++)
            {
                this.faceModifiers[i].Write(bw, ref links);
            }
            bw.Write((byte)this.bodyModifiers.Length);
            for (int i = 0; i < this.bodyModifiers.Length; i++)
            {
                this.bodyModifiers[i].Write(bw, ref links);
            }
            bw.Write(this.voiceActor);
            bw.Write(this.voicePitch);
            bw.Write(this.voiceEffect);
            bw.Write(this.unknown2);
            bw.Write(this.unknown3);

            bw.Write(this.simOutfits.Length);
            for (int i = 0; i < this.simOutfits.Length; i++)
            {
                this.simOutfits[i].Write(bw, ref links);
            }
            bw.Write((byte)this.sculptsGenetic.Length);
            for (int i = 0; i < this.sculptsGenetic.Length; i++)
            {
                int index = links.Count;
                links.Add(this.sculptsGenetic[i]);
                bw.Write((byte)index);
            }
            bw.Write((byte)this.faceModifiersGenetic.Length);
            for (int i = 0; i < this.faceModifiersGenetic.Length; i++)
            {
                this.faceModifiersGenetic[i].Write(bw, ref links);
            }
            bw.Write((byte)this.bodyModifiersGenetic.Length);
            for (int i = 0; i < this.bodyModifiersGenetic.Length; i++)
            {
                this.bodyModifiersGenetic[i].Write(bw, ref links);
            }
            for (int i = 0; i < 4; i++)
            {
                bw.Write(this.genetic_physique[i]);
            }
            bw.Write((byte)this.CASPartsGenetic.Length);
            for (int i = 0; i < this.CASPartsGenetic.Length; i++)
            {
                this.CASPartsGenetic[i].Write(bw, ref links);
            }
            if (this.version >= 32)
            {
                bw.Write((byte)this.GrowthPartsGenetic.Length);
                for (int i = 0; i < this.GrowthPartsGenetic.Length; i++)
                {
                    this.GrowthPartsGenetic[i].Write(bw, ref links);
                }
            }
            bw.Write(this.voiceActorGenetic);
            bw.Write(this.voicePitchGenetic);
            bw.Write(this.flags);
            bw.Write(this.aspirationRef);
            if (this.version >= 32U)
            {
                bw.Write(this.unknown4[0]);
                bw.Write(this.unknown4[1]);
                bw.Write(this.unknown4[2]);
            }
            bw.Write((byte)this.traitRefs.Length);
            for (int i = 0; i < this.traitRefs.Length; i++)
            {
                bw.Write(this.traitRefs[i]);
            }

            long tgiPos = bw.BaseStream.Position;
            uint offset = (uint)(tgiPos - offsetPos - 4);
            bw.BaseStream.Position = offsetPos;
            bw.Write(offset);
            bw.BaseStream.Position = tgiPos;
            bw.Write((byte)links.Count);
            for (int i = 0; i < links.Count; i++)
            {
                links[i].Write(bw, TGI.TGIsequence.IGT);
            }
        }


        public class PeltLayerData
        {
            ulong peltLayerRef;
            uint color;
            public PeltLayerData(ulong peltLayer, uint color)
            {
                this.peltLayerRef = peltLayer;
                this.color = color;
            }
            public PeltLayerData(BinaryReader br)
            {
                this.peltLayerRef = br.ReadUInt64();
                this.color = br.ReadUInt32();
            }
            internal void Write(BinaryWriter bw)
            {
                bw.Write(this.peltLayerRef);
                bw.Write(this.color);
            }
        }

        public class Pronoun
        {
            GrammaticalCase grammaticalCase;
            string pronoun = "";

            public Pronoun()
            {
                this.grammaticalCase = GrammaticalCase.UNKNOWN;
            }
            public Pronoun(GrammaticalCase grammaticalCase, string pronoun)
            {
                this.grammaticalCase = grammaticalCase;
                this.pronoun = pronoun;
            }
            public Pronoun(BinaryReader br)
            {
                uint thecase = br.ReadUInt32();
                this.grammaticalCase = (GrammaticalCase)thecase;
                if (thecase > 0) this.pronoun = br.ReadString();
            }
            internal void Write(BinaryWriter bw)
            {
                uint thecase = (uint)this.grammaticalCase;
                bw.Write(thecase);
                if (thecase > 0) bw.Write(this.pronoun);
            }
        }
        public enum GrammaticalCase
        {
            UNKNOWN = 0,
            SUBJECTIVE = 1,
            OBJECTIVE = 2,
            POSSESSIVE_DEPENDENT = 3,
            POSSESSIVE_INDEPENDENT = 4,
            REFLEXIVE = 5,
        }

        public class SimModifierData
        {
            TGI modifierKey;
            float weight;
            public SimModifierData(TGI modifierKey, float weight)
            {
                this.modifierKey = modifierKey;
                this.weight = weight;
            }
            public SimModifierData(BinaryReader br, TGI[] references)
            {
                this.modifierKey = references[br.ReadByte()];
                this.weight = br.ReadSingle();
            }
            internal void Write(BinaryWriter bw, ref List<TGI> references)
            {
                bool found = false;
                for (int i = 0; i < references.Count; i++)
                {
                    if (this.modifierKey.Equals(references[i]))
                    {
                        bw.Write((byte)i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    references.Add(this.modifierKey);
                    bw.Write((byte)(references.Count - 1));
                }
                bw.Write(this.weight);
            }
        }

        public class OutfitData
        {
            OutfitCategory category;
            uint unknown;
            OutfitDesc[] outfits;

            public OutfitData(OutfitCategory category, uint unknown, OutfitDesc[] outfits)
            {
                this.category = category;
                this.unknown = unknown;
                this.outfits = outfits;
            }
            public OutfitData(BinaryReader br, TGI[] references)
            {
                this.category = (OutfitCategory)br.ReadByte();
                this.unknown = br.ReadUInt32();
                uint num = br.ReadUInt32();
                this.outfits = new OutfitDesc[num];
                for (int i = 0; i < num; i++)
                {
                    this.outfits[i] = new OutfitDesc(br, references);
                }
            }
            internal void Write(BinaryWriter bw, ref List<TGI> references)
            {
                bw.Write((byte)this.category);
                bw.Write(this.unknown);
                bw.Write(this.outfits.Length);
                for (int i = 0; i < this.outfits.Length; i++)
                {
                    this.outfits[i].Write(bw, ref references);
                }
            }

            public class OutfitDesc
            {
                ulong outfitID;
                ulong outfitFlags;
                ulong created;
                bool matchHair;
                PartEntry[] partEntries;

                public OutfitDesc(ulong outfitID, ulong outfitFlags, ulong created, bool matchHair, PartEntry[] parts)
                {
                    this.outfitID = outfitID;
                    this.outfitFlags = outfitFlags;
                    this.created = created;
                    this.matchHair = matchHair;
                    this.partEntries = parts;
                }
                public OutfitDesc(BinaryReader br, TGI[] references)
                {
                    this.outfitID = br.ReadUInt64();
                    this.outfitFlags = br.ReadUInt64();
                    this.created = br.ReadUInt64();
                    this.matchHair = br.ReadBoolean();
                    uint count = br.ReadUInt32();
                    this.partEntries = new PartEntry[count];
                    for (int i = 0; i < count; i++)
                    {
                        this.partEntries[i] = new PartEntry(br, references);
                    }
                }
                internal void Write(BinaryWriter bw, ref List<TGI> references)
                {
                    bw.Write(this.outfitID);
                    bw.Write(this.outfitFlags);
                    bw.Write(this.created);
                    bw.Write(this.matchHair);
                    bw.Write(this.partEntries.Length);
                    for (int i = 0; i < this.partEntries.Length; i++)
                    {
                        this.partEntries[i].Write(bw, ref references);
                    }
                }

                public class PartEntry
                {
                    TGI partKey;
                    BodyType bodyType;
                    ulong colorshift;

                    public PartEntry(TGI partKey, BodyType bodyType, ulong colorShift)
                    {
                        this.partKey = partKey;
                        this.bodyType = bodyType;
                        this.colorshift = colorShift;
                    }
                    public PartEntry(BinaryReader br, TGI[] references)
                    {
                        this.partKey = references[br.ReadByte()];
                        this.bodyType = (BodyType)br.ReadUInt32();
                        this.colorshift = br.ReadUInt64();
                    }
                    internal void Write(BinaryWriter bw, ref List<TGI> references)
                    {
                        bool found = false;
                        for (int i = 0; i < references.Count; i++)
                        {
                            if (this.partKey.Equals(references[i]))
                            {
                                bw.Write((byte)i);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            references.Add(this.partKey);
                            bw.Write((byte)(references.Count - 1));
                        }
                        bw.Write((uint)this.bodyType);
                        bw.Write(this.colorshift);
                    }
                }
            }
        }

        public class PartEntryGenetic
        {
            TGI partKey;
            BodyType bodyType;

            public PartEntryGenetic(TGI partKey, BodyType bodyType)
            {
                this.partKey = partKey;
                this.bodyType = bodyType;
            }
            public PartEntryGenetic(BinaryReader br, TGI[] references)
            {
                this.partKey = references[br.ReadByte()];
                this.bodyType = (BodyType)br.ReadUInt32();
            }
            internal void Write(BinaryWriter bw, ref List<TGI> references)
            {
                bool found = false;
                for (int i = 0; i < references.Count; i++)
                {
                    if (this.partKey.Equals(references[i]))
                    {
                        bw.Write((byte)i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    references.Add(this.partKey);
                    bw.Write((byte)(references.Count - 1));
                }
                bw.Write((uint)this.bodyType);
            }
        }
    }
}
