using System;

namespace LabWork
{
    public class РівностороннійТрикутник
    {
        private double _сторона;
        private const double _кут = 60; // Всі кути в рівносторонньому трикутнику = 60°

        public РівностороннійТрикутник(double сторона)
        {
            if (сторона <= 0)
                throw new ArgumentException("Довжина сторони має бути додатньою");
            _сторона = сторона;
        }

        public virtual void ВстановитиСторону(double сторона)
        {
            if (сторона <= 0)
                throw new ArgumentException("Довжина сторони має бути додатньою");
            _сторона = сторона;
        }

        public virtual double ОтриматиСторону()
        {
            return _сторона;
        }

        public virtual double ОтриматиКут()
        {
            return _кут;
        }

        public virtual double ОбчислитиПериметр()
        {
            return 3 * _сторона;
        }

        public override string ToString()
        {
            return $"Рівносторонній трикутник: сторона = {_сторона:F2}, кут = {_кут}°";
        }
    }

    public class Трикутник : РівностороннійТрикутник
    {
        private double _кут1;
        private double _кут2;
        private double _сторона2;
        private double _сторона3;

        public Трикутник(double сторона, double кут1, double кут2) : base(сторона)
        {
            if (кут1 <= 0 || кут2 <= 0)
                throw new ArgumentException("Кути мають бути додатніми");
            if (кут1 + кут2 >= 180)
                throw new ArgumentException("Сума двох кутів не може бути більшою або рівною 180°");

            _кут1 = кут1;
            _кут2 = кут2;
            ОбчислитиІншіСторони();
        }

        private void ОбчислитиІншіСторони()
        {
            double кут3 = 180 - _кут1 - _кут2;
            double a = ОтриматиСторону();

            // Використання теореми синусів для обчислення інших сторін
            _сторона2 = a * Math.Sin(_кут2 * Math.PI / 180) / Math.Sin(кут3 * Math.PI / 180);
            _сторона3 = a * Math.Sin(_кут1 * Math.PI / 180) / Math.Sin(кут3 * Math.PI / 180);
        }

        public override double ОбчислитиПериметр()
        {
            return ОтриматиСторону() + _сторона2 + _сторона3;
        }

        public double ОтриматиКут1() => _кут1;
        public double ОтриматиКут2() => _кут2;
        public double ОтриматиКут3() => 180 - _кут1 - _кут2;
        public double ОтриматиСторону2() => _сторона2;
        public double ОтриматиСторону3() => _сторона3;

        public override string ToString()
        {
            return $"Трикутник: сторони = {ОтриматиСторону():F2}, {_сторона2:F2}, {_сторона3:F2}; " +
                   $"кути = {_кут1}°, {_кут2}°, {ОтриматиКут3()}°";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                // Створення рівностороннього трикутника
                РівностороннійТрикутник рівнТрикутник = new РівностороннійТрикутник(5);
                Console.WriteLine(рівнТрикутник);
                Console.WriteLine($"Периметр: {рівнТрикутник.ОбчислитиПериметр():F2}");

                Console.WriteLine();

                // Створення звичайного трикутника
                Трикутник трикутник = new Трикутник(6, 45, 60);
                Console.WriteLine(трикутник);
                Console.WriteLine($"Периметр: {трикутник.ОбчислитиПериметр():F2}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
