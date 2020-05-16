namespace MiqoGridFinder
{
    partial class MiqoGridFinder
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiqoGridFinder));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._gridListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._itemLabel = new System.Windows.Forms.Label();
            this._statusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._nodeListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._infoTextBox = new System.Windows.Forms.TextBox();
            this._closestLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._aetheryteListView = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._computedDescriptionLabel = new System.Windows.Forms.Label();
            this._validationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._progressLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this._displayGridButton = new System.Windows.Forms.Button();
            this._downloadFromURLButton = new System.Windows.Forms.Button();
            this._urlTextBox = new System.Windows.Forms.TextBox();
            this._analyzeButton = new System.Windows.Forms.Button();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this._gridListView);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 474);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid List";
            // 
            // _gridListView
            // 
            this._gridListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this._gridListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridListView.HideSelection = false;
            this._gridListView.Location = new System.Drawing.Point(3, 16);
            this._gridListView.Name = "_gridListView";
            this._gridListView.Size = new System.Drawing.Size(312, 455);
            this._gridListView.TabIndex = 0;
            this._gridListView.UseCompatibleStateImageBehavior = false;
            this._gridListView.View = System.Windows.Forms.View.Details;
            this._gridListView.SelectedIndexChanged += new System.EventHandler(this._gridListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item";
            this.columnHeader1.Width = 143;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 154;
            // 
            // _itemLabel
            // 
            this._itemLabel.AutoSize = true;
            this._itemLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._itemLabel.Location = new System.Drawing.Point(336, 28);
            this._itemLabel.Name = "_itemLabel";
            this._itemLabel.Size = new System.Drawing.Size(128, 24);
            this._itemLabel.TabIndex = 1;
            this._itemLabel.Text = "Item Name...";
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoSize = true;
            this._statusLabel.Location = new System.Drawing.Point(340, 56);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(37, 13);
            this._statusLabel.TabIndex = 2;
            this._statusLabel.Text = "Status";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._nodeListView);
            this.groupBox2.Location = new System.Drawing.Point(336, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(452, 143);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "List of gathering Nodes";
            // 
            // _nodeListView
            // 
            this._nodeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this._nodeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nodeListView.HideSelection = false;
            this._nodeListView.Location = new System.Drawing.Point(3, 16);
            this._nodeListView.Name = "_nodeListView";
            this._nodeListView.Size = new System.Drawing.Size(446, 124);
            this._nodeListView.TabIndex = 0;
            this._nodeListView.UseCompatibleStateImageBehavior = false;
            this._nodeListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Node";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Area";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Distance";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this._infoTextBox);
            this.groupBox3.Location = new System.Drawing.Point(336, 72);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(452, 90);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Grid Infos";
            // 
            // _infoTextBox
            // 
            this._infoTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._infoTextBox.Location = new System.Drawing.Point(3, 16);
            this._infoTextBox.Multiline = true;
            this._infoTextBox.Name = "_infoTextBox";
            this._infoTextBox.Size = new System.Drawing.Size(446, 71);
            this._infoTextBox.TabIndex = 0;
            // 
            // _closestLabel
            // 
            this._closestLabel.AutoSize = true;
            this._closestLabel.Location = new System.Drawing.Point(334, 315);
            this._closestLabel.Name = "_closestLabel";
            this._closestLabel.Size = new System.Drawing.Size(125, 13);
            this._closestLabel.TabIndex = 5;
            this._closestLabel.Text = "Closest Gathering Node :";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this._aetheryteListView);
            this.groupBox4.Location = new System.Drawing.Point(336, 331);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(452, 104);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "List of Aetherytes";
            // 
            // _aetheryteListView
            // 
            this._aetheryteListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this._aetheryteListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._aetheryteListView.HideSelection = false;
            this._aetheryteListView.Location = new System.Drawing.Point(3, 16);
            this._aetheryteListView.Name = "_aetheryteListView";
            this._aetheryteListView.Size = new System.Drawing.Size(446, 85);
            this._aetheryteListView.TabIndex = 0;
            this._aetheryteListView.UseCompatibleStateImageBehavior = false;
            this._aetheryteListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Aetheryte";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Area";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Distance";
            this.columnHeader8.Width = 110;
            // 
            // _computedDescriptionLabel
            // 
            this._computedDescriptionLabel.AutoSize = true;
            this._computedDescriptionLabel.Location = new System.Drawing.Point(334, 438);
            this._computedDescriptionLabel.Name = "_computedDescriptionLabel";
            this._computedDescriptionLabel.Size = new System.Drawing.Size(117, 13);
            this._computedDescriptionLabel.TabIndex = 6;
            this._computedDescriptionLabel.Text = "Computed Description :";
            // 
            // _validationLabel
            // 
            this._validationLabel.AutoSize = true;
            this._validationLabel.Location = new System.Drawing.Point(334, 455);
            this._validationLabel.Name = "_validationLabel";
            this._validationLabel.Size = new System.Drawing.Size(92, 13);
            this._validationLabel.TabIndex = 7;
            this._validationLabel.Text = "Validation Status :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(333, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Progress :";
            // 
            // _progressLabel
            // 
            this._progressLabel.AutoSize = true;
            this._progressLabel.Location = new System.Drawing.Point(393, 9);
            this._progressLabel.Name = "_progressLabel";
            this._progressLabel.Size = new System.Drawing.Size(10, 13);
            this._progressLabel.TabIndex = 9;
            this._progressLabel.Text = "-";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 508);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Download Grid List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _displayGridButton
            // 
            this._displayGridButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._displayGridButton.Location = new System.Drawing.Point(645, 476);
            this._displayGridButton.Name = "_displayGridButton";
            this._displayGridButton.Size = new System.Drawing.Size(140, 23);
            this._displayGridButton.TabIndex = 11;
            this._displayGridButton.Text = "Display Grid";
            this._displayGridButton.UseVisualStyleBackColor = true;
            this._displayGridButton.Click += new System.EventHandler(this._displayGridButton_Click);
            // 
            // _downloadFromURLButton
            // 
            this._downloadFromURLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._downloadFromURLButton.Location = new System.Drawing.Point(645, 508);
            this._downloadFromURLButton.Name = "_downloadFromURLButton";
            this._downloadFromURLButton.Size = new System.Drawing.Size(140, 23);
            this._downloadFromURLButton.TabIndex = 12;
            this._downloadFromURLButton.Text = "Download Grid From URL";
            this._downloadFromURLButton.UseVisualStyleBackColor = true;
            this._downloadFromURLButton.Click += new System.EventHandler(this._downloadFromURLButton_Click);
            // 
            // _urlTextBox
            // 
            this._urlTextBox.Location = new System.Drawing.Point(336, 510);
            this._urlTextBox.Name = "_urlTextBox";
            this._urlTextBox.Size = new System.Drawing.Size(303, 20);
            this._urlTextBox.TabIndex = 13;
            this._urlTextBox.Text = "https://miqobot.com/forum/forums/topic/shb-5-2-mats-scenario/";
            // 
            // _analyzeButton
            // 
            this._analyzeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._analyzeButton.Location = new System.Drawing.Point(177, 508);
            this._analyzeButton.Name = "_analyzeButton";
            this._analyzeButton.Size = new System.Drawing.Size(140, 23);
            this._analyzeButton.TabIndex = 14;
            this._analyzeButton.Text = "Analyze Grid List";
            this._analyzeButton.UseVisualStyleBackColor = true;
            this._analyzeButton.Click += new System.EventHandler(this._analyzeButton_Click);
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(12, 9);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(315, 13);
            this._progressBar.TabIndex = 15;
            // 
            // MiqoGridFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 543);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._analyzeButton);
            this.Controls.Add(this._urlTextBox);
            this.Controls.Add(this._downloadFromURLButton);
            this.Controls.Add(this._displayGridButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._progressLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._validationLabel);
            this.Controls.Add(this._computedDescriptionLabel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this._closestLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._statusLabel);
            this.Controls.Add(this._itemLabel);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MiqoGridFinder";
            this.Text = "MiqoGridFinder";
            this.Load += new System.EventHandler(this.MiqoGridFinder_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView _gridListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label _itemLabel;
        private System.Windows.Forms.Label _statusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView _nodeListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox _infoTextBox;
        private System.Windows.Forms.Label _closestLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView _aetheryteListView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Label _computedDescriptionLabel;
        private System.Windows.Forms.Label _validationLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _progressLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button _displayGridButton;
        private System.Windows.Forms.Button _downloadFromURLButton;
        private System.Windows.Forms.TextBox _urlTextBox;
        private System.Windows.Forms.Button _analyzeButton;
        private System.Windows.Forms.ProgressBar _progressBar;
    }
}

