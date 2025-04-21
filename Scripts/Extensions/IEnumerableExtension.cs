using System;
using System.Collections.Generic;
using System.Linq;

namespace GodBrawl.Extensions;

public static class IEnumerableExtension {
	private static readonly Random Random = new();

	public static T GetRandomElement<T>(this IEnumerable<T> collection) {
		var list = collection.ToArray();
		var index = Random.Next(list.Length);

		return list[index];
	}
}