using System;
using System.Drawing;
using System.Windows.Forms;

namespace CharManager
{
    public partial class FormGallery : Form
    {
        public event FormClosedEventHandler AvatarChanged;
        public delegate void FormClosedEventHandler(string gallery, int faceNumber);

        string path, gallery;
        int selectedAvatar, defaultAvatar;

        public FormGallery(string path)
        {
            InitializeComponent();

            this.path = path;

            comboBox1.DataSource = Gallery.ParseGalleries(this.path);

            this.LoadGallery(this.path, (string)comboBox1.Items[0], 0);
        }

        public FormGallery(string path, string galleryName, int faceNumber)
        {
            InitializeComponent();

            this.path = path;
            this.defaultAvatar = faceNumber;

            comboBox1.DataSource = Gallery.ParseGalleries(this.path);

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if ((string)comboBox1.Items[i] == galleryName)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            this.LoadGallery(this.path, galleryName, faceNumber);
        }

        private void LoadGallery(string path, string galleryName, int faceNumber = 0)
        {
            panelAvatars.Visible = false;

            while (panelAvatars.Controls.Count > 0)
            {
                panelAvatars.Controls[0].Dispose();
            }

            panelAvatars.Visible = true;

            Gallery gallery = new Gallery(path + galleryName + ".xgl");

            int count = gallery.GetAvatarCount();

            if (count == 0)
                return;

            if (faceNumber > 0)
            {
                this.selectedAvatar = faceNumber;
                imageAvatarCurrent.Image = gallery.GetAvatar(faceNumber);
                imageAvatarNew.Image = gallery.GetAvatar(faceNumber);
            }
            else
            {
                imageAvatarCurrent.Image = Properties.Resources.none;
            }

            this.gallery = galleryName;

            for (int i = 0; i < count; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Parent = panelAvatars;
                pictureBox.Width = 80;
                pictureBox.Height = 100;
                pictureBox.Left = 10;
                pictureBox.Top = 10;
                pictureBox.Name = (i + 1).ToString();

                pictureBox.Location = new Point(
                    (i % 10) * 80,
                    (i / 10) * 100
                );

                pictureBox.Image = gallery.GetAvatar(i + 1);
                pictureBox.Click += new EventHandler(ChangeAvatar);
            }
        }

        private void ChangeAvatar(object sender, EventArgs e)
        {
            PictureBox item = (PictureBox)sender;
            imageAvatarNew.Image = item.Image;
            this.selectedAvatar = (item.Name.Length > 0) ? Int32.Parse(item.Name) : 0;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.AvatarChanged?.Invoke(this.gallery, this.selectedAvatar);
        }

        private void imageAvatarCurrent_Click(object sender, EventArgs e)
        {
            this.selectedAvatar = this.defaultAvatar;
            imageAvatarNew.Image = imageAvatarCurrent.Image;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string gallery = comboBox1.Text;

            if (gallery == this.gallery)
                return;

            this.LoadGallery(this.path, gallery);
        }
    }
}
