namespace Super.Paula.Guidlines
{
    public interface IInspectionManager
    {
        ValueTask<Inspection> GetAsync(string inspection);
        IQueryable<Inspection> GetQueryable();
        IAsyncEnumerable<Inspection> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspection>, IQueryable<TResult>> query);
        ValueTask InsertAsync(Inspection inspection);
        ValueTask UpdateAsync(Inspection inspection);
        ValueTask DeleteAsync(Inspection inspection);
    }
}