using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class Window1 : Window
    {
        public class Uczen
        {
            public string Pesel { get; set; } = "";
            public string Imie { get; set; } = "";
            public string DrugieImie { get; set; } = "";
            public string Nazwisko { get; set; } = "";
            public string DataUrodzenia { get; set; } = "";
            public string Telefon { get; set; } = "";
            public string Adres { get; set; } = "";
            public string Miejscowosc { get; set; } = "";
            public string KodPocztowy { get; set; } = "";
        }

        public Uczen NowyUczen { get; private set; }
        private bool edycjaTryb;

        public Window1(Uczen uczen = null)
        {
            InitializeComponent();

            if (uczen != null)
            {
                edycjaTryb = true;

                PeselBox.Text = uczen.Pesel;
                ImieBox.Text = uczen.Imie;
                DrugieImieBox.Text = uczen.DrugieImie;
                NazwiskoBox.Text = uczen.Nazwisko;
                DataUrodzeniaBox.Text = uczen.DataUrodzenia;
                TelefonBox.Text = uczen.Telefon;
                AdresBox.Text = uczen.Adres;
                MiejscowoscBox.Text = uczen.Miejscowosc;
                KodPocztowyBox.Text = uczen.KodPocztowy;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (!WalidujDane())
                return;

            NowyUczen = new Uczen
            {
                Pesel = PeselBox.Text.Trim(),
                Imie = Formatuj(ImieBox.Text),
                DrugieImie = string.IsNullOrWhiteSpace(DrugieImieBox.Text)
                             ? "" : Formatuj(DrugieImieBox.Text),
                Nazwisko = Formatuj(NazwiskoBox.Text),
                DataUrodzenia = DataUrodzeniaBox.Text.Trim(),
                Telefon = FormatTelefon(TelefonBox.Text),
                Adres = Formatuj(AdresBox.Text),
                Miejscowosc = Formatuj(MiejscowoscBox.Text),
                KodPocztowy = KodPocztowyBox.Text.Trim()
            };

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (CzyWprowadzonoZmiany())
            {
                var result = MessageBox.Show(
                    "Czy na pewno chcesz anulować bez zapisywania?",
                    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;
            }

            DialogResult = false;
        }

        private bool WalidujDane()
        {
            bool poprawne = true;

           
            foreach (var tb in new[] { PeselBox, ImieBox, NazwiskoBox,
                                      DataUrodzeniaBox, AdresBox,
                                      MiejscowoscBox, KodPocztowyBox })
            {
                tb.ClearValue(BorderBrushProperty);
                tb.ClearValue(BorderThicknessProperty);
            }

            
            TextBox[] wymagane = {
                PeselBox, ImieBox, NazwiskoBox,
                DataUrodzeniaBox, AdresBox,
                MiejscowoscBox, KodPocztowyBox
            };
            foreach (var box in wymagane)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.BorderBrush = Brushes.Red;
                    box.BorderThickness = new Thickness(2);
                    poprawne = false;
                }
            }

            
            if (!SprawdzPesel(PeselBox.Text.Trim()))
            {
                PeselBox.BorderBrush = Brushes.Red;
                PeselBox.BorderThickness = new Thickness(2);
                poprawne = false;
            }


            if (!ZgodnoscPeselZDataUrodzenia(
                    PeselBox.Text.Trim(),
                    DataUrodzeniaBox.Text.Trim()))
            {
                DataUrodzeniaBox.BorderBrush = Brushes.Red;
                DataUrodzeniaBox.BorderThickness = new Thickness(2);
                poprawne = false;
            }

            return poprawne;
        }

        private bool SprawdzPesel(string pesel)
        {
            if (!Regex.IsMatch(pesel, @"^\d{11}$")) return false;
            int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int suma = 0;
            for (int i = 0; i < 10; i++)
                suma += wagi[i] * (pesel[i] - '0');
            int kontrolna = (10 - (suma % 10)) % 10;
            return kontrolna == (pesel[10] - '0');
        }

        private bool ZgodnoscPeselZDataUrodzenia(string pesel, string data)
        {
            if (!DateTime.TryParse(data, out var dt)) return false;
            string rok = dt.Year.ToString("D4").Substring(2, 2);
            int miesBaz = dt.Month + (dt.Year >= 2000 ? 20 : 0);
            string przPesel = pesel.Substring(0, 2)
                              + miesBaz.ToString("D2")
                              + dt.Day.ToString("D2");
            return przPesel == pesel.Substring(0, 6);
        }

        private string Formatuj(string tekst)
        {
            tekst = tekst.Trim();
            if (tekst.Length == 0) return "";
            return char.ToUpper(tekst[0]) + tekst.Substring(1).ToLower();
        }

        private string FormatTelefon(string telefon)
        {
            var digits = Regex.Replace(telefon, @"\D", "");
            if (digits.Length == 9) return "+48 " + digits;
            if (digits.StartsWith("48") && digits.Length == 11)
                return "+" + digits;
            return telefon.Trim();
        }

        private bool CzyWprowadzonoZmiany()
        {
            foreach (var tb in new[] {
                PeselBox, ImieBox, DrugieImieBox, NazwiskoBox,
                DataUrodzeniaBox, TelefonBox, AdresBox,
                MiejscowoscBox, KodPocztowyBox })
            {
                if (!string.IsNullOrWhiteSpace(tb.Text))
                    return true;
            }
            return false;
        }
    }
}
