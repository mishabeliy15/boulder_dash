using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boulder_Dash_Lab3
{
    public partial class Form1 : Form
    {
        Game game;
        public Form1()
        {
            InitializeComponent();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateForm f = new CreateForm();
            if(f.ShowDialog() == DialogResult.OK)
            {
                game = new Game(f.FieldHeight, f.FieldWidth, f.Health, f.NeedDiamonds, pictureBox1, label1, f.Players);
                this.KeyUp += game.Form_KeyUp_Listener;
                game.StartFormGame();
            }
        }

        private void PictureBox1_SizeChanged(object sender, EventArgs e)
            =>  game?.DrawGame();

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (game != null)
                game.EndByUser = true;
            Thread.Sleep(100);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (game != null)
                saveFileDialog1.ShowDialog();
            else
                MessageBox.Show("You try save empty game", "Error");
        }

        private void SaveFileDialog1_FileOk(object sender, CancelEventArgs e)
            => game?.SaveGame(saveFileDialog1.FileName);

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(game != null)
                game.EndByUser = true;
            openFileDialog1.ShowDialog();
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            game = Game.LoadGame(openFileDialog1.FileName);
            game.DrawBox = pictureBox1;
            game.LabelBox = label1;
            this.KeyUp += game.Form_KeyUp_Listener;
            game.StartFormGame();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
            => this.Close();
    }
}
