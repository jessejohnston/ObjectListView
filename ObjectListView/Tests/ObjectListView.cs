// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class ObjectListViewTests
	{
		[TestMethod]
		public void ConstructEmptyList()
		{
			ObjectListView view = new ObjectListView(new ArrayList());
			Assert.IsNull(view.ItemType);
			Assert.AreEqual(0, view.Count);
		}

		[TestMethod]
		public void ConstructEmptyGenericList()
		{
			ObjectListView view = new ObjectListView(new List<SimpleClass>());
			Assert.AreEqual(typeof(SimpleClass), view.ItemType);
			Assert.AreEqual(0, view.Count);
		}

		[TestMethod]
		public void BeginUpdate()
		{
			ViewFactory factory = ViewFactory.IBindingListINotifyPropertyChangedItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.BeginUpdate();

			NotifyingListItem item = new NotifyingListItem();

			view.Add(item);			// normally would raise ListChanged (ItemAdded)
			item.IntegerValue = 9;	// normally would raise ListChanged (ItemChanged, reflecting PropertyChanged)

			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedItemChangedCount);

			view.EndUpdate();

			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedItemChangedCount);
			Assert.AreEqual(1, factory.ListChangedResetCount);
		}

		[TestMethod]
		public void BeginUpdateSortFilter()
		{
			ViewFactory factory = ViewFactory.IBindingListINotifyPropertyChangedItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new NotifyingListItem(1, "aaa", DateTime.Now));

			view.Filter = "IntegerValue < 5";
			view.ApplySort(TypeDescriptor.GetProperties(typeof(NotifyingListItem))["StringValue"], ListSortDirection.Descending);

			factory.ClearEventCounts();

			view.BeginUpdate();

			list.Add(new NotifyingListItem(5, "bbb", DateTime.Now));
			list.Add(new NotifyingListItem(5, "ccc", DateTime.Now));
			list.Add(new NotifyingListItem(2, "ddd", DateTime.Now));
			list.Add(new NotifyingListItem(5, "eee", DateTime.Now));

			((NotifyingListItem)list[1]).IntegerValue = 4;
			list.RemoveAt(3);

			view.EndUpdate();

			Assert.AreEqual(2, view.Count);
			Assert.AreEqual("bbb", ((NotifyingListItem)view[0]).StringValue);
			Assert.AreEqual("aaa", ((NotifyingListItem)view[1]).StringValue);

			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.ListChangedItemChangedCount);
			Assert.AreEqual(0, factory.ListChangedDeletedCount);
			Assert.AreEqual(1, factory.ListChangedResetCount);
		}

		[TestMethod]
		public void EndUpdateWithoutBeginUpdate()
		{
			ViewFactory factory = ViewFactory.IBindingListINotifyPropertyChangedItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			NotifyingListItem item = new NotifyingListItem();

			view.Add(item);			// normally would raise ListChanged (ItemAdded)
			item.IntegerValue = 9;	// normally would raise ListChanged (ItemChanged, reflecting PropertyChanged)

			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedItemChangedCount);

			view.EndUpdate();

			Assert.AreEqual(1, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedItemChangedCount);
			Assert.AreEqual(0, factory.ListChangedResetCount);
		}

		[TestMethod]
		public void EndUpdateWithoutChanges()
		{
			ViewFactory factory = ViewFactory.IBindingListINotifyPropertyChangedItems();
			ObjectListView view = factory.View;

			NotifyingListItem item = new NotifyingListItem();

			view.Add(item);
			item.IntegerValue = 9;

			factory.ClearEventCounts();

			view.BeginUpdate();
			view.EndUpdate();

			Assert.AreEqual(0, factory.ListChangedResetCount);
		}

		[TestMethod]
		public void SortedEvent()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(3, "ddd", now));
			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1960, 1, 1)));

			view.ItemType = typeof(SimpleClass);

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor intProp = props["IntegerValue"];

			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(intProp, ListSortDirection.Ascending)});

			Assert.AreEqual(0, factory.SortedCount);

			view.ApplySort(sorts);

			Assert.AreEqual(1, factory.SortedCount);
		}

		[TestMethod]
		public void RemoveSorted()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(5, "aaa", now));
			list.Add(new SimpleClass(4, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(3, "ddd", now));
			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(2, "bbb", new DateTime(1960, 1, 1)));

			view.ItemType = typeof(SimpleClass);

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor intProp = props["IntegerValue"];

			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(intProp, ListSortDirection.Ascending)});

			view.ApplySort(sorts);
			factory.ClearEventCounts();

			view.RemoveAt(3);
			view.RemoveAt(2);

			Assert.AreEqual(4, view.Count);
			Assert.AreEqual(1, ((SimpleClass)view[0]).IntegerValue);
			Assert.AreEqual(2, ((SimpleClass)view[1]).IntegerValue);
			Assert.AreEqual(4, ((SimpleClass)view[2]).IntegerValue);
			Assert.AreEqual(5, ((SimpleClass)view[3]).IntegerValue);

			Assert.AreEqual(2, factory.ListChangedDeletedCount);
			Assert.AreEqual(0, factory.ListChangedResetCount);
			Assert.AreEqual(0, factory.SortedCount);
		}

		[TestMethod]
		public void RemoveSortedIBindingList()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(5, "aaa", now));
			list.Add(new SimpleClass(4, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(3, "ddd", now));
			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(2, "bbb", new DateTime(1960, 1, 1)));

			view.ItemType = typeof(SimpleClass);

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor intProp = props["IntegerValue"];

			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(intProp, ListSortDirection.Ascending)});

			view.ApplySort(sorts);
			factory.ClearEventCounts();

			view.RemoveAt(3);
			view.RemoveAt(2);

			Assert.AreEqual(4, view.Count);
			Assert.AreEqual(1, ((SimpleClass)view[0]).IntegerValue);
			Assert.AreEqual(2, ((SimpleClass)view[1]).IntegerValue);
			Assert.AreEqual(4, ((SimpleClass)view[2]).IntegerValue);
			Assert.AreEqual(5, ((SimpleClass)view[3]).IntegerValue);

			Assert.AreEqual(2, factory.ListChangedDeletedCount);
			Assert.AreEqual(0, factory.ListChangedResetCount);
			Assert.AreEqual(0, factory.SortedCount);
		}

		[TestMethod]
		public void PropertyComparerDefault()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			Assert.IsNotNull(view.PropertyComparers);
			Assert.AreEqual(0, view.PropertyComparers.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void PropertyComparerNoItemType()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			PropertyComparerCollection c = view.PropertyComparers;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PropertyComparerNullProperty()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers[null] = new StringLengthComparer();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void PropertyComparerEmptyProperty()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers[""] = new StringLengthComparer();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void PropertyComparerInvalidProperty()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["NonexistentProperty"] = new StringLengthComparer();
		}

		[TestMethod]
		public void PropertyComparerAdd()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers.Add("StringProperty", new StringLengthComparer());
			Assert.AreEqual(1, view.PropertyComparers.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void PropertyComparerAddDuplicate()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers.Add("StringProperty", new StringLengthComparer());
			view.PropertyComparers.Add("StringProperty", new StringLengthComparer());
		}

		[TestMethod]
		public void PropertyComparerSet()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			Assert.AreEqual(1, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void PropertyComparerSetTwice()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			Assert.AreEqual(1, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void PropertyComparerSetNull()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["StringValue"] = null;
			Assert.AreEqual(0, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void PropertyComparerSetNullAfterSet()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["StringValue"] = null;
			Assert.AreEqual(0, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void PropertyComparerGet()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			StringLengthComparer s = new StringLengthComparer();
			view.PropertyComparers["StringValue"] = s;
			Assert.AreEqual(s, view.PropertyComparers["StringValue"]);
		}

		[TestMethod]
		public void PropertyComparerGetNotSet()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			Assert.IsNull(view.PropertyComparers["StringValue"]);
		}

		[TestMethod]
		public void PropertyComparerGetSetNull()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers["StringValue"] = null;
			Assert.IsNull(view.PropertyComparers["StringValue"]);
		}

		[TestMethod]
		public void PropertyComparerRemove()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers.Add("StringProperty", new StringLengthComparer());
			view.PropertyComparers.Remove("StringProperty");
			Assert.AreEqual(0, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void PropertyComparerRemoveNoAdd()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;

			view.PropertyComparers.Remove("StringProperty");
			Assert.AreEqual(0, view.PropertyComparers.Count);
		}

		[TestMethod]
		public void UsePropertyComparer()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			factory.ClearEventCounts();

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.ApplySort(stringProp, ListSortDirection.Ascending);

			Assert.AreEqual(1, factory.ListChangedResetCount);
			Assert.AreEqual(1, factory.SortedCount);

			Assert.AreEqual("a", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("aa", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aaaa", ((SimpleClass)view[3]).StringValue);
		}

		[TestMethod]
		public void UsePropertyComparerAfterSort()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			view.ApplySort(stringProp, ListSortDirection.Ascending);

			Assert.AreEqual("a", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("aa", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("aaaa", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[3]).StringValue);

			factory.ClearEventCounts();

			view.PropertyComparers["StringValue"] = new StringLengthComparer();

			Assert.AreEqual(1, factory.ListChangedResetCount);
			Assert.AreEqual(1, factory.SortedCount);

			Assert.AreEqual("a", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("aa", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aaaa", ((SimpleClass)view[3]).StringValue);
		}

		[TestMethod]
		public void UsePropertyComparerUnsorted()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			factory.ClearEventCounts();

			view.PropertyComparers["StringValue"] = new StringLengthComparer();

			Assert.AreEqual(0, factory.ListChangedResetCount);
			Assert.AreEqual(0, factory.SortedCount);

			Assert.AreEqual("a", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("aaaa", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aa", ((SimpleClass)view[3]).StringValue);
		}

		[TestMethod]
		public void UsePropertyComparerOneColumnInMultiColumnSort()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", new DateTime(1991, 3, 5)));
			list.Add(new SimpleClass(1, "aa", new DateTime(1989, 7,22)));

			view.PropertyComparers["StringValue"] = new StringLengthComparer();

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(props["StringValue"], ListSortDirection.Descending),
				new ListSortDescription(props["DateTimeValue"], ListSortDirection.Ascending)
			});
			view.ApplySort(sorts);

			Assert.AreEqual("aaaa", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual(new DateTime(1989, 7, 22), ((SimpleClass)view[2]).DateTimeValue);
			Assert.AreEqual(new DateTime(1991, 3, 5), ((SimpleClass)view[3]).DateTimeValue);
			Assert.AreEqual("a", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void UsePropertyComparerMultipleColumnsInMultiColumnSort()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", new DateTime(1989, 7, 22)));
			list.Add(new SimpleClass(1, "aa", new DateTime(1991, 3, 5)));

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["DateTimeValue"] = new DateTimeDayComparer();

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(props["StringValue"], ListSortDirection.Descending),
				new ListSortDescription(props["DateTimeValue"], ListSortDirection.Ascending)
			});
			view.ApplySort(sorts);

			Assert.AreEqual("aaaa", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual(new DateTime(1991, 3, 5), ((SimpleClass)view[2]).DateTimeValue);
			Assert.AreEqual(new DateTime(1989, 7, 22), ((SimpleClass)view[3]).DateTimeValue);
			Assert.AreEqual("a", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void PropertyComparerEventsMultipleSets()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			view.ApplySort(stringProp, ListSortDirection.Ascending);

			factory.ClearEventCounts();

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["DateTimeValue"] = new DateTimeDayComparer();

			Assert.AreEqual(2, factory.ListChangedResetCount);
			Assert.AreEqual(2, factory.SortedCount);

			Assert.AreEqual("a", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("aa", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aaaa", ((SimpleClass)view[3]).StringValue);
		}

		[TestMethod]
		public void PropertyComparerBeginUpdate()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			view.ApplySort(stringProp, ListSortDirection.Ascending);

			factory.ClearEventCounts();

			view.BeginUpdate();
			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["DateTimeValue"] = new DateTimeDayComparer();
			view.EndUpdate();

			Assert.AreEqual(1, factory.ListChangedResetCount);
			Assert.AreEqual(0, factory.SortedCount);
		}

		[TestMethod]
		public void PropertyComparerEndUpdateWithoutBeginUpdate()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "a", now));
			list.Add(new SimpleClass(1, "bbb", now));
			list.Add(new SimpleClass(1, "aaaa", now));
			list.Add(new SimpleClass(1, "aa", now));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			PropertyDescriptor stringProp = props["StringValue"];

			view.ApplySort(stringProp, ListSortDirection.Ascending);

			factory.ClearEventCounts();

			view.PropertyComparers["StringValue"] = new StringLengthComparer();
			view.PropertyComparers["DateTimeValue"] = new DateTimeDayComparer();

			Assert.AreEqual(2, factory.ListChangedResetCount);
			Assert.AreEqual(2, factory.SortedCount);

			view.EndUpdate();

			Assert.AreEqual(2, factory.ListChangedResetCount);
			Assert.AreEqual(2, factory.SortedCount);
		}

		[TestMethod]
		public void FilterPredicateDefault()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			Assert.IsNull(factory.View.FilterPredicate);
		}

		[TestMethod]
		public void FilterPredicateNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = null;
			Assert.IsNull(view.FilterPredicate);

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("aaa", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("eee", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void FilterPredicateEventRaised()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			factory.ClearEventCounts();

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };

			Assert.IsNotNull(view.FilterPredicate);
			Assert.AreEqual(1, factory.ListChangedResetCount);
		}

		[TestMethod]
		public void FilterPredicateEqualsInt()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };

			Assert.IsNotNull(view.FilterPredicate);

			Assert.AreEqual(3, view.Count);
			Assert.AreEqual("bbb", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("eee", ((SimpleClass)view[2]).StringValue);
		}

		[TestMethod]
		public void FilterPredicateVaryingCriteria()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(2, "bbb", DateTime.Now));
			list.Add(new SimpleClass(3, "ccc", DateTime.Now));
			list.Add(new SimpleClass(4, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			object criteria = 1;
			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue < (int)criteria; };
			Assert.AreEqual(0, view.Count);

			for (int i = 2; i < 6; i++)
			{
				criteria = i;
				view.ApplyFilter();
				Assert.AreEqual(i-1, view.Count);
			}
		}

		[TestMethod]
		public void FilterPredicateVaryingListItemProperties()
		{
			ViewFactory factory = ViewFactory.IBindingListINotifyPropertyChangedItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new NotifyingListItem(1, "aaa", DateTime.Now));
			list.Add(new NotifyingListItem(2, "bbb", DateTime.Now));

			NotifyingListItem changingItem = new NotifyingListItem(3, "ccc", DateTime.Now);
			list.Add(changingItem);
			list.Add(new NotifyingListItem(4, "ddd", DateTime.Now));
			list.Add(new NotifyingListItem(5, "eee", DateTime.Now));

			view.ItemType = typeof(NotifyingListItem);

			view.FilterPredicate = delegate(object listItem) { return ((NotifyingListItem)listItem).IntegerValue < 3; };
			Assert.AreEqual(2, view.Count);

			changingItem.IntegerValue = 0;
			Assert.AreEqual(3, view.Count);
		}

		[TestMethod]
		public void FilterPredicateRemoveFilter()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(2, "bbb", DateTime.Now));
			list.Add(new SimpleClass(3, "ccc", DateTime.Now));
			list.Add(new SimpleClass(4, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };

			Assert.AreEqual(1, view.Count);

			view.RemoveFilter();

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual(1, ((SimpleClass)view[0]).IntegerValue);
			Assert.AreEqual(2, ((SimpleClass)view[1]).IntegerValue);
			Assert.AreEqual(3, ((SimpleClass)view[2]).IntegerValue);
			Assert.AreEqual(4, ((SimpleClass)view[3]).IntegerValue);
			Assert.AreEqual(5, ((SimpleClass)view[4]).IntegerValue);
		}

		[TestMethod]
		public void FilterPredicateRemoveSetNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(2, "bbb", DateTime.Now));
			list.Add(new SimpleClass(3, "ccc", DateTime.Now));
			list.Add(new SimpleClass(4, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };

			Assert.AreEqual(1, view.Count);

			view.FilterPredicate = null;

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual(1, ((SimpleClass)view[0]).IntegerValue);
			Assert.AreEqual(2, ((SimpleClass)view[1]).IntegerValue);
			Assert.AreEqual(3, ((SimpleClass)view[2]).IntegerValue);
			Assert.AreEqual(4, ((SimpleClass)view[3]).IntegerValue);
			Assert.AreEqual(5, ((SimpleClass)view[4]).IntegerValue);
		}

		[TestMethod]
		public void FilterPredicateRemoveSetFilterNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(2, "bbb", DateTime.Now));
			list.Add(new SimpleClass(3, "ccc", DateTime.Now));
			list.Add(new SimpleClass(4, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };
			Assert.AreEqual(1, view.Count);

			view.Filter = null;
			Assert.AreEqual(5, view.Count);
		}

		[TestMethod]
		public void FilterPredicateSetFilter()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(2, "bbb", DateTime.Now));
			list.Add(new SimpleClass(3, "ccc", DateTime.Now));
			list.Add(new SimpleClass(4, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue == 5; };
			Assert.AreEqual(1, view.Count);

			view.Filter = "StringValue < 'ccc'";
			Assert.AreEqual(2, view.Count);
		}

		[TestMethod]
		public void FilterRemoveSetPredicateNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.Filter = "IntegerValue = 5";
			Assert.AreEqual(3, view.Count);

			view.FilterPredicate = null;
			Assert.AreEqual(5, view.Count);
		}

		[TestMethod]
		public void FilterSetPredicate()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			view.Filter = "IntegerValue = 5";
			Assert.AreEqual(3, view.Count);

			view.FilterPredicate = delegate(object listItem) { return ((SimpleClass)listItem).IntegerValue < 5; };
			Assert.AreEqual(2, view.Count);
		}

		[TestMethod]
		public void ToArray()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			list.Add(new SimpleClass(1, "aaa", DateTime.Now));
			list.Add(new SimpleClass(5, "bbb", DateTime.Now));
			list.Add(new SimpleClass(5, "ccc", DateTime.Now));
			list.Add(new SimpleClass(2, "ddd", DateTime.Now));
			list.Add(new SimpleClass(5, "eee", DateTime.Now));

			view.ItemType = typeof(SimpleClass);

			Array arr = view.ToArray();
			Assert.IsNotNull(arr);
			Assert.AreEqual(5, arr.Length);

			for (int i = 0; i < view.Count; i++)
				Assert.AreEqual(view[i], arr.GetValue(i));
		}

		[TestMethod]
		public void ToArrayEmpty()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.ItemType = typeof(SimpleClass);

			Array arr = view.ToArray();
			Assert.IsNotNull(arr);
			Assert.AreEqual(0, arr.Length);
		}

		[TestMethod]
		public void FindCriteria()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			int index = view.Find("DateTimeValue = 6/6/1975");
			Assert.AreEqual(2, index);
		}

		[TestMethod]
		public void FindCriteriaNullValue()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, null, new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			int index = view.Find("StringValue = null");
			Assert.AreEqual(1, index);
		}

		[TestMethod]
		public void FindCriteriaPath()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 12);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, carol);
			Customer mike = new Customer("Mike Palooza", acme, john);

			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));

			int firstIndex = view.Find("Customer.AccountRep.Department = 12 AND Customer.AccountRep.Name = Carol*");

			Assert.AreEqual(2, firstIndex);
		}

		[TestMethod]
		public void FindCriteriaPathInaccessibleValue()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 12);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, carol);
			Customer mike = new Customer("Mike Palooza", acme, john);
			Customer rick = new Customer("Rick Doohan", ajax, null);

			list.Add(new Order(rick, "Glue", 15));
			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));

			int firstIndex = view.Find("Customer.AccountRep.Department = 12 AND Customer.AccountRep.Name = Carol*");

			Assert.AreEqual(3, firstIndex);
		}

		[TestMethod]
		public void FindCriteriaSortedFiltered()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(70, "ddd", new DateTime(1989, 7, 22)));
			view.Filter = "StringValue != 'ccc'";
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			view.ApplySort(props["IntegerValue"], ListSortDirection.Ascending);

			int index = view.Find("DateTimeValue = 12/12/1980");
			Assert.AreEqual(1, index);
		}

		[TestMethod]
		public void FindCriteriaNotFound()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			int index = view.Find("DateTimeValue = 6/7/1975");
			Assert.AreEqual(-1, index);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindCriteriaNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Find((string)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void FindCriteriaEmpty()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Find("");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void FindCriteriaInvalid()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Find("someprop = 7");
		}

		[TestMethod]
		public void FindPredicate()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			int index = view.Find(delegate(object item)
			{
				return ((SimpleClass)item).DateTimeValue == new DateTime(1975, 6, 6);
			});

			Assert.AreEqual(2, index);
		}

		[TestMethod]
		public void FindPrediateNullValue()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, null, new DateTime(1975, 6, 6)));

			int index = view.Find(delegate(object item)
			{
				return ((SimpleClass)item).StringValue == null;
			});

			Assert.AreEqual(2, index);
		}

		[TestMethod]
		public void FindPredicateSortedFiltered()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(70, "ddd", new DateTime(1989, 7, 22)));
			view.Filter = "StringValue != 'ccc'";
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			view.ApplySort(props["IntegerValue"], ListSortDirection.Ascending);

			int index = view.Find(delegate(object item)
			{
				return ((SimpleClass)item).DateTimeValue == new DateTime(1980, 12, 12);
			});

			Assert.AreEqual(1, index);
		}

		[TestMethod]
		public void FindPredicateNotFound()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			int index = view.Find(delegate(object item)
			{
				return ((SimpleClass)item).DateTimeValue == new DateTime(1975, 7, 6);
			});

			Assert.AreEqual(-1, index);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindPredicateNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			view.Find((Predicate)null);
		}

		[TestMethod]
		public void Select()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			IList matches = view.Select("IntegerValue > 60 AND StringValue != 'bbb'");

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual(100, ((SimpleClass)matches[0]).IntegerValue);
			Assert.AreEqual(65, ((SimpleClass)matches[1]).IntegerValue);
		}

		[TestMethod]
		public void SelectNullValue()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, null, new DateTime(1975, 6, 6)));

			IList matches = view.Select("StringValue = null");
			Assert.IsNotNull(matches);
			Assert.AreEqual(1, matches.Count);
			Assert.AreEqual(60, ((SimpleClass)matches[0]).IntegerValue);
		}

		[TestMethod]
		public void SelectCriteriaPath()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 12);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, carol);
			Customer mike = new Customer("Mike Palooza", acme, john);

			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));

			IList matches = view.Select("Customer.AccountRep.Department = 12 AND Customer.AccountRep.Name = Carol*");

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual("Boxes", ((Order)matches[0]).Product);
			Assert.AreEqual("Gadgets", ((Order)matches[1]).Product);
		}

		[TestMethod]
		public void SelectCriteriaPathInaccessibleValue()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 12);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, carol);
			Customer mike = new Customer("Mike Palooza", acme, john);
			Customer rick = new Customer("Rick Doohan", ajax, null);

			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));
			list.Add(new Order(rick, "Glue", 15));

			IList matches = view.Select("Customer.AccountRep.Department = 12 AND Customer.AccountRep.Name = Carol*");

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual("Boxes", ((Order)matches[0]).Product);
			Assert.AreEqual("Gadgets", ((Order)matches[1]).Product);
		}

		[TestMethod]
		public void SelectNoMatches()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			IList matches = view.Select("IntegerValue > 100");

			Assert.IsNotNull(matches);
			Assert.AreEqual(0, matches.Count);
		}

		[TestMethod]
		public void SelectSortedFiltered()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(110, "eee", new DateTime(1975, 6, 6)));

			view.Filter = "StringValue != 'bbb'";
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			view.ApplySort(props["IntegerValue"], ListSortDirection.Ascending);

			IList matches = view.Select("IntegerValue > 60 AND StringValue != 'aaa'");

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual(65, ((SimpleClass)matches[0]).IntegerValue);
			Assert.AreEqual(110, ((SimpleClass)matches[1]).IntegerValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SelectNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Select((string)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SelectEmpty()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Select("");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SelectInvalid()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Select("someProp = 16");
		}

		[TestMethod]
		public void SelectPredicate()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			IList matches = view.Select(delegate(object item)
			{
				return ((SimpleClass)item).IntegerValue > 60 && ((SimpleClass)item).StringValue != "bbb";
			});

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual(100, ((SimpleClass)matches[0]).IntegerValue);
			Assert.AreEqual(65, ((SimpleClass)matches[1]).IntegerValue);
		}

		[TestMethod]
		public void SelectPredicateNoMatches()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			IList matches = view.Select(delegate(object item)
			{
				return ((SimpleClass)item).IntegerValue > 100;
			});

			Assert.IsNotNull(matches);
			Assert.AreEqual(0, matches.Count);
		}

		[TestMethod]
		public void SelectPredicateSortedFiltered()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(110, "eee", new DateTime(1975, 6, 6)));

			view.Filter = "StringValue != 'bbb'";
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			view.ApplySort(props["IntegerValue"], ListSortDirection.Ascending);

			IList matches = view.Select(delegate(object item)
			{
				return ((SimpleClass)item).IntegerValue > 60 && ((SimpleClass)item).StringValue != "aaa";
			});

			Assert.IsNotNull(matches);
			Assert.AreEqual(2, matches.Count);
			Assert.AreEqual(65, ((SimpleClass)matches[0]).IntegerValue);
			Assert.AreEqual(110, ((SimpleClass)matches[1]).IntegerValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SelectPredicateNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;

			view.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			view.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			view.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));
			view.Add(new SimpleClass(65, "ccc", new DateTime(1975, 6, 6)));

			view.Select((Predicate)null);
		}

		[TestMethod]
		public void SortNull()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(3, "ddd", now));

			// Sort should be:
			//  1 eee Now
			//  1 bbb 1950
			//  3 ccc Now
			//  1 aaa Now
			//  3 ddd Now
			view.Sort = null;

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("eee", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aaa", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void SortEmpty()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(3, "ddd", now));

			// Sort should be:
			//  1 eee Now
			//  1 bbb 1950
			//  3 ccc Now
			//  1 aaa Now
			//  3 ddd Now
			view.Sort = "";

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("eee", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("aaa", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void SortGetNotSet()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();

			Assert.AreEqual("", factory.View.Sort);
		}

		[TestMethod]
		public void SortGetApplySort()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			list.Add(new SimpleClass(4, "aaa", new DateTime(1930, 5, 5)));
			list.Add(new SimpleClass(2, "qqq", new DateTime(1950, 5, 5)));
			list.Add(new SimpleClass(1, "zzz", new DateTime(1980, 5, 5)));
			list.Add(new SimpleClass(2, "mmm", new DateTime(1960, 5, 5)));
			list.Add(new SimpleClass(3, "bbb", new DateTime(1940, 5, 5)));
			list.Add(new SimpleClass(2, "mmm", new DateTime(1970, 5, 5)));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(props["IntegerValue"], ListSortDirection.Ascending),
				new ListSortDescription(props["StringValue"], ListSortDirection.Descending),
				new ListSortDescription(props["DateTimeValue"], ListSortDirection.Ascending)
			});
			view.ApplySort(sorts);

			Assert.AreEqual("IntegerValue ASC, StringValue DESC, DateTimeValue ASC", view.Sort);
		}

		[TestMethod]
		public void SortGetRemoveSort()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			list.Add(new SimpleClass(4, "aaa", new DateTime(1930, 5, 5)));
			list.Add(new SimpleClass(2, "qqq", new DateTime(1950, 5, 5)));
			list.Add(new SimpleClass(1, "zzz", new DateTime(1980, 5, 5)));
			list.Add(new SimpleClass(2, "mmm", new DateTime(1960, 5, 5)));
			list.Add(new SimpleClass(3, "bbb", new DateTime(1940, 5, 5)));
			list.Add(new SimpleClass(2, "mmm", new DateTime(1970, 5, 5)));

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(SimpleClass));
			ListSortDescriptionCollection sorts = new ListSortDescriptionCollection(new ListSortDescription[] {
				new ListSortDescription(props["IntegerValue"], ListSortDirection.Ascending),
				new ListSortDescription(props["StringValue"], ListSortDirection.Descending),
				new ListSortDescription(props["DateTimeValue"], ListSortDirection.Ascending)
			});
			view.ApplySort(sorts);
			view.RemoveSort();

			Assert.AreEqual("", view.Sort);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SortInvalidProperty()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			view.Sort = "BadProperty";
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SortInvalidDirection()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			view.Sort = "StringValue XYZ";
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SortInvalidTerm()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			view.Sort = "StringValue ASC, DateTimeValue DESC IntegerValue";
		}

		[TestMethod]
		public void SortSinglePropDefaultDirection()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(3, "ddd", now));

			// Sort should be:
			//  1 aaa Now
			//  1 bbb 1950
			//  3 ccc Now
			//  3 ddd Now
			//  1 eee Now
			view.Sort = "StringValue";

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("aaa", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("eee", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void SortSinglePropAsc()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(3, "ddd", now));

			// Sort should be:
			//  1 aaa Now
			//  1 bbb 1950
			//  3 ccc Now
			//  3 ddd Now
			//  1 eee Now
			view.Sort = "StringValue ASC";

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("aaa", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("eee", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void SortSinglePropDesc()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(3, "ddd", now));

			// Sort should be:
			//  1 eee Now
			//  3 ddd Now
			//  3 ccc Now
			//  1 bbb 1950
			//  1 aaa Now
			view.Sort = "StringValue DESC";

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("eee", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("aaa", ((SimpleClass)view[4]).StringValue);
		}

		[TestMethod]
		public void SortPropertyPath()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 10);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, carol);
			Customer mike = new Customer("Mike Palooza", acme, john);

			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));

			// Sort should be:
			// Bolts
			// Boxes
			// Gadgets
			// Widgets
			// Nails
			view.Sort = "Customer.AccountRep.Department ASC";

			Assert.AreEqual(5, view.Count);
			Assert.AreEqual("Bolts", ((Order)view[0]).Product);
			Assert.AreEqual("Boxes", ((Order)view[1]).Product);
			Assert.AreEqual("Gadgets", ((Order)view[2]).Product);
			Assert.AreEqual("Widgets", ((Order)view[3]).Product);
			Assert.AreEqual("Nails", ((Order)view[4]).Product);
		}

		[TestMethod]
		public void SortPropertyPathInaccessibleValues()
		{
			ViewFactory factory = ViewFactory.IListOrders();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(Order);

			Company acme = new Company("Acme", "Seattle", "WA");
			Company ajax = new Company("Ajax", "Detroit", "MI");
			AccountRep steve = new AccountRep("Steve O'Donnell", 56);
			AccountRep carol = new AccountRep("Carol Hanson", 12);
			AccountRep john = new AccountRep("John Roche", 10);

			Customer bob = new Customer("Bob Smith", acme, steve);
			Customer tom = new Customer("Tom Kelly", ajax, null);
			Customer mike = new Customer("Mike Palooza", acme, carol);
			Customer rick = new Customer("Rick Doohan", ajax, john);

			list.Add(new Order(bob, "Widgets", 10));
			list.Add(new Order(bob, "Nails", 200));
			list.Add(new Order(tom, "Boxes", 20));
			list.Add(new Order(tom, "Gadgets", 7));
			list.Add(new Order(mike, "Bolts", 3000));
			list.Add(new Order(rick, "Glue", 15));

			// Sort should be:
			// (note that the items that compare equally will be sorted in reverse of list order because the sort direction is descending)
			// Nails
			// Widgets
			// Bolts
			// Glue
			// Gadgets
			// Boxes
			view.Sort = "Customer.AccountRep.Department DESC";

			Assert.AreEqual(6, view.Count);
			Assert.AreEqual("Nails", ((Order)view[0]).Product);
			Assert.AreEqual("Widgets", ((Order)view[1]).Product);
			Assert.AreEqual("Bolts", ((Order)view[2]).Product);
			Assert.AreEqual("Glue", ((Order)view[3]).Product);
			Assert.AreEqual("Gadgets", ((Order)view[4]).Product);
			Assert.AreEqual("Boxes", ((Order)view[5]).Product);
		}

		[TestMethod]
		public void SortMultipleProps()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(3, "ddd", now));
			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1960, 1, 1)));

			// Sort should be:
			//  1 eee Now
			//  1 bbb 1950
			//  1 bbb 1960
			//  1 aaa Now
			//  3 ddd Now
			//  3 ccc Now
			view.Sort = "IntegerValue ASC, StringValue DESC, DateTimeValue ASC";

			Assert.AreEqual(6, view.Count);
			Assert.AreEqual("eee", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual(new DateTime(1950, 1, 1), ((SimpleClass)view[1]).DateTimeValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual(new DateTime(1960, 1, 1), ((SimpleClass)view[2]).DateTimeValue);
			Assert.AreEqual("aaa", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[4]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[5]).StringValue);
		}

		[TestMethod]
		public void SortMultiplePropsDefaultDirection()
		{
			ViewFactory factory = ViewFactory.IListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			view.ItemType = typeof(SimpleClass);

			DateTime now = DateTime.Now;

			list.Add(new SimpleClass(1, "aaa", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1950, 1, 1)));
			list.Add(new SimpleClass(3, "ccc", now));
			list.Add(new SimpleClass(3, "ddd", now));
			list.Add(new SimpleClass(1, "eee", now));
			list.Add(new SimpleClass(1, "bbb", new DateTime(1960, 1, 1)));

			// Sort should be:
			//  1 eee Now
			//  1 bbb 1950
			//  1 bbb 1960
			//  1 aaa Now
			//  3 ddd Now
			//  3 ccc Now
			view.Sort = "IntegerValue ASC, StringValue DESC, DateTimeValue";

			Assert.AreEqual(6, view.Count);
			Assert.AreEqual("eee", ((SimpleClass)view[0]).StringValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[1]).StringValue);
			Assert.AreEqual(new DateTime(1950, 1, 1), ((SimpleClass)view[1]).DateTimeValue);
			Assert.AreEqual("bbb", ((SimpleClass)view[2]).StringValue);
			Assert.AreEqual(new DateTime(1960, 1, 1), ((SimpleClass)view[2]).DateTimeValue);
			Assert.AreEqual("aaa", ((SimpleClass)view[3]).StringValue);
			Assert.AreEqual("ddd", ((SimpleClass)view[4]).StringValue);
			Assert.AreEqual("ccc", ((SimpleClass)view[5]).StringValue);
		}

		[TestMethod]
		public void BeforeAfterListChanged()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			int callbackOrder = 0;
			ListChangedEventArgs args = null;
			view.BeforeListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(1, callbackOrder);
				Assert.IsNull(args);
				args = e;
			};
			view.ListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(2, callbackOrder);
				Assert.AreEqual(args, e);
			};
			view.AfterListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(3, callbackOrder);
				Assert.AreEqual(args, e);
			};

			list.Add(new SimpleClass());

			Assert.AreEqual(3, callbackOrder);
		}

		[TestMethod]
		public void BeforeAfterListChangedInUpdate()
		{
			ViewFactory factory = ViewFactory.IBindingListSimpleItems();
			ObjectListView view = factory.View;
			IList list = factory.List;

			int callbackOrder = 0;
			ListChangedEventArgs args = null;
			view.BeforeListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(1, callbackOrder);
				Assert.IsNull(args);
				args = e;
			};
			view.ListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(2, callbackOrder);
				Assert.AreEqual(args, e);
			};
			view.AfterListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				callbackOrder++;
				Assert.AreEqual(3, callbackOrder);
				Assert.AreEqual(args, e);
			};

			view.BeginUpdate();
			list.Add(new SimpleClass());

			Assert.AreEqual(0, callbackOrder);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(0, factory.BeforeListChangedCount);
			Assert.AreEqual(0, factory.AfterListChangedCount);

			view.EndUpdate();

			Assert.AreEqual(3, callbackOrder);
			Assert.AreEqual(0, factory.ListChangedAddedCount);
			Assert.AreEqual(1, factory.ListChangedResetCount);
			Assert.AreEqual(1, factory.BeforeListChangedCount);
			Assert.AreEqual(1, factory.AfterListChangedCount);
		}
	}
}
#endif
