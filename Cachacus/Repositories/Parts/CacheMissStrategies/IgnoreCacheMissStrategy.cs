using System;
using System.Collections.Generic;
using System.Threading;

namespace GAT.Domain.Cache.Repositories.Parts.CacheMissStrategies {
	public class IgnoreCacheMissStrategy<T> : ICacheMissStrategy<T> {
		public IEnumerable<T> Execute(Func<IEnumerable<T>> cacheMissFunc) {
			return new T[] {};
		}

		public IEnumerable<T> Execute<TU>(IEnumerable<TU> keys, Func<IEnumerable<T>> cacheMissFunc, ReaderWriterLockSlim @lock) {
			return new T[] { };
		}

		public T Execute<TU>(TU key, Func<T> cacheMissFunc, ReaderWriterLockSlim @lock) {
			return default(T);
		}

		public void Reset() {
			
		}
	}
}