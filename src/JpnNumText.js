/* JpnNumText 2016.11.26
 * Coded by Raymai97
 * Ported from C# for modern JS
 */

/* make this usable on outdated browser */
if (!String.prototype.startsWith) {
	String.prototype.startsWith = function(key, index) {
		index = index || 0;
		return this.indexOf(key, index) === index;
	}
}
if (!String.prototype.trim) {
	String.prototype.trim = function() {
		return this;
	}
}
/* String.replace only on 1st occurence so... */
String.prototype.Replace = function(key, newKey) {
	return this.split(key).join(newKey);
}

/* JpnNumText is static */
var JpnNumText = {
	/* const */
	ONE_MAN : 10000, // 万
	ONE_OKU : 10000 * 10000, // 億
	ONE_CHO : 10000 * 10000 * 10000, // 兆
	VALID_MAX_INT : (10000 * 10000 * 10000 * 1000 - 1),
	/* method */
	RomajiFromHira : function (hira) {
		var ret = "";
		hira = hira.toString();
		var ltsu = false; // 促音
		for (var i = 0; i < hira.length; i++)
		{
			// First, we think as if every Hira is separated
			var ch = hira[i];
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
				ch == 'ん' ? "n" : ""
			);
			// If next char is available, handle 'ゃ/ゅ/ょ' specifically.
			if (i + 1 < hira.length)
			{
				var nextCh = hira[i + 1];
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
						nextCh == 'ょ' ? "o" : "";
					if (tail != "")
					{
						var mid = (currRomaji.length == 2) ? "y" : "h";
						currRomaji = currRomaji[0] + mid + tail;
					}
				}
			}
			if (ltsu) // if 促音, prepend first char, like "pa" become "ppa"
			{
				ltsu = false;
				currRomaji = currRomaji[0] + currRomaji;
			}
			ret += (currRomaji);
		}
		return ret;
	},
	HiraFromKanji : function (kanji, useSpace) {
		var ret = "";
		kanji = kanji.toString();
		if (kanji.startsWith("マイナス"))
		{
			ret += ("まいなす");
			if (useSpace) { ret += (" "); }
		}
		var hadDot = false, skipOnce = false;
		for (var i = 0; i < kanji.length; i++)
		{
			// If previous round was something like "sanbyaku",
			// we don't need another "hyaku", so we skip once.
			if (skipOnce) { skipOnce = false; continue; }
			// First, we think as if every Kanji is separated
			var ch = kanji[i];
			var currHira = (
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
				ch == '九' ? "きゅう" : "");
			if (ch == '点') { hadDot = true; }
			// If we haven't met '点' and next char is available,
			// try to merge some. For example, "san sen" become "sanzen".
			if (!hadDot && i + 1 < kanji.length)
			{
				var mergedHira = "";
				var nextCh = kanji[i + 1];
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
						ch == '九' ? "きゅうせん" : "");
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
						ch == '九' ? "きゅうひゃく" : "");
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
						ch == '九' ? "きゅうじゅう" : "");
				}
				// If merge successfully, skip the next loop (once only)
				if (mergedHira != "")
				{
					currHira = mergedHira;
					skipOnce = true;
				}
			}
			// If current char makes sense, append it
			if (currHira != "")
			{
				ret += (currHira);
				if (useSpace) { ret += (" "); }
			}
		}
		return ret.trim();
	},
	DaijiFromKanji : function (kanji, useObsolete) {
		var daiji = kanji.toString();
		daiji = daiji
		.Replace("一", "壹")
		.Replace("二", "貳")
		.Replace("三", "參")
		.Replace("十", "拾");
		if (useObsolete)
		{
			daiji = daiji
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
		return daiji;
	},
	KanjiFrom : function (numStr) {
		numStr = numStr.toString().trim();
		// 'num' is for testing if number is valid, negative, non-zero
		var num = parseFloat(numStr);
		var nonZero = (num != 0);
		var isNegative = (num < 0);
		// Split string into integer and fractional part
		var intStr = "";
		var fracStr = "";
		var iDot = numStr.indexOf('.');
		if (iDot == -1) { intStr = numStr; }
		else
		{
			intStr = numStr.substring(0, iDot);
			fracStr = numStr.substring(iDot + 1);
		}
		var ret = "";
		if (nonZero && isNegative) { ret += "マイナス "; }
		// Parse 'integer' as positive 'long', and make sure no out of range
		var absInt = Math.abs(parseInt(intStr));
		if (absInt > this.VALID_MAX_INT)
		{
			throw ("Number must between " + (this.VALID_MAX_INT * -1) +
				" and " + this.VALID_MAX_INT);
		}
		if (absInt >= this.ONE_CHO)
		{
			var absIntInCho = Math.floor(absInt / this.ONE_CHO);
			ret += this.KanjiFrom4Digit(absIntInCho);
			ret += "兆";
			absInt -= absIntInCho * this.ONE_CHO;
		}
		if (absInt >= this.ONE_OKU)
		{
			var absIntInOku = Math.floor(absInt / this.ONE_OKU);
			ret += this.KanjiFrom4Digit(absIntInOku);
			ret += "億";
			absInt -= absIntInOku * this.ONE_OKU;
		}
		if (absInt >= this.ONE_MAN)
		{
			var absIntInMan = Math.floor(absInt / this.ONE_MAN);
			ret += this.KanjiFrom4Digit(absIntInMan);
			ret += "万";
			absInt -= absIntInMan * this.ONE_MAN;
		}
		// If the number is quite big, but last 4 digits of int are zero,
		// don't show '零' (by not appending Kanji of 'absInt')
		if (absInt > 0 || !nonZero) { ret += this.KanjiFrom4Digit(absInt); }
		// If there is number after decimal point
		if (fracStr.length > 0)
		{
			ret += "点";
			for (var i = 0; i < fracStr.length; i++)
			{
				var ch = fracStr[i];
				var digit = parseInt(ch);
				ret += this.KanjiOfDigit(digit, -1);
			}
		}
		return ret;
	},
	KanjiFrom4Digit : function (num) {
		if (num < 0 || num > 9999)
		{
			throw "Number must between 0 and 9999.";
		}
		var ret = "";
		var numStr = num.toString();
		var numStrLen = numStr.length;
		for (var i = 0; i < numStrLen; i++)
		{
			var digit = parseInt(numStr[i]);
			var place = numStrLen - i - 1;
			// If there are '千/百/十', omit '零'
			if (num > 9 && digit == 0) { continue; }
			ret += this.KanjiOfDigit(digit, place);
		}
		return ret;
	},
	KanjiOfDigit : function (digit, place) {
		if (digit < 0 || digit > 9)
		{
			throw "Digit must between 0 and 9.";
		}
		var ret =
			digit == 0 ? "零" :
			digit == 1 ? "一" :
			digit == 2 ? "二" :
			digit == 3 ? "三" :
			digit == 4 ? "四" :
			digit == 5 ? "五" :
			digit == 6 ? "六" :
			digit == 7 ? "七" :
			digit == 8 ? "八" :
			digit == 9 ? "九" : "";
		// If '十/百/千', omit '零/一'.
		if (place > 0 && digit <= 1) { ret = ""; }
		ret +=
			place == 1 ? "十" :
			place == 2 ? "百" :
			place == 3 ? "千" : "";
		return ret;
	}
}
