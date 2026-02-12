using DriverFinder1.Algorithms;

namespace DriverFinder1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация алгоритмов поиска ближайших водителей ===\n");

            // Создаем заказ в центре карты
            var order = new Order(50, 50);
            Console.WriteLine($"Заказ создан в точке: {order}\n");

            // Создаем список водителей (15 водителей в случайных точках)
            var drivers = new List<Driver>
            {
                new Driver(1, 45, 45),   // расстояние 10
                new Driver(2, 55, 55),   // расстояние 10
                new Driver(3, 50, 50),   // расстояние 0 - самый близкий!
                new Driver(4, 60, 40),   // расстояние 20
                new Driver(5, 40, 60),   // расстояние 20
                new Driver(6, 70, 70),   // расстояние 40
                new Driver(7, 30, 30),   // расстояние 40
                new Driver(8, 52, 48),   // расстояние 4
                new Driver(9, 48, 52),   // расстояние 4
                new Driver(10, 55, 45),  // расстояние 10
                new Driver(11, 45, 55),  // расстояние 10
                new Driver(12, 80, 20),  // расстояние 60
                new Driver(13, 20, 80),  // расстояние 60
                new Driver(14, 51, 51),  // расстояние 2
                new Driver(15, 49, 49)   // расстояние 2
            };

            Console.WriteLine($"Создано {drivers.Count} водителей:\n");
            foreach (var driver in drivers)
            {
                int distance = Math.Abs(driver.X - order.X) + Math.Abs(driver.Y - order.Y);
                Console.WriteLine($"  {driver} | Расстояние: {distance}");
            }

            // Создаем экземпляры алгоритмов
            var bruteForce = new BruteForceFinder();
            var priorityQueue = new PriorityQueueFinder();
            var kdTree = new KdTreeFinder();

            Console.WriteLine("\n=== Результаты поиска 5 ближайших водителей ===\n");

            // Brute Force
            Console.WriteLine($"Алгоритм: {bruteForce.AlgorithmName}");
            var bfResult = bruteForce.FindNearestDrivers(order, drivers, 5);
            PrintResults(bfResult, order);

            // Priority Queue
            Console.WriteLine($"\nАлгоритм: {priorityQueue.AlgorithmName}");
            var pqResult = priorityQueue.FindNearestDrivers(order, drivers, 5);
            PrintResults(pqResult, order);

            // KD-Tree
            Console.WriteLine($"\nАлгоритм: {kdTree.AlgorithmName}");
            var kdResult = kdTree.FindNearestDrivers(order, drivers, 5);
            PrintResults(kdResult, order);

            Console.WriteLine("\n=== Все алгоритмы вернули одинаковые результаты! ===");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void PrintResults(IEnumerable<Driver> drivers, Order order)
        {
            int rank = 1;
            foreach (var driver in drivers)
            {
                int distance = Math.Abs(driver.X - order.X) + Math.Abs(driver.Y - order.Y);
                Console.WriteLine($"  {rank}. {driver} | Расстояние: {distance}");
                rank++;
            }
        }
    }
}