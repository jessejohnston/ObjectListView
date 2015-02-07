// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class IEnumerableTests
	{
		private List<SimpleClass> classList;

		[TestInitialize]
		public void Setup()
		{
			classList = new List<SimpleClass>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetEnumeratorNullList()
		{
			ObjectListView view = new ObjectListView(null);
		}

		[TestMethod]
		public void GetEnumeratorEmptyList()
		{
			ObjectListView view = new ObjectListView(classList);
			IEnumerator e = view.GetEnumerator();
			Assert.IsNotNull(e);
			Assert.IsFalse(e.MoveNext());
		}

		[TestMethod]
		public void GetEnumeratorNonEmptyList()
		{
			classList.Add(new SimpleClass(100, "aaa", new DateTime(1970, 1, 1)));
			classList.Add(new SimpleClass(80, "bbb", new DateTime(1980, 12, 12)));
			classList.Add(new SimpleClass(60, "ccc", new DateTime(1975, 6, 6)));

			ObjectListView view = new ObjectListView(classList);
			IEnumerator e = view.GetEnumerator();
			Assert.IsNotNull(e);

			Assert.IsTrue(e.MoveNext());
			object item = e.Current;
			Assert.IsNotNull(item);
			Assert.IsTrue(item is SimpleClass);
			Assert.AreEqual(100, ((SimpleClass)item).IntegerValue);
			Assert.AreEqual("aaa", ((SimpleClass)item).StringValue);
			Assert.AreEqual(new DateTime(1970, 1, 1), ((SimpleClass)item).DateTimeValue);

			Assert.IsTrue(e.MoveNext());
			item = e.Current;
			Assert.IsNotNull(item);
			Assert.IsTrue(item is SimpleClass);
			Assert.AreEqual(80, ((SimpleClass)item).IntegerValue);
			Assert.AreEqual("bbb", ((SimpleClass)item).StringValue);
			Assert.AreEqual(new DateTime(1980, 12, 12), ((SimpleClass)item).DateTimeValue);

			Assert.IsTrue(e.MoveNext());
			item = e.Current;
			Assert.IsNotNull(item);
			Assert.IsTrue(item is SimpleClass);
			Assert.AreEqual(60, ((SimpleClass)item).IntegerValue);
			Assert.AreEqual("ccc", ((SimpleClass)item).StringValue);
			Assert.AreEqual(new DateTime(1975, 6, 6), ((SimpleClass)item).DateTimeValue);

			Assert.IsFalse(e.MoveNext());
		}
	}
}
#endif
