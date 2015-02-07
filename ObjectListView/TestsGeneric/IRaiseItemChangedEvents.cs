// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests.Generic
{
	[TestClass]
	public class IRaiseItemChangedEventsTests
	{
		[TestMethod]
		public void IsIRaiseItemChangedEvents()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			Assert.IsTrue(factory.View is IRaiseItemChangedEvents);
		}

		[TestMethod]
		public void RaisesItemChangedEvents()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			Assert.IsTrue(((IRaiseItemChangedEvents)factory.View).RaisesItemChangedEvents);
		}
	}
}
#endif
