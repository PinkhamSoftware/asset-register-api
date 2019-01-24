namespace HomesEngland.Domain
{
    public interface IPagedQuery 
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}
