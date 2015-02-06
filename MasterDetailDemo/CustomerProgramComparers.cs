// ObjectListView
// Copyright © 2006, 2007 Jesse Johnston.  All rights reserved.

using System;
using System.Collections;

namespace MasterDetailDemo
{
	public class CustomerProgramComparerAlpha : IComparer
	{
		public int Compare(object x, object y)
		{
			return string.Compare(x.ToString(), y.ToString());
		}
	}
	public class CustomerProgramComparerRank : IComparer
	{
		public int Compare(object x, object y)
		{
			CustomerProgram p1 = (CustomerProgram)x;
			CustomerProgram p2 = (CustomerProgram)y;

			if (p1 == p2)
				return 0;

			switch (p1)
			{
				case CustomerProgram.Platinum:
					return 1;
				case CustomerProgram.Gold:
					return (p2 == CustomerProgram.Platinum) ? -1 : 1;
				case CustomerProgram.Silver:
					return (p2 == CustomerProgram.None) ? 1 : -1;
				default:
					return -1;
			}
		}
	}
}
