using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Albelli.Common.Tests.Data
{
	public class InvalidOrderIds : DataAttribute
	{
		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			var orderIds = new[]
			{
				new [] { "ABC_24124124" },
				new [] { "SAL12345678910" },
				new [] { "ABCD327327" }
			};

			return orderIds;
		}
	}
}
