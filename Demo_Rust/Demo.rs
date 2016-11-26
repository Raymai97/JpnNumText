mod JpnNumText;

fn main() {
	println!("JpnNumText in Rust");
	let test_hira = "じゃ それで いい のか".to_string();
	println!("RomajiFromHira('{}')", test_hira);
	println!("  {}", JpnNumText::RomajiFromHira(&test_hira));
	println!("");

	let test_num_str = "-876541230.98765".to_string();
	let mut kanji = "".to_string();
	println!("let kanji = KanjiFrom({})", test_num_str);
	println!("  {}",
		match JpnNumText::KanjiFrom(&test_num_str) {
			Result::Ok(result) => { kanji = result; kanji.clone() },
			Result::Err(what) => { "Error at ".to_string() + &what }
		}
	);
	println!("");
	if kanji.chars().count() > 0 {
		println!("DaijiFromKanji({}, false)", kanji);
		println!("  {}", JpnNumText::DaijiFromKanji(&kanji, false));
		println!("");
		println!("DaijiFromKanji({}, true)", kanji);
		println!("  {}", JpnNumText::DaijiFromKanji(&kanji, true));
		println!("");
		let hira = JpnNumText::HiraFromKanji(&kanji, true);
		println!("let hira = HiraFromKanji(kanji, true)");
		println!("  {}", hira);
		println!("");
		println!("RomajiFromHira(hira)");
		println!("  {}", JpnNumText::RomajiFromHira(&hira));
		println!("");
	}
}
