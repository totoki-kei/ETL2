using System;
using System.Text;

namespace Totoki.Util {

	/// <summary>
	/// 文字列に関する小物関数を提供するクラス。
	/// </summary>
	public static class StringHelper {
		/// <summary>
		/// StringHelper.Escapeによってエスケープされた文字を復元し、有効な文字列にします。
		/// </summary>
		/// <param name="str">復元する文字列</param>
		/// <returns>復元された文字列</returns>
		public static string Unescape(this string str) {
			StringBuilder buff = new StringBuilder(str);
			buff = buff.Replace("\\\\", "\xFFFF");

			buff = buff.Replace("\\t", "\t");
			buff = buff.Replace("\\r", "\r");
			buff = buff.Replace("\\v", "\v");
			buff = buff.Replace("\\n", "\n");
			//buff = buff.Replace("\\qd", "\"");
			//buff = buff.Replace("\\qs", "'");

			buff = buff.Replace("\xFFFF", "\\");

			return buff.ToString();
		}

		/// <summary>
		/// 文字列を、単一行に収まるようエスケープします。
		/// </summary>
		/// <param name="str">エスケープ対象の文字列</param>
		/// <returns>エスケープ済み文字列</returns>
		public static string Escape(this string str) {
			StringBuilder buff = new StringBuilder(str);
			buff = buff.Replace("\\", "\xFFFF");

			buff = buff.Replace("\n", "\\n");
			buff = buff.Replace("\v", "\\v");
			buff = buff.Replace("\r", "\\r");
			buff = buff.Replace("\t", "\\t");
			//buff = buff.Replace("\"", "\\qd");
			//buff = buff.Replace("'", "\\qs");

			buff = buff.Replace("\xFFFF", "\\\\");

			return buff.ToString();
		}

		public static string ReadToken(this string src, ref int p) {
			StringBuilder str = new StringBuilder();

			while (src.Length > p && char.IsWhiteSpace(src[p])) p++;

			if (p == src.Length) return null;

			Predicate<char> chk = c => true;

			if (src[p] == '"') {
				// クォートされた文字列
				chk = c => {
					if (c == '"') chk = cc => false;
					return true;
				};
			}
			//else if (char.IsDigit(src[p]) || src[p] == '.' || src[p] == '-') {
			else if (char.IsDigit(src[p]) || src[p] == '.') {
				// 数値
				chk = c => {
					if (c == '.') {
						var oldchk = chk;
						chk = cc => cc != '.' && oldchk(cc);
					}
					//if (c == '-') {
					//    var oldchk = chk; 
					//    chk = cc => cc != '-' && oldchk(cc);
					//}
					//return char.IsDigit(c) || c == '.' || c == '-';
					return char.IsDigit(c) || c == '.';
				};
			}
			else if (char.IsLetter(src[p])) {
				// 文字
				chk = c => char.IsLetterOrDigit(c);
			}
			else {
				chk = c => false;
			}

			do {
				char c = src[p++];
				str.Append(c);
			} while (src.Length > p && chk(src[p]));

			return str.ToString();
		}

		public static Tuple<int, int> IndexToLineColumn(this string src, int index) {
			int line = 1;
			int p = 0;

			while (true) {
				int cr = src.IndexOf('\r', p, index - p);
				int lf = src.IndexOf('\n', p, index - p);

				int point = Math.Max(cr, lf);
				// CRもLFも見当たらない場合は行探索終了
				if (point < 0) break;

				line++;
				p += point;
			}

			return new Tuple<int, int>(line, index - p + 1);
		}
	}
}
