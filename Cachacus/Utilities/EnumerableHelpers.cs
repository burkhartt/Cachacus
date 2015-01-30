using System;
using System.Collections.Generic;

namespace GAT.Domain.Cache.Utilities {
	internal static class EnumerableHelpers {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var item in items) {
				action(item);
			}
		}
	}
}