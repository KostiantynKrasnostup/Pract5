using System;
using System.Collections.Generic;

// Клас, що представляє глядача
public class Viewer
{
    private int viewerNumber; // Номер глядача
    public int ViewerNumber => viewerNumber; // Властивість для отримання номера глядача

    // Конструктор, що приймає номер глядача
    public Viewer(int number)
    {
        viewerNumber = number;
    }
}

// Клас, що представляє кінотеатр
public class Cinema
{
    // Делегат для події, коли немає місць
    public delegate void NotPlacesEventHandler();
    public static event NotPlacesEventHandler NotPlaces; // Подія, що сповіщає про відсутність місць

    private int seats; // Кількість місць у кінотеатрі
    private List<Viewer> viewers = new List<Viewer>(); // Список глядачів

    // Конструктор, що приймає кількість місць
    public Cinema(int seats)
    {
        this.seats = seats;
    }

    // Метод для додавання глядача до кінотеатру
    public void PushViewer(Viewer viewer)
    {
        // Перевірка, чи є вільні місця
        if (viewers.Count < seats)
        {
            viewers.Add(viewer); // Додаємо глядача до списку
            Console.ForegroundColor = ConsoleColor.Green; // Змінюємо колір тексту на зелений
            Console.WriteLine($"Глядач {viewer.ViewerNumber} зайняв своє місце"); // Інформуємо про зайняте місце
            Console.ResetColor(); // Скидаємо колір тексту

            // Якщо місця закінчилися, викликаємо подію NotPlaces
            if (viewers.Count == seats)
            {
                NotPlaces?.Invoke(); // Викликаємо подію, якщо місць більше немає
            }
        }
    }
}

// Клас, що представляє охорону
public class Security
{
    // Метод, що викликається для закриття залу
    public void CloseZal()
    {
        Console.ForegroundColor = ConsoleColor.Blue; // Змінюємо колір тексту на синій
        Console.WriteLine("Черговий закрив зал"); // Інформуємо про закриття залу
        Console.ResetColor(); // Скидаємо колір тексту
        CinemaEvents.InvokeSwitchOff(); // Викликаємо подію для вимкнення світла
    }
}

// Клас, що представляє світло
public class Light
{
    // Метод, що викликається для вимкнення світла
    public void Turn()
    {
        Console.ForegroundColor = ConsoleColor.Yellow; // Змінюємо колір тексту на жовтий
        Console.WriteLine("Вимикаємо світло!"); // Інформуємо про вимкнення світла
        Console.ResetColor(); // Скидаємо колір тексту
        CinemaEvents.InvokeBegin(); // Викликаємо подію для початку фільму
    }
}

// Клас, що представляє апаратуру
public class Hardware
{
    private string filmName; // Назва фільму

    // Конструктор, що приймає назву фільму
    public Hardware(string filmName)
    {
        this.filmName = filmName;
    }

    // Метод, що викликається для початку показу фільму
    public void FilmOn()
    {
        Console.ForegroundColor = ConsoleColor.Cyan; // Змінюємо колір тексту на блакитний
        Console.WriteLine($"Починається фільм {filmName}"); // Інформуємо про початок фільму
        Console.ResetColor(); // Скидаємо колір тексту
    }
}

// Статичний клас для обробки подій кінотеатру
public static class CinemaEvents
{
    // Делегат для події вимкнення
    public delegate void SwitchOffEventHandler();
    public static event SwitchOffEventHandler SwitchOff; // Подія для вимкнення світла

    // Делегат для події початку фільму
    public delegate void BeginEventHandler();
    public static event BeginEventHandler Begin; // Подія для початку фільму

    // Метод для виклику події вимкнення
    public static void InvokeSwitchOff()
    {
        SwitchOff?.Invoke(); // Викликаємо подію, якщо вона підписана
    }

    // Метод для виклику події початку фільму
    public static void InvokeBegin()
    {
        Begin?.Invoke(); // Викликаємо подію, якщо вона підписана
    }
}

// Головний клас програми
public class Program
{
    static void Main(string[] args)
    {
        int seats = 5;  // Кількість місць у кінотеатрі
        string filmName = "Жахаючий 3"; // Назва фільму

        Cinema cinema = new Cinema(seats); // Створюємо екземпляр кінотеатру
        Security security = new Security(); // Створюємо екземпляр охорони
        Light light = new Light(); // Створюємо екземпляр світла
        Hardware hardware = new Hardware(filmName); // Створюємо екземпляр апаратури з назвою фільму

        // Підписуємо методи на події
        Cinema.NotPlaces += security.CloseZal; // Підписуємо метод закриття залу на подію відсутності місць
        CinemaEvents.SwitchOff += light.Turn; // Підписуємо метод вимкнення світла на подію вимкнення
        CinemaEvents.Begin += hardware.FilmOn; // Підписуємо метод початку фільму на подію початку

        // Додаємо глядачів до кінотеатру
        for (int i = 1; i <= seats; i++)
        {
            Viewer viewer = new Viewer(i); // Створюємо нового глядача
            cinema.PushViewer(viewer); // Додаємо глядача до кінотеатру
        }

        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення..."); // Інструкція для завершення програми
        Console.ReadKey(); // Чекаємо натискання клавіші
    }
}