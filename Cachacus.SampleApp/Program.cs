using System;
using System.Collections.Generic;
using System.Linq;
using Cachacus.Repositories;

namespace ConsoleApplication1 {
	class Program {
		static void Main() {			
			var repo = new CachedPersonRepository(new PersonRepository());

			var allPeople = repo.GetAllPeople();

			var people = repo.GetPeopleNamed("Tim");			
			foreach (var person in people) {
				Console.WriteLine(person.FirstName);
			}

			var person2 = repo.GetPersonWithId(3);
			Console.WriteLine(person2.FirstName);

			allPeople = repo.GetAllPeople();
			Console.ReadKey();
		}
	}

	internal class PersonRepository : IPersonRepository {
		private readonly IEnumerable<Person> people;

		public PersonRepository() {
			people = new[] {
				new Person("Tim", "Jones", 1),
				new Person("Tim", "Burkhart", 2)
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

	internal class CachedPersonRepository : AbstractCacheRepository<Person>, IPersonRepository {
		private readonly IPersonRepository repo;

		public CachedPersonRepository(IPersonRepository repo) {
			this.repo = repo;
		}

		public IEnumerable<Person> GetPeopleNamed(string name) {
			return GetAllByKey(name, () => repo.GetPeopleNamed(name));
		}

		public Person GetPersonWithId(int id) {
			return GetByKey(id, () => repo.GetPersonWithId(id));
		}

		protected override IEnumerable<Person> WarmUp() {
			return new[] {new Person("Joe", "Ramsey", 3)};
		}

		public IEnumerable<Person> GetAllPeople() {
			return GetAll(() => repo.GetAllPeople());
		}
	}
}
