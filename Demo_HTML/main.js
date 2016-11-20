function $(id) { return document.getElementById(id); }

function isNumeric(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}

function updateUI() {
	var inputStr = $("txtInput").value;
	var kanji = "", hira = "", romaji = "";
	if (inputStr) {
		try {
			if (!isNumeric(inputStr)) { throw "Please enter number only!"; }
			kanji = JpnNumText.KanjiFrom(inputStr);
			hira = JpnNumText.HiraFromKanji(kanji, true);
			romaji = JpnNumText.RomajiFromHira(hira);
			var daijiOption = $("cboDaiji").selectedIndex;
			if (daijiOption) {
				kanji = JpnNumText.DaijiFromKanji(kanji, daijiOption == 2);
			}
		}
		catch (errMsg) {
			kanji = hira = romaji = errMsg;
		}
	}		
	$("txtKanji").value = kanji;
	$("txtHira").value = hira;
	$("txtRomaji").value = romaji;
}
//778855662333.1024556