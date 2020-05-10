namespace MiqoCraft
{
    partial class FFXIVCraftingListItem
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this._mainPictureBox = new System.Windows.Forms.PictureBox();
            this._itemNameLabel = new System.Windows.Forms.Label();
            this._gridStatusLabel = new System.Windows.Forms.Label();
            this._quantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._quantityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _mainPictureBox
            // 
            this._mainPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._mainPictureBox.Location = new System.Drawing.Point(3, 3);
            this._mainPictureBox.Name = "_mainPictureBox";
            this._mainPictureBox.Size = new System.Drawing.Size(64, 64);
            this._mainPictureBox.TabIndex = 0;
            this._mainPictureBox.TabStop = false;
            // 
            // _itemNameLabel
            // 
            this._itemNameLabel.AutoSize = true;
            this._itemNameLabel.Font = new System.Drawing.Font("Nirmala UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._itemNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(201)))), ((int)(((byte)(176)))));
            this._itemNameLabel.Location = new System.Drawing.Point(73, 12);
            this._itemNameLabel.Name = "_itemNameLabel";
            this._itemNameLabel.Size = new System.Drawing.Size(83, 19);
            this._itemNameLabel.TabIndex = 7;
            this._itemNameLabel.Text = "Item Name";
            // 
            // _gridStatusLabel
            // 
            this._gridStatusLabel.AutoSize = true;
            this._gridStatusLabel.Font = new System.Drawing.Font("Nirmala UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._gridStatusLabel.Location = new System.Drawing.Point(73, 33);
            this._gridStatusLabel.Name = "_gridStatusLabel";
            this._gridStatusLabel.Size = new System.Drawing.Size(70, 19);
            this._gridStatusLabel.TabIndex = 18;
            this._gridStatusLabel.Text = "Quantity :";
            // 
            // _quantityNumericUpDown
            // 
            this._quantityNumericUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this._quantityNumericUpDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(201)))), ((int)(((byte)(176)))));
            this._quantityNumericUpDown.Location = new System.Drawing.Point(149, 34);
            this._quantityNumericUpDown.Name = "_quantityNumericUpDown";
            this._quantityNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this._quantityNumericUpDown.TabIndex = 19;
            this._quantityNumericUpDown.ValueChanged += new System.EventHandler(this._quantityNumericUpDown_ValueChanged);
            // 
            // FFXIVCraftingListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.Controls.Add(this._gridStatusLabel);
            this.Controls.Add(this._quantityNumericUpDown);
            this.Controls.Add(this._itemNameLabel);
            this.Controls.Add(this._mainPictureBox);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(201)))), ((int)(((byte)(176)))));
            this.Name = "FFXIVCraftingListItem";
            this.Size = new System.Drawing.Size(229, 72);
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._quantityNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _mainPictureBox;
        private System.Windows.Forms.Label _itemNameLabel;
        private System.Windows.Forms.Label _gridStatusLabel;
        private System.Windows.Forms.NumericUpDown _quantityNumericUpDown;
    }
}
