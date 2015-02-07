// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class ITypedListTests
	{
		[TestMethod]
		public void GetListName()
		{
			List<SimpleClass> list = new List<SimpleClass>();
			ObjectListView view = new ObjectListView(list);

			Assert.IsTrue(view is ITypedList);
			Assert.AreEqual("", ((ITypedList)view).GetListName(null));
		}

		[TestMethod]
		public void GetListNameAccessors()
		{
			List<SimpleClass> list = new List<SimpleClass>();
			ObjectListView view = new ObjectListView(list);

			// Get some random property descriptors.
			PropertyDescriptorCollection accessors = TypeDescriptor.GetProperties(typeof(ISite));
			PropertyDescriptor[] listAccessors = new PropertyDescriptor[accessors.Count];
			accessors.CopyTo(listAccessors, 0);

			Assert.AreEqual("", ((ITypedList)view).GetListName(listAccessors));
		}

		[TestMethod]
		public void GetItemProperties()
		{
			List<SimpleClass> list = new List<SimpleClass>();
			ObjectListView view = new ObjectListView(list);

			PropertyDescriptorCollection props = ((ITypedList)view).GetItemProperties(null);
			PropertyDescriptorCollection typeProps = TypeDescriptor.GetProperties(typeof(SimpleClass));
			Assert.AreEqual(typeProps, props);
		}

		[TestMethod]
		public void GetItemPropertiesAccessors()
		{
			List<SimpleClass> list = new List<SimpleClass>();
			ObjectListView view = new ObjectListView(list);

			// Get some random property descriptors.
			PropertyDescriptorCollection accessors = TypeDescriptor.GetProperties(typeof(ISite));
			PropertyDescriptor[] listAccessors = new PropertyDescriptor[accessors.Count];
			accessors.CopyTo(listAccessors, 0);

			PropertyDescriptorCollection props = ((ITypedList)view).GetItemProperties(listAccessors);
			PropertyDescriptorCollection typeProps = TypeDescriptor.GetProperties(typeof(SimpleClass));
			Assert.AreEqual(typeProps, props);
		}
	}
}
#endif
