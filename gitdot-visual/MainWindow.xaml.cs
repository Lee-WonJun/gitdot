using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gitdot_visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // yearPicker is combobox, and it selects year, 2000 ~ current year
            var currentYear = DateTime.Now.Year;
            yearPicker.ItemsSource = Enumerable.Range(2000, currentYear - 2000 + 1).Reverse();
            yearPicker.SelectedIndex = 0;
        }
    }
}
