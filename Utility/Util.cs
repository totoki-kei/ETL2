using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Totoki.Util {

	/// <summary>
	/// 2�̒l�̃y�A
	/// </summary>
	/// <typeparam name="TA">1�߂̌^</typeparam>
	/// <typeparam name="TB">2�߂̌^</typeparam>
	[Obsolete("System.Tuple<T1, T2>���g���܂��傤�B")]
	public class Pair<TA, TB> {
		TA valA;
		TB valB;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="ValA">�l1</param>
		/// <param name="ValB">�l2</param>
		public Pair(TA ValA, TB ValB) {
			A = ValA;
			B = ValB;
		}

		/// <summary>
		/// 1�߂̒l
		/// </summary>
		public TA A {
			get { return valA; }
			set { valA = value; }
		}

		/// <summary>
		/// 2�߂̒l
		/// </summary>
		public TB B {
			get { return valB; }
			set { valB = value; }
		}

		/// <summary>
		/// �^���w�肵�Ēl���擾����B
		/// </summary>
		/// <param name="type">�l�����o���^</param>
		/// <returns>
		/// �w�肳�ꂽ�^�Ɉ�v����v�f�̒l
		/// �^A�ƌ^B������ł���ꍇ�͌^A�̒l
		/// �ǂ���Ƃ���v���Ȃ��ꍇ��null
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
		/// ������`�����擾�B
		/// </summary>
		/// <returns>������</returns>
		public override string ToString() {
			return "Pair{" + A.ToString() + ", " + B.ToString() + "}";
		}

	}

	[Obsolete("System.Tuple<T1, T2, T3>���g���܂��傤�B")]
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
	/// �l���܂ފȈՓI��EventArgs
	/// </summary>
	/// <typeparam name="T">�l�̌^</typeparam>
	public class EventArgsEx<T> : EventArgs {
		T val;

		/// <summary>
		/// �܂܂�Ă���l
		/// </summary>
		public T Value {
			get { return val; }
			set { val = value; }
		}

		/// <summary>
		/// �l���w�肵�ăC���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="value">�ݒ肷��l</param>
		public EventArgsEx(T value) {
			val = value;
		}

		/// <summary>
		/// �f�t�H���g�l���g�p���āA�C���X�^���X���쐬���܂��B
		/// </summary>
		public EventArgsEx() {
			val = default(T);
		}
	}

	//public delegate bool Behavior<T>(T obj);
	//public delegate bool Behavior<T, TOpt>(T obj, TOpt optional);

	/// <summary>
	/// EventArgs&lt;T&gt;���󂯎��EventHandler
	/// </summary>
	/// <typeparam name="T">EventArgs&lt;T&gt;�̌^�p�����[�^</typeparam>
	/// <param name="sender">�C�x���g�̃\�[�X</param>
	/// <param name="e">�C�x���g �f�[�^���i�[���Ă��� System.EventArgs&lt;T&gt;</param>
	public delegate void EventHandlerEx<T>(object sender, EventArgsEx<T> e);

	/// <summary>
	/// ���s���̃��\�b�h�����܂񂾃f�o�b�O���b�Z�[�W���쐬�E�o�͂��邽�߂̃N���X�B
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
	/// �T�[���O���t�I�ȐF���擾���郆�[�e�B���e�B�N���X
	/// </summary>
	public static class Thermo {
		/// <summary>
		/// 0.0�`1.0�̎����l����A�T�[���O���t���̐F���쐬���܂��B
		/// </summary>
		/// <param name="val">0.0�`1.0�̐��l�B0.0�ō����A1.0�Ŕ���\���B</param>
		/// <returns>Color�^�̐F</returns>
		public static Color GetColor(double val) {
			return GetSimpleColor(val).Color;
		}

		/// <summary>
		/// 0.0�`1.0�̎����l����A�T�[���O���t���̐F���쐬���܂��B
		/// </summary>
		/// <param name="val">0.0�`1.0�̐��l�B0.0�ō����A1.0�Ŕ���\���B</param>
		/// <returns>SimpleColor�^�̐F</returns>
		public static SimpleColor GetSimpleColor(double val) {
			val *= 7;

			// �� - �� - �� - �� - �� - �� - �� - ��
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
				throw new ArgumentException("�l�� 0�ȏ�1�ȉ��łȂ���΂Ȃ�܂���B", "val");

		}

		public static int GetInt(double val) {
			return GetSimpleColor(val).Argb;
		}
	}
#endif

	public static class MP3 {
		// 
		// http://eternalwindows.jp/winmm/mp3/mp309.html ����ێʂ�
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

			// �擾�����r�b�g���[�g�Ȃǂ���ɂ���lpmf������������

			return true;
		}

	}

	/// <summary>
	/// ���p����C���X�^���X�Ɩ��g�p�̃C���X�^���X���Ǘ�����N���X
	/// </summary>
	/// <typeparam name="T">�Ǘ��Ώۂ̃f�[�^�̌^</typeparam>
	public class Pool<T> where T : class, new() {
		Queue<T> pool;
		List<T> active;

		/// <summary>
		/// �R���X�g���N�^�B�v�f�͍쐬���܂���B
		/// </summary>
		public Pool() {
			pool = new Queue<T>();
			active = new List<T>();
		}

		/// <summary>
		/// �R���X�g���N�^�B�w�肵�����̗v�f�����O�쐬���܂��B
		/// </summary>
		/// <param name="initialObjectNum">���O�쐬����v�f�̐�</param>
		public Pool(int initialObjectNum) {
			pool = new Queue<T>();
			active = new List<T>(initialObjectNum);
			for (int i = 0; i < initialObjectNum; i++)
				pool.Enqueue(new T());
		}

		/// <summary>
		/// �R���X�g���N�^�B�w�肵���R���N�V�����̗v�f���Ǘ��ΏۂƂ��܂��B
		/// </summary>
		/// <param name="initialList">�v�[���ɒǉ�����v�f�̃R���N�V�����B�v�f�̓R�s�[�����B</param>
		public Pool(ICollection<T> initialList) {
			pool = new Queue<T>(initialList);
			active = new List<T>();
		}

		/// <summary>
		/// �v�[������C���X�^���X�����o���܂��B���o���ꂽ�C���X�^���X�̓A�N�e�B�u�Ƃ��ă}�[�N����܂��B
		/// �v�[���ɃC���X�^���X�����݂��Ȃ��ꍇ��null��Ԃ��܂��B
		/// </summary>
		/// <returns>���o���ꂽ�C���X�^���X�A�܂��̓v�[���ɃC���X�^���X�����݂��Ȃ��ꍇ��null�B</returns>
		public T Get() {
			T ret = default(T);
			if (pool.Count > 0) {
				ret = pool.Dequeue();
				active.Add(ret);
			}
			
			return null;
		}

		/// <summary>
		/// �v�[������C���X�^���X�����o���܂��B���o���ꂽ�C���X�^���X�̓A�N�e�B�u�Ƃ��ă}�[�N����܂��B
		/// </summary>
		/// <param name="createFlag">True���w�肳���ƁA�v�[���ɃC���X�^���X�����݂��Ȃ��ꍇ�ɃC���X�^���X���쐬���܂��B</param>
		/// <returns>���o���ꂽ�C���X�^���X�B�܂��́A�v�[���ɃC���X�^���X�����݂���createFlag��False�̏ꍇ��null�B</returns>
		public T Get(bool createFlag) {
			T ret = default(T);
			if (pool.Count > 0)
				ret = pool.Dequeue();
			else if(createFlag)
				active.Add(ret = new T());

			return ret;
		}
		
		/// <summary>
		/// �Ǘ����̃C���X�^���X�̂����A����̏����𖞂������C���X�^���X���A�v�[���ɖ߂��܂��B
		/// ���łɃv�[������Ă���v�f�͑ΏۊO�ł��B
		/// </summary>
		/// <param name="pred">�v�[���ɖ߂�������\��Predicate&lt;T&gt;</param>
		public void Repository(Predicate<T> pred) {
			foreach (var obj in active.FindAll(pred))
				pool.Enqueue(obj);
			active.RemoveAll(pred);
			
		}

		/// <summary>
		/// �v�[���O�̃C���X�^���X(�L���ɂȂ��Ă���C���X�^���X)�S�̂ɑ΂��锽���������s���܂��B
		/// </summary>
		/// <param name="e">���{���锽������</param>
		public void ForEachActives(Action<T> e) {
			active.ForEach(e);
			
		}

		/// <summary>
		/// �v�[���O�̃C���X�^���X��\���R���N�V�������擾���܂��B
		/// </summary>
		public IList<T> Actives {
			get { return active.AsReadOnly(); }
		}
	}

	/// <summary>
	/// IDisposable�ȃC���X�^���X�̍쐬������ۑ����A�t���ɉ�����邽�߂̃X�^�b�N�ł��B
	/// </summary>
	public class DisposeStack : IDisposableEx {
		LinkedList<IDisposable> disposeStack = new LinkedList<IDisposable>();

		/// <summary>
		/// ����X�^�b�N�ɐV�����C���X�^���X��ǉ����܂��B
		/// </summary>
		/// <param name="ev">�ǉ�����IDisposable�I�u�W�F�N�g</param>
		public void Add(IDisposable ev) {
			disposeStack.AddFirst(ev);
		}

		/// <summary>
		/// ����X�^�b�N����A�w�肵���C���X�^���X�����O���܂��B
		/// </summary>
		/// <param name="ev">���O����IDisposable�I�u�W�F�N�g</param>
		public void Remove(IDisposable ev) {
			disposeStack.Remove(ev);
		}

#region IDisposable �����o

		/// <summary>
		/// �o�^����Ă���I�u�W�F�N�g���A�o�^�Ƌt���ɑS�ĉ�����܂��B
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


#region IDisposableEX �����o

		/// <summary>
		/// ���łɉ�����s��ꂽ�����擾���܂��B
		/// ����́A�X�^�b�N�ɗv�f���Ȃ��ꍇ�ɂ�True�ɂȂ�܂��B
		/// </summary>
		public bool Disposed {
			get { return disposeStack.Count == 0; }
		}

		public event EventHandler Disposing;

#endregion
	}

	/// <summary>
	/// �g�� IDisposable�C���^�[�t�F�C�X
	/// </summary>
	public interface IDisposableEx : IDisposable {
		/// <summary>
		/// ���ł�Dispose�ς݂ł���ꍇ�Atrue��Ԃ��B
		/// </summary>
		bool Disposed { get; }
		/// <summary>
		/// Dispose���s��ꂽ�ۂɌĂ΂��C�x���g�B
		/// </summary>
		event EventHandler Disposing;
	}

	/// <summary>
	/// ��ɃQ�[����FPS�����p�̃^�C�}�[�N���X
	/// </summary>
	public class GameTimer {
		long ticksSpan;
		long ticksPerSec;

		long ticksNext;

		int fps;
		/// <summary>
		/// �ڕWFPS
		/// </summary>
		public int FPS {
			get { return fps; }
			set {
				fps = value;
				ticksSpan = ticksPerSec / fps;
			}
		}

		/// <summary>
		/// �ڕWFPS�ɑ΂��đ傫���x�ꂽ�ۂɁA�^�C�}�[�𑁑��肵���Ԋu���ێ����悤�Ƃ��邩�ǂ����B
		/// </summary>
		public bool AutoFastForward { get; set; }

		/// <summary>
		/// �ڕW�Ƃ���FPS�ƂP�b�Ԃ������Ticks���w�肵�āAGameTimer�C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="fps">�ڕW�Ƃ���FPS�B</param>
		/// <param name="ticksPerSec">��b�ԓ������DateTime��Ticks���i�ޗʁB</param>
		[Obsolete("�񐄏��ł��B�����P�̃R���X�g���N�^���g�p���Ă��������B")]
		public GameTimer(int fps, long ticksPerSec) {
			this.ticksPerSec = ticksPerSec;
			FPS = fps;
			ticksSpan = ticksPerSec / fps;

			ticksNext = -1L;
		}

		/// <summary>
		/// �ڕW�Ƃ���FPS���w�肵�āAGameTimer�C���X�^���X���쐬���܂��B
		/// �P�b�Ԃ������Ticks�́A���\�b�h���Ōv�Z����܂��B
		/// </summary>
		/// <param name="targetFPS">�ڕW�Ƃ���FPS�B</param>
		public GameTimer(int fps) {
			this.ticksPerSec = TimeSpan.TicksPerSecond;
			FPS = fps;

			ticksNext = -1L;
		}

		/// <summary>
		/// �P�b�Ԃ������Tick�����v�Z����B
		/// </summary>
		/// <param name="testtime_millisec">�v�Z���邽�߂ɑ҂��ԁB</param>
		/// <returns>�v�Z���ꂽTick���B</returns>
		/// <remarks>���̊֐��͎��ۂɎ��Ԃ̌o�߂�҂��Ƃő��肵�Ă���B</remarks>
		[Obsolete("�񐄏��ł��BTimeSpan.TicksPerSecond���g�p���Ă��������B")]
		public static long CalcTPS(int testtime_millisec) {
			long time_a, time_b;
			time_a = DateTime.Now.Ticks;
			Thread.Sleep(testtime_millisec);
			time_b = DateTime.Now.Ticks;

			return (time_b - time_a) * 1000 / testtime_millisec;
		}

		/// <summary>
		/// ���Ԍv�Z�̊�_�����݂̎��ԂɍX�V����B
		/// </summary>
		public void UpdateStartPoint() {
			ticksNext = DateTime.Now.Ticks + ticksSpan;
		}

		/// <summary>
		/// ��_�����łɎw�肳��A���p�\�ȏ�Ԃł��邩�B
		/// </summary>
		public bool Enabled {
			get { return ticksNext != -1L; }
		}

		bool spinWait = false;

		/// <summary>
		/// ���Ԃ�҂ۂɁA�󃋁[�v���g�p���邩�ǂ����B
		/// </summary>
		public bool UseSpinWait {
			get { return spinWait; }
			set { spinWait = value; }
		}

		/// <summary>
		/// �ݒ肳�ꂽ���Ԃ�҂B
		/// </summary>
		/// <returns>�ݒ肳�ꂽ���Ԃ܂ł��Ɖ��~���b�c���Ă��邩��\�����l�B���̏ꍇ�͐ݒ肳�ꂽ���Ԃ��߂��Ă���B</returns>
		public int Wait() {
			//			long now = DateTime.Now.Ticks;

			// ���b�҂Ă΂悢�����v�Z
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
					// �҂��Ă��Ď��Ԃ��߂��Ă��܂��Ă͌����q���Ȃ��̂ŁA�P�~���b�Z������
					// �Ӗ������邩�H�@�����ˁB
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
	/// �[���I�ȃv���p�e�B
	/// </summary>
	/// <typeparam name="T">�i�[����l�̌^</typeparam>
	public class Property<T> {
		private Func<T> getter;
		private Action<T> setter;

		public Property(Func<T> _getter) {
			if (_getter == null) throw new ArgumentNullException("_getter ��null���w�肷�邱�Ƃ͂ł��܂���B");
			this.getter = _getter;
			this.setter = null;
		}
		public Property(Func<T> _getter, Action<T> _setter) {
			if (_getter == null) throw new ArgumentNullException("_getter ��null���w�肷�邱�Ƃ͂ł��܂���B");
			this.getter = _getter;
			this.setter = _setter;
		}

		public T Value {
			get { return getter(); }
			set {
				if (setter == null) throw new InvalidOperationException("setter���o�^����Ă��Ȃ����߁Aset�͕s���ȏ����ł��B");
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
	/// ���b�N���ꂽ�f�[�^�o�b�t�@��\���N���X
	/// </summary>
	/// <typeparam name="T">�o�b�t�@����1�v�f�̌^</typeparam>
	public class BufferData<T> : IDisposable where T : struct {
		/// <summary>
		/// ���b�N���������邽�߂̃��\�b�h��\���f���Q�[�g
		/// </summary>
		/// <param name="data">���f������f�[�^</param>
		public delegate void UnlockCallback(T[] data);
		UnlockCallback callback;

		/// <summary>
		/// �o�b�t�@�Ɋi�[����Ă���f�[�^��
		/// </summary>
		public T[] Data { get; set; }

		/// <summary>
		/// �o�b�t�@���쐬����B
		/// </summary>
		/// <param name="callback">���b�N�����p�R�[���o�b�N</param>
		/// <param name="data">���b�N���ꂽ�f�[�^��</param>
		public BufferData(UnlockCallback callback, T[] data) {
			this.callback = callback;
			this.Data = data;
		}

		/// <summary>
		/// ���b�N����������B
		/// </summary>
		public void Dispose() {
			this.callback(Data);
		}

		/// <summary>
		/// �����ւ��邽�߂̃f�[�^�z����쐬���郆�[�e�B���e�B�֐�
		/// </summary>
		/// <param name="size">�쐬����z��̃T�C�Y</param>
		/// <returns>�쐬���ꂽ�z��</returns>
		public T[] MakeArray(int size) {
			return new T[size];
		}
	}


}
