// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests.Generic
{
	[TestClass]
	public class ITypedListTests
	{
		[TestMethod]
		public void GetListName()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			Assert.IsTrue(view is ITypedList);
			Assert.AreEqual("", ((ITypedList)view).GetListName(null));
		}

		[TestMethod]
		public void GetListNameAccessors()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			// Get some random property descriptors.
			PropertyDescriptorCollection accessors = TypeDescriptor.GetProperties(typeof(ISite));
			PropertyDescriptor[] listAccessors = new PropertyDescriptor[accessors.Count];
			accessors.CopyTo(listAccessors, 0);

			Assert.AreEqual("", ((ITypedList)view).GetListName(listAccessors));
		}

		[TestMethod]
		public void GetItemProperties()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

			PropertyDescriptorCollection props = ((ITypedList)view).GetItemProperties(null);
			PropertyDescriptorCollection typeProps = TypeDescriptor.GetProperties(typeof(SimpleClass));
			Assert.AreEqual(typeProps, props);
		}

		[TestMethod]
		public void GetItemPropertiesAccessors()
		{
			ViewFactory<SimpleClass> factory = ViewFactory<SimpleClass>.IList();
			ObjectListView<SimpleClass> view = factory.View;

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
