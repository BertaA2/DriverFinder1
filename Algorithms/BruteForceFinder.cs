using System;
using System.Collections.Generic;
using System.Linq;

namespace DriverFinder1.Algorithms
{
    /// <summary>
    /// Алгоритм поиска ближайших водителей методом полного перебора (Brute Force)
    /// Сложность: O(n log n) из-за сортировки
    /// </summary>
    public class BruteForceFinder : IAlgorithm
    {
        public string AlgorithmName => "Brute Force";

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count = 5)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (drivers == null)
                throw new ArgumentNullException(nameof(drivers));

            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            // Используем Манхэттенское расстояние (соответствует прямоугольной сетке)
            return drivers
                .Select(driver => new
                {
                    Driver = driver,
                    Distance = Math.Abs(driver.X - order.X) + Math.Abs(driver.Y - order.Y)
                })
                .OrderBy(x => x.Distance)
                .Take(count)
                .Select(x => x.Driver);
        }
    }
}