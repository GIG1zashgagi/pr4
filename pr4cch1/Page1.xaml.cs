using System;
using System.Windows;
using System.Windows.Controls;

namespace pr4cch1
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tbX.Text, out double x) ||
                !double.TryParse(tbY.Text, out double y) ||
                !double.TryParse(tbZ.Text, out double z))
            {
                MessageBox.Show("Введите корректные числовые значения для x, y и z!");
                return;
            }

            try
            {
                double tgZ = Math.Tan(z);
                double absYminusX = Math.Abs(y - x);

                double numerator = Math.Pow(x, y + 1) + Math.Exp(y - 1);
                double denominator = 1 + x * Math.Abs(y - tgZ);

                if (denominator == 0)
                {
                    MessageBox.Show("Деление на ноль! Знаменатель равен 0.");
                    return;
                }

                double part1 = (numerator / denominator) * (1 + absYminusX);
                double part2 = Math.Pow(absYminusX, 2) / 2;
                double part3 = Math.Pow(absYminusX, 3) / 3;

                double h = part1 + part2 - part3;

                tbResult.Text = h.ToString("F6");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            tbX.Text = "";
            tbY.Text = "";
            tbZ.Text = "";
            tbResult.Text = "";
        }
    }
}