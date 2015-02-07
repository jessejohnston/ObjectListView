// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

using System;
using System.Windows.Forms;
using JesseJohnston;

namespace Demo
{
	public partial class MainForm : Form
	{
		private ObjectListView viewCompanies;
		private ObjectListView viewOrders;

		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			Database db = new Database();
			this.textBoxConnectionString.Text = db.ConnectionString;
		}

		private void textBoxFilter_TextChanged(object sender, EventArgs e)
		{
			this.viewCompanies.Filter = "Company=" + this.textBoxFilter.Text + "*";
		}
		private void comboBoxCustomers_SelectedValueChanged(object sender, EventArgs e)
		{
			if (this.comboBoxCustomers.SelectedValue == null)
				this.viewOrders.Filter = "CustomerId=null";
			else
				this.viewOrders.Filter = "CustomerId='" + this.comboBoxCustomers.SelectedValue.ToString() + "'";
		}
		private void buttonGetData_Click(object sender, EventArgs e)
		{
			Database db = new Database();
			db.ConnectionString = this.textBoxConnectionString.Text;

			viewCompanies = new ObjectListView(db.GetCustomers());
			viewOrders = new ObjectListView(db.GetOrders());

			this.comboBoxCustomers.DataSource = viewCompanies;
			this.comboBoxCustomers.DisplayMember = "Company";
			this.comboBoxCustomers.ValueMember = "Id";

			this.dataGridView.AutoGenerateColumns = false;
			this.dataGridView.DataSource = viewOrders;

			this.textBoxFilter.Text = "";
			this.textBoxFilter.Enabled = true;
		}
	}
}