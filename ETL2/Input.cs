using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Totoki.ETL {
	public static class Input {
		//static KeyboardManager kbm;
		static KeyboardState stat;

		public static void Initialize() {
			InitializeKeyboard();
			InitializeJoystick();
		}

		private static void InitializeKeyboard() {
			//kbm = new KeyboardManager(Program.Instance);
		}

		public static void UpdateKeyboard() {
			//stat = kbm.GetState();
			stat = Keyboard.GetState();
		}

		public static bool IsPushed(InputUnit u) {

			AxisType ax = AxisType.None;
			JoypadButtons btn = JoypadButtons.None;
			for (int i = 0; i < JoystickCount; i++) {
				if (!joystickAvailable[i]) continue;
				switch (u) {
					case InputUnit.Up:
						ax = Program.Configuration.Input.UpAxis;
						break;
					case InputUnit.Down:
						ax = Program.Configuration.Input.UpAxis.Invert();
						break;
					case InputUnit.Left:
						ax = Program.Configuration.Input.LeftAxis;
						break;
					case InputUnit.Right:
						ax = Program.Configuration.Input.LeftAxis.Invert();
						break;
					case InputUnit.MainAction:
						btn = Program.Configuration.Input.MainActionButton;
						break;
					case InputUnit.Subaction:
						btn = Program.Configuration.Input.SubActionButton;
						break;
					case InputUnit.Optional:
						btn = Program.Configuration.Input.OptionalButton;
						break;
					case InputUnit.Menu:
						btn = Program.Configuration.Input.MenuButton;
						break;
				}

				float threshold = Program.Configuration.Input.AxisDigitalThreshold;

				if ((joystickState[i].Buttons & btn) != JoypadButtons.None) {
					return true;
				}
				else if (joystickState[i].Axis.Get((JoystickID)i, ax) > threshold) {
					return true;
				}
				else if ((joystickState[i].Flags & JoyInfoFlags.ReturnPOV) != 0) {
					Program.AddStatistics("POV", "[#{0}]{1}", i, joystickState[i].POV);
					if (joystickState[i].POV != 0xffff) {
						double povAngle = joystickState[i].POV / 18000.0 * Math.PI;
						// (0, -1)を0度として右回り
						double x = Math.Sin(povAngle);
						double y = -Math.Cos(povAngle);
						switch (ax) {
							case AxisType.XPlus:
								if (x > threshold) return true;
								break;
							case AxisType.XMinus:
								if (-x > threshold) return true;
								break;
							case AxisType.YPlus:
								if (y > threshold) return true;
								break;
							case AxisType.YMinus:
								if (-y > threshold) return true;
								break;
						}
					}
				}
			}


			Keys k = Keys.None;
			switch (u) {
				case InputUnit.Up:
					k = Program.Configuration.Input.UpKey;
					break;
				case InputUnit.Down:
					k = Program.Configuration.Input.DownKey;
					break;
				case InputUnit.Left:
					k = Program.Configuration.Input.LeftKey;
					break;
				case InputUnit.Right:
					k = Program.Configuration.Input.RightKey;
					break;
				case InputUnit.MainAction:
					k = Program.Configuration.Input.MainActionKey;
					break;
				case InputUnit.Subaction:
					k = Program.Configuration.Input.SubActionKey;
					break;
				case InputUnit.Optional:
					k = Program.Configuration.Input.OptionalKey;
					break;
				case InputUnit.Menu:
					k = Program.Configuration.Input.MenuKey;
					break;
			}

			return stat.IsKeyDown(k);
		}

		public static float GetUpDownAxisValue() {
			return GetAxisValueInternal(
				Program.Configuration.Input.UpKey,
				Program.Configuration.Input.DownKey,
				Program.Configuration.Input.UpAxis,
				0
			);
		}

		public static float GetLeftRightAxisValue() {
			return GetAxisValueInternal(
				Program.Configuration.Input.LeftKey,
				Program.Configuration.Input.RightKey,
				Program.Configuration.Input.LeftAxis,
				MathF.PI / 2
			);
		}

		private static float GetAxisValueInternal(Keys plusKey, Keys minusKey, AxisType axis, float povAngleOffset) {
			for (int i = 0; i < JoystickCount; i++) {
				if (!joystickAvailable[i]) continue;

				float threshold = Program.Configuration.Input.AxisAnalogThreshold;

				var value = joystickState[i].Axis.Get((JoystickID)i, axis);
				if (value < -threshold || threshold < value) {
					return value;
				}

				if ((joystickState[i].Flags & JoyInfoFlags.ReturnPOV) != 0) {
					//Program.AddStatistics("POV", "[#{0}]{1}", i, joystickState[i].POV);
					if (joystickState[i].POV != 0xffff) {
						var povAngle = joystickState[i].POV / 18000.0f * MathF.PI;

						return MathF.Cos(povAngle + povAngleOffset);
					}
				}
			}


			return (stat.IsKeyDown(plusKey) ? 1.0f : 0.0f)
				+ (stat.IsKeyDown(minusKey) ? -1.0f : 0.0f);
		}

		#region Joystick P/Invoke
		/*
typedef struct joyinfoex_tag {
    DWORD dwSize;                // size of structure //
    DWORD dwFlags;               // flags to indicate what to return //
    DWORD dwXpos;                // x position //
    DWORD dwYpos;                // y position //
    DWORD dwZpos;                // z position //
    DWORD dwRpos;                // rudder/4th axis position //
    DWORD dwUpos;                // 5th axis position //
    DWORD dwVpos;                // 6th axis position //
    DWORD dwButtons;             // button states //
    DWORD dwButtonNumber;        // current button number pressed //
    DWORD dwPOV;                 // point of view state //
    DWORD dwReserved1;           // reserved for communication between winmm & driver //
    DWORD dwReserved2;           // reserved for future expansion //
} JOYINFOEX, *PJOYINFOEX, NEAR *NPJOYINFOEX, FAR *LPJOYINFOEX;
		*/

		[StructLayout(LayoutKind.Sequential)]
		private struct JoyInfoEx {
			public uint Size;         /* size of structure */
			public JoyInfoFlags Flags;        /* flags to indicate what to return */
			[StructLayout(LayoutKind.Sequential)]
			public struct JoypadAxis {
				public uint Xpos;         /* x position */
				public uint Ypos;         /* y position */
				public uint Zpos;         /* z position */
				public uint Rpos;         /* rudder/4th axis position */
				public uint Upos;         /* 5th axis position */
				public uint Vpos;         /* 6th axis position */

				public float GetX(JoystickID id) { return 2 * ((float)Xpos + joystickCaps[(int)id].wXmin) / (joystickCaps[(int)id].wXmax - joystickCaps[(int)id].wXmin) - 1; }
				public float GetY(JoystickID id) { return 2 * ((float)Ypos + joystickCaps[(int)id].wYmin) / (joystickCaps[(int)id].wYmax - joystickCaps[(int)id].wYmin) - 1; }
				public float GetZ(JoystickID id) { return 2 * ((float)Zpos + joystickCaps[(int)id].wZmin) / (joystickCaps[(int)id].wZmax - joystickCaps[(int)id].wZmin) - 1; }
				public float GetR(JoystickID id) { return 2 * ((float)Rpos + joystickCaps[(int)id].wRmin) / (joystickCaps[(int)id].wRmax - joystickCaps[(int)id].wRmin) - 1; }
				public float GetU(JoystickID id) { return 2 * ((float)Upos + joystickCaps[(int)id].wUmin) / (joystickCaps[(int)id].wUmax - joystickCaps[(int)id].wUmin) - 1; }
				public float GetV(JoystickID id) { return 2 * ((float)Vpos + joystickCaps[(int)id].wVmin) / (joystickCaps[(int)id].wVmax - joystickCaps[(int)id].wVmin) - 1; }

				public float Get(JoystickID id, AxisType axis) {
					// 軸から値の取得
					switch (axis) {
						case AxisType.XPlus:
						case AxisType.XMinus:
							return GetX(id) * axis.GetSign();
						case AxisType.YPlus:
						case AxisType.YMinus:
							return GetY(id) * axis.GetSign();
						case AxisType.ZPlus:
						case AxisType.ZMinus:
							return GetZ(id) * axis.GetSign();
						case AxisType.RPlus:
						case AxisType.RMinus:
							return GetR(id) * axis.GetSign();
						case AxisType.UPlus:
						case AxisType.UMinus:
							return GetU(id) * axis.GetSign();
						case AxisType.VPlus:
						case AxisType.VMinus:
							return GetV(id) * axis.GetSign();
					}
					return float.NaN;
				}
			}
			public JoypadAxis Axis;
			public JoypadButtons Buttons;      /* button states */
			public uint ButtonNumber; /* current button number pressed */
			public uint POV;          /* point of view state */
			public uint Reserved1;    /* reserved for communication between winmm & driver */
			public uint Reserved2;    /* reserved for future expansion */


		}
		/*
		 * typedef struct tagJOYCAPS2W {
    WORD    wMid;                // manufacturer ID //
    WORD    wPid;                // product ID //
    WCHAR   szPname[MAXPNAMELEN];// product name (NULL terminated string) //
    UINT    wXmin;               // minimum x position value //
    UINT    wXmax;               // maximum x position value //
    UINT    wYmin;               // minimum y position value //
    UINT    wYmax;               // maximum y position value //
    UINT    wZmin;               // minimum z position value //
    UINT    wZmax;               // maximum z position value //
    UINT    wNumButtons;         // number of buttons //
    UINT    wPeriodMin;          // minimum message period when captured //
    UINT    wPeriodMax;          // maximum message period when captured //
    UINT    wRmin;               // minimum r position value //
    UINT    wRmax;               // maximum r position value //
    UINT    wUmin;               // minimum u (5th axis) position value //
    UINT    wUmax;               // maximum u (5th axis) position value //
    UINT    wVmin;               // minimum v (6th axis) position value //
    UINT    wVmax;               // maximum v (6th axis) position value //
    UINT    wCaps;               // joystick capabilites //
    UINT    wMaxAxes;            // maximum number of axes supported //
    UINT    wNumAxes;            // number of axes in use //
    UINT    wMaxButtons;         // maximum number of buttons supported //
    WCHAR   szRegKey[MAXPNAMELEN];// registry key //
    WCHAR   szOEMVxD[MAX_JOYSTICKOEMVXDNAME]; // OEM VxD in use //
    GUID    ManufacturerGuid;    // for extensible MID mapping //
    GUID    ProductGuid;         // for extensible PID mapping //
    GUID    NameGuid;            // for name lookup in registry //
} JOYCAPS2W, *PJOYCAPS2W, *NPJOYCAPS2W, *LPJOYCAPS2W;
		 */
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct Guid {
			public int Data1;
			public short Data2;
			public short Data3;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Data4;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct JoystickCaps {
			public ushort wMid;              /* manufacturer ID */
			public ushort wPid;              /* product ID */
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
			public string szPname;           /* product name (NULL terminated string) */
			public uint wXmin;               /* minimum x position value */
			public uint wXmax;               /* maximum x position value */
			public uint wYmin;               /* minimum y position value */
			public uint wYmax;               /* maximum y position value */
			public uint wZmin;               /* minimum z position value */
			public uint wZmax;               /* maximum z position value */
			public uint wNumButtons;         /* number of buttons */
			public uint wPeriodMin;          /* minimum message period when captured */
			public uint wPeriodMax;          /* maximum message period when captured */
			public uint wRmin;               /* minimum r position value */
			public uint wRmax;               /* maximum r position value */
			public uint wUmin;               /* minimum u (5th axis) position value */
			public uint wUmax;               /* maximum u (5th axis) position value */
			public uint wVmin;               /* minimum v (6th axis) position value */
			public uint wVmax;               /* maximum v (6th axis) position value */
			public uint wCaps;               /* joystick capabilites */
			public uint wMaxAxes;            /* maximum number of axes supported */
			public uint wNumAxes;            /* number of axes in use */
			public uint wMaxButtons;         /* maximum number of buttons supported */
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
			public string szRegKey;          /* registry key */
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_JOYSTICKOEMVXDNAME)]
			public string szOEMVxD;          /* OEM VxD in use */
			public Guid ManufacturerGuid;    /* for extensible MID mapping */
			public Guid ProductGuid;         /* for extensible PID mapping */
			public Guid NameGuid;            /* for name lookup in registry */

			public static uint Size { get { return (uint)Marshal.SizeOf(typeof(JoystickCaps)); } }
		}

		private const int MMSYSERR_BASE = 0;
		private const int JOYERR_BASE = 160;
		private const int MAXPNAMELEN = 32;
		private const int MAX_JOYSTICKOEMVXDNAME = 260;

		private enum MmResult : uint {
			NoError = 0,
			BadDeviceID = (MMSYSERR_BASE + 2),   /* device ID out of range */
			InvalidParam = (MMSYSERR_BASE + 11), /* invalid parameter passed */
			NoDriver = (MMSYSERR_BASE + 6),      /* no device driver present */
			BadParam = (JOYERR_BASE + 5),        /* bad parameters */
			Unplugged = (JOYERR_BASE + 7),       /* joystick is unplugged */
		}
		private enum POVState : uint {
			Centered = unchecked((uint)-1),
			Forward = 0,
			Right = 9000,
			Backward = 18000,
			Left = 27000,
		}
		[Flags]
		private enum JoyInfoFlags : uint {
			ReturnX = 0x00000001,
			ReturnY = 0x00000002,
			ReturnZ = 0x00000004,
			ReturnR = 0x00000008,
			ReturnU = 0x00000010,     /* axis 5 */
			ReturnV = 0x00000020,     /* axis 6 */
			ReturnPOV = 0x00000040,
			ReturnButtons = 0x00000080,
			ReturnRawData = 0x00000100,
			ReturnPOVCTS = 0x00000200,
			ReturnCentered = 0x00000400,
			UseDeadZone = 0x00000800,
			ReturnALL = (ReturnX | ReturnY | ReturnZ |
									 ReturnR | ReturnU | ReturnV |
									 ReturnPOV | ReturnButtons),
		}
		private enum JoystickID : uint {
			Joystick1 = 0,
			Joystick2 = 1,
		}
		private const int JoystickCount = 2;

		[DllImport("winmm.dll")]
		static extern MmResult joyGetPosEx(JoystickID id, out JoyInfoEx joyInfo);
		[DllImport("winmm.dll", CharSet = CharSet.Unicode, EntryPoint = "joyGetDevCapsW")]
		static extern MmResult joyGetDevCaps(JoystickID id, out JoystickCaps caps, uint capsSize);

		#endregion

		static JoystickCaps[] joystickCaps;
		static bool[] joystickAvailable;
		static JoyInfoEx[] joystickState;

		public static void InitializeJoystick() {
			joystickCaps = new JoystickCaps[JoystickCount];
			joystickAvailable = new bool[JoystickCount];
			joystickState = new JoyInfoEx[JoystickCount];

			MmResult ret = MmResult.NoError;

			for (int i = 0; i < JoystickCount; i++) {
				switch (ret = joyGetDevCaps((JoystickID)i, out joystickCaps[i], JoystickCaps.Size)) {
					case MmResult.NoError:
						joystickAvailable[i] = true;
						joystickState[i].Size = (uint)Marshal.SizeOf(typeof(JoyInfoEx));
						joystickState[i].Flags = JoyInfoFlags.ReturnALL;
						break;
					default:
						Debug.WriteLine(ret, "Joystick" + i.ToString());
						joystickAvailable[i] = false;
						break;
				}
			}

		}

		public static void UpdateJoystick() {
			for (int i = 0; i < JoystickCount; i++) {
				if (!joystickAvailable[i]) continue;
				MmResult ret = MmResult.NoError;

				switch (ret = joyGetPosEx((JoystickID)i, out joystickState[i])) {
					case MmResult.NoError:
						// ここにエラー発生数をリセットする処理を追加する。
						break;
					default:
						// ここにエラー発生数をカウントアップする処理を追加する。
						break;
				}
			}
		}

	}

	public enum AxisType : int {
		XPlus, XMinus,
		YPlus, YMinus,
		ZPlus, ZMinus,
		RPlus, RMinus,
		UPlus, UMinus,
		VPlus, VMinus,
		None = -1, None2 = -2,
	}
	[Flags]
	public enum JoypadButtons : uint {
		B01 = 0x0001, B02 = 0x0002, B03 = 0x0004, B04 = 0x0008,
		B05 = 0x0010, B06 = 0x0020, B07 = 0x0040, B08 = 0x0080,
		B09 = 0x0100, B10 = 0x0200, B11 = 0x0400, B12 = 0x0800,
		B13 = 0x1000, B14 = 0x2000, B15 = 0x4000, B16 = 0x8000,
		B17 = 0x00010000, B18 = 0x00020000, B19 = 0x00040000, B20 = 0x00080000,
		B21 = 0x00100000, B22 = 0x00200000, B23 = 0x00400000, B24 = 0x00800000,
		B25 = 0x01000000, B26 = 0x02000000, B27 = 0x04000000, B28 = 0x08000000,
		B29 = 0x10000000, B30 = 0x20000000, B31 = 0x40000000, B32 = 0x80000000,

		None = 0x00000000,
	}


	public class InputSettings {
		public Keys UpKey { get; set; }
		public Keys DownKey { get; set; }
		public Keys LeftKey { get; set; }
		public Keys RightKey { get; set; }
		public Keys MainActionKey { get; set; }
		public Keys SubActionKey { get; set; }
		public Keys OptionalKey { get; set; }
		public Keys MenuKey { get; set; }

		public AxisType UpAxis { get; set; }
		public AxisType LeftAxis { get; set; }
		public float AxisAnalogThreshold { get; set; }
		public float AxisDigitalThreshold { get; set; }

		public JoypadButtons MainActionButton { get; set; }
		public JoypadButtons SubActionButton { get; set; }
		public JoypadButtons OptionalButton { get; set; }
		public JoypadButtons MenuButton { get; set; }

		public InputSettings() {
			UpKey = Keys.Up;
			DownKey = Keys.Down;
			LeftKey = Keys.Left;
			RightKey = Keys.Right;
			MainActionKey = Keys.Z;
			SubActionKey = Keys.X;
			OptionalKey = Keys.C;
			MenuKey = Keys.Escape;

			UpAxis = AxisType.YMinus;
			LeftAxis = AxisType.XMinus;
			AxisAnalogThreshold = 0.1f;
			AxisDigitalThreshold = 0.4f;

			MainActionButton = JoypadButtons.B01;
			SubActionButton = JoypadButtons.B02;
			OptionalButton = JoypadButtons.B03;
			MenuButton = JoypadButtons.B04;
		}

	}

	public enum InputUnit {
		None,
		Up,
		Down,
		Left,
		Right,
		MainAction,
		Subaction,
		Optional,
		Menu,
	}
}
