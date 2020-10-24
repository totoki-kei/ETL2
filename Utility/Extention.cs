using System;
using System.Collections.Generic;
using System.Text;

namespace Totoki.Util {
	public static class Extention {
		public static void ForEach<T>(this IEnumerable<T> e, Action<T> f) {
			foreach (var x in e) f(x);
		}
	}
}
