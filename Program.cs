using DriverFinder.Algorithms;
using DriverFinder1.Algorithms;

namespace DriverFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== Driver Finder System ===");
            Console.WriteLine("Система поиска ближайших водителей к заказу\n");

            if (args.Length > 0 && args[0] == "demo")
            {
                RunDemo();
            }
            else if (args.Length > 0 && args[0] == "benchmark")
            {
                RunQuickBenchmark();
            }
            else if (args.Length > 0 && args[0] == "test")
            {
                RunCorrectnessTest();
            }
            else
            {
                ShowMenu();
            }
        }

        static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Меню ===");
                Console.WriteLine("1. Демонстрация работы алгоритмов");
                Console.WriteLine("2. Быстрое сравнение производительности");
                Console.WriteLine("3. Тест на корректность");
                Console.WriteLine("4. Выход");
                Console.Write("\nВыберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        RunDemo();
                        break;
                    case "2":
                        RunQuickBenchmark();
                        break;
                    case "3":
                        RunCorrectnessTest();
                        break;
                    case "4":
                        Console.WriteLine("Выход...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void RunDemo()
        {
            Console.WriteLine("\n=== Демонстрация работы алгоритмов ===\n");

            // Создаем тестовых водителей
            var drivers = new List<Driver>
            {
                new Driver(1, 10, 10),
                new Driver(2, 20, 20),
                new Driver(3, 30, 30),
                new Driver(4, 40, 40),
                new Driver(5, 50, 50),
                new Driver(6, 60, 60),
                new Driver(7, 70, 70),
                new Driver(8, 80, 80),
                new Driver(9, 90, 90),
                new Driver(10, 100, 100)
            };

            // Создаем заказ
            var order = new Order(15, 15);

            Console.WriteLine($"Заказ в точке: ({order.X}, {order.Y})");
            Console.WriteLine($"Всего водителей: {drivers.Count}");
            Console.WriteLine($"Ищем 5 ближайших водителей\n");

            // Тестируем 3 алгоритма
            var algorithms = new IAlgorithm[]
            {
                new BruteForceFinder(),
                new PriorityQueueFinder(),
                new KdTreeFinder()
            };

            foreach (var algorithm in algorithms)
            {
                Console.WriteLine($"\n--- Алгоритм: {algorithm.AlgorithmName} ---");

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var nearestDrivers = algorithm.FindNearestDrivers(order, drivers, 5).ToList();
                stopwatch.Stop();

                Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");

                for (int i = 0; i < nearestDrivers.Count; i++)
                {
                    var driver = nearestDrivers[i];
                    var distance = Math.Sqrt(driver.SquareDistanceTo(order));
                    Console.WriteLine($"  {i + 1}. Driver {driver.Id} ({driver.X}, {driver.Y}) - расстояние: {distance:F2}");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        static void RunQuickBenchmark()
        {
            Console.WriteLine("\n=== Быстрое сравнение производительности ===\n");

            var sizes = new[] { 100, 1000, 5000, 10000 };
            var algorithms = new IAlgorithm[]
            {
                new BruteForceFinder(),
                new PriorityQueueFinder(),
                new KdTreeFinder()
            };

            var random = new Random(42); // Фиксированный seed для воспроизводимости

            Console.WriteLine("| Кол-во водителей | Алгоритм        | Время (мс) | Найдено | Ближайший |");
            Console.WriteLine("|------------------|-----------------|------------|---------|-----------|");

            foreach (var size in sizes)
            {
                // Генерируем водителей
                var drivers = GenerateRandomDrivers(size, random);
                var order = new Order(500, 500);

                foreach (var algorithm in algorithms)
                {
                    // Прогрев
                    algorithm.FindNearestDrivers(order, drivers, 5);

                    // Измерение
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    var result = algorithm.FindNearestDrivers(order, drivers, 5).ToList();
                    stopwatch.Stop();

                    Console.WriteLine($"| {size,16} | {algorithm.AlgorithmName,-15} | {stopwatch.ElapsedMilliseconds,10} | {result.Count,7} | Driver{result.First().Id,9} |");
                }
                Console.WriteLine("|------------------|-----------------|------------|---------|-----------|");
            }

            Console.WriteLine("\nВыводы:");
            Console.WriteLine("- Brute Force: простой, но медленный на больших данных");
            Console.WriteLine("- Priority Queue: эффективен для поиска топ-N элементов");
            Console.WriteLine("- KD-Tree: самый быстрый для пространственных запросов");

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        static void RunCorrectnessTest()
        {
            Console.WriteLine("\n=== Тест на корректность ===\n");

            // Создаем водителей в известных позициях
            var drivers = new List<Driver>
            {
                new Driver(1, 0, 0),
                new Driver(2, 10, 0),
                new Driver(3, 0, 10),
                new Driver(4, 10, 10),
                new Driver(5, 20, 0),
                new Driver(6, 0, 20),
                new Driver(7, 20, 20),
                new Driver(8, 5, 5),
                new Driver(9, 15, 5),
                new Driver(10, 5, 15)
            };

            var order = new Order(5, 5);
            Console.WriteLine($"Заказ в точке: ({order.X}, {order.Y})");
            Console.WriteLine("Ближайшие водители:");
            Console.WriteLine("  - Driver 8 (5,5) - 0 единиц (самый близкий)");
            Console.WriteLine("  - Остальные (1,2,3,4) на расстоянии 7.07 единиц (одинаково!)");
            Console.WriteLine("  При одинаковом расстоянии порядок может отличаться\n");

            var algorithms = new IAlgorithm[]
            {
                new BruteForceFinder(),
                new PriorityQueueFinder(),
                new KdTreeFinder()
            };

            bool allCorrect = true;

            foreach (var algorithm in algorithms)
            {
                Console.WriteLine($"\n{algorithm.AlgorithmName}:");

                try
                {
                    var result = algorithm.FindNearestDrivers(order, drivers, 5).ToList();

                    Console.Write("  Найдены (ID): ");
                    foreach (var driver in result)
                    {
                        Console.Write($"{driver.Id} ");
                    }
                    Console.WriteLine();

                    // Проверяем что Driver 8 первый
                    bool driver8First = result[0].Id == 8;

                    // Проверяем что остальные 4 водителя это 1,2,3,4 (в любом порядке)
                    var otherDrivers = result.Skip(1).Select(d => d.Id).OrderBy(id => id).ToList();
                    var expectedOthers = new[] { 1, 2, 3, 4 }.OrderBy(id => id).ToList();

                    bool othersCorrect = otherDrivers.SequenceEqual(expectedOthers);

                    bool correct = driver8First && othersCorrect;

                    Console.WriteLine($"  Корректность: {(correct ? " ДА" : " НЕТ")}");

                    if (!correct)
                    {
                        if (!driver8First)
                            Console.WriteLine("  Ошибка: Driver 8 должен быть первым!");

                        if (!othersCorrect)
                            Console.WriteLine($"  Ошибка: Ожидались водители 1,2,3,4 (в любом порядке), получено: {string.Join(", ", otherDrivers)}");

                        allCorrect = false;
                    }
                    else
                    {
                        Console.WriteLine("  Driver 8 первый (правильно)");
                        Console.WriteLine($"  Остальные: {string.Join(", ", result.Skip(1).Select(d => d.Id))} (корректно, так как расстояния одинаковые)");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"  Ошибка: {ex.Message}");
                    allCorrect = false;
                }
            }

            Console.WriteLine($"\n{(allCorrect ? " ВСЕ АЛГОРИТМЫ РАБОТАЮТ КОРРЕКТНО!" : " Есть ошибки в алгоритмах")}");

            // Дополнительная проверка: расстояния
            Console.WriteLine("\n--- Проверка расстояний ---");
            var referenceResult = new BruteForceFinder().FindNearestDrivers(order, drivers, 5).ToList();

            for (int i = 0; i < referenceResult.Count; i++)
            {
                var driver = referenceResult[i];
                var distance = Math.Sqrt(driver.SquareDistanceTo(order));
                Console.WriteLine($"  {i + 1}. Driver {driver.Id} ({driver.X}, {driver.Y}) - {distance:F2} единиц");
            }

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        /// <summary>
        /// Генерирует случайных водителей
        /// </summary>
        static List<Driver> GenerateRandomDrivers(int count, Random random)
        {
            var drivers = new List<Driver>(count);

            for (int i = 0; i < count; i++)
            {
                drivers.Add(new Driver(
                    i + 1,
                    random.Next(0, 1000),
                    random.Next(0, 1000)
                ));
            }

            return drivers;
        }

        /// <summary>
        /// Быстрый тест производительности
        /// </summary>
        static void QuickPerformanceTest()
        {
            Console.WriteLine("\n= Быстрый тест производительности ===\n");

            var random = new Random(42);
            var drivers = GenerateRandomDrivers(10000, random);
            var order = new Order(500, 500);

            var algorithms = new IAlgorithm[]
            {
                new BruteForceFinder(),
                new PriorityQueueFinder(),
                new KdTreeFinder()
            };

            Console.WriteLine("Тест с 10000 водителей, поиск 5 ближайших:");
            Console.WriteLine("----------------------------------------");

            foreach (var algorithm in algorithms)
            {
                // 5 итераций для усреднения
                long totalTime = 0;
                for (int i = 0; i < 5; i++)
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    algorithm.FindNearestDrivers(order, drivers, 5).ToList();
                    stopwatch.Stop();
                    totalTime += stopwatch.ElapsedMilliseconds;
                }

                Console.WriteLine($"{algorithm.AlgorithmName}: {totalTime / 5} мс (среднее)");
            }
        }
    }
}