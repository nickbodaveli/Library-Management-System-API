namespace Library.Application.Data
{
    public interface ICacheInvalidationService
    {
        Task InvalidateAsync(string key, CancellationToken cancellationToken = default);
        Task InvalidateByPatternAsync(string pattern, CancellationToken cancellationToken = default);
    }
}
