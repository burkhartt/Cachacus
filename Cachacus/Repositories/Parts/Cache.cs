using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cachacus.Repositories.Parts.CacheMissStrategies;
using Cachacus.Repositories.Parts.Locks;
using Cachacus.Repositories.Parts.Stores;
using Cachacus.Repositories.Parts.Stores.Strategies;

namespace Cachacus.Repositories.Parts {
	public class Cache<T> : Cached<T> where T : class, new() {
		private readonly Func<IEnumerable<T>> initialCacheFunc;
		private PrimaryCache<T> primaryCache;
		private SecondaryCaches<T> secondaryCaches;		
		private bool cacheIsBuilt = false;
		private readonly ICacheStorageStrategy cacheStorageStrategy;
		private ICacheMissStrategy<T> cacheMissStrategy;

		public Cache(Func<IEnumerable<T>> initialCacheFunc) {
			this.initialCacheFunc = initialCacheFunc;
			primaryCache = new PrimaryCache<T>();
			secondaryCaches = new SecondaryCaches<T>();			
			cacheStorageStrategy = CacheStorageStrategyManager.GetStrategy();
			cacheMissStrategy = new DefaultCacheMissStrategy<T>();
		}

		public void SetCacheMissStrategy(ICacheMissStrategy<T> cacheMissStrategy) {
			this.cacheMissStrategy = cacheMissStrategy;
		}

	    public IEnumerable<T> GetAll(Func<IEnumerable<T>> cacheMissFunc) {
			BuildCache();
			var data = CacheLock.Read(Lock, () => {
				var data1 = primaryCache.GetAll();
				return data1.Any() ? data1 : secondaryCaches.GetAll();
			});
			
			if (!data.Any()) {
				data = cacheMissStrategy.Execute(cacheMissFunc);
				Bust(data);
			}
			
			return data;
		}

	    public T GetByKey<TU>(TU key, Func<T> cacheMissAction) {
			BuildCache();
			var results = GetCachedItems(new[] {key});

			if (!results.Any()) {
				results = new[] {cacheMissStrategy.Execute(key, cacheMissAction, Lock)}.ToList();
				Bust(results);
			}

			return results.SingleOrDefault();
		}

	    public IEnumerable<T> GetAllByKeys<TU>(IEnumerable<TU> keys, Func<IEnumerable<T>> cacheMissAction) {
			if (!keys.Any()) {
				return new T[] {};
			}

			BuildCache();
			var results = GetCachedItems(keys);

			if (!results.Any()) {
				results = cacheMissStrategy.Execute(keys, cacheMissAction, Lock).ToList();
				Bust(results);
			}

			return results;
		}

		private List<T> GetCachedItems<TU>(IEnumerable<TU> keys) {
			var results = new List<T>();
			foreach (var key in keys) {
				if (primaryCache.Contains(key)) {
					results.Add(CacheLock.Read(Lock, () => primaryCache.GetByKey(key)));
				} else {
					results.AddRange(CacheLock.Read(Lock, () => secondaryCaches.Get(key)));
				}
			}
			return results;
		}

		private void UpdateCache(IEnumerable<T> data) {
			CacheLock.Write(Lock, () => {
				primaryCache.Update(data);
				secondaryCaches.Update(data);
			});
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		internal void BuildCache() {
			if (cacheIsBuilt) {
				return;
			}

			if (cacheStorageStrategy.HasExpired<T>()) {
				var data = initialCacheFunc().ToList();
				InitializeCache(data);
			} else {
				primaryCache = cacheStorageStrategy.Load<PrimaryCache<T>>("Primary");
				secondaryCaches = cacheStorageStrategy.Load<SecondaryCaches<T>>("Secondary");
			}
			
			cacheIsBuilt = true;
		}

		private void InitializeCache(IEnumerable<T> data) {
			CacheLock.Write(Lock, () => {
				primaryCache.Initialize(data);
				secondaryCaches.Initialize(data);
				cacheStorageStrategy.Save("Primary", primaryCache);
				cacheStorageStrategy.Save("Secondary", secondaryCaches);
			});
		}

		public override void Bust(T @object) {
			Bust(new[] {@object});
		}

		public override void Bust(IEnumerable<T> objects) {
			var objectsToBust = objects.ToList();
			objectsToBust.RemoveAll(x => x == null);
			if (!objectsToBust.Any()) {
				return;
			}
			UpdateCache(objectsToBust);
		}		

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Clear() {
			if (!cacheIsBuilt) {
				return;
			}

			primaryCache.Clear();
			secondaryCaches.Clear();
			cacheMissStrategy.Reset();
		}

	    public void Remove(T data) {
	        Remove(new[] {data});
	    }

	    public void Remove(IEnumerable<T> data) {
            CacheLock.Write(Lock, () => {
                primaryCache.Remove(data);
            });
	    }
	}
}