
namespace Totoki.Util {
	/// <summary>
	/// 構造体によるラッパ
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct Struct<T> {
		/// <summary>
		/// 値
		/// </summary>
		public T Value;

		/// <summary>
		/// 初期化付きコンストラクタ。
		/// </summary>
		/// <param name="value"></param>
		public Struct(T value) {
			this.Value = value;
		}

		/// <summary>
		/// 内部情報が初期状態かどうか。
		/// </summary>
		public bool Initialized {
			get { return Value.Equals(default(T)); }
		}

		public static implicit operator T(Struct<T> w) {
			return w.Value;
		}
	}
}
