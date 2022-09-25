namespace Pushfi.Domain.Models
{
    public class PageModel
    {
        public PageModel()
        {
            this.Sorts = new List<SortsModel>();
        }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public List<SortsModel> Sorts { get; set; }
    }
}
