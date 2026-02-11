using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverFinder1
{
    public class Order
    {
        /// <summary>
        /// Координата X на карте (0 <= X < N)
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y на карте (0 <= Y < M)
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Инициализирует новый заказ
        /// </summary>
        public Order(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Возвращает строковое представление заказа
        /// </summary>
        public override string ToString()
        {
            return $"Order at ({X}, {Y})";
        }
    }
}
