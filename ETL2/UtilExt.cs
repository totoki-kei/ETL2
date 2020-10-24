using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Totoki.ETL {
	public static class UtilExt {
		public static int ToInt(this InputUnit e) {
			return (int)e;
		}

		public static AxisType Invert(this AxisType e) {
			return (AxisType)((int)e ^ 0x01);
		}
		public static int GetSign(this AxisType e) {
			return ((int)e & 0x01) == 0 ? 1 : -1;
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> fn) {
			foreach (T t in list) {
				fn(t);
			}
		}

		public static void ForEachMesh(this Model model, Action<ModelMesh> fn) {
			foreach (var mesh in model.Meshes) fn(mesh);
		}

		public static void ForEachMesh(this Model model, Action<ModelMesh, int> fn) {
			foreach (var mesh in model.Meshes.WithIndex())
				fn(mesh.Item2, mesh.Item1);
		}

		public static void ForEachMeshPart(this Model model, Action<ModelMeshPart> fn) {
			foreach (var mesh in model.Meshes)
				foreach (var part in mesh.MeshParts)
					fn(part);
		}

		public static void ForEachMeshPart(this Model model, Action<ModelMeshPart, int, int> fn) {
			foreach (var mesh in model.Meshes.WithIndex())
				foreach (var part in mesh.Item2.MeshParts.WithIndex())
					fn(part.Item2, mesh.Item1, part.Item1);
		}

		public static IEnumerable<ValueTuple<int, T>> WithIndex<T>(this IEnumerable<T> enumerator) {
			int index = 0;
			foreach (var element in enumerator) {
				yield return ValueTuple.Create(index++, element);
			}
		}
	}

}
