# ObjectListView

## .NET DataView for objects


A DataView-like sorting and filtering view of a collection of arbitrary objects.  This is intended primarily for use in .NET WinForms applications with DataGridViews.It allows you to bind a list of your own classes (e.g. List\<Customer>) to a DataGridView and get a sorted, filtered view of your list without having to use an intermediary DataTable.ObjectListView is a full implementation of IBindingListView.  See the demo code and tests for examples of how to use it.  In general, just create yourlist, create an ObjectListView for that list, and use the ObjectListView as the DataSource for your Windows Formscontrol.  You'll need to reference ObjectListView.dll in your project.  The full class name is JesseJohnston.ObjectListView.Two versions are provided:
Class | Usage
----- | -----  ObjectListView | Works with any list item type.  List items are always referenced as type **object**.ObjectListView\<T> | Works with any list item type.  List items are always referenced as type T.The functionality of the two versions is identical.  The generic ObjectListView<T> offers slightly better performance and convenience(not having to cast back and forth from object to list item type).**Note:** ObjectListView/ObjectListView\<T> work with any list type that implements IList, and any list item type.  However, the best list type to use is BindingList\<T>, and your list item types should implement INotifyPropertyChanged.  If you don't use BindingList\<T> and/or your list items don't support INotifyPropertyChanged, ObjectListView won't update properly as list items change or when list items are added or removed from the underlying list.  If your underlying list and list items are unchanging, it doesn't matter what kind of list or list item types you have; ObjectListView will work just fine in that case.## UsageHere's an abridged example from the Demo.exe project.  Database.GetCustomers() returns a List\<Customer> and Database.GetOrders() returns a List\<Order>.	public partial class MainForm : Form
	{
		private ObjectListView viewCompanies;
		private ObjectListView viewOrders;

		private void buttonGetData_Click(object sender, EventArgs e)
		{
			Database db = new Database();
			db.ConnectionString = this.textBoxConnectionString.Text;

			this.viewCompanies = new ObjectListView(db.GetCustomers());
			this.viewOrders = new ObjectListView(db.GetOrders());

			this.comboBoxCustomers.DataSource = viewCompanies;
			this.comboBoxCustomers.DisplayMember = "Company";
			this.comboBoxCustomers.ValueMember = "Id";

			this.dataGridView.AutoGenerateColumns = false;
			this.dataGridView.DataSource = viewOrders;

			this.textBoxFilter.Text = "";
			this.textBoxFilter.Enabled = true;
		}
	}
## LicenseObjectListView is free.  You are granted a perpetual, non-exclusive, royalty-free license to use the code in any commercial and non-commercialsoftware development works.  No attribution is required.  Jesse Johnston retains the copyright to ObjectListView and reserves all rights not explicitly granted in this license.## Building the sourceTests are included in the same project as ObjectListView, for ease of testing internal methods.  However, the test code is automatically compiled out of the release build.  The build configurations containing the word "test" define the TEST pragma which includes all of the unit testing code.## DemosTwo example demos are provided.**Demo.exe** shows customer and order data from the Northwind database, illustrating data binding of ObjectListView to a ComboBox and a DataGridView.**MasterDetailDemo.exe** is a simple but real-world data entry form showing a master-detail view of customer data, with currency management through a BindingNavigator, editing in both the DataGridView and details controls, and multi-column sorting in the DataGridView.  This demo also illustrates saving and loading data and the use of BeginUpdate()/EndUpdate() for better view performance.### To run the demosMasterDetailDemo.exe requires no database or special configuration.  The following steps are for Demo.exe only, which requires the Northwind database.1. SQL Server 2000/Express (or newer) must be installed.2. Install the Microsoft sample Northwind database, which can be downloaded from:	[http://www.microsoft.com/downloads/details.aspx?FamilyID=06616212-0356-46A0-8DA2-EEBC53A68034&displaylang=en][2]3. Edit the connection string in Demo.exe.config to reference your computer name in the data source (instead of "DADBOX").Note that SQL Server is not required for general use of ObjectListView.  This demo reads data from the Northwind database, and that's the only reason why SQL Server is involved.### Demo.exeThe form shows a ComboBox and a DataGridView, each bound to a separate ObjectListView.  The two ObjectListViews share a common list of Customer objects.Select a list type and a list item type to observe the behavior of the ObjectListView with those types.List item type | Description
-------------- | -----------Customer | A simple list item with no property change notification.CustomerNotifying | A list item that implements INotifyPropertyChanged.List type | Description
--------- | -----------ArrayList | An ArrayList of the selected list item type (Customer or CustomerNotifying)List\<T> | A generic list of the selected list item type (behaves identically to ArrayList).BindingList\<T> | A generic BindingList of the selected list item type.  BindingList provides a ListChanged event.Using CustomerNotifying and BindingList\<T> produces optimal results: both list changes (add/delete) and list itemchanges (changing column values) are reflected correctly in the DataGridView and the ComboBox.### MasterDetailDemo.exeThe form shows TextBoxes in the top half of the form, for editing an individual list item.  The New button adds a row (a new Customer object) to the DataGridView in the bottom half of the form.  Click on a grid row to change the current list item, or use the navigation controls in the BindingNavigator at the bottom of the form.  The data bindings for the TextBoxes cause the properties of the currentgrid row to be reflected in the TextBoxes.  Edits in the grid rows are reflected in the TextBoxes and vice versa.Click on the grid column headers to change the sort.  Clicking on a column that is already sorted will reverse the order of the sort. Holding down the shift key while clicking adds an additional column to the sort.  Holding down the control key while clicking removes the column from the sort.As the sort changes, the current grid row position is changed to keep the previously selected item current.The main menu options for "Program Sort" allow you to select different custom sorts for the Program column.  Changing one of these options changesthe IComparer used for the Customer.Program property by updating the entry in the PropertyComparers collection of IComparers.
The main menu option "Use New Row" allows you to switch between the new row in the DataGridView and the New button to add new rows to the grid.The main menu options for "Program Filter" allow you to specify a custom filter for the Program column.  Check any combination of Program values to includethem in the filter.  Uncheck values to remove them.  Check "Unfiltered" to remove the filter and display all items in the list.Use the File/Load and Save menu options to load/save the data displayed in the grid.  Some sample data is provided in the file customers.dat.
## Change Log


### Version 1.0.0.13, 2/7/15
1. Upgraded solution to Visual Studio 2013.  The project still targets .NET 2.0 and above.
2. Removed dependency on NUnit.  The project now uses the Visual Studio test framework.
3. Merged readme and change list into a single markdown document.
4. Added sample data file to MasterDetailsDemo project.

### Version 1.0.0.12, 5/28/07
1. Added option in BeginUpdate() to defer sorting and filtering until call to EndUpdate().
2. Fixed bug where ListChanged events were raised for list items not in the view.
3. Added Sort property that takes a string parameter containing sort property names and directions (ala DataView.Sort).
4. Added support for property paths in sorting.

### Version 1.0.0.11, 5/6/07
1. Added Select() methods to return list items that match some criteria.
2. Added Find() convenience methods and made IBindingListView.Find() an explicit interface implementation.
3. Added property path support to refer to properties of list item properties in Filter, Find() and Select().
4. Fixed bug when deleting items from a sorted view (ListChanged+Reset and Sorted events are no longer raised).
5. Fixed bug with trailing whitespace in Filter expression.
6. Added RemovingItem event.  This is raised just prior a list item removal, but only when the removal is done through
   a method of ObjectListView.
7. Replaced ListItemFilter<T> usage with equivalent Predicate<T> (and non-generic Predicate) for better framework consistency.
8. Added documentation XML file for Intellisense.

### Version 1.0.0.10, 4/1/07
1. Added documentation file.
2. Made the following properties explicit interface implementations:
   IBindingList.SupportsChangeNotification
   IBindingList.SupportsSearching
   IBindingList.SupportsSorting
   IBindingListView.SupportsAdvancedSorting
   IBindingListView.SupportsFiltering
3. Changed the accessibility of the following classes to internal:
   AnalysisForm
   SortDescription
   SortDescriptionCollection
   VisualizerForm
   VisualizerTForm
4. IList.Remove() no longer throws an exception if the item is not present in the list.
5. Fixed Filter expression bug: quotes not parsed correctly (e.g. Filter = "FirstName = 'Smi*' AND LastName = 'J*'").


### Version 1.0.0.9, 3/11/07
1. Don't invalidate enumerators when a list item property is changed, unless the property is in the view's sort or filter criteria.
2. Exposed OnListChanged(), OnSorted(), and OnAddingNew() as protected virtual for extensibility.
3. Added ToArray() method.
4. Fixed buttonNew_Click() bug in MasterDetail demo code.
5. Fixed bug where list item property change events were not wired for items already in list at ObjectListView<T> construction time.
6. Included license terms in Readme.txt (it's free!).
7. Added debugger visualizer!

### Version 1.0.0.8, 2/25/07
1. Extended Filter property to allow complex expressions.
2. Added ApplyFilter() method to update view when FilterPredicate criteria has changed (but the delegate has not).

### Version 1.0.0.7, 1/1/07
1. Added a generic version of ObjectListView: ObjectListView<T>.
2. Bug fix: during AddNew() when the view is filtered, return the last view position as the index of the added item (instead of the last list position).
3. Bug fix: Raise ListChanged+Reset event when FilterPredicate property is set.

### Version 1.0.0.6, 12/26/06
1. Added ICancelAddNew support.  A list item added via AddNew() is now "uncommitted" until EndNew() is called.  "Uncommitted" means
   that the item will appear as the last item in the view, and will not be repositioned due to sorting, or excluded due to filtering.
   Also, no ListChanged+ItemChanged events will be raised for the uncommitted row.  The uncommitted item is available through the indexer and
   reflected in the Count property however.
2. Behavior changes for list item types implementing IEditableObject:
	- During a call to AddNew(), the new (uncommitted) item is added immediately to the underlying list.
	- The uncommitted item is available through the indexer and Count properties.
3. Bug fix: Events are not raised when a lock is held.

### Version 1.0.0.5, 12/17/06
1. Added FilterPredicate property for code-based filtering.
2. Bug fix: When raising ListChanged+ItemDeleted for a call to ObjectListView.Remove() for a non-notifying list, set ListChangedEventArgs.NewIndex to the correct value (was zero).


### Version 1.0.0.4, 12/10/06
Added property comparers for custom sorting.


### Version 1.0.0.3, 12/3/06
1. Ensure that sorting is stable (preserves original list order when item sort values are equal).
2. Added BeginUpdate() and EndUpdate() methods to improve performance when making large changes to the underlying list.
3. Added ITypedList support, allowing binding components to detect the properties of the list items.
4. Added Sorted event, which is raised after an explicit sort (ApplySort()) or an implicit sort (item property change).
5. Bug fix: Raise ListChanged+Reset only once during a sort operation when items have been added to the underlying list, unknown to the view.
6. Added additional filter operators (!=, <>, <, <=, >, >=).

### Version 1.0.0.1, 10/11/06
1. Fixed RemovedFilter (previously threw NotImplementedException).
2. Respect IsSynchronized and SyncRoot properties of underlying list
	- All ObjectListView methods and properties are now thread-safe IF the underlying list is synchronized.
	- ListChanged events are raised after the view is unlocked, to avoid deadlocks on list access in event handlers.

### Version 1.0.0.0, 10/9/06

Initial version.  

---  
Questions?  Comments?  Email me at <jesse@teamjohnston.net> or post a comment to my blog at [www.teamjohnston.net/blog][1].Cheers,  
Jesse Johnston  
Februrary 2015

[1]: http://www.teamjohnston.net/blog/
[2]: http://www.microsoft.com/downloads/details.aspx?FamilyID=06616212-0356-46A0-8DA2-EEBC53A68034&displaylang=en