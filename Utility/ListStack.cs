using System;
using System.Collections.Generic;

namespace Totoki.Util {
	/// <summary>
	/// スタックと同様のメソッドを実装したList
	/// </summary>
	/// <typeparam name="T">保持する型</typeparam>
	public class ListStack<T> : IList<T> {
		List<T> list;

		public ListStack() {
			list = new List<T>();
		}

		public T Push(T val) {
			list.Add(val);
			return val;
		}

		public T Pop() {
			var ret = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return ret;
		}

		public T Peek() {
			return list[list.Count - 1];
		}

		public T Top {
			get { return list[list.Count - 1]; }
			set { list[list.Count - 1] = value; }
		}

		/// <summary>
		/// 指定した数のデータを、別のListStack&lt;T&gt;にコピーします。
		/// </summary>
		/// <param name="to">コピー先のListStack&lt;T&gt;</param>
		/// <param name="count">コピーするデータ数</param>
		public void XCopy(ListStack<T> to, int count) {
			if (count <= 0) return;
			to.list.AddRange(list.GetRange(list.Count - count, count));
		}

		#region IList<T> メンバ

		public int IndexOf(T item) {
			return list.IndexOf(item);
		}

		public void Insert(int index, T item) {
			list.Insert(index, item);
		}

		public void RemoveAt(int index) {
			list.RemoveAt(index);
		}

		public T this[int index] {
			get {
				if (list.Count == 0)
					throw new InvalidOperationException("要素のないListStackへのアクセスはできません。");
				while (index < 0)
					index += list.Count;
				return list[index];
			}
			set {
				if (list.Count == 0)
					throw new InvalidOperationException("要素のないListStackへのアクセスはできません。");
				while (index < 0)
					index += list.Count;
				list[index] = value;
			}
		}

		#endregion

		#region ICollection<T> メンバ

		public void Add(T item) {
			list.Add(item);
		}

		public void Clear() {
			list.Clear();
		}

		public bool Contains(T item) {
			return list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			list.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return list.Count; }
		}

		public bool IsReadOnly {
			get { return ((IList<T>)list).IsReadOnly; }
		}

		public bool Remove(T item) {
			return list.Remove(item);
		}

		#endregion

		#region IEnumerable<T> メンバ

		public IEnumerator<T> GetEnumerator() {
			return list.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバ

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return ((System.Collections.IEnumerable)list).GetEnumerator();
		}

		#endregion
	}

}
