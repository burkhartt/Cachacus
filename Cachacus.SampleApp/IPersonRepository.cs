using System.Collections.Generic;

namespace ConsoleApplication1 {
	internal interface IPersonRepository {
		IEnumerable<Person> GetPeopleNamed(string name);
		Person GetPersonWithId(int id);
		IEnumerable<Person> GetAllPeople();
	}
}