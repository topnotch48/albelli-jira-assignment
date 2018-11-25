namespace Albelli.Core.Contracts
{
    public class Paging
    {
	    public static Paging Default { get; } = new Paging(10, 0);

	    public uint Skip { get; set; }

		public uint Take { get; set; }

	    public bool IncludeTotalCnt { get; set; } = true;

	    public Paging() : this(Default.Take, Default.Skip)
	    {
	    }

	    public Paging(uint take, uint skip)
	    {
			this.Take = take;
		    this.Skip = skip;
	    }
	}
}
