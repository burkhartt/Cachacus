using System.Configuration;

namespace GAT.Domain.Cache.Configuration {
	public class CacheConfiguration : ConfigurationElement {
		[ConfigurationProperty("type", IsKey = true, IsRequired = true)]
		public string Type {
			get { return base["type"] as string; }
			set { base["type"] = value; }
		}

		[ConfigurationProperty("path", IsKey = true, IsRequired = true)]
		public string Path {
			get { return base["path"] as string; }
			set { base["path"] = value; }
		}

		[ConfigurationProperty("expirationInMinutes", IsKey = true, IsRequired = true)]
		public string ExpirationInMinutes {
			get { return base["expirationInMinutes"] as string; }
			set { base["expirationInMinutes"] = value; }
		}
	}
}