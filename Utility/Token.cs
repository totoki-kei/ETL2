using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Totoki.Util {
	public class Token<T> {
		public string Word { get; private set; }
		public int Index { get; private set; }
		public T Tag { get; private set; }

		public static IEnumerator<Token<T>> Split(string str, IDictionary<string, T> dic) {
			StringBuilder sb = new StringBuilder(str);
			Dictionary<string, Regex> regexList = new Dictionary<string, Regex>();
			int i = 0;
			Token<T> t;
			do {
				yield return t = ReadToken(sb, dic, regexList, ref i);
			} while (t != null);
		}

		static Token<T> ReadToken(StringBuilder src, IDictionary<string, T> dic, IDictionary<string, Regex> regexList, ref int p) {
			StringBuilder str = src;

			while (src.Length > p && char.IsWhiteSpace(src[p])) p++;

			if (p == src.Length) return null;

			int index = p;

			{
				string subsrc = str.ToString(p, src.Length - p);
				foreach (var ptn in dic.Keys) {
					Regex regex = null;
					if (regexList.ContainsKey(ptn)) {
						regex = regexList[ptn];
					}
					else {
						regex = new Regex(ptn);
						regexList.Add(ptn, regex);
					}
					var match = regex.Match(subsrc);
					if (match.Success) {
						p += match.Captures[0].Length;
						return new Token<T>() { Word = match.Captures[0].Value, Index = index, Tag = dic[ptn] };
					}
				}
			}

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

			return new Token<T> { Word = str.ToString(), Tag = default(T) };
		}
	}

}
