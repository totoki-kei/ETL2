using System;
using System.Collections.Generic;

namespace Totoki.Util {
	public class PartialArray<T> : IList<T> {
		T[] baseArray;
		int offset, count;
		bool readOnly;

		public T[] BaseArray { get { return baseArray; } }

		public PartialArray(T[] _array, int _offset, int _count, bool _readonly) {
			baseArray = _array;
			offset = _offset;
			count = _count;
			readOnly = _readonly;
		}

		public int Length {
			get { return count; }
		}

		public T this[int index] {
			get {
				if (index < 0 || count < index)
					throw new IndexOutOfRangeException(indexErrorMessage);
				return baseArray[index + offset];
			}

			set {
				if (readOnly)
					throw new ApplicationException(readonlyErrorMessage);

				if (index < 0 || count < index)
					throw new IndexOutOfRangeException(indexErrorMessage);

				baseArray[index + offset] = value;
			}
		}

		public T[] ToArray() {
			T[] ret = new T[count];
			for (int i = 0; i < count; i++) {
				ret[i] = this[i];
			}

			return ret;
		}

		public void CopyTo(int offset, T[] destArray, int dstOffset, int count) {
			Array.Copy(baseArray, offset, destArray, dstOffset, count);
		}

		#region 静的メンバ

		const string indexErrorMessage = "インデックスが割り当てられた領域外です。";
		const string readonlyErrorMessage = "このPartialArrayは読み取り専用として定義されています。";

		#endregion

		#region IEnumerable<T> メンバ

		public IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < count; i++)
				yield return baseArray[i + offset];

		}

		#endregion

		#region IList<T> メンバ

		public int IndexOf(T item) {
			for (int i = 0; i < count; i++)
				if (baseArray[offset + i].Equals(item))
					return i;
			return -1;
		}

		public void Insert(int index, T item) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		public void RemoveAt(int index) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		#endregion

		#region ICollection<T> メンバ

		public void Add(T item) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		public void Clear() {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		public bool Contains(T item) {
			return IndexOf(item) != -1;
		}

		public void CopyTo(T[] array, int arrayIndex) {
			CopyTo(0, array, arrayIndex, Math.Min(array.Length - arrayIndex, count));
		}

		public int Count {
			get { return Length; }
		}

		public bool IsReadOnly {
			get { return this.readOnly; }
		}

		public bool Remove(T item) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		#endregion

		#region IEnumerable メンバ

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		#endregion
	}

}
