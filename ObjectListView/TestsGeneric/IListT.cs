// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests.Generic
{
	[TestClass]
	public class IListTTests
	{
		[TestMethod]
		public void IndexOf()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			IList<SimpleClass> list = factory.List;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			SimpleClass bbb = new SimpleClass(80, "bbb", new DateTime(1980, 12, 12));
			list.Add(bbb);
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			Assert.AreEqual(1, factory.View.IndexOf(bbb));
		}

		[TestMethod]
		public void IndexOfItemNotInList()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			IList<SimpleClass> list = factory.List;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			SimpleClass bbb = new SimpleClass(80, "bbb", new DateTime(1980, 12, 12));
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			Assert.AreEqual(-1, factory.View.IndexOf(bbb));
		}

		[TestMethod]
		public void Insert()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			IList<SimpleClass> list = factory.List;
			ObjectListView<SimpleClass> view = factory.View;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			list.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			view.Insert(1, new SimpleClass(200, "ddd", new DateTime(1963, 3, 9)));
			Assert.AreEqual(4, view.Count);
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			SimpleClass item = list[1];
			Assert.AreEqual(200, item.IntegerValue);
			Assert.AreEqual("ddd", item.StringValue);
			Assert.AreEqual(new DateTime(1963, 3, 9), item.DateTimeValue);
		}

		[TestMethod]
		public void InsertIBindingList()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IBindingList();

			factory.List.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			factory.List.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			factory.List.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			factory.View.Insert(1, new SimpleClass(200, "ddd", new DateTime(1963, 3, 9)));
			Assert.AreEqual(4, factory.View.Count);
			Assert.AreEqual(1, factory.ListChangedAddedCount);

			SimpleClass item = factory.List[1];
			Assert.AreEqual(200, item.IntegerValue);
			Assert.AreEqual("ddd", item.StringValue);
			Assert.AreEqual(new DateTime(1963, 3, 9), item.DateTimeValue);
		}

		[TestMethod]
		public void RemoveAt()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			IList<SimpleClass> list = factory.List;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			list.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			int removingIndex = -1;
			factory.View.RemovingItem += delegate(object sender, RemovingItemEventArgs args)
			{
				removingIndex = args.Index;
			};

			factory.View.RemoveAt(0);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("bbb", list[0].StringValue);
			Assert.AreEqual(1, factory.RemovingItemCount);
			Assert.AreEqual(0, removingIndex);
		}

		[TestMethod]
		public void RemoveAtIBindingList()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IBindingList();
			IList<SimpleClass> list = factory.List;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			list.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			int removingIndex = -1;
			factory.View.RemovingItem += delegate(object sender, RemovingItemEventArgs args)
			{
				removingIndex = args.Index;
			};

			factory.View.RemoveAt(0);

			Assert.AreEqual(2, factory.List.Count);
			Assert.AreEqual("bbb", ((SimpleClass)factory.List[0]).StringValue);
			Assert.AreEqual(1, factory.RemovingItemCount);
			Assert.AreEqual(0, removingIndex);
		}

		[TestMethod]
		public void ItemGet()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			IList<SimpleClass> list = factory.List;

			list.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			SimpleClass bbb = new SimpleClass(80, "bbb", new DateTime(1980, 12, 12));
			list.Add(bbb);
			list.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			SimpleClass item = factory.View[1];
			Assert.AreEqual(bbb, item);
		}

		[TestMethod]
		public void ItemSet()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();

			factory.List.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			factory.List.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			factory.List.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			factory.View[1] = new SimpleClass(200, "ddd", new DateTime(1963, 3, 9));

			Assert.AreEqual(1, factory.ListChangedItemChangedCount);

			SimpleClass item = factory.List[1];
			Assert.AreEqual(200, item.IntegerValue);
			Assert.AreEqual("ddd", item.StringValue);
			Assert.AreEqual(new DateTime(1963, 3, 9), item.DateTimeValue);
		}

		[TestMethod]
		public void ItemSetIBindingList()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IBindingList();

			factory.List.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			factory.List.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			factory.List.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			factory.ClearEventCounts();

			factory.View[1] = new SimpleClass(200, "ddd", new DateTime(1963, 3, 9));

			Assert.AreEqual(1, factory.ListChangedItemChangedCount);

			SimpleClass item = factory.List[1];
			Assert.AreEqual(200, item.IntegerValue);
			Assert.AreEqual("ddd", item.StringValue);
			Assert.AreEqual(new DateTime(1963, 3, 9), item.DateTimeValue);
		}
	}
}
#endif
