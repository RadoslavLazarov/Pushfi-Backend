namespace Pushfi.Domain.Models
{
	public class PageResult<T>
		where T : class
	{
		public PageResult()
		{
			Items = new List<T>();
		}

		public ICollection<T> Items { get; set; }

		public int TotalCount { get; set; }
	}
}