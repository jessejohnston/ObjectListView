// ObjectListView
// Copyright © 2006-2015 Jesse Johnston.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Demo
{
	public class Database
	{
		private string connectionString;

		public string ConnectionString
		{
			get
			{
				if (connectionString == null)
					connectionString = ConfigurationManager.ConnectionStrings["Demo.Properties.Settings.NorthwindConnectionString"].ConnectionString;
				return connectionString;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("ConnectionString");
				this.connectionString = value;
			}
		}

		public List<Customer> GetCustomers()
		{
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				conn.Open();

				List<Customer> list = new List<Customer>();

				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax FROM Customers";
				cmd.CommandType = System.Data.CommandType.Text;
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Customer c = new Customer();
						c.Id = reader.GetString(0);
						c.Company = reader.GetString(1);
						c.ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2);
						c.ContactTitle = reader.IsDBNull(3) ? "" : reader.GetString(3);
						c.Address = reader.IsDBNull(4) ? "" : reader.GetString(4);
						c.City = reader.IsDBNull(5) ? "" : reader.GetString(5);
						c.Region = reader.IsDBNull(6) ? "" : reader.GetString(6);
						c.ZipCode = reader.IsDBNull(7) ? "" : reader.GetString(7);
						c.Country = reader.IsDBNull(8) ? "" : reader.GetString(8);
						c.Phone = reader.IsDBNull(9) ? "" : reader.GetString(9);
						c.Fax = reader.IsDBNull(10) ? "" : reader.GetString(10);
						list.Add(c);
					}
				}

				return list;
			}
		}
		public List<Order> GetOrders()
		{
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				conn.Open();

				List<Order> list = new List<Order>();

				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "SELECT OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry FROM Orders";
				cmd.CommandType = System.Data.CommandType.Text;
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Order o = new Order();

						o.OrderId = reader.GetInt32(0);
						o.CustomerId = reader.IsDBNull(1) ? "" : reader.GetString(1);
						o.EmployeeId = reader.GetInt32(2);
						o.OrderDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
						o.RequiredDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
						o.ShippedDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
						o.ShipVia = reader.GetInt32(6);
						o.Freight = reader.IsDBNull(7) ? (Decimal?)null : reader.GetDecimal(7);
						o.ShipName = reader.IsDBNull(8) ? "" : reader.GetString(8);
						o.ShipAddress = reader.IsDBNull(9) ? "" : reader.GetString(9);
						o.ShipCity = reader.IsDBNull(10) ? "" : reader.GetString(10);
						o.ShipRegion = reader.IsDBNull(11) ? "" : reader.GetString(11);
						o.ShipPostalCode = reader.IsDBNull(12) ? "" : reader.GetString(12);
						o.ShipCountry = reader.IsDBNull(13) ? "" : reader.GetString(13);

						list.Add(o);
					}
				}

				return list;
			}
		}
	}
}
