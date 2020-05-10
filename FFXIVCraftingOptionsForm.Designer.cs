namespace MiqoCraft
{
    partial class FFXIVCraftingOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFXIVCraftingOptionsForm));
            this.label1 = new System.Windows.Forms.Label();
            this._craftTextBox = new System.Windows.Forms.TextBox();
            this._ignoreCheckBox = new System.Windows.Forms.CheckBox();
            this._titleLabel = new System.Windows.Forms.Label();
            this._itemPictureBox = new System.Windows.Forms.PictureBox();
            this._okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._itemPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Custom craft macro :";
            // 
            // _craftTextBox
            // 
            this._craftTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._craftTextBox.Location = new System.Drawing.Point(304, 37);
            this._craftTextBox.Name = "_craftTextBox";
            this._craftTextBox.Size = new System.Drawing.Size(240, 20);
            this._craftTextBox.TabIndex = 1;
            // 
            // _ignoreCheckBox
            // 
            this._ignoreCheckBox.AutoSize = true;
            this._ignoreCheckBox.Location = new System.Drawing.Point(150, 65);
            this._ignoreCheckBox.Name = "_ignoreCheckBox";
            this._ignoreCheckBox.Size = new System.Drawing.Size(79, 17);
            this._ignoreCheckBox.TabIndex = 2;
            this._ignoreCheckBox.Text = "Ignore Item";
            this._ignoreCheckBox.UseVisualStyleBackColor = true;
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._titleLabel.Location = new System.Drawing.Point(147, 13);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(78, 13);
            this._titleLabel.TabIndex = 3;
            this._titleLabel.Text = "Item Options";
            // 
            // _itemPictureBox
            // 
            this._itemPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._itemPictureBox.Location = new System.Drawing.Point(13, 13);
            this._itemPictureBox.Name = "_itemPictureBox";
            this._itemPictureBox.Size = new System.Drawing.Size(128, 128);
            this._itemPictureBox.TabIndex = 4;
            this._itemPictureBox.TabStop = false;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.Location = new System.Drawing.Point(433, 120);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(111, 23);
            this._okButton.TabIndex = 5;
            this._okButton.Text = "Save Options";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // FFXIVCraftingOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(556, 155);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._itemPictureBox);
            this.Controls.Add(this._titleLabel);
            this.Controls.Add(this._ignoreCheckBox);
            this.Controls.Add(this._craftTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FFXIVCraftingOptionsForm";
            this.Text = "Item Options";
            ((System.ComponentModel.ISupportInitialize)(this._itemPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _craftTextBox;
        private System.Windows.Forms.CheckBox _ignoreCheckBox;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.PictureBox _itemPictureBox;
        private System.Windows.Forms.Button _okButton;
    }
}