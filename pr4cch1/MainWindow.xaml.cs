using System.Windows;
using pr4cch1;

namespace pr4cch1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new Page1());
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Page1());
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Page2());
        }

        private void Page3_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Page3());
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?",
                "Подтверждение выхода", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}