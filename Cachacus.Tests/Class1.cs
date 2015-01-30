//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using GAT.Domain.Cache.Attributes;
//using GAT.Domain.Cache.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace GAT.Domain.Cache.Tests {
//	[TestClass]
//	public class CachedDomainRepositoryTests {
//		private CachedTestDatabaseObjectRepository cachedDomainRepository;

//		[TestInitialize]
//		public void Setup() {
//			var mock = new Mock<IDataAccessRepository<TestDatabaseObject>>();
//			mock.Setup(x => x.Query("234")).Returns(() => new[] {new TestDatabaseObject {Id = "ZZZ", SecondaryId = "234", Name = "Sigur Ros"}});
//			mock.Setup(x => x.WarmUp()).Returns(new[] {
//				new TestDatabaseObject {Id = "ABC", SecondaryId = "111", Name = "Tim"},
//				new TestDatabaseObject {Id = "DEF", SecondaryId = "111", Name = "Ice Cream"},
//				new TestDatabaseObject {Id = "GHI", SecondaryId = "123", Name = "Jellybeans"},
//				new TestDatabaseObject {Id = "JKL", SecondaryId = "123", Name = "Sunshine"},
//				new TestDatabaseObject {Id = "MNO", SecondaryId = "123", Name = "Cloudy"},
//			});

//			cachedDomainRepository = new CachedTestDatabaseObjectRepository(mock.Object);
//			cachedDomainRepository.BuildCache();
//		}

//		[TestMethod]
//		public void When_an_object_that_is_in_the_cache_is_updated_then_the_object_should_be_updated() {
//			cachedDomainRepository.Bust(new TestDatabaseObject {Id = "ABC", SecondaryId = "111", Name = "Timothy"});
//			cachedDomainRepository.GetByKey("ABC").Name.ShouldEqual("Timothy");
//		}

//		[TestMethod]
//		public void When_getting_a_list_of_objects_by_key_then_the_objects_with_that_key_should_be_returned() {
//			cachedDomainRepository.GetAllByKey("123").Count().ShouldEqual(3);
//		}

//		[TestMethod]
//		public void When_getting_a_list_of_objects_by_key_after_they_have_been_updated_then_they_should_still_be_returned() {
//			cachedDomainRepository.Bust(new TestDatabaseObject {Id = "MNO", SecondaryId = "123", Name = "Rainy"});
//			cachedDomainRepository.GetAllByKey("123").Count().ShouldEqual(3);
//			cachedDomainRepository.GetAllByKey("123").Count(t => t.Name.Equals("Rainy")).ShouldEqual(1);
//		}

//		[TestMethod]
//		public void When_getting_data_by_a_property_that_does_not_contain_any_data_then_the_cache_should_be_automatically_updated_and_the_new_item_returned() {
//			cachedDomainRepository.Query("234").Count().ShouldEqual(1);
//		}
//	}

//	[TestClass]
//	public class CachedDomainRepositoryWithClusteredKeyesTests : BaseTest {
//		private CachedTestDatabaseObjectWithClusteredKeysRepository cachedDomainRepository;

//		[TestInitialize]
//		public void Setup() {
//			var mock = new Mock<IDataAccessRepository<TestDatabaseObjectWithClusteredKeys>>();
//			mock.Setup(x => x.WarmUp()).Returns(new[] {
//				new TestDatabaseObjectWithClusteredKeys {FirstPart = "ABC", SecondPart = "123", Name = "Tim"},
//				new TestDatabaseObjectWithClusteredKeys {FirstPart = "ABC", SecondPart = "456", Name = "Cloudy"},
//			});

//			cachedDomainRepository = new CachedTestDatabaseObjectWithClusteredKeysRepository(mock.Object);
//			cachedDomainRepository.BuildCacheAsync();
//		}

//		[TestMethod]
//		public void When_updating_a_clustered_key_database_object_then_the_data_with_matching_keys_should_be_updated() {
//			cachedDomainRepository.Bust(new TestDatabaseObjectWithClusteredKeys {FirstPart = "ABC", SecondPart = "123", Name = "Timothy"});
//			cachedDomainRepository.GetAllByKey("123").Single().Name.ShouldEqual("Timothy");
//		}
//	}

//	public class CachedTestDatabaseObjectRepository : AbstractCacheRepository<TestDatabaseObject> {
//		public CachedTestDatabaseObjectRepository(IDataAccessRepository<TestDatabaseObject> repo){
//		}

//		protected override IEnumerable<TestDatabaseObject> WarmUp() {
//			throw new NotImplementedException();
//		}
//	}

//	public class CachedTestDatabaseObjectWithClusteredKeysRepository : AbstractCacheRepository<TestDatabaseObjectWithClusteredKeys> {
//		public CachedTestDatabaseObjectWithClusteredKeysRepository(IDataAccessRepository<TestDatabaseObjectWithClusteredKeys> delegateRepository) : base(delegateRepository) {
//		}

//		protected override IEnumerable<TestDatabaseObjectWithClusteredKeys> WarmUp() {
//			throw new NotImplementedException();
//		}
//	}

//	public class TestDatabaseObjectRepository : IDataAccessRepository<TestDatabaseObject> {
//		private readonly IEnumerable<TestDatabaseObject> initialCache;

//		public TestDatabaseObjectRepository(IEnumerable<TestDatabaseObject> initialCache) {
//			this.initialCache = initialCache;
//		}

//		public void Save(IEnumerable<TestDatabaseObject> objects) {
//		}

//		public TestDatabaseObject Save(TestDatabaseObject @object) {
//			return null;
//		}

//		public IEnumerable<TestDatabaseObject> GetAll() {
//			return null;
//		}

//		public IEnumerable<TestDatabaseObject> WarmUp() {
//			return initialCache;
//		}

//		public IEnumerable<TestDatabaseObject> Query<TU>(IEnumerable<TU> keys) {
//			return null;
//		}

//		public IEnumerable<TestDatabaseObject> Find(Expression<Func<TestDatabaseObject, object>> property, object value) {
//			return null;
//		}

//		public IEnumerable<TestDatabaseObject> GetAllByKey<TU>(TU key) {
//			throw new NotImplementedException();
//		}

//		public IEnumerable<TestDatabaseObject> FindByProperty(Expression<Func<TestDatabaseObject, object>> property, object value) {
//			throw new NotImplementedException();
//		}

//		public IEnumerable<TestDatabaseObject> GetAllByKeys<TU>(IEnumerable<TU> keys) {
//			throw new NotImplementedException();
//		}

//		public TestDatabaseObject GetByKey<TU>(TU key) {
//			throw new NotImplementedException();
//		}
//	}

//	public interface IDataAccessRepository<T> {
//	}

//	public class TestDatabaseObject {
//		[CacheKey]
//		public string Id { get; set; }

//		public string Name { get; set; }

//		[CacheIndex]
//		public string SecondaryId { get; set; }
//	}

//	public class TestDatabaseObjectWithClusteredKeys {
//		[CacheKey]
//		public string Key {
//			get { return FirstPart + "." + SecondPart; }
//		}

//		public string FirstPart { get; set; }

//		[CacheIndex]
//		public string SecondPart { get; set; }

//		public string Name { get; set; }
//	}
//}