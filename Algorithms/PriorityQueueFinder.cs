using System;
using System.Collections.Generic;
using System.Linq;

namespace DriverFinder1.Algorithms
{
    /// <summary>
    /// Алгоритм поиска ближайших водителей с использованием приоритетной очереди (макс-куча)
    /// Сложность: O(n log k), где k = количество запрашиваемых водителей (обычно 5)
    /// Использует Манхэттенское расстояние для прямоугольной сетки
    /// </summary>
    public class PriorityQueueFinder : IAlgorithm
    {
        public string AlgorithmName => "Priority Queue";

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count = 5)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (drivers == null)
                throw new ArgumentNullException(nameof(drivers));

            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            var driverList = drivers.ToList();

            // Обработка крайних случаев
            if (driverList.Count == 0)
                return Enumerable.Empty<Driver>();

            if (driverList.Count <= count)
            {
                // Сортируем напрямую с использованием вычисления расстояния
                return driverList.OrderBy(d =>
                    Math.Abs(d.X - order.X) + Math.Abs(d.Y - order.Y));
            }

            // Используем приоритетную очередь как макс-кучу
            // Для этого инвертируем приоритет (используем отрицательные расстояния)
            var maxHeap = new PriorityQueue<Driver, int>();
            int maxSize = count;

            foreach (var driver in driverList)
            {
                // Прямое вычисление Манхэттенского расстояния
                int distance = Math.Abs(driver.X - order.X) + Math.Abs(driver.Y - order.Y);

                if (maxHeap.Count < maxSize)
                {
                    // Добавляем с отрицательным приоритетом для макс-кучи
                    maxHeap.Enqueue(driver, -distance);
                }
                else
                {
                    // Правильный способ проверить приоритет
                    if (maxHeap.TryPeek(out _, out int currentMaxPriority) &&
                        distance < -currentMaxPriority)
                    {
                        maxHeap.Dequeue();
                        maxHeap.Enqueue(driver, -distance);
                    }
                }
            }

            // Извлекаем и сортируем по возрастанию расстояния
            var result = new List<Driver>(maxSize);
            while (maxHeap.Count > 0)
            {
                result.Add(maxHeap.Dequeue());
            }

            // Сортируем по возрастанию расстояния
            result.Sort((a, b) =>
                (Math.Abs(a.X - order.X) + Math.Abs(a.Y - order.Y))
                .CompareTo(Math.Abs(b.X - order.X) + Math.Abs(b.Y - order.Y)));

            return result;
        }
    }
}