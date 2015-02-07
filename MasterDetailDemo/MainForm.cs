// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using JesseJohnston;

namespace MasterDetailDemo
{
	public partial class MainForm : Form
	{
		private enum ProgramSortType
		{
			Rank,
			Alpha,
			Value
		}
		[Flags]
		private enum ProgramFilter
		{
			Unfiltered = 0,
			None = 1,
			Platinum = 2,
			Gold = 4,
			Silver = 8
		}
		private BindingList<Customer> customers;
		private Customer current;
		private ObjectListView<Customer> view;
		private ProgramSortType programSort = ProgramSortType.Value;	// the default IComparable implementation.
		private ProgramFilter programFilter = ProgramFilter.Unfiltered;

		public MainForm()
		{
			InitializeComponent();

			customers = new BindingList<Customer>();

			view = new ObjectListView<Customer>(customers);

			view.Sorted += new EventHandler(view_Sorted);
			this.bindingSource.DataSource = view;

			this.comboBoxProgram.DataSource = new CustomerProgram[] { CustomerProgram.None, CustomerProgram.Silver, CustomerProgram.Gold, CustomerProgram.Platinum };
		}

		// Remember the current item as the current grid row changes, so that if the grid is sorted, we can restore
		// the current item.
		private void bindingSource_PositionChanged(object sender, EventArgs e)
		{
			if (this.bindingSource.Position < 0)
				this.current = null;
			else
				this.current = this.bindingSource.Current as Customer;
		}

		// When a column header is clicked, update the sort on the view, and manually set the sort glyphs in the DataGridView columns.
		// This is required to support multi-column sorting in the DataGridView.  To support single-column sorting, remove this event
		// handler and set the sort mode to automatic for each DataGridView column.
		private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			bool shiftPressed = (Control.ModifierKeys == Keys.Shift);
			bool ctrlPressed = (Control.ModifierKeys == Keys.Control);

			string sortPropName = this.dataGridView.Columns[e.ColumnIndex].DataPropertyName;
			PropertyDescriptor sortProp = TypeDescriptor.GetProperties(typeof(Customer)).Find(sortPropName, false);

			List<ListSortDescription> newSorts = new List<ListSortDescription>();

			bool clickedColumnInExistingSort = ctrlPressed;
			foreach (ListSortDescription desc in this.view.SortDescriptions)
			{
				if (desc.PropertyDescriptor.Name == sortPropName)
				{
					if (!ctrlPressed)
					{
						ListSortDirection dir = (desc.SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);
						newSorts.Add(new ListSortDescription(sortProp, dir));
					}
					clickedColumnInExistingSort = true;
				}
				else if (shiftPressed || ctrlPressed)
					newSorts.Add(desc);
			}

			if (!clickedColumnInExistingSort)
				newSorts.Add(new ListSortDescription(sortProp, ListSortDirection.Ascending));

			this.view.ApplySort(new ListSortDescriptionCollection(newSorts.ToArray()));

			foreach (ListSortDescription desc in this.view.SortDescriptions)
			{
				foreach (DataGridViewColumn column in this.dataGridView.Columns)
				{
					if (column.DataPropertyName == desc.PropertyDescriptor.Name)
					{
						column.HeaderCell.SortGlyphDirection = (desc.SortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending);
						break;
					}
				}
			}
		}

		// After sorting, move the current grid row to the list item that was current before the sort.
		// If the sort mode on the DataGridView columns was automatic, we could handle the DataGridView.Sorted event instead of the view
		// event.  Because the sort mode is programmatic, the DataGridView.Sorted event is not raised.
		private void view_Sorted(object sender, EventArgs e)
		{
			if (this.current != null)
				this.bindingSource.Position = this.bindingSource.IndexOf(this.current);
		}

		// Add a new customer to the list, and move the current grid row to the new item.
		private void buttonNew_Click(object sender, EventArgs e)
		{
			ObjectListView<Customer> view = (ObjectListView<Customer>)this.bindingSource.DataSource;
			view.Add(new Customer());
			this.bindingSource.Position = view.Count;
		}

		// Show the debugger visualizer.
		private void buttonShowVisualizer_Click(object sender, EventArgs e)
		{
			ObjectListViewVisualizer.TestShowVisualizer(this.view);
		}

		// Save the customers to a file.
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Stream strm = null;

			try
			{
				strm = File.OpenWrite("customers.dat");
				BinaryFormatter fmtr = new BinaryFormatter();
				fmtr.Serialize(strm, this.customers);
			}
			catch (IOException)
			{
			}
			finally
			{
				if (strm != null)
					strm.Close();
			}
		}

		// Load the customers from a file.
		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Stream strm = null;

			try
			{
				strm = File.OpenRead("customers.dat");
				BinaryFormatter fmtr = new BinaryFormatter();
				BindingList<Customer> newCustomers = (BindingList<Customer>)fmtr.Deserialize(strm);

				this.view.BeginUpdate();

				this.customers.Clear();
				foreach (Customer c in newCustomers)
					this.customers.Add(c);

				this.view.EndUpdate();
			}
			catch (IOException)
			{
			}
			finally
			{
				if (strm != null)
					strm.Close();
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// Switch sorting methods on Program column.
		private void programSortToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			this.alphabeticallyToolStripMenuItem.Checked = false;
			this.byRankToolStripMenuItem.Checked = false;
			this.byValueToolStripMenuItem.Checked = false;

			switch (this.programSort)
			{
				case ProgramSortType.Alpha:
					this.alphabeticallyToolStripMenuItem.Checked = true;
					break;
				case ProgramSortType.Rank:
					this.byRankToolStripMenuItem.Checked = true;
					break;
				case ProgramSortType.Value:
					this.byValueToolStripMenuItem.Checked = true;
					break;
			}
		}
		private void byRankToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programSort = ProgramSortType.Rank;
			this.view.PropertyComparers["Program"] = new CustomerProgramComparerRank();
		}
		private void alphabeticallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programSort = ProgramSortType.Alpha;
			this.view.PropertyComparers["Program"] = new CustomerProgramComparerAlpha();
		}
		private void byValueToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programSort = ProgramSortType.Alpha;
			this.view.PropertyComparers.Remove("Program");
		}

		private void useNewRowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.dataGridView.AllowUserToAddRows = this.useNewRowToolStripMenuItem.Checked;
			this.buttonNew.Enabled = !this.useNewRowToolStripMenuItem.Checked;
		}

		// Select filter on Program column.
		private void programFilterToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			this.filterAllToolStripMenuItem.Checked = (this.programFilter == ProgramFilter.Unfiltered);
			if (this.filterAllToolStripMenuItem.Checked)
			{
				this.filterNoneToolStripMenuItem.Checked = false;
				this.filterSilverToolStripMenuItem.Checked = false;
				this.filterGoldToolStripMenuItem.Checked = false;
				this.filterPlatinumToolStripMenuItem.Checked = false;
			}
			else
			{
				this.filterNoneToolStripMenuItem.Checked = (this.programFilter & ProgramFilter.None) != 0;
				this.filterSilverToolStripMenuItem.Checked = (this.programFilter & ProgramFilter.Silver) != 0;
				this.filterGoldToolStripMenuItem.Checked = (this.programFilter & ProgramFilter.Gold) != 0;
				this.filterPlatinumToolStripMenuItem.Checked = (this.programFilter & ProgramFilter.Platinum) != 0;
			}
		}
		private void filterAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programFilter = ProgramFilter.Unfiltered;
			this.view.RemoveFilter();
		}
		private void filterNoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.filterNoneToolStripMenuItem.Checked)
				this.programFilter ^= ProgramFilter.None;
			else
				this.programFilter |= ProgramFilter.None;
			SetFilter(this.programFilter);
		}
		private void filterSilverToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.filterSilverToolStripMenuItem.Checked)
				this.programFilter ^= ProgramFilter.Silver;
			else
				this.programFilter |= ProgramFilter.Silver;
			SetFilter(this.programFilter);
		}
		private void filterGoldToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.filterGoldToolStripMenuItem.Checked)
				this.programFilter ^= ProgramFilter.Gold;
			else
				this.programFilter |= ProgramFilter.Gold;
			SetFilter(this.programFilter);
		}
		private void filterPlatinumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.filterPlatinumToolStripMenuItem.Checked)
				this.programFilter ^= ProgramFilter.Platinum;
			else
				this.programFilter |= ProgramFilter.Platinum;
			SetFilter(this.programFilter);
		}

		private void SetFilter(ProgramFilter filter)
		{
			this.view.FilterPredicate = delegate(Customer listItem)
			{
				switch (listItem.Program)
				{
					case CustomerProgram.Gold:
						return (filter & ProgramFilter.Gold) != 0;
					case CustomerProgram.None:
						return (filter & ProgramFilter.None) != 0;
					case CustomerProgram.Platinum:
						return (filter & ProgramFilter.Platinum) != 0;
					case CustomerProgram.Silver:
						return (filter & ProgramFilter.Silver) != 0;
					default:
						return false;
				}
			};
		}
	}
}