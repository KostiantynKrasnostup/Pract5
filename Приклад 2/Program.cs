using System;
using System.Collections.Generic;

public class Car
{
    private string nomer;
    public string Nomer
    {
        get { return nomer; }
    }

    public Car(string nomer)
    {
        this.nomer = nomer;
    }
}

public class Security
{
    private string name;
    public string Name
    {
        get { return name; }
    }

    public Security(string name)
    {
        this.name = name;
    }

    public void CloseParking()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Мест нет. Охранник {0} закрыл стоянку", name);
        Parking.NotPlaces -= CloseParking;
    }
}

public class Police
{
    private string name;
    public string Name
    {
        get { return name; }
    }

    public Police(string name)
    {
        this.name = name;
    }

    public void VideoSwitchOn()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Полицейский {0} включил видеонаблюдение", name);
        Parking.NotPlaces -= VideoSwitchOn;
    }

    public void DroveOutAddress(int t)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Сработала сигнализация {0} раз", t);
        Console.WriteLine("Полицейский {0} приехал на стоянку", name);
    }
}

public class Parking
{
    public delegate void SignalTriggeredEventHandler(int t);
    public static event SignalTriggeredEventHandler SignalTriggered;

    public delegate void NotPlacesEventHandler();
    public static event NotPlacesEventHandler NotPlaces;

    private bool therePlaces;
    private string adr;
    private int allPlaces;
    private List<Car> cars;
    private int t;

    public bool TherePlaces
    {
        get { return therePlaces; }
    }

    public Parking(string adr, int allPlaces)
    {
        this.adr = adr;
        this.allPlaces = allPlaces;
        cars = new List<Car>();
        this.therePlaces = true;
        t = 0;
    }

    public void PushCar(Car car)
    {
        Random random = new Random();
        if ((NotPlaces != null) && cars.Count >= allPlaces)
        {
            NotPlaces(); // Trigger NotPlaces event
            therePlaces = false; // No more spaces
        }
        else
        {
            cars.Add(car);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("На стоянку прибыла " + car.Nomer);
            int x = random.Next(1, 8);
            if (x == 1) // Randomly trigger the alarm
            {
                t++;
                SignalTriggered?.Invoke(t);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Creating objects
        Parking parking = new Parking("ул.Краснова", 10);
        Security securityMan = new Security("Николай");
        Police policeMan = new Police("Александр");

        // Subscribing to events
        Parking.NotPlaces += securityMan.CloseParking;
        Parking.NotPlaces += policeMan.VideoSwitchOn;
        Parking.SignalTriggered += policeMan.DroveOutAddress;

        int i = 1;
        while (parking.TherePlaces)
        {
            Car car = new Car("машина " + i);
            parking.PushCar(car);
            i++;
        }

        Console.ReadKey();
    }
}
