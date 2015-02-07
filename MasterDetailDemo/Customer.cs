// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

using System;
using System.ComponentModel;

namespace MasterDetailDemo
{
	[Serializable]
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
		private CustomerProgram program;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Address
		{
			get { return address; }
			set
			{
				if (address != value)
				{
					address = value;
					OnPropertyChanged("Address");
				}
			}
		}
		public string City
		{
			get { return city; }
			set
			{
				if (city != value)
				{
					city = value;
					OnPropertyChanged("City");
				}
			}
		}
		public string Company
		{
			get { return company; }
			set
			{
				if (company != value)
				{
					company = value;
					OnPropertyChanged("Company");
				}
			}
		}
		public string ContactName
		{
			get { return contactName; }
			set
			{
				if (contactName != value)
				{
					contactName = value;
					OnPropertyChanged("ContactName");
				}
			}
		}
		public string ContactTitle
		{
			get { return contactTitle; }
			set
			{
				if (contactTitle != value)
				{
					contactTitle = value;
					OnPropertyChanged("ContactTitle");
				}
			}
		}
		public string Country
		{
			get { return country; }
			set
			{
				if (country != value)
				{
					country = value;
					OnPropertyChanged("Country");
				}
			}
		}
		public string Fax
		{
			get { return fax; }
			set
			{
				if (fax != value)
				{
					fax = value;
					OnPropertyChanged("Fax");
				}
			}
		}
		public string Id
		{
			get { return id; }
			set
			{
				if (id != value)
				{
					id = value;
					OnPropertyChanged("Id");
				}
			}
		}
		public string Phone
		{
			get { return phone; }
			set
			{
				if (phone != value)
				{
					phone = value;
					OnPropertyChanged("Phone");
				}
			}
		}
		public CustomerProgram Program
		{
			get { return program; }
			set
			{
				if (program != value)
				{
					program = value;
					OnPropertyChanged("Program");
				}
			}
		}
		public string Region
		{
			get { return region; }
			set
			{
				if (region != value)
				{
					region = value;
					OnPropertyChanged("Region");
				}
			}
		}
		public string ZipCode
		{
			get { return zipcode; }
			set
			{
				if (zipcode != value)
				{
					zipcode = value;
					OnPropertyChanged("ZipCode");
				}
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
