using System;
using System.Configuration;
using Cachacus.Configuration;

namespace Cachacus.Repositories.Parts.Stores.Strategies {
	internal class CacheStorageStrategyManager {
		public static ICacheStorageStrategy GetStrategy() {
			var section = (CacheSection)ConfigurationManager.GetSection("caching");
			if (section != null && section.Setup.Type.Equals("FileSystem", StringComparison.OrdinalIgnoreCase)) {
				return new FileSystemStorageStrategy(section.Setup);
			}
			return new NoStorageStrategy();
		}
	}
}