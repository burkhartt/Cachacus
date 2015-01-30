using System;
using System.Collections.Generic;
using GAT.Domain.Cache.Utilities;

namespace GAT.Domain.Cache.Repositories.Parts.Stores {
	[Serializable]
	internal class NoDataCache {
		private HashSet<object> cache;

		public NoDataCache() {
			CreateCacheStorage();
		}

		public void Update<TU>(IEnumerable<TU> keys) {
			keys.ForEach(k => cache.Add(k));
		}

		public bool Contains<TU>(TU key) {
			return cache.Contains(key);
		}

		public void Clear() {
			CreateCacheStorage();
		}

		private void CreateCacheStorage() {
			cache = new HashSet<object>();
		}
	}
}