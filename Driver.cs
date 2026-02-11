using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverFinder1.Algorithms;

namespace DriverFinder1
{
    public class Driver
    {
        /// <summary>
        /// Уникальный идентификатор водителя
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Координата X на карте (0 <= X < N)
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y на карте (0 <= Y < M)
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Инициализирует нового водителя
        /// </summary>
        public Driver(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Возвращает строковое представление водителя
        /// </summary>
        public override string ToString()
        {
            return $"Driver {Id} ({X}, {Y})";
        }

        /// <summary>
        /// Вычисляет квадрат расстояния до другой точки
        /// </summary>
        public int SquareDistanceTo(int x, int y)
        {
            int dx = X - x;
            int dy = Y - y;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Вычисляет квадрат расстояния до заказа
        /// </summary>
        public int SquareDistanceTo(Order order)
        {
            return SquareDistanceTo(order.X, order.Y);
        }
    }
}
