using System.Collections.Generic;

namespace Cachacus.Repositories {
	public interface ICacheRepository<in T> {
		void Bust(T @object);
		void Bust(IEnumerable<T> newData);
        void RecycleCache();
	}
}