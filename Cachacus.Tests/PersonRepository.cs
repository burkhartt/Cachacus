using System;
using System.Collections.Generic;
using System.Linq;

namespace Cachacus.Tests {
    internal class PersonRepository : IPersonRepository {
        private readonly IEnumerable<Person> people;

        public PersonRepository() {
            people = new Person[] {
                new Person("Tim", "Jones", 1),
                new Person("Michael", "Jordan", 2)
            };
        }

        public IEnumerable<Person> GetPeopleNamed(string name) {
            return people.Where(p => p.FirstName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Person GetPersonWithId(int id) {
            return people.Single(p => p.Id == id);
        }

        public IEnumerable<Person> GetAllPeople() {
            return people;
        }
    }
}