using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boulder_Dash_Lab3
{
    public partial class CreateForm : Form
    {
        public int FieldWidth { private set; get; } = 15;
        public int FieldHeight { private set; get; } = 10;
        public int NeedDiamonds { private set; get; } = 10;
        public int Players { private set; get; } = 1;
        public int Health { private set; get; } = 3;
        public CreateForm()
            => InitializeComponent();
        public CreateForm(int init_height, int init_width)
            => (Width, Height) = (init_width, init_height);

        private void NumericUpDown1_Validated(object sender, EventArgs e)
            => FieldWidth = (int)numericUpDown1.Value;

        private void NumericUpDown2_Validated(object sender, EventArgs e)
            => FieldHeight = (int)numericUpDown2.Value;

        private void NumericUpDown4_Validated(object sender, EventArgs e)
            => NeedDiamonds = (int)numericUpDown4.Value;

        private void NumericUpDown3_Validated(object sender, EventArgs e)
            => Players = (int)numericUpDown3.Value;

        private void NumericUpDown5_Validated(object sender, EventArgs e)
            => Health = (int)numericUpDown5.Value;

        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
