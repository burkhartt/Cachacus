using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GAT.Domain.Cache.Configuration;

namespace GAT.Domain.Cache.Repositories.Parts.Stores.Strategies {
	internal class FileSystemStorageStrategy : ICacheStorageStrategy {
		private readonly string path;
		private readonly TimeSpan cacheWindow;

		public FileSystemStorageStrategy(CacheConfiguration configuration) {
			path = configuration.Path;			
			cacheWindow = new TimeSpan(0, 0, int.Parse(configuration.ExpirationInMinutes), 0, 0);
		}

		public void Save(string key, object obj) {
			new FileInfo(GetBasePathForObject(obj.GetType())).Directory.Create();
			var binaryFormatter = new BinaryFormatter();
			var writer = new FileStream(GetCacheStorageFilePath(key, obj.GetType()), FileMode.Create);
			binaryFormatter.Serialize(writer, obj);
			writer.Close();
			var timestampFile = new StreamWriter(GetTimestampFilePath(obj.GetType()));
			timestampFile.Write(DateTime.Now);
			timestampFile.Close();
		}

		public T Load<T>(string key) {
			try {
				var serializer = new BinaryFormatter();
				var reader = new FileStream(GetCacheStorageFilePath(key, typeof(T)), FileMode.OpenOrCreate);
				if (reader.Length < 1) {
					return default(T);
				}
				return (T) serializer.Deserialize(reader);
			} catch (Exception) {
				return default(T);
			}
		}

		private string GetBasePathForObject(Type objectType) {
			var folder = objectType.IsGenericType ? objectType.GenericTypeArguments[0].Name : objectType.Name;
			return string.Format("{0}{1}\\", path, folder);
		}

		private string GetCacheStorageFilePath(string key, Type objectType) {
			return GetBasePathForObject(objectType) + key + ".bak";
		}

		private string GetTimestampFilePath(Type objectType) {
			return GetBasePathForObject(objectType) + "cachetimestamp.txt";
		}

		public bool HasExpired<T>() {
			try {
				var reader = new StreamReader(GetTimestampFilePath(typeof(T)));
				var timestamp = DateTime.Parse(reader.ReadLine());
				return DateTime.Now.Subtract(timestamp) > cacheWindow;
			} catch (Exception) {
				return true;
			}
		}
	}
}