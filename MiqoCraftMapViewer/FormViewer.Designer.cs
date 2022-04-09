namespace MiqoCraftMapViewer
{
    partial class FormViewer
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._aetheryteComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(582, 604);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // _aetheryteComboBox
            // 
            this._aetheryteComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._aetheryteComboBox.FormattingEnabled = true;
            this._aetheryteComboBox.Location = new System.Drawing.Point(12, 622);
            this._aetheryteComboBox.Name = "_aetheryteComboBox";
            this._aetheryteComboBox.Size = new System.Drawing.Size(582, 21);
            this._aetheryteComboBox.TabIndex = 1;
            this._aetheryteComboBox.SelectedIndexChanged += new System.EventHandler(this._aetheryteComboBox_SelectedIndexChanged);
            // 
            // FormViewer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 655);
            this.Controls.Add(this._aetheryteComboBox);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormViewer";
            this.Text = "Map Viewer - Drag&Drop map file";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormViewer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormViewer_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox _aetheryteComboBox;
    }
}

