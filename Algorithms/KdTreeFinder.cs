using System;
using System.Collections.Generic;
using System.Linq;
using DriverFinder1.Algorithms;

namespace DriverFinder1.Algorithms
{
    public class KdTreeFinder : IAlgorithm
    {
        // Добавлено '?' для nullable-ссылки
        private KdTreeNode? _root = null;
        private bool _isTreeBuilt = false;

        public string AlgorithmName => "KD-Tree";

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count = 5)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (drivers == null)
                throw new ArgumentNullException(nameof(drivers));

            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            var driverList = drivers.ToList();

            if (driverList.Count == 0)
                return Enumerable.Empty<Driver>();

            if (driverList.Count <= count)
                return driverList.OrderBy(d => d.SquareEuclideanDistanceTo(order));

            // Перестраиваем дерево при первом запросе
            if (!_isTreeBuilt)
            {
                _root = BuildKdTree(driverList, 0);
                _isTreeBuilt = true;
            }

            var nearest = new List<(Driver Driver, int DistanceSquared)>(count);
            FindNearestNeighbors(_root, order, nearest, count, 0);

            return nearest
                .OrderBy(x => x.DistanceSquared)
                .Select(x => x.Driver);
        }

        private KdTreeNode? BuildKdTree(List<Driver> drivers, int depth)
        {
            if (drivers.Count == 0)
                return null;  // Теперь корректно возвращаем null

            int axis = depth % 2;
            var sorted = drivers.OrderBy(d => (axis == 0) ? d.X : d.Y).ToList();
            int medianIndex = sorted.Count / 2;
            var medianDriver = sorted[medianIndex];

            var leftDrivers = sorted.Take(medianIndex).ToList();
            var rightDrivers = sorted.Skip(medianIndex + 1).ToList();

            return new KdTreeNode(
                medianDriver,
                BuildKdTree(leftDrivers, depth + 1),
                BuildKdTree(rightDrivers, depth + 1)
            );
        }

        private void FindNearestNeighbors(
            KdTreeNode? node,  // Добавлено '?'
            Order order,
            List<(Driver Driver, int DistanceSquared)> nearest,
            int count,
            int depth)
        {
            // Добавлена проверка на null
            if (node == null)
                return;

            int distanceSquared = node.Driver.SquareEuclideanDistanceTo(order);

            if (nearest.Count < count)
            {
                nearest.Add((node.Driver, distanceSquared));
                nearest.Sort((a, b) => a.DistanceSquared.CompareTo(b.DistanceSquared));
            }
            else if (distanceSquared < nearest[^1].DistanceSquared)
            {
                nearest[^1] = (node.Driver, distanceSquared);
                nearest.Sort((a, b) => a.DistanceSquared.CompareTo(b.DistanceSquared));
            }

            int axis = depth % 2;
            bool goLeft = (axis == 0)
                ? order.X < node.Driver.X
                : order.Y < node.Driver.Y;

            FindNearestNeighbors(goLeft ? node.Left : node.Right, order, nearest, count, depth + 1);

            int distanceToPlane = (axis == 0)
                ? Math.Abs(order.X - node.Driver.X)
                : Math.Abs(order.Y - node.Driver.Y);

            if (nearest.Count > 0 && distanceToPlane * distanceToPlane < nearest[^1].DistanceSquared)
            {
                FindNearestNeighbors(goLeft ? node.Right : node.Left, order, nearest, count, depth + 1);
            }
        }

        private sealed class KdTreeNode
        {
            public Driver Driver { get; }
            public KdTreeNode? Left { get; }  // Добавлено '?'
            public KdTreeNode? Right { get; } // Добавлено '?'

            public KdTreeNode(Driver driver, KdTreeNode? left = null, KdTreeNode? right = null)
            {
                Driver = driver;
                Left = left;
                Right = right;
            }
        }
    }
}