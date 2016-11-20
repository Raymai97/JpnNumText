using System;
using System.Windows;
using System.Windows.Controls;
using MaiSoft;

namespace TellNumInJpnWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtInput.Focus();
        }

		void Update()
		{
			if (!IsLoaded) { return; }
			string kanji, hira, romaji;
			try
			{
				kanji = JpnNumText.KanjiFrom(txtInput.Text);
				hira = JpnNumText.HiraFromKanji(kanji, chkUseSpace.IsChecked.Value);
				romaji = JpnNumText.RomajiFromHira(hira);
				if (cbiUseCurrentDaiji.IsSelected)
				{
					kanji = JpnNumText.DaijiFromKanji(kanji, false);
				}
				else if (cbiUseObsoleteDaiji.IsSelected)
				{
					kanji = JpnNumText.DaijiFromKanji(kanji, true);
				}
			}
			catch (FormatException)
			{
				kanji = hira = romaji = "Please enter number only!";
			}
			catch (ArgumentOutOfRangeException)
			{
				kanji = hira = romaji = "MINDBLOWN! Try enter a smaller number with lesser decimal places.";
			}
			catch (Exception ex)
			{
				kanji = hira = romaji = "Error: " + ex.Message;
			}
			if (String.IsNullOrEmpty(txtInput.Text)) { kanji = hira = romaji = string.Empty; }
			txtKanji.Text = kanji;
			txtHiragana.Text = hira;
			txtRomaji.Text = romaji;
		}
		     
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
			Update();
		}

        private void chkUseSpace_CheckStateChanged(object sender, RoutedEventArgs e)
        {
			Update();
		}

		private void cboDaiji_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Update();
		}
	}
}
