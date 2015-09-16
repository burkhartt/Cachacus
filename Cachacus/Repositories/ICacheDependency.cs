using System;
using System.Collections.Generic;
using Cachacus.Repositories.Parts.CacheMissStrategies;

namespace Cachacus.Repositories {
	public interface ICacheDependency<T> where T : class, new() {
		void Initialize(Func<IList<T>> warmupFunction, ICacheMissStrategy<T> cacheMissStrategy = null);
		IEnumerable<T> GetAll(Func<IEnumerable<T>> cacheMissAction);
		T GetByKey<T2>(T2 key, Func<T> cacheMissAction);
		IEnumerable<T> GetAllByKey<T2>(T2 key, Func<IEnumerable<T>> cacheMissAction);
		IEnumerable<T> GetAllByKeys<T2>(IEnumerable<T2> keys, Func<IEnumerable<T>> cacheMissAction);
		void Bust(IEnumerable<T> newData);
		void Bust(T newData);
		void RecycleCache(Func<IList<T>> warmUpFunction);
		string Identifier { get; }
	}
}