namespace HomesEngland.Domain
{
    public class PagedQuery:IPagedQuery
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public PagedQuery()
        {
            Page = 1;
            PageSize = 25;
        }
    }
}
