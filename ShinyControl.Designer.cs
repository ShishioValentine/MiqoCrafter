namespace MiqoCraft
{
    partial class ShinyControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShinyControl));
            this._craftListGroupBox = new System.Windows.Forms.GroupBox();
            this._ingredientsListView = new System.Windows.Forms.ListView();
            this._nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._levelColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._quantityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._prerequisiteImageList = new System.Windows.Forms.ImageList(this.components);
            this._shinyLabel = new System.Windows.Forms.Label();
            this._infoLabel = new System.Windows.Forms.Label();
            this._generateButton = new System.Windows.Forms.Button();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this._ignoreShardCheckBox = new System.Windows.Forms.CheckBox();
            this._collectableCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._rotationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._craftingpresetTextBox = new System.Windows.Forms.TextBox();
            this._teleportTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._nghqTextBox = new System.Windows.Forms.TextBox();
            this._quantityPerNodeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._closeButton = new System.Windows.Forms.Button();
            this._miqoPathTextBox = new System.Windows.Forms.TextBox();
            this._openMiqoPresetButton = new System.Windows.Forms.Button();
            this._jobPictureBox = new System.Windows.Forms.PictureBox();
            this._shinyPictureBox = new System.Windows.Forms.PictureBox();
            this._optionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._craftListGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._quantityPerNodeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._jobPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._shinyPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _craftListGroupBox
            // 
            this._craftListGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._craftListGroupBox.Controls.Add(this._ingredientsListView);
            this._craftListGroupBox.Location = new System.Drawing.Point(3, 84);
            this._craftListGroupBox.Name = "_craftListGroupBox";
            this._craftListGroupBox.Size = new System.Drawing.Size(788, 482);
            this._craftListGroupBox.TabIndex = 0;
            this._craftListGroupBox.TabStop = false;
            this._craftListGroupBox.Text = "Craft Requirements";
            // 
            // _ingredientsListView
            // 
            this._ingredientsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._nameColumnHeader,
            this._levelColumnHeader,
            this._quantityColumnHeader,
            this._optionColumnHeader});
            this._ingredientsListView.Cursor = System.Windows.Forms.Cursors.Default;
            this._ingredientsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ingredientsListView.HideSelection = false;
            this._ingredientsListView.LargeImageList = this._prerequisiteImageList;
            this._ingredientsListView.Location = new System.Drawing.Point(3, 16);
            this._ingredientsListView.Name = "_ingredientsListView";
            this._ingredientsListView.Size = new System.Drawing.Size(782, 463);
            this._ingredientsListView.SmallImageList = this._prerequisiteImageList;
            this._ingredientsListView.TabIndex = 0;
            this._ingredientsListView.UseCompatibleStateImageBehavior = false;
            this._ingredientsListView.View = System.Windows.Forms.View.Details;
            this._ingredientsListView.ItemActivate += new System.EventHandler(this._ingredientsListView_ItemActivate);
            this._ingredientsListView.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            // 
            // _nameColumnHeader
            // 
            this._nameColumnHeader.Text = "Item Name";
            this._nameColumnHeader.Width = 279;
            // 
            // _levelColumnHeader
            // 
            this._levelColumnHeader.Text = "Level";
            this._levelColumnHeader.Width = 75;
            // 
            // _quantityColumnHeader
            // 
            this._quantityColumnHeader.Text = "Quantity";
            // 
            // _prerequisiteImageList
            // 
            this._prerequisiteImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._prerequisiteImageList.ImageSize = new System.Drawing.Size(32, 32);
            this._prerequisiteImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _shinyLabel
            // 
            this._shinyLabel.AutoSize = true;
            this._shinyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._shinyLabel.Location = new System.Drawing.Point(74, 7);
            this._shinyLabel.Name = "_shinyLabel";
            this._shinyLabel.Size = new System.Drawing.Size(137, 20);
            this._shinyLabel.TabIndex = 2;
            this._shinyLabel.Text = "Loading Shiny...";
            // 
            // _infoLabel
            // 
            this._infoLabel.AutoSize = true;
            this._infoLabel.Location = new System.Drawing.Point(77, 30);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new System.Drawing.Size(75, 13);
            this._infoLabel.TabIndex = 3;
            this._infoLabel.Text = "Loading Info...";
            // 
            // _generateButton
            // 
            this._generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._generateButton.Location = new System.Drawing.Point(794, 512);
            this._generateButton.Name = "_generateButton";
            this._generateButton.Size = new System.Drawing.Size(163, 23);
            this._generateButton.TabIndex = 6;
            this._generateButton.Text = "Generate Scenario";
            this._generateButton.UseVisualStyleBackColor = true;
            this._generateButton.Click += new System.EventHandler(this._generateButton_Click);
            // 
            // _toolTip
            // 
            this._toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this._toolTip_Popup);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(797, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Custom Craft Teleport :";
            this._toolTip.SetToolTip(this.label3, "If valuated, a teleport location will be added at the begining of the crafting se" +
        "ction of the generated scenario.");
            // 
            // _ignoreShardCheckBox
            // 
            this._ignoreShardCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ignoreShardCheckBox.AutoSize = true;
            this._ignoreShardCheckBox.Location = new System.Drawing.Point(795, 260);
            this._ignoreShardCheckBox.Name = "_ignoreShardCheckBox";
            this._ignoreShardCheckBox.Size = new System.Drawing.Size(101, 17);
            this._ignoreShardCheckBox.TabIndex = 16;
            this._ignoreShardCheckBox.Text = "Ignore Catalysts";
            this._toolTip.SetToolTip(this._ignoreShardCheckBox, "Ignore shards, crystals and clusters gathering.");
            this._ignoreShardCheckBox.UseVisualStyleBackColor = true;
            // 
            // _collectableCheckBox
            // 
            this._collectableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._collectableCheckBox.AutoSize = true;
            this._collectableCheckBox.Location = new System.Drawing.Point(794, 283);
            this._collectableCheckBox.Name = "_collectableCheckBox";
            this._collectableCheckBox.Size = new System.Drawing.Size(117, 17);
            this._collectableCheckBox.TabIndex = 16;
            this._collectableCheckBox.Text = "Craft as Collectable";
            this._toolTip.SetToolTip(this._collectableCheckBox, "Last item will be crafted as collectable.");
            this._collectableCheckBox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(791, 308);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Nb item per node :";
            this._toolTip.SetToolTip(this.label5, "Nb of items you expect to gather per node.");
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(797, 338);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Miqobot Installation Path :";
            this._toolTip.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // _rotationTextBox
            // 
            this._rotationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._rotationTextBox.Location = new System.Drawing.Point(794, 116);
            this._rotationTextBox.Name = "_rotationTextBox";
            this._rotationTextBox.Size = new System.Drawing.Size(163, 20);
            this._rotationTextBox.TabIndex = 8;
            this._rotationTextBox.Text = "HQ +10%";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(797, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Gathering Rotation :";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(797, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Crafting Preset :";
            // 
            // _craftingpresetTextBox
            // 
            this._craftingpresetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._craftingpresetTextBox.Location = new System.Drawing.Point(794, 155);
            this._craftingpresetTextBox.Name = "_craftingpresetTextBox";
            this._craftingpresetTextBox.Size = new System.Drawing.Size(163, 20);
            this._craftingpresetTextBox.TabIndex = 10;
            this._craftingpresetTextBox.Text = "recommended";
            // 
            // _teleportTextBox
            // 
            this._teleportTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._teleportTextBox.Location = new System.Drawing.Point(794, 233);
            this._teleportTextBox.Name = "_teleportTextBox";
            this._teleportTextBox.Size = new System.Drawing.Size(163, 20);
            this._teleportTextBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(797, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Crafting NQHQ :";
            // 
            // _nghqTextBox
            // 
            this._nghqTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._nghqTextBox.Location = new System.Drawing.Point(794, 194);
            this._nghqTextBox.Name = "_nghqTextBox";
            this._nghqTextBox.Size = new System.Drawing.Size(163, 20);
            this._nghqTextBox.TabIndex = 14;
            this._nghqTextBox.Text = "balanced";
            // 
            // _quantityPerNodeNumericUpDown
            // 
            this._quantityPerNodeNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._quantityPerNodeNumericUpDown.Location = new System.Drawing.Point(891, 306);
            this._quantityPerNodeNumericUpDown.Name = "_quantityPerNodeNumericUpDown";
            this._quantityPerNodeNumericUpDown.Size = new System.Drawing.Size(66, 20);
            this._quantityPerNodeNumericUpDown.TabIndex = 18;
            this._quantityPerNodeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(794, 540);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(163, 23);
            this._closeButton.TabIndex = 19;
            this._closeButton.Text = "Close Item Tab";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _miqoPathTextBox
            // 
            this._miqoPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._miqoPathTextBox.Location = new System.Drawing.Point(794, 355);
            this._miqoPathTextBox.Name = "_miqoPathTextBox";
            this._miqoPathTextBox.Size = new System.Drawing.Size(139, 20);
            this._miqoPathTextBox.TabIndex = 20;
            // 
            // _openMiqoPresetButton
            // 
            this._openMiqoPresetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._openMiqoPresetButton.BackgroundImage = global::MiqoCraft.Properties.Resources.folder_open_document_text;
            this._openMiqoPresetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._openMiqoPresetButton.Location = new System.Drawing.Point(935, 354);
            this._openMiqoPresetButton.Name = "_openMiqoPresetButton";
            this._openMiqoPresetButton.Size = new System.Drawing.Size(22, 22);
            this._openMiqoPresetButton.TabIndex = 22;
            this._openMiqoPresetButton.UseVisualStyleBackColor = true;
            this._openMiqoPresetButton.Click += new System.EventHandler(this._openMiqoPresetButton_Click);
            // 
            // _jobPictureBox
            // 
            this._jobPictureBox.Location = new System.Drawing.Point(74, 49);
            this._jobPictureBox.Name = "_jobPictureBox";
            this._jobPictureBox.Size = new System.Drawing.Size(22, 22);
            this._jobPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._jobPictureBox.TabIndex = 7;
            this._jobPictureBox.TabStop = false;
            // 
            // _shinyPictureBox
            // 
            this._shinyPictureBox.Location = new System.Drawing.Point(4, 7);
            this._shinyPictureBox.Name = "_shinyPictureBox";
            this._shinyPictureBox.Size = new System.Drawing.Size(64, 64);
            this._shinyPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._shinyPictureBox.TabIndex = 1;
            this._shinyPictureBox.TabStop = false;
            // 
            // _optionColumnHeader
            // 
            this._optionColumnHeader.Text = "Options";
            this._optionColumnHeader.Width = 1029;
            // 
            // ShinyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._openMiqoPresetButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._miqoPathTextBox);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._quantityPerNodeNumericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._collectableCheckBox);
            this.Controls.Add(this._ignoreShardCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._nghqTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._teleportTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._rotationTextBox);
            this.Controls.Add(this._craftingpresetTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._jobPictureBox);
            this.Controls.Add(this._generateButton);
            this.Controls.Add(this._infoLabel);
            this.Controls.Add(this._shinyLabel);
            this.Controls.Add(this._shinyPictureBox);
            this.Controls.Add(this._craftListGroupBox);
            this.Name = "ShinyControl";
            this.Size = new System.Drawing.Size(963, 569);
            this._craftListGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._quantityPerNodeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._jobPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._shinyPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox _craftListGroupBox;
        private System.Windows.Forms.PictureBox _shinyPictureBox;
        private System.Windows.Forms.Label _shinyLabel;
        private System.Windows.Forms.Label _infoLabel;
        private System.Windows.Forms.ListView _ingredientsListView;
        private System.Windows.Forms.ColumnHeader _nameColumnHeader;
        private System.Windows.Forms.ColumnHeader _levelColumnHeader;
        private System.Windows.Forms.ColumnHeader _quantityColumnHeader;
        private System.Windows.Forms.ImageList _prerequisiteImageList;
        private System.Windows.Forms.Button _generateButton;
        private System.Windows.Forms.PictureBox _jobPictureBox;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.ListViewGroup _gatheredListViewGroup;
        private System.Windows.Forms.ListViewGroup _craftedListViewGroup;
        private System.Windows.Forms.ListViewGroup _otherListViewGroup;
        private System.Windows.Forms.ListViewGroup _npcListViewGroup;
        private System.Windows.Forms.ListViewGroup _reducedListViewGroup;
        private System.Windows.Forms.TextBox _rotationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _craftingpresetTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _teleportTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _nghqTextBox;
        private System.Windows.Forms.CheckBox _ignoreShardCheckBox;
        private System.Windows.Forms.CheckBox _collectableCheckBox;
        private System.Windows.Forms.NumericUpDown _quantityPerNodeNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _miqoPathTextBox;
        private System.Windows.Forms.Button _openMiqoPresetButton;
        private System.Windows.Forms.ColumnHeader _optionColumnHeader;
    }
}
