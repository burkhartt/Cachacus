using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cachacus.Repositories.Parts;

namespace Cachacus.Repositories {
	public abstract class AbstractCacheRepository<T> : ICacheRepository<T>, IInitializable where T : class, new() {
		protected readonly Cache<T> Cache;
		
		protected AbstractCacheRepository() {
			this.Cache = new Cache<T>(WarmUp);
		}

		protected IEnumerable<T> GetAll(Func<IEnumerable<T>> cacheMissAction) {
			return Cache.GetAll(cacheMissAction).ToList();			
		}

		protected T GetByKey<T2>(T2 key, Func<T> cacheMissAction) {
			return Cache.GetByKey(key, cacheMissAction);			
		}

		protected IEnumerable<T> GetAllByKey<T2>(T2 key, Func<IEnumerable<T>> cacheMissAction) {
			return GetAllByKeys(new[] {key}, cacheMissAction);
		}

		protected IEnumerable<T> GetAllByKeys<T2>(IEnumerable<T2> keys, Func<IEnumerable<T>> cacheMissAction) {
			return Cache.GetAllByKeys(keys, cacheMissAction).ToList();
		}		

		protected abstract IEnumerable<T> WarmUp();

		public void Bust(T newData) {
			Bust(new[] {newData});
		}

		public void Bust(IEnumerable<T> newData) {
			Cache.Bust(newData);
		}

		protected void ClearCache() {
			Cache.Clear();
		}

	    protected void RecycleCache() {
	        new Thread(() => {
	            ClearCache();
	            Cache.BuildCache();
	        }).Start();
	    }

		public void InitializeAsync() {
			new Thread(Cache.BuildCache).Start();
		}
	}	
}