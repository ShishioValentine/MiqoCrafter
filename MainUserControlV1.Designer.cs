namespace MiqoCraft
{
    partial class MainUserControlV1
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this._craftButton = new System.Windows.Forms.Button();
            this._searchButton = new System.Windows.Forms.Button();
            this._searchTextBox = new System.Windows.Forms.TextBox();
            this._searchLabel = new System.Windows.Forms.Label();
            this._shinyComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._statusPanel = new System.Windows.Forms.Panel();
            this._logSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this._quantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._logTextBox = new System.Windows.Forms.TextBox();
            this._miqoCrafterPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._logSplitContainer)).BeginInit();
            this._logSplitContainer.Panel1.SuspendLayout();
            this._logSplitContainer.Panel2.SuspendLayout();
            this._logSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._quantityNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._miqoCrafterPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _craftButton
            // 
            this._craftButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._craftButton.Location = new System.Drawing.Point(699, 67);
            this._craftButton.Name = "_craftButton";
            this._craftButton.Size = new System.Drawing.Size(126, 23);
            this._craftButton.TabIndex = 8;
            this._craftButton.Text = "Craft Shiny";
            this._craftButton.UseVisualStyleBackColor = true;
            this._craftButton.Click += new System.EventHandler(this._craftButton_Click);
            // 
            // _searchButton
            // 
            this._searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._searchButton.Location = new System.Drawing.Point(699, 41);
            this._searchButton.Name = "_searchButton";
            this._searchButton.Size = new System.Drawing.Size(126, 23);
            this._searchButton.TabIndex = 7;
            this._searchButton.Text = "Search Shiny";
            this._searchButton.UseVisualStyleBackColor = true;
            this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
            // 
            // _searchTextBox
            // 
            this._searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._searchTextBox.Location = new System.Drawing.Point(261, 43);
            this._searchTextBox.Name = "_searchTextBox";
            this._searchTextBox.Size = new System.Drawing.Size(432, 20);
            this._searchTextBox.TabIndex = 4;
            this._searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this._searchTextBox_KeyUp);
            // 
            // _searchLabel
            // 
            this._searchLabel.AutoSize = true;
            this._searchLabel.Location = new System.Drawing.Point(107, 46);
            this._searchLabel.Name = "_searchLabel";
            this._searchLabel.Size = new System.Drawing.Size(133, 13);
            this._searchLabel.TabIndex = 3;
            this._searchLabel.Text = "Search for shinies to craft :";
            // 
            // _shinyComboBox
            // 
            this._shinyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._shinyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._shinyComboBox.FormattingEnabled = true;
            this._shinyComboBox.Location = new System.Drawing.Point(261, 69);
            this._shinyComboBox.Name = "_shinyComboBox";
            this._shinyComboBox.Size = new System.Drawing.Size(235, 21);
            this._shinyComboBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select corresponding shiny :";
            // 
            // _statusPanel
            // 
            this._statusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._statusPanel.Location = new System.Drawing.Point(3, 101);
            this._statusPanel.Name = "_statusPanel";
            this._statusPanel.Size = new System.Drawing.Size(822, 316);
            this._statusPanel.TabIndex = 1;
            // 
            // _logSplitContainer
            // 
            this._logSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._logSplitContainer.Name = "_logSplitContainer";
            this._logSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _logSplitContainer.Panel1
            // 
            this._logSplitContainer.Panel1.Controls.Add(this.label4);
            this._logSplitContainer.Panel1.Controls.Add(this._quantityNumericUpDown);
            this._logSplitContainer.Panel1.Controls.Add(this.label3);
            this._logSplitContainer.Panel1.Controls.Add(this.label2);
            this._logSplitContainer.Panel1.Controls.Add(this._craftButton);
            this._logSplitContainer.Panel1.Controls.Add(this._miqoCrafterPictureBox);
            this._logSplitContainer.Panel1.Controls.Add(this._searchButton);
            this._logSplitContainer.Panel1.Controls.Add(this._searchTextBox);
            this._logSplitContainer.Panel1.Controls.Add(this._searchLabel);
            this._logSplitContainer.Panel1.Controls.Add(this._shinyComboBox);
            this._logSplitContainer.Panel1.Controls.Add(this.label1);
            this._logSplitContainer.Panel1.Controls.Add(this._statusPanel);
            // 
            // _logSplitContainer.Panel2
            // 
            this._logSplitContainer.Panel2.Controls.Add(this._logTextBox);
            this._logSplitContainer.Size = new System.Drawing.Size(828, 489);
            this._logSplitContainer.SplitterDistance = 420;
            this._logSplitContainer.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(778, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "V1.3.2";
            // 
            // _quantityNumericUpDown
            // 
            this._quantityNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._quantityNumericUpDown.Location = new System.Drawing.Point(573, 70);
            this._quantityNumericUpDown.Name = "_quantityNumericUpDown";
            this._quantityNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this._quantityNumericUpDown.TabIndex = 11;
            this._quantityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(511, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(108, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 26);
            this.label2.TabIndex = 9;
            this.label2.Text = "MiqoCrafter";
            // 
            // _logTextBox
            // 
            this._logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logTextBox.Location = new System.Drawing.Point(0, 0);
            this._logTextBox.Multiline = true;
            this._logTextBox.Name = "_logTextBox";
            this._logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._logTextBox.Size = new System.Drawing.Size(828, 65);
            this._logTextBox.TabIndex = 0;
            // 
            // _miqoCrafterPictureBox
            // 
            this._miqoCrafterPictureBox.BackColor = System.Drawing.SystemColors.Control;
            this._miqoCrafterPictureBox.BackgroundImage = global::MiqoCraft.Properties.Resources.MiqoCrafterSerious;
            this._miqoCrafterPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._miqoCrafterPictureBox.Location = new System.Drawing.Point(3, 3);
            this._miqoCrafterPictureBox.Name = "_miqoCrafterPictureBox";
            this._miqoCrafterPictureBox.Size = new System.Drawing.Size(102, 95);
            this._miqoCrafterPictureBox.TabIndex = 0;
            this._miqoCrafterPictureBox.TabStop = false;
            // 
            // MainUserControlV1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._logSplitContainer);
            this.Name = "MainUserControlV1";
            this.Size = new System.Drawing.Size(828, 489);
            this._logSplitContainer.Panel1.ResumeLayout(false);
            this._logSplitContainer.Panel1.PerformLayout();
            this._logSplitContainer.Panel2.ResumeLayout(false);
            this._logSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._logSplitContainer)).EndInit();
            this._logSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._quantityNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._miqoCrafterPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _craftButton;
        private System.Windows.Forms.PictureBox _miqoCrafterPictureBox;
        private System.Windows.Forms.Button _searchButton;
        private System.Windows.Forms.TextBox _searchTextBox;
        private System.Windows.Forms.Label _searchLabel;
        private System.Windows.Forms.ComboBox _shinyComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _statusPanel;
        private System.Windows.Forms.SplitContainer _logSplitContainer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown _quantityNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _logTextBox;
    }
}
