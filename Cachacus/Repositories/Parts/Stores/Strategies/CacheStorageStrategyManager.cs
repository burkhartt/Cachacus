using System;
using System.Configuration;
using GAT.Domain.Cache.Configuration;

namespace GAT.Domain.Cache.Repositories.Parts.Stores.Strategies {
	internal class CacheStorageStrategyManager {
		public static ICacheStorageStrategy GetStrategy() {
			var section = (CacheSection)ConfigurationManager.GetSection("caching");
			if (section.Setup.Type.Equals("FileSystem", StringComparison.OrdinalIgnoreCase)) {
				return new FileSystemStorageStrategy(section.Setup);
			}
			return new NoStorageStrategy();
		}
	}
}