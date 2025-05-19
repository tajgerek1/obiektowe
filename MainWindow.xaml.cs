using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Window1.Uczen> uczniowie = new ObservableCollection<Window1.Uczen>();

        public MainWindow()
        {
            InitializeComponent();
            lvUczniowie.ItemsSource = uczniowie;
        }

        
        private void MenuWczytaj_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "CSV (,) |*.csv|CSV (;) |*.csv",
                Title = "Wczytaj listę uczniów"
            };
            if (dlg.ShowDialog() == true)
            {
                uczniowie.Clear();
                string delimiter = dlg.FilterIndex == 1 ? "," : ";";
                foreach (var line in File.ReadAllLines(dlg.FileName, Encoding.UTF8))
                {
                    var cols = line.Split(delimiter);
                    var u = new Window1.Uczen
                    {
                        Pesel = cols.ElementAtOrDefault(0) ?? "",
                        Imie = cols.ElementAtOrDefault(1) ?? "",
                        DrugieImie = cols.ElementAtOrDefault(2) ?? "",
                        Nazwisko = cols.ElementAtOrDefault(3) ?? ""
                    };
                   
                    uczniowie.Add(u);
                }
            }
        }

        
        private void MenuZapisz_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "CSV (,) |*.csv|CSV (;) |*.csv",
                Title = "Zapisz listę uczniów"
            };
            if (dlg.ShowDialog() == true)
            {
                string delimiter = dlg.FilterIndex == 1 ? "," : ";";
                using var writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
                foreach (var u in uczniowie)
                {
                    writer.WriteLine(
                        $"{u.Pesel}{delimiter}" +
                        $"{u.Imie}{delimiter}" +
                        $"{u.DrugieImie}{delimiter}" +
                        $"{u.Nazwisko}{delimiter}" +
                        $"{u.DataUrodzenia:d}{delimiter}" +
                        $"{u.Telefon}{delimiter}" +
                        $"{u.Adres}{delimiter}" +
                        $"{u.Miejscowosc}{delimiter}" +
                        $"{u.KodPocztowy}");
                }
            }
        }

        
        private void MenuDodaj_Click(object sender, RoutedEventArgs e)
        {
            var okno = new Window1();
            if (okno.ShowDialog() == true)
                uczniowie.Add(okno.NowyUczen);
        }

        
        private void MenuUsun_Click(object sender, RoutedEventArgs e)
        {
            var zazn = lvUczniowie.SelectedItems;
            var doUsun = new List<Window1.Uczen>();
            foreach (Window1.Uczen u in zazn)
                doUsun.Add(u);
            foreach (var u in doUsun)
                uczniowie.Remove(u);
        }

        
        private void ContextMenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lvUczniowie.SelectedItem is Window1.Uczen wybrany)
            {
                var okno = new Window1(wybrany);
                if (okno.ShowDialog() == true)
                {
                    int idx = uczniowie.IndexOf(wybrany);
                    uczniowie[idx] = okno.NowyUczen;
                }
            }
        }

        
        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuUsun_Click(sender, e);
        }

        
        private void MenuOProgramie_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }
    }
}
