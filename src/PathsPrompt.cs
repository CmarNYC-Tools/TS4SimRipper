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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ookii.Dialogs.Wpf;

namespace TS4SimRipper
{
    public partial class PathsPrompt : Form
    {
        public PathsPrompt()
        {
            InitializeComponent();
        }

        public PathsPrompt(string path, string contentpath, string userpath, string savespath)
        {
            InitializeComponent();
            TS4PathString.Text = path;
            TS4ContentString.Text = contentpath;
            TS4UserPathString.Text = userpath;
            TS4SavesPathString.Text = savespath;
        }

        private void Folder_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog findFolder = new VistaFolderBrowserDialog();
            findFolder.ShowNewFolderButton = false;
            findFolder.Description = "Select the folder where your game packages are located";
            findFolder.UseDescriptionForTitle = true;
            findFolder.SelectedPath = TS4PathString.Text;
            if (findFolder.ShowDialog() == true)
            {
                TS4PathString.Text = findFolder.SelectedPath;
            }
        }

        private void Content_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog findFolder = new VistaFolderBrowserDialog();
            findFolder.ShowNewFolderButton = false;
            findFolder.Description = "Select the folder where your Sims 4 SDX downloaded content is located";
            findFolder.UseDescriptionForTitle = true;
            findFolder.SelectedPath = TS4ContentString.Text;
            if (findFolder.ShowDialog() == true)
            {
                TS4ContentString.Text = findFolder.SelectedPath;
            }
        }

        private void Folder_button2_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog findFolder = new VistaFolderBrowserDialog();
            findFolder.ShowNewFolderButton = false;
            findFolder.Description = "Select the folder where your Sims 4 mods and custom content are located";
            findFolder.UseDescriptionForTitle = true;
            findFolder.SelectedPath = TS4UserPathString.Text;
            if (findFolder.ShowDialog() == true)
            {
                TS4UserPathString.Text = findFolder.SelectedPath;
            }
        }

        private void Folder_button3_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog findFolder = new VistaFolderBrowserDialog();
            findFolder.ShowNewFolderButton = false;
            findFolder.Description = "Select the folder where your Sims 4 savegame files are located";
            findFolder.UseDescriptionForTitle = true;
            findFolder.SelectedPath = TS4SavesPathString.Text;
            if (findFolder.ShowDialog() == true)
            {
                TS4SavesPathString.Text = findFolder.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TS4Path = TS4PathString.Text;
            Properties.Settings.Default.TS4ContentPath = TS4ContentString.Text;
            Properties.Settings.Default.TS4ModsPath = TS4UserPathString.Text;
            Properties.Settings.Default.TS4SavesPath = TS4SavesPathString.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
