using System;

namespace LabWork
{
    /// <summary>
    /// Базовий клас для роботи з полярною системою координат
    /// </summary>
    public class ПолярнаСистемаКоординат
    {
        private double _радіус;
        private double _кут; // у градусах

        public ПолярнаСистемаКоординат(double радіус, double кут)
        {
            if (радіус < 0)
                throw new ArgumentException("Радіус не може бути від'ємним");
            if (кут < 0 || кут >= 360)
                throw new ArgumentException("Кут має бути в діапазоні [0, 360)");

            _радіус = радіус;
            _кут = кут;
        }

        public double Радіус
        {
            get => _радіус;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Радіус не може бути від'ємним");
                _радіус = value;
            }
        }

        public double Кут
        {
            get => _кут;
            set
            {
                if (value < 0 || value >= 360)
                    throw new ArgumentException("Кут має бути в діапазоні [0, 360)");
                _кут = value;
            }
        }

        /// <summary>
        /// Перетворення з полярної в декартову систему
        /// </summary>
        public virtual (double x, double y) ПеретворитиУДекартову()
        {
            double радіанів = _кут * Math.PI / 180;
            double x = _радіус * Math.Cos(радіанів);
            double y = _радіус * Math.Sin(радіанів);
            return (x, y);
        }

        public override string ToString()
        {
            return $"Полярна система: r = {_радіус:F2}, θ = {_кут}°";
        }
    }

    /// <summary>
    /// Клас для роботи з декартовою системою координат
    /// </summary>
    public class ДекартоваСистемаКоординат
    {
        private double _x;
        private double _y;

        public ДекартоваСистемаКоординат(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double X
        {
            get => _x;
            set => _x = value;
        }

        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Перетворення з декартової в полярну систему
        /// </summary>
        public (double r, double angle) ПеретворитиУПолярну()
        {
            double радіус = Math.Sqrt(_x * _x + _y * _y);
            double кут = Math.Atan2(_y, _x) * 180 / Math.PI;
            
            if (кут < 0)
                кут += 360;
            
            return (радіус, кут);
        }

        public override string ToString()
        {
            return $"Декартова система: x = {_x:F2}, y = {_y:F2}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                // Створення точки в полярній системі
                ПолярнаСистемаКоординат полярна = new ПолярнаСистемаКоординат(5, 45);
                Console.WriteLine(полярна);
                var (x, y) = полярна.ПеретворитиУДекартову();
                Console.WriteLine($"У декартовій системі: x = {x:F2}, y = {y:F2}");

                Console.WriteLine();

                // Створення точки в декартовій системі
                ДекартоваСистемаКоординат декартова = new ДекартоваСистемаКоординат(3, 4);
                Console.WriteLine(декартова);
                var (r, angle) = декартова.ПеретворитиУПолярну();
                Console.WriteLine($"У полярній системі: r = {r:F2}, θ = {angle:F2}°");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
