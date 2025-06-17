using System;
using System.ComponentModel;
using System.Windows.Media;

namespace BouncingBalls.Models
{
    public class Ball : INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        private double _vx;
        private double _vy;
        private double _radius;
        private Color _color;

        public int Id { get; set; }

        public double X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public double Vx
        {
            get => _vx;
            set
            {
                if (_vx != value)
                {
                    _vx = value;
                    OnPropertyChanged(nameof(Vx));
                }
            }
        }

        public double Vy
        {
            get => _vy;
            set
            {
                if (_vy != value)
                {
                    _vy = value;
                    OnPropertyChanged(nameof(Vy));
                }
            }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    OnPropertyChanged(nameof(Radius));
                }
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public Ball()
        {
        }

        public Ball(double x, double y, double vx, double vy, double radius, Color color)
        {
            _x = x;
            _y = y;
            _vx = vx;
            _vy = vy;
            _radius = radius;
            _color = color;
        }

        public void Move(double canvasWidth, double canvasHeight, double deltaTime)
        {
            double newX = X + Vx * deltaTime;
            double newY = Y + Vy * deltaTime;

            if (newX - Radius < 0 && Vx < 0)
            {
                Vx = -Vx;
                newX = Radius;
                newY = Y + Vy * (Radius - X) / Vx;
            }
            else if (newX + Radius > canvasWidth && Vx > 0)
            {
                Vx = -Vx;
                newX = canvasWidth - Radius;
                newY = Y + Vy * (canvasWidth - Radius - X) / Vx;
            }

            if (newY - Radius < 0 && Vy < 0)
            {
                Vy = -Vy;
                newY = Radius;
                newX = X + Vx * (Radius - Y) / Vy;
            }
            else if (newY + Radius > canvasHeight && Vy > 0)
            {
                Vy = -Vy;
                newY = canvasHeight - Radius;
                newX = X + Vx * (canvasHeight - Radius - Y) / Vy;
            }

            X = newX;
            Y = newY;

            Console.WriteLine($"Ball {Id}: X={X}, Y={Y}, Vx={Vx}, Vy={Vy}, CanvasWidth={canvasWidth}, CanvasHeight={canvasHeight}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}