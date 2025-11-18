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
            SetPolar(радіус, кут);
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
                value = NormalizeAngle(value);
                if (value < 0 || value >= 360)
                    throw new ArgumentException("Кут має бути в діапазоні [0, 360)");
                _кут = value;
            }
        }

        /// <summary>
        /// Перетворення з полярної в декартову систему (2D)
        /// </summary>
        public virtual (double x, double y) ПеретворитиУДекартову()
        {
            double радіанів = _кут * Math.PI / 180;
            double x = _радіус * Math.Cos(радіанів);
            double y = _радіус * Math.Sin(радіанів);
            return (x, y);
        }

        /// <summary>
        /// Перетворення у 3D: за замовчуванням z = 0. Переоприділяється в похідних класах.
        /// </summary>
        public virtual (double x, double y, double z) ПеретворитиУДекартову3D()
        {
            var (x, y) = ПеретворитиУДекартову();
            return (x, y, 0.0);
        }

        /// <summary>
        /// Явні методи для задання координат
        /// </summary>
        public void SetPolar(double радіус, double кут)
        {
            if (радіус < 0)
                throw new ArgumentException("Радіус не може бути від'ємним");
            кут = NormalizeAngle(кут);
            if (кут < 0 || кут >= 360)
                throw new ArgumentException("Кут має бути в діапазоні [0, 360)");
            _радіус = радіус;
            _кут = кут;
        }

        public void SetCartesian(double x, double y)
        {
            double радіус = Math.Sqrt(x * x + y * y);
            double кут = Math.Atan2(y, x) * 180 / Math.PI;
            if (кут < 0) кут += 360;
            SetPolar(радіус, кут);
        }

        protected static double NormalizeAngle(double angle)
        {
            // приводить к діапазону [0, 360)
            angle %= 360;
            if (angle < 0) angle += 360;
            return angle;
        }

        public override string ToString()
        {
            return $"Полярна система: r = {_радіус:F2}, θ = {_кут:F2}°";
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
        /// Перетворення з декартової в полярну систему (2D)
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

    /// <summary>
    /// Похідний клас: циліндрична система (r, θ, z)
    /// </summary>
    public class ЦиліндричнаСистемаКоординат : ПолярнаСистемаКоординат
    {
        private double _z;

        public ЦиліндричнаСистемаКоординат(double радіус, double кут, double z)
            : base(радіус, кут)
        {
            Z = z;
        }

        public double Z
        {
            get => _z;
            set => _z = value;
        }

        /// <summary>
        /// Перевизначене перетворення у декартову систему (3D)
        /// </summary>
        public override (double x, double y, double z) ПеретворитиУДекартову3D()
        {
            var (x, y) = base.ПеретворитиУДекартову();
            return (x, y, _z);
        }

        /// <summary>
        /// Задати циліндричні координати
        /// </summary>
        public void SetCylindrical(double радіус, double кут, double z)
        {
            SetPolar(радіус, кут);
            Z = z;
        }

        /// <summary>
        /// Заповнити з декартових координат (x,y,z) -> (r,θ,z)
        /// </summary>
        public void SetFromCartesian3D(double x, double y, double z)
        {
            double радіус = Math.Sqrt(x * x + y * y);
            double кут = Math.Atan2(y, x) * 180 / Math.PI;
            if (кут < 0) кут += 360;
            SetCylindrical(радіус, кут, z);
        }

        public override string ToString()
        {
            return $"Циліндрична система: r = {Радіус:F2}, θ = {Кут:F2}°, z = {Z:F2}";
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

                Console.WriteLine();

                // Демонстрація циліндричної системи (3D)
                ЦиліндричнаСистемаКоординат циліндр = new ЦиліндричнаСистемаКоординат(5, 45, 2.5);
                Console.WriteLine(циліндр);
                var (cx, cy, cz) = циліндр.ПеретворитиУДекартову3D();
                Console.WriteLine($"У декартовій 3D: x = {cx:F2}, y = {cy:F2}, z = {cz:F2}");

                Console.WriteLine();

                // Заповнення циліндричної точки з декартових координат
                ЦиліндричнаСистемаКоординат ц2 = new ЦиліндричнаСистемаКоординат(0, 0, 0);
                ц2.SetFromCartesian3D(3, 4, 5);
                Console.WriteLine(ц2);
                var (r2x, r2y, r2z) = ц2.ПеретворитиУДекартову3D();
                Console.WriteLine($"Після конвертації назад: x = {r2x:F2}, y = {r2y:F2}, z = {r2z:F2}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}