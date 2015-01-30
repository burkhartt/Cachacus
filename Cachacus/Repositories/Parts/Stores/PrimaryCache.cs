using System;
using System.Collections.Generic;
using System.Linq;
using Cachacus.Repositories.Parts.Stores.Helpers;

namespace Cachacus.Repositories.Parts.Stores {
	[Serializable]
	internal class PrimaryCache<T> {
		private Dictionary<object, T> cache;

		public PrimaryCache() {
			CreateCacheStorage();
		}

		public void Update(IEnumerable<T> data) {
			foreach (var item in data.Where(item => CacheManager.PrimaryKey(item) != null)) {
				cache[CacheManager.PrimaryKey(item)] = item;
			}
		}

		public bool Contains(object key) {
			return cache.ContainsKey(key);
		}

		public IEnumerable<T> GetAll() {
			return cache.Values;
		}

		public T GetByKey(object key) {
			return cache[key];
		}

		public void Initialize(IEnumerable<T> data) {
			Update(data);
		}

		public void Clear() {
			CreateCacheStorage();
		}

		private void CreateCacheStorage() {
			cache = new Dictionary<object, T>();
		}
	}
}