using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using ProtoBuf;
using s4pi.Package;
using s4pi.Interfaces;
using s4pi.ImageResource;
using TS4SaveGame = EA.Sims4.Persistence;
namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
        string version = "TS4 SimRipper Classic v" + typeof(Form1).Assembly.GetName().Version.ToString();
        ulong[] frameIDMtF4male = new ulong[] { 0x27FE2BD7D11FDE65UL, 0x7A9D44AB67D00802UL };
        ulong[] frameIDMtF4female = new ulong[] { 0xA1A3F64ED26BCED8UL, 0x8ABEBBC4544AAE5BUL };
        ulong[] frameIDFtM = new ulong[] { 0x73290F92433C9DCCUL, 0xBD2A4BDE5C973977UL };
        Size humanTextureSize = new Size(1024, 2048);
        Size petTextureSize = new Size(2048, 1024);
        Size humanTextureSizeHQ = new Size(2048, 4096);
        Size petTextureSizeHQ = new Size(4096, 2048);
        Size currentSize = new Size();
        CASModifierTuning CASTuning;
        static string SaveFilter = "Game save files (*.save, *.ver*)|*.save;*.ver*|All files (*.*)|*.*";
        static string GEOMfilter = "GEOM mesh files (*.simgeom, *.geom)|*.simgeom;*.geom|All files (*.*)|*.*";
        static string OBJfilter = "OBJ files (*.obj)|*.obj|All files (*.*)|*.*";
        static string MS3Dfilter = "Milkshape MS3D files (*.ms3d)|*.ms3d|All files (*.*)|*.*";
        static string DAEfilter = "Collada .dae files (*.dae)|*.dae|All files (*.*)|*.*";
        static string Imagefilter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
        static string SIMOfilter = "Sim Info/Outfit files (*.simo)|*.simo|All files (*.*)|*.*";
        string[] physiqueNamesHuman;
        string[] physiqueNamesAnimal;
        Package currentSaveGame;
        string currentSaveName;
        //TS4SaveGame.SimData[] simsArray;
        SimListing[] simsArray;
        TS4SaveGame.SimData currentSim;
        TONE currentTONE;
        float currentSkinShift;
        int currentSkinSet;
        Bitmap currentTanLines;
        float[] currentPhysique;
        List<TS4SaveGame.PeltLayerData> currentPelt;
        ulong currentPaintedCoatInstance;

        MorphMap[] frameModifier = new MorphMap[2];
        BOND frameModifierFlat;
        MorphMap[] frameBoobs;
        bool isPregnant;
        float pregnancyProgress;
        MorphMap[] pregnantModifier;

        bool canHaveBreasts;

        Dictionary<ulong, string> worldNames;

        string startupErrors = "";
        string simDesc = "";
        string errorList = "";
        FileStream log;

        public Form1()
        {
            InitializeComponent();
            this.menuStrip1.Font = new Font(new FontFamily("Microsoft Sans Serif"), 8f);
            bool debug = false;
            StartMessage starter = new StartMessage();
            starter.Show();
            this.Text = version;
            physiqueNamesHuman = new string[] { "Body_Heavy", "Body_Fit", "Body_Lean", "Body_Bony", "Body_Pregnant", "Hips_Wide", "Hips_Narrow", "Waist_Wide", "Waist_Narrow" };
            physiqueNamesAnimal = new string[] { "headModifier_Body_Fat", "headModifier_Body_Fit", "headModifier_Body_Skinny", "headModifier_Body_Bony",
                "headModifier_Body_Pregnant", "headModifier_Hips_Wide", "headModifier_Hips_Narrow", "headModifier_Waist_Wide", "headModifier_Waist_Narrow" };

            while (!DetectFilePaths())
            {
                DialogResult res = MessageBox.Show("Can't find game and/or user files. Do you want to set them manually?", "Files not found", MessageBoxButtons.RetryCancel);
                if (res == DialogResult.Cancel) break;
                Form f = new PathsPrompt(Properties.Settings.Default.TS4Path, Properties.Settings.Default.TS4ContentPath, Properties.Settings.Default.TS4ModsPath, Properties.Settings.Default.TS4SavesPath);
                f.ShowDialog();
            }

            if (debug) MessageBox.Show("Detected file paths");

            if (SetupGamePacks(debug))
            {
                try
                {
                    startupErrors = "";
                    ulong flatID = FNVhash.FNV64("yfheadChest_Small");
                    BOND flatten = FetchGameBOND(new TGI((uint)ResourceTypes.BoneDelta, 0, flatID), ref startupErrors);
                    if (flatten != null)
                    {
                        flatten.weight = .8f;
                        frameModifierFlat = flatten;
                    }
                    //ulong shapeID = FNVhash.FNV64("ymBody_Female_Shape");
                    //DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref startupErrors);
                    //if (shape == null)
                    DMap shape = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.ymBody_Female_Shape)));
                    //ulong normalID = FNVhash.FNV64("ymBody_Female_Normals");
                    //DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref startupErrors);
                    //if (normals == null)
                    DMap normals = new DMap(new BinaryReader(new MemoryStream(Properties.Resources.ymBody_Female_Normals)));
                    if (shape != null && normals != null) frameBoobs = new MorphMap[] { shape.ToMorphMap(), normals.ToMorphMap() };
                    GameModifierMorphs = SetupMorphsToSMOD(ref startupErrors);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error setting up frame modifiers" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                }
            }

            if (debug) MessageBox.Show("Read frame morphs");

            try
            {
                CASTuning = new CASModifierTuning(gamePackages, gamePackageNames, notBaseGame, this);
            }
            catch (Exception e)
            {
                MessageBox.Show("CASModifierTuning error: " + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
            }

            if (debug) MessageBox.Show("Read CAS Tuning");

            SimFilter_checkedListBox.ItemCheck -= SimFilter_checkedListBox_ItemCheck;
            for (int i = 0; i < SimFilter_checkedListBox.Items.Count; i++)
            {
                SimFilter_checkedListBox.SetItemChecked(i, true);
            }
            SimFilter_checkedListBox.ItemCheck += SimFilter_checkedListBox_ItemCheck;
            SortBy_comboBox.SelectedIndexChanged -= SortBy_comboBox_SelectedIndexChanged;
            SortBy_comboBox.SelectedIndex = 0;
            SortBy_comboBox.SelectedIndexChanged += SortBy_comboBox_SelectedIndexChanged;

            BoneSize_numericUpDown.Value = Properties.Settings.Default.BlenderBoneLength;
            CleanDAE_checkBox.Checked = Properties.Settings.Default.BlenderClean;
            SeparateMeshes_comboBox.SelectedIndex = Properties.Settings.Default.SeparateMeshesIndex;
            HQSize_radioButton.Checked = Properties.Settings.Default.HQTextures;
            LinkTexture_checkBox.Checked = Properties.Settings.Default.LinkTextures;
            OverlaySort_comboBox.SelectedIndexChanged -= OverlaySort_comboBox_SelectedIndexChanged;
            OverlaySort_comboBox.SelectedIndex = Properties.Settings.Default.OverlaySortOrder;
            OverlaySort_comboBox.SelectedIndexChanged += OverlaySort_comboBox_SelectedIndexChanged;
            if (Properties.Settings.Default.SkinBlendIndex == 0) SkinBlend1_radioButton.Checked = true;
            else if (Properties.Settings.Default.SkinBlendIndex == 1) Skinblend37_radioButton.Checked = true;
            else if (Properties.Settings.Default.SkinBlendIndex == 2) SkinBlend2_radioButton.Checked = true;
            else if (Properties.Settings.Default.SkinBlendIndex == 3) SkinBlend3_radioButton.Checked = true;
            NormalConvert_checkBox.Checked = Properties.Settings.Default.ConvertBump;

            if (debug) MessageBox.Show("Set up screens");
            starter.Close();
        }

        public class SimListing
        {
            public TS4SaveGame.SimData sim;
            public bool isMannequin;
            public SimListing(TS4SaveGame.SimData sim)
            {
                this.sim = sim;
                this.isMannequin = false;
            }
            public SimListing(TS4SaveGame.MannequinSimData man)
            {
                this.sim = new TS4SaveGame.SimData();
                sim.gender = man.gender;
                sim.age = man.age;
                sim.extended_species = man.extended_species;
                sim.physique = man.physique;
                sim.facial_attr = man.facial_attributes;
                sim.skin_tone = man.skin_tone;
                sim.outfits = man.outfits;
                sim.current_outfit_index = man.current_outfit_index;
                sim.world_id = man.world_id;
                sim.skin_tone_val_shift = man.skin_tone_val_shift;
                sim.first_name = ((AgeGender)man.age != AgeGender.YoungAdult ? ((AgeGender)man.age).ToString() : "YA") + ((AgeGender)man.gender).ToString();
                sim.last_name = "Mannequin" + man.mannequin_id.ToString("X16");
                this.isMannequin = true;
            }
            public string sortNameLast
            {
                get { return sim.last_name + sim.first_name; }
            }
            public string sortNameFirst
            {
                get { return sim.first_name + sim.last_name; }
            }
            public string sortHousehold
            {
                get { return sim.household_name + sim.last_name + sim.first_name; }
            }
            public override string ToString()
            {
                return sim.first_name + " " + sim.last_name + " (" + sim.household_name + ")";
            }
        }

        private void SaveGameFile_button_Click(object sender, EventArgs e)
        {
            string savesFolder = null;
            if (Properties.Settings.Default.TS4SavesPath != null && Properties.Settings.Default.TS4SavesPath.Length > 0)
            {
                savesFolder = Properties.Settings.Default.TS4SavesPath;
                savesFolder = savesFolder.Replace(@"\\", @"\");
            }
            SaveGameFile.Text = GetFilename("Select game save file", SaveFilter, savesFolder);
            if (SaveGameFile.Text.Length == 0) return;
            Package p = (Package)Package.OpenPackage(0, SaveGameFile.Text, false);
            Predicate<IResourceIndexEntry> idel = r => r.ResourceType == 0x0000000D;
            IResourceIndexEntry iries = p.Find(idel);
            Stream s = p.GetResource(iries);
            TS4SaveGame.SaveGameData save = Serializer.Deserialize<TS4SaveGame.SaveGameData>(s);
            GameName.Text = save.save_slot.slot_name;
            worldNames = new Dictionary<ulong, string>();
            if (save.zones != null)
            {
                foreach (TS4SaveGame.ZoneData zone in save.zones)
                {
                    foreach (TS4SaveGame.NeighborhoodData neighborhood in save.neighborhoods)
                    {
                        if (neighborhood.neighborhood_id == zone.neighborhood_id)
                        {
                            if (!worldNames.ContainsKey(zone.zone_id)) worldNames.Add(zone.zone_id, neighborhood.name);
                        }
                    }
                }
            }
            currentSaveGame = p;
            currentSaveName = GameName.Text;
            List<SimListing> tmp = new List<SimListing>();
            if (save.sims != null)
            {
                foreach (TS4SaveGame.SimData sim in save.sims)
                {
                    tmp.Add(new SimListing(sim));
                }
            }
            if (save.mannequins != null)
            {
                foreach (TS4SaveGame.MannequinSimData man in save.mannequins)
                {
                    tmp.Add(new SimListing(man));
                }
            }
            simsArray = tmp.ToArray();
            ListSims();
        }

        public void ListSims()
        {
            if (simsArray == null || simsArray.Length == 0)
            {
                MessageBox.Show("Saved game does not contain any readable sims!" + Environment.NewLine + "It may be an unrecognized version.");
                return;
            }
            List<SimListing> simsList = new List<SimListing>();
            for (int i = 0; i < simsArray.Length; i++)
            {
                if (((simsArray[i].sim.extended_species <= 1 && SimFilter_checkedListBox.GetItemChecked(0) ||
                     (simsArray[i].sim.extended_species > 1 && SimFilter_checkedListBox.GetItemChecked(1)) ||
                      simsArray[i].isMannequin && SimFilter_checkedListBox.GetItemChecked(2))) &
                    ((simsArray[i].sim.gender == (uint)AgeGender.Male && SimFilter_checkedListBox.GetItemChecked(3)) ||
                     (simsArray[i].sim.gender == (uint)AgeGender.Female && SimFilter_checkedListBox.GetItemChecked(4))) &
                    ((simsArray[i].sim.age == (uint)AgeGender.Infant && SimFilter_checkedListBox.GetItemChecked(5)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.Toddler && SimFilter_checkedListBox.GetItemChecked(6)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.Child && SimFilter_checkedListBox.GetItemChecked(7)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.Teen && SimFilter_checkedListBox.GetItemChecked(8)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.YoungAdult && SimFilter_checkedListBox.GetItemChecked(9)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.Adult && SimFilter_checkedListBox.GetItemChecked(10)) ||
                     (simsArray[i].sim.age == (uint)AgeGender.Elder && SimFilter_checkedListBox.GetItemChecked(11))))
                    simsList.Add(simsArray[i]);
            }
            if (SortBy_comboBox.SelectedIndex == 1)
            {
                simsList.Sort((x, y) => x.sortNameFirst.CompareTo(y.sortNameFirst));
            }
            else if (SortBy_comboBox.SelectedIndex == 2)
            {
                simsList.Sort((x, y) => x.sortHousehold.CompareTo(y.sortHousehold));
            }
            else
            {
                simsList.Sort((x, y) => x.sortNameLast.CompareTo(y.sortNameLast));
            }
            sims_listBox.Items.Clear();
            for (int i = 0; i < simsList.Count; i++)
            {
                sims_listBox.Items.Add(simsList[i]);
            }
        }


        internal string GetFilename(string title, string filter, string defaultFolder)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = filter;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Title = title;
            if (defaultFolder != null && defaultFolder.Length > 0 && Directory.Exists(defaultFolder)) openFileDialog1.InitialDirectory = defaultFolder;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal class OutfitInfo
        {
            internal CASP[] casps;
            internal ulong[] colorShifts;
            internal uint[] layerIds;
            internal string[] packages;
            internal OutfitInfo(CASP[] casps, ulong[] colorShifts, string[] packageNames, uint[] layerIds)
            {
                this.casps = casps;
                this.colorShifts = colorShifts;
                this.packages = packageNames;
                this.layerIds = layerIds;
            }
        }

        private void sims_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TS4SaveGame.SimData sim = ((SimListing)sims_listBox.SelectedItem).sim;
            currentSim = sim;
            if (sim == null)
            {
                MessageBox.Show("Selected sim is null!");
                return;
            }
            currentName = sim.first_name + " " + sim.last_name;
            currentSpecies = (Species)sim.extended_species;
            if (sim.extended_species == 0) currentSpecies = Species.Human;
            currentAge = (AgeGender)sim.age;
            currentGender = (AgeGender)sim.gender;
            if (HQSize_radioButton.Checked) currentSize = currentSpecies == Species.Human ? humanTextureSizeHQ : petTextureSizeHQ;
            else currentSize = currentSpecies == Species.Human ? humanTextureSize : petTextureSize;
            currentPaintedCoatInstance = sim.custom_texture;
            errorList = "";

            bool frameFeminine = false, frameMasculine = false;
            canHaveBreasts = true;
            isPregnant = false;
            if (sim.attributes != null && sim.attributes.trait_tracker != null && sim.attributes.trait_tracker.trait_ids != null)
                foreach (ulong t in sim.attributes.trait_tracker.trait_ids)
                {
                    TS4Traits trait = (TS4Traits)t;
                    if (trait == TS4Traits.trait_GenderOptions_Frame_Feminine) frameFeminine = true;
                    if (trait == TS4Traits.trait_GenderOptions_Frame_Masculine) frameMasculine = true;
                    if (trait == TS4Traits.trait_isPregnant || trait == TS4Traits.trait_isPregnant_Alien_Abduction) isPregnant = true;
                    if (trait == TS4Traits.trait_Breasts_ForceOff) canHaveBreasts = false;
                }
            if (currentOccult == SimOccult.Werewolf) canHaveBreasts = false;

            currentFrame = currentGender;
            if (currentSpecies == Species.Human && currentAge > AgeGender.Child && currentAge != AgeGender.Infant)
            {
                if (currentGender == AgeGender.Male && frameFeminine)
                {
                    currentFrame = AgeGender.Female;
                }
                else if (currentGender == AgeGender.Female && frameMasculine)
                {
                    currentFrame = AgeGender.Male;
                }
            }

            currentOccult = SimOccult.Human;
            Occults_comboBox.SelectedIndexChanged -= Occults_comboBox_SelectedIndexChanged;
            Occults_comboBox.Items.Clear();
            Occults_comboBox.Refresh();
            if (sim.attributes != null && sim.attributes.occult_tracker != null && sim.attributes.occult_tracker.occult_sim_infos != null)
            {
                Occults_comboBox.Enabled = true;
                for (int i = 0; i < sim.attributes.occult_tracker.occult_sim_infos.Count; i++)
                {
                    TS4SaveGame.PersistableOccultTracker.OccultSimData occult = sim.attributes.occult_tracker.occult_sim_infos[i];
                    SimOccult occultType = (SimOccult)occult.occult_type;
                    Occults_comboBox.Items.Add(sim.attributes.occult_tracker.occult_types == 16 && occultType == SimOccult.Human ? SimOccult.Spellcaster : occultType);
                    if (sim.attributes.occult_tracker.current_occult_types == occult.occult_type)
                    {
                        Occults_comboBox.SelectedIndex = i;
                        currentOccult = occultType;
                    }
                }
            }
            else
            {
                Occults_comboBox.Items.Clear();
                Occults_comboBox.Enabled = false;
            }
            Occults_comboBox.SelectedIndexChanged += Occults_comboBox_SelectedIndexChanged;

            if (currentAge > AgeGender.Child && currentAge != AgeGender.Infant)
            {
                Pregnancy_trackBar.Enabled = true;
                pregnancyProgress = sim.pregnancy_progress;
                Pregnancy_trackBar.Scroll -= Pregnancy_trackBar_Scroll;
                Pregnancy_trackBar.Value = (int)(sim.pregnancy_progress * 10f);
                Pregnancy_trackBar.Scroll += Pregnancy_trackBar_Scroll;

                if (currentSpecies == Species.Human)
                {
                    string dmapName = GetPhysiquePrefix(currentSpecies, currentAge, AgeGender.Female) + "Belly_Big";
                    ulong shapeID = FNVhash.FNV64(dmapName + "_Shape");
                    DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                    ulong normalID = FNVhash.FNV64(dmapName + "_Normals");
                    DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);

                    pregnantModifier = new MorphMap[] { shape != null ? shape.ToMorphMap() : null, normals != null ? normals.ToMorphMap() : null };
                }
                else if (currentSpecies == Species.Horse)
                {
                    string dmapName = GetPhysiquePrefix(currentSpecies, currentAge, AgeGender.Female) + "Body_Pregnant";
                    ulong shapeID = FNVhash.FNV64(dmapName + "_Shape");
                    DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                    ulong normalID = FNVhash.FNV64(dmapName + "_Normals");
                    DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);

                    pregnantModifier = new MorphMap[] { shape != null ? shape.ToMorphMap() : null, normals != null ? normals.ToMorphMap() : null };
                }
                else
                {
                    string dmapName = GetPhysiquePrefix(currentSpecies, currentAge, AgeGender.Female) + "Body_Fat";
                    ulong shapeID = FNVhash.FNV64(dmapName + "_Shape");
                    DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                    ulong normalID = FNVhash.FNV64(dmapName + "_Normals");
                    DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);

                    pregnantModifier = new MorphMap[] { shape != null ? shape.ToMorphMap() : null, normals != null ? normals.ToMorphMap() : null };
                }
            }
            else
            {
                pregnancyProgress = 0f;
                Pregnancy_trackBar.Value = 0;
                Pregnancy_trackBar.Enabled = false;
            }

            DisplaySim(sim, currentOccult);
        }

        //private class CCPackage
        //{
        //    internal Package package;
        //    internal string packageName;
        //    internal int outfit;-*

        //    internal CCPackage(Package pack, string packName, int outfitNumber)
        //    {
        //        package = pack;
        //        packageName = packName;
        //        outfit = outfitNumber;
        //    }
        //}

        private static void LogMe(FileStream log, string text)
        {
            if (log != null && log.CanWrite)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text + Environment.NewLine);
                log.Write(bytes, 0, bytes.Length);
                log.Flush();
            }
        }

        private void DisplaySim(TS4SaveGame.SimData sim, SimOccult occultState)
        {
            string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logPath = Path.Combine(executingPath, "SimBuilderLog.txt");
            if (log != null && log.CanWrite) log.Close();
            try
            {
                log = File.Open(logPath, FileMode.Create, FileAccess.Write);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to create log file:" + Environment.NewLine + e.Message);
            }
            LogMe(log, "SimRipper V" + version + " log " + DateTime.Now.ToString());
            LogMe(log, sim.first_name + " " + sim.last_name + " Outfit: " + sim.current_outfit_index.ToString() + " Occult: " + occultState.ToString());
            TroubleshootPackageBasic = (Package)Package.NewPackage(0);
            Working_label.Visible = true;
            Working_label.Refresh();
            string info = "";
            //  Package testpack = (Package)Package.NewPackage(1);

            ulong[] sculpts = new ulong[0];
            List<TS4SaveGame.BlobSimFacialCustomizationData.Modifier> faceModifiers = new List<TS4SaveGame.BlobSimFacialCustomizationData.Modifier>();
            List<EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier> bodyModifiers = new List<TS4SaveGame.BlobSimFacialCustomizationData.Modifier>();
            string[] physique = null;
            List<TS4SaveGame.OutfitData> outfits = new List<TS4SaveGame.OutfitData>();
            ulong skintone = 0;
            float skincolorShift = 0;

            LogMe(log, "DisplaySim start");

            currentOccult = occultState == SimOccult.Spellcaster ? SimOccult.Human : occultState;

            AgeGender adjustedAge = currentAge >= AgeGender.Teen && currentAge <= AgeGender.Elder ? AgeGender.Adult : currentAge;
            ulong rigInstance = 0;
            string rigPackage = "";
            baseRig = GetTS4Rig(currentSpecies, adjustedAge, currentOccult, out rigInstance, out rigPackage);
            LogMe(log, "Got Sim Rig: " + rigInstance.ToString("X16") + " Package: " + rigPackage);
            currentRig = new RIG(baseRig);

            if (currentOccult != SimOccult.Human && sim.attributes != null && sim.attributes.occult_tracker != null && sim.attributes.occult_tracker.occult_sim_infos != null)
            {
                for (int i = 0; i < sim.attributes.occult_tracker.occult_sim_infos.Count; i++)
                {
                    TS4SaveGame.PersistableOccultTracker.OccultSimData occultSim = sim.attributes.occult_tracker.occult_sim_infos[i];
                    SimOccult occultType = (SimOccult)occultSim.occult_type;
                    if (occultType == currentOccult)
                    {
                        Stream s2 = new MemoryStream(occultSim.facial_attributes);
                        TS4SaveGame.BlobSimFacialCustomizationData morphs = Serializer.Deserialize<TS4SaveGame.BlobSimFacialCustomizationData>(s2);
                        sculpts = morphs.sculpts != null ? morphs.sculpts : new ulong[0];
                        faceModifiers = morphs.face_modifiers != null ? morphs.face_modifiers : new List<EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier>();
                        bodyModifiers = morphs.body_modifiers != null ? morphs.body_modifiers : new List<EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier>();
                        physique = occultSim.physique.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        outfits = occultSim.outfits.outfits;
                        skintone = occultSim.skin_tone;
                        //skincolorShift = occultSim.GetType().GetProperty("skin_tone_val_shift") != null ? occultSim.skin_tone_val_shift : 0;
                        skincolorShift = occultSim.skin_tone_val_shift;
                    }
                }
            }
            else
            {
                Stream s2 = new MemoryStream(sim.facial_attr);
                TS4SaveGame.BlobSimFacialCustomizationData morphs = Serializer.Deserialize<TS4SaveGame.BlobSimFacialCustomizationData>(s2);
                sculpts = morphs.sculpts != null ? morphs.sculpts : new ulong[0];
                faceModifiers = morphs.face_modifiers != null ? morphs.face_modifiers : new List<EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier>();
                bodyModifiers = morphs.body_modifiers != null ? morphs.body_modifiers : new List<EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier>();
                physique = sim.physique.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                outfits = sim.outfits.outfits;
                skintone = sim.skin_tone;
                //skincolorShift = sim.GetType().GetProperty("skin_tone_val_shift") != null ? sim.skin_tone_val_shift : 0;
                skincolorShift = sim.skin_tone_val_shift;
            }

            LogMe(log, "Loaded physiques and skintones");
            //  if (debug) errorList += "DisplaySim loaded occult lists" + Environment.NewLine;


            List<BGEO> bgeoList = new List<BGEO>();
            List<MorphMap> shapeList = new List<MorphMap>();
            List<MorphMap> normalList = new List<MorphMap>();
            List<BOND> bondList = new List<BOND>();

            currentOutfits = new List<OutfitInfo>();
            Outfits_comboBox.Items.Clear();
            List<TGI> caspsNotFound = new List<TGI>();
            OutfitCategory? currentCategory = null;
            int categoryCount = 1;
            int categoryIndex = 0;
            for (int o = 0; o < outfits.Count; o++)
            {
                LogMe(log, "Loading outfit " + o.ToString() + " from savegame");
                if (outfits[o].parts.ids == null) continue;
                TS4SaveGame.OutfitData outfit = outfits[o];
                var parts = outfit.parts;
                List<CASP> casps = new List<CASP>();
                List<string> packNames = new List<string>();
                List<ulong> colorShifts = new List<ulong>();
                var layerIds = new List<uint>();
                bool gotShifts = outfit.part_shifts != null && outfit.part_shifts.color_shift.Length == parts.ids.Length;
                bool gotLayers = outfit.layer_ids != null && outfit.layer_ids.layer_id.Length == parts.ids.Length;
                for (int i = 0; i < outfit.parts.ids.Length; i++)
                {
                    ulong id = outfit.parts.ids[i];
                    string packname = "";
                    TGI caspTGI = new TGI((uint)ResourceTypes.CASP, 0, id);
                    if (caspsNotFound.Contains(caspTGI)) continue;
                    CASP casp = FetchGameCASP(caspTGI, out packname, (BodyType)outfit.body_types_list.body_types[i], o, ref errorList, false);
                    if (casp == null) { caspsNotFound.Add(caspTGI); continue; }
                    casps.Add(casp);
                    packNames.Add(packname);
                    colorShifts.Add(gotShifts ? outfit.part_shifts.color_shift[i] : 0x4000000000000000UL);
                    layerIds.Add(gotLayers ? outfit.layer_ids.layer_id[i]: 0);
                }
                currentOutfits.Add(new OutfitInfo(casps.ToArray(), colorShifts.ToArray(), packNames.ToArray(), layerIds.ToArray()));
                OutfitCategory category = (OutfitCategory)outfit.category;
                if
                    (category == currentCategory) categoryCount++;
                else
                {
                    categoryCount = 1;
                    currentCategory = category;
                    if (category == (OutfitCategory)sim.current_outfit_type) categoryIndex = o;
                }
                Outfits_comboBox.Items.Add(category.ToString() + (categoryCount > 1 ? categoryCount.ToString() : ""));
            }

            Outfits_comboBox.SelectedIndexChanged -= Outfits_comboBox_SelectedIndexChanged;
            outfitIndex = categoryIndex + (int)sim.current_outfit_index;
            if (outfitIndex > Outfits_comboBox.Items.Count - 1) outfitIndex = 0;
            Outfits_comboBox.SelectedIndex = outfitIndex;
            Outfits_comboBox.SelectedIndexChanged += Outfits_comboBox_SelectedIndexChanged;

            //  if (debug) errorList += "DisplaySim loaded outfits" + Environment.NewLine;

            if (currentSpecies == Species.Human && currentAge == AgeGender.Elder)
            {
                LogMe(log, "Loading elder morph");
                string prefix = "e" + currentFrame.ToString().Substring(0, 1).ToLower();
                ulong shapeID = FNVhash.FNV64(prefix + "Body_Average_Shape");
                DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                ulong normalID = FNVhash.FNV64(prefix + "Body_Average_Normals");
                DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                if (shape != null && normals != null)
                {
                    shapeList.Add(shape.ToMorphMap());
                    normalList.Add(normals.ToMorphMap());
                }
            }

            //  if (debug) errorList += "DisplaySim loaded elder morph" + Environment.NewLine;

            if (currentSpecies == Species.Human && currentOccult == SimOccult.Werewolf)
            {
                LogMe(log, "Loading werewolf morph");
                //DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, werewolfMorph[0]), ref errorList);
                //DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, werewolfMorph[1]), ref errorList);
                string prefix = "y" + currentFrame.ToString().Substring(0, 1).ToLower();
                ulong shapeID = FNVhash.FNV64(prefix + "Body_Werewolf_Shape");
                DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                ulong normalID = FNVhash.FNV64(prefix + "Body_Werewolf_Normals");
                DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                if (shape != null && normals != null)
                {
                    shapeList.Add(shape.ToMorphMap());
                    normalList.Add(normals.ToMorphMap());
                }
                //  if (debug) errorList += "DisplaySim loaded werewolf morph" + Environment.NewLine;
            }

            float[] physiqueWeights = new float[physique.Length];
            if (currentSpecies == Species.Human)
            {
                LogMe(log, "Loading physique morphs");
                for (int i = 0; i < physique.Length; i++)
                {
                    if (float.TryParse(physique[i], NumberStyles.Float, CultureInfo.InvariantCulture, out physiqueWeights[i]))
                    {
                        if (physiqueWeights[i] > 0.0)
                        {
                            string dmapName = GetPhysiquePrefix(currentSpecies, currentAge, currentFrame) + physiqueNamesHuman[i];
                            ulong shapeID = FNVhash.FNV64(dmapName + "_Shape");
                            DMap shape = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, shapeID), ref errorList);
                            if (shape == null) continue;
                            shape.weight = physiqueWeights[i];

                            ulong normalID = FNVhash.FNV64(dmapName + "_Normals");
                            DMap normals = FetchGameDMap(new TGI((uint)ResourceTypes.DeformerMap, 0, normalID), ref errorList);
                            if (normals == null) continue;
                            normals.weight = physiqueWeights[i];

                            shapeList.Add(shape.ToMorphMap());
                            normalList.Add(normals.ToMorphMap());
                        }
                    }
                }
            }
            else
            {
                LogMe(log, "Loading physique morphs");
                for (int i = 0; i < physique.Length; i++)
                {
                    float weight = 0;
                    if (float.TryParse(physique[i], NumberStyles.Float, CultureInfo.InvariantCulture, out weight))
                    {
                        if (weight > 0.0)
                        {
                            string smodName = GetPhysiquePrefix(currentSpecies, currentAge, currentGender) + physiqueNamesAnimal[i];
                            ulong smodID = FNVhash.FNV64(smodName);
                            SMOD smod = FetchGameSMOD(new TGI((uint)ResourceTypes.SimModifier, 0, smodID), ref errorList);
                            if (smod == null) continue;
                            if (smod.BGEOKey != null && smod.BGEOKey.Length > 0 && smod.BGEOKey[0].Instance > 0)
                            {
                                BGEO bgeo = FetchGameBGEO(smod.BGEOKey[0], ref errorList);
                                if (bgeo != null)
                                {
                                    bgeo.weight = weight;
                                    bgeoList.Add(bgeo);
                                }
                            }
                            if (smod.deformerMapShapeKey.Instance > 0)
                            {
                                DMap shape = FetchGameDMap(smod.deformerMapShapeKey, ref errorList);
                                if (shape != null)
                                {
                                    shape.weight = weight;
                                    shapeList.Add(shape.ToMorphMap());
                                }
                            }
                            if (smod.deformerMapNormalKey.Instance > 0)
                            {
                                DMap normal = FetchGameDMap(smod.deformerMapNormalKey, ref errorList);
                                if (normal != null)
                                {
                                    normal.weight = weight;
                                    normalList.Add(normal.ToMorphMap());
                                }
                            }
                            if (smod.bonePoseKey.Instance > 0)
                            {
                                BOND bond = FetchGameBOND(smod.bonePoseKey, ref errorList);
                                if (bond != null)
                                {
                                    bond.weight = weight;
                                    bondList.Add(bond);
                                }
                            }
                        }
                    }
                }
            }

            //  if (debug) errorList += "DisplaySim loaded physique weights" + Environment.NewLine;

            Bitmap sculptOverlay = new Bitmap(currentSize.Width, currentSize.Height);
            string morphInfo = "";
            List<SculptOrder> sculptTextures = new List<SculptOrder>();
            //int count = 1;
            //List<Dictionary<ulong, float>> sculptWeights;   //sculpt id, weight
            //List<string> dampenModifiers = CASTuning.SculptDampening(currentSpecies, occultState, currentAge, currentGender, out sculptWeights); //SMODs to dampen

            Sculpt[] simSculpts = new Sculpt[Enum.GetValues(typeof(SimRegion)).Cast<int>().Max()];
            // bool[] sculptAreas = new bool[Enum.GetValues(typeof(SimRegion)).Cast<int>().Max()];
            foreach (ulong id in sculpts)
            {
                TGI tgi = new TGI((uint)ResourceTypes.Sculpt, 0, id);
                Sculpt sculpt = FetchGameSculpt(tgi, ref errorList);
                if (sculpt == null) continue;
                if (!(sculpt.publicKey[0].Instance == id)) sculpt.publicKey[0].Instance = id;
                simSculpts[(uint)sculpt.region] = sculpt;
            }

            foreach (Sculpt sculpt in simSculpts)
            {
                // TGI tgi = new TGI((uint)ResourceTypes.Sculpt, 0, id);
                // Sculpt sculpt = FetchGameSculpt(tgi, ref errorList);
                if (sculpt == null) continue;
                // if (sculptAreas[(uint)sculpt.region]) continue;
                // sculptAreas[(uint)sculpt.region] = true;
                TGI tgi = sculpt.publicKey[0];
                LogMe(log, "Loading sculpt: 0x" + tgi.Instance.ToString("X16") + " Package: " + sculpt.package);
                morphInfo += "Sculpt: " + tgi.ToString() + " (" + sculpt.region.ToString() + ")" + Environment.NewLine;
                if (sculpt.BGEOKey != null && sculpt.BGEOKey.Length > 0 && sculpt.BGEOKey[0].Instance > 0)
                {
                    BGEO bgeo = FetchGameBGEO(sculpt.BGEOKey[0], ref errorList);
                    if (bgeo != null) bgeoList.Add(bgeo);
                }
                if (sculpt.dmapShapeRef.Instance > 0)
                {
                    DMap shape = FetchGameDMap(sculpt.dmapShapeRef, ref errorList);
                    if (shape != null)
                    {
                        //for (int i = 0; i < dampenModifiers.Count; i++)
                        //{
                        //    if (FNVhash.FNV64(dampenModifiers[i] + "_Shape") == sculpt.dmapShapeRef.Instance)
                        //    {
                        //        float dampen;
                        //        if (sculptWeights[i].TryGetValue(id, out dampen))
                        //        {
                        //            shape.weight = dampen;
                        //        }
                        //    }
                        //}  
                        shape.region = sculpt.region;
                        shapeList.Add(shape.ToMorphMap());
                    }
                }
                if (sculpt.dmapNormalRef.Instance > 0)
                {
                    DMap normal = FetchGameDMap(sculpt.dmapNormalRef, ref errorList);
                    if (normal != null)
                    {
                        //int ind = dampenModifiers.IndexOf(sculpt.dmapNormalRef.Instance);
                        //if (ind >= 0)
                        //{
                        //    float dampen = 1;
                        //    if (sculptWeights[ind].TryGetValue(id, out dampen))
                        //    {
                        //        normal.weight = dampen;
                        //    }
                        //}
                        normal.region = sculpt.region;
                        normalList.Add(normal.ToMorphMap());
                    }
                }
                if (sculpt.boneDeltaRef != null)
                {
                    if (sculpt.boneDeltaRef.Instance > 0)
                    {
                        BOND bond = FetchGameBOND(sculpt.boneDeltaRef, ref errorList);
                        if (bond != null)
                        {
                            bondList.Add(bond);
                        }
                    }
                }
                else
                {
                    LogMe(log, $"{sim.first_name} {sim.last_name} using old sculpt {sculpt.publicKey[0]} version {sculpt.Version:X8}. Skipping boneDeltaRef");
                }
                if (sculpt.textureRef != null)
                {
                    if (sculpt.textureRef.Instance > 0u)
                    {
                        Bitmap texture = FetchGameTexture(sculpt.textureRef, -1, ref errorList, false);
                        //texture.Save("F:\\Sims4Workspace\\scultTexture" + count.ToString() + ".png");
                        //count++;
                        if (texture != null)
                        {
                            int order = 0;
                            switch (sculpt.region)
                            {
                                case SimRegion.CHEEKS: order = 1; break;
                                case SimRegion.FOREHEAD: order = 2; break;
                                case SimRegion.JAW: order = 3; break;
                                case SimRegion.CHIN: order = 4; break;
                                case SimRegion.MOUTH: order = 5; break;
                                case SimRegion.NOSE: order = 6; break;
                                case SimRegion.EYES: order = 7; break;
                                default: order = 8; break;
                            }
                            sculptTextures.Add(new SculptOrder(texture, order));
                        }
                    }
                }
                else
                {
                    LogMe(log, $"{sim.first_name} {sim.last_name} using old sculpt {sculpt.publicKey[0]} version {sculpt.Version:X8}. Skipping textureRef");
                }
            }
            sculptTextures.Sort((a, b) => a.order.CompareTo(b.order));
            using (Graphics g = Graphics.FromImage(sculptOverlay))
            {
                for (int i = 0; i < sculptTextures.Count; i++)
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(sculptTextures[i].texture, new Rectangle(0, 0, sculptOverlay.Width, sculptOverlay.Height));
                }
            }
            currentSculptOverlay = sculptOverlay;

            //  if (debug) errorList += "DisplaySim loaded sculpts" + Environment.NewLine;

            Dictionary<ulong, string> modifierNames = CASTuning.CASModifierNames(currentSpecies, occultState, currentAge, currentGender);
            Dictionary<ulong, float> modifierScaling = CASTuning.CASModifierScales(currentSpecies, occultState, currentAge, currentGender);

            foreach (EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier m in faceModifiers)
            {
                TGI tgi = new TGI((uint)ResourceTypes.SimModifier, 0, m.key);
                SMOD smod = FetchGameSMOD(tgi, ref errorList);
                if (smod == null) continue;
                LogMe(log, "Loading SMOD: 0x" + m.key.ToString("X16") + " Package: " + smod.package);

                string modName = smod.region.ToString();
                if (modifierNames != null)
                {
                    string tmp = "";
                    if (modifierNames.TryGetValue(m.key, out tmp)) modName = tmp;
                }
                float modAdjust = 1f;
                if (modifierScaling != null)
                {
                    float tmp;
                    if (modifierScaling.TryGetValue(m.key, out tmp)) modAdjust = tmp;
                }
                float modDampen = 1f;
                float modOffset = 0f;
                morphInfo += "Sim Modifier: " + tgi.ToString() + " (" + modName + "), Weight: " + m.amount.ToString() + ", Scaling: " + modAdjust.ToString() + ", Offset: " + modOffset.ToString() + Environment.NewLine;

                if (smod.BGEOKey != null && smod.BGEOKey.Length > 0 && smod.BGEOKey[0].Instance > 0)
                {
                    BGEO bgeo = FetchGameBGEO(smod.BGEOKey[0], ref errorList);
                    if (bgeo != null)
                    {
                        bgeo.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        bgeoList.Add(bgeo);
                    }
                }
                if (smod.deformerMapShapeKey.Instance > 0)
                {
                    DMap shape = FetchGameDMap(smod.deformerMapShapeKey, ref errorList);
                    if (shape != null)
                    {
                        shape.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        shape.region = smod.region;
                        shapeList.Add(shape.ToMorphMap());
                    }
                }
                if (smod.deformerMapNormalKey.Instance > 0)
                {
                    DMap normal = FetchGameDMap(smod.deformerMapNormalKey, ref errorList);
                    if (normal != null)
                    {
                        normal.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        normal.region = smod.region;
                        normalList.Add(normal.ToMorphMap());
                    }
                }
                if (smod.bonePoseKey.Instance > 0)
                {
                    BOND bond = FetchGameBOND(smod.bonePoseKey, ref errorList);
                    if (bond != null)
                    {
                        bond.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        bondList.Add(bond);
                    }
                }
            }

            //  if (debug) errorList += "DisplaySim loaded face modifiers" + Environment.NewLine;

            foreach (EA.Sims4.Persistence.BlobSimFacialCustomizationData.Modifier m in bodyModifiers)
            {
                TGI tgi = new TGI((uint)ResourceTypes.SimModifier, 0, m.key);
                SMOD smod = FetchGameSMOD(tgi, ref errorList);
                if (smod == null) continue;
                LogMe(log, "Loading SMOD: 0x" + m.key.ToString("X16") + " Package: " + smod.package);

                string modName = smod.region.ToString();
                if (modifierNames != null)
                {
                    string tmp = "";
                    if (modifierNames.TryGetValue(m.key, out tmp)) modName = tmp;
                }
                float modAdjust = 1f;
                if (modifierScaling != null)
                {
                    float tmp;
                    if (modifierScaling.TryGetValue(m.key, out tmp)) modAdjust = tmp;
                }
                float modDampen = 1f;
                float modOffset = 0f;
                morphInfo += "Sim Modifier: " + tgi.ToString() + " (" + modName + "), Weight: " + m.amount.ToString() + ", Scaling: " + modAdjust.ToString() +
                    ", Offset: " + modOffset.ToString() + ", Dampen: " + modDampen.ToString() + Environment.NewLine;
                //  TestPackageBuilder(smod, new TGI((uint)ResourceTypes.SimModifier, 0, m.key), testpack);

                if (smod.BGEOKey != null && smod.BGEOKey.Length > 0 && smod.BGEOKey[0].Instance > 0)
                {
                    BGEO bgeo = FetchGameBGEO(smod.BGEOKey[0], ref errorList);
                    if (bgeo != null)
                    {
                        bgeo.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        bgeoList.Add(bgeo);
                    }
                }
                if (smod.deformerMapShapeKey.Instance > 0)
                {
                    DMap shape = FetchGameDMap(smod.deformerMapShapeKey, ref errorList);
                    if (shape != null)
                    {
                        shape.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        shape.region = smod.region;
                        shapeList.Add(shape.ToMorphMap());
                    }
                }
                if (smod.deformerMapNormalKey.Instance > 0)
                {
                    DMap normal = FetchGameDMap(smod.deformerMapNormalKey, ref errorList);
                    if (normal != null)
                    {
                        normal.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        normal.region = smod.region;
                        normalList.Add(normal.ToMorphMap());
                    }
                }
                if (smod.bonePoseKey.Instance > 0)
                {
                    BOND bond = FetchGameBOND(smod.bonePoseKey, ref errorList);
                    if (bond != null)
                    {
                        bond.weight = (m.amount + modOffset) * modAdjust * modDampen;
                        bondList.Add(bond);
                    }
                }
            }

            //  if (debug) errorList += "DisplaySim loaded body modifiers" + Environment.NewLine;

            morphPreview1.Stop_Mesh();
            morphBGEO = bgeoList;
            morphBOND = bondList;
            morphShape = shapeList;
            morphNormals = normalList;

            string tonePackage = "";
            if (currentSpecies == Species.Human && currentOccult != SimOccult.Werewolf)
            {
                SkinState_comboBox.SelectedIndexChanged -= SkinState_comboBox_SelectedIndexChanged;
                currentTONE = FetchGameTONE(new TGI((uint)ResourceTypes.TONE, 0, skintone), out tonePackage, ref errorList);
                LogMe(log, "Applying TONE: 0x" + skintone.ToString("X16") + " Package: " + tonePackage);
                currentSkinShift = skincolorShift;
                if (currentTONE != null && sim.attributes != null && sim.attributes.suntan_tracker != null)
                {
                    currentSkinSet = (int)sim.attributes.suntan_tracker.tan_level;
                    currentTanLines = null;
                    if (currentTONE.SkinSets.Length > 1 && currentTONE.SkinSets[1].textureInstance > 0)
                    {
                        SkinState_comboBox.Enabled = true;
                        SkinState_comboBox.SelectedIndex = currentSkinSet;
                        if (sim.attributes.suntan_tracker.outfit_part_data_list != null && sim.attributes.suntan_tracker.outfit_part_data_list.Count > 0)
                        {
                            foreach (TS4SaveGame.PartData part in sim.attributes.suntan_tracker.outfit_part_data_list)
                            {
                                string dummy = "";
                                TGI tgi = new TGI((uint)ResourceTypes.CASP, 0, part.id);
                                CASP casp = FetchGameCASP(tgi, out dummy, (BodyType)part.body_type, -1, ref errorList, true);
                                LogMe(log, "Applying suntan CASP: 0x" + part.id.ToString("X16") + " Package: " + dummy);
                                Bitmap tmp = FetchGameImageFromRLE(casp.LinkList[casp.TextureIndex], -1, ref errorList);
                                if (tmp == null) continue;
                                if (currentTanLines == null)
                                {
                                    currentTanLines = new Bitmap(tmp, 512, 1024);
                                }
                                else
                                {
                                    using (Graphics g = Graphics.FromImage(currentTanLines))
                                    {
                                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.DrawImage(tmp, new Rectangle(0, 0, 512, 1024));
                                    }
                                }
                            }
                            DdsFile ddsAlpha = new DdsFile();
                            ddsAlpha.CreateImage(currentTanLines, false);
                            currentTanLines = ddsAlpha.GetGreyscaleFromAlpha();
                            ddsAlpha.Dispose();
                            ColorMatrix invertMatrix = new ColorMatrix(
                               new float[][]
                               {
                              new float[] {-1, 0, 0, 0, 0},
                              new float[] {0, -1, 0, 0, 0},
                              new float[] {0, 0, -1, 0, 0},
                              new float[] {0, 0, 0, .9f, 0},
                              new float[] {1, 1, 1, 0, 1}
                               });
                            using (Graphics gr = Graphics.FromImage(currentTanLines))
                            {
                                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                ImageAttributes tanAttributes = new ImageAttributes();
                                tanAttributes.SetColorMatrix(invertMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gr.DrawImage(currentTanLines, new Rectangle(0, 0, currentTanLines.Width, currentTanLines.Height), 0, 0, currentTanLines.Width, currentTanLines.Height, GraphicsUnit.Pixel, tanAttributes);
                            }
                        }
                    }
                    else
                    {
                        SkinState_comboBox.Enabled = false;
                        SkinState_comboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    SkinState_comboBox.Enabled = false;
                    SkinState_comboBox.SelectedIndex = 0;
                }
                SkinState_comboBox.SelectedIndexChanged += SkinState_comboBox_SelectedIndexChanged;
            }
            else
            {
                currentPelt = sim.pelt_layers.layers;
                SkinState_comboBox.Enabled = false;
            }

            //   if (debug) errorList += "DisplaySim loaded skin/pelt" + Environment.NewLine;

            currentPhysique = physiqueWeights;

            string worldName = "";
            info = "Name: " + currentName + Environment.NewLine + "Household: " + sim.household_name + ", " +
                "World: " + (worldNames.TryGetValue(sim.zone_id, out worldName) ? worldName : "None") +
                Environment.NewLine + "Occult: " + currentOccult.ToString() + Environment.NewLine + "Age: " + ((AgeGender)sim.age).ToString() + Environment.NewLine +
                "Gender: " + ((AgeGender)sim.gender).ToString() + " / Frame: " + currentFrame.ToString() + Environment.NewLine +
                "Pregnant: " + isPregnant.ToString() + Environment.NewLine + "Rig: " + rigInstance.ToString("X16") +
                " (" + (rigPackage.Length > 0 ? Path.GetFileName(rigPackage) : "Not Found") + ")" + Environment.NewLine +
                "Skintone: " + sim.skin_tone.ToString("X16") +
                ", Overlay " + (currentTONE != null ? "Hue: " + currentTONE.Hue.ToString() + " Saturation: " + currentTONE.Saturation.ToString() : "-") +
                ", Shift: " + skincolorShift.ToString() + " (" + (tonePackage.Length > 0 ? Path.GetFileName(tonePackage) : "Not Found") + ")" + Environment.NewLine;
            for (int i = 0; i < 5; i++)
            {
                if (currentSpecies == Species.Human)
                    info += physiqueNamesHuman[i] + ": " + physique[i] + Environment.NewLine;
                else
                    info += physiqueNamesAnimal[i] + ": " + physique[i] + Environment.NewLine;
            }
            simDesc = info + Environment.NewLine + morphInfo + Environment.NewLine;

            //  if (debug) errorList += "DisplaySim loaded info listing" + Environment.NewLine;

            GetCurrentModel();
            Working_label.Visible = false;
            //// info += "Traits: ";
            //// foreach (ulong trait in sim.attributes.trait_tracker.trait_ids) { info += ((TS4Traits)trait).ToString() + Environment.NewLine; };
            // SimInfo.Text = info;

            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.AddExtension = true;
            //saveFileDialog1.CheckPathExists = true;
            //saveFileDialog1.DefaultExt = "package";
            //saveFileDialog1.OverwritePrompt = true;
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK) testpack.SaveAs(saveFileDialog1.FileName);

            morphPreview1.Stop_Mesh();
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, true, SeparateMeshes_comboBox.SelectedIndex == 2);
        }

        private class SculptOrder
        {
            internal Bitmap texture;
            internal int order;
            internal SculptOrder(Bitmap texture, int order)
            {
                this.texture = texture;
                this.order = order;
            }
        }

        private void TestPackageBuilder(object resource, TGI tgi, Package testpack)
        {
            Stream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            if (resource is Sculpt)
            {
                Sculpt sculpt = resource as Sculpt;
                sculpt.Write(bw);
            }
            else if (resource is SMOD)
            {
                SMOD smod = resource as SMOD;
                smod.Write(bw);
            }
            else if (resource is BGEO)
            {
                BGEO bgeo = resource as BGEO;
                bgeo.Write(bw);
            }
            else if (resource is DMap)
            {
                DMap dmap = resource as DMap;
                dmap.Write(bw);
            }
            else if (resource is BOND)
            {
                BOND bond = resource as BOND;
                bond.Write(bw);
            }
            testpack.AddResource(new TGIBlock(1, null, tgi.Type, tgi.Group, tgi.Instance), s, true);
        }

        internal Package OpenPackage(string packagePath, bool readwrite)
        {
            try
            {
                Package package = (Package)Package.OpenPackage(0, packagePath, readwrite);
                return package;
            }
            catch
            {
                MessageBox.Show("Unable to read valid package data from " + packagePath);
                return null;
            }
        }

        internal static string WriteGEOM(string title, GEOM geom, string defaultFilename)
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = GEOMfilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "simgeom";
            saveFileDialog1.OverwritePrompt = true;
            if (defaultFilename != null && String.Compare(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryWriter bw = new BinaryWriter(myStream);
                            geom.WriteFile(bw);
                        }
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file " + saveFileDialog1.FileName + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                    myStream.Close();
                }
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal static string WriteMS3D(string title, MS3D ms3d, string defaultFilename)
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = MS3Dfilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            if (defaultFilename != null && String.Compare(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            saveFileDialog1.DefaultExt = "ms3d";
            saveFileDialog1.OverwritePrompt = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryWriter bw = new BinaryWriter(myStream);
                            ms3d.Write(bw);
                        }
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file " + saveFileDialog1.FileName + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                    myStream.Close();
                }
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal static string WriteOBJFile(string title, OBJ myOBJ, string defaultFilename)
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = OBJfilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "obj";
            saveFileDialog1.OverwritePrompt = true;
            if (defaultFilename != null && String.Compare(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamWriter sw = new StreamWriter(myStream);
                            myOBJ.Write(sw);
                        }
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file " + saveFileDialog1.FileName + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                    myStream.Close();
                }
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal string WriteDAEFile(string title, DAE dae, bool flipYZ, string path, string basename)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = DAEfilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "dae";
            saveFileDialog1.OverwritePrompt = true;
            string defaultFilename = path + "\\" + basename;
            if (defaultFilename != null && String.CompareOrdinal(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                float boneDivider = ((10f - (float)BoneSize_numericUpDown.Value) / 4f) * 100f;
                dae.Write(saveFileDialog1.FileName, flipYZ, boneDivider, LinkTexture_checkBox.Checked, SeparateMeshes_comboBox.SelectedIndex == 2);
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal static string WriteSIMO(string title, SimInfo simo, string defaultFilename)
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = SIMOfilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            if (defaultFilename != null && String.Compare(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            saveFileDialog1.DefaultExt = "simo";
            saveFileDialog1.OverwritePrompt = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryWriter bw = new BinaryWriter(myStream);
                            simo.Write(bw);
                        }
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file " + saveFileDialog1.FileName + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                    myStream.Close();
                }
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal string WriteImage(string title, Image image, string defaultFilename)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = Imagefilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.OverwritePrompt = true;
            if (defaultFilename != null && String.CompareOrdinal(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image.Save(saveFileDialog1.FileName);
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal void SaveImagePng(Image image, string path)
        {
            Stream s = new MemoryStream();
            image.Save(s, ImageFormat.Png);
            using (var fileStream = File.Create(path))
            {
                s.Position = 0;
                s.CopyTo(fileStream);
            }
        }

        public RIG GetTS4Rig(Species species, AgeGender age, SimOccult occult, out ulong rigInstance, out string rigPackage)
        {
            TGI rigTGI;
            if (occult == SimOccult.Werewolf)
            {
                rigTGI = new TGI((uint)ResourceTypes.Rig, 0u, 0x60FAA42F9B0B4E39);
            }
            else
            {
                String rigName = GetRigPrefix(species, age, AgeGender.Unisex) + "Rig";
                rigTGI = new TGI((uint)ResourceTypes.Rig, 0u, FNVhash.FNV64(rigName));
            }
            rigInstance = rigTGI.Instance;
            RIG rig = FetchGameRig(rigTGI, ref errorList, out rigPackage);
            if (rig == null)
            {

                BinaryReader br = null;
                string path = "";
                if (species == Species.Human)
                {
                    if (age >= AgeGender.Teen)
                    {
                        path = Application.StartupPath + "\\S4_Adult_RIG.grannyrig";
                    }
                    else
                        path = Application.StartupPath + "\\S4_" + age.ToString() + "_RIG.grannyrig";
                }
                else
                {
                    path = Application.StartupPath + "\\S4_" + Enum.GetName(typeof(AgeGender), age) + Enum.GetName(typeof(Species), species) + "_RIG.grannyrig";
                }
                if ((br = new BinaryReader(File.OpenRead(path))) != null)
                {
                    using (br)
                    {
                        rig = new RIG(br);
                    }
                    br.Dispose();
                }
                else
                {
                    MessageBox.Show("Can't open " + age.ToString() + species.ToString() + "RIG file!");
                    return null;
                }
            }
            return rig;
        }

        public static double[] ArrayToDouble(float[] array)
        {
            double[] result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(version + Environment.NewLine + "by cmar" + Environment.NewLine + "Available free from ModTheSims.info");
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new PathsPrompt(Properties.Settings.Default.TS4Path, Properties.Settings.Default.TS4ContentPath, Properties.Settings.Default.TS4ModsPath, Properties.Settings.Default.TS4SavesPath);
            f.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveOBJ_button_Click(object sender, EventArgs e)
        {
            SaveModelMorph(MeshFormat.OBJ);
        }

        private void SaveDAE_button_Click(object sender, EventArgs e)
        {
            SaveModelMorph(MeshFormat.DAE);
        }

        private void SaveMS3D_button_Click(object sender, EventArgs e)
        {
            SaveModelMorph(MeshFormat.MS3D);
        }

        private void SaveGEOM_button_Click(object sender, EventArgs e)
        {
            SaveModelMorph(MeshFormat.GEOM);
        }

        private void SaveSIMO_button_Click(object sender, EventArgs e)
        {
            SimInfo simo = new SimInfo(currentSim, GameTGIs);
            WriteSIMO("Save Sim Info/Outfit resource to file", simo, null);
        }

        private void SimFilter_checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SimFilter_checkedListBox.ItemCheck -= SimFilter_checkedListBox_ItemCheck;
            SimFilter_checkedListBox.SetItemCheckState(e.Index, e.NewValue);
            SimFilter_checkedListBox.ItemCheck += SimFilter_checkedListBox_ItemCheck;

            ListSims();
        }

        private void SortBy_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSims();
        }

        private void SimInfo_Click(object sender, EventArgs e)
        {
            ListingForm fulllist = new ListingForm((string)SimInfo_button.Tag, "Sim Information");
            fulllist.Show();
        }

        private void SimError_Click(object sender, EventArgs e)
        {
            ListingForm fulllist = new ListingForm(startupErrors + Environment.NewLine + errorList, "Error List");
            fulllist.Show();
        }

        private void Occults_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TS4SaveGame.SimData sim = ((SimListing)sims_listBox.SelectedItem).sim;
            currentSim = sim;
            DisplaySim(sim, (SimOccult)Occults_comboBox.SelectedItem);
        }

        private void Outfits_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            outfitIndex = Outfits_comboBox.SelectedIndex;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel();
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void Pregnancy_trackBar_Scroll(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            pregnancyProgress = Pregnancy_trackBar.Value / 10f;
            if (pregnantModifier[0] != null) pregnantModifier[0].weight = pregnancyProgress;
            if (pregnantModifier[1] != null) pregnantModifier[1].weight = pregnancyProgress;
            for (int i = 0; i < BaseModel.Length; i++)
            {
                CurrentModel[i] = LoadDMapMorph(log, BaseModel[i], pregnantModifier[0], pregnantModifier[1]);
            }
            morphPreview1.Stop_Mesh();
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, true, SeparateMeshes_comboBox.SelectedIndex == 2);
        }

        private void SkinState_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            currentSkinSet = SkinState_comboBox.SelectedIndex;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(true);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void TanLines_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(true);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void SkinBlend_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(true);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void SkinOverlay_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(true);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void OverlaySort_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(false);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // MessageBox.Show("Startup Errors");
            if (startupErrors != null && startupErrors.Length > 0) MessageBox.Show(startupErrors);
        }

        private void HQSize_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BaseModel == null) return;
            Working_label.Visible = true;
            Working_label.Refresh();
            morphPreview1.Stop_Mesh();
            GetCurrentModel(false);
            morphPreview1.Start_Mesh(CurrentModel, GlassModel, currentTexture, currentClothingSpecular,
                currentGlassTexture, currentGlassSpecular, false, SeparateMeshes_comboBox.SelectedIndex == 2);
            Working_label.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.BlenderBoneLength = BoneSize_numericUpDown.Value;
            Properties.Settings.Default.BlenderClean = CleanDAE_checkBox.Checked;
            Properties.Settings.Default.SeparateMeshesIndex = SeparateMeshes_comboBox.SelectedIndex;
            Properties.Settings.Default.HQTextures = HQSize_radioButton.Checked;
            Properties.Settings.Default.LinkTextures = LinkTexture_checkBox.Checked;
            Properties.Settings.Default.OverlaySortOrder = OverlaySort_comboBox.SelectedIndex;
            if (SkinBlend1_radioButton.Checked == true) Properties.Settings.Default.SkinBlendIndex = 0;
            else if (Skinblend37_radioButton.Checked == true) Properties.Settings.Default.SkinBlendIndex = 1;
            else if (SkinBlend2_radioButton.Checked == true) Properties.Settings.Default.SkinBlendIndex = 2;
            else if (SkinBlend3_radioButton.Checked == true) Properties.Settings.Default.SkinBlendIndex = 3;
            Properties.Settings.Default.ConvertBump = NormalConvert_checkBox.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
