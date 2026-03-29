using System;
using System.Windows;
using System.Windows.Controls;

namespace pr4cch1
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Check if i is entered and valid
            if (string.IsNullOrWhiteSpace(tbI.Text))
            {
                MessageBox.Show("Введите значение i!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(tbI.Text, out int i))
            {
                MessageBox.Show("Введите корректное целое значение для i!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if x is entered and valid
            if (string.IsNullOrWhiteSpace(tbX.Text))
            {
                MessageBox.Show("Введите значение x!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(tbX.Text, out double x))
            {
                MessageBox.Show("Введите корректное числовое значение для x!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if a function is selected
            if (!(rbSinh.IsChecked == true || rbX2.IsChecked == true || rbExp.IsChecked == true))
            {
                MessageBox.Show("Выберите функцию f(x)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Calculate f(x)
            double fx;
            if (rbSinh.IsChecked == true)
                fx = Math.Sinh(x);
            else if (rbX2.IsChecked == true)
                fx = x * x;
            else
                fx = Math.Exp(x);

            double result;
            bool isOdd = i % 2 != 0;
            bool isEven = i % 2 == 0;

            try
            {
                if (isOdd && x > 0)
                {
                    if (fx < 0)
                    {
                        MessageBox.Show("Квадратный корень из отрицательного числа при f(x) < 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    result = i * Math.Sqrt(fx);
                }
                else if (isEven && x < 0)
                {
                    result = (double)i / 2 * Math.Sqrt(Math.Abs(fx));
                }
                else
                {
                    double underRoot = Math.Abs(i * fx);
                    result = Math.Sqrt(underRoot);
                }

                tbResult.Text = result.ToString("F6");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            tbI.Text = "";
            tbX.Text = "";
            tbResult.Text = "";
            rbSinh.IsChecked = false;
            rbX2.IsChecked = false;
            rbExp.IsChecked = false;
        }
    }
}