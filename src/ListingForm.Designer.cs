namespace TS4SimRipper
{
    partial class ListingForm
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
            this.Listing_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Listing_textBox
            // 
            this.Listing_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Listing_textBox.Location = new System.Drawing.Point(0, 0);
            this.Listing_textBox.Multiline = true;
            this.Listing_textBox.Name = "Listing_textBox";
            this.Listing_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Listing_textBox.Size = new System.Drawing.Size(800, 450);
            this.Listing_textBox.TabIndex = 0;
            // 
            // ListingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Listing_textBox);
            this.Name = "ListingForm";
            this.Text = "CAS Parts Listing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Listing_textBox;
    }
}