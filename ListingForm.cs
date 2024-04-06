using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS4SimRipper
{
    public partial class ListingForm : Form
    {
        public ListingForm(string listing, string title)
        {
            InitializeComponent();
            this.Listing_textBox.Text = listing;
            this.Listing_textBox.Select(0, 0);
            this.Text = title;
        }
    }
}
