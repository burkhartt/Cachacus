namespace Cachacus.Repositories.Parts.Stores.Strategies {
	internal interface ICacheStorageStrategy {
		void Save(string key, object obj);
		bool HasExpired<T>();
		T Load<T>(string key);
	}
}