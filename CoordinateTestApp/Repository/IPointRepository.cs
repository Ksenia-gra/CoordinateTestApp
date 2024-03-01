using CoordinateTestApp.Data.Models;

namespace CoordinateTestApp.Repository
{
	public interface IPointRepository
    {
		Task<IEnumerable<Point>> GetAllAsync(CancellationToken token = default);

		Task<Point> GetByIdAsync(int id, CancellationToken token = default);

		Task InsertAsync(IEnumerable<Point> point, CancellationToken token = default);

		Task UpdateAsync(Point point, CancellationToken token = default);

		Task DeleteAsync(int id, CancellationToken token = default);

		Task DeleteAsync(CancellationToken token = default);

	}
}
