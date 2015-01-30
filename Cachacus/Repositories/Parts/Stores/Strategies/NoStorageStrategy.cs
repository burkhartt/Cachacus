using System;

namespace GAT.Domain.Cache.Repositories.Parts.Stores.Strategies {
	internal class NoStorageStrategy : ICacheStorageStrategy {
		public void Save(string key, object obj) {
			
		}

		public bool HasExpired<T>() {
			return true;
		}

		public T Load<T>(string key) {
			throw new NotImplementedException();
		}
	}
}