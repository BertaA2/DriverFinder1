using System;
using System.Collections.Generic;
using System.Linq;
using DriverFinder1.Algorithms;

namespace DriverFinder1.Algorithms
{
    public class KdTreeFinder : IAlgorithm
    {
        
        
        public string AlgorithmName => "KD-Tree Algorithm";

        // Класс для узла KD-дерева
        private class KdNode
        {
            public Driver Driver { get; }
            public KdNode Left { get; set; }
            public KdNode Right { get; set; }
            public int Depth { get; }

            public KdNode(Driver driver, int depth)
            {
                Driver = driver;
                Depth = depth;
            }

            public int GetAxisValue()
            {
                return Depth % 2 == 0 ? Driver.X : Driver.Y;
            }
        }

        private KdNode _root;
        private bool _isTreeBuilt = false;

        public IEnumerable<Driver> FindNearestDrivers(Order order, IEnumerable<Driver> drivers, int count)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (drivers == null)
                throw new ArgumentNullException(nameof(drivers));
            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0", nameof(count));

            var driversList = drivers.ToList();

            // Если дерево еще не построено или данные изменились, строим дерево
            if (!_isTreeBuilt)
            {
                _root = BuildTree(driversList, 0);
                _isTreeBuilt = true;
            }

            if (_root == null)
                return Enumerable.Empty<Driver>();

            // Используем список для хранения ближайших водителей
            var nearest = new List<(Driver Driver, int DistanceSquared)>();

            // Ищем ближайших водителей
            SearchNearest(_root, order, count, nearest, 0);

            // Возвращаем отсортированный результат
            return nearest
                .OrderBy(x => x.DistanceSquared)
                .Select(x => x.Driver)
                .Take(count);
        }

        private KdNode BuildTree(List<Driver> drivers, int depth)
        {
            if (drivers == null || drivers.Count == 0)
                return null;

            // Определяем ось для разделения (0 - X, 1 - Y)
            int axis = depth % 2;

            // Сортируем по выбранной оси
            drivers.Sort((a, b) =>
                axis == 0 ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

            // Берем медиану
            int medianIndex = drivers.Count / 2;
            var node = new KdNode(drivers[medianIndex], depth);

            // Рекурсивно строим левое поддерево
            if (medianIndex > 0)
            {
                node.Left = BuildTree(
                    drivers.GetRange(0, medianIndex),
                    depth + 1);
            }

            // Рекурсивно строим правое поддерево
            if (medianIndex + 1 < drivers.Count)
            {
                node.Right = BuildTree(
                    drivers.GetRange(medianIndex + 1, drivers.Count - medianIndex - 1),
                    depth + 1);
            }

            return node;
        }

        private void SearchNearest(KdNode node, Order order, int k,
                                 List<(Driver Driver, int DistanceSquared)> nearest, int depth)
        {
            if (node == null)
                return;

            // Вычисляем квадрат расстояния
            int distanceSquared = CalculateDistanceSquared(
                node.Driver.X, node.Driver.Y, order.X, order.Y);

            // Добавляем водителя в список
            nearest.Add((node.Driver, distanceSquared));

            // Сортируем по расстоянию
            nearest.Sort((a, b) => a.DistanceSquared.CompareTo(b.DistanceSquared));

            // Если набрали больше k, удаляем самый дальний
            if (nearest.Count > k)
            {
                nearest.RemoveAt(nearest.Count - 1);
            }

            // Определяем, в каком поддереве сначала искать
            int axis = depth % 2;
            int targetValue = axis == 0 ? order.X : order.Y;
            int nodeValue = axis == 0 ? node.Driver.X : node.Driver.Y;

            KdNode firstChild = targetValue < nodeValue ? node.Left : node.Right;
            KdNode secondChild = targetValue < nodeValue ? node.Right : node.Left;

            // Ищем в первом поддереве
            SearchNearest(firstChild, order, k, nearest, depth + 1);

            // Проверяем, нужно ли искать во втором поддереве
            if (secondChild != null && nearest.Count > 0)
            {
                // Вычисляем расстояние до разделяющей плоскости
                int planeDistance = (targetValue - nodeValue) * (targetValue - nodeValue);

                // Получаем максимальное расстояние из найденных ближайших
                int maxDistanceInNearest = nearest.Last().DistanceSquared;

                // Если есть шанс найти более близкие точки в другом поддереве
                if (nearest.Count < k || planeDistance < maxDistanceInNearest)
                {
                    SearchNearest(secondChild, order, k, nearest, depth + 1);
                }
            }
        }

        private int CalculateDistanceSquared(int x1, int y1, int x2, int y2)
        {
            int dx = x1 - x2;
            int dy = y1 - y2;
            return dx * dx + dy * dy;
        }

        // Метод для перестройки дерева с новыми данными
        public void RebuildTree(IEnumerable<Driver> drivers)
        {
            var driversList = drivers.ToList();
            _root = BuildTree(driversList, 0);
            _isTreeBuilt = true;
        }

        // Метод для очистки дерева
        public void ClearTree()
        {
            _root = null;
            _isTreeBuilt = false;
        }
    }

    public class Driver
    {
        public string Id { get; }
        public int X { get; }
        public int Y { get; }

        public Driver(string id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public int SquareDistanceTo(Order order)
        {
            int dx = X - order.X;
            int dy = Y - order.Y;
            return dx * dx + dy * dy;
        }
    }

    public class Order
    {
        public int X { get; }
        public int Y { get; }

        public Order(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
