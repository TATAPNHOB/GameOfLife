using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        int n; int m;
        int max = 1000;
        int width, height;
        int[,] cells;
        float cell_size;
        int hover_x, hover_y;
        int population;
        public Form1()
        {
            InitializeComponent();
            width = pictureBox1.Width;
            height = pictureBox1.Height - panel1.Height;
            pictureBox1.BackColor = Color.Black;
            cells = new int[max, max];
            cell_size = (float)numericUpDown1.Value;
            n = (int)(height / cell_size);
            m = (int)(width / cell_size);
            hover_x = -1;
            hover_y = -1;
            population = 0;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            label3.Text = "n: " + n + " m: " + m;
            for (int i = 0; i <= n + 1; i++)
            {
                e.Graphics.DrawLine(Pens.Gray, 0, i * cell_size, (m + 1) * cell_size, i * cell_size);
            }
            for (int i = 0; i <= m + 1; i++)
            {
                e.Graphics.DrawLine(Pens.Gray, i * cell_size, 0, i * cell_size, (n + 1) * cell_size);
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (cells[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(i * (int)cell_size, j * (int)cell_size, (int)cell_size, (int)cell_size));
                    }
                }
            }
            if (hover_x != -1 && hover_y != -1)
            e.Graphics.FillRectangle(cells[hover_x, hover_y] == 1 ? Brushes.LightGray : Brushes.Gray, new Rectangle(hover_x * (int)cell_size, hover_y * (int)cell_size, (int)cell_size, (int)cell_size));
        }

        private void Progress()
        {
            int[,] new_cells = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int neighbours = GetLiveNeighbours(i, j);
                    if (cells[i, j] == 1)
                    {
                        if (neighbours < 2) new_cells[i, j] = 0;
                        else if (neighbours > 3) new_cells[i, j] = 0;
                        else new_cells[i, j] = cells[i, j];
                    }
                    else
                    {
                        if (neighbours == 3) new_cells[i, j] = 1;
                        else new_cells[i, j] = cells[i, j];
                    }
                }
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    cells[i, j] = new_cells[i, j];
                }
            }
            population++;
            label2.Text = "Population N: " + population;
        }
        private int GetLiveNeighbours(int i, int j)
        {
            int res = 0;

            res += (i != 0 && j != 0) ? cells[i - 1, j - 1] : 0;
            res += (i != 0) ? cells[i - 1, j] : 0;
            res += (i != 0 && j != max - 1) ? cells[i - 1, j + 1] : 0;
            res += (j != 0) ? cells[i, j - 1] : 0;
            res += (j != max - 1) ? cells[i, j + 1] : 0;
            res += (i != max - 1 && j != 0) ? cells[i + 1, j - 1] : 0;
            res += (i != max - 1) ? cells[i + 1, j] : 0;
            res += (i != max - 1 && j != max - 1) ? cells[i + 1, j + 1] : 0;

            return res;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (int)(e.X / cell_size);
            int y = (int)(e.Y / cell_size);
            if (cells[x, y] != 1)
            cells[x, y] = 1;
            else cells[x, y] = 0;
            pictureBox1.Refresh();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            width = pictureBox1.Width;
            height = pictureBox1.Height - panel1.Height;
            n = (int)(height / cell_size);
            m = (int)(width / cell_size);
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Progress();
            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                button1.Text = "Start";
            }
            else
            {
                timer1.Start();
                button1.Text = "Stop";
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            hover_x = -1;
            hover_y = -1;
            pictureBox1.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cell_size = (float)numericUpDown1.Value;
            n = (int)(height / cell_size);
            m = (int)(width / cell_size);
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            hover_x = (int)(e.X / cell_size);
            hover_y = (int)(e.Y / cell_size);
            label1.Text = "X: " + hover_x + " Y: " + hover_y;
            pictureBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Progress();
            pictureBox1.Refresh();
        }
    }
}
