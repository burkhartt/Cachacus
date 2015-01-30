using System.Configuration;

namespace GAT.Domain.Cache.Configuration {	
	public class CacheSection : ConfigurationSection {
		[ConfigurationProperty("setup", IsRequired = true)]
		public CacheConfiguration Setup {
			get { return base["setup"] as CacheConfiguration; }
		}
	}
}