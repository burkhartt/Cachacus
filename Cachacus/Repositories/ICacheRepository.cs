using System.Collections.Generic;

namespace GAT.Domain.Cache.Repositories {
	public interface ICacheRepository<in T> {
		void Bust(T @object);
		void Bust(IEnumerable<T> newData);
	}
}