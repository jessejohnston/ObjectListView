// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.Collections;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class IRaiseItemChangedEventsTests
	{
		[TestMethod]
		public void IsIRaiseItemChangedEvents()
		{
			ObjectListView view = new ObjectListView(new ArrayList());
			Assert.IsTrue(view is IRaiseItemChangedEvents);
		}

		[TestMethod]
		public void RaisesItemChangedEvents()
		{
			ObjectListView view = new ObjectListView(new ArrayList());
			Assert.IsTrue(((IRaiseItemChangedEvents)view).RaisesItemChangedEvents);
		}
	}
}
#endif
