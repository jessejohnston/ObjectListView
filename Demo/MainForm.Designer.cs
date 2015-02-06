namespace Demo
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxCustomers = new System.Windows.Forms.ComboBox();
			this.labelMatches = new System.Windows.Forms.Label();
			this.textBoxFilter = new System.Windows.Forms.TextBox();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.columnOrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnEmployeeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnRequiredDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShippedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipVia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnFreight = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipPostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnShipCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBoxConnectionString = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonGetData = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 57);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Company:";
			// 
			// comboBoxCustomers
			// 
			this.comboBoxCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxCustomers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCustomers.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboBoxCustomers.FormattingEnabled = true;
			this.comboBoxCustomers.Location = new System.Drawing.Point(97, 54);
			this.comboBoxCustomers.Margin = new System.Windows.Forms.Padding(4);
			this.comboBoxCustomers.Name = "comboBoxCustomers";
			this.comboBoxCustomers.Size = new System.Drawing.Size(309, 24);
			this.comboBoxCustomers.TabIndex = 4;
			this.comboBoxCustomers.SelectedValueChanged += new System.EventHandler(this.comboBoxCustomers_SelectedValueChanged);
			// 
			// labelMatches
			// 
			this.labelMatches.AutoSize = true;
			this.labelMatches.Location = new System.Drawing.Point(975, 33);
			this.labelMatches.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelMatches.Name = "labelMatches";
			this.labelMatches.Size = new System.Drawing.Size(0, 16);
			this.labelMatches.TabIndex = 2;
			// 
			// textBoxFilter
			// 
			this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxFilter.Enabled = false;
			this.textBoxFilter.Location = new System.Drawing.Point(97, 22);
			this.textBoxFilter.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxFilter.Name = "textBoxFilter";
			this.textBoxFilter.Size = new System.Drawing.Size(161, 22);
			this.textBoxFilter.TabIndex = 2;
			this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
			// 
			// dataGridView
			// 
			this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnOrderId,
            this.columnEmployeeId,
            this.columnOrderDate,
            this.columnRequiredDate,
            this.columnShippedDate,
            this.columnShipVia,
            this.columnFreight,
            this.columnShipName,
            this.columnShipAddress,
            this.columnShipCity,
            this.columnShipRegion,
            this.columnShipPostalCode,
            this.columnShipCountry});
			this.dataGridView.Location = new System.Drawing.Point(13, 259);
			this.dataGridView.Margin = new System.Windows.Forms.Padding(4);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dataGridView.RowHeadersWidth = 25;
			this.dataGridView.Size = new System.Drawing.Size(669, 193);
			this.dataGridView.TabIndex = 5;
			// 
			// columnOrderId
			// 
			this.columnOrderId.DataPropertyName = "OrderId";
			this.columnOrderId.HeaderText = "Order ID";
			this.columnOrderId.Name = "columnOrderId";
			this.columnOrderId.Width = 83;
			// 
			// columnEmployeeId
			// 
			this.columnEmployeeId.DataPropertyName = "EmployeeId";
			this.columnEmployeeId.HeaderText = "Employee ID";
			this.columnEmployeeId.Name = "columnEmployeeId";
			this.columnEmployeeId.Width = 111;
			// 
			// columnOrderDate
			// 
			this.columnOrderDate.DataPropertyName = "OrderDate";
			this.columnOrderDate.HeaderText = "Ordered";
			this.columnOrderDate.Name = "columnOrderDate";
			this.columnOrderDate.Width = 83;
			// 
			// columnRequiredDate
			// 
			this.columnRequiredDate.DataPropertyName = "RequiredDate";
			this.columnRequiredDate.HeaderText = "Required";
			this.columnRequiredDate.Name = "columnRequiredDate";
			this.columnRequiredDate.Width = 89;
			// 
			// columnShippedDate
			// 
			this.columnShippedDate.DataPropertyName = "ShippedDate";
			this.columnShippedDate.HeaderText = "Shipped";
			this.columnShippedDate.Name = "columnShippedDate";
			this.columnShippedDate.Width = 84;
			// 
			// columnShipVia
			// 
			this.columnShipVia.DataPropertyName = "ShipVia";
			this.columnShipVia.HeaderText = "Ship Via";
			this.columnShipVia.Name = "columnShipVia";
			this.columnShipVia.Width = 83;
			// 
			// columnFreight
			// 
			this.columnFreight.HeaderText = "Freight";
			this.columnFreight.Name = "columnFreight";
			this.columnFreight.Width = 74;
			// 
			// columnShipName
			// 
			this.columnShipName.DataPropertyName = "ShipName";
			this.columnShipName.HeaderText = "Name";
			this.columnShipName.Name = "columnShipName";
			this.columnShipName.Width = 70;
			// 
			// columnShipAddress
			// 
			this.columnShipAddress.DataPropertyName = "ShipAddress";
			this.columnShipAddress.HeaderText = "Address";
			this.columnShipAddress.Name = "columnShipAddress";
			this.columnShipAddress.Width = 84;
			// 
			// columnShipCity
			// 
			this.columnShipCity.DataPropertyName = "ShipCity";
			this.columnShipCity.HeaderText = "City";
			this.columnShipCity.Name = "columnShipCity";
			this.columnShipCity.Width = 55;
			// 
			// columnShipRegion
			// 
			this.columnShipRegion.DataPropertyName = "ShipRegion";
			this.columnShipRegion.HeaderText = "State";
			this.columnShipRegion.Name = "columnShipRegion";
			this.columnShipRegion.Width = 64;
			// 
			// columnShipPostalCode
			// 
			this.columnShipPostalCode.DataPropertyName = "ShipPostalCode";
			this.columnShipPostalCode.HeaderText = "Zip Code";
			this.columnShipPostalCode.Name = "columnShipPostalCode";
			this.columnShipPostalCode.Width = 88;
			// 
			// columnShipCountry
			// 
			this.columnShipCountry.DataPropertyName = "ShipCountry";
			this.columnShipCountry.HeaderText = "Country";
			this.columnShipCountry.Name = "columnShipCountry";
			this.columnShipCountry.Width = 78;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(47, 25);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Filter:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBoxConnectionString);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.buttonGetData);
			this.groupBox1.Location = new System.Drawing.Point(13, 15);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(420, 108);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data Source";
			// 
			// textBoxConnectionString
			// 
			this.textBoxConnectionString.Location = new System.Drawing.Point(136, 29);
			this.textBoxConnectionString.Name = "textBoxConnectionString";
			this.textBoxConnectionString.Size = new System.Drawing.Size(277, 22);
			this.textBoxConnectionString.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(113, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Connection string:";
			// 
			// buttonGetData
			// 
			this.buttonGetData.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonGetData.Location = new System.Drawing.Point(20, 64);
			this.buttonGetData.Margin = new System.Windows.Forms.Padding(4);
			this.buttonGetData.Name = "buttonGetData";
			this.buttonGetData.Size = new System.Drawing.Size(100, 28);
			this.buttonGetData.TabIndex = 5;
			this.buttonGetData.Text = "Get Data";
			this.buttonGetData.UseVisualStyleBackColor = true;
			this.buttonGetData.Click += new System.EventHandler(this.buttonGetData_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textBoxFilter);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.comboBoxCustomers);
			this.groupBox2.Location = new System.Drawing.Point(13, 130);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(413, 96);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Company";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 239);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(52, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Orders:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(695, 465);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.labelMatches);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ObjectListView Demo";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxCustomers;
		private System.Windows.Forms.Label labelMatches;
		private System.Windows.Forms.TextBox textBoxFilter;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonGetData;
		private System.Windows.Forms.TextBox textBoxConnectionString;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderId;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnEmployeeId;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnOrderDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnRequiredDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShippedDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipVia;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnFreight;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipName;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipAddress;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipCity;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipRegion;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipPostalCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnShipCountry;
	}
}

