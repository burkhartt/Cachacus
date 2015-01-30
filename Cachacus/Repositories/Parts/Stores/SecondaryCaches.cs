using System;
using System.Collections.Generic;
using System.Linq;
using Cachacus.Repositories.Parts.Stores.Helpers;
using Cachacus.Utilities;

namespace Cachacus.Repositories.Parts.Stores {
	[Serializable]
	internal class SecondaryCaches<T> {
		private readonly Dictionary<string, SecondaryCache<T>> secondaryCaches = new Dictionary<string, SecondaryCache<T>>();

		public SecondaryCaches() {
			CreateCacheStorage();
		}

		public IEnumerable<T> Get(object key) {
			return secondaryCaches.Values.SelectMany(c => c.GetAllByKey(key));
		}

		public void Update(IEnumerable<T> data) {
			secondaryCaches.Values.ForEach(s => s.Update(data));
		}

		public IEnumerable<T> GetAllByValue(string indexName, object value) {
			return !secondaryCaches.ContainsKey(indexName) ? new T[] { } : secondaryCaches[indexName].GetAllByKey(value);
		}

		public IEnumerable<T> GetAll() {
			return secondaryCaches.Any() ? secondaryCaches.First().Value.GetAll() : new T[] { };
		}

		public void Initialize(IEnumerable<T> data) {
			secondaryCaches.Values.ForEach(s => s.Initialize(data));
		}

		public void Clear() {
			CreateCacheStorage();
		}

		private void CreateCacheStorage() {
			var secondaryKeys = CacheManager.IndexNames<T>();
			foreach (var secondaryKey in secondaryKeys) {
				secondaryCaches[secondaryKey] = new SecondaryCache<T>(secondaryKey);
			}
		}
	}
}