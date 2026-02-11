using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverFinder1.Algorithms
{
    public class BruteForceFinder : IAlgorithm
    {
        public string AlgorithmName => "Brute Force";

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be greater than 0", nameof(count));

            // Используем квадрат расстояния для избежания вычисления квадратного корня
            return drivers
                .Select(driver => new
                {
                    Driver = driver,
                    DistanceSquared = driver.SquareDistanceTo(order)
                })
                .OrderBy(x => x.DistanceSquared)
                .Take(count)
                .Select(x => x.Driver);
        }
    }
}
