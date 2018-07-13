using System;

namespace Vita.Contracts
{
	public class Customer : ValueObject
	{
		public Name Name { get; set; }
		public Address Address { get; set; }
		public string Phone { get; set; }
		public EmailAddress Email { get; set; }
		public DateTime Dob { get; set; }
		public Income Income { get; set; }

		public Customer(Name name, Address address, string phone, EmailAddress email,  DateTime dob, Income income)
		{
			Name = name;
			Address = address;
			Phone = phone;
			Email = email;
			Dob = dob;
			Income = income;
		}
	}
}