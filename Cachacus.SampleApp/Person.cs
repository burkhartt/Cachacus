using System;
using GAT.Domain.Cache.Attributes;

namespace ConsoleApplication1 {
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