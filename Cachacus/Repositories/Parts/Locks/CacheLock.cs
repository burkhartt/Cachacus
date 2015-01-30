using System;
using System.Collections.Generic;
using System.Threading;

namespace Cachacus.Repositories.Parts.Locks {
	[Serializable]
	public class CacheLock {
		public static IEnumerable<T> Read<T>(ReaderWriterLockSlim @lock, Func<IEnumerable<T>> func) {
			@lock.EnterReadLock();
			try {
				return func();
			} finally {
				@lock.ExitReadLock();
			}
		}

		public static T Read<T>(ReaderWriterLockSlim @lock, Func<T> func) {
			@lock.EnterReadLock();
			try {
				return func();
			} finally {
				@lock.ExitReadLock();
			}
		}

		public static void Write(ReaderWriterLockSlim @lock, Action action) {
			@lock.EnterWriteLock();
			try {
				action();
			} finally {
				@lock.ExitWriteLock();
			}
		}
	}
}