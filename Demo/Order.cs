// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

using System;
using System.ComponentModel;

namespace Demo
{
	public class Order : INotifyPropertyChanged
	{
		private int orderId;
		private string customerId;
		private int employeeId;
		private DateTime? orderDate;
		private DateTime? requiredDate;
		private DateTime? shippedDate;
		private int shipVia;
		private Decimal? freight;
		private string shipName;
		private string shipAddress;
		private string shipCity;
		private string shipRegion;
		private string shipPostalCode;
		private string shipCountry;

		public event PropertyChangedEventHandler PropertyChanged;

		public string CustomerId
		{
			get { return customerId; }
			set
			{
				customerId = value;
				OnPropertyChanged("CustomerId");
			}
		}
		public int EmployeeId
		{
			get { return employeeId; }
			set
			{
				employeeId = value;
				OnPropertyChanged("EmployeeId");
			}
		}
		public Decimal? Freight
		{
			get { return freight; }
			set
			{
				freight = value;
				OnPropertyChanged("Freight");
			}
		}
		public DateTime? OrderDate
		{
			get { return orderDate; }
			set
			{
				orderDate = value;
				OnPropertyChanged("OrderDate");
			}
		}
		public int OrderId
		{
			get { return orderId; }
			set
			{
				orderId = value;
				OnPropertyChanged("OrderId");
			}
		}
		public DateTime? RequiredDate
		{
			get { return requiredDate; }
			set
			{
				requiredDate = value;
				OnPropertyChanged("RequiredDate");
			}
		}
		public string ShipAddress
		{
			get { return shipAddress; }
			set
			{
				shipAddress = value;
				OnPropertyChanged("ShipAddress");
			}
		}
		public string ShipCity
		{
			get { return shipCity; }
			set
			{
				shipCity = value;
				OnPropertyChanged("ShipCity");
			}
		}
		public string ShipCountry
		{
			get { return shipCountry; }
			set
			{
				shipCountry = value;
				OnPropertyChanged("ShipCountry");
			}
		}
		public string ShipName
		{
			get { return shipName; }
			set
			{
				shipName = value;
				OnPropertyChanged("ShipName");
			}
		}
		public DateTime? ShippedDate
		{
			get { return shippedDate; }
			set
			{
				shippedDate = value;
				OnPropertyChanged("ShippedDate");
			}
		}
		public string ShipPostalCode
		{
			get { return shipPostalCode; }
			set
			{
				shipPostalCode = value;
				OnPropertyChanged("ShipPostalCode");
			}
		}
		public string ShipRegion
		{
			get { return shipRegion; }
			set
			{
				shipRegion = value;
				OnPropertyChanged("ShipRegion");
			}
		}
		public int ShipVia
		{
			get { return shipVia; }
			set
			{
				shipVia = value;
				OnPropertyChanged("ShipVia");
			}
		}

		public Order()
		{
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
