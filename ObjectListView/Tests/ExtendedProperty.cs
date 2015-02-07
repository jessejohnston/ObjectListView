// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class ExtendedPropertyTests
	{
		[TestMethod]
		public void Construct()
		{
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total");
			Assert.IsNull(prop.Default);
			Assert.AreEqual("Total", prop.Name);
			Assert.IsNotNull(prop.PropertyDescriptor);
			Assert.AreEqual(typeof(string), prop.PropertyType);
			Assert.IsTrue(prop.IsReadOnly);
		}

		[TestMethod]
		public void ConstructTyped()
		{
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(int));
			Assert.IsNull(prop.Default);
			Assert.AreEqual("Total", prop.Name);
			Assert.IsNotNull(prop.PropertyDescriptor);
			Assert.AreEqual(typeof(int), prop.PropertyType);
			Assert.IsTrue(prop.IsReadOnly);
		}

		[TestMethod]
		public void ConstructWritable()
		{
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(int), true, null);
			Assert.IsNull(prop.Default);
			Assert.AreEqual("Total", prop.Name);
			Assert.IsNotNull(prop.PropertyDescriptor);
			Assert.AreEqual(typeof(int), prop.PropertyType);
			Assert.IsFalse(prop.IsReadOnly);
		}

		[TestMethod]
		public void ConstructDefaultValue()
		{
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(int), true, 42);
			Assert.AreEqual(42, prop.Default);
			Assert.AreEqual("Total", prop.Name);
			Assert.IsNotNull(prop.PropertyDescriptor);
			Assert.AreEqual(typeof(int), prop.PropertyType);
			Assert.IsFalse(prop.IsReadOnly);
		}

		[TestMethod]
		public void GetValue()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total");

			int callbacks = 0;
			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				callbacks++;
				Assert.AreEqual("Total", args.PropertyName);
				Assert.AreSame(item, args.Target);
				Assert.AreEqual(null, args.Value);

				args.Value = string.Format("{0}: {1}", ((SimpleClass)args.Target).StringValue, ((SimpleClass)args.Target).IntegerValue);
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);

			Assert.AreEqual(1, callbacks);
			Assert.IsNotNull(propValue);
			Assert.AreEqual(typeof(string), propValue.GetType());
			Assert.AreEqual("Orders: 42", propValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetValueWrongTypeReferenceType()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(string), true, null);

			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				args.Value = 42;
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetValueWrongTypeValueType()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(DateTime), true, null);

			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				args.Value = 42;
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetValueNullValueType()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(int), true, null);

			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				args.Value = null;
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);
		}

		[TestMethod]
		public void GetValueNullNullableValueType()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(int?), true, null);

			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				args.Value = null;
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);

			Assert.IsNull(propValue);
		}

		[TestMethod]
		public void GetValueNullReferenceType()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(string), true, null);

			int callbacks = 0;
			prop.GetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				callbacks++;
				args.Value = null;
			};

			object propValue = prop.PropertyDescriptor.GetValue(item);

			Assert.IsNull(propValue);
		}

		[TestMethod]
		public void SetValue()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total", typeof(string), true, null);

			int callbacks = 0;
			prop.SetValue += delegate(object sender, ExtendedPropertyEventArgs args)
			{
				callbacks++;
				Assert.AreEqual("Total", args.PropertyName);
				Assert.AreSame(item, args.Target);
				Assert.IsNotNull(args.Value);
				Assert.AreEqual(typeof(string), args.Value.GetType());
				Assert.AreEqual("Orders: 57", args.Value);
			};

			prop.PropertyDescriptor.SetValue(item, "Orders: 57");

			Assert.AreEqual(1, callbacks);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SetValueReadOnly()
		{
			SimpleClass item = new SimpleClass(42, "Orders", DateTime.Now);
			ExtendedProperty prop = new ExtendedProperty(typeof(SimpleClass), "Total");
			prop.PropertyDescriptor.SetValue(item, "Orders: 57");
		}
	}
}
#endif

