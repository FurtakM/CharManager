using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace CharManager
{
    public partial class FormMain : Form
    {
        FormGallery formGallery;
        FormLegend formLegend;
        FormAbout formAbout;

        ModManager modManager;
        Character character;

        public FormMain(string mod)
        {
            InitializeComponent();

            this.Text += " " + mod;

            this.modManager = new ModManager(mod);

            if (!this.modManager.ParseCampaigns())
            {
                MessageBox.Show(
                    "Campaings directory not found or it's empty! \nCheck path: " + this.modManager.path,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                ); ;

                Process.GetCurrentProcess().Kill();
                return;
            }

            for (int i = 0; i <= 10; i++)
            {
                comboBoxC1.Items.Add(i);
                comboBoxC2.Items.Add(i);
                comboBoxE1.Items.Add(i);
                comboBoxE2.Items.Add(i);
                comboBoxM1.Items.Add(i);
                comboBoxM2.Items.Add(i);
                comboBoxS1.Items.Add(i);
                comboBoxS2.Items.Add(i);
            }

            foreach (var campaign in this.modManager.campaings)
            {
                comboBoxCamp.Items.Add(campaign);
            }
        }
        private void FormMain_Shown(object sender, EventArgs e)
        {
            comboBoxCamp.SelectedIndex = 0;
            this.LoadCampaign(0);
        }

        private void LoadCampaign(int index)
        {
            if (!modManager.LoadCampaign(index))
            {
                MessageBox.Show(
                    "Start.txt parsing error.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Process.GetCurrentProcess().Kill();
                return;
            }

            listBoxCharacters.DataSource = modManager.GetCharactersList();
            labelCharacters.Text = modManager.charactersCount.ToString();
        }

        private void LoadCharacter(int index)
        {
            this.character = modManager.characters.ElementAt(index);

            textBoxName.Text = character.GetName();
            textBoxSpeed.Text = character.GetSpeed().ToString();
            textBoxStrength.Text = character.GetArmor().ToString();
            textBoxVoice.Text = character.GetVoice().ToString();
            comboBoxNation.SelectedIndex = character.GetNation();
            comboBoxSex.SelectedIndex = character.GetSex() - 1;
            comboBoxClass.SelectedIndex = character.GetProfession() - 1;
            comboBoxFirstMission.SelectedIndex = character.GetFirstMission() - 1;
            checkBoxCommander.Checked = character.GetImportance() >= 100;

            comboBoxC1.SelectedIndex = character.GetBasicSkill(0);
            comboBoxC2.SelectedIndex = character.GetSkill(0);
            comboBoxE1.SelectedIndex = character.GetBasicSkill(1);
            comboBoxE2.SelectedIndex = character.GetSkill(1);
            comboBoxM1.SelectedIndex = character.GetBasicSkill(2);
            comboBoxM2.SelectedIndex = character.GetSkill(2);
            comboBoxS1.SelectedIndex = character.GetBasicSkill(3);
            comboBoxS2.SelectedIndex = character.GetSkill(3);

            textBoxE1.Text = character.GetExp(0).ToString();
            textBoxE2.Text = character.GetExp(1).ToString();
            textBoxE3.Text = character.GetExp(2).ToString();
            textBoxE4.Text = character.GetExp(3).ToString();

            if (character.GetFaceNumber() > 0)
            {
                try
                {
                    Gallery gallery = new Gallery(modManager.path + "\\Gallery\\" + character.GetGallery() + ".xgl");
                    avatarImage.Image = gallery.GetAvatar(character.GetFaceNumber());
                    avatarImage.Enabled = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "No avatar found! \nCheck path: " + modManager.path + "\\Gallery\\" + character.GetGallery() + ".xgl\n\nReason: " + e.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    avatarImage.Image = Properties.Resources.none;
                }
            }
            else if (character.GetPortret().Count > 0)
            {
                avatarImage.Enabled = false;
                avatarImage.Image = Properties.Resources.none;

                MessageBox.Show(
                    "This character using PORTRET which is not support by this program. You can't change avatar.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else
            {
                avatarImage.Image = Properties.Resources.none;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void listBoxCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadCharacter(listBoxCharacters.SelectedIndex);
        }

        private void btnRandAttr_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            textBoxSpeed.Text = rand.Next(8, 12).ToString();
            textBoxStrength.Text = rand.Next(8, 12).ToString();

            this.character.SetArmor(Int32.Parse(textBoxStrength.Text));
            this.character.SetSpeed(Int32.Parse(textBoxSpeed.Text));
        }

        private void AvatarChanged(string galleryName, int faceNumber)
        {
            this.character.SetGallery(galleryName, faceNumber);

            try
            {
                avatarImage.Image = (new Gallery(modManager.path + "\\Gallery\\" + galleryName + ".xgl")).GetAvatar(faceNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "No avatar found!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                avatarImage.Image = Properties.Resources.none;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void btnLevelHelp_Click(object sender, EventArgs e)
        {
            if (this.formLegend == null || !this.formLegend.Visible)
            {
                this.formLegend = new FormLegend();
                this.formLegend.Show();
            }
            else
            {
                this.formLegend.Activate();
            }
        }

        private void toolStripMenuRestart_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really wanna reset all your changes?", "Warning", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                this.modManager.RestoreDefault();
                this.LoadCampaign(comboBoxCamp.SelectedIndex);
                this.progressBarCompile.Value = 0;
                this.textBoxCompile.Text = "";
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really wanna change campaign? All unsaved changes will be lost.", "Warning", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                this.LoadCampaign(comboBoxCamp.SelectedIndex);
            }
        }

        private void avatarImage_Click(object sender, EventArgs e)
        {
            if (character.GetGallery().Length > 0)
            {
                using (this.formGallery = new FormGallery(modManager.path + "\\Gallery\\", character.GetGallery(), character.GetFaceNumber()))
                {
                    this.formGallery.AvatarChanged += AvatarChanged;
                    this.formGallery.ShowDialog();
                }
            }
            else
            {
                using (this.formGallery = new FormGallery(modManager.path + "\\Gallery\\"))
                {
                    this.formGallery.AvatarChanged += AvatarChanged;
                    this.formGallery.ShowDialog();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Compile())
                return;

            if (this.modManager.Save())
            {
                MessageBox.Show(
                    "Save complete!",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.LoadCampaign(comboBoxCamp.SelectedIndex);
            }
            else
            {
                MessageBox.Show(
                    "Save failed.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.formAbout == null || !this.formAbout.Visible)
            {
                this.formAbout = new FormAbout();
                formAbout.Show();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really wanna reset all your changes for this character?", "Warning", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                int index = listBoxCharacters.SelectedIndex;

                this.modManager.RestoreCurrentCharacterDefault(index);                
                listBoxCharacters.DataSource = modManager.GetCharactersList();
                listBoxCharacters.SelectedIndex = index;
            }
        }

        private void textBoxName_Leave(object sender, EventArgs e)
        {
            string name = textBoxName.Text;

            this.character.SetName(name);
        }

        private void comboBoxNation_Leave(object sender, EventArgs e)
        {
            this.character.SetNation(comboBoxNation.SelectedIndex);
        }

        private void comboBoxSex_Leave(object sender, EventArgs e)
        {
            this.character.SetSex(comboBoxSex.SelectedIndex + 1);
        }

        private void textBoxSpeed_Leave(object sender, EventArgs e)
        {
            this.character.SetSpeed(Int32.Parse(textBoxSpeed.Text));
        }

        private void textBoxSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBoxClass_Leave(object sender, EventArgs e)
        {
            this.character.SetProfession(comboBoxClass.SelectedIndex + 1);
        }

        private void textBoxStrength_Leave(object sender, EventArgs e)
        {
            this.character.SetArmor(Int32.Parse(textBoxStrength.Text));
        }

        private void textBoxStrength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBoxFirstMission_Leave(object sender, EventArgs e)
        {
            this.character.SetFirstMission(comboBoxFirstMission.SelectedIndex + 1);
        }

        private void textBoxVoice_Leave(object sender, EventArgs e)
        {
            this.character.SetVoice(Int32.Parse(textBoxVoice.Text));
        }

        private void textBoxVoice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBoxC1_Leave(object sender, EventArgs e)
        {
            this.character.SetBasicSkill(0, comboBoxC1.SelectedIndex);
            comboBoxC2.SelectedIndex = this.character.SetSkill(0);
        }

        private void comboBoxE1_Leave(object sender, EventArgs e)
        {
            this.character.SetBasicSkill(1, comboBoxE1.SelectedIndex);
            comboBoxE2.SelectedIndex = this.character.SetSkill(1);
        }

        private void comboBoxM1_MouseLeave(object sender, EventArgs e)
        {
            this.character.SetBasicSkill(2, comboBoxM1.SelectedIndex);
            comboBoxM2.SelectedIndex = this.character.SetSkill(2);
        }

        private void comboBoxS1_Leave(object sender, EventArgs e)
        {
            this.character.SetBasicSkill(3, comboBoxS1.SelectedIndex);
            comboBoxS2.SelectedIndex = this.character.SetSkill(3);
        }

        private void btnRandomExp_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                this.character.SetExp(i, rand.Next(0, 150000));
            }

            textBoxE1.Text = this.character.GetExp(0).ToString();
            textBoxE2.Text = this.character.GetExp(1).ToString();
            textBoxE3.Text = this.character.GetExp(2).ToString();
            textBoxE4.Text = this.character.GetExp(3).ToString();

            textBoxE1_Leave(null, null);
            textBoxE2_Leave(null, null);
            textBoxE3_Leave(null, null);
            textBoxE4_Leave(null, null);
        }

        private void textBoxE1_Leave(object sender, EventArgs e)
        {
            this.character.SetExp(0, Int32.Parse(textBoxE1.Text));
            comboBoxC2.SelectedIndex = this.character.SetSkill(0);
        }

        private void textBoxE1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxE2_Leave(object sender, EventArgs e)
        {
            this.character.SetExp(1, Int32.Parse(textBoxE2.Text));
            comboBoxE2.SelectedIndex = this.character.SetSkill(1);
        }

        private void textBoxE2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxE3_Leave(object sender, EventArgs e)
        {
            this.character.SetExp(2, Int32.Parse(textBoxE3.Text));
            comboBoxM2.SelectedIndex = this.character.SetSkill(2);
        }

        private void textBoxE3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxE4_Leave(object sender, EventArgs e)
        {
            this.character.SetExp(3, Int32.Parse(textBoxE4.Text));
            comboBoxS2.SelectedIndex = this.character.SetSkill(3);
        }

        private void textBoxE4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBoxCommander_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.character.GetImportance() < 100)
                this.character.SetImportance(100);
            else
                this.character.SetImportance(0);
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnRemoveHero_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really wanna delete this hero?", "Warning", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                int index = listBoxCharacters.SelectedIndex;

                this.modManager.Delete(index);

                listBoxCharacters.DataSource = modManager.GetCharactersList();
                labelCharacters.Text = modManager.charactersCount.ToString();

                if (index > 0)
                    index--;

                this.listBoxCharacters.SelectedIndex = index;
            }
        }

        private void btnAddHero_Click(object sender, EventArgs e)
        {
            using (var form = new FormID())
            {
                var result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (form.id.Length == 0)
                    {
                        MessageBox.Show(
                            "ID cannot be empty!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        return;
                    }

                    if (this.modManager.Add(form.id))
                    {
                        listBoxCharacters.DataSource = modManager.GetCharactersList();
                        labelCharacters.Text = modManager.charactersCount.ToString();

                        this.listBoxCharacters.SelectedIndex = this.listBoxCharacters.Items.Count - 1;
                    }
                    else
                    {
                        MessageBox.Show(
                            "This ID is already taken!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        private bool Compile()
        {
            int count = this.modManager.charactersCount;
            bool result = true;
            int firstIssueFoundId = -1;

            this.textBoxCompile.Text = "";

            this.progressBarCompile.Value = 0;
            this.progressBarCompile.Maximum = this.modManager.charactersCount;
            this.progressBarCompile.ForeColor = System.Drawing.Color.Green;

            for (int i = 0; i < count; i++)
            {
                try
                {
                    this.modManager.characters[i].Compile();
                }
                catch (Exception ex)
                {
                    this.textBoxCompile.Text += ex.Message + "\n";
                    result = false;

                    if (firstIssueFoundId == -1)
                        firstIssueFoundId = i;
                }
            }

            if (firstIssueFoundId > -1)
            {
                this.progressBarCompile.ForeColor = System.Drawing.Color.Red;
                this.progressBarCompile.Value = firstIssueFoundId;
            }
            else
            {
                this.progressBarCompile.Value = count;
                this.textBoxCompile.Text = "Compile completed!";
            }

            return result;
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            Compile();   
        }
    }
}
