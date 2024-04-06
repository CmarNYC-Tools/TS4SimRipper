using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ProtoBuf;
using s4pi.Package;
using s4pi.Interfaces;
using s4pi.ImageResource;

namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
        Package[] gamePackages = new Package[0];
        string[] gamePackageNames = new string[0];
        bool[] notBaseGame = new bool[0];
        public Dictionary<TGI, uint> GameTGIs = new Dictionary<TGI, uint>();
        public Package TroubleshootPackageTuning = (Package)Package.NewPackage(0);
        Package TroubleshootPackageBasic = (Package)Package.NewPackage(0);
        Package TroubleshootPackageOutfit = (Package)Package.NewPackage(0);
        public Dictionary<ulong, List<ulong>> GameModifierMorphs = new Dictionary<ulong, List<ulong>>();

        private bool DetectFilePaths()
        {
            try
            {
                if (String.Compare(Properties.Settings.Default.TS4Path, " ") <= 0)
                {
                    string tmp = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Maxis\\The Sims 4", "Install Dir", null);
                    if (tmp != null) Properties.Settings.Default.TS4Path = tmp;
                    //MessageBox.Show(tmp);
                    Properties.Settings.Default.Save();
                }

                if (String.Compare(Properties.Settings.Default.TS4ContentPath, " ") <= 0)
                {
                    string tmp = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\The Sims 4\\content";
                    if (!Directory.Exists(tmp))
                    {
                        string[] tmp2 = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\", "*Sims 4*", SearchOption.AllDirectories);
                        if (tmp2.Length == 1)
                        {
                            // tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            tmp = tmp2[0] + Path.DirectorySeparatorChar + "content" + Path.DirectorySeparatorChar;
                        }
                        else if (tmp2.Length > 1)
                        {
                            if (tmp2[0].Length < tmp2[1].Length)
                                tmp = tmp2[0] + Path.DirectorySeparatorChar + "content" + Path.DirectorySeparatorChar;
                            //tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            else
                                tmp = tmp2[1] + Path.DirectorySeparatorChar + "content" + Path.DirectorySeparatorChar;
                            //tmp = Path.GetDirectoryName(tmp2[1]) + Path.DirectorySeparatorChar;
                        }
                    }
                    if (tmp != null) Properties.Settings.Default.TS4ContentPath = tmp;
                }

                if (String.Compare(Properties.Settings.Default.TS4ModsPath, " ") <= 0)
                {
                    string tmp = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\The Sims 4\\Mods";
                    if (!Directory.Exists(tmp))
                    {
                        string[] tmp2 = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\", "*Sims 4*", SearchOption.AllDirectories);
                        if (tmp2.Length == 1)
                        {
                            // tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            tmp = tmp2[0] + Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar;
                        }
                        else if (tmp2.Length > 1)
                        {
                            if (tmp2[0].Length < tmp2[1].Length)
                                tmp = tmp2[0] + Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar;
                            //tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            else
                                tmp = tmp2[1] + Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar;
                            //tmp = Path.GetDirectoryName(tmp2[1]) + Path.DirectorySeparatorChar;
                        }
                    }
                    if (tmp != null) Properties.Settings.Default.TS4ModsPath = tmp;
                }

                if (String.Compare(Properties.Settings.Default.TS4SavesPath, " ") <= 0)
                {
                    string tmp = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\The Sims 4\\saves";
                    if (!Directory.Exists(tmp))
                    {
                        string[] tmp2 = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Electronic Arts\\", "*Sims 4*", SearchOption.AllDirectories);
                        if (tmp2.Length == 1)
                        {
                            // tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            tmp = tmp2[0] + Path.DirectorySeparatorChar + "saves";
                        }
                        else if (tmp2.Length > 1)
                        {
                            if (tmp2[0].Length < tmp2[1].Length)
                                tmp = tmp2[0] + Path.DirectorySeparatorChar + "saves";
                            //tmp = Path.GetDirectoryName(tmp2[0]) + Path.DirectorySeparatorChar;
                            else
                                tmp = tmp2[1] + Path.DirectorySeparatorChar + "saves";
                            //tmp = Path.GetDirectoryName(tmp2[1]) + Path.DirectorySeparatorChar;
                        }
                    }
                    if (tmp != null) Properties.Settings.Default.TS4SavesPath = tmp; Properties.Settings.Default.Save();
                }

                if (Properties.Settings.Default.TS4Path == null || Properties.Settings.Default.TS4ModsPath == null) return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error detecting paths" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            return (Directory.Exists(Properties.Settings.Default.TS4Path) & Directory.Exists(Properties.Settings.Default.TS4ModsPath));
        }

        private bool SetupGamePacks(bool debug)
        {
            if (debug) MessageBox.Show("Starting setup game packs");

            string TS4FilesPath = Properties.Settings.Default.TS4Path;
            List<Package> gamePacks = new List<Package>();
            List<string> paths = new List<string>();
            List<bool> notBase = new List<bool>();
            try
            {
                List<string> pathsSim = new List<string>(Directory.GetFiles(TS4FilesPath, "Simulation*Build*.package", SearchOption.AllDirectories));
                List<string> pathsClient = new List<string>(Directory.GetFiles(TS4FilesPath, "Client*Build*.package", SearchOption.AllDirectories));
                
                if(Path.GetFileName(TS4FilesPath) == "Contents"){
                    var appPath = Path.GetDirectoryName(Path.GetDirectoryName(TS4FilesPath));
                    var packs = Path.Combine(appPath, "The Sims 4 Packs");
                    if(Directory.Exists(packs)){
                        pathsSim.AddRange(Directory.GetFiles(packs, "Simulation*Build*.package", SearchOption.AllDirectories));
                        pathsClient.AddRange(Directory.GetFiles(packs, "Client*Build*.package", SearchOption.AllDirectories));
                    }

                }
                paths.AddRange(pathsClient);
                paths.AddRange(pathsSim);
                pathsSim.Sort();
                pathsClient.Sort();
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Your game packages path is invalid! Please go to File / Change Settings and correct it or make it blank to reset, then restart." 
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (IOException e)
            {
                MessageBox.Show("Your game packages path is invalid or a network error has occurred! Please go to File / Change Settings and correct it or make it blank to reset, then restart."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Your game packages path is not specified correctly! Please go to File / Change Settings and correct it or make it blank to reset, then restart."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("You do not have the required permissions to access the game packages folder! Please restart with admin privileges."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            if (paths.Count == 0)
            {
                MessageBox.Show("Can't find game packages! Please go to File / Change Settings and correct the game packages path or make it blank to reset, then restart.");
                return false;
            }

            // Predicate<IResourceIndexEntry> testPred = r => r.ResourceType == (uint)ResourceTypes.Rig;
            Predicate<IResourceIndexEntry>[] gamePreds = new Predicate<IResourceIndexEntry>[]
            {
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.Sculpt),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.SimModifier),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.BlendGeometry),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.DeformerMap),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.BoneDelta),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.CASP),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.GEOM),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.RMap),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.RLE2),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.RLES),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.LRLE),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.DDS),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.DDSuncompressed),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.TONE),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.PeltLayer),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.Rig),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.Tuning1),
                new Predicate<IResourceIndexEntry>(r => r.ResourceType == (uint)ResourceTypes.Tuning2)
            };

            uint index = 0;

            List<string> ccPaths = new List<string>();
            string[] localCC = null;
            string err = "";
            try
            {
                localCC = Directory.GetFiles(Properties.Settings.Default.TS4ModsPath,
                    "*.package", SearchOption.AllDirectories);
            }
            catch
            {
                MessageBox.Show("Either the path to your user Sims 4 folder in Documents is incorrect or you have no custom content." +
                    Environment.NewLine + "If the path is incorrect, go to File / Change Settings and correct it or make it blank to reset, then restart.");
            }
            if (localCC != null)
            {
                ccPaths = new List<string>(localCC);
                // ccPaths.Sort((a, b) => b.CompareTo(a));     //descending sort
                ccPaths.Sort();
                for (int i = 0; i < ccPaths.Count; i++)
                {
                    try
                    {
                        Package p = OpenPackage(ccPaths[i], false);
                        if (p == null)
                        {
                            err += ccPaths[i] + " is NULL" + Environment.NewLine;
                            ccPaths[i] = null;
                            continue;
                        }
                        //IResourceIndexEntry testIrie = p.Find(testPred);
                        foreach (Predicate<IResourceIndexEntry> pred in gamePreds)
                        {
                            List<IResourceIndexEntry> tmp = p.FindAll(pred);
                            foreach (IResourceIndexEntry ires in tmp)
                            {
                                TGI tgi;
                                if (ires.ResourceType == (uint)ResourceTypes.CASP)
                                {
                                    tgi = new TGI(ires.ResourceType, 0, ires.Instance);
                                }
                                else
                                {
                                    tgi = new TGI(ires.ResourceType, ires.ResourceGroup, ires.Instance);
                                }
                                if (!GameTGIs.ContainsKey(tgi))
                                {
                                    GameTGIs.Add(tgi, index);
                                }
                            }
                        }
                        //ccPacks.Add(p);
                        //isCC.Add(true);
                        gamePacks.Add(p);
                        notBase.Add(true);
                        index++;
                    }
                    catch (Exception e)
                    {
                        err += ccPaths[i] + " : " + e.Message + Environment.NewLine;
                        ccPaths[i] = null;
                    }
                }
                if (err.Length > 0) MessageBox.Show("Unable to open the following mod packages:" + Environment.NewLine + err);
            }

            if (debug) MessageBox.Show("Set up mods packages");

            err = "";
            List<string> contentPaths = new List<string>();
            string[] content = null;
            try
            {
                content = Directory.GetFiles(Properties.Settings.Default.TS4ContentPath,
                    "*.package", SearchOption.AllDirectories);
            }
            catch
            {
                MessageBox.Show("Either the path to your Sims 4 'content' folder in Documents is incorrect or you have no SDX content." +
                    Environment.NewLine + "If the path is incorrect, go to File / Change Settings and correct it or make it blank to reset, then restart.");
            }

            if (content != null)        //add SDX packages
            {
                contentPaths = new List<string>(content);
                contentPaths.Sort();
                for (int i = 0; i < contentPaths.Count; i++)
                {
                    try
                    {
                        Package p = OpenPackage(contentPaths[i], false);
                        if (p == null)
                        {
                            err += contentPaths[i] + " is NULL" + Environment.NewLine;
                            contentPaths[i] = null;
                            continue;
                        }
                        //IResourceIndexEntry testIrie = p.Find(testPred);
                        foreach (Predicate<IResourceIndexEntry> pred in gamePreds)
                        {
                            List<IResourceIndexEntry> tmp = p.FindAll(pred);
                            foreach (IResourceIndexEntry ires in tmp)
                            {
                                TGI tgi;
                                if (ires.ResourceType == (uint)ResourceTypes.CASP)
                                {
                                    tgi = new TGI(ires.ResourceType, 0, ires.Instance);
                                }
                                else
                                {
                                    tgi = new TGI(ires.ResourceType, ires.ResourceGroup, ires.Instance);
                                }
                                if (!GameTGIs.ContainsKey(tgi))
                                {
                                    GameTGIs.Add(tgi, index);
                                }
                            }
                        }
                        gamePacks.Add(p);
                        notBase.Add(true);
                        index++;
                    }
                    catch (Exception e)
                    {
                        err += contentPaths[i] + " : " + e.Message + Environment.NewLine;
                        contentPaths[i] = null;
                    }
                }
                if (err.Length > 0) MessageBox.Show("Unable to open the following SDX content packages:" + Environment.NewLine + err);
            }

            if (debug) MessageBox.Show("Set up SDX packages");

            err = "";

            try         //add EA packages
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    try
                    {
                        Package p = OpenPackage(paths[i], false);
                        if (p == null)
                        {
                            err += paths[i] + " is NULL" + Environment.NewLine;
                            paths[i] = null;
                            continue;
                        }
                        //IResourceIndexEntry testIrie = p.Find(testPred);
                        foreach (Predicate<IResourceIndexEntry> pred in gamePreds)
                        {
                            List<IResourceIndexEntry> tmp = p.FindAll(pred);
                            foreach (IResourceIndexEntry ires in tmp)
                            {
                                TGI tgi = new TGI(ires.ResourceType, ires.ResourceGroup, ires.Instance);
                                if (!GameTGIs.ContainsKey(tgi))
                                {
                                    GameTGIs.Add(tgi,index);
                                }
                            }
                        }
                        gamePacks.Add(p);
                        notBase.Add(!paths[i].Contains("\\Data\\"));
                        index++;
                    }
                    catch (Exception e)
                    {
                        err += paths[i] + " : " + e.Message + Environment.NewLine;
                        paths[i] = null;
                    }
                }
                if (err.Length > 0) MessageBox.Show("Unable to open the following game packages:" + Environment.NewLine + err);

                if (debug) MessageBox.Show("Set up EA game packages");

                //ccPacks.AddRange(gamePacks);
                //ccPaths.AddRange(paths);
                //isCC.AddRange(notBase);
                //ccPaths.RemoveAll(item => item == null);
                //gamePackages = ccPacks.ToArray();
                //gamePackageNames = ccPaths.ToArray();
                //notBaseGame = isCC.ToArray();

                //gamePacks.AddRange(ccPacks);
                //paths.AddRange(ccPaths);
                //notBase.AddRange(isCC);

                ccPaths.AddRange(contentPaths);
                ccPaths.AddRange(paths);
                ccPaths.RemoveAll(item => item == null);
                gamePackages = gamePacks.ToArray();
                gamePackageNames = ccPaths.ToArray();
                notBaseGame = notBase.ToArray();
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("You do not have the required permissions to open the game and/or mod packages! Please restart with admin privileges."
                    + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }

            return true;
        }

        private Dictionary<ulong, List<ulong>> SetupMorphsToSMOD(ref string errorMsg)
        {
            Dictionary<ulong, List<ulong>> dict = new Dictionary<ulong, List<ulong>>();
            List<KeyValuePair<TGI, uint>> tmp = GameTGIs.Where(x => x.Key.Type == (uint)ResourceTypes.SimModifier).ToList();
            foreach (KeyValuePair<TGI, uint> kv in tmp)
            {
                SMOD smod = FetchGameSMOD(kv.Key, ref errorMsg);
                List<ulong> morphs = new List<ulong>();
                if (smod.BGEOKey != null)
                {
                    foreach (TGI tgi in smod.BGEOKey)
                    {
                        if (tgi.Instance > 0) AddModToDictionary(dict, kv.Key.Instance, tgi.Instance);
                    }
                }
                if (smod.bonePoseKey.Instance > 0) AddModToDictionary(dict, kv.Key.Instance, smod.bonePoseKey.Instance);
                if (smod.deformerMapShapeKey.Instance > 0) AddModToDictionary(dict, kv.Key.Instance, smod.deformerMapShapeKey.Instance); 
                if (smod.deformerMapNormalKey.Instance > 0) AddModToDictionary(dict, kv.Key.Instance, smod.deformerMapNormalKey.Instance);
            }
            return dict;
        }

        private void AddModToDictionary(Dictionary<ulong, List<ulong>> dict, ulong smodInstance, ulong morphInstance)
        {
            if (dict.ContainsKey(morphInstance))
            {
                List<ulong> mods = dict[morphInstance];
                mods.Add(smodInstance);
                dict[morphInstance] = mods;
            }
            else
            {
                dict.Add(morphInstance, new List<ulong>() { smodInstance });
            }
        }

        private void SaveStream(IResourceIndexEntry irie, BinaryReader br, Package pack)
        {
            Stream s = new MemoryStream();
            br.BaseStream.CopyTo(s);
            s.Position = 0;
            pack.AddResource(irie, s, true);
        }
        private void SaveStream(TGI tgi, Stream r, Package pack)
        {
            Stream s = new MemoryStream();
            r.CopyTo(s);
            s.Position = 0;
            TGIBlock tgiblock = new TGIBlock(1, null, tgi.Type, tgi.Group, tgi.Instance);
            pack.AddResource(tgiblock, s, true);
        }
        private void SaveStream(IResourceIndexEntry irie, Stream r, Package pack)
        {
            Stream s = new MemoryStream();
            r.CopyTo(s);
            s.Position = 0;
            pack.AddResource(irie, s, true);
        }

        private Sculpt FetchGameSculpt(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null; 
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            { 
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    {
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                Sculpt sculpt = new Sculpt(br);
                                sculpt.package = gamePackageNames[i];
                                return sculpt;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read Sculpt " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get Sculpt resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find Sculpt " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private SMOD FetchGameSMOD(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                SMOD smod = new SMOD(br);
                                smod.package = gamePackageNames[i];
                                return smod;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read SimModifier " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get SimModifier resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find SimModifier " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private BGEO FetchGameBGEO(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            { 
                                BGEO bgeo = new BGEO(br);
                                bgeo.package = gamePackageNames[i];
                                bgeo.instance = tgi.Instance;
                                return bgeo;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read BGEO " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get BGEO resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find BGEO " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private DMap FetchGameDMap(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                DMap dmap = new DMap(br);
                                dmap.package = gamePackageNames[i];
                                dmap.instance = tgi.Instance;
                                return dmap;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read DMap " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get DMap resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find DeformerMap " + tgi.ToString() + Environment.NewLine;
            return null;
        }

        private BOND FetchGameBOND(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                BOND bond = new BOND(br);
                                bond.package = gamePackageNames[i];
                                bond.instance = tgi.Instance;
                                return bond;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read BOND " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get BOND resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find BoneDelta " + tgi.ToString() + Environment.NewLine;
            return null;
        }

        private CASP FetchGameCASP(TGI tgi, out string packageName, BodyType partType, int outfitNumber, ref string errorMsg, bool saveme)
        {
            if (tgi.Instance == 0ul) { packageName = ""; return null; }
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;

            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i] && saveme) SaveStream(irie, br, TroubleshootPackageOutfit);
                            try
                            {
                                CASP casp = new CASP(br);
                                casp.tgi = new TGI(irie.ResourceType, irie.ResourceGroup, irie.Instance);
                                casp.notBaseGame = notBaseGame[i];
                                casp.package = gamePackageNames[i];
                                packageName = gamePackageNames[i];
                                return casp;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read " + partType.ToString() + " CASP " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                packageName = gamePackageNames[i];
                                if (notBaseGame[i] && !saveme) SaveStream(irie, br, TroubleshootPackageOutfit);
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get CASP resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        packageName = gamePackageNames[i];
                        return null;
                    }
                }

            }
            errorMsg += "Can't find " + partType.ToString() + " CASP " + tgi.ToString() + Environment.NewLine;
            packageName = "";
            return null;
        }
        private GEOM FetchGameGEOM(TGI tgi, string packname, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageOutfit);
                            try
                            {
                                GEOM geom = new GEOM(br);
                                geom.package = gamePackageNames[i];
                                geom.instance = new ulong[] { tgi.Instance };
                                geom.StandardizeFormat();
                                return geom;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read GEOM " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get GEOM resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find GEOM mesh " + tgi.ToString() + ", Linked from package: " + packname + Environment.NewLine;
            return null;
        }
        private RegionMap FetchGameRMap(TGI tgi, string packname, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageOutfit);
                            try
                            {
                                RegionMap rmap = new RegionMap(br);
                                return rmap;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read RMap " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get RMap resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find RegionMap " + tgi.ToString() + ", Linked from package: " + packname + Environment.NewLine;
            return null;
        }
        private RLEResource FetchGameRLE(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, TroubleshootPackageOutfit);
                        try
                        {
                            RLEResource rle = new RLEResource(1, s);
                            return rle;
                        }
                        catch (Exception e)
                        {
                            errorMsg += "Can't read RLE " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get RLE resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find RLE texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private Bitmap FetchGameImageFromRLE(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            return FetchGameImageFromRLE(tgi, false, outfitNumber, ref errorMsg, true);
        }
        private Bitmap FetchGameImageFromRLE(TGI tgi, bool isSpecular, int outfitNumber, ref string errorMsg, bool writeLog)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            Predicate<IResourceIndexEntry> predLrle = r => r.ResourceType == (uint)ResourceTypes.LRLE & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                IResourceIndexEntry irieLrle = p.Find(predLrle);
                if (irie != null || irieLrle != null)
                {
                    Stream s = new MemoryStream();
                    try
                    {
                        if (irie != null)
                        {
                            s = p.GetResource(irie);
                        }
                        if (s.Length < 4)
                        {
                            if (irieLrle != null)
                            {
                                s = p.GetResource(irieLrle);
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get RLE/LRLE resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                    if (notBaseGame[i]) SaveStream(irie, s, outfitNumber >= 0 ? TroubleshootPackageOutfit : TroubleshootPackageBasic);
                    try { 
                        using (RLEResource rle = new RLEResource(1, s))
                        {
                            if (rle != null && rle.AsBytes.Length > 0)
                            {
                                using (DdsFile dds = new DdsFile())
                                {
                                    dds.Load(rle.ToDDS(), false);
                                    Bitmap texture;
                                    if (isSpecular)
                                    {
                                        Size specSize = new Size(currentSize.Width / 2, currentSize.Height / 2);
                                        if (dds.Size == specSize) texture = new Bitmap(dds.Image);
                                        else texture = new Bitmap(dds.Image, specSize);
                                    }
                                    else
                                    {
                                        Bitmap tmp = dds.Image;
                                        if (tmp.Size == currentSize) texture = new Bitmap(tmp);
                                        else texture = new Bitmap(tmp, currentSize);
                                    }
                                    return texture;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (isSpecular && writeLog)
                        {
                            errorMsg += "Can't read RLES image " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                        }
                        else if (writeLog)
                        {
                            errorMsg += "Can't read RLE texture " + tgi.ToString() + Environment.NewLine;
                        }
                    }
                }
            }
            if (isSpecular && writeLog)
            {
                errorMsg += "Can't find RLES texture " + tgi.ToString() + Environment.NewLine;
            }
            else if (writeLog)
            {
                errorMsg += "Can't find RLE texture " + tgi.ToString() + Environment.NewLine;
            }
            return null;
        }
        private LRLE FetchGameLRLE(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageOutfit);
                            try
                            {
                                LRLE lrle = new LRLE(br);
                                return lrle;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read LRLE " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get LRLE resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find LRLE texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private Bitmap FetchGameImageFromLRLE(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            return FetchGameImageFromLRLE(tgi, outfitNumber, ref errorMsg, true);
        }
        private Bitmap FetchGameImageFromLRLE(TGI tgi, int outfitNumber, ref string errorMsg, bool writeLog)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            // for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, outfitNumber >= 0 ? TroubleshootPackageOutfit : TroubleshootPackageBasic);
                        try
                        {
                            BinaryReader br = new BinaryReader(s);
                            LRLE lrle = new LRLE(br);
                            Bitmap tmp = lrle.image;
                            if (tmp.Size == currentSize) return new Bitmap(tmp);
                            else return new Bitmap(tmp, currentSize);
                        }
                        catch
                        {
                            try
                            {
                                using (RLEResource rle = new RLEResource(1, s))
                                {
                                    if (rle != null && rle.AsBytes.Length > 0)
                                    {
                                        using (DdsFile dds = new DdsFile())
                                        {
                                            dds.Load(rle.ToDDS(), false);
                                            Bitmap texture;
                                            Bitmap tmp = dds.Image;
                                            if (tmp.Size == currentSize) texture = new Bitmap(tmp);
                                            else texture = new Bitmap(tmp, currentSize);
                                            return texture;
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                if (writeLog) errorMsg += "Can't read LRLE image " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get LRLE resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            //Predicate<IResourceIndexEntry> pred2 = r => r.ResourceType == (uint)ResourceTypes.RLE2 & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            //{
            //    Package p = gamePackages[i];
            //    IResourceIndexEntry irie = p.Find(pred2);
            //    if (irie != null)
            //    {
            //        if (isCCPackage[i] && !currentCCPackages.Any(x => x.package == p)) { currentCCPackages.Add(new CCPackage(p, Path.GetFileName(gamePackageNames[i]), outfitNumber)); }
            //        Stream s = p.GetResource(irie);
            //        try
            //        {
            //            using (RLEResource rle = new RLEResource(1, s))
            //            {
            //                if (rle != null && rle.AsBytes.Length > 0)
            //                {
            //                    using (DdsFile dds = new DdsFile())
            //                    {
            //                        dds.Load(rle.ToDDS(), false);
            //                        Bitmap texture;
            //                        Bitmap tmp = dds.Image;
            //                        if (tmp.Size == currentSize) texture = new Bitmap(tmp);
            //                        else texture = new Bitmap(tmp, currentSize);
            //                        return texture;
            //                    }
            //                }
            //            }
            //        }
            //        catch
            //        {
            //            errorMsg += "Can't read RLE image " + tgi.ToString() + ", Package: " + gamePackageNames[i] + Environment.NewLine;
            //        }
            //    }
            //}
            if (writeLog) errorMsg += "Can't find LRLE texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }

        private Bitmap FetchGameTexture(ulong instance, int outfitNumber, ref string errorMsg, bool includeDDS) //Looks for LRLE, RLE2, and optionally DDS
        {
            return FetchGameTexture(new TGI((uint)ResourceTypes.LRLE, 0, instance), outfitNumber, ref errorMsg, includeDDS);
        }
        private Bitmap FetchGameTexture(TGI tgi, int outfitNumber, ref string errorMsg, bool includeDDS) //Looks for LRLE, RLE2, and optionally DDS
        {
            TGI lrleTGI = new TGI((uint)ResourceTypes.LRLE, tgi.Group, tgi.Instance);
            TGI rle2TGI = new TGI((uint)ResourceTypes.RLE2, tgi.Group, tgi.Instance);
            TGI ddsTGI = new TGI((uint)ResourceTypes.DDSuncompressed, tgi.Group, tgi.Instance);
            Bitmap image = FetchGameImageFromLRLE(lrleTGI, outfitNumber, ref errorMsg, false);
            if (image != null) return image;
            image = FetchGameImageFromRLE(rle2TGI, false, outfitNumber, ref errorMsg, false);
            if (image != null) return image;
            if (includeDDS) image = FetchGameImageFromDDS(ddsTGI, outfitNumber, ref errorMsg, false);
            if (image == null) errorMsg += "Can't find or read texture, instance: 0x" + tgi.Instance.ToString("X16") + Environment.NewLine;
            return image;
        }

        private DSTResource FetchGameDST(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            // for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, TroubleshootPackageOutfit);
                        try
                        {
                            DSTResource dst = new DSTResource(1, s);
                            return dst;
                        }
                        catch (Exception e)
                        {
                            errorMsg += "Can't read DST " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                            return null;
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get DST resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find DST texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private Bitmap FetchGameImageFromDST(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, TroubleshootPackageOutfit);
                        try
                        {
                            using (DSTResource dst = new DSTResource(1, s))
                            {
                                if (dst != null)
                                {
                                    using (DdsFile dds = new DdsFile())
                                    {
                                        dds.Load(dst.ToDDS(), false);
                                        return new Bitmap(dds.Image);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            errorMsg += "Can't read DST image " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                            return null;
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get DST resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find DST texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }

        private DdsFile FetchGameDDS(TGI tgi, int outfitNumber, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, TroubleshootPackageOutfit);
                        try
                        {
                            DdsFile dds = new DdsFile();
                            dds.Load(s, false);
                            return dds;
                        }
                        catch (Exception e)
                        {
                            errorMsg += "Can't read DDS " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                            return null;
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get DDS resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find DDS texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private Bitmap FetchGameImageFromDDS(TGI tgi, int outfitNumber, ref string errorMsg, bool writeLog)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        Stream s = p.GetResource(irie);
                        if (notBaseGame[i]) SaveStream(irie, s, outfitNumber >= 0 ? TroubleshootPackageOutfit : TroubleshootPackageBasic);
                        using (DdsFile dds = new DdsFile())
                        {
                            try
                            {
                                dds.Load(s, false);
                                Bitmap texture;
                                if (dds.Size == currentSize) texture = new Bitmap(dds.Image);
                                else texture = new Bitmap(dds.Image, currentSize);
                                return texture;
                            }
                            catch (Exception e)
                            {
                                if (writeLog)
                                    errorMsg += "Can't read DDS image " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get DDS resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            if (writeLog) errorMsg += "Can't find DDS texture " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private TONE FetchGameTONE(TGI tgi, out string packageName, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) { packageName = ""; return null; }
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                TONE tone = new TONE(br);
                                packageName = gamePackageNames[i];
                                return tone;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read TONE " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                packageName = gamePackageNames[i];
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get TONE resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        packageName = gamePackageNames[i];
                        return null;
                    }
                }
            }
            errorMsg += "Can't find skin TONE " + tgi.ToString() + Environment.NewLine;
            packageName = "";
            return null;
        }
        private PeltLayer FetchGamePeltLayer(TGI tgi, ref string errorMsg)
        {
            if (tgi.Instance == 0ul) return null;
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                PeltLayer pelt = new PeltLayer(br);
                                return pelt;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read Pelt " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get Pelt resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        return null;
                    }
                }
            }
            errorMsg += "Can't find cat/dog/horse Pelt Layer " + tgi.ToString() + Environment.NewLine;
            return null;
        }
        private RIG FetchGameRig(TGI tgi, ref string errorMsg, out string rigPackage)
        {
            if (tgi.Instance == 0ul) { rigPackage = ""; return null; }
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.Instance == tgi.Instance;
            //for (int i = 0; i < gamePackages.Length; i++)
            uint i;
            if (GameTGIs.TryGetValue(tgi, out i))
            {
                Package p = gamePackages[i];
                IResourceIndexEntry irie = p.Find(pred);
                if (irie != null)
                {
                    try
                    { 
                        using (BinaryReader br = new BinaryReader(p.GetResource(irie)))
                        {
                            if (notBaseGame[i]) SaveStream(irie, br, TroubleshootPackageBasic);
                            try
                            {
                                RIG rig = new RIG(br);
                                rigPackage = gamePackageNames[i];
                                return rig;
                            }
                            catch (Exception e)
                            {
                                errorMsg += "Can't read Rig " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e.Message + Environment.NewLine;
                                rigPackage = gamePackageNames[i];
                                return null;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        errorMsg += "Can't get RIG resource " + tgi.ToString() + ", Package: " + gamePackageNames[i] + " : " + e1.Message + Environment.NewLine;
                        rigPackage = gamePackageNames[i];
                        return null;
                    }
                }
            }
            errorMsg += "Can't find Rig " + tgi.ToString() + Environment.NewLine;
            rigPackage = "";
            return null;
        }
    }
}
