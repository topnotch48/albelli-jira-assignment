namespace Albelli.Orders.Contracts.Models
{
    public static class OrderConstants
    {
	    public static class OrderId
	    {
			public const string ValueFormat = @"^[A-Z]{3}\d{6,9}$";
		}

	    public static class PostCode
	    {
			public const string ValueFormat = @"^([0-9]{4} ?[a-zA-Z]{2})$";
		}

	    public static class CustomerName
	    {
		    public const int MaxLength = 60;
		    public const int MinLength = 2;
	    }

	    public static class HouseNumber
	    {
		    public const uint Max = 99999;
		    public const uint Min = 1;
	    }
    }
}
