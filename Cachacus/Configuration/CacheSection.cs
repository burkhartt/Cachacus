using System.Configuration;

namespace Cachacus.Configuration {	
	public class CacheSection : ConfigurationSection {
		[ConfigurationProperty("setup", IsRequired = true)]
		public CacheConfiguration Setup {
			get { return base["setup"] as CacheConfiguration; }
		}
	}
}