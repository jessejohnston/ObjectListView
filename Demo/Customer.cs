// ObjectListView
// Copyright © 2006, 2007, Jesse Johnston.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Demo
{
	public class Customer : INotifyPropertyChanged
	{
		private string id;
		private string company;
		private string contactName;
		private string contactTitle;
		private string address;
		private string city;
		private string region;
		private string zipcode;
		private string country;
		private string phone;
		private string fax;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Address
		{
			get { return address; }
			set
			{
				address = value;
				OnPropertyChanged("Address");
			}
		}
		public string City
		{
			get { return city; }
			set
			{
				city = value;
				OnPropertyChanged("City");
			}
		}
		public string Company
		{
			get { return company; }
			set
			{
				company = value;
				OnPropertyChanged("Company");
			}
		}
		public string ContactName
		{
			get { return contactName; }
			set
			{
				contactName = value;
				OnPropertyChanged("ContactName");
			}
		}
		public string ContactTitle
		{
			get { return contactTitle; }
			set
			{
				contactTitle = value;
				OnPropertyChanged("ContactTitle");
			}
		}
		public string Country
		{
			get { return country; }
			set
			{
				country = value;
				OnPropertyChanged("Country");
			}
		}
		public string Fax
		{
			get { return fax; }
			set
			{
				fax = value;
				OnPropertyChanged("Fax");
			}
		}
		public string Id
		{
			get { return id; }
			set
			{
				id = value;
				OnPropertyChanged("Id");
			}
		}
		public string Phone
		{
			get { return phone; }
			set
			{
				phone = value;
				OnPropertyChanged("Phone");
			}
		}
		public string Region
		{
			get { return region; }
			set
			{
				region = value;
				OnPropertyChanged("Region");
			}
		}
		public string ZipCode
		{
			get { return zipcode; }
			set
			{
				zipcode = value;
				OnPropertyChanged("ZipCode");
			}
		}

		public Customer()
		{
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
