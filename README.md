# JpnNumText
Library that converts number to Japanese text. （日本漢字、平仮名 and Romaji）

### Currently there are 4 exported functions.
#### RomajiFromHira(hira)
Accept any hiragana.
Example: RomajiFromHira("きみ の なまえ") returns "kimi no namae".

#### HiraFromKanji(kanji, useSpace)
Accept kanji returned by KanjiFrom() ONLY.
Example: HiraFromKanji("二千二十", true) returns "にせん にじゅう".

#### DaijiFromKanji(kanji, useObsolete)
Accept kanji returned by KanjiFrom() ONLY.
Example: DaijiFromKanji("二千二十", false) returns "貳千二拾".

#### KanjiFrom(numStr)
Example: KanjiFrom("2020") returns "二千二十".
