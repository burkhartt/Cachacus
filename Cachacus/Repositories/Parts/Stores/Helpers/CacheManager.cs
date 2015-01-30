using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using GAT.Domain.Cache.Attributes;

namespace GAT.Domain.Cache.Repositories.Parts.Stores.Helpers {
	public class CacheManager {
		protected static readonly Dictionary<Type, MemberGetter> primaryKeys =
			new Dictionary<Type, MemberGetter>();

		protected static readonly Dictionary<Type, Dictionary<string, MemberGetter>> secondaryKeys =
			new Dictionary<Type, Dictionary<string, MemberGetter>>();

		public static object PrimaryKey<T>(T o) {
			BuildIndexes(typeof(T));
			return primaryKeys[typeof(T)](o);
		}

		public static IEnumerable<string> IndexNames<T>() {
			BuildIndexes(typeof(T));
			return secondaryKeys[typeof(T)].Keys;
		}

		public static object KeyForIndex<T>(string indexName, T o) {
			BuildIndexes(typeof(T));
			return secondaryKeys[typeof(T)][indexName](o);
		}

		private static void BuildIndexes(Type type) {
			if (primaryKeys.ContainsKey(type)) {
				return;
			}
			lock (type) { // Double checked lock is intentional
				if (primaryKeys.ContainsKey(type)) {
					return;
				}
				var primaryKeyProperty = type.GetProperties().SingleOrDefault(prop => Attribute.IsDefined((MemberInfo) prop, typeof(CacheKey)));
				primaryKeys[type] = type.DelegateForGetPropertyValue(primaryKeyProperty.Name);

				var secondaryKeyProperties = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(CacheIndex)));
				var dictionary = secondaryKeyProperties.ToDictionary(kp => kp.Name, kp => type.DelegateForGetPropertyValue(kp.Name));
				secondaryKeys[type] = dictionary;
			}
		}

		public static bool IsSame<T>(T o1, T o2) {
			return PrimaryKey<T>(o1).Equals(PrimaryKey<T>(o2));
		}
	}
}