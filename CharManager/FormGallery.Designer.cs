
namespace CharManager
{
    partial class FormGallery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGallery));
            this.panelAvatars = new System.Windows.Forms.Panel();
            this.imageAvatarNew = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.imageAvatarCurrent = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageAvatarNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageAvatarCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // panelAvatars
            // 
            this.panelAvatars.AutoScroll = true;
            this.panelAvatars.Location = new System.Drawing.Point(12, 12);
            this.panelAvatars.Name = "panelAvatars";
            this.panelAvatars.Size = new System.Drawing.Size(820, 300);
            this.panelAvatars.TabIndex = 0;
            // 
            // imageAvatarNew
            // 
            this.imageAvatarNew.Image = global::CharManager.Properties.Resources.none;
            this.imageAvatarNew.Location = new System.Drawing.Point(98, 318);
            this.imageAvatarNew.Name = "imageAvatarNew";
            this.imageAvatarNew.Size = new System.Drawing.Size(80, 100);
            this.imageAvatarNew.TabIndex = 1;
            this.imageAvatarNew.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(190, 320);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(375, 318);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(69, 23);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // imageAvatarCurrent
            // 
            this.imageAvatarCurrent.Location = new System.Drawing.Point(12, 318);
            this.imageAvatarCurrent.Name = "imageAvatarCurrent";
            this.imageAvatarCurrent.Size = new System.Drawing.Size(80, 100);
            this.imageAvatarCurrent.TabIndex = 5;
            this.imageAvatarCurrent.TabStop = false;
            this.imageAvatarCurrent.Click += new System.EventHandler(this.imageAvatarCurrent_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(450, 318);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 422);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.imageAvatarCurrent);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.imageAvatarNew);
            this.Controls.Add(this.panelAvatars);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OWCharManager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form3_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.imageAvatarNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageAvatarCurrent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAvatars;
        private System.Windows.Forms.PictureBox imageAvatarNew;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.PictureBox imageAvatarCurrent;
        private System.Windows.Forms.Button btnSave;
    }
}