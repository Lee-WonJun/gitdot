using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            InitControl();
        }

        readonly Dictionary<(int, int), Button> AllButtons = new();
        private void InitControl()
        {
            for (int i = 0; i < 53; i++)
            {
                canvas.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 7; i++)
            {
                canvas.RowDefinitions.Add(new RowDefinition());
            }

            // draw button every cell in canvas grid
            for (int col = 0; col < 53; col++)
            {
                for (int row = 0; row < 7; row++)
                {
                    var button = new Button();

                    button.Margin = new Thickness(1);
                    button.Content = 0;
                    button.FontSize = 10;

                    button.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    button.Background = new SolidColorBrush(Color.FromRgb(45, 51, 59));

                    button.BorderBrush = new SolidColorBrush(Color.FromArgb(0xE5, 0x00, 0x00, 0x00));

                    button.Visibility = Visibility.Visible;
                    button.SetValue(Grid.ColumnProperty, col);
                    button.SetValue(Grid.RowProperty, row);

                    button.MouseRightButtonDown += Button_MouseRightButtonDown;
                    button.Click += Button_Click; ;

                    AllButtons.Add((row, col), button);
                    canvas.Children.Add(button);
                }
            }


            // yearPicker is combobox, and it selects year, 2000 ~ current year
            var currentYear = DateTime.Now.Year;
            yearPicker.ItemsSource = Enumerable.Range(2000, currentYear - 2000 + 1).Reverse();
            yearPicker.SelectionChanged += YearPicker_SelectionChanged;
            yearPicker.SelectedIndex = 0;
        }
        

        private void YearPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetButtonByYear();

        }

        private void ResetButtonByYear()
        {
            ResetAllButton();

            var year = (int)yearPicker.SelectedItem;
            var firstDay = new DateTime(year, 1, 1);
            var firstDayOfWeek = (int)firstDay.DayOfWeek;


            //disable rows and columns
            for (int day = 0; day < firstDayOfWeek; day++)
            {
                AllButtons[(day, 0)].Visibility = Visibility.Hidden;
            }

            var validCellCount = DateTime.IsLeapYear(year) ? 366 : 365;
            AllButtons.Values.ToList()
                .Where(b => b.Visibility == Visibility.Visible)
                .Skip(validCellCount).ToList()
                .ForEach(b => b.Visibility = Visibility.Hidden);
        }

        private void ResetAllButton()
        {
            AllButtons.Values.ToList().ForEach(b =>
            {
                b.Content = 0;
                b.Visibility = Visibility.Visible;
                b.Foreground = new SolidColorBrush(Color.FromRgb(148, 110, 37));
                b.Background = new SolidColorBrush(Color.FromRgb(45, 51, 59));
            }
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var count = (int)button.Content;
            count++;
            button.Content = count;
            ColoringButton(button);
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var count= (int)button.Content;
            
            if (count > 0)
            {
                count--;
                button.Content = count;
                ColoringButton(button);
            }
        }
        
        private void ColoringButton(Button button)
        {
            var count = (int)button.Content;
            if (count == 0)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(148, 110, 37));
                button.Background = new SolidColorBrush(Color.FromRgb(45, 51, 59));
            }
            else if (count > 0)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(148, 37, 60));
                button.Background = new SolidColorBrush(Color.FromRgb(57, 211, 83));
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonByYear();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var year = (int)yearPicker.SelectedItem;
            var firstDay = new DateTime(year, 1, 1);
            var countPerDay = AllButtons.Values.ToList()
                .Where(b => b.Visibility == Visibility.Visible)
                .Select((b,idx) => (b,idx))
                .Where(pair =>
                {
                    var (b, idx) = pair;
                    return (int)b.Content > 0;
                })
                .Select(pair => 
                {
                    var (b, idx) = pair;
                    return $"{firstDay.AddDays(idx).ToString("d")},{(int)b.Content}";
                })
                .ToList();
            
            var result = string.Join(Environment.NewLine, countPerDay);

            // file save dialog
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = $"{year}.txt";
            dialog.DefaultExt = ".txt";
            if (dialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(dialog.FileName, result);
            }
        }
    }
}
