using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverFinder1.Algorithms;

namespace DriverFinder1
{
    /// <summary>
    /// Представляет водителя на карте
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Уникальный идентификатор водителя
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Координата X (0 <= X < N)
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y (0 <= Y < M)
        /// </summary>
        public int Y { get; }

        public Driver(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Возвращает строковое представление водителя для отладки
        /// </summary>
        public override string ToString()
        {
            return $"Driver {Id} ({X}, {Y})";
        }

        /// <summary>
        /// Вычисляет Манхэттенское расстояние до заказа (рекомендуется для сетки)
        /// </summary>
        public int DistanceTo(Order order)
        {
            return Math.Abs(X - order.X) + Math.Abs(Y - order.Y);
        }
    }
}
