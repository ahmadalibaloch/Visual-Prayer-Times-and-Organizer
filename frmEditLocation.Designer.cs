namespace PrayerTiming
{
    partial class frmEditLocation
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBoxPlace = new System.Windows.Forms.TextBox();
            this.dataGridLocations = new System.Windows.Forms.DataGridView();
            this.clmCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbEnterCityName = new System.Windows.Forms.Label();
            this.btnLocationOk = new System.Windows.Forms.Button();
            this.btnLocationCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(300, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // textBoxPlace
            // 
            this.textBoxPlace.Location = new System.Drawing.Point(71, 9);
            this.textBoxPlace.Name = "textBoxPlace";
            this.textBoxPlace.Size = new System.Drawing.Size(211, 20);
            this.textBoxPlace.TabIndex = 12;
            // 
            // dataGridLocations
            // 
            this.dataGridLocations.AllowUserToAddRows = false;
            this.dataGridLocations.AllowUserToDeleteRows = false;
            this.dataGridLocations.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLocations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCity,
            this.clmLat,
            this.clmLng,
            this.Zone});
            this.dataGridLocations.GridColor = System.Drawing.Color.LightSteelBlue;
            this.dataGridLocations.Location = new System.Drawing.Point(12, 35);
            this.dataGridLocations.Name = "dataGridLocations";
            this.dataGridLocations.ReadOnly = true;
            this.dataGridLocations.RowHeadersVisible = false;
            this.dataGridLocations.ShowEditingIcon = false;
            this.dataGridLocations.Size = new System.Drawing.Size(420, 150);
            this.dataGridLocations.TabIndex = 13;
            this.dataGridLocations.TabStop = false;
            // 
            // clmCity
            // 
            this.clmCity.DataPropertyName = "Name";
            this.clmCity.HeaderText = "City Name";
            this.clmCity.Name = "clmCity";
            this.clmCity.ReadOnly = true;
            // 
            // clmLat
            // 
            this.clmLat.DataPropertyName = "Latitude";
            this.clmLat.HeaderText = "Latitiude";
            this.clmLat.Name = "clmLat";
            this.clmLat.ReadOnly = true;
            // 
            // clmLng
            // 
            this.clmLng.DataPropertyName = "Longitude";
            this.clmLng.HeaderText = "Langitude";
            this.clmLng.Name = "clmLng";
            this.clmLng.ReadOnly = true;
            // 
            // Zone
            // 
            this.Zone.DataPropertyName = "Zone";
            this.Zone.HeaderText = "Time Zone";
            this.Zone.Name = "Zone";
            this.Zone.ReadOnly = true;
            // 
            // lbEnterCityName
            // 
            this.lbEnterCityName.AutoSize = true;
            this.lbEnterCityName.Location = new System.Drawing.Point(9, 9);
            this.lbEnterCityName.Name = "lbEnterCityName";
            this.lbEnterCityName.Size = new System.Drawing.Size(59, 13);
            this.lbEnterCityName.TabIndex = 14;
            this.lbEnterCityName.Text = "Enter City:";
            // 
            // btnLocationOk
            // 
            this.btnLocationOk.Enabled = false;
            this.btnLocationOk.Location = new System.Drawing.Point(336, 203);
            this.btnLocationOk.Name = "btnLocationOk";
            this.btnLocationOk.Size = new System.Drawing.Size(75, 23);
            this.btnLocationOk.TabIndex = 15;
            this.btnLocationOk.Text = "OK";
            this.btnLocationOk.UseVisualStyleBackColor = true;
            this.btnLocationOk.Click += new System.EventHandler(this.btnLocationOk_Click);
            // 
            // btnLocationCancel
            // 
            this.btnLocationCancel.Location = new System.Drawing.Point(255, 203);
            this.btnLocationCancel.Name = "btnLocationCancel";
            this.btnLocationCancel.Size = new System.Drawing.Size(75, 23);
            this.btnLocationCancel.TabIndex = 16;
            this.btnLocationCancel.Text = "Cencel";
            this.btnLocationCancel.UseVisualStyleBackColor = true;
            this.btnLocationCancel.Click += new System.EventHandler(this.btnLocationCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(9, 208);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 13);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Status";
            // 
            // frmEditLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(447, 238);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnLocationCancel);
            this.Controls.Add(this.btnLocationOk);
            this.Controls.Add(this.lbEnterCityName);
            this.Controls.Add(this.dataGridLocations);
            this.Controls.Add(this.textBoxPlace);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmEditLocation";
            this.Opacity = 0.9;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Location";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmEditLocation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLocations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textBoxPlace;
        private System.Windows.Forms.DataGridView dataGridLocations;
        private System.Windows.Forms.Label lbEnterCityName;
        private System.Windows.Forms.Button btnLocationOk;
        private System.Windows.Forms.Button btnLocationCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLat;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLng;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zone;
        private System.Windows.Forms.Label lblStatus;
    }
}