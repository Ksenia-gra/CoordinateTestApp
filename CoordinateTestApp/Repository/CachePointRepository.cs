using CoordinateTestApp.Data.Models;
using CoordinateTestApp.Services;

namespace CoordinateTestApp.Repository
{
	public class CachePointRepository : IPointRepository
	{
		private readonly ICacheCreator<Point> cacheCreator;

		public CachePointRepository(ICacheCreator<Point> cacheCreator)
		{
			this.cacheCreator = cacheCreator;
		}

		public async Task InsertAsync(IEnumerable<Point> point, CancellationToken token = default)
		{
			await cacheCreator.WriteAsync(point, token);
		}

		public async Task DeleteAsync(int id, CancellationToken token = default)
		{
			await DeleteAsync(token);
		}

		public async Task DeleteAsync(CancellationToken token = default)
		{
			await cacheCreator.DeleteAsync(token);
		}

		public async Task<IEnumerable<Point>> GetAllAsync(CancellationToken token = default)
		{
			return await cacheCreator.ReadAsync(token);
		}

		public async Task<Point> GetByIdAsync(int id, CancellationToken token = default)
		{
			IEnumerable<Point> points = await cacheCreator.ReadAsync(token);
			return points.FirstOrDefault(p => p.Id == id);
		}

		public Task UpdateAsync(Point point, CancellationToken token = default)
		{
			return Task.CompletedTask;
		}
	}
}
