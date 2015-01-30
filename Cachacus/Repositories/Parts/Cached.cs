using System.Collections.Generic;
using System.Threading;

namespace Cachacus.Repositories.Parts {
	public abstract class Cached<T> {
		protected ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		public abstract void Bust(T @object);
		public abstract void Bust(IEnumerable<T> objects);
	}
}