/* JpnNumText 2016.11.26
 * Coded by Raymai97
 * In Visual Studio 2017 RC
 */

using System;
using System.Text;

namespace MaiSoft
{
	public class JpnNumText
	{
		private static readonly long ONE_MAN = 10000; // 万
		private static readonly long ONE_OKU = ONE_MAN * ONE_MAN; // 億
		private static readonly long ONE_CHO = ONE_OKU * ONE_MAN; // 兆
		private static readonly long VALID_MAX_INT = (ONE_CHO * ONE_MAN - 1);

		/// <summary>Return Romaji of Hiragana.</summary>
		public static string RomajiFromHira(string hira)
		{
			var sb = new StringBuilder(hira.Length * 2);
			bool ltsu = false; // 促音
			for (int i = 0; i < hira.Length; i++)
			{
				// First, we think as if every Hira is separated
				char ch = hira[i];
				if (ch == 'っ') { ltsu = true; continue; }
				var currRomaji = (
					ch == ' ' ? " " :
					ch == 'あ' ? "a" :
					ch == 'い' ? "i" :
					ch == 'う' ? "u" :
					ch == 'え' ? "e" :
					ch == 'お' ? "o" :
					ch == 'か' ? "ka" : ch == 'が' ? "ga" :
					ch == 'き' ? "ki" : ch == 'ぎ' ? "gi" :
					ch == 'く' ? "ku" : ch == 'ぐ' ? "gu" :
					ch == 'け' ? "ke" : ch == 'げ' ? "ge" :
					ch == 'こ' ? "ko" : ch == 'ご' ? "go" :
					ch == 'さ' ? "sa" : ch == 'ざ' ? "za" :
					ch == 'し' ? "shi" : ch == 'じ' ? "ji" :
					ch == 'す' ? "su" : ch == 'ず' ? "zu" :
					ch == 'せ' ? "se" : ch == 'ぜ' ? "ze" :
					ch == 'そ' ? "so" : ch == 'ぞ' ? "zo" :
					ch == 'た' ? "ta" : ch == 'だ' ? "da" :
					ch == 'ち' ? "chi" : ch == 'ぢ' ? "dzi" :
					ch == 'つ' ? "tsu" : ch == 'づ' ? "dzu" :
					ch == 'て' ? "te" : ch == 'で' ? "de" :
					ch == 'と' ? "to" : ch == 'ど' ? "do" :
					ch == 'な' ? "na" :
					ch == 'に' ? "ni" :
					ch == 'ぬ' ? "nu" :
					ch == 'ね' ? "ne" : 
					ch == 'の' ? "no" :
					ch == 'は' ? "ha" : ch == 'ば' ? "ba" : ch == 'ぱ' ? "pa" :
					ch == 'ひ' ? "hi" : ch == 'び' ? "bi" : ch == 'ぴ' ? "pi" :
					ch == 'ふ' ? "fu" : ch == 'ぶ' ? "bu" : ch == 'ぷ' ? "pu" :
					ch == 'へ' ? "he" : ch == 'べ' ? "be" : ch == 'ぺ' ? "pe" :
					ch == 'ほ' ? "ho" : ch == 'ぼ' ? "bo" : ch == 'ぽ' ? "po" :
					ch == 'ま' ? "ma" :
					ch == 'み' ? "mi" :
					ch == 'む' ? "mu" :
					ch == 'め' ? "me" :
					ch == 'も' ? "mo" :
					ch == 'や' ? "ya" : 
					ch == 'ゆ' ? "yu" :
					ch == 'よ' ? "yo" :
					ch == 'ら' ? "ra" :
					ch == 'り' ? "ri" :
					ch == 'る' ? "ru" :
					ch == 'れ' ? "re" :
					ch == 'ろ' ? "ro" :
					ch == 'わ' ? "wa" :
					ch == 'を' ? "wo" :
					ch == 'ん' ? "n" : string.Empty
				);
				// If next char is available, handle 'ゃ/ゅ/ょ' specifically.
				if (i + 1 < hira.Length)
				{
					char nextCh = hira[i + 1];
					if (ch == 'じ')
					{
						currRomaji =
							nextCh == 'ゃ' ? "ja" :
							nextCh == 'ゅ' ? "ju" :
							nextCh == 'ょ' ? "jo" : currRomaji;
					}
					else
					{
						var tail =
							nextCh == 'ゃ' ? "a" :
							nextCh == 'ゅ' ? "u" :
							nextCh == 'ょ' ? "o" : string.Empty;
						if (tail != string.Empty)
						{
							var mid = (currRomaji.Length == 2) ? "y" : "h";
							currRomaji = currRomaji[0] + mid + tail;
						}
					}
				}
				if (ltsu) // if 促音, prepend first char, like "pa" become "ppa"
				{
					ltsu = false;
					currRomaji = currRomaji[0] + currRomaji;
				}
				sb.Append(currRomaji);
			}
			return sb.ToString();
		}

		/// <param name="kanji">Kanji that represents number. Example: '三百四点六六'</param>
		/// <summary>Return Hiragana of Kanji that represents number.</summary>
		public static string HiraFromKanji(string kanji, bool useSpace)
		{
			var sb = new StringBuilder(kanji.Length * 2);
			if (kanji.StartsWith("マイナス"))
			{
				sb.Append("まいなす");
				if (useSpace) { sb.Append(" "); }
			}
			bool hadDot = false, skipOnce = false;
			for (int i = 0; i < kanji.Length; i++)
			{
				// If previous round was something like "sanbyaku",
				// we don't need another "hyaku", so we skip once.
				if (skipOnce) { skipOnce = false; continue; }
				// First, we think as if every Kanji is separated
				char ch = kanji[i];
				string currHira = (
					ch == '点' ? "てん" :
					ch == '十' ? "じゅう" :
					ch == '百' ? "ひゃく" :
					ch == '千' ? "せん" :
					ch == '万' ? "まん" :
					ch == '億' ? "おく" :
					ch == '兆' ? "ちょう" :
					ch == '零' ? "れい" :
					ch == '一' ? "いち" :
					ch == '二' ? "に" :
					ch == '三' ? "さん" :
					ch == '四' ? "よん" :
					ch == '五' ? "ご" :
					ch == '六' ? "ろく" :
					ch == '七' ? "なな" :
					ch == '八' ? "はち" :
					ch == '九' ? "きゅう" : string.Empty);
				if (ch == '点') { hadDot = true; }
				// If we haven't met '点' and next char is available,
				// try to merge some. For example, "san sen" become "sanzen".
				if (!hadDot && i + 1 < kanji.Length)
				{
					string mergedHira = string.Empty;
					char nextCh = kanji[i + 1];
					if (nextCh == '千')
					{
						mergedHira = (
							ch == '二' ? "にせん" :
							ch == '三' ? "さんぜん" :
							ch == '四' ? "よんせん" :
							ch == '五' ? "ごせん" :
							ch == '六' ? "ろくせん" :
							ch == '七' ? "ななせん" :
							ch == '八' ? "はっせん" :
							ch == '九' ? "きゅうせん" : string.Empty);
					}
					else if (nextCh == '百')
					{
						mergedHira = (
							ch == '二' ? "にひゃく" :
							ch == '三' ? "さんびゃく" :
							ch == '四' ? "よんひゃく" :
							ch == '五' ? "ごひゃく" :
							ch == '六' ? "ろっぴゃく" :
							ch == '七' ? "ななひゃく" :
							ch == '八' ? "はっぴゃく" :
							ch == '九' ? "きゅうひゃく" : string.Empty);
					}
					else if (nextCh == '十')
					{
						mergedHira = (
							ch == '二' ? "にじゅう" :
							ch == '三' ? "さんじゅう" :
							ch == '四' ? "よんじゅう" :
							ch == '五' ? "ごじゅう" :
							ch == '六' ? "ろくじゅう" :
							ch == '七' ? "ななじゅう" :
							ch == '八' ? "はちじゅう" :
							ch == '九' ? "きゅうじゅう" : string.Empty);
					}
					// If merge successfully, skip the next loop (once only)
					if (mergedHira != string.Empty)
					{
						currHira = mergedHira;
						skipOnce = true;
					}
				}
				// If current char makes sense, append it
				if (currHira != string.Empty)
				{
					sb.Append(currHira);
					if (useSpace) { sb.Append(" "); }
				}
			}
			return sb.ToString().Trim();
		}

		/// <param name="kanji">Kanji that represents number. Example: '三百四点六六'</param>
		/// <summary>Return Daiji representation of Kanji that represents number.</summary>
		public static string DaijiFromKanji(string kanji, bool useObsolete)
		{
			var sb = new StringBuilder(kanji);
			sb
			.Replace("一", "壹")
			.Replace("二", "貳")
			.Replace("三", "參")
			.Replace("十", "拾");
			if (useObsolete)
			{
				sb
				.Replace("四", "肆")
				.Replace("五", "伍")
				.Replace("六", "陸")
				.Replace("七", "柒")
				.Replace("八", "捌")
				.Replace("九", "玖")
				.Replace("百", "佰")
				.Replace("千", "仟")
				.Replace("万", "萬");
			}
			return sb.ToString();
		}

		/// <summary>Return Kanji representation of a decimal number string.</summary>
		/// <param name="numStr">A decimal number string, like "-3.214"</param>
		public static string KanjiFrom(string numStr)
		{
			numStr = numStr.Trim();
			// 'num' is for testing if number is valid, negative, non-zero
			var num = decimal.Parse(numStr);
			bool nonZero = (num != 0);
			bool isNegative = (num < 0);
			// Split string into integer and fractional part
			string intStr = string.Empty;
			string fracStr = string.Empty;
			int iDot = numStr.IndexOf('.');
			if (iDot == -1) { intStr = numStr; }
			else
			{
				intStr = numStr.Substring(0, iDot);
				fracStr = numStr.Substring(iDot + 1);
			}
			var sb = new StringBuilder(64);
			if (nonZero && isNegative) { sb.Append("マイナス "); }
			// Parse 'integer' as positive 'long', and make sure no out of range
			long absInt = Math.Abs(long.Parse(intStr));
			if (absInt > VALID_MAX_INT)
			{
				throw new ArgumentOutOfRangeException(nameof(numStr),
					"Must between " + (VALID_MAX_INT * -1).ToString() +
					" and " + VALID_MAX_INT.ToString());
			}
			if (absInt >= ONE_CHO)
			{
				var absIntInCho = absInt / ONE_CHO;
				sb.Append(KanjiFrom4Digit(absIntInCho));
				sb.Append("兆");
				absInt -= absIntInCho * ONE_CHO;
			}
			if (absInt >= ONE_OKU)
			{
				var absIntInOku = absInt / ONE_OKU;
				sb.Append(KanjiFrom4Digit(absIntInOku));
				sb.Append("億");
				absInt -= absIntInOku * ONE_OKU;
			}
			if (absInt >= ONE_MAN)
			{
				var absIntInMan = absInt / ONE_MAN;
				sb.Append(KanjiFrom4Digit(absIntInMan));
				sb.Append("万");
				absInt -= absIntInMan * ONE_MAN;
			}
			// If the number is quite big, but last 4 digits of int are zero,
			// don't show '零' (by not appending Kanji of 'absInt')
			if (absInt > 0 || !nonZero) { sb.Append(KanjiFrom4Digit(absInt)); }
			// If there is number after decimal point
			if (fracStr.Length > 0)
			{
				sb.Append("点");
				foreach (char ch in fracStr)
				{
					var digit = (int)char.GetNumericValue(ch);
					sb.Append(KanjiOfDigit(digit, -1));
				}
			}
			return sb.ToString();
		}

		/// <summary>Return Kanji that represents 4 digit number</summary>
		/// <param name="num">Integer between 0 and 9999</param>
		private static string KanjiFrom4Digit(long num)
		{
			if (num < 0 || num > 9999)
			{
				throw new ArgumentOutOfRangeException(nameof(num), "Number must between 0 and 9999.");
			}
			var sb = new StringBuilder(64);
			string numStr = num.ToString();
			int numStrLen = numStr.Length;
			for (int i = 0; i < numStrLen; i++)
			{
				var digit = (int)char.GetNumericValue(numStr[i]);
				var place = numStrLen - i - 1;
				// If there are '千/百/十', omit '零'
				if (num > 9 && digit == 0) { continue; }
				sb.Append(KanjiOfDigit(digit, place));
			}
			return sb.ToString();
		}

		/// <summary>Return Kanji that represents the digit, along with '十', '百', '千' as needed</summary>
		/// <param name="digit">Integer between 0 and 9</param>
		/// <param name="place">Example '23.45', '2' at 1, '3' at 0, '4' and '5' at -1</param>
		private static string KanjiOfDigit(int digit, int place)
		{
			if (digit < 0 || digit > 9)
			{
				throw new ArgumentOutOfRangeException(nameof(digit), "Digit must between 0 and 9.");
			}
			string ret =
				digit == 0 ? "零" :
				digit == 1 ? "一" :
				digit == 2 ? "二" :
				digit == 3 ? "三" :
				digit == 4 ? "四" :
				digit == 5 ? "五" :
				digit == 6 ? "六" :
				digit == 7 ? "七" :
				digit == 8 ? "八" :
				digit == 9 ? "九" : string.Empty;
			// If '十/百/千', omit '零/一'.
			if (place > 0 && digit <= 1) { ret = string.Empty; }
			ret +=
				place == 1 ? "十" :
				place == 2 ? "百" :
				place == 3 ? "千" : string.Empty;
			return ret;
		}


	}
}
