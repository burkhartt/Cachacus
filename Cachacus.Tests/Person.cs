using System;
using Cachacus.Attributes;

namespace Cachacus.Tests {
	[Serializable]
	public class Person {
		[CacheKey]
		public int Id { get; set; }

		[CacheIndex]
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public Person(string firstName, string lastName, int id) : this() {
			FirstName = firstName;
			LastName = lastName;
			Id = id;
		}

		public Person() {
			
		}
	}
}