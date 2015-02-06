// ObjectListView
// Copyright © 2006, 2007 Jesse Johnston.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MasterDetailDemo
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}