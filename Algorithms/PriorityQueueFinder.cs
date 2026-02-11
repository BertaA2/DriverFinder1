using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverFinder1.Algorithms
{
    public class PriorityQueueFinder : IAlgorithm
    {
        public string AlgorithmName => "Priority Queue";

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            if (count <= 0) throw new ArgumentException("Count must be greater than 0", nameof(count));

            // Используем PriorityQueue (библиотечная реализация из .NET 6+)
            var pq = new PriorityQueue<Driver, int>();

            foreach (var driver in drivers)
            {
                int distanceSquared = driver.SquareDistanceTo(order);
                pq.Enqueue(driver, distanceSquared);
            }

            var result = new List<Driver>();
            int resultCount = Math.Min(count, pq.Count);
            for (int i = 0; i < resultCount; i++)
            {
                result.Add(pq.Dequeue());
            }

            return result;
        }
    }
}
