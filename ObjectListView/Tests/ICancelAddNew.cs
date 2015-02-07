// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class ICancelAddNewTests
	{
		[TestMethod]
		public void AddNewAndEndNew()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			ObjectView added = (ObjectView)view.AddNew();

			((ICancelAddNew)view).EndNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void AddNewAndCancelNew()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			ObjectView added = (ObjectView)view.AddNew();

			((ICancelAddNew)view).CancelNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void AddNewEditableObjectAndEndNew()
		{
			ViewFactory factory = ViewFactory.IListIEditableObjectItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			ObjectView added = (ObjectView)view.AddNew();

			((ICancelAddNew)view).EndNew(view.IndexOf(added.Object));
			Assert.AreEqual(2, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void AddNewEditableObjectAndCancelNew()
		{
			ViewFactory factory = ViewFactory.IListIEditableObjectItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			ObjectView added = (ObjectView)view.AddNew();

			((ICancelAddNew)view).CancelNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void EndNewWithoutAddNew()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			((ICancelAddNew)view).EndNew(0);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void EndNewWithoutAddNewWithListAdd()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			factory.List.Add(new SimpleClass());
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			((ICancelAddNew)view).EndNew(0);
			Assert.AreEqual(1, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void CancelNewWithoutAddNew()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			view.ItemType = factory.ListItemType;

			((ICancelAddNew)view).CancelNew(0);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void CancelNewWithoutAddNewWithListAdd()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			factory.List.Add(new SimpleClass());
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			((ICancelAddNew)view).CancelNew(0);
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedDeletedCount);
		}
	}
}
#endif
