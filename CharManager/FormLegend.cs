using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CharManager
{
    public partial class FormLegend : Form
    {
        public FormLegend()
        {
            InitializeComponent();

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height / 2 - (this.Height / 2));

            dataGridView1.Rows.Add("0", "0", "1000");
            dataGridView1.Rows.Add("0", "1", "3000");
            dataGridView1.Rows.Add("0", "2", "7000");
            dataGridView1.Rows.Add("0", "3", "11000");
            dataGridView1.Rows.Add("0", "4", "20000");
            dataGridView1.Rows.Add("0", "5", "35000");
            dataGridView1.Rows.Add("0", "6", "55000");
            dataGridView1.Rows.Add("0", "7", "90000");
            dataGridView1.Rows.Add("0", "8", "150000");
            dataGridView1.Rows.Add("0", "9", "240000");
            dataGridView1.Rows.Add("1", "1", "1000");
            dataGridView1.Rows.Add("1", "2", "3000");
            dataGridView1.Rows.Add("2", "2", "1000");
            dataGridView1.Rows.Add("2", "3", "3000");
            dataGridView1.Rows.Add("2", "4", "7000");
        }
    }
}
