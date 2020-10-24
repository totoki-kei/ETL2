#if !UTIL_NO_SYSTEM_DRAWING

using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Totoki.Util {
	/// <summary>
	/// 色コードのみの色構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SimpleColor {
		int code;

		/// <summary>
		/// SimpleColorの作成
		/// </summary>
		/// <param name="baseColor">元にする System.Drawing.Color 構造体</param>
		public SimpleColor(Color baseColor) {
			code = baseColor.ToArgb();
		}

		/// <summary>
		/// SimpleColorの作成
		/// </summary>
		/// <param name="colorCode">元にする色コード</param>
		public SimpleColor(int colorCode) {
			this.code = colorCode;
		}

		/// <summary>
		/// SimpleColorの作成
		/// </summary>
		/// <param name="alpha">アルファ値</param>
		/// <param name="red">赤要素</param>
		/// <param name="green">緑要素</param>
		/// <param name="blue">青要素</param>
		public SimpleColor(int alpha, int red, int green, int blue) {
			code = ToArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
		}

		/// <summary>
		/// SimpleColorの作成
		/// </summary>
		/// <param name="red">赤要素</param>
		/// <param name="green">緑要素</param>
		/// <param name="blue">青要素</param>
		public SimpleColor(int red, int green, int blue) {
			code = ToArgb((byte)red, (byte)green, (byte)blue);
		}

		/// <summary>
		/// アルファ値
		/// </summary>
		public byte A {
			get {
				return (byte)(code >> 0x18);
			}
			set {
				code = (int)((uint)(value << 0x18) | ((uint)code & 0x00FFFFFF));
			}
		}

		/// <summary>
		/// 赤要素
		/// </summary>
		public byte R {
			get {
				return (byte)((code >> 0x10) & 0xFF);
			}
			set {
				code = (int)((uint)(value << 0x10) | ((uint)code & 0xFF00FFFF));
			}
		}

		/// <summary>
		/// 緑要素
		/// </summary>
		public byte G {
			get {
				return (byte)((code >> 0x8) & 0xFF);
			}
			set {
				code = (int)((uint)(value << 8) | ((uint)code & 0xFFFF00FF));
			}
		}

		/// <summary>
		/// 青要素
		/// </summary>
		public byte B {
			get {
				return (byte)(code & 0xFF);
			}
			set {
				code = (int)(value | ((uint)code & 0xFFFFFF00));
			}
		}

		/// <summary>
		/// 色コード
		/// </summary>
		public int Argb {
			get {
				return code;
			}
			set {
				code = value;
			}
		}

		/// <summary>
		/// System.Drawing.Color 構造体
		/// </summary>
		public Color Color {
			get {
				return Color.FromArgb(code);
			}
			set {
				code = value.ToArgb();
			}
		}

		/// <summary>
		/// 文字列に変換
		/// </summary>
		/// <returns>このインスタンスを表す文字列</returns>
		public override string ToString() {
			StringBuilder sb = new StringBuilder(32);

			sb.Append("SimpleColor[0x");
			sb.Append(code.ToString("X").PadLeft(8, '0'));
			sb.Append("]");

			return sb.ToString();
		}

		/// <summary>
		/// 2つのオブジェクトが等価かを調べる
		/// </summary>
		/// <param name="obj">比較対象の System.Object</param>
		/// <returns>同一であれば true 、そうでないときは false</returns>
		public override bool Equals(object obj) {
			return obj is SimpleColor && ((SimpleColor)obj).code == this.code;
		}

		/// <summary>
		/// この色のハッシュコードを得る
		/// </summary>
		/// <returns>ハッシュ値</returns>
		public override int GetHashCode() {
			return code;
		}

		/// <summary>
		/// 構造体を介さず、直接色コードを得る
		/// </summary>
		/// <param name="alpha">アルファ値</param>
		/// <param name="red">赤要素</param>
		/// <param name="green">緑要素</param>
		/// <param name="blue">青要素</param>
		/// <returns>色コード</returns>
		public static int ToArgb(byte alpha, byte red, byte green, byte blue) {
			int c;
			c = red << 0x10;
			c |= green << 8;
			c |= blue;
			c |= alpha << 0x18;

			return c;
		}

		/// <summary>
		/// 構造体を介さず、直接色コードを得る
		/// </summary>
		/// <param name="red">赤要素</param>
		/// <param name="green">緑要素</param>
		/// <param name="blue">青要素</param>
		/// <returns>色コード</returns>
		public static int ToArgb(byte red, byte green, byte blue) {
			return ToArgb(0xFF, red, green, blue);
		}

		/// <summary>
		/// 構造体を介さず、直接色コードを得る
		/// </summary>
		/// <param name="alpha">アルファ値</param>
		/// <param name="color">ベースとなる色</param>
		/// <returns>色コード</returns>
		public static int ToArgb(int alpha, Color color) {
			return color.ToArgb() & 0x00FFFFFF | ((byte)alpha << 24);
		}

		/// <summary>
		/// 2つの色のアルファブレンディングを行い、色を更新する
		/// </summary>
		/// <param name="src">下になる色</param>
		/// <returns>更新されたインスタンス自身</returns>
		public SimpleColor BlendTo(SimpleColor colorBase) {
			this.code = Blend(this, colorBase);
			return this;
		}

		/// <summary>
		/// 2つの色のアルファブレンディングを行う
		/// </summary>
		/// <param name="colorUpper">2色のうち、上になる色</param>
		/// <param name="colorLower">2色のうち、下になる色</param>
		/// <returns>合成された色</returns>
		public static SimpleColor Blend(SimpleColor colorUpper, SimpleColor colorLower) {
			int a1 = colorUpper.A,
				a2 = colorLower.A,
				a12 = a1 * 255,
				a22 = a2 * 255;
			int a = a12 + a22 - a1 * a2;
			return new SimpleColor(
				a / 255,
				(a12 * colorUpper.R + a22 * colorLower.R - a1 * a2 * colorLower.R) / a,
				(a12 * colorUpper.G + a22 * colorLower.G - a1 * a2 * colorLower.G) / a,
				(a12 * colorUpper.B + a22 * colorLower.B - a1 * a2 * colorLower.B) / a
				);
		}

		/// <summary>
		/// 32ビット整数への暗黙の変換。色コードを返す。
		/// </summary>
		/// <param name="color">変換元のSimpleColor構造体</param>
		/// <returns>色コード</returns>
		public static implicit operator int(SimpleColor color) {
			return color.code;
		}
	}

}

#endif
