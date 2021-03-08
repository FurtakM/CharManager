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
    public partial class FormID : Form
    {
        public string id;

        public FormID()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.id = this.textBoxID.Text;
            this.Close();
        }

        private void textBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            btnAdd.Enabled = (textBoxID.Text.Length > 2);

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
