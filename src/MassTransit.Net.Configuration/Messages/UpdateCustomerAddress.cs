namespace MassTransit.Net.Configuration.Messages
{
	using System;
	//https://masstransit-project.com/usage/messages.html
	public interface UpdateCustomerAddress
	{
		Guid CommandId { get; }
		DateTime Timestamp { get; }
		string CustomerId { get; }
		string HouseNumber { get; }
		string Street { get; }
		string City { get; }
		string State { get; }
		string PostalCode { get; }
	}
}
