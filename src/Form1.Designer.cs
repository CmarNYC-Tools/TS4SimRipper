namespace TS4SimRipper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveGameFile = new System.Windows.Forms.TextBox();
            this.SaveGameFile_button = new System.Windows.Forms.Button();
            this.sims_listBox = new System.Windows.Forms.ListBox();
            this.SaveOBJ_button = new System.Windows.Forms.Button();
            this.SaveDAE_button = new System.Windows.Forms.Button();
            this.SaveMS3D_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Working_label = new System.Windows.Forms.Label();
            this.Outfits_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Occults_comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Pregnancy_trackBar = new System.Windows.Forms.TrackBar();
            this.CleanDAE_checkBox = new System.Windows.Forms.CheckBox();
            this.SimInfo_button = new System.Windows.Forms.Button();
            this.SimError_button = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.HQSize_radioButton = new System.Windows.Forms.RadioButton();
            this.StandardSize_radioButton = new System.Windows.Forms.RadioButton();
            this.Skinblend37_radioButton = new System.Windows.Forms.RadioButton();
            this.SaveMakeup_button = new System.Windows.Forms.Button();
            this.SaveSkin_button = new System.Windows.Forms.Button();
            this.SaveClothing_button = new System.Windows.Forms.Button();
            this.SaveNormals_button = new System.Windows.Forms.Button();
            this.NormalConvert_checkBox = new System.Windows.Forms.CheckBox();
            this.SaveEmission_button = new System.Windows.Forms.Button();
            this.SaveTextures_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SkinState_comboBox = new System.Windows.Forms.ComboBox();
            this.SimFilter_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.TanLines_checkBox = new System.Windows.Forms.CheckBox();
            this.BoneSize_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SeparateMeshes_comboBox = new System.Windows.Forms.ComboBox();
            this.SaveGlass_button = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.LinkTexture_checkBox = new System.Windows.Forms.CheckBox();
            this.SimTrouble_button = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SkinBlend3_radioButton = new System.Windows.Forms.RadioButton();
            this.SkinBlend2_radioButton = new System.Windows.Forms.RadioButton();
            this.SkinBlend1_radioButton = new System.Windows.Forms.RadioButton();
            this.SkinOverlay_checkBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.OverlaySort_comboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SortBy_comboBox = new System.Windows.Forms.ComboBox();
            this.GameName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.morphPreview1 = new TS4SimRipper.MorphPreview();
            this.SaveSIMO_button = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pregnancy_trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSize_numericUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Game:";
            // 
            // SaveGameFile
            // 
            this.SaveGameFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveGameFile.Location = new System.Drawing.Point(83, 41);
            this.SaveGameFile.Name = "SaveGameFile";
            this.SaveGameFile.Size = new System.Drawing.Size(604, 20);
            this.SaveGameFile.TabIndex = 1;
            // 
            // SaveGameFile_button
            // 
            this.SaveGameFile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveGameFile_button.Location = new System.Drawing.Point(693, 35);
            this.SaveGameFile_button.Name = "SaveGameFile_button";
            this.SaveGameFile_button.Size = new System.Drawing.Size(289, 30);
            this.SaveGameFile_button.TabIndex = 2;
            this.SaveGameFile_button.Text = "Select";
            this.SaveGameFile_button.UseVisualStyleBackColor = true;
            this.SaveGameFile_button.Click += new System.EventHandler(this.SaveGameFile_button_Click);
            // 
            // sims_listBox
            // 
            this.sims_listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sims_listBox.FormattingEnabled = true;
            this.sims_listBox.Location = new System.Drawing.Point(9, 247);
            this.sims_listBox.Name = "sims_listBox";
            this.sims_listBox.Size = new System.Drawing.Size(219, 355);
            this.sims_listBox.TabIndex = 4;
            this.sims_listBox.SelectedIndexChanged += new System.EventHandler(this.sims_listBox_SelectedIndexChanged);
            // 
            // SaveOBJ_button
            // 
            this.SaveOBJ_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveOBJ_button.Location = new System.Drawing.Point(746, 73);
            this.SaveOBJ_button.Name = "SaveOBJ_button";
            this.SaveOBJ_button.Size = new System.Drawing.Size(70, 30);
            this.SaveOBJ_button.TabIndex = 6;
            this.SaveOBJ_button.Text = "OBJ";
            this.SaveOBJ_button.UseVisualStyleBackColor = true;
            this.SaveOBJ_button.Click += new System.EventHandler(this.SaveOBJ_button_Click);
            // 
            // SaveDAE_button
            // 
            this.SaveDAE_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveDAE_button.Location = new System.Drawing.Point(912, 73);
            this.SaveDAE_button.Name = "SaveDAE_button";
            this.SaveDAE_button.Size = new System.Drawing.Size(70, 30);
            this.SaveDAE_button.TabIndex = 7;
            this.SaveDAE_button.Text = "DAE";
            this.SaveDAE_button.UseVisualStyleBackColor = true;
            this.SaveDAE_button.Click += new System.EventHandler(this.SaveDAE_button_Click);
            // 
            // SaveMS3D_button
            // 
            this.SaveMS3D_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveMS3D_button.Location = new System.Drawing.Point(831, 73);
            this.SaveMS3D_button.Name = "SaveMS3D_button";
            this.SaveMS3D_button.Size = new System.Drawing.Size(70, 30);
            this.SaveMS3D_button.TabIndex = 8;
            this.SaveMS3D_button.Text = "MS3D";
            this.SaveMS3D_button.UseVisualStyleBackColor = true;
            this.SaveMS3D_button.Click += new System.EventHandler(this.SaveMS3D_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Working_label
            // 
            this.Working_label.AutoSize = true;
            this.Working_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Working_label.Location = new System.Drawing.Point(395, 427);
            this.Working_label.Name = "Working_label";
            this.Working_label.Size = new System.Drawing.Size(172, 37);
            this.Working_label.TabIndex = 12;
            this.Working_label.Text = "Working....";
            this.Working_label.Visible = false;
            // 
            // Outfits_comboBox
            // 
            this.Outfits_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Outfits_comboBox.FormattingEnabled = true;
            this.Outfits_comboBox.Location = new System.Drawing.Point(737, 368);
            this.Outfits_comboBox.Name = "Outfits_comboBox";
            this.Outfits_comboBox.Size = new System.Drawing.Size(245, 21);
            this.Outfits_comboBox.TabIndex = 14;
            this.Outfits_comboBox.SelectedIndexChanged += new System.EventHandler(this.Outfits_comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(690, 371);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Outfit:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(690, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Occult:";
            // 
            // Occults_comboBox
            // 
            this.Occults_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Occults_comboBox.FormattingEnabled = true;
            this.Occults_comboBox.Location = new System.Drawing.Point(737, 341);
            this.Occults_comboBox.Name = "Occults_comboBox";
            this.Occults_comboBox.Size = new System.Drawing.Size(245, 21);
            this.Occults_comboBox.TabIndex = 21;
            this.Occults_comboBox.SelectedIndexChanged += new System.EventHandler(this.Occults_comboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(690, 401);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Pregnant:";
            // 
            // Pregnancy_trackBar
            // 
            this.Pregnancy_trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pregnancy_trackBar.LargeChange = 1;
            this.Pregnancy_trackBar.Location = new System.Drawing.Point(749, 395);
            this.Pregnancy_trackBar.Name = "Pregnancy_trackBar";
            this.Pregnancy_trackBar.Size = new System.Drawing.Size(172, 45);
            this.Pregnancy_trackBar.TabIndex = 23;
            this.Pregnancy_trackBar.Scroll += new System.EventHandler(this.Pregnancy_trackBar_Scroll);
            // 
            // CleanDAE_checkBox
            // 
            this.CleanDAE_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CleanDAE_checkBox.AutoSize = true;
            this.CleanDAE_checkBox.Location = new System.Drawing.Point(746, 136);
            this.CleanDAE_checkBox.Name = "CleanDAE_checkBox";
            this.CleanDAE_checkBox.Size = new System.Drawing.Size(206, 17);
            this.CleanDAE_checkBox.TabIndex = 24;
            this.CleanDAE_checkBox.Text = "Clean DAE mesh? (Removes doubles)";
            this.toolTip1.SetToolTip(this.CleanDAE_checkBox, "Removes doubles");
            this.CleanDAE_checkBox.UseVisualStyleBackColor = true;
            // 
            // SimInfo_button
            // 
            this.SimInfo_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SimInfo_button.Location = new System.Drawing.Point(693, 538);
            this.SimInfo_button.Name = "SimInfo_button";
            this.SimInfo_button.Size = new System.Drawing.Size(145, 30);
            this.SimInfo_button.TabIndex = 25;
            this.SimInfo_button.Text = "Sim Information Listing";
            this.SimInfo_button.UseVisualStyleBackColor = true;
            this.SimInfo_button.Click += new System.EventHandler(this.SimInfo_Click);
            // 
            // SimError_button
            // 
            this.SimError_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SimError_button.Location = new System.Drawing.Point(844, 538);
            this.SimError_button.Name = "SimError_button";
            this.SimError_button.Size = new System.Drawing.Size(138, 30);
            this.SimError_button.TabIndex = 26;
            this.SimError_button.Text = "Sim Error Listing";
            this.SimError_button.UseVisualStyleBackColor = true;
            this.SimError_button.Click += new System.EventHandler(this.SimError_Click);
            // 
            // HQSize_radioButton
            // 
            this.HQSize_radioButton.AutoSize = true;
            this.HQSize_radioButton.Location = new System.Drawing.Point(81, 3);
            this.HQSize_radioButton.Name = "HQSize_radioButton";
            this.HQSize_radioButton.Size = new System.Drawing.Size(41, 17);
            this.HQSize_radioButton.TabIndex = 1;
            this.HQSize_radioButton.Text = "HQ";
            this.toolTip1.SetToolTip(this.HQSize_radioButton, "Must be set before loading model");
            this.HQSize_radioButton.UseVisualStyleBackColor = true;
            this.HQSize_radioButton.CheckedChanged += new System.EventHandler(this.HQSize_radioButton_CheckedChanged);
            // 
            // StandardSize_radioButton
            // 
            this.StandardSize_radioButton.AutoSize = true;
            this.StandardSize_radioButton.Checked = true;
            this.StandardSize_radioButton.Location = new System.Drawing.Point(3, 3);
            this.StandardSize_radioButton.Name = "StandardSize_radioButton";
            this.StandardSize_radioButton.Size = new System.Drawing.Size(68, 17);
            this.StandardSize_radioButton.TabIndex = 0;
            this.StandardSize_radioButton.TabStop = true;
            this.StandardSize_radioButton.Text = "Standard";
            this.toolTip1.SetToolTip(this.StandardSize_radioButton, "Must be set before loading model");
            this.StandardSize_radioButton.UseVisualStyleBackColor = true;
            // 
            // Skinblend37_radioButton
            // 
            this.Skinblend37_radioButton.AutoSize = true;
            this.Skinblend37_radioButton.Location = new System.Drawing.Point(61, 4);
            this.Skinblend37_radioButton.Name = "Skinblend37_radioButton";
            this.Skinblend37_radioButton.Size = new System.Drawing.Size(60, 17);
            this.Skinblend37_radioButton.TabIndex = 3;
            this.Skinblend37_radioButton.TabStop = true;
            this.Skinblend37_radioButton.Text = "Original";
            this.toolTip1.SetToolTip(this.Skinblend37_radioButton, "Does NOT apply color shifts");
            this.Skinblend37_radioButton.UseVisualStyleBackColor = true;
            // 
            // SaveMakeup_button
            // 
            this.SaveMakeup_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveMakeup_button.Location = new System.Drawing.Point(776, 248);
            this.SaveMakeup_button.Name = "SaveMakeup_button";
            this.SaveMakeup_button.Size = new System.Drawing.Size(65, 25);
            this.SaveMakeup_button.TabIndex = 63;
            this.SaveMakeup_button.Text = "Makeup";
            this.toolTip1.SetToolTip(this.SaveMakeup_button, "Tattoos, skin details, eyebrows, eye color, makeup texture");
            this.SaveMakeup_button.UseVisualStyleBackColor = true;
            this.SaveMakeup_button.Click += new System.EventHandler(this.SaveMakeup_button_Click);
            // 
            // SaveSkin_button
            // 
            this.SaveSkin_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSkin_button.Location = new System.Drawing.Point(917, 217);
            this.SaveSkin_button.Name = "SaveSkin_button";
            this.SaveSkin_button.Size = new System.Drawing.Size(65, 25);
            this.SaveSkin_button.TabIndex = 54;
            this.SaveSkin_button.Text = "Skin";
            this.toolTip1.SetToolTip(this.SaveSkin_button, "Skin texture only");
            this.SaveSkin_button.UseVisualStyleBackColor = true;
            this.SaveSkin_button.Click += new System.EventHandler(this.SaveSkin_button_Click);
            // 
            // SaveClothing_button
            // 
            this.SaveClothing_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveClothing_button.Location = new System.Drawing.Point(846, 248);
            this.SaveClothing_button.Name = "SaveClothing_button";
            this.SaveClothing_button.Size = new System.Drawing.Size(65, 25);
            this.SaveClothing_button.TabIndex = 55;
            this.SaveClothing_button.Text = "Clothing";
            this.toolTip1.SetToolTip(this.SaveClothing_button, "Clothing, hair, accessories, nail polish texture");
            this.SaveClothing_button.UseVisualStyleBackColor = true;
            this.SaveClothing_button.Click += new System.EventHandler(this.SaveClothing_button_Click);
            // 
            // SaveNormals_button
            // 
            this.SaveNormals_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveNormals_button.Location = new System.Drawing.Point(776, 279);
            this.SaveNormals_button.Name = "SaveNormals_button";
            this.SaveNormals_button.Size = new System.Drawing.Size(65, 25);
            this.SaveNormals_button.TabIndex = 60;
            this.SaveNormals_button.Text = "Normals";
            this.toolTip1.SetToolTip(this.SaveNormals_button, "Normal / Bump map");
            this.SaveNormals_button.UseVisualStyleBackColor = true;
            this.SaveNormals_button.Click += new System.EventHandler(this.SaveNormals_button_Click);
            // 
            // NormalConvert_checkBox
            // 
            this.NormalConvert_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NormalConvert_checkBox.AutoSize = true;
            this.NormalConvert_checkBox.Checked = true;
            this.NormalConvert_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NormalConvert_checkBox.Location = new System.Drawing.Point(847, 284);
            this.NormalConvert_checkBox.Name = "NormalConvert_checkBox";
            this.NormalConvert_checkBox.Size = new System.Drawing.Size(107, 17);
            this.NormalConvert_checkBox.TabIndex = 61;
            this.NormalConvert_checkBox.Text = "Convert to RGB?";
            this.toolTip1.SetToolTip(this.NormalConvert_checkBox, "Convert normals to RGB format");
            this.NormalConvert_checkBox.UseVisualStyleBackColor = true;
            // 
            // SaveEmission_button
            // 
            this.SaveEmission_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveEmission_button.Location = new System.Drawing.Point(917, 248);
            this.SaveEmission_button.Name = "SaveEmission_button";
            this.SaveEmission_button.Size = new System.Drawing.Size(65, 25);
            this.SaveEmission_button.TabIndex = 62;
            this.SaveEmission_button.Text = "Emisson";
            this.toolTip1.SetToolTip(this.SaveEmission_button, "Emission / Glow map");
            this.SaveEmission_button.UseVisualStyleBackColor = true;
            this.SaveEmission_button.Click += new System.EventHandler(this.SaveEmission_button_Click);
            // 
            // SaveTextures_button
            // 
            this.SaveTextures_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveTextures_button.Location = new System.Drawing.Point(775, 217);
            this.SaveTextures_button.Name = "SaveTextures_button";
            this.SaveTextures_button.Size = new System.Drawing.Size(65, 25);
            this.SaveTextures_button.TabIndex = 27;
            this.SaveTextures_button.Text = "Combined";
            this.SaveTextures_button.UseVisualStyleBackColor = true;
            this.SaveTextures_button.Click += new System.EventHandler(this.SaveTextures_button_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(689, 447);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Skin:";
            // 
            // SkinState_comboBox
            // 
            this.SkinState_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SkinState_comboBox.FormattingEnabled = true;
            this.SkinState_comboBox.Items.AddRange(new object[] {
            "Normal",
            "Tanned",
            "Burned"});
            this.SkinState_comboBox.Location = new System.Drawing.Point(736, 444);
            this.SkinState_comboBox.Name = "SkinState_comboBox";
            this.SkinState_comboBox.Size = new System.Drawing.Size(69, 21);
            this.SkinState_comboBox.TabIndex = 29;
            this.SkinState_comboBox.SelectedIndexChanged += new System.EventHandler(this.SkinState_comboBox_SelectedIndexChanged);
            // 
            // SimFilter_checkedListBox
            // 
            this.SimFilter_checkedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SimFilter_checkedListBox.CheckOnClick = true;
            this.SimFilter_checkedListBox.ColumnWidth = 100;
            this.SimFilter_checkedListBox.FormattingEnabled = true;
            this.SimFilter_checkedListBox.Items.AddRange(new object[] {
            "Human",
            "Animal",
            "Mannequin",
            "Male",
            "Female",
            "Infant",
            "Toddler",
            "Child",
            "Teen",
            "YA",
            "Adult",
            "Elder"});
            this.SimFilter_checkedListBox.Location = new System.Drawing.Point(12, 99);
            this.SimFilter_checkedListBox.MultiColumn = true;
            this.SimFilter_checkedListBox.Name = "SimFilter_checkedListBox";
            this.SimFilter_checkedListBox.Size = new System.Drawing.Size(216, 109);
            this.SimFilter_checkedListBox.TabIndex = 36;
            this.SimFilter_checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SimFilter_checkedListBox_ItemCheck);
            // 
            // TanLines_checkBox
            // 
            this.TanLines_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TanLines_checkBox.AutoSize = true;
            this.TanLines_checkBox.Checked = true;
            this.TanLines_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TanLines_checkBox.Location = new System.Drawing.Point(811, 439);
            this.TanLines_checkBox.Name = "TanLines_checkBox";
            this.TanLines_checkBox.Size = new System.Drawing.Size(53, 30);
            this.TanLines_checkBox.TabIndex = 37;
            this.TanLines_checkBox.Text = "Tan\r\nlines?";
            this.TanLines_checkBox.UseVisualStyleBackColor = true;
            this.TanLines_checkBox.CheckedChanged += new System.EventHandler(this.TanLines_checkBox_CheckedChanged);
            // 
            // BoneSize_numericUpDown
            // 
            this.BoneSize_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BoneSize_numericUpDown.Location = new System.Drawing.Point(836, 159);
            this.BoneSize_numericUpDown.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.BoneSize_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BoneSize_numericUpDown.Name = "BoneSize_numericUpDown";
            this.BoneSize_numericUpDown.Size = new System.Drawing.Size(36, 20);
            this.BoneSize_numericUpDown.TabIndex = 38;
            this.BoneSize_numericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // SeparateMeshes_comboBox
            // 
            this.SeparateMeshes_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SeparateMeshes_comboBox.FormattingEnabled = true;
            this.SeparateMeshes_comboBox.Items.AddRange(new object[] {
            "Single mesh and texture",
            "All separate meshes, one texture",
            "Solid and glass meshes and textures"});
            this.SeparateMeshes_comboBox.Location = new System.Drawing.Point(746, 109);
            this.SeparateMeshes_comboBox.Name = "SeparateMeshes_comboBox";
            this.SeparateMeshes_comboBox.Size = new System.Drawing.Size(236, 21);
            this.SeparateMeshes_comboBox.TabIndex = 40;
            this.SeparateMeshes_comboBox.SelectedIndexChanged += new System.EventHandler(this.SeparateMeshes_comboBox_SelectedIndexChanged);
            // 
            // SaveGlass_button
            // 
            this.SaveGlass_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveGlass_button.Location = new System.Drawing.Point(846, 217);
            this.SaveGlass_button.Name = "SaveGlass_button";
            this.SaveGlass_button.Size = new System.Drawing.Size(65, 25);
            this.SaveGlass_button.TabIndex = 41;
            this.SaveGlass_button.Text = "Glass";
            this.SaveGlass_button.UseVisualStyleBackColor = true;
            this.SaveGlass_button.Click += new System.EventHandler(this.SaveGlass_button_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(690, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Texture Size:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.HQSize_radioButton);
            this.panel1.Controls.Add(this.StandardSize_radioButton);
            this.panel1.Location = new System.Drawing.Point(773, 185);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 26);
            this.panel1.TabIndex = 43;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(743, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Blender bone size";
            // 
            // LinkTexture_checkBox
            // 
            this.LinkTexture_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkTexture_checkBox.AutoSize = true;
            this.LinkTexture_checkBox.Checked = true;
            this.LinkTexture_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LinkTexture_checkBox.Location = new System.Drawing.Point(891, 161);
            this.LinkTexture_checkBox.Name = "LinkTexture_checkBox";
            this.LinkTexture_checkBox.Size = new System.Drawing.Size(86, 17);
            this.LinkTexture_checkBox.TabIndex = 45;
            this.LinkTexture_checkBox.Text = "Link textures";
            this.LinkTexture_checkBox.UseVisualStyleBackColor = true;
            // 
            // SimTrouble_button
            // 
            this.SimTrouble_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SimTrouble_button.Location = new System.Drawing.Point(693, 574);
            this.SimTrouble_button.Name = "SimTrouble_button";
            this.SimTrouble_button.Size = new System.Drawing.Size(289, 30);
            this.SimTrouble_button.TabIndex = 46;
            this.SimTrouble_button.Text = "Save Sim Troubleshooting Info";
            this.SimTrouble_button.UseVisualStyleBackColor = true;
            this.SimTrouble_button.Click += new System.EventHandler(this.SimTrouble_button_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.Skinblend37_radioButton);
            this.panel2.Controls.Add(this.SkinBlend3_radioButton);
            this.panel2.Controls.Add(this.SkinBlend2_radioButton);
            this.panel2.Controls.Add(this.SkinBlend1_radioButton);
            this.panel2.Location = new System.Drawing.Point(736, 471);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 27);
            this.panel2.TabIndex = 48;
            // 
            // SkinBlend3_radioButton
            // 
            this.SkinBlend3_radioButton.AutoSize = true;
            this.SkinBlend3_radioButton.Location = new System.Drawing.Point(180, 4);
            this.SkinBlend3_radioButton.Name = "SkinBlend3_radioButton";
            this.SkinBlend3_radioButton.Size = new System.Drawing.Size(46, 17);
            this.SkinBlend3_radioButton.TabIndex = 2;
            this.SkinBlend3_radioButton.TabStop = true;
            this.SkinBlend3_radioButton.Text = "HSL";
            this.SkinBlend3_radioButton.UseVisualStyleBackColor = true;
            this.SkinBlend3_radioButton.CheckedChanged += new System.EventHandler(this.SkinBlend_radioButton_CheckedChanged);
            // 
            // SkinBlend2_radioButton
            // 
            this.SkinBlend2_radioButton.AutoSize = true;
            this.SkinBlend2_radioButton.Location = new System.Drawing.Point(127, 4);
            this.SkinBlend2_radioButton.Name = "SkinBlend2_radioButton";
            this.SkinBlend2_radioButton.Size = new System.Drawing.Size(47, 17);
            this.SkinBlend2_radioButton.TabIndex = 1;
            this.SkinBlend2_radioButton.Text = "HSV";
            this.SkinBlend2_radioButton.UseVisualStyleBackColor = true;
            this.SkinBlend2_radioButton.CheckedChanged += new System.EventHandler(this.SkinBlend_radioButton_CheckedChanged);
            // 
            // SkinBlend1_radioButton
            // 
            this.SkinBlend1_radioButton.AutoSize = true;
            this.SkinBlend1_radioButton.Checked = true;
            this.SkinBlend1_radioButton.Location = new System.Drawing.Point(3, 4);
            this.SkinBlend1_radioButton.Name = "SkinBlend1_radioButton";
            this.SkinBlend1_radioButton.Size = new System.Drawing.Size(52, 17);
            this.SkinBlend1_radioButton.TabIndex = 0;
            this.SkinBlend1_radioButton.TabStop = true;
            this.SkinBlend1_radioButton.Text = "Blend";
            this.SkinBlend1_radioButton.UseVisualStyleBackColor = true;
            this.SkinBlend1_radioButton.CheckedChanged += new System.EventHandler(this.SkinBlend_radioButton_CheckedChanged);
            // 
            // SkinOverlay_checkBox
            // 
            this.SkinOverlay_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SkinOverlay_checkBox.AutoSize = true;
            this.SkinOverlay_checkBox.Checked = true;
            this.SkinOverlay_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SkinOverlay_checkBox.Location = new System.Drawing.Point(870, 446);
            this.SkinOverlay_checkBox.Name = "SkinOverlay_checkBox";
            this.SkinOverlay_checkBox.Size = new System.Drawing.Size(69, 17);
            this.SkinOverlay_checkBox.TabIndex = 49;
            this.SkinOverlay_checkBox.Text = "Colorize?";
            this.SkinOverlay_checkBox.UseVisualStyleBackColor = true;
            this.SkinOverlay_checkBox.CheckedChanged += new System.EventHandler(this.SkinOverlay_checkBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(690, 504);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 26);
            this.label6.TabIndex = 50;
            this.label6.Text = "Skin Details Sort:\r\n(Comp Method 3)";
            // 
            // OverlaySort_comboBox
            // 
            this.OverlaySort_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OverlaySort_comboBox.FormattingEnabled = true;
            this.OverlaySort_comboBox.Items.AddRange(new object[] {
            "Ascending",
            "Descending",
            "By Sort Layer"});
            this.OverlaySort_comboBox.Location = new System.Drawing.Point(788, 508);
            this.OverlaySort_comboBox.Name = "OverlaySort_comboBox";
            this.OverlaySort_comboBox.Size = new System.Drawing.Size(194, 21);
            this.OverlaySort_comboBox.TabIndex = 51;
            this.OverlaySort_comboBox.SelectedIndexChanged += new System.EventHandler(this.OverlaySort_comboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(690, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 52;
            this.label9.Text = "Save As:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(690, 223);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Save Textures:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 217);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 56;
            this.label11.Text = "Sort by:";
            // 
            // SortBy_comboBox
            // 
            this.SortBy_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SortBy_comboBox.FormattingEnabled = true;
            this.SortBy_comboBox.Items.AddRange(new object[] {
            "Last name",
            "First name",
            "Household name"});
            this.SortBy_comboBox.Location = new System.Drawing.Point(53, 214);
            this.SortBy_comboBox.Name = "SortBy_comboBox";
            this.SortBy_comboBox.Size = new System.Drawing.Size(175, 21);
            this.SortBy_comboBox.TabIndex = 57;
            this.SortBy_comboBox.SelectedIndexChanged += new System.EventHandler(this.SortBy_comboBox_SelectedIndexChanged);
            // 
            // GameName
            // 
            this.GameName.Location = new System.Drawing.Point(50, 73);
            this.GameName.Name = "GameName";
            this.GameName.Size = new System.Drawing.Size(178, 20);
            this.GameName.TabIndex = 58;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 59;
            this.label12.Text = "Game:";
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(237, 73);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(447, 531);
            this.elementHost1.TabIndex = 17;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.morphPreview1;
            // 
            // SaveSIMO_button
            // 
            this.SaveSIMO_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSIMO_button.Location = new System.Drawing.Point(693, 310);
            this.SaveSIMO_button.Name = "SaveSIMO_button";
            this.SaveSIMO_button.Size = new System.Drawing.Size(289, 25);
            this.SaveSIMO_button.TabIndex = 65;
            this.SaveSIMO_button.Text = "Save Sim Info Resource";
            this.SaveSIMO_button.UseVisualStyleBackColor = true;
            this.SaveSIMO_button.Click += new System.EventHandler(this.SaveSIMO_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 616);
            this.Controls.Add(this.SaveSIMO_button);
            this.Controls.Add(this.SaveMakeup_button);
            this.Controls.Add(this.SaveEmission_button);
            this.Controls.Add(this.NormalConvert_checkBox);
            this.Controls.Add(this.SaveNormals_button);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.GameName);
            this.Controls.Add(this.SortBy_comboBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.SaveClothing_button);
            this.Controls.Add(this.SaveSkin_button);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.OverlaySort_comboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SkinOverlay_checkBox);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SimTrouble_button);
            this.Controls.Add(this.LinkTexture_checkBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SaveGlass_button);
            this.Controls.Add(this.SeparateMeshes_comboBox);
            this.Controls.Add(this.BoneSize_numericUpDown);
            this.Controls.Add(this.TanLines_checkBox);
            this.Controls.Add(this.SimFilter_checkedListBox);
            this.Controls.Add(this.SkinState_comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SaveTextures_button);
            this.Controls.Add(this.SimError_button);
            this.Controls.Add(this.SimInfo_button);
            this.Controls.Add(this.CleanDAE_checkBox);
            this.Controls.Add(this.Pregnancy_trackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Occults_comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Working_label);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Outfits_comboBox);
            this.Controls.Add(this.SaveMS3D_button);
            this.Controls.Add(this.SaveDAE_button);
            this.Controls.Add(this.SaveOBJ_button);
            this.Controls.Add(this.sims_listBox);
            this.Controls.Add(this.SaveGameFile_button);
            this.Controls.Add(this.SaveGameFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pregnancy_trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSize_numericUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SaveGameFile;
        private System.Windows.Forms.Button SaveGameFile_button;
        private System.Windows.Forms.ListBox sims_listBox;
        private System.Windows.Forms.Button SaveOBJ_button;
        private System.Windows.Forms.Button SaveDAE_button;
        private System.Windows.Forms.Button SaveMS3D_button;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label Working_label;
        private System.Windows.Forms.ComboBox Outfits_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private MorphPreview morphPreview3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Occults_comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar Pregnancy_trackBar;
        private System.Windows.Forms.CheckBox CleanDAE_checkBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button SimInfo_button;
        private System.Windows.Forms.Button SimError_button;
        private System.Windows.Forms.Button SaveTextures_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SkinState_comboBox;
        private System.Windows.Forms.CheckedListBox SimFilter_checkedListBox;
        private System.Windows.Forms.CheckBox TanLines_checkBox;
        private System.Windows.Forms.NumericUpDown BoneSize_numericUpDown;
        private System.Windows.Forms.ComboBox SeparateMeshes_comboBox;
        private System.Windows.Forms.Button SaveGlass_button;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton HQSize_radioButton;
        private System.Windows.Forms.RadioButton StandardSize_radioButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox LinkTexture_checkBox;
        private System.Windows.Forms.Button SimTrouble_button;
        private MorphPreview morphPreview1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton SkinBlend2_radioButton;
        private System.Windows.Forms.RadioButton SkinBlend1_radioButton;
        private System.Windows.Forms.RadioButton SkinBlend3_radioButton;
        private System.Windows.Forms.CheckBox SkinOverlay_checkBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox OverlaySort_comboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button SaveSkin_button;
        private System.Windows.Forms.Button SaveClothing_button;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox SortBy_comboBox;
        private System.Windows.Forms.TextBox GameName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton Skinblend37_radioButton;
        private System.Windows.Forms.Button SaveNormals_button;
        private System.Windows.Forms.CheckBox NormalConvert_checkBox;
        private System.Windows.Forms.Button SaveEmission_button;
        private System.Windows.Forms.Button SaveMakeup_button;
        private System.Windows.Forms.Button SaveSIMO_button;
    }
}

