using System.Collections.Generic;

namespace Cachacus.Tests {
	internal interface IPersonRepository {
		IEnumerable<Person> GetPeopleNamed(string name);
		Person GetPersonWithId(int id);
		IEnumerable<Person> GetAllPeople();
	}
}