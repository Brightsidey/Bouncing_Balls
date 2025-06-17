using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BouncingBalls.Models;
using System;

namespace BouncingBalls.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseContext _dbContext;
        private CancellationTokenSource? _cancellationTokenSource;
        private double _canvasWidth = 850;
        private double _canvasHeight = 600;
        private double _innerWidth = 700; 
        private double _innerHeight = 550;
        private const double Margin = 50; 

        public ObservableCollection<Ball> Balls { get; } = new ObservableCollection<Ball>();

        public MainViewModel()
        {
            _dbContext = new DatabaseContext();
            LoadBalls();
            StartAnimation();
        }

        public double CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                if (!Equals(_canvasWidth, value))
                {
                    _canvasWidth = value;
                    InnerWidth = value - 2 * Margin; 
                    OnPropertyChanged(nameof(CanvasWidth));
                }
            }
        }

        public double CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                if (!Equals(_canvasHeight, value))
                {
                    _canvasHeight = value;
                    InnerHeight = value - 2 * Margin; 
                    OnPropertyChanged(nameof(CanvasHeight));
                }
            }
        }

        public double InnerWidth
        {
            get => _innerWidth;
            set
            {
                if (!Equals(_innerWidth, value))
                {
                    _innerWidth = value;
                    OnPropertyChanged(nameof(InnerWidth));
                }
            }
        }

        public double InnerHeight
        {
            get => _innerHeight;
            set
            {
                if (!Equals(_innerHeight, value))
                {
                    _innerHeight = value;
                    OnPropertyChanged(nameof(InnerHeight));
                }
            }
        }

        private void LoadBalls()
        {
            var balls = _dbContext.GetBalls();
            foreach (var ball in balls)
            {
                Balls.Add(ball);
            }
        }

        private void StartAnimation()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(() => AnimateBalls(_cancellationTokenSource.Token));
        }

        private async Task AnimateBalls(CancellationToken cancellationToken)
        {
            const double deltaTime = 0.008; 
            while (!cancellationToken.IsCancellationRequested)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    double currentInnerWidth = InnerWidth;
                    double currentInnerHeight = InnerHeight;
                    foreach (var ball in Balls)
                    {
                        ball.Move(currentInnerWidth, currentInnerHeight, deltaTime);
                    }
                });
                await Task.Delay((int)(deltaTime * 400), cancellationToken);
            }
        }

        public void StopAnimation()
        {
            _cancellationTokenSource?.Cancel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}