using System;

namespace TaskInd
{
    class ManagingCompany
    {
        public Lodger loader;

        public void ReportCheck()
        {
            Console.WriteLine("Ваш долг {0}", Lodger.duty);
        }
    }

    class Lodger
    {
        public static string name;
        public static int check;
        public static int duty;

        public Lodger(string name, int check, int duty)
        {
            Lodger.name = name;
            Lodger.check = check;
            Lodger.duty = duty;
        }

        public void Payment()
        {
            int temp = Lodger.check;
            Lodger.check -= duty;
            Console.WriteLine("{0} заплатил {1}", Lodger.name, duty);
            Lodger.duty = 0;
            Console.WriteLine("На счету было {0}", temp);
            Console.WriteLine("На счету осталось {0}", check);
        }
    }

    class Meter
    {
        public delegate void Handler();
        public static event Handler Event;

        public void CountDay(int i)
        {
            if (i == 10)
            {
                Event?.Invoke();
            }
            else
            {
                Console.WriteLine("Сегодня {0}", i);
            }
        }

        public void SayCountDay()
        {
            Console.WriteLine("Сегодня 10. Пора платить.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ManagingCompany mCo = new ManagingCompany();
            Lodger lodger = new Lodger("Ivanov", 900, 500);
            Meter checkPayments = new Meter();

            // Підписка на події
            Meter.Event += checkPayments.SayCountDay;
            Meter.Event += mCo.ReportCheck;
            Meter.Event += lodger.Payment;

            // Перевірка дня для платежів
            for (int i = 1; i <= 30; i++)
            {
                checkPayments.CountDay(i);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
