using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CharManager
{
    public partial class FormModBrowser : Form
    {
        public List<string> ModsList = new List<string>();
        public List<string> ModsNamesList = new List<string>();

        public FormModBrowser()
        {
            InitializeComponent();

            if (!Gallery.DLLExist())
            {
                MessageBox.Show(
                    "XichtDLL.dll not found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Process.GetCurrentProcess().Kill();
                return;
            }

            ModLoader modLoader = new ModLoader();
            ModsList = modLoader.GetModsList();
            ModsNamesList = modLoader.GetModsNamesList();

            if (ModsList.Count == 0)
            {
                MessageBox.Show(
                    "Mods directory not found or is empty!", 
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );

                Process.GetCurrentProcess().Kill();
                return;
            }

            foreach (string item in ModsNamesList)
            {
                comboBox1.Items.Add(item);
            }

            comboBox1.SelectedIndex = 0;
        }

        // select mod changed
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var SelectedMod = ModsList.ElementAt(comboBox1.SelectedIndex);

            if (!String.IsNullOrEmpty(SelectedMod))
            {
                pictureBox1.Image = ModLoader.GetModImage(SelectedMod);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void btnLoadMod_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            FormMain form2 = new FormMain(ModsList.ElementAt(comboBox1.SelectedIndex));
            form2.ShowDialog();
        }
    }
}
