using CoordinateTestApp.Data.Exceptions;
using Newtonsoft.Json;
using System.IO;

namespace CoordinateTestApp.Services
{
	public class FileCacheCreator<T> : ICacheCreator<T>
	{
		private const int MaxObjectsInFile = 5;
		private const string AppDir = "CoordinateTestApp";
		private const string CacheDir = "cache";		
		protected readonly string pathToCacheDir;
		protected readonly DirectoryInfo cacheDirectory;		

		public FileCacheCreator()
		{			
			pathToCacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDir, CacheDir);
			cacheDirectory = Directory.CreateDirectory(pathToCacheDir);
		}

		public async Task WriteAsync(IEnumerable<T> collection, CancellationToken token = default)
		{
			int cacheFilesCount = (int)Math.Ceiling((double)collection.Count() / MaxObjectsInFile);
			IEnumerable<string> paths = CreateCacheFiles(cacheFilesCount);
			List<T> objs = new List<T>();
			int fileIndex = 0;
			for (int i = 1; i <= collection.Count(); i++)
			{
				objs.Add(collection.ElementAt(i-1));
				if (i % MaxObjectsInFile == 0 || i == collection.Count())
				{
					await WriteObjectsToCache(objs, paths.ElementAt(fileIndex), token);
					fileIndex++;
					objs.Clear();
				}
				
			}
		}

		private async Task WriteObjectsToCache(IEnumerable<T> collection, string cachePath, CancellationToken token = default)
		{
			string data = JsonConvert.SerializeObject(collection);
			await File.WriteAllTextAsync(cachePath, data, token);
		}

		public async Task<IEnumerable<T>> ReadAsync(CancellationToken token = default)
		{
			IEnumerable<string> paths = GetCacheFilesPath();
			List<T> values = new List<T>();
			foreach (string path in paths)
			{		
				values.AddRange(await ReadPointsFromCacheAsync(path, token));
			}

			return values;
		}

		private async Task<IEnumerable<T>> ReadPointsFromCacheAsync(string cachePath, CancellationToken token = default)
		{
			try
			{
				string data = await File.ReadAllTextAsync(cachePath, token);
				return JsonConvert.DeserializeObject<List<T>>(data);
			}
			catch (JsonReaderException)
			{
				return new List<T>();
			}

		}

		private IEnumerable<string> CreateCacheFiles(int count)
		{
			return CreateFilesInDirectory(cacheDirectory, count);
		}

		private IEnumerable<string> CreateFilesInDirectory(DirectoryInfo directory, int count)
		{
			List<string> files = directory.GetFiles().Select(f => f.FullName).ToList();
			if (files.Count < count)
			{
				files.AddRange(CreateUniqueFiles(count - files.Count));
			}

			return files;
		}

		private IEnumerable<string> CreateUniqueFiles(int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return CreateUniqueFile();
			}
		}

		private string CreateUniqueFile()
		{
			string pathToFile = Path.Combine(pathToCacheDir, Guid.NewGuid().ToString());			
			File.Create(pathToFile).Close();
			return pathToFile;
		}

		private IEnumerable<string> GetCacheFilesPath()
		{			
			return GetFilesPathInDirectory(cacheDirectory);
		}		

		private IEnumerable<string> GetFilesPathInDirectory(DirectoryInfo directory)
		{
			return directory.GetFiles().Select(x => x.FullName);
		}

		public Task DeleteAsync(CancellationToken token = default)
		{
			return Task.Run(() =>
			{
				foreach (FileInfo file in cacheDirectory.GetFiles())
				{
					token.ThrowIfCancellationRequested();
					file.Delete();
				}
			}, token);
        }
	}
}
