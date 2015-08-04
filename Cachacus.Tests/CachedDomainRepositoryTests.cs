using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Cachacus.Tests {
    [TestClass]
    public class CachedDomainRepositoryTests {
        private CachedPersonRepository repo;

        [TestInitialize]
        public void Setup() {
            repo = new CachedPersonRepository(new PersonRepository());
        }

        [TestMethod]
        public void When_an_item_is_removed_from_the_cache_it_should_not_be_returned() {
            var allPeople = repo.GetAllPeople();
            var personToRemove = allPeople.First();
            repo.Remove(personToRemove);
            repo.GetAllPeople().Count().ShouldEqual(allPeople.Count() - 1);
        }

        [TestMethod]
        public void When_multiple_items_are_removed_from_the_cache_they_should_not_be_returned() {
            var allPeople = repo.GetAllPeople();
            var peopleToRemove = allPeople.Take(2).ToList();
            repo.Remove(peopleToRemove);
            repo.GetAllPeople().Count().ShouldEqual(allPeople.Count() - 2);
        }

        [TestMethod]
        public void When_item_is_removed_it_cannot_be_retrieved_from_secondary_cache() {
            var personToRemove = repo.GetAllPeople().First();
            repo.Remove(personToRemove);
            repo.GetPeopleNamed(personToRemove.FirstName).Any().ShouldBeFalse("person not available by first name");
        }
    }
}