using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Cachacus.Tests {
    [TestClass]
    public class CacheBustTests {
        private CachedPersonRepository repo;

        [TestInitialize]
        public void Setup() {
            repo = new CachedPersonRepository(new PersonRepository(), WarmUpQuery());
        }

        private Func<IEnumerable<Person>> WarmUpQuery() {
            return () => new[] {
                        new Person {FirstName = "Tim", LastName = "Burkhart", Id = 1},
                        new Person {FirstName = "Tim", LastName = "Smith", Id = 2},
                        new Person {FirstName = "Tim", LastName = "McManaman", Id = 3}
                    };
        }

        [TestMethod]
        public void When_an_object_with_the_same_index_and_key_is_updated_then_the_original_object_in_the_cache_should_be_replaced() {
            var allPeople = repo.GetAllPeople();
            var person = allPeople.Single(p => p.Id == 1);
            person.FirstName.ShouldEqual("Tim");
            person.LastName.ShouldEqual("Burkhart");

            repo.Bust(new Person {FirstName = "Tim", LastName = "Jones", Id = 1});
            var updatedPeople = repo.GetAllPeople();
            var updatedPerson = updatedPeople.Single(p => p.Id == 1);
            updatedPerson.FirstName.ShouldEqual("Tim");
            updatedPerson.LastName.ShouldEqual("Jones");
        }
    }
}