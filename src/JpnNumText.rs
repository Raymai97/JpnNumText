/* JpnNumText 2016.11.26
 * Coded by Raymai97
 * Ported from C# to Rust
 */

#![allow(non_snake_case)]
#![allow(non_camel_case_types)]
#![allow(non_upper_case_globals)]
#![allow(dead_code)]

const ONE_MAN : i64 = 10000; // 万
const ONE_OKU : i64 = ONE_MAN * ONE_MAN; // 億
const ONE_CHO : i64 = ONE_OKU * ONE_MAN; // 兆
const VALID_MAX_INT : i64 = (ONE_CHO * ONE_MAN - 1);

/// Return Romaji of Hiragana.
///
pub fn RomajiFromHira(hira: &String) -> String {
	let mut ret : String = "".to_string();
	let mut ltsu = false;
	let mut peekable = hira.chars().peekable();
	while peekable.peek() != None {
		let ch = peekable.next().unwrap();
		// First, we think as if every Hira is separated
		if ch == 'っ' { ltsu = true; continue; }
		let mut currRomaji : String = match ch {
			' '=> " ",
			'あ' => "a",
			'い' => "i",
			'う' => "u",
			'え' => "e",
			'お' => "o",
			'か' => "ka", 'が' => "ga",
			'き' => "ki", 'ぎ' => "gi",
			'く'=> "ku", 'ぐ' => "gu",
			'け' => "ke", 'げ' => "ge",
			'こ' => "ko", 'ご' => "go",
			'さ' => "sa", 'ざ' => "za",
			'し' => "shi", 'じ' => "ji",
			'す' => "su", 'ず' => "zu",
			'せ' => "se", 'ぜ' => "ze",
			'そ' => "so", 'ぞ' => "zo",
			'た' => "ta", 'だ' => "da",
			'ち' => "chi", 'ぢ' => "dzi",
			'つ' => "tsu", 'づ' => "dzu",
			'て' => "te", 'で' => "de",
			'と' => "to", 'ど' => "do",
			'な' => "na",
			'に' => "ni",
			'ぬ' => "nu",
			'ね' => "ne", 
			'の' => "no",
			'は' => "ha", 'ば' => "ba", 'ぱ' => "pa",
			'ひ' => "hi", 'び' => "bi", 'ぴ' => "pi",
			'ふ' => "fu", 'ぶ' => "bu", 'ぷ' => "pu",
			'へ' => "he", 'べ' => "be", 'ぺ' => "pe",
			'ほ' => "ho", 'ぼ' => "bo", 'ぽ' => "po",
			'ま' => "ma",
			'み' => "mi",
			'む' => "mu",
			'め' => "me",
			'も' => "mo",
			'や' => "ya", 
			'ゆ' => "yu",
			'よ' => "yo",
			'ら' => "ra",
			'り' => "ri",
			'る' => "ru",
			'れ' => "re",
			'ろ' => "ro",
			'わ' => "wa",
			'を' => "wo",
			'ん' => "n",
			_ => ""
		}.to_string();
		if currRomaji == "" { continue; }
		let currRomajiFirstChar = currRomaji.chars().nth(0).unwrap();
		// If next char is available, handle 'ゃ/ゅ/ょ' specifically.
		if peekable.peek() != None {
			let nextCh = *(peekable.peek().unwrap());
			if ch == 'じ' {
				currRomaji = match nextCh {
					'ゃ' => "ja".to_string(),
					'ゅ' => "ju".to_string(),
					'ょ' => "jo".to_string(),
					_ => currRomaji
				};
			}
			else {
				let tail = match nextCh {
					'ゃ' => "a",
					'ゅ' => "u",
					'ょ' => "o",
					_ => ""
				};
				if tail != "" {
					let mid = match currRomaji.chars().count() { 2 => "y", _ => "h" };
					currRomaji = currRomajiFirstChar.to_string() + mid + tail;
				}
			}
		}
		if ltsu {
			ltsu = false;
			currRomaji = currRomajiFirstChar.to_string() + &currRomaji;
		}
		ret += &currRomaji;
	}
	ret
}

/// Return Hiragana of Kanji that represents number.
///
/// Params:
/// 'kanji' = Kanji that represents number. Example: '三百四点六六'
///
pub fn HiraFromKanji(kanji : &String, useSpace : bool) -> String {
	let mut ret = "".to_string();
	if kanji.starts_with("マイナス") {
		ret = "まいなす".to_string();
		if useSpace { ret += " "; }
	}
	let mut hadDot = false;
	let mut skipOnce = false;
	let mut peekable = kanji.chars().peekable();
	while peekable.peek() != None {
		let ch = peekable.next().unwrap();
		// If previous round was something like "sanbyaku",
		// we don't need another "hyaku", so we skip once.
		if skipOnce { skipOnce = false; continue; }
		// First, we think as if every Kanji is separated
		let mut currHira = match ch {
			'点' => "てん",
			'十' => "じゅう",
			'百' => "ひゃく",
			'千' => "せん",
			'万' => "まん",
			'億' => "おく",
			'兆' => "ちょう",
			'零' => "れい",
			'一' => "いち",
			'二' => "に",
			'三' => "さん",
			'四' => "よん",
			'五' => "ご",
			'六' => "ろく",
			'七' => "なな",
			'八' => "はち",
			'九' => "きゅう", _ => ""
		}.to_string();
		if ch == '点' { hadDot = true; }
		// If we haven't met '点' and next char is available,
		// try to merge some. For example, "san sen" become "sanzen".
		if !hadDot && peekable.peek() != None {
			let nextCh = *(peekable.peek().unwrap());
			let mergedHira = match nextCh {
				'千' => {
					match ch {
						'二' => "にせん",
						'三' => "さんぜん",
						'四' => "よんせん",
						'五' => "ごせん",
						'六' => "ろくせん",
						'七' => "ななせん",
						'八' => "はっせん",
						'九' => "きゅうせん", _ => ""
					}
				},
				'百' => {
					match ch {
						'二' => "にひゃく",
						'三' => "さんびゃく",
						'四' => "よんひゃく",
						'五' => "ごひゃく",
						'六' => "ろっぴゃく",
						'七' => "ななひゃく",
						'八' => "はっぴゃく",
						'九' => "きゅうひゃく", _ => ""
					}
				},
				'十' => {
					match ch {
						'二' => "にじゅう",
						'三' => "さんじゅう",
						'四' => "よんじゅう",
						'五' => "ごじゅう",
						'六' => "ろくじゅう",
						'七' => "ななじゅう",
						'八' => "はちじゅう",
						'九' => "きゅうじゅう", _ => ""
					}
				}, _ => ""
			}.to_string();
			// If merge successfully, skip the next loop (once only)
			if mergedHira.chars().count() > 0 {
				currHira = mergedHira;
				skipOnce = true;
			}
		}
		// If current char makes sense, append it
		if currHira.chars().count() > 0 {
			ret += &currHira;
			if useSpace { ret += " "; }
		}
	}
	ret.trim().to_string()
}

/// Return Daiji representation of Kanji that represents number.
///
/// Params:
/// 'kanji' = Kanji that represents number. Example: '三百四点六六'
///
pub fn DaijiFromKanji(kanji : &String, useObsolete : bool) -> String {
	kanji.chars().map( |ch: char|
		match ch {
			'一' => '壹',
			'二' => '貳',
			'三' => '參',
			'十' => '拾',
			_ => {
				if useObsolete {
					match ch {
						'四' => '肆',
						'五' => '伍',
						'六' => '陸',
						'七' => '柒',
						'八' => '捌',
						'九' => '玖',
						'百' => '佰',
						'千' => '仟',
						'万' => '萬',
						_ => ch
					}
				}
				else { ch }
			}
		}
	).collect()
}

/// Return Kanji representation of a decimal number string.
///
/// Params:
/// 'numStr' = A decimal number string, like "-3.214"
///
pub fn KanjiFrom(numStr : &String) -> Result<String, String> {
	let err_msg = |msg : &str| Err("KanjiFrom(): ".to_string() + msg) ;
	let numStr = numStr.trim().to_string();
	// 'num' is for testing if number is valid, negative, non-zero
	let numResult = numStr.parse::<f64>();
	if numResult.is_err() { return err_msg("numStr must be a decimal number!");	}
	let num = numResult.unwrap();
	let nonZero = num != 0.0;
	let isNegative = num < 0.0;
	// Split string into integer and fractional part
	let intStr : String;
	let fracStr : String;
	match numStr.chars().position(|ch: char| ch == '.') {
		Some(iDot) => {
			intStr = numStr.chars().take(iDot).collect();
			fracStr = numStr.chars().skip(iDot+1).collect();
		},
		None => {
			intStr = numStr.chars().collect();
			fracStr = "".to_string();
		}
	};
	let mut ret = match nonZero && isNegative { true => "マイナス ", _ => "" }.to_string();
	// Parse 'integer' as positive 'long', and make sure no out of range
	let mut absInt = intStr.parse::<i64>().unwrap().abs();
	if absInt > VALID_MAX_INT {
		return err_msg(&format!("numStr must between {} and {}",
			VALID_MAX_INT * -1, VALID_MAX_INT));
	}
	if absInt >= ONE_CHO {
		let absIntInCho = absInt / ONE_CHO;
		ret += &KanjiFrom4Digit(absIntInCho);
		ret += "兆";
		absInt -= absIntInCho * ONE_CHO;
	}
	if absInt >= ONE_OKU {
		let absIntInCho = absInt / ONE_OKU;
		ret += &KanjiFrom4Digit(absIntInCho);
		ret += "億";
		absInt -= absIntInCho * ONE_OKU;
	}
	if absInt >= ONE_MAN {
		let absIntInCho = absInt / ONE_MAN;
		ret += &KanjiFrom4Digit(absIntInCho);
		ret += "万";
		absInt -= absIntInCho * ONE_MAN;
	}
	// If the number is quite big, but last 4 digits of int are zero,
	// don't show '零' (by not appending Kanji of 'absInt')
	if absInt > 0 || !nonZero { ret += &KanjiFrom4Digit(absInt); }
	// If there is number after decimal point
	if fracStr.chars().count() > 0 {
		ret += "点";
		for ch in fracStr.chars() {
			let digit = ch.to_string().parse::<i32>().unwrap();
			ret += &KanjiOfDigit(digit, -1);
		}
	}
	Ok(ret)
}

/// Return Kanji that represents 4 digit number
///
/// Params:
/// 'num' = Integer between 0 and 9999
///
fn KanjiFrom4Digit(num: i64) -> String {
	if num < 0 || num > 9999 {
		panic!("KanjiFrom4Digit(): Number must between 0 and 9999.");
	}
	let mut ret = "".to_string();
	let numStr = num.to_string();
	let numStrLen = numStr.chars().count();
	for i in 0..numStrLen {
		let digit = numStr.chars().nth(i).unwrap().to_string().parse::<i32>().unwrap();
		let place = (numStrLen - i) as i32 - 1;
		// If there are '千/百/十', omit '零'
		if num > 9 && digit == 0 { continue; }
		ret += &KanjiOfDigit(digit, place);
	}
	ret
}

/// Return Kanji that represents the digit, along with '十', '百', '千' as needed
///
/// Params:
/// 'digit' = Integer between 0 and 9
/// 'place' = Example '23.45', '2' at 1, '3' at 0, '4' and '5' at -1
///
fn KanjiOfDigit(digit: i32, place: i32) -> String {
	if digit < 0 || digit > 9 {
		panic!("KanjiOfDigit(): Digit must between 0 and 9, not {}", digit);
	}
	let mut ret = match digit {
		0 => "零" ,
		1 => "一" ,
		2 => "二" ,
		3 => "三" ,
		4 => "四" ,
		5 => "五" ,
		6 => "六" ,
		7 => "七" ,
		8 => "八" ,
		9 => "九" , _ => ""
	}.to_string();
	// If '十/百/千', omit '零/一'.
	if place > 0 && digit <= 1 { ret = "".to_string(); }
	ret += match place {
		1 => "十" ,
		2 => "百" ,
		3 => "千" , _ => ""
	};
	ret
}
