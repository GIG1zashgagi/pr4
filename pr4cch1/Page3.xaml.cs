using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pr4cch1
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(tbX0.Text) || string.IsNullOrWhiteSpace(tbXk.Text) ||
                string.IsNullOrWhiteSpace(tbDx.Text) || string.IsNullOrWhiteSpace(tbB.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(tbX0.Text, out double x0) ||
                !double.TryParse(tbXk.Text, out double xk) ||
                !double.TryParse(tbDx.Text, out double dx) ||
                !double.TryParse(tbB.Text, out double b))
            {
                MessageBox.Show("Введите корректные числовые значения для всех полей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dx == 0)
            {
                MessageBox.Show("Шаг dx не может быть равен 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if ((x0 > xk && dx > 0) || (x0 < xk && dx < 0))
            {
                MessageBox.Show("Неправильное направление шага! dx должен быть положительным при x0 < xk и отрицательным при x0 > xk",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<PointData> data = new List<PointData>();
            string results = "";

            int iterations = 0;
            const int maxIterations = 10000;

            for (double x = x0; dx > 0 ? x <= xk : x >= xk; x += dx)
            {
                iterations++;
                if (iterations > maxIterations)
                {
                    MessageBox.Show("Слишком много итераций! Проверьте шаг и диапазон.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double y;
                try
                {
                    double absX = Math.Abs(x);
                    double xMinusB = Math.Abs(x - b);

                    if (xMinusB <= 0)
                    {
                        MessageBox.Show($"Ошибка при x={x}: логарифм от неположительного числа!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    double powTerm = 0.001 * Math.Pow(absX, 2.5);
                    double logTerm = Math.Log(xMinusB);
                    y = powTerm + logTerm;

                    results += $"x = {x:F6}, y = {y:F6}\n";
                    data.Add(new PointData(x, y));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка вычисления при x={x}: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            tbResults.Text = results;

            // Draw chart
            DrawChart(data);
        }

        private void DrawChart(List<PointData> data)
        {
            if (data.Count == 0)
                return;

            // Clear canvas
            chartCanvas.Children.Clear();

            double width = chartCanvas.ActualWidth;
            double height = chartCanvas.ActualHeight;

            if (width == 0 || height == 0)
            {
                // If canvas not measured yet, schedule drawing after layout
                chartCanvas.SizeChanged += (s, e) => DrawChart(data);
                return;
            }

            // Find min and max values
            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var point in data)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            // Add padding
            double xPadding = (maxX - minX) * 0.1;
            double yPadding = (maxY - minY) * 0.1;

            if (xPadding == 0) xPadding = 1;
            if (yPadding == 0) yPadding = 1;

            double xMin = minX - xPadding;
            double xMax = maxX + xPadding;
            double yMin = minY - yPadding;
            double yMax = maxY + yPadding;

            // Draw axes
            DrawAxes(width, height, xMin, xMax, yMin, yMax);

            // Draw grid
            DrawGrid(width, height, xMin, xMax, yMin, yMax);

            // Draw function line
            DrawFunctionLine(data, width, height, xMin, xMax, yMin, yMax);
        }

        private void DrawAxes(double width, double height, double xMin, double xMax, double yMin, double yMax)
        {
            // X-axis
            double xAxisY = TransformY(0, height, yMin, yMax);
            if (xAxisY >= 0 && xAxisY <= height)
            {
                Line xAxis = new Line
                {
                    X1 = 0,
                    Y1 = xAxisY,
                    X2 = width,
                    Y2 = xAxisY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                chartCanvas.Children.Add(xAxis);
            }

            // Y-axis
            double yAxisX = TransformX(0, width, xMin, xMax);
            if (yAxisX >= 0 && yAxisX <= width)
            {
                Line yAxis = new Line
                {
                    X1 = yAxisX,
                    Y1 = 0,
                    X2 = yAxisX,
                    Y2 = height,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                chartCanvas.Children.Add(yAxis);
            }
        }

        private void DrawGrid(double width, double height, double xMin, double xMax, double yMin, double yMax)
        {
            SolidColorBrush gridBrush = new SolidColorBrush(Colors.LightGray);
            gridBrush.Opacity = 0.5;

            // Vertical grid lines
            double xStep = (xMax - xMin) / 10;
            for (double x = xMin; x <= xMax; x += xStep)
            {
                double xCoord = TransformX(x, width, xMin, xMax);
                if (xCoord >= 0 && xCoord <= width)
                {
                    Line gridLine = new Line
                    {
                        X1 = xCoord,
                        Y1 = 0,
                        X2 = xCoord,
                        Y2 = height,
                        Stroke = gridBrush,
                        StrokeThickness = 1
                    };
                    chartCanvas.Children.Add(gridLine);
                }
            }

            // Horizontal grid lines
            double yStep = (yMax - yMin) / 10;
            for (double y = yMin; y <= yMax; y += yStep)
            {
                double yCoord = TransformY(y, height, yMin, yMax);
                if (yCoord >= 0 && yCoord <= height)
                {
                    Line gridLine = new Line
                    {
                        X1 = 0,
                        Y1 = yCoord,
                        X2 = width,
                        Y2 = yCoord,
                        Stroke = gridBrush,
                        StrokeThickness = 1
                    };
                    chartCanvas.Children.Add(gridLine);
                }
            }
        }

        private void DrawFunctionLine(List<PointData> data, double width, double height, double xMin, double xMax, double yMin, double yMax)
        {
            if (data.Count < 2)
                return;

            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };

            foreach (var point in data)
            {
                double x = TransformX(point.X, width, xMin, xMax);
                double y = TransformY(point.Y, height, yMin, yMax);

                if (x >= 0 && x <= width && y >= 0 && y <= height)
                {
                    polyline.Points.Add(new Point(x, y));
                }
            }

            chartCanvas.Children.Add(polyline);

            // Add markers for points
            foreach (var point in data)
            {
                double x = TransformX(point.X, width, xMin, xMax);
                double y = TransformY(point.Y, height, yMin, yMax);

                if (x >= 0 && x <= width && y >= 0 && y <= height)
                {
                    Ellipse marker = new Ellipse
                    {
                        Width = 4,
                        Height = 4,
                        Fill = Brushes.Red,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };

                    Canvas.SetLeft(marker, x - 2);
                    Canvas.SetTop(marker, y - 2);
                    chartCanvas.Children.Add(marker);
                }
            }
        }

        private double TransformX(double x, double width, double xMin, double xMax)
        {
            return (x - xMin) / (xMax - xMin) * width;
        }

        private double TransformY(double y, double height, double yMin, double yMax)
        {
            return height - (y - yMin) / (yMax - yMin) * height;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            tbX0.Text = "";
            tbXk.Text = "";
            tbDx.Text = "";
            tbB.Text = "";
            tbResults.Text = "";
            chartCanvas.Children.Clear();
        }
    }

    public class PointData
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointData(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}