// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

#pragma warning disable 1591	// Missing XML comment

#if TEST
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JesseJohnston.Tests
{
	[TestClass]
	public class RelationalExpressionTests
	{
		[TestMethod]
		public void Construct()
		{
			RelationalExpression exp = new RelationalExpression("prop", "value", RelationalOperator.Equal);
			Assert.AreEqual("prop", exp.PropertyName);
			Assert.AreEqual("value", exp.Value);
			Assert.AreEqual(RelationalOperator.Equal, exp.Operator);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructPropNull()
		{
			RelationalExpression exp = new RelationalExpression(null, "value", RelationalOperator.Equal);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructPropEmpty()
		{
			RelationalExpression exp = new RelationalExpression("", "value", RelationalOperator.Equal);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructVauleNull()
		{
			RelationalExpression exp = new RelationalExpression("prop", null, RelationalOperator.Equal);
		}

		[TestMethod]
		public void ConstructVauleEmpty()
		{
			RelationalExpression exp = new RelationalExpression("prop", "", RelationalOperator.Equal);
			Assert.AreEqual("prop", exp.PropertyName);
			Assert.AreEqual("", exp.Value);
			Assert.AreEqual(RelationalOperator.Equal, exp.Operator);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructOpNone()
		{
			RelationalExpression exp = new RelationalExpression("prop", "value", RelationalOperator.None);
		}
	}
}
#endif
