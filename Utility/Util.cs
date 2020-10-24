using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Totoki.Util {

	/// <summary>
	/// 2つの値のペア
	/// </summary>
	/// <typeparam name="TA">1つめの型</typeparam>
	/// <typeparam name="TB">2つめの型</typeparam>
	[Obsolete("System.Tuple<T1, T2>を使いましょう。")]
	public class Pair<TA, TB> {
		TA valA;
		TB valB;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ValA">値1</param>
		/// <param name="ValB">値2</param>
		public Pair(TA ValA, TB ValB) {
			A = ValA;
			B = ValB;
		}

		/// <summary>
		/// 1つめの値
		/// </summary>
		public TA A {
			get { return valA; }
			set { valA = value; }
		}

		/// <summary>
		/// 2つめの値
		/// </summary>
		public TB B {
			get { return valB; }
			set { valB = value; }
		}

		/// <summary>
		/// 型を指定して値を取得する。
		/// </summary>
		/// <param name="type">値を取り出す型</param>
		/// <returns>
		/// 指定された型に一致する要素の値
		/// 型Aと型Bが同一である場合は型Aの値
		/// どちらとも一致しない場合はnull
		/// </returns>
		public object this[Type type] {
			get {
				if (type == typeof(TA))
					return A;
				else if (type == typeof(TB))
					return B;
				else
					return null;
			}
		}

		/// <summary>
		/// 文字列形式を取得。
		/// </summary>
		/// <returns>文字列</returns>
		public override string ToString() {
			return "Pair{" + A.ToString() + ", " + B.ToString() + "}";
		}

	}

	[Obsolete("System.Tuple<T1, T2, T3>を使いましょう。")]
	public class Trio<TA, TB, TC> {
		TA valA;
		TB valB;
		TC valC;
		public Trio(TA ValA, TB ValB, TC ValC) {
			A = ValA;
			B = ValB;
			C = ValC;
		}

		public TA A {
			get { return valA; }
			set { valA = value; }
		}

		public TB B {
			get { return valB; }
			set { valB = value; }
		}

		public TC C {
			get { return valC; }
			set { valC = value; }
		}

		public object this[Type type] {
			get {
				if (type == typeof(TA))
					return A;
				else if (type == typeof(TB))
					return B;
				else if (type == typeof(TC))
					return C;
				else
					return null;
			}
		}

		public override string ToString() {
			return "Trio{" + A.ToString() + ", " + B.ToString() + ", " + C.ToString() + "}";
		}

	}



	/// <summary>
	/// 値を含む簡易的なEventArgs
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
	public class EventArgsEx<T> : EventArgs {
		T val;

		/// <summary>
		/// 含まれている値
		/// </summary>
		public T Value {
			get { return val; }
			set { val = value; }
		}

		/// <summary>
		/// 値を指定してインスタンスを作成します。
		/// </summary>
		/// <param name="value">設定する値</param>
		public EventArgsEx(T value) {
			val = value;
		}

		/// <summary>
		/// デフォルト値を使用して、インスタンスを作成します。
		/// </summary>
		public EventArgsEx() {
			val = default(T);
		}
	}

	//public delegate bool Behavior<T>(T obj);
	//public delegate bool Behavior<T, TOpt>(T obj, TOpt optional);

	/// <summary>
	/// EventArgs&lt;T&gt;を受け取るEventHandler
	/// </summary>
	/// <typeparam name="T">EventArgs&lt;T&gt;の型パラメータ</typeparam>
	/// <param name="sender">イベントのソース</param>
	/// <param name="e">イベント データを格納している System.EventArgs&lt;T&gt;</param>
	public delegate void EventHandlerEx<T>(object sender, EventArgsEx<T> e);

	/// <summary>
	/// 実行中のメソッド名を含んだデバッグメッセージを作成・出力するためのクラス。
	/// </summary>
	public static class DebugHelper {

		public static TextWriter OutTarget = System.Console.Error;

		public static void Output(string Message) {
			if (OutTarget == null) return;
			lock (OutTarget)
				OutTarget.WriteLine(GetDebugString(Message));
		}

		public static void Output() {
			if (OutTarget == null) return;
			lock (OutTarget)
				OutTarget.WriteLine(GetDebugString());
		}

		public static string GetDebugString(string message) {
			StackFrame CallStack = new StackFrame(1, true);

			string file = CallStack.GetFileName();
			int line = CallStack.GetFileLineNumber();

			return message + " - File: " + file + "Line: " + line.ToString();
		}

		public static string GetDebugString() {
			StackFrame CallStack = new StackFrame(1, true);

			string file = CallStack.GetFileName();
			int line = CallStack.GetFileLineNumber();

			return "File: " + file + "Line: " + line.ToString();
		}

		[Conditional("DEBUG")]
		public static void OutputDebug(string Message) {
			Output(Message);
		}

		public static StackFrame GetStackFrame() {
			return new StackFrame(1, true);
		}
	}

#if !UTIL_NO_SYSTEM_DRAWING
	/// <summary>
	/// サーモグラフ的な色を取得するユーティリティクラス
	/// </summary>
	public static class Thermo {
		/// <summary>
		/// 0.0～1.0の実数値から、サーモグラフ調の色を作成します。
		/// </summary>
		/// <param name="val">0.0～1.0の数値。0.0で黒を、1.0で白を表す。</param>
		/// <returns>Color型の色</returns>
		public static Color GetColor(double val) {
			return GetSimpleColor(val).Color;
		}

		/// <summary>
		/// 0.0～1.0の実数値から、サーモグラフ調の色を作成します。
		/// </summary>
		/// <param name="val">0.0～1.0の数値。0.0で黒を、1.0で白を表す。</param>
		/// <returns>SimpleColor型の色</returns>
		public static SimpleColor GetSimpleColor(double val) {
			val *= 7;

			// 黒 - 青 - 水 - 緑 - 黄 - 赤 - 紫 - 白
			// 0    1    2    3    4    5    6    7
			if (0 <= val && val < 1) {
				return new SimpleColor(
					0,
					0,
					(int)(val * 255));
			}
			else if (val <= 2) {
				val -= 1;
				return new SimpleColor(
					0,
					(int)(val * 255),
					255);
			}
			else if (val <= 3) {
				val -= 2;
				return new SimpleColor(
					0,
					255,
					255 - (int)(val * 255));
			}
			else if (val <= 4) {
				val -= 3;
				return new SimpleColor(
					(int)(val * 255),
					255,
					0);
			}
			else if (val <= 5) {
				val -= 4;
				return new SimpleColor(
					255,
					255 - (int)(val * 255),
					0);
			}
			else if (val <= 6) {
				val -= 5;
				return new SimpleColor(
					255,
					0,
					(int)(val * 255));
			}
			else if (val <= 7) {
				val -= 6;
				return new SimpleColor(
					255,
					(int)(val * 255),
					255);
			}
			else
				throw new ArgumentException("値は 0以上1以下でなければなりません。", "val");

		}

		public static int GetInt(double val) {
			return GetSimpleColor(val).Argb;
		}
	}
#endif

	public static class MP3 {
		// 
		// http://eternalwindows.jp/winmm/mp3/mp309.html から丸写し
		//
		public static bool TryGetFormat(byte[] lpData, out int bitRate, out int sampleRate, out int channel, out int padding) {
			int index;
			int version;
			int[,] dwBitTableLayer3 = new int[2, 16] {
			{0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0}, // MPEG1
			{0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0} // MPEG2	
			};
			int[,] dwSampleTable = new int[2, 3] {
			{44100, 48000, 32000}, // MPEG1
			{22050, 24000, 16000} // MPEG2
			};

			bitRate = sampleRate = channel = padding = 0;

			if (lpData[0] != 0xff || lpData[1] >> 5 != 0x07)
				return false;

			switch (lpData[1] >> 3 & 0x03) {

				case 3:
					version = 1;
					break;

				case 2:
					version = 2;
					break;

				default:
					return false;

			}

			if ((lpData[1] >> 1 & 0x03) != 1)
				return false;

			index = lpData[2] >> 4;
			bitRate = dwBitTableLayer3[version - 1, index];

			index = lpData[2] >> 2 & 0x03;
			sampleRate = dwSampleTable[version - 1, index];

			padding = lpData[2] >> 1 & 0x01;
			channel = (lpData[3] >> 6) == 3 ? 1 : 2;

			// 取得したビットレートなどを基にしてlpmfを初期化する

			return true;
		}

	}

	/// <summary>
	/// 利用するインスタンスと未使用のインスタンスを管理するクラス
	/// </summary>
	/// <typeparam name="T">管理対象のデータの型</typeparam>
	public class Pool<T> where T : class, new() {
		Queue<T> pool;
		List<T> active;

		/// <summary>
		/// コンストラクタ。要素は作成しません。
		/// </summary>
		public Pool() {
			pool = new Queue<T>();
			active = new List<T>();
		}

		/// <summary>
		/// コンストラクタ。指定した数の要素を事前作成します。
		/// </summary>
		/// <param name="initialObjectNum">事前作成する要素の数</param>
		public Pool(int initialObjectNum) {
			pool = new Queue<T>();
			active = new List<T>(initialObjectNum);
			for (int i = 0; i < initialObjectNum; i++)
				pool.Enqueue(new T());
		}

		/// <summary>
		/// コンストラクタ。指定したコレクションの要素を管理対象とします。
		/// </summary>
		/// <param name="initialList">プールに追加する要素のコレクション。要素はコピーされる。</param>
		public Pool(ICollection<T> initialList) {
			pool = new Queue<T>(initialList);
			active = new List<T>();
		}

		/// <summary>
		/// プールからインスタンスを取り出します。取り出されたインスタンスはアクティブとしてマークされます。
		/// プールにインスタンスが存在しない場合はnullを返します。
		/// </summary>
		/// <returns>取り出されたインスタンス、またはプールにインスタンスが存在しない場合はnull。</returns>
		public T Get() {
			T ret = default(T);
			if (pool.Count > 0) {
				ret = pool.Dequeue();
				active.Add(ret);
			}
			
			return null;
		}

		/// <summary>
		/// プールからインスタンスを取り出します。取り出されたインスタンスはアクティブとしてマークされます。
		/// </summary>
		/// <param name="createFlag">Trueが指定されると、プールにインスタンスが存在しない場合にインスタンスを作成します。</param>
		/// <returns>取り出されたインスタンス。または、プールにインスタンスが存在せずcreateFlagがFalseの場合はnull。</returns>
		public T Get(bool createFlag) {
			T ret = default(T);
			if (pool.Count > 0)
				ret = pool.Dequeue();
			else if(createFlag)
				active.Add(ret = new T());

			return ret;
		}
		
		/// <summary>
		/// 管理中のインスタンスのうち、特定の条件を満たしたインスタンスを、プールに戻します。
		/// すでにプールされている要素は対象外です。
		/// </summary>
		/// <param name="pred">プールに戻す条件を表すPredicate&lt;T&gt;</param>
		public void Repository(Predicate<T> pred) {
			foreach (var obj in active.FindAll(pred))
				pool.Enqueue(obj);
			active.RemoveAll(pred);
			
		}

		/// <summary>
		/// プール外のインスタンス(有効になっているインスタンス)全体に対する反復処理を行います。
		/// </summary>
		/// <param name="e">実施する反復処理</param>
		public void ForEachActives(Action<T> e) {
			active.ForEach(e);
			
		}

		/// <summary>
		/// プール外のインスタンスを表すコレクションを取得します。
		/// </summary>
		public IList<T> Actives {
			get { return active.AsReadOnly(); }
		}
	}

	/// <summary>
	/// IDisposableなインスタンスの作成順序を保存し、逆順に解放するためのスタックです。
	/// </summary>
	public class DisposeStack : IDisposableEx {
		LinkedList<IDisposable> disposeStack = new LinkedList<IDisposable>();

		/// <summary>
		/// 解放スタックに新しいインスタンスを追加します。
		/// </summary>
		/// <param name="ev">追加するIDisposableオブジェクト</param>
		public void Add(IDisposable ev) {
			disposeStack.AddFirst(ev);
		}

		/// <summary>
		/// 解放スタックから、指定したインスタンスを除外します。
		/// </summary>
		/// <param name="ev">除外するIDisposableオブジェクト</param>
		public void Remove(IDisposable ev) {
			disposeStack.Remove(ev);
		}

#region IDisposable メンバ

		/// <summary>
		/// 登録されているオブジェクトを、登録と逆順に全て解放します。
		/// </summary>
		public void Dispose() {
			if (Disposing != null) Disposing(this, EventArgs.Empty);
			while (disposeStack.Count > 0) {
				var obj = disposeStack.First.Value;
				disposeStack.RemoveFirst();
				obj.Dispose();
			}
		}

#endregion


#region IDisposableEX メンバ

		/// <summary>
		/// すでに解放が行われたかを取得します。
		/// これは、スタックに要素がない場合にもTrueになります。
		/// </summary>
		public bool Disposed {
			get { return disposeStack.Count == 0; }
		}

		public event EventHandler Disposing;

#endregion
	}

	/// <summary>
	/// 拡張 IDisposableインターフェイス
	/// </summary>
	public interface IDisposableEx : IDisposable {
		/// <summary>
		/// すでにDispose済みである場合、trueを返す。
		/// </summary>
		bool Disposed { get; }
		/// <summary>
		/// Disposeが行われた際に呼ばれるイベント。
		/// </summary>
		event EventHandler Disposing;
	}

	/// <summary>
	/// 主にゲームのFPS調整用のタイマークラス
	/// </summary>
	public class GameTimer {
		long ticksSpan;
		long ticksPerSec;

		long ticksNext;

		int fps;
		/// <summary>
		/// 目標FPS
		/// </summary>
		public int FPS {
			get { return fps; }
			set {
				fps = value;
				ticksSpan = ticksPerSec / fps;
			}
		}

		/// <summary>
		/// 目標FPSに対して大きく遅れた際に、タイマーを早送りし一定間隔を維持しようとするかどうか。
		/// </summary>
		public bool AutoFastForward { get; set; }

		/// <summary>
		/// 目標とするFPSと１秒間あたりのTicksを指定して、GameTimerインスタンスを作成します。
		/// </summary>
		/// <param name="fps">目標とするFPS。</param>
		/// <param name="ticksPerSec">一秒間当たりにDateTimeのTicksが進む量。</param>
		[Obsolete("非推奨です。引数１つのコンストラクタを使用してください。")]
		public GameTimer(int fps, long ticksPerSec) {
			this.ticksPerSec = ticksPerSec;
			FPS = fps;
			ticksSpan = ticksPerSec / fps;

			ticksNext = -1L;
		}

		/// <summary>
		/// 目標とするFPSを指定して、GameTimerインスタンスを作成します。
		/// １秒間あたりのTicksは、メソッド内で計算されます。
		/// </summary>
		/// <param name="targetFPS">目標とするFPS。</param>
		public GameTimer(int fps) {
			this.ticksPerSec = TimeSpan.TicksPerSecond;
			FPS = fps;

			ticksNext = -1L;
		}

		/// <summary>
		/// １秒間あたりのTick数を計算する。
		/// </summary>
		/// <param name="testtime_millisec">計算するために待つ時間。</param>
		/// <returns>計算されたTick数。</returns>
		/// <remarks>この関数は実際に時間の経過を待つことで測定している。</remarks>
		[Obsolete("非推奨です。TimeSpan.TicksPerSecondを使用してください。")]
		public static long CalcTPS(int testtime_millisec) {
			long time_a, time_b;
			time_a = DateTime.Now.Ticks;
			Thread.Sleep(testtime_millisec);
			time_b = DateTime.Now.Ticks;

			return (time_b - time_a) * 1000 / testtime_millisec;
		}

		/// <summary>
		/// 時間計算の基準点を現在の時間に更新する。
		/// </summary>
		public void UpdateStartPoint() {
			ticksNext = DateTime.Now.Ticks + ticksSpan;
		}

		/// <summary>
		/// 基準点がすでに指定され、利用可能な状態であるか。
		/// </summary>
		public bool Enabled {
			get { return ticksNext != -1L; }
		}

		bool spinWait = false;

		/// <summary>
		/// 時間を待つ際に、空ループを使用するかどうか。
		/// </summary>
		public bool UseSpinWait {
			get { return spinWait; }
			set { spinWait = value; }
		}

		/// <summary>
		/// 設定された時間を待つ。
		/// </summary>
		/// <returns>設定された時間まであと何ミリ秒残っているかを表す数値。負の場合は設定された時間を過ぎている。</returns>
		public int Wait() {
			//			long now = DateTime.Now.Ticks;

			// 何秒待てばよいかを計算
			int t = (int)(1000 * (ticksNext - DateTime.Now.Ticks) / ticksPerSec);

			if (t > 0) {
				if (spinWait) {
					long rest = 1;

					do
						rest = ticksNext - DateTime.Now.Ticks;
					while (rest > 0);

					t = (int)(rest / ticksPerSec);
				}
				else {
					// 待っていて時間が過ぎてしまっては元も子もないので、１ミリ秒短くする
					// 意味があるか？　さぁね。
					Thread.Sleep(t - 1);
					t = (int)((ticksNext - DateTime.Now.Ticks) / ticksPerSec);
				}
			}
			do {
				ticksNext += ticksSpan;
			} while (AutoFastForward && ticksNext < DateTime.Now.Ticks);
			return t;
		}

	}

	/// <summary>
	/// 擬似的なプロパティ
	/// </summary>
	/// <typeparam name="T">格納する値の型</typeparam>
	public class Property<T> {
		private Func<T> getter;
		private Action<T> setter;

		public Property(Func<T> _getter) {
			if (_getter == null) throw new ArgumentNullException("_getter にnullを指定することはできません。");
			this.getter = _getter;
			this.setter = null;
		}
		public Property(Func<T> _getter, Action<T> _setter) {
			if (_getter == null) throw new ArgumentNullException("_getter にnullを指定することはできません。");
			this.getter = _getter;
			this.setter = _setter;
		}

		public T Value {
			get { return getter(); }
			set {
				if (setter == null) throw new InvalidOperationException("setterが登録されていないため、setは不正な処理です。");
				setter(value);
			}
		}

		public static implicit operator T(Property<T> p){
			return p.Value;
		}

		public static explicit operator Property<T>(T v) {
			return new Property<T>(() => v, (p) => v = p);
		}
	}

	/// <summary>
	/// ロックされたデータバッファを表すクラス
	/// </summary>
	/// <typeparam name="T">バッファ中の1要素の型</typeparam>
	public class BufferData<T> : IDisposable where T : struct {
		/// <summary>
		/// ロックを解除するためのメソッドを表すデリゲート
		/// </summary>
		/// <param name="data">反映させるデータ</param>
		public delegate void UnlockCallback(T[] data);
		UnlockCallback callback;

		/// <summary>
		/// バッファに格納されているデータ列
		/// </summary>
		public T[] Data { get; set; }

		/// <summary>
		/// バッファを作成する。
		/// </summary>
		/// <param name="callback">ロック解除用コールバック</param>
		/// <param name="data">ロックされたデータ列</param>
		public BufferData(UnlockCallback callback, T[] data) {
			this.callback = callback;
			this.Data = data;
		}

		/// <summary>
		/// ロックを解除する。
		/// </summary>
		public void Dispose() {
			this.callback(Data);
		}

		/// <summary>
		/// 差し替えるためのデータ配列を作成するユーティリティ関数
		/// </summary>
		/// <param name="size">作成する配列のサイズ</param>
		/// <returns>作成された配列</returns>
		public T[] MakeArray(int size) {
			return new T[size];
		}
	}


}
