namespace CoordinateTestApp.Services
{
	public interface ICacheCreator<T>
	{
		Task WriteAsync(IEnumerable<T> collection, CancellationToken token = default);
		Task<IEnumerable<T>> ReadAsync(CancellationToken token = default);
		Task DeleteAsync(CancellationToken token = default);
	}
}
