using System;
using System.Collections.Generic;
using System.Threading;

namespace GAT.Domain.Cache.Repositories.Parts.CacheMissStrategies {
	public interface ICacheMissStrategy<T> {
		IEnumerable<T> Execute(Func<IEnumerable<T>> cacheMissFunc);
		IEnumerable<T> Execute<TU>(IEnumerable<TU> keys, Func<IEnumerable<T>> cacheMissFunc, ReaderWriterLockSlim @lock);
		T Execute<TU>(TU key, Func<T> cacheMissFunc, ReaderWriterLockSlim @lock);
		void Reset();
	}
}