using System;
using System.Windows;
using BouncingBalls.ViewModels;

namespace BouncingBalls.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("MainWindow initialized.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.StopAnimation();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.CanvasWidth = e.NewSize.Width;
                viewModel.CanvasHeight = e.NewSize.Height;
                Console.WriteLine($"Window resized to Width={e.NewSize.Width}, Height={e.NewSize.Height}");
            }
        }
    }
}