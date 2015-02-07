// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests.Generic
{
	[TestClass]
	public class ICancelAddNewTests
	{
		[TestMethod]
		public void AddNewAndEndNew()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			ObjectView<SimpleClass> added = (ObjectView<SimpleClass>)view.AddNew();

			((ICancelAddNew)view).EndNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void AddNewAndCancelNew()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			ObjectView<SimpleClass> added = (ObjectView<SimpleClass>)view.AddNew();

			((ICancelAddNew)view).CancelNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void AddNewEditableObjectAndEndNew()
		{
			ViewFactory<SimpleEditableObject> factory = ViewFactory<SimpleEditableObject>.IList();
			ObjectListView<SimpleEditableObject> view = factory.View;

			ObjectView<SimpleEditableObject> added = (ObjectView<SimpleEditableObject>)view.AddNew();

			((ICancelAddNew)view).EndNew(view.IndexOf(added.Object));
			Assert.AreEqual(2, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void AddNewEditableObjectAndCancelNew()
		{
			ViewFactory<SimpleEditableObject> factory = ViewFactory<SimpleEditableObject>.IList();
			ObjectListView<SimpleEditableObject> view = factory.View;

			ObjectView<SimpleEditableObject> added = (ObjectView<SimpleEditableObject>)view.AddNew();

			((ICancelAddNew)view).CancelNew(view.IndexOf(added.Object));
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void EndNewWithoutAddNew()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			((ICancelAddNew)view).EndNew(0);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void EndNewWithoutAddNewWithListAdd()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IBindingList();
			ObjectListView<SimpleClass> view = factory.View;

			factory.List.Add(new SimpleClass());
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			((ICancelAddNew)view).EndNew(0);
			Assert.AreEqual(1, factory.ListChangedAddedCount);
		}

		[TestMethod]
		public void CancelNewWithoutAddNew()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			((ICancelAddNew)view).CancelNew(0);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedDeletedCount);
		}

		[TestMethod]
		public void CancelNewWithoutAddNewWithListAdd()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IBindingList();
			ObjectListView<SimpleClass> view = factory.View;

			factory.List.Add(new SimpleClass());
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			((ICancelAddNew)view).CancelNew(0);
			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedDeletedCount);
		}
	}
}
#endif
