using System;
using System.Collections.Generic;
using System.Text;

namespace Totoki.Util {
	/// <summary>
	/// 符号なし整数と36進数文字列の相互変換を行うクラス。
	/// </summary>
	public static class Base36 {
		/// <summary>
		/// 36進数文字列を符号なし整数に変換します。
		/// </summary>
		/// <param name="base36">36進数文字列</param>
		/// <returns>整数値</returns>
		/// <exception cref="FormatException">引数base36が、有効な36進数文字列ではない。</exception>
		public static uint ToUInt(string base36) {
			uint buff = 0;
			int strlen = base36.Length;
			char c;
			uint err = 0;
			for (int i = 0; i < strlen; i++) {
				c = base36[i];
				buff *= 36;
				buff += ('0' <= c && c <= '9') ? (uint)(c - '0') :
					('a' <= c && c <= 'z') ? (uint)(c - 'a' + 10) :
					('A' <= c && c <= 'Z') ? (uint)(c - 'A' + 10) : (err = 1u);
				if (err != 0)
					throw new FormatException(base36 + "は有効な36進数ではありません。");

			}

			return buff;
		}

		/// <summary>
		/// 整数値を36進数文字列に変換します。
		/// </summary>
		/// <param name="Num">変換対象の数値</param>
		/// <returns>36進数文字列</returns>
		public static string ToString(uint Num) {
			Stack<char> cs = new Stack<char>();
			StringBuilder buff = new StringBuilder();

			long lnum = Num;
			long lrem = 0;

			while (lnum != 0) {
				lnum = Math.DivRem(lnum, 36, out lrem);
				cs.Push((0 <= lrem && lrem <= 9) ? (char)(lrem + '0') : (char)(lrem - 10 + 'A'));
			}

			while (cs.Count > 0)
				buff.Append(cs.Pop());

			return buff.Length > 0 ? buff.ToString() : "0";
		}

	}

}
