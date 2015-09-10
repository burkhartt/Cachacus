using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cachacus.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cachacus.Tests {
	[TestClass]
	public class TestCachedDependency {
		private CacheDependency<Person> personCache;

		[TestInitialize]
		public void Initialize() {
			personCache = new CacheDependency<Person>();
		}

		private class CachedPersonRepository {
			private readonly ICacheDependency<Person> _personCacheDependency;
			private static int WarmUpCount = 0;

			public CachedPersonRepository(ICacheDependency<Person> personCacheDependency) {
				_personCacheDependency = personCacheDependency;
				_personCacheDependency.Initialize(Warmup);
			}

			private IList<Person> Warmup() {
				Interlocked.Increment(ref WarmUpCount);
				return new [] { new Person("Bob", "Bobson", 2), new Person("Penny", "Nichols", 4) };
			}

			public int CountPersons {
				get { return _personCacheDependency.GetAll(() => new Person[0]).Count(); }
			}

			public static int CountOfWarmUps { get { return WarmUpCount; } }
		}


		[TestMethod]
		public void When_Cache_Is_Accessed_From_Multiple_Threads_It_Only_Initialized_Once() {
			var tasks = new List<Task>();
			for(int i = 0; i < 1000; i++)
				tasks.Add(Task.Factory.StartNew(() => {
					var repository = new CachedPersonRepository(personCache);
					//System.Diagnostics.Trace.WriteLine(repository.CountPersons);
				}));

			Task.WaitAll(tasks.ToArray());
			Assert.AreEqual(CachedPersonRepository.CountOfWarmUps, 1);
		}
	}
}
