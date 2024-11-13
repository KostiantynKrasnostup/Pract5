using System;

namespace TrafficLightProject
{
    class TrafficLight
    {
        public delegate void Handler();

        public static event Handler Event1; 
        public static event Handler Event2;

        public void ChangeLight(int i)
        {
            if (i % 2 == 0)
            {
                OnEvent2();
            }
            else
            {
                OnEvent1();
            }
        }

        protected virtual void OnEvent1()
        {
            if (Event1 != null)
            {
                Event1.Invoke();
            }
        }

        protected virtual void OnEvent2()
        {
            if (Event2 != null)
            {
                Event2.Invoke();
            }
        }

        public void Red()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Горить червоний");
            Console.ResetColor();
        }

        public void Green()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Горить зелений");
            Console.ResetColor();
        }
    }

    class Driver
    {
        string name;

        public Driver() { }

        public Driver(string name)
        {
            this.name = name;
        }

        public void Ride()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{name} їду");
            Console.ResetColor();

            TrafficLight.Event1 -= this.Ride;
            TrafficLight.Event2 -= this.Stand;
        }
        public void Stand()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{name} стою");
            Console.ResetColor();
        }
    }
    class Pedestrian
    {
        string name;

        public Pedestrian() { }

        public Pedestrian(string name)
        {
            this.name = name;
        }

        public void Go()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{name} іду");
            Console.ResetColor();

            TrafficLight.Event1 -= this.Stand;
            TrafficLight.Event2 -= this.Go;
        }

        public void Stand()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{name} стою");
            Console.ResetColor();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            TrafficLight svet = new TrafficLight();

            TrafficLight.Event1 += svet.Green;
            TrafficLight.Event2 += svet.Red;

            int numberOfParticipants = 5;
            Driver[] drivers = new Driver[numberOfParticipants];
            Pedestrian[] pedestrians = new Pedestrian[numberOfParticipants];

            for (int i = 0; i < numberOfParticipants; i++)
            {
                drivers[i] = new Driver($"Водій {i + 1}");
                pedestrians[i] = new Pedestrian($"Пішохід {i + 1}");

                TrafficLight.Event1 += drivers[i].Ride;
                TrafficLight.Event1 += pedestrians[i].Stand;

                TrafficLight.Event2 += drivers[i].Stand;
                TrafficLight.Event2 += pedestrians[i].Go;
            }

            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"\nЗміна світла №{i}:");
                svet.ChangeLight(i);

                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
