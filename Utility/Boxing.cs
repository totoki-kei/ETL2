
namespace Totoki.Util {
	/// <summary>
	/// クラスの参照型ラッパ
	/// </summary>
	/// <typeparam name="T">内部で使用する型</typeparam>
	public class Boxing<T> {
		/// <summary>
		/// 値
		/// </summary>
		public T Value;

		/// <summary>
		/// 引数なしのコンストラクタ。
		/// </summary>
		public Boxing() {
			Value = default(T);
		}

		/// <summary>
		/// 初期化付きコンストラクタ。
		/// </summary>
		/// <param name="refer"></param>
		public Boxing(T refer) {
			this.Value = refer;
		}

		/// <summary>
		/// 内部情報が初期状態かどうか。
		/// </summary>
		public bool Initialized {
			get { return Value.Equals(default(T)); }
		}

		public static implicit operator T(Boxing<T> w) {
			return w.Value;
		}
	}

}
