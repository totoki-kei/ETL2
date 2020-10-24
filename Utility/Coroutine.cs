//#define INDEXER_COROUTINE
using System;
using System.Collections.Generic;

namespace Totoki.Util {

#if !INDEXER_COROUTINE
	/// <summary>
	/// IEnumeratorによるコルーチンの基本型
	/// </summary>
	/// <typeparam name="T">コルーチンの戻り値の型</typeparam>
	public abstract class CoroutineBase<T> : IDisposable {
		/// <summary>
		/// コルーチンのメソッド本体。
		/// </summary>
		/// <returns>IEnumerator&lt;T&gt;</returns>
		protected abstract IEnumerator<T> Action();

		private IEnumerator<T> enumerator;

		/// <summary>
		/// Coroutineの基底コンストラクタ
		/// </summary>
		public CoroutineBase() {
			enumerator = null;
			Finished = false;
		}

		/// <summary>
		/// コルーチンが終了しているかどうか
		/// </summary>
		public bool Finished { get; private set; }

		/// <summary>
		/// コルーチンを1ステップ実行する。
		/// </summary>
		/// <param name="ret">実行時の戻り値。終了している場合はdefault(T)。</param>
		/// <returns>1ステップ動作した場合はTrue, 終了している場合はfalse。</returns>
		public bool Resume(out T ret) {
			if (enumerator == null) enumerator = Action();

			if (!enumerator.MoveNext()) {
				Finished = true;
				ret = default(T);
				return false;
			}
			else {
				Finished = false;
				ret = enumerator.Current;
				return true;
			}

		}


		/// <summary>
		/// コルーチンを1ステップ実行する。
		/// </summary>
		/// <returns>1ステップ動作した場合はTrue, 終了している場合はfalse。</returns>
		public bool Resume() {
			if (enumerator == null) enumerator = Action();

			if (!enumerator.MoveNext()) {
				Finished = true;
				return false;
			}
			else {
				Finished = false;
				return true;
			}

		}

		delegate TOut Decorator<out TOut, in TIn>(TIn input);

		/// <summary>
		/// リソース(IEnumerator)を破棄する。
		/// </summary>
		public virtual void Dispose() {
			enumerator.Dispose();
		}
	}

	/// <summary>
	/// シンプルなコルーチン実装
	/// </summary>
	/// <typeparam name="T">コルーチンの戻り値の型</typeparam>
	public class Coroutine<T> : CoroutineBase<T> {
		IEnumerator<T> en;

		/// <summary>
		/// インスタンスの作成
		/// </summary>
		/// <param name="e">動作を表すIEnumerator&lt;T&gt;</param>
		public Coroutine(IEnumerator<T> e) {
			en = e;
		}

		protected override IEnumerator<T> Action() {
			return en;
		}

		/// <summary>
		/// リソース(IEnumerator)を破棄する。
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			en.Dispose();
		}

	}
#elif false
	public class Coroutine : IDisposable {
		IEnumerator<bool> co;

		public Coroutine(IEnumerator<bool> fn) {
			co = fn;
		}

		public bool this[int n] {
			get {
				while (n-- > 0) {
					if (co == null || !co.MoveNext() || !co.Current) {
						return false;
					}

				}
				return true;
			}
		}

		public void Dispose() {
			co.Dispose();
		}
	}
#else
	public delegate object Coroutine();
	public static class CoroutineHelper {
		class CoroutineImpl {
			public CoroutineImpl() { }
			public IEnumerator en;
			public object Resume() {
				en.MoveNext();

				return en.Current;
			}
		}

		public static Coroutine ToCoroutine<U>(this IEnumerator<U> enumerator) {
			return new CoroutineImpl() { en = enumerator }.Resume;
		}

	}
#endif
}
