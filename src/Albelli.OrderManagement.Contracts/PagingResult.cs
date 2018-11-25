using System.Collections.Generic;

namespace Albelli.Core.Contracts
{
    public class PagingResult<TItem> where TItem : class
    {
		public uint Skipped { get; set; }

		public uint Take { get; set; }

		public uint? TotalCount { get; set; }

		public IList<TItem> Items { get; set; }

	    public PagingResult()
	    {
	    }

	    public PagingResult(uint skipped, uint take, IList<TItem> items)
	    {
		    this.Skipped = skipped;
		    this.Take = take;
			this.Items = new List<TItem>(items);
	    }

	    public PagingResult(uint skipped, uint take, IList<TItem> items, uint? total) : this(skipped, take, items)
	    {
		    this.TotalCount = total;
	    }
	}
}
