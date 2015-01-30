using System;
using System.Collections.Generic;
using System.Linq;
using GAT.Domain.Cache.Repositories.Parts.Stores.Helpers;

namespace GAT.Domain.Cache.Repositories.Parts.Stores {
	[Serializable]
	internal class SecondaryCache<T> {
		private readonly string index;
		private readonly Dictionary<object, List<T>> cache;

		public SecondaryCache(string index) {
			this.index = index;
			cache = new Dictionary<object, List<T>>();
		}

		public void Update(IEnumerable<T> data) {
			foreach (var item in data) {
				Update(item);
			}
		}

		private void Update(T item) {
			List<T> list;			
			var key = CacheManager.KeyForIndex(index, item);
			if (key == null) { // in case the index has no value
				return; 
			}			
			if (!cache.TryGetValue(key, out list)) {
				cache[key] = new List<T> { item }; 
				return;
			}

			list = new List<T>(list); // Clone list fot thread safety

			// Remove any existing objects with same Key (id)
			var existingItem = list.FirstOrDefault(i => CacheManager.IsSame(item, i));
			if (existingItem != null) {
				list.Remove(existingItem);
			}

			list.Add(item);
			cache[key] = list;
		}

		public IEnumerable<T> GetAllByKey(object key) {
			if (!cache.ContainsKey(key)) {
				return new T[] { };
			}

			return cache[key];
		}

		// XXX ?!?
		public IEnumerable<T> GetAll() {
			return cache.Values.First().Select(d => d);
		}

		public void Initialize(IEnumerable<T> data) {
			foreach (var item in data) {
				var key = CacheManager.KeyForIndex(index, item);
				if (key == null) {
					continue;
				}
				if (!cache.ContainsKey(key)) {
					cache[key] = new List<T>();
				}
				cache[key].Add(item);
			}
		}
	}
}