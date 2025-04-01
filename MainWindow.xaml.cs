using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Myitem
    {
        public string Name { get; set; }

        public string Sname { get; set; }

        public string Pesel { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Myitem item = new Myitem();

            item.Name = "tak";
            item.Sname = "nie";
            item.Pesel = "11111111111";

            listView.Items.Add(new {m_nID = 1, m_strName = item.Name});
        }
    }
}