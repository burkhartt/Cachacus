using System;
using System.Collections.Generic;
using System.Linq;
using Cachacus.Repositories.Parts;
using Cachacus.Repositories.Parts.CacheMissStrategies;

namespace Cachacus.Repositories {
	public class CacheDependency<T> : ICacheDependency<T> where T : class, new() {
		protected Cache<T> Cache;
		private volatile bool initialized;
		private readonly object initializationLock = new object();
		private ICacheMissStrategy<T> cacheMissStrategy;

		public CacheDependency() {
			Identifier = Guid.NewGuid().ToString();
		}

		public IEnumerable<T> GetAll(Func<IEnumerable<T>> cacheMissAction) {
			return Cache.GetAll(cacheMissAction).ToList();
		}

		public T GetByKey<T2>(T2 key, Func<T> cacheMissAction) {
			return Cache.GetByKey(key, cacheMissAction);			
		}

		public IEnumerable<T> GetAllByKey<T2>(T2 key, Func<IEnumerable<T>> cacheMissAction) {
			return GetAllByKeys(new[] {key}, cacheMissAction);
		}

		public IEnumerable<T> GetAllByKeys<T2>(IEnumerable<T2> keys, Func<IEnumerable<T>> cacheMissAction) {
			return Cache.GetAllByKeys(keys, cacheMissAction).ToList();
		}		

		public void Bust(T newData) {
			Bust(new[] {newData});
		}

		public void Bust(IEnumerable<T> newData) {
			Cache.Bust(newData);
		}

		public void RecycleCache(Func<IList<T>> warmUpFunction) {
			lock (initializationLock) {
				initialized = false;
				FillCache(warmUpFunction);
			}
		}

		public string Identifier { get; private set; }

		public void Initialize(Func<IList<T>> warmupFunction, ICacheMissStrategy<T> cacheMissStrategy) {
			this.cacheMissStrategy = cacheMissStrategy;
			FillCache(warmupFunction);
		}


		private void FillCache(Func<IList<T>> warmupFunction) {
			if (!initialized) {
				lock (initializationLock) {
					if (!initialized) {
						Cache = new Cache<T>(warmupFunction);
						Cache.BuildCache();
						if (cacheMissStrategy != null) {
							Cache.SetCacheMissStrategy(cacheMissStrategy);
						}
						initialized = true;
					}
				}
			}
		}
	}
}
