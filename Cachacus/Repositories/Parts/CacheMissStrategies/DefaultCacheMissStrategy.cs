using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cachacus.Repositories.Parts.Locks;
using Cachacus.Repositories.Parts.Stores;

namespace Cachacus.Repositories.Parts.CacheMissStrategies {
	public class DefaultCacheMissStrategy<T> : ICacheMissStrategy<T> {
		private readonly NoDataCache noDataCache;

		public DefaultCacheMissStrategy() {
			noDataCache = new NoDataCache();
		}

		public T Execute<TU>(TU key, Func<T> cacheMissFunc, ReaderWriterLockSlim @lock) {
			if (noDataCache.Contains(key)) {
				return default(T);
			}

			var newData = cacheMissFunc();
			if (newData == null) {
				AddKeysToNoDataCache(new[] { key }, @lock);
			}

			return newData;
		}

		public IEnumerable<T> Execute(Func<IEnumerable<T>> cacheMissFunc) {
			return cacheMissFunc();
		}

		public IEnumerable<T> Execute<TU>(IEnumerable<TU> keys, Func<IEnumerable<T>> cacheMissFunc, ReaderWriterLockSlim @lock) {
			if (keys.All(k => noDataCache.Contains(k))) {
				return new T[] {};
			}

			var newData = cacheMissFunc().ToList();
			if (!newData.Any()) {
				AddKeysToNoDataCache(keys, @lock);
			}

			return newData;
		}

		public void Reset() {
			noDataCache.Clear();
		}

		private void AddKeysToNoDataCache<TU>(IEnumerable<TU> keys, ReaderWriterLockSlim @lock) {
			var filteredKeys = keys.ToList();
			filteredKeys.RemoveAll(k => noDataCache.Contains(k));
			CacheLock.Write(@lock, () => noDataCache.Update(filteredKeys));
		}
	}
}